from fastapi import FastAPI, HTTPException, WebSocket, WebSocketDisconnect, Request, APIRouter, Depends, Query, status
from fastapi.responses import HTMLResponse, JSONResponse
from fastapi.middleware.cors import CORSMiddleware
from fastapi.templating import Jinja2Templates
from pathlib import Path
from typing import List, Optional
import json
import logging
import os
import re
from pydantic import BaseModel
from prometheus_client import make_asgi_app, Counter, Histogram
from .auth import CurrentUser, current_user, websocket_user
from .models import Message, Room, RoomWithLastMessage, StandardResponse, PaginatedResponse, MessageWithFullNames
from .crud import create_message, get_room_messages, create_room, get_room, get_user_rooms_with_last_messages, get_room_by_users
from .database import mongo_db

logger = logging.getLogger(__name__)

APP_ENV = os.getenv("APP_ENV", "production")
MAX_MESSAGE_LENGTH = 2000
MAX_PAGE_SIZE = 100

# Prometheus metrics
http_requests_total = Counter(
    'http_requests_total',
    'Total number of HTTP requests',
    ['method', 'endpoint', 'status']
)

http_request_duration_seconds = Histogram(
    'http_request_duration_seconds',
    'HTTP request duration in seconds',
    ['method', 'endpoint']
)

# Create Prometheus metrics endpoint
metrics_app = make_asgi_app()

def validate_user_id(user_id: str) -> bool:
    # Remove spaces and special characters, keep only alphanumeric characters
    cleaned_id = re.sub(r'[\W_]', '', user_id)
    return len(cleaned_id) >= 3

app = FastAPI(
    title="Chat Microservice API",
    description="İki kullanıcı arasında özel mesajlaşma sağlayan bir API",
    version="1.0.0",
    docs_url="/",
    redoc_url="/redoc"
)

# Mount Prometheus metrics endpoint
app.mount("/metrics", metrics_app)

# Create router for chat endpoints
chat_router = APIRouter(prefix="/api/chat", tags=["chat"])

# Template ve static dosyaların yolunu belirleme
BASE_PATH = Path(__file__).resolve().parent
templates = Jinja2Templates(directory=str(BASE_PATH / "templates"))

# CORS ayarları
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)


@app.exception_handler(HTTPException)
async def http_exception_handler(request: Request, exc: HTTPException):
    """Errors keep the StandardResponse shape and carry the real HTTP status."""
    body = StandardResponse(isSuccess=False, resultCode=str(exc.status_code), resultMessage=str(exc.detail))
    return JSONResponse(status_code=exc.status_code, content=body.model_dump())


def forbidden(message: str = "Bu işlem için yetkiniz yok."):
    return HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail=message)


class ConnectionManager:
    def __init__(self):
        self.active_connections: dict[str, list[WebSocket]] = {}

    async def connect(self, websocket: WebSocket, room_id: str):
        await websocket.accept()
        if room_id not in self.active_connections:
            self.active_connections[room_id] = []
        self.active_connections[room_id].append(websocket)

    def disconnect(self, websocket: WebSocket, room_id: str):
        if room_id in self.active_connections:
            if websocket in self.active_connections[room_id]:
                self.active_connections[room_id].remove(websocket)
            if not self.active_connections[room_id]:
                del self.active_connections[room_id]

    async def broadcast_to_room(self, message: str, room_id: str):
        if room_id in self.active_connections:
            for connection in self.active_connections[room_id]:
                await connection.send_text(message)

manager = ConnectionManager()

@app.on_event("startup")
async def startup_event():
    await mongo_db.connect()

@app.on_event("shutdown")
async def shutdown_event():
    await mongo_db.close()

@app.get("/health")
async def health():
    return {"status": "running"}

@app.get("/chat", response_class=HTMLResponse, include_in_schema=False)
async def get(request: Request):
    # Manual test page; not served outside development.
    if APP_ENV != "development":
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Not found")
    return templates.TemplateResponse("chat.html", {"request": request})

class CreateRoomRequest(BaseModel):
    user1_id: str
    user2_id: str
    user1_fullname: str
    user2_fullname: str

@chat_router.post("/create-room", response_model=StandardResponse[Room],
         summary="Yeni sohbet odası oluştur",
         description="İki kullanıcı arasında özel bir sohbet odası oluşturur veya varsa mevcut odayı döner")
async def create_chat_room(request: CreateRoomRequest, user: CurrentUser = Depends(current_user)):
    """
    İki kullanıcı arasında özel bir sohbet odası oluşturur veya varsa mevcut odayı döner.
    Oda yalnızca katılımcılardan biri tarafından oluşturulabilir.

    - **user1_id**: Birinci kullanıcının ID'si
    - **user2_id**: İkinci kullanıcının ID'si
    - **user1_fullname**: Birinci kullanıcının tam adı
    - **user2_fullname**: İkinci kullanıcının tam adı
    """
    if user.user_id not in (request.user1_id, request.user2_id):
        raise forbidden("Yalnızca katılımcısı olduğunuz bir oda oluşturabilirsiniz.")

    try:
        # Validate user IDs
        if not validate_user_id(request.user1_id) or not validate_user_id(request.user2_id):
            return StandardResponse(
                isSuccess=False,
                resultCode="400",
                resultMessage="Kullanıcı ID'leri en az 3 geçerli karakter içermelidir (boşluk ve özel karakterler sayılmaz)"
            )

        # Önce mevcut odayı kontrol et
        existing_room = await get_room_by_users(request.user1_id, request.user2_id)

        if existing_room:
            return StandardResponse(
                response=existing_room,
                isSuccess=True,
                resultCode="200",
                resultMessage=f"Bu kullanıcılar arasında zaten bir oda mevcut (room_id: {existing_room.room_id})"
            )

        # Mevcut oda yoksa yeni oda oluştur
        room = await create_room(
            request.user1_id,
            request.user2_id,
            request.user1_fullname,
            request.user2_fullname
        )
        if not room.isSuccess:
            if request.user1_id == request.user2_id:
                return StandardResponse(
                    isSuccess=False,
                    resultCode="400",
                    resultMessage="Aynı kullanıcı ID'si ile oda oluşturulamaz"
                )
            elif not request.user1_id or not request.user2_id:
                return StandardResponse(
                    isSuccess=False,
                    resultCode="400",
                    resultMessage="Kullanıcı ID'leri boş olamaz"
                )
            return StandardResponse(
                isSuccess=False,
                resultCode="500",
                resultMessage="Oda oluşturulurken bir hata oluştu"
            )

        return StandardResponse(
            response=room,
            isSuccess=True,
            resultCode="200",
            resultMessage="Oda başarıyla oluşturuldu"
        )
    except Exception:
        logger.exception("Oda oluşturulamadı")
        return StandardResponse(
            isSuccess=False,
            resultCode="500",
            resultMessage="Oda oluşturulurken bir hata oluştu"
        )

@chat_router.get("/get-rooms-by-user-id",
         response_model=StandardResponse[List[RoomWithLastMessage]],
         summary="Kullanıcının sohbet odalarını listele",
         description="Giriş yapan kullanıcının sohbet odalarını son mesajlarıyla birlikte listeler")
async def get_user_chat_rooms(user_id: Optional[str] = None, user: CurrentUser = Depends(current_user)):
    # user_id is optional and, when given, must be the caller's own id (admins may list anyone's rooms).
    target_user_id = user_id or user.user_id
    if target_user_id != user.user_id and not user.is_admin:
        raise forbidden("Yalnızca kendi odalarınızı görebilirsiniz.")
    try:
        rooms = await get_user_rooms_with_last_messages(target_user_id)
        return StandardResponse(
            response=rooms,
            isSuccess=True,
            resultCode="200",
            resultMessage="Odalar başarıyla getirildi"
        )
    except Exception:
        logger.exception("Odalar getirilemedi")
        return StandardResponse(
            isSuccess=False,
            resultCode="500",
            resultMessage="Odalar getirilirken bir hata oluştu"
        )

@chat_router.get("/get-messages-by-room-id",
         response_model=StandardResponse[PaginatedResponse[MessageWithFullNames]],
         summary="Oda mesajlarını getir",
         description="Katılımcısı olunan sohbet odasındaki mesajları sayfalı şekilde getirir")
async def get_messages_in_room(
    room_id: str,
    page: int = Query(1, ge=1),
    size: int = Query(MAX_PAGE_SIZE, ge=1, le=MAX_PAGE_SIZE),
    user: CurrentUser = Depends(current_user)
):
    room = await get_room(room_id)
    if not room:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Oda bulunamadı.")
    if user.user_id not in (room.user1_id, room.user2_id) and not user.is_admin:
        raise forbidden("Bu odanın mesajlarını görme izniniz yok.")

    try:
        skip = (page - 1) * size
        messages, total = await get_room_messages(room_id, skip, size)

        pages = (total + size - 1) // size  # Toplam sayfa sayısı

        paginated_response = PaginatedResponse(
            items=messages,
            total=total,
            page=page,
            size=size,
            pages=pages
        )

        return StandardResponse(
            response=paginated_response,
            isSuccess=True,
            resultCode="200",
            resultMessage="Mesajlar başarıyla getirildi"
        )
    except Exception:
        logger.exception("Mesajlar getirilemedi")
        return StandardResponse(
            isSuccess=False,
            resultCode="500",
            resultMessage="Mesajlar getirilirken bir hata oluştu"
        )

@app.websocket("/api/chat/ws/{room_id}/{user_id}")
async def websocket_endpoint(websocket: WebSocket, room_id: str, user_id: str):
    # The path keeps user_id for compatibility, but it must match the token; the token decides who speaks.
    user = await websocket_user(websocket)
    if user is None or user.user_id != user_id:
        await websocket.close(code=status.WS_1008_POLICY_VIOLATION)
        return

    try:
        room = await get_room(room_id)
        if not room:
            await websocket.accept()
            await websocket.send_text("Geçersiz oda ID'si veya oda bulunamadı.")
            await websocket.close(code=4000)
            return

        if user.user_id not in [room.user1_id, room.user2_id]:
            await websocket.accept()
            await websocket.send_text("Bu odaya erişim izniniz yok.")
            await websocket.close(code=4001)
            return

        # Alıcıyı belirleme - odadaki diğer kullanıcı
        receiver_user_id = room.user2_id if user.user_id == room.user1_id else room.user1_id

        await manager.connect(websocket, room_id)
        try:
            while True:
                data = await websocket.receive_text()
                # Only the text comes from the client; sender, receiver and room are fixed by this connection.
                content = str(json.loads(data).get("content", "")).strip()
                if not content:
                    continue
                message = Message(
                    sender_user_id=user.user_id,
                    receiver_user_id=receiver_user_id,
                    room_id=room_id,
                    content=content[:MAX_MESSAGE_LENGTH],
                )
                created_message = await create_message(message)
                await manager.broadcast_to_room(
                    f"{created_message.sender_user_id}: {created_message.content}",
                    room_id
                )
        except WebSocketDisconnect:
            manager.disconnect(websocket, room_id)
            await manager.broadcast_to_room(
                f"Kullanıcı {user.user_id} sohbetten ayrıldı",
                room_id
            )
        except Exception:
            logger.exception("WebSocket error")
            manager.disconnect(websocket, room_id)
            await websocket.close()
    except Exception:
        logger.exception("Connection error")
        await websocket.close()

# Include the chat router
app.include_router(chat_router)
