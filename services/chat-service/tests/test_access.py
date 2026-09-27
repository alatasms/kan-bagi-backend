"""Access rules of the chat API. Token validation and MongoDB are replaced with fakes."""
import pytest
from fastapi.testclient import TestClient
from starlette.websockets import WebSocketDisconnect

import app.auth as auth
import app.main as main
from app.models import Room, MessageInDB

ALICE, BOB, EVE = "alice-id", "bob-id", "eve-id"
ROOM = Room(room_id="room-1", user1_id=ALICE, user2_id=BOB, user1_fullname="Alice", user2_fullname="Bob")


@pytest.fixture(autouse=True)
def fakes(monkeypatch):
    # A token is simply the user id; "admin-id" carries the admin role.
    def decode(token):
        if not token:
            raise auth.jwt.InvalidTokenError("empty")
        return auth.CurrentUser(user_id=token, roles=frozenset({"admin"} if token == "admin-id" else set()))

    async def get_room(room_id):
        return ROOM if room_id == ROOM.room_id else None

    async def no_rooms(user_id):
        return []

    async def no_messages(room_id, skip, limit):
        return [], 0

    stored = []

    async def create_message(message):
        stored.append(message)
        return MessageInDB(**message.model_dump(), message_id="m1")

    monkeypatch.setattr(auth, "decode_token", decode)
    monkeypatch.setattr(main, "get_room", get_room)
    monkeypatch.setattr(main, "get_user_rooms_with_last_messages", no_rooms)
    monkeypatch.setattr(main, "get_room_messages", no_messages)
    monkeypatch.setattr(main, "create_message", create_message)
    return stored


client = TestClient(main.app)


def as_user(user_id):
    return {"Authorization": f"Bearer {user_id}"}


def test_request_without_token_is_rejected():
    response = client.get("/api/chat/get-rooms-by-user-id")
    assert response.status_code == 401
    assert response.json()["isSuccess"] is False


def test_user_lists_own_rooms_but_not_someone_elses():
    assert client.get("/api/chat/get-rooms-by-user-id", headers=as_user(ALICE)).status_code == 200
    assert client.get(f"/api/chat/get-rooms-by-user-id?user_id={BOB}", headers=as_user(ALICE)).status_code == 403
    assert client.get(f"/api/chat/get-rooms-by-user-id?user_id={BOB}", headers=as_user("admin-id")).status_code == 200


def test_only_participants_read_room_messages():
    url = f"/api/chat/get-messages-by-room-id?room_id={ROOM.room_id}"
    assert client.get(url, headers=as_user(ALICE)).status_code == 200
    assert client.get(url, headers=as_user(EVE)).status_code == 403
    assert client.get("/api/chat/get-messages-by-room-id?room_id=missing", headers=as_user(ALICE)).status_code == 404


def test_page_size_is_bounded():
    url = f"/api/chat/get-messages-by-room-id?room_id={ROOM.room_id}&size=1000"
    assert client.get(url, headers=as_user(ALICE)).status_code == 422


def test_cannot_create_a_room_for_other_users():
    body = {"user1_id": BOB, "user2_id": EVE, "user1_fullname": "Bob", "user2_fullname": "Eve"}
    assert client.post("/api/chat/create-room", json=body, headers=as_user(ALICE)).status_code == 403


def test_websocket_rejects_user_id_that_does_not_match_token():
    with pytest.raises(WebSocketDisconnect):
        with client.websocket_connect(f"/api/chat/ws/{ROOM.room_id}/{BOB}", headers=as_user(EVE)) as ws:
            ws.receive_text()


def test_websocket_ignores_sender_and_room_sent_by_the_client(fakes):
    with client.websocket_connect(f"/api/chat/ws/{ROOM.room_id}/{ALICE}?access_token={ALICE}") as ws:
        ws.send_text('{"content": "merhaba", "sender_user_id": "bob-id", "room_id": "other-room"}')
        assert ws.receive_text() == f"{ALICE}: merhaba"

    stored = fakes[0]
    assert (stored.sender_user_id, stored.receiver_user_id, stored.room_id) == (ALICE, BOB, ROOM.room_id)
