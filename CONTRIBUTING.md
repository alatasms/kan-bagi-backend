# Katkı rehberi

Kan Bağı'na katkı vermek istediğin için teşekkürler. Hata bildirimleri, dokümantasyon düzeltmeleri ve kod katkılarının hepsi değerli.

## Başlamadan önce

- Küçük düzeltmeler için doğrudan pull request açabilirsin.
- Davranış değiştiren ya da birden fazla servisi etkileyen işler için önce bir issue açıp yaklaşımı konuşalım.
- Güvenlik açıklarını issue olarak açma; [SECURITY.md](SECURITY.md) dosyasındaki yolu kullan.

## Geliştirme ortamı

```bash
cd deploy
cp .env.example .env
docker compose up --build
./smoke-test.sh
```

Servislerin ve adreslerin listesi [README.md](README.md) dosyasında. Kimlik doğrulama için [docs/auth.md](docs/auth.md), servisler arası mesajlar için [docs/events.md](docs/events.md) okunmalı.

## Kurallar

Bu kurallar projenin güvenliğini ve servislerin bağımsızlığını korur; pull request'ler bunlara göre incelenir.

1. **Kullanıcı kimliği yalnızca doğrulanmış token'dan alınır.** Header, URL ya da istek gövdesindeki bir id asla "istek yapan kişi" olarak kullanılmaz. Her servis token'ı kendisi doğrular.
2. **Yetki rollere dayanır** (`donor`, `hospital_staff`, `admin`). Kullanıcının kendi rolünü seçtiği bir akış eklenmez.
3. **Servisler birbirinin veritabanını okumaz.** Veri event'ler ya da açık API'ler üzerinden paylaşılır. Yeni ya da değişen bir event, [docs/events.md](docs/events.md) dosyasına yazılır.
4. **Event'ler gereğinden fazla kişisel veri taşımaz.** Alıcı listeleri, telefon listeleri ve kimlik numaraları event'e konmaz.
5. **.NET servislerinde event'ler outbox üzerinden yayınlanır:** önce `Publish`, en son `SaveChangesAsync`.
6. **Repoya gizli bilgi girmez.** Yeni bir ayar gerekiyorsa `deploy/.env.example` dosyasına geliştirme değeriyle eklenir.
7. **Mobil uygulamayı etkileyen API değişiklikleri** pull request açıklamasında açıkça belirtilir; mobil uygulama ayrı bir repoda ([bedirhantong/kan-bagi](https://github.com/bedirhantong/kan-bagi)).

## Kod

- Çevredeki kodun stiline uy. Yorumlar kodun ne yaptığını ve neden öyle yaptığını anlatır; eski kodun nasıl olduğunu anlatmaz.
- .NET çözümü uyarısız derlenir (`dotnet build -warnaserror`); öyle kalmalı.
- Davranış değiştiren her değişiklik bir test içerir. Birden fazla servisi ilgilendiren akışlar için `deploy/smoke-test.sh` genişletilebilir.
- Veritabanı şeması değişiyorsa migration eklenir (`dotnet ef migrations add ...`); mevcut veriyi bozan bir migration, veriyi dönüştüren adımı da içerir.

## Lisans

Gönderdiğin katkılar, projenin lisansı olan [Apache License 2.0](LICENSE) altında kabul edilir.

## Pull request

- Ne değiştiğini ve neden değiştiğini açıkla; ilgili issue'yu bağla.
- CI yeşil olmalı: build, testler, açıklı paket kontrolü ve uçtan uca smoke test.
- Dokümantasyonu kodla aynı pull request'te güncelle.
