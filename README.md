# Kan Bağı: backend

Kan ihtiyacı çağrıları bugün WhatsApp gruplarına, Instagram hikâyelerine ve kurum içi e-postalara dağılmış durumda. Çoğu geç görülüyor, bir kısmı doğrulanamıyor. Kan Bağı bu çağrıları tek bir mobil uygulamada toplar:

- Kullanıcılar Keycloak ile kaydolur. KPS erişimi olan bir kurum, TC kimlik doğrulamasını açabilir ([docs/identity-verification.md](docs/identity-verification.md)).
- İhtiyaç sahibi hastane ve kan grubu seçerek ilan açar. İlanlar harita ve liste üzerinde görünür.
- Kan grubu uyumlu ve bildirim tercihleri uyan kullanıcılara e-posta ile haber verilir.
- Donör ilan sahibiyle uygulama içinden yazışır, uygunluk formunu doldurur ve bir QR kod alır.
- Hastane personeli QR kodu okutarak bağışın gerçekten yapıldığını doğrular.

Bu repo backend servislerini içerir. Mobil uygulama ayrı bir repoda: [bedirhantong/kan-bagi](https://github.com/bedirhantong/kan-bagi) (Android uygulaması bu backend'i, iOS uygulaması Supabase'i kullanır). Hastane verisi şimdilik Antalya ile sınırlı.

## Mimari

| Servis | Teknoloji | Görev |
|---|---|---|
| `services/gateway` | .NET 9, Ocelot | Tek giriş noktası, token doğrulama, yönlendirme |
| `services/user-service` | .NET 9, PostgreSQL | Profil, isteğe bağlı TC kimlik doğrulaması, bildirim tercihleri |
| `services/post-service` | .NET 9, PostgreSQL | Kan ilanları, ilan süresi, hastane ve konum verisi |
| `services/notification-service` | Python, Flask, MongoDB | Bildirim alıcılarını seçer, e-posta gönderir |
| `services/chat-service` | Python, FastAPI, MongoDB | Donör ve ilan sahibi arasında mesajlaşma |
| `services/matching-service` | Spring Boot, PostgreSQL | Eşleşme, uygunluk formu, QR ile bağış doğrulama |

Kimlik doğrulama Keycloak ile yapılır; her servis token'ı kendisi doğrular ([docs/auth.md](docs/auth.md)). Servisler RabbitMQ üzerinden event'lerle haberleşir ([docs/events.md](docs/events.md)). Dağıtık izleme OpenTelemetry (OTLP) ve Jaeger ile yapılır.

## Yerelde çalıştırma

Gereksinim: Docker. Servisleri IDE'den çalıştırmak için ayrıca .NET 9 SDK, Python 3.11 ve Java 21.

```bash
cd deploy
cp .env.example .env
docker compose up --build
```

| Adres | Ne |
|---|---|
| http://localhost:8000 | API gateway |
| http://localhost:8080 | Keycloak (yönetim: `admin` / `.env` içindeki şifre) |
| http://localhost:8025 | Mailpit: geliştirmede gönderilen tüm e-postalar |
| http://localhost:16686 | Jaeger: dağıtık izleme |
| http://localhost:15672 | RabbitMQ yönetim paneli |

Test kullanıcıları ve token alma örneği [docs/auth.md](docs/auth.md) dosyasında.

Bir servisi IDE'den çalıştırmak için altyapıyı compose ile başlatın (`docker compose up postgres mongo rabbitmq jaeger keycloak mailpit`) ve servisi `Development` ortamında çalıştırın. `appsettings.Development.json` dosyaları `.env.example` içindeki geliştirme değerlerini kullanır.

> Bu compose kurulumu **yalnızca geliştirme içindir**: Keycloak geliştirme modunda çalışır, test kullanıcıları ve şifreleri herkesçe bilinir, TLS yoktur. Gerçek bir kurulum için [docs/deployment.md](docs/deployment.md).

## Test

```bash
dotnet test BloodApp.sln                                           # .NET birim testleri
cd services/notification-service && python -m pytest tests
cd services/chat-service && pip install -r requirements-dev.txt && python -m pytest tests
cd deploy && ./smoke-test.sh                                        # çalışan stack üzerinde uçtan uca kontrol
```

CI aynı testleri ve uçtan uca kontrolü her push'ta çalıştırır ([.github/workflows/ci.yml](.github/workflows/ci.yml)).

## Katkı

Katkılar memnuniyetle karşılanır; nasıl başlanacağı [CONTRIBUTING.md](CONTRIBUTING.md), açık işler [docs/roadmap.md](docs/roadmap.md) dosyasında. Güvenlik açıklarını herkese açık bir issue yerine [SECURITY.md](SECURITY.md) dosyasındaki yolla bildirin.

## Ekip

- Şeyhmus Alataş: user-service, post-service, gateway
- İbrahim Serhan Baymaz: notification-service, chat-service
- Onur Çetin: matching-service
- Bedirhan Tonğ: mobil uygulama

## Lisans

[Apache License 2.0](LICENSE). Telif ve atıf bilgisi [NOTICE](NOTICE) dosyasında.
