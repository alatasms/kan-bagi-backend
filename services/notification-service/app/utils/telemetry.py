# OpenTelemetry instrumentation utilities
from opentelemetry import trace
from opentelemetry.sdk.resources import Resource
from opentelemetry.sdk.trace import TracerProvider
from opentelemetry.sdk.trace.export import BatchSpanProcessor
from opentelemetry.exporter.otlp.proto.http.trace_exporter import OTLPSpanExporter
from opentelemetry.instrumentation.flask import FlaskInstrumentor
from opentelemetry.instrumentation.requests import RequestsInstrumentor
from opentelemetry.instrumentation.pymongo import PymongoInstrumentor
from opentelemetry.trace.status import Status, StatusCode

from flask import request, Flask

from app.config import Config
import logging

def setup_opentelemetry(app: Flask):
    """
    Configure OpenTelemetry for the Flask application
    
    Args:
        app: The Flask application instance
    """
    try:
        # Create a resource with service and deployment environment information
        resource = Resource.create({
            "service.name": Config.OTEL_SERVICE_NAME,
            "deployment.environment": Config.OTEL_DEPLOYMENT_ENVIRONMENT
        })
        
        # Create a tracer provider
        tracer_provider = TracerProvider(resource=resource)
        
        try:
            # OTLP over HTTP; Jaeger in deploy/docker-compose.yml receives it.
            otlp_exporter = OTLPSpanExporter(endpoint=Config.OTEL_EXPORTER_OTLP_TRACES_ENDPOINT)
            tracer_provider.add_span_processor(BatchSpanProcessor(otlp_exporter))
            logging.info(f"OTLP exporter configured with endpoint: {Config.OTEL_EXPORTER_OTLP_TRACES_ENDPOINT}")
        except Exception as e:
            logging.warning(f"Failed to configure OTLP exporter: {str(e)}. Continuing without tracing.")
        
        # Set the tracer provider
        trace.set_tracer_provider(tracer_provider)
        
        # Get a tracer
        tracer = trace.get_tracer(__name__)
        
        # Instrument Flask
        FlaskInstrumentor().instrument_app(
            app,
            tracer_provider=tracer_provider,
            excluded_urls="health,metrics",  # Exclude health and metrics endpoints from tracing
        )
        
        # Instrument requests library (for HTTP client calls)
        RequestsInstrumentor().instrument(tracer_provider=tracer_provider)
        
        # Instrument pymongo for MongoDB operations
        PymongoInstrumentor().instrument(tracer_provider=tracer_provider)
        
        # Configure request hooks to enrich spans with request information
        @app.before_request
        def before_request():
            current_span = trace.get_current_span()
            if current_span and hasattr(request, 'headers'):
                # Add route information
                current_span.set_attribute("http.route", request.path)
                
                # Add user information if available
                if 'X-User-ID' in request.headers:
                    current_span.set_attribute("user.id", request.headers.get('X-User-ID'))
                    
                if 'X-User-Email' in request.headers:
                    current_span.set_attribute("user.email", request.headers.get('X-User-Email'))
                
                # Add custom event
                current_span.add_event("RequestStarted")
        
        # Register error handler to capture exceptions in spans
        @app.errorhandler(Exception)
        def handle_exception(e):
            current_span = trace.get_current_span()
            if current_span:
                current_span.set_status(Status(StatusCode.ERROR))
                current_span.set_attribute("error.message", str(e))
                current_span.set_attribute("error.type", e.__class__.__name__)
                current_span.record_exception(e)
            
            # Re-raise the exception for normal exception handling
            raise e
        
        logging.info(f"OpenTelemetry initialized for service {Config.OTEL_SERVICE_NAME}")
        return tracer
        
    except Exception as e:
        logging.error(f"Failed to initialize OpenTelemetry: {str(e)}. Application will continue without tracing.")
        return None
