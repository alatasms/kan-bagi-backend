"""Keeps the subscriber list and answers "who should hear about this post?"."""
import datetime
from mongoengine.queryset.visitor import Q
from ..models.subscriber import Subscriber


def upsert_from_preferences_event(message):
    """Apply a `notification-preferences-changed` event (the MassTransit envelope's `message` part)."""
    user_id = str(message["userId"])
    Subscriber.objects(user_id=user_id).update_one(
        upsert=True,
        set__name=message.get("name", ""),
        set__email=message.get("email", ""),
        set__email_enabled=bool(message.get("emailEnabled", False)),
        set__phone_enabled=bool(message.get("phoneNumberEnabled", False)),
        set__push_enabled=bool(message.get("pushNotificationEnabled", False)),
        set__preferred_hospital_ids=[str(h) for h in message.get("preferredHospitalIds") or []],
        set__preferred_blood_types=[str(b) for b in message.get("preferredBloodTypes") or []],
        set__updated_at=datetime.datetime.now(datetime.timezone.utc),
    )


def email_recipients_for_post(post):
    """
    Subscribers to email about a new post: email enabled, the post's blood type among their preferred
    types, the post's hospital among their hospitals (or no hospital filter), and not the post owner.
    """
    blood_type_code = post.get("bloodTypeCode")
    if not blood_type_code:
        return []
    hospital_id = str(post.get("hospitalId", ""))
    return list(
        Subscriber.objects(
            Q(email_enabled=True)
            & Q(preferred_blood_types=blood_type_code)
            & Q(user_id__ne=str(post.get("ownerId", "")))
            & Q(email__ne="")
            & (Q(preferred_hospital_ids__size=0) | Q(preferred_hospital_ids=hospital_id))
        )
    )


def find(user_id):
    return Subscriber.objects(user_id=str(user_id)).first()
