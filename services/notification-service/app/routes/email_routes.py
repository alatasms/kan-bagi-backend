# Email API endpoints
from flask_restx import Namespace, Resource, fields
from app.services.email_service import (
    send_email_message,
    get_email_message_latest,
    get_email_message_all,
)
from datetime import datetime

# Helper function to serialize MongoDB documents
def serialize_mongo_document(document):
    """Convert MongoDB document to JSON-serializable format."""
    if not document:
        return None
    doc = document.to_mongo().to_dict()
    doc["_id"] = str(doc["_id"])  # Convert ObjectId to string
    for key, value in doc.items():
        if isinstance(value, datetime):
            doc[key] = value.isoformat()  # Convert datetime to ISO 8601 string
    return doc

email_ns = Namespace("api/email", description="Email-related operations")

# Request models

message_request = email_ns.model("EmailMessageRequest", {
    "to_email": fields.String(required=True, description="Recipient email address", example="user@example.com"),
    "subject": fields.String(required=True, description="Email subject", example="Important Update"),
    "message": fields.String(required=True, description="Email message body", example="This is an important update."),
})

# Standard response model
standard_response = email_ns.model("StandardResponse", {
    "response": fields.Raw(description="Response data"),
    "isSuccess": fields.Boolean(description="Success status"),
    "resultCode": fields.String(description="Result code"),
    "resultMessage": fields.String(description="Result message")
})

# Error handler for empty request body
@email_ns.errorhandler(KeyError)
def handle_key_error(error):
    return {
        "response": None,
        "isSuccess": False,
        "resultCode": "400",
        "resultMessage": f"Missing required field: {str(error)}"
    }, 400

# Endpoints

@email_ns.route("/create")
class SendEmailMessage(Resource):
    @email_ns.expect(message_request)
    @email_ns.response(200, "Success", standard_response)
    @email_ns.response(400, "Invalid request", standard_response)
    @email_ns.response(500, "Server error", standard_response)
    def post(self):
        """Send a message via email"""
        try:
            data = email_ns.payload
            if not data or 'to_email' not in data or 'subject' not in data or 'message' not in data:
                return {
                    "response": None,
                    "isSuccess": False,
                    "resultCode": "400",
                    "resultMessage": "Missing required fields"
                }, 400
            
            success = send_email_message(data["to_email"], data["subject"], data["message"])
            if success:
                return {
                    "response": {"sent": True},
                    "isSuccess": True,
                    "resultCode": "200",
                    "resultMessage": "Message sent successfully"
                }, 200
            return {
                "response": {"sent": False},
                "isSuccess": False,
                "resultCode": "500",
                "resultMessage": "Failed to send message"
            }, 500
        except Exception as e:
            return {
                "response": None,
                "isSuccess": False,
                "resultCode": "500",
                "resultMessage": f"Server error: {str(e)}"
            }, 500

@email_ns.route("/get/<string:to_email>")
class GetLatestEmailMessage(Resource):
    @email_ns.response(200, "Success", standard_response)
    @email_ns.response(404, "Not found", standard_response)
    @email_ns.response(500, "Server error", standard_response)
    def get(self, to_email):
        """Get the latest message sent to an email"""
        try:
            message = get_email_message_latest(to_email)
            if message:
                return {
                    "response": serialize_mongo_document(message),
                    "isSuccess": True,
                    "resultCode": "200",
                    "resultMessage": "Latest message retrieved successfully"
                }, 200
            return {
                "response": None,
                "isSuccess": False,
                "resultCode": "404",
                "resultMessage": "No message found"
            }, 404
        except Exception as e:
            return {
                "response": None,
                "isSuccess": False,
                "resultCode": "500",
                "resultMessage": f"Server error: {str(e)}"
            }, 500

@email_ns.route("/getall/<string:to_email>")
class GetAllEmailMessages(Resource):
    @email_ns.response(200, "Success", standard_response)
    @email_ns.response(500, "Server error", standard_response)
    def get(self, to_email):
        """Get all messages sent to an email"""
        try:
            messages = get_email_message_all(to_email)
            return {
                "response": [serialize_mongo_document(message) for message in messages],
                "isSuccess": True,
                "resultCode": "200",
                "resultMessage": "All messages retrieved successfully"
            }, 200
        except Exception as e:
            return {
                "response": None,
                "isSuccess": False,
                "resultCode": "500",
                "resultMessage": f"Server error: {str(e)}"
            }, 500