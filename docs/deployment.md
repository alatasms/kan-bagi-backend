# Gerçek bir ortama kurulum

`deploy/docker-compose.yml` yerel geliştirme için hazırlanmıştır. Herkese açık bir sunucuya kurmadan önce aşağıdakilerin hepsi yapılmalıdır.

## Gizli bilgiler

- `deploy/.env` içindeki **her** değeri değiştirin. `.env.example`'daki şifreler herkesçe bilinir.
- TC kimlik doğrulaması açılacaksa `IDENTITY_HASH_KEY` için güçlü bir anahtar üretin (`openssl rand -base64 32`) ve saklayın. Anahtar sonradan değişirse kayıtlı hash'ler eşleşmez.
- Gizli bilgileri repoya koymayın. Bir secret yöneticisi ya da sunucuda yetkisi kısıtlı bir `.env` dosyası kullanın.

## Keycloak

- `start-dev` yerine `start` ile, TLS arkasında ve üretim veritabanıyla çalıştırın. `KC_HOSTNAME`, kullanıcıların gördüğü HTTPS adresi olmalı; bu adres token'lardaki issuer olur, bu yüzden `KEYCLOAK_PUBLIC_URL` ile aynı olmalıdır.
- `deploy/keycloak/bloodapp-realm.json` dosyasını olduğu gibi import etmeyin:
  - `donor@`, `staff@` ve `admin@bloodapp.local` test kullanıcılarını çıkarın.
  - `bloodapp-mobile` client'ında `directAccessGrantsEnabled` değerini `false` yapın (şifreyle doğrudan token alma yalnızca yerel testler içindir).
  - `redirectUris` ve `post.logout.redirect.uris` içinden `http://localhost:*` adresini çıkarın, mobil uygulamanın gerçek adresini bırakın.
  - Kullanıcı kaydı için e-posta doğrulamasını (`verifyEmail`) açın ve Keycloak'a bir SMTP sunucusu tanımlayın.
- Yönetici hesabının şifresini değiştirin ve yönetim arayüzünü herkese açmayın.
- `hospital_staff` rolünü yalnızca kimliği doğrulanmış hastane personeline verin. Bu rol QR ile bağış doğrulamasına ve donör formlarını okumaya izin verir.

## Ağ

- Dışarıya yalnızca gateway ve Keycloak açılmalıdır, ikisi de TLS sonlandıran bir reverse proxy arkasında.
- Compose'daki altyapı portları (`127.0.0.1` üzerindeki 5432, 5672, 15672, 27017, 16686, 4317, 4318 ve 8025) geliştirme kolaylığı içindir; sunucuda bunları kaldırın.
- Servisler arası trafik iç ağda kalmalıdır.

## E-posta

- Mailpit yerine gerçek bir SMTP sağlayıcısı tanımlayın: `SMTP_HOST`, `SMTP_PORT`, `SMTP_USE_TLS=true`, `SMTP_USERNAME`, `SMTP_PASSWORD`, `SMTP_FROM`.
- Gönderen alan adı için SPF, DKIM ve DMARC kayıtlarını ayarlayın; aksi halde bildirimler spam'e düşer.

## Veri

- PostgreSQL ve MongoDB için düzenli yedek alın ve geri yüklemeyi deneyin.
- Veritabanı kullanıcılarını servis başına ayırmak (her servis yalnızca kendi veritabanına erişsin) iyi bir sonraki adımdır.
- Profiller, bildirim tercihleri ve sohbetler kişisel veri içerir. KVKK kapsamında aydınlatma metni, saklama süresi ve silme talepleri için bir süreç belirleyin.

## Ayarlar

- .NET servisleri varsayılan olarak `Production` ortamında çalışır; Swagger kapalıdır.
- notification-service için `APP_ENV=production` bırakın; geliştirme endpoint'leri yalnızca `development` modunda açılır.
- `IDENTITY_VERIFICATION_PROVIDER` için `DevChecksum` production'da kabul edilmez ([identity-verification.md](identity-verification.md)).
- İlan süresini `POST_ACTIVE_DURATION` ile ayarlayın (varsayılan 3 gün).
