# T.C. kimlik doğrulaması

## Neden varsayılan olarak kapalı

TC kimlik numarasının gerçekten o kişiye ait olup olmadığını doğrulamanın tek resmi yolu **KPS (Kimlik Paylaşımı Sistemi)**. KPS'e yalnızca yasal dayanağı olan kurumlar başvurabiliyor ve sorgular ücretli (başvuru: https://kpsbasvuru.nvi.gov.tr).

Doğrulanamayan bir TC numarasını toplamanın bir faydası yok, sadece zararı var:
- Başka birinin numarasıyla kayıt olunabilir ve numaranın gerçek sahibi daha sonra kayıt olamaz.
- "Doğrulanmış kullanıcı" izlenimi verir ama doğrulama yapılmamıştır.
- Hiçbir amaca hizmet etmeyen kişisel veri toplanmış olur (KVKK).

Bu nedenle varsayılan ayar `None`: TC numarası sorulmaz ve saklanmaz. Hastane personeli bağış sırasında donörün kimliğini zaten kimlik kartından kontrol eder.

## Modlar

`IdentityVerification:Provider` ayarı (compose'da `IDENTITY_VERIFICATION_PROVIDER`):

| Değer | Davranış |
|---|---|
| `None` (varsayılan) | TC numarası sorulmaz ve saklanmaz. Profil `isIdentityVerified: false` olur. |
| `DevChecksum` | **Sadece geliştirme ve test için.** Gerçek doğrulama yapmaz, yalnızca kontrol hanelerine bakar (`10000000146` geçer). `Production` ortamında açılırsa servis başlamaz. |
| *(ör. `Kps`)* | KPS erişimi olan bir kurumun ekleyeceği gerçek doğrulayıcı. |

Doğrulama açıkken (`None` dışındaki modlarda):
- TC numarası zorunludur ve doğrulanır.
- Numara düz metin olarak saklanmaz, `IdentityVerification:HashKey` anahtarıyla HMAC-SHA256 hash'i alınarak saklanır. Bu hash, aynı numarayla ikinci kaydı engeller. Veritabanında ayrıca unique index vardır.
- Başarılı doğrulamadan sonra profil `isIdentityVerified: true` olur.

Mobil uygulama, profil formunda TC alanını gösterip göstermeyeceğini `GET /profile/config` endpoint'inden öğrenir:

```json
{ "response": { "identityVerificationEnabled": false }, "isSuccess": true }
```

## Gerçek bir doğrulayıcı eklemek (ör. Kızılay + KPS)

1. `services/user-service/src/UserService.Infrastructure/Services/IdentityVerification/` altında `IIdentityVerifier` arayüzünü uygulayan bir sınıf yazın:

   ```csharp
   public class KpsIdentityVerifier : IIdentityVerifier
   {
       public Task<bool> VerifyAsync(IdentityVerificationRequest request, CancellationToken cancellationToken)
       {
           // KPS istemcisi: kurumun kimlik bilgileri ve NVİ'nin verdiği teknik dokümana göre.
       }
   }
   ```

2. `ServiceRegistration.AddIdentityVerification` içindeki `switch`'e `case "Kps"` ekleyin. KPS erişim bilgilerini yapılandırmadan okuyun, repoya koymayın.
3. `IDENTITY_VERIFICATION_PROVIDER=Kps` yapın ve güçlü bir `IDENTITY_HASH_KEY` üretin (`openssl rand -base64 32`). **Hash anahtarı sonradan değişirse** kayıtlı hash'ler eşleşmez ve tekrar kayıt kontrolü çalışmaz.

Profil tamamlama akışı, hata kodları (409: numara zaten kayıtlı, 422: doğrulanamadı) ve mobil uygulamadaki davranış değişmez.

