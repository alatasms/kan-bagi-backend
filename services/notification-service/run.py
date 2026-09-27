from flask import Flask, jsonify, request, Response
from flask_restx import Api, Namespace, Resource, fields
from mongoengine import connect, disconnect
from app.routes.email_routes import email_ns
import os
from pymongo import MongoClient
import logging
import threading
from consumer import consume_messages
from app.config import Config
from app.models.message_log import MessageLog
import uuid
from datetime import datetime, timezone
import pika
import json
from prometheus_flask_exporter import PrometheusMetrics
from prometheus_client import generate_latest, CONTENT_TYPE_LATEST
from app.utils.telemetry import setup_opentelemetry

# Initialize Flask app
app = Flask(__name__)

# Initialize OpenTelemetry
tracer = setup_opentelemetry(app)

# Initialize Prometheus metrics with default metrics
metrics = PrometheusMetrics(app, path='/metrics')

# Add default metrics
metrics.info('app_info', 'Application info', version='1.0.0')

# Add custom metrics
http_requests_total = metrics.counter(
    'http_requests_total', 'Total HTTP requests',
    labels={'method': lambda: request.method, 'endpoint': lambda: request.endpoint}
)

http_request_duration_seconds = metrics.histogram(
    'http_request_duration_seconds', 'HTTP request duration in seconds',
    labels={'method': lambda: request.method, 'endpoint': lambda: request.endpoint}
)

# Everything except health and metrics is a development tool: the endpoints below read email logs,
# consume queues or send mail to any address, and none of them is authenticated.
PUBLIC_PATHS = ('/health', '/metrics')


@app.before_request
def restrict_dev_endpoints():
    if Config.APP_ENV != 'development' and request.path not in PUBLIC_PATHS:
        return {"response": None, "isSuccess": False, "resultCode": "404", "resultMessage": "Not found"}, 404


@app.route('/health')
def home():
    return {
        "response": {"status": "running", "port": 5001},
        "isSuccess": True,
        "resultCode": "200",
        "resultMessage": "Notification Service is running on port 5001!"
    }

@app.route('/data', methods=['GET'])
def get_data():
    try:
        client = MongoClient(Config.MONGODB_URI_ADDRESS)
        db = client[Config.MONGODB_NOTIFICATION_DB_NAME]
        collection_email_log = db['email_log'] 
        data_email = list(collection_email_log.find())


        for item in data_email:
            item['_id'] = str(item['_id'])
            
        return {
            "response": {"email_log": data_email},
            "isSuccess": True,
            "resultCode": "200",
            "resultMessage": "Data retrieved successfully"
        }
    except Exception as e:
        return {
            "response": None,
            "isSuccess": False,
            "resultCode": "500",
            "resultMessage": f"Failed to retrieve data: {str(e)}"
        }, 500

@app.route('/messages', methods=['GET'])
def get_messages():
    """
    Get RabbitMQ message logs from MongoDB.
    This endpoint retrieves message logs that have been stored in MongoDB after being processed by RabbitMQ.
    
    Query Parameters:
        queue (str): Filter messages by queue name
        status (str): Filter messages by status (received, processed, failed)
        limit (int): Limit number of results (default: 100)
    """
    try:
        # Query parameters
        queue = request.args.get('queue')  # Specific queue filter
        status = request.args.get('status')  # Status filter
        limit = int(request.args.get('limit', 100))  # Limit results, default 100
        
        # Build query
        query = {}
        if queue:
            query['queue_name'] = queue
        if status:
            query['status'] = status
            
        # Get messages
        messages = MessageLog.objects(**query).order_by('-processed_at').limit(limit)
        
        # Serialize results
        results = []
        for msg in messages:
            results.append({
                'id': str(msg.id),
                'queue_name': msg.queue_name,
                'message_data': msg.message_data,
                'processed_at': msg.processed_at.isoformat(),
                'status': msg.status,
                'error': msg.error
            })
            
        return {
            "response": results,
            "isSuccess": True,
            "resultCode": "200",
            "resultMessage": "Messages retrieved successfully"
        }
    except Exception as e:
        return {
            "response": None,
            "isSuccess": False,
            "resultCode": "500",
            "resultMessage": f"Failed to retrieve messages: {str(e)}"
        }, 500

@app.route('/messages/realtime', methods=['GET'])
def get_realtime_messages():
    """
    Get and consume all messages directly from RabbitMQ queues.
    This endpoint connects to RabbitMQ, retrieves all messages currently in the specified queue,
    and consumes them (removes them from the queue).
    
    Query Parameters:
        queue (str): Specific queue name to check (required)
    """
    try:
        # Query parameters
        queue = request.args.get('queue')
        if not queue:
            return {
                "response": None,
                "isSuccess": False,
                "resultCode": "400",
                "resultMessage": "Queue parameter is required"
            }, 400
        
        # Connect to RabbitMQ
        parameters = pika.URLParameters(Config.RABBITMQ_URI_ADDRESS)
        connection = pika.BlockingConnection(parameters)
        channel = connection.channel()
        
        # Declare queue
        channel.queue_declare(queue=queue, durable=True)
        
        # Get queue information
        queue_info = channel.queue_declare(queue=queue, durable=True, passive=True)
        message_count = queue_info.method.message_count
        
        # Get all messages
        messages = []
        processed_count = 0
        
        while processed_count < message_count:
            method_frame, header_frame, body = channel.basic_get(queue=queue, auto_ack=False)
            if not method_frame:
                break
                
            try:
                message_data = json.loads(body)
                messages.append({
                    'delivery_tag': method_frame.delivery_tag,
                    'message_data': message_data,
                    'redelivered': method_frame.redelivered,
                    'exchange': method_frame.exchange,
                    'routing_key': method_frame.routing_key
                })
                
                # Acknowledge the message
                channel.basic_ack(method_frame.delivery_tag)
                processed_count += 1
                
            except json.JSONDecodeError:
                # If message is not valid JSON, reject it
                channel.basic_reject(method_frame.delivery_tag, requeue=False)
                processed_count += 1
                continue
        
        # Close connection
        connection.close()
        
        return {
            "response": {
                "queue": queue,
                "total_messages": message_count,
                "retrieved_messages": len(messages),
                "messages": messages
            },
            "isSuccess": True,
            "resultCode": "200",
            "resultMessage": "Real-time messages retrieved and consumed successfully"
        }
    except Exception as e:
        return {
            "response": None,
            "isSuccess": False,
            "resultCode": "500",
            "resultMessage": f"Failed to retrieve real-time messages: {str(e)}"
        }, 500

@app.route('/test/registereduser', methods=['POST'])
def test_register():
    try:
        data = request.get_json()
        if not data:
            return {
                "response": None,
                "isSuccess": False,
                "resultCode": "400",
                "resultMessage": "Request body is required"
            }, 400

        # Default values for all fields
        defaults = {
            'name': 'Test User',
            'email': 'test@example.com',
        }

        # Apply default values for missing fields
        for field, default_value in defaults.items():
            if field not in data:
                data[field] = default_value


        # Send message to RabbitMQ for event sourcing
        # Create registration message with provided or default data
        message = {
            "conversationId": str(uuid.uuid4()),
            "correlationId": str(uuid.uuid4()),
            "message": {
                "name": data['name'],
                "email": data['email'],
                "userId": str(uuid.uuid4()),
                "occurredOn": datetime.now(timezone.utc).isoformat()
            },
            "messageId": str(uuid.uuid4()),
            "messageType": [
                "urn:message:AuthNZService.Domain.Events:UserRegisteredEvent",
                "urn:message:AuthNZService.Domain.Events:IDomainEvent"
            ],
            "sentTime": datetime.now(timezone.utc).isoformat(),
            "sourceAddress": Config.RABBITMQ_URI_ADDRESS
        }

        from producer_ornek import send_registration_message
        mq_success = send_registration_message(message)
        
        if mq_success:
            return {
                "response": {"sent": True, "message": message},
                "isSuccess": True,
                "resultCode": "200",
                "resultMessage": "Test registration message sent successfully"
            }
        return {
            "response": {"sent": False, "message": message},
            "isSuccess": False,
            "resultCode": "500",
            "resultMessage": "Failed to send test registration message"
        }, 500
    except Exception as e:
        return {
            "response": None,
            "isSuccess": False,
            "resultCode": "500",
            "resultMessage": f"Error sending test registration message: {str(e)}"
        }, 500

@app.route('/metrics')
def metrics_endpoint():
    return Response(generate_latest(), mimetype=CONTENT_TYPE_LATEST)

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

# Disconnect from any existing connections
disconnect()

# Connect to MongoDB
try:
    connect(db=Config.MONGODB_NOTIFICATION_DB_NAME, host=Config.MONGODB_URI_ADDRESS)
    logger.info("Successfully connected to MongoDB")
except Exception as e:
    logger.error(f"Error connecting to MongoDB: {e}")
    raise

# Initialize Flask-RESTx API
api = Api(
    app,
    version="1.0.0",
    title="Notification Service API",
    description="API for sending and retrieving email notifications.",
)

# Test namespace for test endpoints
test_ns = Namespace("test", description="Test endpoints for notification service")
api.add_namespace(test_ns, path="/test")

# Input Models for test endpoints
registered_user_input = api.model('RegisteredUserInput', {
    'name': fields.String(required=False, description='User name', example='Test User', default='Test User'),
    'email': fields.String(required=False, description='User email', example='test@example.com', default='test@example.com'),
})

@test_ns.route('/registereduser')
class RegisteredUserTest(Resource):
    @test_ns.doc('test_registered_user',
        description='Test endpoint for sending registered user notification')
    @test_ns.expect(registered_user_input)
    def post(self):
        return test_register()

# Test input model for post creation
post_input = api.model('PostInput', {
    'ownerName': fields.String(required=False, description='Post owner name', example='Test User', default='Test User'),
    'ownerSurname': fields.String(required=False, description='Post owner surname', example='Test Surname', default='Test Surname'),
    'patientFullName': fields.String(required=False, description='Patient full name', example='Test Patient', default='Test Patient'),
    'patientAge': fields.Integer(required=False, description='Patient age', example=40, default=40),
    'title': fields.String(required=False, description='Post title', example='Blood Need', default='Blood Need'),
    'description': fields.String(required=False, description='Post description', example='Urgent blood need', default='Urgent blood need'),
    'bloodType': fields.String(required=False, description='Blood type needed', example='A+', default='A+'),
    'phoneNumbers': fields.String(required=False, description='Contact phone numbers', example='+905551234567', default='+905551234567'),
    'hospitalName': fields.String(required=False, description='Hospital name', example='Test Hospital', default='Test Hospital'),
    'hospitalAddress': fields.String(required=False, description='Hospital address', example='Test Address', default='Test Address'),
    'district': fields.String(required=False, description='District', example='Test District', default='Test District'),
    'city': fields.String(required=False, description='City', example='Test City', default='Test City'),
    'country': fields.String(required=False, description='Country', example='Turkey', default='Turkey'),
    'emails': fields.List(fields.String, required=False, description='Email addresses for notification', example=['test@example.com'], default=['test@example.com'])
})

@test_ns.route('/post')
class PostTest(Resource):
    @test_ns.doc('test_post_creation',
        description='Test endpoint for sending new post notification')
    @test_ns.expect(post_input)
    def post(self):
        try:
            data = request.get_json()
            if not data:
                return {
                    "response": None,
                    "isSuccess": False,
                    "resultCode": "400",
                    "resultMessage": "Request body is required"
                }, 400

            # Default values for message level fields
            defaults = {
                'messageId': str(uuid.uuid4()),
                'requestId': None,
                'correlationId': str(uuid.uuid4()),
                'conversationId': str(uuid.uuid4()),
                'initiatorId': None,
                'sourceAddress': Config.RABBITMQ_URI_ADDRESS,
                
                # Default values for post fields
                'ownerName': 'Test User',
                'ownerSurname': 'Test Surname',
                'patientFullName': 'Test Patient',
                'patientAge': 40,
                'title': 'Blood Need',
                'description': 'Urgent blood need',
                'bloodType': 'A+',
                'phoneNumbers': '+905551234567',
                'hospitalName': 'Test Hospital',
                'hospitalAddress': 'Test Address',
                'district': 'Test District',
                'city': 'Test City',
                'country': 'Turkey',
                'emails': ['test@example.com']
            }

            # Apply default values for missing fields
            for field, default_value in defaults.items():
                if field not in data:
                    data[field] = default_value

            # Create post message
            message = {
                "messageId": data['messageId'],
                "requestId": data['requestId'],
                "correlationId": data['correlationId'],
                "conversationId": data['conversationId'],
                "initiatorId": data['initiatorId'],
                "sourceAddress": data['sourceAddress'],
                "destinationAddress": f"{Config.RABBITMQ_URI_ADDRESS}/new-post-created",
                "responseAddress": None,
                "faultAddress": None,
                "messageType": [
                    "urn:message:PostService.Domain.Events:NewPostCreatedEvent",
                    "urn:message:PostService.Domain.Events:IDomainEvent"
                ],
                "message": {
                    "postId": str(uuid.uuid4()),
                    "patientAge": data['patientAge'],
                    "ownerName": data['ownerName'],
                    "ownerSurname": data['ownerSurname'],
                    "patientFullName": data['patientFullName'],
                    "title": data['title'],
                    "description": data['description'],
                    "phoneNumbers": data['phoneNumbers'],
                    "bloodType": data['bloodType'],
                    "hospital": {
                        "hospitalName": data['hospitalName'],
                        "hospitalAddress": data['hospitalAddress'],
                        "district": data['district'],
                        "city": data['city'],
                        "country": data['country']
                    },
                    "notificationChannels": {
                        "emails": data['emails'],
                        "phoneNumbers": [],
                        "userIds": []
                    },
                    "correlationId": data['correlationId'],
                    "occurredOn": datetime.now(timezone.utc).isoformat()
                },
                "expirationTime": None,
                "sentTime": datetime.now(timezone.utc).isoformat(),
                "headers": {},
                "host": {
                    "machineName": "example-host",
                    "processName": "PostService.API",
                    "processId": 43484,
                    "assembly": "PostService.API",
                    "assemblyVersion": "1.0.0.0",
                    "frameworkVersion": "9.0.3",
                    "massTransitVersion": "8.4.0.0",
                    "operatingSystemVersion": "Microsoft Windows NT 10.0.26100.0"
                }
            }

            from producer_ornek import send_post_created_message
            success = send_post_created_message(message)
            
            if success:
                return {
                    "response": {"sent": True, "message": message},
                    "isSuccess": True,
                    "resultCode": "200",
                    "resultMessage": "Test post created message sent successfully"
                }
            return {
                "response": {"sent": False, "message": message},
                "isSuccess": False,
                "resultCode": "500",
                "resultMessage": "Failed to send test post created message"
            }, 500
        except Exception as e:
            return {
                "response": None,
                "isSuccess": False,
                "resultCode": "500",
                "resultMessage": f"Error sending test post created message: {str(e)}"
            }, 500

# Test input model for post expiration
post_expired_input = api.model('PostExpiredInput', {
    'ownerName': fields.String(required=False, description='Post owner name', example='Test User', default='Test User'),
    'ownerSurname': fields.String(required=False, description='Post owner surname', example='Test Surname', default='Test Surname'),
    'ownerEmail': fields.String(required=False, description='Post owner email', example='test@example.com', default='test@example.com'),
    'patientFullName': fields.String(required=False, description='Patient full name', example='Test Patient', default='Test Patient'),
    'patientAge': fields.Integer(required=False, description='Patient age', example=40, default=40),
    'title': fields.String(required=False, description='Post title', example='Blood Need', default='Blood Need'),
    'description': fields.String(required=False, description='Post description', example='Urgent blood need', default='Urgent blood need'),
    'bloodType': fields.String(required=False, description='Blood type needed', example='A+', default='A+'),
    'phoneNumbers': fields.String(required=False, description='Contact phone numbers', example='+905551234567', default='+905551234567'),
    'hospitalName': fields.String(required=False, description='Hospital name', example='Test Hospital', default='Test Hospital'),
    'hospitalAddress': fields.String(required=False, description='Hospital address', example='Test Address', default='Test Address'),
    'district': fields.String(required=False, description='District', example='Test District', default='Test District'),
    'city': fields.String(required=False, description='City', example='Test City', default='Test City'),
    'country': fields.String(required=False, description='Country', example='Turkey', default='Turkey')
})

@test_ns.route('/post/expired')
class PostExpiredTest(Resource):
    @test_ns.doc('test_post_expired',
        description='Test endpoint for sending post expiration notification')
    @test_ns.expect(post_expired_input)
    def post(self):
        try:
            data = request.get_json()
            if not data:
                return {
                    "response": None,
                    "isSuccess": False,
                    "resultCode": "400",
                    "resultMessage": "Request body is required"
                }, 400

            # Default values for message level fields
            defaults = {
                'messageId': str(uuid.uuid4()),
                'requestId': None,
                'correlationId': str(uuid.uuid4()),
                'conversationId': str(uuid.uuid4()),
                'initiatorId': None,
                'sourceAddress': Config.RABBITMQ_URI_ADDRESS,
                
                # Default values for post fields
                'ownerName': 'Test User',
                'ownerSurname': 'Test Surname',
                'ownerEmail': 'test@example.com',
                'patientFullName': 'Test Patient',
                'patientAge': 40,
                'title': 'Blood Need',
                'description': 'Urgent blood need',
                'bloodType': 'A+',
                'phoneNumbers': '+905551234567',
                'hospitalName': 'Test Hospital',
                'hospitalAddress': 'Test Address',
                'district': 'Test District',
                'city': 'Test City',
                'country': 'Turkey'
            }

            # Apply default values for missing fields
            for field, default_value in defaults.items():
                if field not in data:
                    data[field] = default_value

            # Create post expired message
            message = {
                "messageId": data['messageId'],
                "requestId": data['requestId'],
                "correlationId": data['correlationId'],
                "conversationId": data['conversationId'],
                "initiatorId": data['initiatorId'],
                "sourceAddress": data['sourceAddress'],
                "destinationAddress": f"{Config.RABBITMQ_URI_ADDRESS}/user-post-expired-queue",
                "responseAddress": None,
                "faultAddress": None,
                "messageType": [
                    "urn:message:PostService.Domain.Events:PostExpiredEvent",
                    "urn:message:PostService.Domain.Events:IDomainEvent"
                ],
                "message": {
                    "postId": str(uuid.uuid4()),
                    "patientAge": data['patientAge'],
                    "ownerName": data['ownerName'],
                    "ownerSurname": data['ownerSurname'],
                    "ownerEmail": data['ownerEmail'],
                    "patientFullName": data['patientFullName'],
                    "title": data['title'],
                    "description": data['description'],
                    "phoneNumbers": data['phoneNumbers'],
                    "bloodType": data['bloodType'],
                    "hospital": {
                        "hospitalName": data['hospitalName'],
                        "hospitalAddress": data['hospitalAddress'],
                        "district": data['district'],
                        "city": data['city'],
                        "country": data['country']
                    },
                    "correlationId": data['correlationId'],
                    "occurredOn": datetime.now(timezone.utc).isoformat()
                },
                "expirationTime": None,
                "sentTime": datetime.now(timezone.utc).isoformat(),
                "headers": {},
                "host": {
                    "machineName": "example-host",
                    "processName": "PostService.API",
                    "processId": 43484,
                    "assembly": "PostService.API",
                    "assemblyVersion": "1.0.0.0",
                    "frameworkVersion": "9.0.3",
                    "massTransitVersion": "8.4.0.0",
                    "operatingSystemVersion": "Microsoft Windows NT 10.0.26100.0"
                }
            }

            from producer_ornek import send_post_expired_message
            success = send_post_expired_message(message)
            
            if success:
                return {
                    "response": {"sent": True, "message": message},
                    "isSuccess": True,
                    "resultCode": "200",
                    "resultMessage": "Test post expired message sent successfully"
                }
            return {
                "response": {"sent": False, "message": message},
                "isSuccess": False,
                "resultCode": "500",
                "resultMessage": "Failed to send test post expired message"
            }, 500
        except Exception as e:
            return {
                "response": None,
                "isSuccess": False,
                "resultCode": "500",
                "resultMessage": f"Error sending test post expired message: {str(e)}"
            }, 500

# Add new namespace for message logs
message_ns = Namespace("messages", description="RabbitMQ message log operations")
api.add_namespace(message_ns, path="/messages")

# Message log response model
message_log_model = api.model('MessageLog', {
    'id': fields.String(description='Message log ID'),
    'queue_name': fields.String(description='Queue name'),
    'message_data': fields.Raw(description='Message content'),
    'processed_at': fields.DateTime(description='Processing timestamp'),
    'status': fields.String(description='Processing status'),
    'error': fields.String(description='Error message if any')
})

# Real-time message response model
realtime_message_model = api.model('RealtimeMessage', {
    'delivery_tag': fields.Integer(description='Message delivery tag'),
    'message_data': fields.Raw(description='Message content'),
    'redelivered': fields.Boolean(description='Whether message was redelivered'),
    'exchange': fields.String(description='Exchange name'),
    'routing_key': fields.String(description='Routing key')
})

realtime_response_model = api.model('RealtimeResponse', {
    'queue': fields.String(description='Queue name'),
    'total_messages': fields.Integer(description='Total number of messages in queue'),
    'retrieved_messages': fields.Integer(description='Number of messages retrieved'),
    'messages': fields.List(fields.Nested(realtime_message_model), description='List of messages')
})

@message_ns.route("")
class MessageLogs(Resource):
    @message_ns.doc('get_messages',
        params={
            'queue': 'Filter by queue name',
            'status': 'Filter by status',
            'limit': 'Limit number of results (default: 100)'
        }
    )
    @message_ns.response(200, 'Success', message_log_model)
    def get(self):
        """Get RabbitMQ message logs from MongoDB"""
        return get_messages()

@message_ns.route("/realtime")
class RealtimeMessages(Resource):
    @message_ns.doc('get_realtime_messages',
        params={
            'queue': {'description': 'Queue name to check. This endpoint will consume (remove) all messages from the specified queue.', 'required': True, 'type': 'string'}
        }
    )
    @message_ns.response(200, 'Success', realtime_response_model)
    @message_ns.response(400, 'Bad Request - Queue parameter is required')
    @message_ns.response(500, 'Internal Server Error')
    def get(self):
        """Get and consume all messages directly from RabbitMQ queues"""
        return get_realtime_messages()

# Add existing namespaces
api.add_namespace(email_ns)  # Remove path parameter since it's included in the namespace name

if __name__ == "__main__":
    # Start RabbitMQ consumer in a separate thread
    consumer_thread = threading.Thread(target=consume_messages, daemon=True)
    consumer_thread.start()

    # Start Flask app
    port = int(os.environ.get("FLASK_RUN_PORT", 5001))
    # The Werkzeug debugger allows running code from the browser; never enable it outside development.
    app.run(host='0.0.0.0', port=port, debug=Config.APP_ENV == 'development' and os.environ.get("FLASK_DEBUG") == "1")