# notification-service

Kullanıcılara e-posta bildirimi gönderir ve kimin hangi bildirimi alacağına karar verir. Python 3.11, Flask, pika (RabbitMQ) ve MongoDB kullanır. Geliştiren: İbrahim Serhan Baymaz.

Dışarıya açık bir API'si yoktur; gateway'den erişilmez. Diğer servislerle yalnızca event'ler üzerinden konuşur ([docs/events.md](../../docs/events.md)).

## Ne yapar

| Event | Sonuç |
|---|---|
| `notification-preferences-changed` | Kullanıcının abonelik kaydını (`subscriber` koleksiyonu) oluşturur ya da günceller |
| `user-registered` | Hoş geldin e-postası |
| `new-post-created` | E-postası açık, ilanın kan grubunu tercih eden, hastane filtresi boş ya da ilanın hastanesini içeren ve ilan sahibi olmayan abonelere kan ihtiyacı e-postası |
| `user-post-expired` | İlan sahibine, abonelik kaydındaki adresinden "ilanınızın süresi doldu" e-postası |

Kuyruklar ve exchange bağlantıları servis açılırken kod içinde oluşturulur. Mesajlar işlendikten sonra onaylanır; başarıyla işlenmiş bir mesaj tekrar gelirse (`messageId`) atlanır. İşlenen her mesaj `message_log` koleksiyonuna yazılır.

E-posta şablonları `app/templates/emails.py` içindedir; kullanıcıdan gelen alanlar HTML kaçışından geçirilir.

## Ayarlar

| Değişken | Açıklama |
|---|---|
| `RABBITMQ_URI` | ör. `amqp://kullanici:sifre@rabbitmq:5672/` |
| `MONGODB_URI_ADDRESS`, `MONGODB_NOTIFICATION_DB_NAME` | MongoDB bağlantısı ve veritabanı adı |
| `EMAIL_SERVER_ADDRESS`, `EMAIL_PORT_ADDRESS` | SMTP sunucusu |
| `EMAIL_USE_TLS` | STARTTLS (varsayılan `true`; yerel Mailpit için `false`) |
| `EMAIL_SENDER_USERNAME`, `EMAIL_SENDER_PASSWORD` | SMTP girişi; ikisi de boşsa giriş yapılmaz |
| `EMAIL_FROM` | Gönderen adresi (varsayılan: SMTP kullanıcı adı) |
| `SUPPORT_EMAIL` | E-postalarda gösterilen destek adresi; boşsa e-postalarda destek satırı yer almaz |
| `OTEL_EXPORTER_OTLP_TRACES_ENDPOINT` | Trace'lerin gönderileceği OTLP/HTTP adresi |
| `APP_ENV` | `development` olduğunda geliştirme endpoint'leri açılır |

`deploy/docker-compose.yml` bu değerleri `deploy/.env` dosyasından verir; varsayılan olarak e-postalar Mailpit'e gider (http://localhost:8025).

## Geliştirme endpoint'leri

`APP_ENV=development` iken `/test/*`, `/messages`, `/data` ve `/api/email/*` açılır: test mesajı gönderme, mesaj ve e-posta kayıtlarını görme. Kimlik doğrulaması olmadıkları için başka ortamlarda yalnızca `/health` ve `/metrics` yanıt verir.

## Test

```bash
pip install -r requirements.txt
python -m pytest tests
```
