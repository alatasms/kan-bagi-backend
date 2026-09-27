from pydantic import BaseModel
from datetime import datetime,timezone
from typing import Optional, List, TypeVar, Generic, Any

T = TypeVar('T')

class StandardResponse(BaseModel, Generic[T]):
    response: Optional[T] = None
    isSuccess: bool
    resultCode: str
    resultMessage: str

class Room(BaseModel):
    room_id: Optional[str] = None
    user1_id: str
    user2_id: str
    user1_fullname: Optional[str] = None
    user2_fullname: Optional[str] = None
    created_at: Optional[datetime] = None
    isSuccess: bool = True

class Message(BaseModel):
    sender_user_id: str
    receiver_user_id: str
    content: str
    room_id: str
    timestamp: Optional[datetime] = None

class MessageInDB(Message):
    message_id: Optional[str] = None

class MessageWithFullNames(MessageInDB):
    sender_fullname: Optional[str] = None
    receiver_fullname: Optional[str] = None

class RoomWithLastMessage(Room):
    last_message: Optional[Message] = None

class PaginatedResponse(BaseModel, Generic[T]):
    items: List[T]
    total: int
    page: int
    size: int
    pages: int