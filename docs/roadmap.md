# Yol haritası

Katkı vermek isteyenler için açık işler. Bir işe başlamadan önce issue açıp konuşmak iyi olur ([CONTRIBUTING.md](../CONTRIBUTING.md)).

## Ürün

- **Android uygulaması:** [bedirhantong/kan-bagi](https://github.com/bedirhantong/kan-bagi) içindeki Android uygulamasının Keycloak girişine ve bu backend'in güncel API'sine uyarlanması.
- **Telefon doğrulaması:** Telefon numarasının SMS koduyla (OTP) doğrulanması.
- **İlan onayı:** İlanların, ilgili hastanenin personeli tarafından onaylanması. Altyapı hazır: `hospital_staff` rolü mevcut; personelin bir hastaneyle ilişkilendirilmesi gerekiyor.
- **Kimlik doğrulaması:** KPS erişimi olan bir kurum için `IIdentityVerifier` uygulaması ([identity-verification.md](identity-verification.md)).
- **Anlık bildirim:** Bildirim tercihlerinde push seçeneği var, gönderimi yok.
- **Hastane verisi:** Şimdilik Antalya ile sınırlı; diğer iller.

## Teknik

- **chat-service:** Oda oluşturulurken katılımcıların adları istekten alınıyor; profillerden gelmeli (user-service event'leriyle, post-service'teki `PostOwners` gibi).
- **matching-service:** Eşleşme oluşturulurken `ownerId` istekten alınıyor; ilan sahibi post-service'ten doğrulanmalı.
- **notification-service:** Geliştirme endpoint'lerinin ayrı bir araca taşınması.
- **Veritabanı kullanıcıları:** Her servisin yalnızca kendi veritabanına erişen ayrı bir kullanıcısı olması.
- **Health check'ler:** Compose'da uygulama servisleri için healthcheck tanımları.
