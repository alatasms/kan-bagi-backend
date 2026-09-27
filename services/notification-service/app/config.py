# Configuration settings for the application
# Database connection settings
# API keys and secrets
# Environment variables 
import os
from dotenv import load_dotenv

load_dotenv()

class Config:    
    # Environment
    APP_ENV = os.getenv('APP_ENV', 'development')

    # Email settings
    EMAIL_SERVER_ADDRESS = os.getenv('EMAIL_SERVER_ADDRESS')
    EMAIL_PORT_ADDRESS = os.getenv('EMAIL_PORT_ADDRESS')
    EMAIL_SENDER_USERNAME = os.getenv('EMAIL_SENDER_USERNAME')
    EMAIL_SENDER_PASSWORD = os.getenv('EMAIL_SENDER_PASSWORD')
    # From address; defaults to the SMTP username.
    EMAIL_FROM = os.getenv('EMAIL_FROM') or os.getenv('EMAIL_SENDER_USERNAME')
    # STARTTLS for real providers (e.g. Gmail); a local test server such as Mailpit needs 'false'.
    EMAIL_USE_TLS = os.getenv('EMAIL_USE_TLS', 'true').lower() == 'true'
    # Address shown in emails for questions; left out of emails when empty.
    SUPPORT_EMAIL = os.getenv('SUPPORT_EMAIL', '')

    # MongoDB settings
    MONGODB_URI_ADDRESS = os.getenv('MONGODB_URI_ADDRESS', 'mongodb://mongo:27017')
    
    # OpenTelemetry settings
    OTEL_SERVICE_NAME = os.getenv('OTEL_SERVICE_NAME', 'NotificationService')
    OTEL_EXPORTER_OTLP_TRACES_ENDPOINT = os.getenv('OTEL_EXPORTER_OTLP_TRACES_ENDPOINT', 'http://jaeger:4318/v1/traces')
    OTEL_DEPLOYMENT_ENVIRONMENT = os.getenv('OTEL_DEPLOYMENT_ENVIRONMENT', APP_ENV)
    MONGODB_REMOTE_USER_NAME = os.getenv('MONGODB_REMOTE_USER_NAME')
    MONGODB_REMOTE_PASSWORD = os.getenv('MONGODB_REMOTE_PASSWORD')
    MONGODB_NOTIFICATION_DB_NAME = os.getenv('MONGODB_NOTIFICATION_DB_NAME', 'notification_service_db')
    MONGODB_SETTINGS = {
        'tz_aware': True,
        'connect': True
    }

    # RabbitMQ settings. RABBITMQ_URI wins over RABBITMQ_DEV_URI / RABBITMQ_PROD_URI.
    RABBITMQ_URI_ADDRESS = os.getenv('RABBITMQ_URI') or (
        os.getenv('RABBITMQ_PROD_URI') if os.getenv('APP_ENV') == 'production'
        else os.getenv('RABBITMQ_DEV_URI', 'amqp://guest:guest@rabbitmq:5672/'))
