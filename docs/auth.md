# Kimlik ve yetkilendirme

Kimlik sağlayıcı olarak Keycloak kullanılır. Realm tanımı `deploy/keycloak/bloodapp-realm.json` dosyasındadır ve compose her açılışta bu dosyayı import eder. Azure AD B2C tamamen kaldırıldı.

## Realm: `bloodapp`

| Öğe | Değer |
|---|---|
| Issuer | `${KEYCLOAK_PUBLIC_URL}/realms/bloodapp` (varsayılan: `http://localhost:8080/realms/bloodapp`) |
| Mobil client | `bloodapp-mobile`: public client, Authorization Code + PKCE (S256) |
| Redirect URI | `bloodapp://auth/callback`, geliştirme için `http://localhost:*` |
| API audience | `bloodapp-api`. Tüm servisler token'daki `aud` değerinde bunu arar |
| Roller | `donor` (her yeni kullanıcıya otomatik verilir), `hospital_staff`, `admin` |

`hospital_staff` ve `admin` rollerini sadece bir admin, Keycloak yönetim panelinden atayabilir (`http://localhost:8080/admin`). Kullanıcı kendi rolünü seçemez.

### Access token'daki claim'ler

| Claim | İçerik |
|---|---|
| `sub` | Kullanıcı id'si (UUID). Tüm servislerde kullanıcı kimliği olarak bu kullanılır |
| `email` | E-posta |
| `roles` | Realm rolleri, düz bir dizi olarak: `["donor", "hospital_staff", ...]` |
| `aud` | `bloodapp-api` içerir |

## Geliştirme kullanıcıları

Realm dosyasında tanımlıdırlar ve **yalnızca yerel geliştirme için** kullanılmalıdır.

| E-posta | Şifre | Roller |
|---|---|---|
| `donor@bloodapp.local` | `donor123` | donor |
| `staff@bloodapp.local` | `staff123` | donor, hospital_staff |
| `admin@bloodapp.local` | `admin123` | donor, admin |

Terminalden token almak için:

```bash
TOKEN=$(curl -s http://localhost:8080/realms/bloodapp/protocol/openid-connect/token \
  -d grant_type=password -d client_id=bloodapp-mobile \
  -d username=donor@bloodapp.local -d password=donor123 | jq -r .access_token)

curl -H "Authorization: Bearer $TOKEN" http://localhost:8000/profile/get-session-info
```

Password grant (`directAccessGrantsEnabled`) yalnızca bu tür yerel testler için açık bırakıldı. Gerçek bir ortamda kapatılmalıdır.

## Servisler için kurallar

1. **Her servis JWT'yi kendisi doğrular:** imza (JWKS), `iss`, `aud=bloodapp-api` ve `exp`. Discovery adresi: `http://keycloak:8080/realms/bloodapp/.well-known/openid-configuration`.
2. **Kullanıcı kimliği yalnızca `sub` claim'inden alınır.** Header'lardan (`sub`, `email`, `user_id`), path'ten ya da istek gövdesinden alınmaz. Gateway bu header'ları gelen isteklerden siler.
3. Rol kontrolü `roles` claim'i üzerinden yapılır. Örnek: QR doğrulama yalnızca `hospital_staff` rolüne açıktır.
4. Gateway, `Authorization` header'ını olduğu gibi alt servise iletir. Servisler birbirini çağırırken de kullanıcının token'ını iletir.

Referans uygulama: .NET servislerinde `Auth/KeycloakAuthenticationExtensions.cs` ve `Application/Security/CurrentUser.cs`.

### Python (chat)

Referans uygulama: `services/chat-service/app/auth.py`. `PyJWT` ve `PyJWKClient` ile imza, `iss`, `aud` ve `exp` kontrol edilir; ayarlar `AUTH_ISSUER`, `AUTH_JWKS_URL`, `AUTH_AUDIENCE`.

**Chat WebSocket:** Token `Authorization` header'ında ya da tarayıcılar header gönderemediği için `?access_token=` parametresinde gelir. Adresteki `user_id` token'daki `sub` ile aynı değilse bağlantı reddedilir. Gönderen, alıcı ve oda bağlantıdan belirlenir; istemciden sadece mesaj metni alınır.

### Spring Boot (matching)

Referans uygulama: `services/matching-service/src/main/java/com/onurcetin/BloodApp/config/SecurityConfig.java`. `spring-boot-starter-oauth2-resource-server` ile JWKS'ten imza, issuer, audience ve süre doğrulanır; `roles` claim'indeki değerler önek olmadan yetkiye çevrilir (ör. `hospital_staff`). Ayarlar `AUTH_ISSUER`, `AUTH_JWKS_URI`, `AUTH_AUDIENCE`. Çağıran kullanıcı `Authentication#getName()` (token'daki `sub`).

