# chat-service

Donör ile ilan sahibi arasında gerçek zamanlı mesajlaşma. FastAPI, WebSocket ve MongoDB kullanır. Geliştiren: İbrahim Serhan Baymaz.

## Erişim kuralları

Her istek Keycloak access token'ı ister ([docs/auth.md](../../docs/auth.md)). Kullanıcı her zaman token'dan belirlenir:

- Oda yalnızca katılımcılarından biri tarafından açılabilir.
- Kullanıcı yalnızca kendi odalarını listeler; odanın mesajlarını yalnızca katılımcılar okur (admin hepsini görebilir).
- WebSocket bağlantısında adresteki `user_id` token'daki kullanıcı olmalıdır. Gönderen, alıcı ve oda bağlantıdan belirlenir; istemciden yalnızca mesaj metni alınır.

## Endpoint'ler

Gateway üzerinden `/chat/...`, servis içinde `/api/chat/...`.

| Metot | Yol | Açıklama |
|---|---|---|
| POST | `/create-room` | İki kullanıcı arasında oda açar ya da var olanı döner |
| GET | `/get-rooms-by-user-id` | Kullanıcının odaları ve son mesajları |
| GET | `/get-messages-by-room-id?room_id=&page=&size=` | Odanın mesajları (`size` en fazla 100) |
| WebSocket | `/ws/{room_id}/{user_id}` | Gerçek zamanlı mesajlaşma. Token `Authorization` header'ında ya da `?access_token=` parametresinde |

Gönderilen mesaj biçimi: `{"content": "Merhaba"}`. Odadaki herkese `"<gönderen id>: <metin>"` olarak iletilir.

## Ayarlar

| Değişken | Açıklama |
|---|---|
| `MONGO_URI` | MongoDB bağlantısı (`mongodb://mongo:27017`) |
| `AUTH_ISSUER` | Token issuer'ı, ör. `http://localhost:8080/realms/bloodapp` |
| `AUTH_JWKS_URL` | Keycloak imza anahtarları, ör. `http://keycloak:8080/realms/bloodapp/protocol/openid-connect/certs` |
| `AUTH_AUDIENCE` | `bloodapp-api` |
| `APP_ENV` | `development` olduğunda `/chat` adresinde elle test sayfası açılır |

## Çalıştırma ve test

Servis `deploy/docker-compose.yml` ile diğer servislerle birlikte çalışır. Testler:

```bash
pip install -r requirements-dev.txt
python -m pytest tests
```
