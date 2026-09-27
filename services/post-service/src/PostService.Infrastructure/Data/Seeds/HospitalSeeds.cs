using PostService.Domain.Entities;

namespace PostService.Infrastructure.Data.Seeds
{
    public static class HospitalSeeds
    {
        public static async Task Seed(PostServiceDbContext context)
        {
            var hospitals = new List<Hospital>
            {
                new Hospital
                {
                    Id = 1,
                    Name = "Antalya Atatürk Devlet Hastanesi",
                    Address = "Üçgen, Anafartalar Cd. No:100, 07040 Muratpaşa/Antalya",
                    PhoneNumber = "(0242) 345 45 50",
                    Email = null,
                    WebSite = "https://antalyaataturkdh.saglik.gov.tr",
                    Lat = 36.8969,
                    Lon = 30.6831,
                    DistrictId = 1
                },
                new Hospital
                {
                    Id = 2,
                    Name = "Antalya Eğitim ve Araştırma Hastanesi",
                    Address = "Varlık, Kazım Karabekir Cd., 07100 Muratpaşa/Antalya",
                    PhoneNumber = "(0242) 249 44 00",
                    Email = null,
                    WebSite = "https://antalyaeah.saglik.gov.tr",
                    Lat = 36.8609,
                    Lon = 30.7317,
                    DistrictId = 1
                },
                new Hospital
                {
                    Id = 3,
                    Name = "Kepez Devlet Hastanesi",
                    Address = "No:124, Hüsnü Karakaş, 15. Temmuz Şehitler Cd, 07320 Kepez/Antalya",
                    PhoneNumber = "(0242) 339 11 00",
                    Email = null,
                    WebSite = "https://kepezdh.saglik.gov.tr",
                    Lat = 36.9258,
                    Lon = 30.6597,
                    DistrictId = 2
                },
                new Hospital
                {
                    Id = 4,
                    Name = "Alanya Eğitim ve Araştırma Hastanesi",
                    Address = "Oba, Fidanlık Cd., 07400 Alanya/Antalya",
                    PhoneNumber = "(0242) 513 48 41",
                    Email = null,
                    WebSite = "https://alanyaea.saglik.gov.tr",
                    Lat = 36.5556,
                    Lon = 32.0032,
                    DistrictId = 3
                },
                new Hospital
                {
                    Id = 5,
                    Name = "Manavgat Devlet Hastanesi",
                    Address = "Çağlayan, Şelale Cd. Üzeri, 07600 Manavgat/Antalya",
                    PhoneNumber = "(0242) 746 11 22",
                    Email = null,
                    WebSite = "https://manavgatdh.saglik.gov.tr",
                    Lat = 36.7814,
                    Lon = 31.4478,
                    DistrictId = 4
                },
                new Hospital
                {
                    Id = 6,
                    Name = "Kemer Devlet Hastanesi",
                    Address = "Dedeler Mevkii, Yeni, 07980 Kemer/Antalya",
                    PhoneNumber = "(0242) 814 15 50",
                    Email = null,
                    WebSite = "https://kemerdh.saglik.gov.tr",
                    Lat = 36.6049,
                    Lon = 30.5602,
                    DistrictId = 5
                },
                new Hospital
                {
                    Id = 7,
                    Name = "Kaş Devlet Hastanesi",
                    Address = "Gökçeören, Antalya Fethiye Yolu, 07580 Kaş/Antalya",
                    PhoneNumber = "(0242) 836 10 22",
                    Email = null,
                    WebSite = "https://kasdh.saglik.gov.tr",
                    Lat = 36.2007,
                    Lon = 29.6391,
                    DistrictId = 6
                },
                new Hospital
                {
                    Id = 8,
                    Name = "Korkuteli Devlet Hastanesi",
                    Address = "UZUNOLUK MA, MEHMET AKİF ERSOY BULVARI :125/1, 07800 Korkuteli/Antalya",
                    PhoneNumber = "(0242) 643 60 22",
                    Email = null,
                    WebSite = "https://korkutelidh.saglik.gov.tr",
                    Lat = 37.0701,
                    Lon = 30.1956,
                    DistrictId = 7
                },
                new Hospital
                {
                    Id = 9,
                    Name = "Finike Devlet Hastanesi",
                    Address = "Finike, Antalya",
                    PhoneNumber = "(0242) 855 16 00",
                    Email = null,
                    WebSite = "https://finikedh.saglik.gov.tr",
                    Lat = 36.2998,
                    Lon = 30.1412,
                    DistrictId = 8
                },
                new Hospital
                {
                    Id = 10,
                    Name = "Gazipaşa Devlet Hastanesi",
                    Address = "Gazipaşa, Antalya",
                    PhoneNumber = "(0242) 572 72 00",
                    Email = null,
                    WebSite = "https://gazipasadh.saglik.gov.tr",
                    Lat = 36.2701,
                    Lon = 32.3165,
                    DistrictId = 9
                },
                new Hospital
                {
                    Id = 11,
                    Name = "Serik Devlet Hastanesi",
                    Address = "Merkez, 2026. Sk., 07500 Serik/Antalya",
                    PhoneNumber = "(0242) 722 15 40",
                    Email = null,
                    WebSite = "https://serikdh.saglik.gov.tr",
                    Lat = 36.9147,
                    Lon = 31.1049,
                    DistrictId = 10
                },
                new Hospital
                {
                    Id = 12,
                    Name = "Kumluca Devlet Hastanesi",
                    Address = "Kumluca, Antalya",
                    PhoneNumber = "(0242) 887 11 22",
                    Email = null,
                    WebSite = "https://kumlucadh.saglik.gov.tr",
                    Lat = 36.3712,
                    Lon = 30.2856,
                    DistrictId = 11
                },
                new Hospital
                {
                    Id = 13,
                    Name = "Özel Antalya Yaşam Hastanesi",
                    Address = "Muratpaşa, Antalya",
                    PhoneNumber = "(0242) 316 77 77",
                    Email = null,
                    WebSite = "https://www.yasamhastaneleri.com",
                    Lat = 36.8781,
                    Lon = 30.7249,
                    DistrictId = 1
                },
                new Hospital
                {
                    Id = 14,
                    Name = "Özel Akdeniz Şifa Hastanesi",
                    Address = "Konyaaltı, Antalya",
                    PhoneNumber = "(0242) 228 68 68",
                    Email = null,
                    WebSite = "https://www.akdenizsifa.com",
                    Lat = 36.8742,
                    Lon = 30.6298,
                    DistrictId = 12
                },
                new Hospital
                {
                    Id = 15,
                    Name = "Özel Medstar Antalya Hastanesi",
                    Address = "Muratpaşa, Antalya",
                    PhoneNumber = "(0242) 310 31 31",
                    Email = null,
                    WebSite = "https://www.medstar.com.tr",
                    Lat = 36.8586,
                    Lon = 30.7443,
                    DistrictId = 1
                },
                new Hospital
                {
                    Id = 16,
                    Name = "Özel medya Anadolu Hastanesi",
                    Address = "Kepez, Antalya",
                    PhoneNumber = "(0242) 249 70 00",
                    Email = null,
                    WebSite = "https://www.anadoluhastanesi.com",
                    Lat = 36.9119,
                    Lon = 30.6623,
                    DistrictId = 2
                },
                new Hospital
                {
                    Id = 17,
                    Name = "Özel Alanya Anadolu Hastanesi",
                    Address = "Alanya, Antalya",
                    PhoneNumber = "(0242) 522 62 62",
                    Email = null,
                    WebSite = "https://www.anadoluhastanesi.com",
                    Lat = 36.5483,
                    Lon = 31.9954,
                    DistrictId = 3
                },
                new Hospital
                {
                    Id = 18,
                    Name = "Özel Lara Anadolu Hastanesi",
                    Address = "Muratpaşa, Antalya",
                    PhoneNumber = "(0242) 317 11 11",
                    Email = null,
                    WebSite = "https://www.anadoluhastanesi.com",
                    Lat = 36.8502,
                    Lon = 30.7667,
                    DistrictId = 1
                },
                new Hospital
                {
                    Id = 19,
                    Name = "Özel Memorial Antalya Hastanesi",
                    Address = "Kepez, Antalya",
                    PhoneNumber = "(0242) 314 66 66",
                    Email = null,
                    WebSite = "https://www.memorial.com.tr",
                    Lat = 36.8859,
                    Lon = 30.6604,
                    DistrictId = 2
                },
                new Hospital
                {
                    Id = 20,
                    Name = "Özel Medical Park Antalya Hastane Kompleksi",
                    Address = "Muratpaşa, Antalya",
                    PhoneNumber = "(0242) 314 34 34",
                    Email = null,
                    WebSite = "https://www.medicalpark.com.tr",
                    Lat = 36.8643,
                    Lon = 30.7265,
                    DistrictId = 1
                },
                new Hospital
                {
                    Id = 21,
                    Name = "Özel Opera Yaşam Hastanesi",
                    Address = "Konyaaltı, Antalya",
                    PhoneNumber = "(0242) 229 59 59",
                    Email = null,
                    WebSite = "https://www.operayasam.com",
                    Lat = 36.8701,
                    Lon = 30.6267,
                    DistrictId = 12
                },
                new Hospital
                {
                    Id = 22,
                    Name = "Özel Alanya Yaşam Hastanesi",
                    Address = "Alanya, Antalya",
                    PhoneNumber = "(0242) 513 33 33",
                    Email = null,
                    WebSite = "https://www.yasamhastaneleri.com",
                    Lat = 36.5438,
                    Lon = 32.0021,
                    DistrictId = 3
                },
                new Hospital
                {
                    Id = 23,
                    Name = "Özel Side Anadolu Hastanesi",
                    Address = "Manavgat, Antalya",
                    PhoneNumber = "(0242) 753 11 11",
                    Email = null,
                    WebSite = "https://www.anadoluhastanesi.com",
                    Lat = 36.7669,
                    Lon = 31.3912,
                    DistrictId = 4
                },
                new Hospital
                {
                    Id = 24,
                    Name = "Özel OFM Antalya Hastanesi",
                    Address = "Muratpaşa, Antalya",
                    PhoneNumber = "(0242) 316 66 66",
                    Email = null,
                    WebSite = "https://www.ofmantalya.com",
                    Lat = 36.8847,
                    Lon = 30.7043,
                    DistrictId = 1
                },
                new Hospital
                {
                    Id = 25,
                    Name = "Akdeniz Üniversitesi Hastanesi",
                    Address = "Pınarbaşı, Akdeniz Ünv., 07070 Konyaaltı/Antalya",
                    PhoneNumber = "(0242) 249 60 00",
                    Email = null,
                    WebSite = "https://www.hastane.akdeniz.edu.tr",
                    Lat = 36.8850,
                    Lon = 30.6320,
                    DistrictId = 12
                }
            };

            foreach (var hospital in hospitals)
            {
                var existingHospital = context.Hospitals.FirstOrDefault(h => h.Id == hospital.Id);
                if (existingHospital == null)
                {
                    await context.Hospitals.AddAsync(hospital);
                }
                else
                {
                    existingHospital.Name = hospital.Name;
                    existingHospital.Address = hospital.Address;
                    existingHospital.PhoneNumber = hospital.PhoneNumber;
                    existingHospital.WebSite = hospital.WebSite;
                    existingHospital.Lat = hospital.Lat;
                    existingHospital.Lon = hospital.Lon;
                    existingHospital.DistrictId = hospital.DistrictId;
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
