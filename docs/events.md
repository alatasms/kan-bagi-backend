# Event sözleşmeleri

Servisler RabbitMQ üzerinden event'lerle haberleşir. .NET servisleri MassTransit kullanır; her event, **adı sabit olan bir fanout exchange'e** yayınlanır. Bir event'i dinlemek isteyen her servis kendi kuyruğunu oluşturur ve o exchange'e bağlar. Kuyruk adları servis adıyla başlamalıdır; aksi halde iki servis aynı kuyruğu paylaşır ve mesajları birbirinden kapar.

Kuyruğu ve bağlamayı dinleyen servis kendisi tanımlar (`exchange_declare` fanout durable → `queue_declare` → `queue_bind`). RabbitMQ panelinden elle kurulum gerekmez.

## Mesaj formatı

MassTransit mesajları bir zarf (envelope) içinde gönderir. Asıl veri `message` alanındadır ve alan adları camelCase'tir:

```json
{
  "messageId": "…",
  "correlationId": "…",
  "messageType": ["urn:message:PostService.Domain.Events:NewPostCreatedEvent", "…"],
  "sentTime": "2026-09-27T15:17:40Z",
  "message": { "postId": "…", "bloodTypeCode": "A_Positive", "…": "…" }
}
```

Python servisleri `json.loads(body)["message"]` ile okur. Bir .NET servisi başka bir servisin event'ini dinleyecekse, iki taraf da aynı `[MessageUrn]` değerini kullanmalıdır (bkz. `user-profile-completed`). MassTransit mesajları tip adıyla eşleştirir ve namespace farklıysa mesajı sessizce atlar.

**Kişisel veri kuralı:** Event'ler yalnızca işi yapmak için gereken veriyi taşır. Alıcı listeleri, telefon listeleri ve TC kimlik numarası event'e konmaz. İstisna `notification-preferences-changed` event'idir: bildirim gönderebilmesi için e-posta adresi yalnızca notification-service'e gider.

## Event'ler

### `user-registered`, user-service
Profil tamamlandığında yayınlanır. Hoş geldin e-postası için kullanılır.

| Alan | Açıklama |
|---|---|
| `userId`, `name`, `email`, `correlationId`, `occurredOn` | |

Dinleyen: notification-service (`user-registered-queue`).

### `user-profile-completed`, user-service
URN: `urn:message:user-profile-completed`

| Alan | Açıklama |
|---|---|
| `userId`, `name`, `surname`, `occurredOn` | Ad ve soyad profilden gelir |

Dinleyen: post-service (`post-service.user-profile-completed`). İlan sahibinin adını buradan doldurulan `PostOwners` tablosundan alır; istekte gönderilen isim kullanılmaz.

### `notification-preferences-changed`, user-service
Profil tamamlandığında (varsayılan tercihlerle) ve tercihler her değiştiğinde yayınlanır.

| Alan | Açıklama |
|---|---|
| `userId`, `name`, `email` | |
| `emailEnabled`, `phoneNumberEnabled`, `pushNotificationEnabled` | Kanal tercihleri |
| `preferredHospitalIds` | Hastane id'leri (string). **Boş liste = tüm hastaneler** |
| `preferredBloodTypes` | Bildirim alınmak istenen ilan kan grupları, ör. `["A_Positive", "AB_Positive"]`. Varsayılan değer, donörün kan verebileceği grupların tamamıdır |

Dinleyen: notification-service (`notification-preferences-changed-queue`). Bu event'lerden kendi `subscriber` koleksiyonunu tutar.

### `new-post-created`, post-service

| Alan | Açıklama |
|---|---|
| `postId`, `ownerId`, `ownerName`, `ownerSurname` | |
| `patientFullName`, `patientAge`, `title`, `description`, `phoneNumbers` | İlanda zaten herkese görünen bilgiler |
| `bloodType` | Görünen biçim, ör. `A+` |
| `bloodTypeCode` | Enum adı, ör. `A_Positive`; tercihlerdeki değerlerle aynı biçim |
| `hospitalId`, `hospital` (`hospitalName`, `hospitalAddress`, `district`, `city`, `country`) | |

Dinleyen: notification-service (`new-post-created-queue`). Alıcıları kendisi seçer: e-postası açık, `bloodTypeCode` değeri tercihlerinde bulunan, hastane filtresi boş ya da ilanın hastanesini içeren ve ilan sahibi olmayan aboneler.

### `user-post-expired`, post-service

| Alan | Açıklama |
|---|---|
| `postId`, `ownerId`, `ownerName`, `ownerSurname`, ilan alanları, `hospital` | Sahibin e-postası **yoktur**; notification-service onu `ownerId` ile abone listesinden bulur |

Dinleyen: notification-service (`user-post-expired-queue`).

### `post-deleted`, post-service
`postId`, `correlationId`. Şu an dinleyen yok.

### Servis içi
- `PostExpiryService` (post-service): süresi dolan ilanları periyodik olarak pasif yapar ve `user-post-expired` yayınlar.

## Teslim garantisi

user-service ve post-service, MassTransit'in **transactional outbox**'ını kullanır. Bir istek sırasında yayınlanan event'ler, veri değişikliğiyle aynı veritabanı transaction'ında `OutboxMessage` tablosuna yazılır ve commit'ten sonra RabbitMQ'ya gönderilir. Bu yüzden:

- Veri kaydedildiyse event de er geç gönderilir; RabbitMQ o an kapalıysa açıldığında gönderilir.
- Kayıt başarısız olursa event gönderilmez.
- Event'ler en az bir kez (at-least-once) teslim edilir. Dinleyen servis aynı mesajı iki kez alabileceğini hesaba katmalıdır. post-service consumer'larında bunun için inbox var. notification-service'te abonelik güncellemeleri upsert olduğu için tekrarlar zararsız, ancak aynı `new-post-created` iki kez gelirse e-posta iki kez gidebilir (plan: 5. adım).

Kod kuralı: handler'larda önce `Publish`, en son `SaveChangesAsync` çağrılır. `SaveChangesAsync`'ten sonra yapılan bir `Publish` outbox'a hiç yazılmaz.
