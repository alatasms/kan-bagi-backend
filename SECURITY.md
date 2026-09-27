# Güvenlik

Kan Bağı kişisel ve sağlıkla ilgili veri işler; güvenlik bildirimlerini ciddiye alıyoruz.

## Açık bildirme

Bir güvenlik açığı bulduysan lütfen **herkese açık bir issue açma**. Bunun yerine GitHub'daki depoda **Security → Report a vulnerability** yolunu kullan. Bildirim yalnızca proje ekibine görünür.

Bildirimde şunlar yardımcı olur:

- Etkilenen servis ve endpoint
- Açığı yeniden üretme adımları
- Olası etkisi (ör. başka bir kullanıcının verisine erişim)

Bildirimi aldığımızı birkaç gün içinde doğrularız. Düzeltme yayınlandıktan sonra, istersen adınla teşekkür ederiz.

## Kapsam

- Bu repodaki servisler ve `deploy/` altındaki yapılandırma.
- `deploy/.env.example` ve `deploy/keycloak/bloodapp-realm.json` içindeki şifreler ve test kullanıcıları yalnızca yerel geliştirme içindir ve bilerek herkese açıktır; bunlar açık sayılmaz. Gerçek kurulum için [docs/deployment.md](docs/deployment.md).
