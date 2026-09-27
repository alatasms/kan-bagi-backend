"""Recipient selection, subscriber updates and email content. MongoDB is mocked, SMTP is patched."""
import json
from unittest.mock import patch

import mongomock
import pytest
from mongoengine import connect, disconnect

import consumer
from app.models.subscriber import Subscriber
from app.notifications import subscribers
from app.templates import emails


@pytest.fixture(autouse=True)
def mongo():
    disconnect()
    connect("notification_test", host="mongodb://localhost", mongo_client_class=mongomock.MongoClient)
    yield
    Subscriber.drop_collection()
    disconnect()


def preferences(user_id, **overrides):
    event = {
        "userId": user_id,
        "name": f"User {user_id}",
        "email": f"{user_id}@example.com",
        "emailEnabled": True,
        "phoneNumberEnabled": False,
        "pushNotificationEnabled": False,
        "preferredHospitalIds": [],
        "preferredBloodTypes": ["A_Positive", "AB_Positive"],
    }
    event.update(overrides)
    return event


def post(**overrides):
    event = {
        "postId": "p1",
        "ownerId": "owner",
        "ownerName": "Ayşe",
        "ownerSurname": "Yılmaz",
        "patientFullName": "Hasta",
        "patientAge": 40,
        "description": "Acil kan ihtiyacı",
        "phoneNumbers": "05551112233",
        "bloodType": "A+",
        "bloodTypeCode": "A_Positive",
        "hospitalId": 25,
        "hospital": {"hospitalName": "Akdeniz", "hospitalAddress": "Antalya"},
    }
    event.update(overrides)
    return event


def recipient_ids(post_event):
    return sorted(s.user_id for s in subscribers.email_recipients_for_post(post_event))


def test_preferences_event_creates_then_updates_subscriber():
    subscribers.upsert_from_preferences_event(preferences("u1"))
    subscribers.upsert_from_preferences_event(preferences("u1", email="new@example.com", preferredHospitalIds=[25]))

    assert Subscriber.objects.count() == 1
    subscriber = subscribers.find("u1")
    assert subscriber.email == "new@example.com"
    assert subscriber.preferred_hospital_ids == ["25"]


def test_recipients_match_blood_type_and_hospital():
    subscribers.upsert_from_preferences_event(preferences("any-hospital"))
    subscribers.upsert_from_preferences_event(preferences("this-hospital", preferredHospitalIds=["25"]))
    subscribers.upsert_from_preferences_event(preferences("other-hospital", preferredHospitalIds=["7"]))
    subscribers.upsert_from_preferences_event(preferences("other-blood", preferredBloodTypes=["O_Negative"]))
    subscribers.upsert_from_preferences_event(preferences("email-off", emailEnabled=False))

    assert recipient_ids(post()) == ["any-hospital", "this-hospital"]


def test_owner_is_not_notified_about_own_post():
    subscribers.upsert_from_preferences_event(preferences("owner"))
    subscribers.upsert_from_preferences_event(preferences("donor"))

    assert recipient_ids(post(ownerId="owner")) == ["donor"]


def test_post_without_blood_type_code_notifies_nobody():
    subscribers.upsert_from_preferences_event(preferences("u1"))

    assert recipient_ids(post(bloodTypeCode=None)) == []


def test_new_post_email_greets_recipient_not_owner():
    subscribers.upsert_from_preferences_event(preferences("donor", name="Mehmet"))

    with patch.object(consumer, "send_email_message", return_value=True) as send:
        assert consumer.handle_post_created({"message": post()}) is None

    to, _subject, body = send.call_args.args
    assert to == "donor@example.com"
    assert "Merhaba Mehmet" in body


def test_expired_post_email_goes_to_owner_address_from_subscribers():
    subscribers.upsert_from_preferences_event(preferences("owner", email="owner@example.com"))

    with patch.object(consumer, "send_email_message", return_value=True) as send:
        assert consumer.handle_post_expired({"message": post(ownerId="owner")}) is None

    assert send.call_args.args[0] == "owner@example.com"


def test_expired_post_with_unknown_owner_reports_error():
    with patch.object(consumer, "send_email_message") as send:
        assert consumer.handle_post_expired({"message": post(ownerId="nobody")}) is not None
    send.assert_not_called()


def test_user_supplied_fields_are_html_escaped():
    _subject, body = emails.new_post("Mehmet", post(description='<a href="https://evil.example">tıkla</a>'))

    assert '<a href="https://evil.example">' not in body
    assert "&lt;a href=" in body


class FakeChannel:
    def __init__(self):
        self.acked = []

    def basic_ack(self, delivery_tag):
        self.acked.append(delivery_tag)


class FakeMethod:
    def __init__(self, delivery_tag):
        self.delivery_tag = delivery_tag


def test_redelivered_message_is_acknowledged_but_not_emailed_twice():
    from app.models.message_log import MessageLog
    subscribers.upsert_from_preferences_event(preferences("donor"))
    body = json.dumps({"messageId": "m-1", "message": post()})
    channel = FakeChannel()

    with patch.object(consumer, "send_email_message", return_value=True) as send:
        consumer.process(consumer.QUEUE_NAME_POST_CREATED, channel, FakeMethod(1), body)
        consumer.process(consumer.QUEUE_NAME_POST_CREATED, channel, FakeMethod(2), body)

    assert send.call_count == 1
    assert channel.acked == [1, 2]
    MessageLog.drop_collection()


def test_emails_show_support_address_only_when_configured(monkeypatch):
    from app.config import Config

    monkeypatch.setattr(Config, "SUPPORT_EMAIL", "")
    subject, body = emails.welcome("Ayşe")
    assert "mailto:" not in body
    assert "Kan Bağı" in subject

    monkeypatch.setattr(Config, "SUPPORT_EMAIL", "destek@example.org")
    _subject, body = emails.post_expired(post())
    assert 'href="mailto:destek@example.org"' in body
