from mongoengine import Document, StringField, DictField, DateTimeField
import datetime

class MessageLog(Document):
    """ Message log model for storing consumed RabbitMQ messages """
    queue_name = StringField(required=True)
    # MassTransit envelope id; with the queue name it identifies a delivery.
    message_id = StringField()
    message_data = DictField(required=True)
    processed_at = DateTimeField(default=lambda: datetime.datetime.now(datetime.timezone.utc))
    status = StringField(choices=['received', 'processed', 'failed'], default='received')
    error = StringField()

    meta = {
        'collection': 'message_log',
        'indexes': [
            'queue_name',
            'processed_at',
            'status',
            ('queue_name', 'message_id'),
        ]
    }