from mongoengine import Document, StringField, BooleanField, ListField, DateTimeField
import datetime


class Subscriber(Document):
    """
    The notification service's own copy of who wants which notifications, kept up to date from
    user-service's `notification-preferences-changed` events. Nothing else reads user data directly.
    """
    user_id = StringField(required=True, unique=True)
    name = StringField(default="")
    email = StringField(default="")
    email_enabled = BooleanField(default=False)
    phone_enabled = BooleanField(default=False)
    push_enabled = BooleanField(default=False)
    # Hospital ids as strings. Empty means every hospital.
    preferred_hospital_ids = ListField(StringField(), default=list)
    # Blood type codes as published by user-service, e.g. "A_Positive".
    preferred_blood_types = ListField(StringField(), default=list)
    updated_at = DateTimeField(default=lambda: datetime.datetime.now(datetime.timezone.utc))

    meta = {
        'collection': 'subscriber',
        'indexes': ['user_id', 'preferred_blood_types'],
    }
