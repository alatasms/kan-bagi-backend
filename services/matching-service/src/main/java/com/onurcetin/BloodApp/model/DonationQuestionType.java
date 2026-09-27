package com.onurcetin.BloodApp.model;

import com.fasterxml.jackson.annotation.JsonValue;

public enum DonationQuestionType {
    ONAM_FORMU_OKUNDUMU(1, "Kan Bağışçısı Bilgilendirme Onam Formu'nu okuyup anladınız mı?"),
    SAGLIKLI_MI(2, "Kendinizi sağlıklı ve iyi hissediyor musunuz?"),
    TEHLIKELI_HOBI_VAR_MI(3, "Tehlikeli bir hobiniz var mıdır?"),
    DAHA_ONCE_GERI_CEVRILDINIZ_MI(4, "Daha önce kan bağışı için gittiğiniz kan bağış merkezinden herhangi bir nedenle geri çevrildiniz mi?"),
    ILAC_KULLANIYOR_MUSUNUZ(5, "Prostat büyümesi, sivilce tedavisi, sedef hastalığı, kellik için herhangi bir ilaç alıyor musunuz?"),
    ENFEKSIYON_ILAC_ALIMI(6, "Herhangi bir enfeksiyon hastalığı için, son bir hafta içinde ilaç (antibiyotik, ateş düşürücü, vb.) aldınız mı?"),
    AGRI_KESECI_ALIMI(7, "Son 5 gün içinde aspirin, ağrı kesici veya romatizma ilacı aldınız mı?"),
    ALERJI_TEDAVISI(8, "Alerjik reaksiyon geçirdiniz mi, buna yönelik tedavi aldınız mı?"),
    DIGER_ILAC_KULLANIMI(9, "Yukarıda belirtilenler dışında kullandığınız herhangi bir ilaç var mı?"),
    DIS_TEDAVISI(10, "Son 12 ay içinde diş tedavisi oldunuz mu?"),
    ISHAL(11, "Son 1 hafta içinde ishal (diyare) oldunuz mu?"),
    ASI_OLUNDU_MU(12, "Son 1 ay içinde herhangi bir aşı oldunuz mu?"),
    KRONIK_HASTALIK(13, "Kronik (müzmin, süreğen) bir hastalığınız var mı?"),
    PARA_KARSILIGI_ILISKI(14, "Para veya uyuşturucu karşılığında cinsel ilişkiniz oldu mu?"),
    FRENGI_GONORE(15, "Frengi (Sifilis) veya bel soğukluğu (Gonore) nedeniyle tedavi oldunuz mu?"),
    AIDS_HASTALIGI(16, "AIDS hastalığınız var mı, kendinizde böyle bir hastalık olduğuna dair şüpheleriniz var mı?"),
    AIDS_HASTASI_ILE_ILISKI(17, "AIDS hastası olduğunu bildiğiniz biriyle cinsel ilişkiniz oldu mu?"),
    KAN_ALAN_KISI_ILE_ILISKI(18, "Kan ve kan ürünü alan, diyalize giren veya hemofili hastası olan biri ile cinsel ilişkiniz oldu mu?"),
    UYUSTURUCU_KULLANIMI(19, "Hiç uyuşturucu kullandınız mı?"),
    HORMON_ILAC_KULLANIMI(20, "İnsülin, büyüme hormonu, immünglobulin (gamaglobulin), tamoksifen kullandınız mı?"),
    AMELIYAT_ENDOSKOPI(21, "Son 12 ay içinde ameliyat veya endoskopik muayene oldunuz mu?"),
    KALP_AKCIGER_HASTALIK(22, "Kalp-damar, akciğer, mide-barsak, böbrek hastalığınız var mı?"),
    NOBET_EPILEPSI_FELC(23, "Bugüne kadar hiç nöbet, sara (Epilepsi) krizi veya felç geçirdiniz mi?"),
    KANSER_TEDAVISI(24, "Bugüne kadar hiç kanser tanısı aldınız mı, kanser tedavisi gördünüz mü?"),
    SEKER_ROMATIZMA(25, "Şeker hastalığınız ya da yaygın romatizmal bir hastalığınız var mı?"),
    KAN_HASTALIGI(26, "Kanamalı bir hastalık veya kan hastalığınız var mı?"),
    SITMA_TUBERKULOZ(
        27, "Sıtma (malarya), verem (tüberküloz), malta humması (brucella), kemik iltihabı (osteomyelit) veya karahumma (kala-azar) geçirdiniz mi?"
    ),
    HEPATIT_TASIYICILIK(28, "Hepatit (sarılık hastalığı) geçirdiniz mi, taşıyıcısı mısınız?"),
    HEPATITLI_ILISKI(29, "Hepatit (sarılık hastalığı) olan biriyle aynı evde yaşıyor musunuz veya cinsel ilişkiniz oldu mu?"),
    TOKSOPLAZMA(30, "Toksoplazma geçirdiniz mi?"),
    BELIRLI_ULKELERDE_BULUNDUNUZ_MU(31, "Kamerun, Orta Afrika, Çad, Kongo, Ekvatoryal Gine, Gabon, Nijer ya da Nijerya'da hiç bulundunuz mu?"),
    INGILTERE_KUZAY_IRLANDA(32, "1980-1996 yılları arasında İngiltere, Kuzey İrlanda, Galler ya da İskoçya'da bulundunuz mu?"),
    DIGER_ULKELER(33, "Son 3 yıl içinde yukarıdaki ülkeler dışında başka ülkelerde bulundunuz mu?"),
    DELI_DANA_HASTALIGI(34, "Ailenizde Deli Dana Hastalığı (Creutzfeldt-Jakob) olan birisi oldu mu?"),
    BEYIN_ZARI_KORNEA_NAKLI(35, "Size Dura mater (beyin zarı) veya kornea nakli yapıldı mı?"),
    KAN_ORGAN_NAKLI(36, "Son 12 ay içinde size kan, doku veya organ nakli yapıldı mı?"),
    BASKASININ_KANI_ILE_TEMAS(37, "Son 12 ay içinde bir başkasının kanı ile temasınız oldu mu?"),
    DOVME_ESTETIK_MUDAHALE(
        38, "Son 12 ay içinde dövme, hacamat, akupunktur, botoks, takı için cilt deldirme, saç ekimi veya estetik müdahaleler yaptırdınız mı?"
    ),
    KUDUZ_ASISI(39, "Son 12 ay içinde hayvan ısırığı nedeni ile kuduz aşısı oldunuz mu?"),
    TUTUKLULUK(40, "Son 12 ay içinde üç günden fazla tutuklu kaldınız mı veya üç günden fazla tutuklu kalan birisiyle cinsel ilişkiniz oldu mu?"),
    KAN_BAGISI_ERKEK(41, "Son 3 ay içinde kan bağışı yaptınız mı? (erkekler için)"),
    ERKEK_ERKEGE_ILISKI_KADIN_HAMILELIK(42, "Erkek erkeğe cinsel ilişki / Kadınlar için hamilelik durumu");

    private final int id;
    private final String question;

    private DonationQuestionType(int id, String question) {
        this.id = id;
        this.question = question;
    }

    public int getId() {
        return this.id;
    }

    @JsonValue
    public String getQuestion() {
        return this.question;
    }
}
