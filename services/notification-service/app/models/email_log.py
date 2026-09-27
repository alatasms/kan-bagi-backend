# Email log model definition
# MongoDB schema for email notifications 
from mongoengine import Document, EmailField, StringField, DateTimeField
import datetime

class EmailLog(Document):
    """ Email log model definition for MongoDB """
    to_email = EmailField(required=True)
    type = StringField(required=True, choices=['otp', 'message'])
    subject = StringField(required=True)
    message = StringField(required=True)
    timestamp = DateTimeField(required=True, default=lambda: datetime.datetime.now(datetime.timezone.utc))