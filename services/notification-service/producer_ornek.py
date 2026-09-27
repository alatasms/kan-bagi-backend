import pika
import json
import os
from dotenv import load_dotenv
from app.config import Config
from datetime import datetime, timezone

# Load environment variables
load_dotenv()

def send_registration_message(message_data):
    """
    Send a registration message to RabbitMQ queue.
    
    Args:
        message_data (dict): Message data containing all necessary fields
    """
    try:
        # Create connection parameters from the URI
        parameters = pika.URLParameters(Config.RABBITMQ_URI_ADDRESS)
        
        # Establish connection
        connection = pika.BlockingConnection(parameters)
        channel = connection.channel()
        
        # Declare queue (same as in consumer)
        queue_name = "user-registered-queue"
        channel.queue_declare(queue=queue_name, durable=True)
        
        # Convert message to JSON and send
        channel.basic_publish(
            exchange='',
            routing_key=queue_name,
            body=json.dumps(message_data),
            properties=pika.BasicProperties(
                delivery_mode=2,  # make message persistent
            )
        )
        
        print(f" [x] Sent registration message: {json.dumps(message_data, indent=2)}")
        
        # Close connection
        connection.close()
        return True
        
    except Exception as e:
        print(f"Error sending message: {e}")
        return False

def send_post_created_message(message_data):
    """
    Send a new post created message to RabbitMQ queue.
    
    Args:
        message_data (dict): Message data containing all necessary fields
    """
    try:
        # Create connection parameters from the URI
        parameters = pika.URLParameters(Config.RABBITMQ_URI_ADDRESS)
        
        # Establish connection
        connection = pika.BlockingConnection(parameters)
        channel = connection.channel()
        
        # Declare queue (same as in consumer)
        queue_name = "new-post-created-queue"
        channel.queue_declare(queue=queue_name, durable=True)
        
        # Convert message to JSON and send
        channel.basic_publish(
            exchange='',
            routing_key=queue_name,
            body=json.dumps(message_data),
            properties=pika.BasicProperties(
                delivery_mode=2,  # make message persistent
            )
        )
        
        print(f" [x] Sent post created message: {json.dumps(message_data, indent=2)}")
        
        # Close connection
        connection.close()
        return True
        
    except Exception as e:
        print(f"Error sending message: {e}")
        return False

def send_post_expired_message(message):
    """
    Send a post expired message to RabbitMQ.
    """
    try:
        # Connect to RabbitMQ
        parameters = pika.URLParameters(Config.RABBITMQ_URI_ADDRESS)
        connection = pika.BlockingConnection(parameters)
        channel = connection.channel()

        # Declare queue
        channel.queue_declare(queue='user-post-expired-queue', durable=True)

        # Send message
        channel.basic_publish(
            exchange='',
            routing_key='user-post-expired-queue',
            body=json.dumps(message),
            properties=pika.BasicProperties(
                delivery_mode=2,  # make message persistent
            )
        )

        # Close connection
        connection.close()
        return True
    except Exception as e:
        print(f"Error sending post expired message: {e}")
        return False

if __name__ == "__main__":
    # Test successful message
    successful_message = {
        "conversationId": "trueeeeeeedeneme-0000-0000-f497-08dd88dad45e",
        "correlationId": "637ca161-622d-4752-9870-f5fb7eb333db",
        "message": {
            "name": "Test User",
            "email": "test@example.com"
        },
        "messageId": "01000000-0000-0000-1b9e-08dd88dad45a",
        "messageType": [
            "urn:message:AuthNZService.Domain.Events:UserRegisteredEvent",
            "urn:message:AuthNZService.Domain.Events:IDomainEvent"
        ],
        "sentTime": datetime.now(timezone.utc).isoformat(),
        "sourceAddress": f"{Config.RABBITMQ_URI_ADDRESS}/d0402c54c6ca_AuthNZServiceAPI_bus_yryyyyyyyyyyy3xqbdqatsujy8?temporary=true"
    }
    
    # Test failed message (with invalid email)
    failed_message = {
        "conversationId": "falseeedeneme-0000-0000-f497-08dd88dad45e",
        "correlationId": "637ca161-622d-4752-9870-f5fb7eb333db",
        "message": {
            "correlationId": "637ca161-622d-4752-9870-f5fb7eb333db",
            "email": "not-an-email",  # Invalid email to trigger failure
            "name": "Test",
            "occurredOn": datetime.now(timezone.utc).isoformat(),
            "userId": "86fea724-e4fd-4e4f-8c5c-2a510f12a472"
        },
        "messageId": "01000000-0000-0000-1b9e-08dd88dad45a",
        "messageType": [
            "urn:message:AuthNZService.Domain.Events:UserRegisteredEvent",
            "urn:message:AuthNZService.Domain.Events:IDomainEvent"
        ],
        "sentTime": datetime.now(timezone.utc).isoformat(),
        "sourceAddress": f"{Config.RABBITMQ_URI_ADDRESS}/d0402c54c6ca_AuthNZServiceAPI_bus_yryyyyyyyyyyy3xqbdqatsujy8?temporary=true"
    }
    
    print("\nSending successful test message...")
    success1 = send_registration_message(successful_message)
    if success1:
        print("Successful test message sent!")
    else:
        print("Failed to send successful test message")
        
    print("\nSending message that should fail...")
    success2 = send_registration_message(failed_message)
    if success2:
        print("Failed test message sent!")
    else:
        print("Failed to send failed test message")
    
    # Test successful post created message
    successful_post_message = {
        "messageId": "dca90000-1ab4-c63d-791b-08dd8a666586",
        "requestId": None,
        "correlationId": "76765cb6-2a90-4f65-95d4-004187793f7d",
        "conversationId": "dca90000-1ab4-c63d-24fd-08dd8a666593",
        "initiatorId": None,
        "sourceAddress": f"{Config.RABBITMQ_URI_ADDRESS}/LAPTOPE6VHAOG6_PostServiceAPI_bus_51woyyy4sudd5mgkbdqaw316rn?temporary=true",
        "destinationAddress": f"{Config.RABBITMQ_URI_ADDRESS}/new-post-created",
        "responseAddress": None,
        "faultAddress": None,
        "messageType": [
            "urn:message:PostService.Domain.Events:NewPostCreatedEvent",
            "urn:message:PostService.Domain.Events:IDomainEvent"
        ],
        "message": {
            "postId": "01969727-3f48-759c-8505-2fb8345f19e1",
            "patientAge": 40,
            "ownerName": "Ayşe",
            "ownerSurname": "Yılmaz",
            "patientFullName": "Mehmet Demir",
            "title": "Kan İhtiyacı",
            "description": "Kan İhtiyacı, Kan İhtiyacı, Kan İhtiyacı, Kan İhtiyacı",
            "phoneNumbers": "+905551234567,+905551234567",
            "bloodType": "B+",
            "hospital": {
                "hospitalName": "Akdeniz Üniversitesi Hastanesi",
                "hospitalAddress": "Pınarbaşı, Akdeniz Ünv., 07070 Konyaaltı/Antalya",
                "district": "Konyaaltı",
                "city": "Antalya",
                "country": "Türkiye"
            },
            "notificationChannels": {
                "emails": ["test@example.com"],
                "phoneNumbers": [],
                "userIds": []
            },
            "correlationId": "76765cb6-2a90-4f65-95d4-004187793f7d",
            "occurredOn": "2025-05-03T17:17:35.0368552Z"
        },
        "expirationTime": None,
        "sentTime": "2025-05-03T17:17:35.1087387Z",
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
    
    # Test failed post created message (with empty email list)
    failed_post_message = {
        "messageId": "dca90000-1ab4-c63d-791b-08dd8a666587",
        "requestId": None,
        "correlationId": "76765cb6-2a90-4f65-95d4-004187793f7e",
        "conversationId": "dca90000-1ab4-c63d-24fd-08dd8a666594",
        "initiatorId": None,
        "sourceAddress": f"{Config.RABBITMQ_URI_ADDRESS}/LAPTOPE6VHAOG6_PostServiceAPI_bus_51woyyy4sudd5mgkbdqaw316rn?temporary=true",
        "destinationAddress": f"{Config.RABBITMQ_URI_ADDRESS}/new-post-created",
        "responseAddress": None,
        "faultAddress": None,
        "messageType": [
            "urn:message:PostService.Domain.Events:NewPostCreatedEvent",
            "urn:message:PostService.Domain.Events:IDomainEvent"
        ],
        "message": {
            "postId": "01969727-3f48-759c-8505-2fb8345f19e2",
            "patientAge": 35,
            "ownerName": "Ahmet",
            "ownerSurname": "Yılmaz",
            "patientFullName": "Test Hasta",
            "title": "Acil Kan İhtiyacı",
            "description": "Acil kan ihtiyacı bulunmaktadır",
            "phoneNumbers": "+905551234567",
            "bloodType": "A+",
            "hospital": {
                "hospitalName": "Özel Medical Park Hastanesi",
                "hospitalAddress": "Fener Mah., Medical Park Cad., No:1 Muratpaşa/Antalya",
                "district": "Muratpaşa",
                "city": "Antalya",
                "country": "Türkiye"
            },
            "notificationChannels": {
                "emails": [],  # Boş email listesi - bu yüzden başarısız olacak
                "phoneNumbers": [],
                "userIds": []
            },
            "correlationId": "76765cb6-2a90-4f65-95d4-004187793f7e",
            "occurredOn": "2025-05-03T17:20:35.0368552Z"
        },
        "expirationTime": None,
        "sentTime": "2025-05-03T17:20:35.1087387Z",
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
    
    print("\nSending successful post created message...")
    success3 = send_post_created_message(successful_post_message)
    if success3:
        print("Successful post created message sent!")
    else:
        print("Failed to send successful post created message")
        
    print("\nSending post created message that should fail...")
    success4 = send_post_created_message(failed_post_message)
    if success4:
        print("Failed post created message sent!")
    else:
        print("Failed to send failed post created message")