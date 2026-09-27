# matching-service

Donör ile ilan arasındaki eşleşmeyi, kan bağışı uygunluk formunu ve QR kod ile bağış doğrulamasını yönetir. Spring Boot 3.4, Java 21 ve PostgreSQL kullanır. Geliştiren: Onur Çetin.

## Akış

1. Donör bir ilana yanıt verir (`POST /create`) ve eşleşme id'sini taşıyan bir QR kod alır.
2. Donör uygunluk formunu doldurur (`POST /evaluation-form/createForm`).
3. Hastanede personel QR kodu okutur, donörün formunu açar (`GET /evaluation-form/getFormById?donorId=`) ve bağışı doğrular (`POST /validate`). Doğrulayan kişi ve zaman kaydedilir.

## Erişim kuralları

Her istek Keycloak access token'ı ister ([docs/auth.md](../../docs/auth.md)). Kullanıcı token'daki `sub` claim'idir; istekteki id'ler kullanıcıyı belirlemez.

| Endpoint | Kim |
|---|---|
| `POST /api/matching/create` | Donör (kendisi için) |
| `GET /api/matching/{matchingId}` | Donör, ilan sahibi, `hospital_staff`, `admin` |
| `GET /api/matching/user/{userId}` | Kullanıcının kendisi, `hospital_staff`, `admin` |
| `POST /api/matching/validate` | Yalnızca `hospital_staff`; her bağış bir kez doğrulanır |
| `POST /api/evaluation-form/createForm`, `PUT /updateForm`, `GET /getForm` | Donör (kendi formu; her donörün tek formu var) |
| `GET /api/evaluation-form/getFormById?donorId=` | `hospital_staff`, `admin` |
| `GET /api/evaluation-form/questions` | Giriş yapmış herkes |

Hatalar `BaseResponse` biçiminde, gerçek HTTP status koduyla döner. Yetkilendirme kuralları `config/SecurityConfig.java` içindedir.

## Ayarlar

| Değişken | Açıklama |
|---|---|
| `DB_URL`, `DB_USERNAME`, `DB_PASSWORD` | PostgreSQL bağlantısı (`jdbc:postgresql://postgres:5432/matching`) |
| `AUTH_ISSUER` | Token issuer'ı, ör. `http://localhost:8080/realms/bloodapp` |
| `AUTH_JWKS_URI` | Keycloak imza anahtarları, ör. `http://keycloak:8080/realms/bloodapp/protocol/openid-connect/certs` |
| `AUTH_AUDIENCE` | `bloodapp-api` |

Actuator yalnızca `/actuator/health` ve `/actuator/prometheus` sunar.

## Build

```bash
mvn package
docker build -t matching-service .
```

