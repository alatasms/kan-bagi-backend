from bson import ObjectId
from bson.errors import InvalidId
from typing import Optional
from .database import mongo_db
from .models import Message, MessageInDB, Room, RoomWithLastMessage, MessageWithFullNames
from datetime import datetime, timezone

async def create_message(message: Message) -> MessageInDB:
    message_dict = message.dict()
    message_dict["timestamp"] = datetime.now(timezone.utc)
    result = await mongo_db.db.messages.insert_one(message_dict)
    message_dict["message_id"] = str(result.inserted_id)
    return MessageInDB(**message_dict)

async def get_messages() -> list[MessageInDB]:
    messages = []
    async for message in mongo_db.db.messages.find():
        message["message_id"] = str(message["_id"])
        messages.append(MessageInDB(**message))
    return messages

async def get_message(message_id: str) -> Optional[MessageInDB]:
    message = await mongo_db.db.messages.find_one({"_id": ObjectId(message_id)})
    if message:
        message["message_id"] = str(message["_id"])
        return MessageInDB(**message)
    return None

async def get_room_by_users(user1_id: str, user2_id: str) -> Optional[Room]:
    room = await mongo_db.db.rooms.find_one({
        "$or": [
            {"$and": [{"user1_id": user1_id}, {"user2_id": user2_id}]},
            {"$and": [{"user1_id": user2_id}, {"user2_id": user1_id}]}
        ]
    })
    if room:
        room["room_id"] = str(room["_id"])
        return Room(**room)
    return None

async def create_room(user1_id: str, user2_id: str, user1_fullname: str = None, user2_fullname: str = None) -> Room:
    try:
        if not user1_id or not user2_id:
            return Room(user1_id="", user2_id="", isSuccess=False)
            
        if user1_id == user2_id:
            return Room(user1_id=user1_id, user2_id=user2_id, isSuccess=False)

        # Check if room already exists
        existing_room = await get_room_by_users(user1_id, user2_id)
        if existing_room:
            existing_room.isSuccess = True  # Mevcut odayı başarılı olarak işaretliyoruz
            return existing_room

        # Create new room only if it doesn't exist
        room_dict = {
            "user1_id": user1_id,
            "user2_id": user2_id,
            "user1_fullname": user1_fullname,
            "user2_fullname": user2_fullname,
            "created_at": datetime.now(timezone.utc)
        }
        result = await mongo_db.db.rooms.insert_one(room_dict)
        room_dict["room_id"] = str(result.inserted_id)
        return Room(**room_dict, isSuccess=True)
    except Exception as e:
        print(f"Error creating room: {e}")
        return Room(user1_id=user1_id, user2_id=user2_id, isSuccess=False)

async def get_room(room_id: str) -> Optional[Room]:
    try:
        if not ObjectId.is_valid(room_id):
            return None
        room = await mongo_db.db.rooms.find_one({"_id": ObjectId(room_id)})
        if room:
            room["room_id"] = str(room["_id"])
            return Room(**room)
        return None
    except InvalidId:
        return None

async def get_user_rooms(user_id: str) -> list[Room]:
    rooms = []
    async for room in mongo_db.db.rooms.find({
        "$or": [
            {"user1_id": user_id},
            {"user2_id": user_id}
        ]
    }):
        room["room_id"] = str(room["_id"])
        rooms.append(Room(**room))
    return rooms

async def get_room_messages(room_id: str, skip: int = 0, limit: int = 100) -> tuple[list[MessageWithFullNames], int]:
    messages = []
    total = await mongo_db.db.messages.count_documents({"room_id": room_id})
    
    # Get room info to get full names
    room = await get_room(room_id)
    if not room:
        return [], 0
    
    # Create a mapping of user IDs to full names
    user_fullnames = {
        room.user1_id: room.user1_fullname,
        room.user2_id: room.user2_fullname
    }
    
    cursor = mongo_db.db.messages.find({"room_id": room_id}) \
        .sort("timestamp", 1) \
        .skip(skip) \
        .limit(limit)
    
    async for message in cursor:
        message["message_id"] = str(message["_id"])
        # Add full names to the message
        message["sender_fullname"] = user_fullnames.get(message["sender_user_id"])
        message["receiver_fullname"] = user_fullnames.get(message["receiver_user_id"])
        messages.append(MessageWithFullNames(**message))
    
    return messages, total

async def get_user_rooms_with_last_messages(user_id: str) -> list[RoomWithLastMessage]:
    rooms = []
    async for room in mongo_db.db.rooms.find({
        "$or": [
            {"user1_id": user_id},
            {"user2_id": user_id}
        ]
    }):
        room["room_id"] = str(room["_id"])
        room_obj = Room(**room)
        
        # Get last message for this room
        last_message = await mongo_db.db.messages.find_one(
            {"room_id": str(room["_id"])},
            sort=[("timestamp", -1)]
        )
        
        if last_message:
            last_message["message_id"] = str(last_message["_id"])
            last_message = MessageInDB(**last_message)
        
        rooms.append(RoomWithLastMessage(
            **room_obj.dict(),
            last_message=last_message
        ))
    return rooms