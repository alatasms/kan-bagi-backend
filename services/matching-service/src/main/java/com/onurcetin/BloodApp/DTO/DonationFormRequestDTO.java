package com.onurcetin.BloodApp.DTO;

import com.fasterxml.jackson.annotation.JsonProperty;
import io.swagger.v3.oas.annotations.media.Schema;
import lombok.Generated;

public class DonationFormRequestDTO {
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("ONAM_FORMU_OKUNDUMU")
    private Boolean onamFormuOkundumu;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("SAGLIKLI_MI")
    private Boolean saglikliMi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("TEHLIKELI_HOBI_VAR_MI")
    private Boolean tehlikeliHobiVarMi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("DAHA_ONCE_GERI_CEVRILDINIZ_MI")
    private Boolean dahaOnceGeriCevrildinizMi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("ILAC_KULLANIYOR_MUSUNUZ")
    private Boolean ilacKullaniyorMusunuz;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("ENFEKSIYON_ILAC_ALIMI")
    private Boolean enfeksiyonIlacAlimi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("AGRI_KESECI_ALIMI")
    private Boolean agriKesiciAlimi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("ALERJI_TEDAVISI")
    private Boolean alerjiTedavisi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("DIGER_ILAC_KULLANIMI")
    private Boolean digerIlacKullanimi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("DIS_TEDAVISI")
    private Boolean disTedavisi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("ISHAL")
    private Boolean ishal;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("ASI_OLUNDU_MU")
    private Boolean asiOlunduMu;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("KRONIK_HASTALIK")
    private Boolean kronikHastalik;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("PARA_KARSILIGI_ILISKI")
    private Boolean paraKarsiligiIliski;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("FRENGI_GONORE")
    private Boolean frengiGonore;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("AIDS_HASTALIGI")
    private Boolean aidsHastaligi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("AIDS_HASTASI_ILE_ILISKI")
    private Boolean aidsHastasiIleIliski;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("KAN_ALAN_KISI_ILE_ILISKI")
    private Boolean kanAlanKisiIleIliski;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("UYUSTURUCU_KULLANIMI")
    private Boolean uyusturucuKullanimi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("HORMON_ILAC_KULLANIMI")
    private Boolean hormonIlacKullanimi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("AMELIYAT_ENDOSKOPI")
    private Boolean ameliyatEndoskopi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("KALP_AKCIGER_HASTALIK")
    private Boolean kalpAkcigerHastalik;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("NOBET_EPILEPSI_FELC")
    private Boolean nobetEpilepsiFelc;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("KANSER_TEDAVISI")
    private Boolean kanserTedavisi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("SEKER_ROMATIZMA")
    private Boolean sekerRomatizma;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("KAN_HASTALIGI")
    private Boolean kanHastaligi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("SITMA_TUBERKULOZ")
    private Boolean sıtmaTuberkuloz;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("HEPATIT_TASIYICILIK")
    private Boolean hepatitTasiyicilik;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("HEPATITLI_ILISKI")
    private Boolean hepatitliIliski;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("TOKSOPLAZMA")
    private Boolean toksoplazma;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("BELIRLI_ULKELERDE_BULUNDUNUZ_MU")
    private Boolean belirliUlkelerdeBulundunuzMu;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("INGILTERE_KUZAY_IRLANDA")
    private Boolean ingiltereKuzayIrlanda;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("DIGER_ULKELER")
    private Boolean digerUlkeler;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("DELI_DANA_HASTALIGI")
    private Boolean deliDanaHastaligi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("BEYIN_ZARI_KORNEA_NAKLI")
    private Boolean beyinZariKorneaNakli;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("KAN_ORGAN_NAKLI")
    private Boolean kanOrganNakli;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("BASKASININ_KANI_ILE_TEMAS")
    private Boolean baskasininKaniIleTemas;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("DOVME_ESTETIK_MUDAHALE")
    private Boolean dovmeEstetikMudahale;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("KUDUZ_ASISI")
    private Boolean kuduzAsisi;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("TUTUKLULUK")
    private Boolean tutukluluk;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("KAN_BAGISI_ERKEK")
    private Boolean kanBagisiErkek;
    @Schema(
        defaultValue = "false"
    )
    @JsonProperty("ERKEK_ERKEGE_ILISKI_KADIN_HAMILELIK")
    private Boolean erkekErkegeIliskiKadinHamilelik;

    @Generated
    public Boolean getOnamFormuOkundumu() {
        return this.onamFormuOkundumu;
    }

    @Generated
    public Boolean getSaglikliMi() {
        return this.saglikliMi;
    }

    @Generated
    public Boolean getTehlikeliHobiVarMi() {
        return this.tehlikeliHobiVarMi;
    }

    @Generated
    public Boolean getDahaOnceGeriCevrildinizMi() {
        return this.dahaOnceGeriCevrildinizMi;
    }

    @Generated
    public Boolean getIlacKullaniyorMusunuz() {
        return this.ilacKullaniyorMusunuz;
    }

    @Generated
    public Boolean getEnfeksiyonIlacAlimi() {
        return this.enfeksiyonIlacAlimi;
    }

    @Generated
    public Boolean getAgriKesiciAlimi() {
        return this.agriKesiciAlimi;
    }

    @Generated
    public Boolean getAlerjiTedavisi() {
        return this.alerjiTedavisi;
    }

    @Generated
    public Boolean getDigerIlacKullanimi() {
        return this.digerIlacKullanimi;
    }

    @Generated
    public Boolean getDisTedavisi() {
        return this.disTedavisi;
    }

    @Generated
    public Boolean getIshal() {
        return this.ishal;
    }

    @Generated
    public Boolean getAsiOlunduMu() {
        return this.asiOlunduMu;
    }

    @Generated
    public Boolean getKronikHastalik() {
        return this.kronikHastalik;
    }

    @Generated
    public Boolean getParaKarsiligiIliski() {
        return this.paraKarsiligiIliski;
    }

    @Generated
    public Boolean getFrengiGonore() {
        return this.frengiGonore;
    }

    @Generated
    public Boolean getAidsHastaligi() {
        return this.aidsHastaligi;
    }

    @Generated
    public Boolean getAidsHastasiIleIliski() {
        return this.aidsHastasiIleIliski;
    }

    @Generated
    public Boolean getKanAlanKisiIleIliski() {
        return this.kanAlanKisiIleIliski;
    }

    @Generated
    public Boolean getUyusturucuKullanimi() {
        return this.uyusturucuKullanimi;
    }

    @Generated
    public Boolean getHormonIlacKullanimi() {
        return this.hormonIlacKullanimi;
    }

    @Generated
    public Boolean getAmeliyatEndoskopi() {
        return this.ameliyatEndoskopi;
    }

    @Generated
    public Boolean getKalpAkcigerHastalik() {
        return this.kalpAkcigerHastalik;
    }

    @Generated
    public Boolean getNobetEpilepsiFelc() {
        return this.nobetEpilepsiFelc;
    }

    @Generated
    public Boolean getKanserTedavisi() {
        return this.kanserTedavisi;
    }

    @Generated
    public Boolean getSekerRomatizma() {
        return this.sekerRomatizma;
    }

    @Generated
    public Boolean getKanHastaligi() {
        return this.kanHastaligi;
    }

    @Generated
    public Boolean getSıtmaTuberkuloz() {
        return this.sıtmaTuberkuloz;
    }

    @Generated
    public Boolean getHepatitTasiyicilik() {
        return this.hepatitTasiyicilik;
    }

    @Generated
    public Boolean getHepatitliIliski() {
        return this.hepatitliIliski;
    }

    @Generated
    public Boolean getToksoplazma() {
        return this.toksoplazma;
    }

    @Generated
    public Boolean getBelirliUlkelerdeBulundunuzMu() {
        return this.belirliUlkelerdeBulundunuzMu;
    }

    @Generated
    public Boolean getIngiltereKuzayIrlanda() {
        return this.ingiltereKuzayIrlanda;
    }

    @Generated
    public Boolean getDigerUlkeler() {
        return this.digerUlkeler;
    }

    @Generated
    public Boolean getDeliDanaHastaligi() {
        return this.deliDanaHastaligi;
    }

    @Generated
    public Boolean getBeyinZariKorneaNakli() {
        return this.beyinZariKorneaNakli;
    }

    @Generated
    public Boolean getKanOrganNakli() {
        return this.kanOrganNakli;
    }

    @Generated
    public Boolean getBaskasininKaniIleTemas() {
        return this.baskasininKaniIleTemas;
    }

    @Generated
    public Boolean getDovmeEstetikMudahale() {
        return this.dovmeEstetikMudahale;
    }

    @Generated
    public Boolean getKuduzAsisi() {
        return this.kuduzAsisi;
    }

    @Generated
    public Boolean getTutukluluk() {
        return this.tutukluluk;
    }

    @Generated
    public Boolean getKanBagisiErkek() {
        return this.kanBagisiErkek;
    }

    @Generated
    public Boolean getErkekErkegeIliskiKadinHamilelik() {
        return this.erkekErkegeIliskiKadinHamilelik;
    }

    @JsonProperty("ONAM_FORMU_OKUNDUMU")
    @Generated
    public void setOnamFormuOkundumu(final Boolean onamFormuOkundumu) {
        this.onamFormuOkundumu = onamFormuOkundumu;
    }

    @JsonProperty("SAGLIKLI_MI")
    @Generated
    public void setSaglikliMi(final Boolean saglikliMi) {
        this.saglikliMi = saglikliMi;
    }

    @JsonProperty("TEHLIKELI_HOBI_VAR_MI")
    @Generated
    public void setTehlikeliHobiVarMi(final Boolean tehlikeliHobiVarMi) {
        this.tehlikeliHobiVarMi = tehlikeliHobiVarMi;
    }

    @JsonProperty("DAHA_ONCE_GERI_CEVRILDINIZ_MI")
    @Generated
    public void setDahaOnceGeriCevrildinizMi(final Boolean dahaOnceGeriCevrildinizMi) {
        this.dahaOnceGeriCevrildinizMi = dahaOnceGeriCevrildinizMi;
    }

    @JsonProperty("ILAC_KULLANIYOR_MUSUNUZ")
    @Generated
    public void setIlacKullaniyorMusunuz(final Boolean ilacKullaniyorMusunuz) {
        this.ilacKullaniyorMusunuz = ilacKullaniyorMusunuz;
    }

    @JsonProperty("ENFEKSIYON_ILAC_ALIMI")
    @Generated
    public void setEnfeksiyonIlacAlimi(final Boolean enfeksiyonIlacAlimi) {
        this.enfeksiyonIlacAlimi = enfeksiyonIlacAlimi;
    }

    @JsonProperty("AGRI_KESECI_ALIMI")
    @Generated
    public void setAgriKesiciAlimi(final Boolean agriKesiciAlimi) {
        this.agriKesiciAlimi = agriKesiciAlimi;
    }

    @JsonProperty("ALERJI_TEDAVISI")
    @Generated
    public void setAlerjiTedavisi(final Boolean alerjiTedavisi) {
        this.alerjiTedavisi = alerjiTedavisi;
    }

    @JsonProperty("DIGER_ILAC_KULLANIMI")
    @Generated
    public void setDigerIlacKullanimi(final Boolean digerIlacKullanimi) {
        this.digerIlacKullanimi = digerIlacKullanimi;
    }

    @JsonProperty("DIS_TEDAVISI")
    @Generated
    public void setDisTedavisi(final Boolean disTedavisi) {
        this.disTedavisi = disTedavisi;
    }

    @JsonProperty("ISHAL")
    @Generated
    public void setIshal(final Boolean ishal) {
        this.ishal = ishal;
    }

    @JsonProperty("ASI_OLUNDU_MU")
    @Generated
    public void setAsiOlunduMu(final Boolean asiOlunduMu) {
        this.asiOlunduMu = asiOlunduMu;
    }

    @JsonProperty("KRONIK_HASTALIK")
    @Generated
    public void setKronikHastalik(final Boolean kronikHastalik) {
        this.kronikHastalik = kronikHastalik;
    }

    @JsonProperty("PARA_KARSILIGI_ILISKI")
    @Generated
    public void setParaKarsiligiIliski(final Boolean paraKarsiligiIliski) {
        this.paraKarsiligiIliski = paraKarsiligiIliski;
    }

    @JsonProperty("FRENGI_GONORE")
    @Generated
    public void setFrengiGonore(final Boolean frengiGonore) {
        this.frengiGonore = frengiGonore;
    }

    @JsonProperty("AIDS_HASTALIGI")
    @Generated
    public void setAidsHastaligi(final Boolean aidsHastaligi) {
        this.aidsHastaligi = aidsHastaligi;
    }

    @JsonProperty("AIDS_HASTASI_ILE_ILISKI")
    @Generated
    public void setAidsHastasiIleIliski(final Boolean aidsHastasiIleIliski) {
        this.aidsHastasiIleIliski = aidsHastasiIleIliski;
    }

    @JsonProperty("KAN_ALAN_KISI_ILE_ILISKI")
    @Generated
    public void setKanAlanKisiIleIliski(final Boolean kanAlanKisiIleIliski) {
        this.kanAlanKisiIleIliski = kanAlanKisiIleIliski;
    }

    @JsonProperty("UYUSTURUCU_KULLANIMI")
    @Generated
    public void setUyusturucuKullanimi(final Boolean uyusturucuKullanimi) {
        this.uyusturucuKullanimi = uyusturucuKullanimi;
    }

    @JsonProperty("HORMON_ILAC_KULLANIMI")
    @Generated
    public void setHormonIlacKullanimi(final Boolean hormonIlacKullanimi) {
        this.hormonIlacKullanimi = hormonIlacKullanimi;
    }

    @JsonProperty("AMELIYAT_ENDOSKOPI")
    @Generated
    public void setAmeliyatEndoskopi(final Boolean ameliyatEndoskopi) {
        this.ameliyatEndoskopi = ameliyatEndoskopi;
    }

    @JsonProperty("KALP_AKCIGER_HASTALIK")
    @Generated
    public void setKalpAkcigerHastalik(final Boolean kalpAkcigerHastalik) {
        this.kalpAkcigerHastalik = kalpAkcigerHastalik;
    }

    @JsonProperty("NOBET_EPILEPSI_FELC")
    @Generated
    public void setNobetEpilepsiFelc(final Boolean nobetEpilepsiFelc) {
        this.nobetEpilepsiFelc = nobetEpilepsiFelc;
    }

    @JsonProperty("KANSER_TEDAVISI")
    @Generated
    public void setKanserTedavisi(final Boolean kanserTedavisi) {
        this.kanserTedavisi = kanserTedavisi;
    }

    @JsonProperty("SEKER_ROMATIZMA")
    @Generated
    public void setSekerRomatizma(final Boolean sekerRomatizma) {
        this.sekerRomatizma = sekerRomatizma;
    }

    @JsonProperty("KAN_HASTALIGI")
    @Generated
    public void setKanHastaligi(final Boolean kanHastaligi) {
        this.kanHastaligi = kanHastaligi;
    }

    @JsonProperty("SITMA_TUBERKULOZ")
    @Generated
    public void setSıtmaTuberkuloz(final Boolean sıtmaTuberkuloz) {
        this.sıtmaTuberkuloz = sıtmaTuberkuloz;
    }

    @JsonProperty("HEPATIT_TASIYICILIK")
    @Generated
    public void setHepatitTasiyicilik(final Boolean hepatitTasiyicilik) {
        this.hepatitTasiyicilik = hepatitTasiyicilik;
    }

    @JsonProperty("HEPATITLI_ILISKI")
    @Generated
    public void setHepatitliIliski(final Boolean hepatitliIliski) {
        this.hepatitliIliski = hepatitliIliski;
    }

    @JsonProperty("TOKSOPLAZMA")
    @Generated
    public void setToksoplazma(final Boolean toksoplazma) {
        this.toksoplazma = toksoplazma;
    }

    @JsonProperty("BELIRLI_ULKELERDE_BULUNDUNUZ_MU")
    @Generated
    public void setBelirliUlkelerdeBulundunuzMu(final Boolean belirliUlkelerdeBulundunuzMu) {
        this.belirliUlkelerdeBulundunuzMu = belirliUlkelerdeBulundunuzMu;
    }

    @JsonProperty("INGILTERE_KUZAY_IRLANDA")
    @Generated
    public void setIngiltereKuzayIrlanda(final Boolean ingiltereKuzayIrlanda) {
        this.ingiltereKuzayIrlanda = ingiltereKuzayIrlanda;
    }

    @JsonProperty("DIGER_ULKELER")
    @Generated
    public void setDigerUlkeler(final Boolean digerUlkeler) {
        this.digerUlkeler = digerUlkeler;
    }

    @JsonProperty("DELI_DANA_HASTALIGI")
    @Generated
    public void setDeliDanaHastaligi(final Boolean deliDanaHastaligi) {
        this.deliDanaHastaligi = deliDanaHastaligi;
    }

    @JsonProperty("BEYIN_ZARI_KORNEA_NAKLI")
    @Generated
    public void setBeyinZariKorneaNakli(final Boolean beyinZariKorneaNakli) {
        this.beyinZariKorneaNakli = beyinZariKorneaNakli;
    }

    @JsonProperty("KAN_ORGAN_NAKLI")
    @Generated
    public void setKanOrganNakli(final Boolean kanOrganNakli) {
        this.kanOrganNakli = kanOrganNakli;
    }

    @JsonProperty("BASKASININ_KANI_ILE_TEMAS")
    @Generated
    public void setBaskasininKaniIleTemas(final Boolean baskasininKaniIleTemas) {
        this.baskasininKaniIleTemas = baskasininKaniIleTemas;
    }

    @JsonProperty("DOVME_ESTETIK_MUDAHALE")
    @Generated
    public void setDovmeEstetikMudahale(final Boolean dovmeEstetikMudahale) {
        this.dovmeEstetikMudahale = dovmeEstetikMudahale;
    }

    @JsonProperty("KUDUZ_ASISI")
    @Generated
    public void setKuduzAsisi(final Boolean kuduzAsisi) {
        this.kuduzAsisi = kuduzAsisi;
    }

    @JsonProperty("TUTUKLULUK")
    @Generated
    public void setTutukluluk(final Boolean tutukluluk) {
        this.tutukluluk = tutukluluk;
    }

    @JsonProperty("KAN_BAGISI_ERKEK")
    @Generated
    public void setKanBagisiErkek(final Boolean kanBagisiErkek) {
        this.kanBagisiErkek = kanBagisiErkek;
    }

    @JsonProperty("ERKEK_ERKEGE_ILISKI_KADIN_HAMILELIK")
    @Generated
    public void setErkekErkegeIliskiKadinHamilelik(final Boolean erkekErkegeIliskiKadinHamilelik) {
        this.erkekErkegeIliskiKadinHamilelik = erkekErkegeIliskiKadinHamilelik;
    }

    @Generated
    @Override
    public boolean equals(final Object o) {
        if (o == this) {
            return true;
        } else if (!(o instanceof DonationFormRequestDTO other)) {
            return false;
        } else if (!other.canEqual(this)) {
            return false;
        } else {
            Object this$onamFormuOkundumu = this.getOnamFormuOkundumu();
            Object other$onamFormuOkundumu = other.getOnamFormuOkundumu();
            if (this$onamFormuOkundumu == null ? other$onamFormuOkundumu == null : this$onamFormuOkundumu.equals(other$onamFormuOkundumu)) {
                Object this$saglikliMi = this.getSaglikliMi();
                Object other$saglikliMi = other.getSaglikliMi();
                if (this$saglikliMi == null ? other$saglikliMi == null : this$saglikliMi.equals(other$saglikliMi)) {
                    Object this$tehlikeliHobiVarMi = this.getTehlikeliHobiVarMi();
                    Object other$tehlikeliHobiVarMi = other.getTehlikeliHobiVarMi();
                    if (this$tehlikeliHobiVarMi == null ? other$tehlikeliHobiVarMi == null : this$tehlikeliHobiVarMi.equals(other$tehlikeliHobiVarMi)) {
                        Object this$dahaOnceGeriCevrildinizMi = this.getDahaOnceGeriCevrildinizMi();
                        Object other$dahaOnceGeriCevrildinizMi = other.getDahaOnceGeriCevrildinizMi();
                        if (this$dahaOnceGeriCevrildinizMi == null
                            ? other$dahaOnceGeriCevrildinizMi == null
                            : this$dahaOnceGeriCevrildinizMi.equals(other$dahaOnceGeriCevrildinizMi)) {
                            Object this$ilacKullaniyorMusunuz = this.getIlacKullaniyorMusunuz();
                            Object other$ilacKullaniyorMusunuz = other.getIlacKullaniyorMusunuz();
                            if (this$ilacKullaniyorMusunuz == null
                                ? other$ilacKullaniyorMusunuz == null
                                : this$ilacKullaniyorMusunuz.equals(other$ilacKullaniyorMusunuz)) {
                                Object this$enfeksiyonIlacAlimi = this.getEnfeksiyonIlacAlimi();
                                Object other$enfeksiyonIlacAlimi = other.getEnfeksiyonIlacAlimi();
                                if (this$enfeksiyonIlacAlimi == null
                                    ? other$enfeksiyonIlacAlimi == null
                                    : this$enfeksiyonIlacAlimi.equals(other$enfeksiyonIlacAlimi)) {
                                    Object this$agriKesiciAlimi = this.getAgriKesiciAlimi();
                                    Object other$agriKesiciAlimi = other.getAgriKesiciAlimi();
                                    if (this$agriKesiciAlimi == null ? other$agriKesiciAlimi == null : this$agriKesiciAlimi.equals(other$agriKesiciAlimi)) {
                                        Object this$alerjiTedavisi = this.getAlerjiTedavisi();
                                        Object other$alerjiTedavisi = other.getAlerjiTedavisi();
                                        if (this$alerjiTedavisi == null ? other$alerjiTedavisi == null : this$alerjiTedavisi.equals(other$alerjiTedavisi)) {
                                            Object this$digerIlacKullanimi = this.getDigerIlacKullanimi();
                                            Object other$digerIlacKullanimi = other.getDigerIlacKullanimi();
                                            if (this$digerIlacKullanimi == null
                                                ? other$digerIlacKullanimi == null
                                                : this$digerIlacKullanimi.equals(other$digerIlacKullanimi)) {
                                                Object this$disTedavisi = this.getDisTedavisi();
                                                Object other$disTedavisi = other.getDisTedavisi();
                                                if (this$disTedavisi == null ? other$disTedavisi == null : this$disTedavisi.equals(other$disTedavisi)) {
                                                    Object this$ishal = this.getIshal();
                                                    Object other$ishal = other.getIshal();
                                                    if (this$ishal == null ? other$ishal == null : this$ishal.equals(other$ishal)) {
                                                        Object this$asiOlunduMu = this.getAsiOlunduMu();
                                                        Object other$asiOlunduMu = other.getAsiOlunduMu();
                                                        if (this$asiOlunduMu == null ? other$asiOlunduMu == null : this$asiOlunduMu.equals(other$asiOlunduMu)) {
                                                            Object this$kronikHastalik = this.getKronikHastalik();
                                                            Object other$kronikHastalik = other.getKronikHastalik();
                                                            if (this$kronikHastalik == null
                                                                ? other$kronikHastalik == null
                                                                : this$kronikHastalik.equals(other$kronikHastalik)) {
                                                                Object this$paraKarsiligiIliski = this.getParaKarsiligiIliski();
                                                                Object other$paraKarsiligiIliski = other.getParaKarsiligiIliski();
                                                                if (this$paraKarsiligiIliski == null
                                                                    ? other$paraKarsiligiIliski == null
                                                                    : this$paraKarsiligiIliski.equals(other$paraKarsiligiIliski)) {
                                                                    Object this$frengiGonore = this.getFrengiGonore();
                                                                    Object other$frengiGonore = other.getFrengiGonore();
                                                                    if (this$frengiGonore == null
                                                                        ? other$frengiGonore == null
                                                                        : this$frengiGonore.equals(other$frengiGonore)) {
                                                                        Object this$aidsHastaligi = this.getAidsHastaligi();
                                                                        Object other$aidsHastaligi = other.getAidsHastaligi();
                                                                        if (this$aidsHastaligi == null
                                                                            ? other$aidsHastaligi == null
                                                                            : this$aidsHastaligi.equals(other$aidsHastaligi)) {
                                                                            Object this$aidsHastasiIleIliski = this.getAidsHastasiIleIliski();
                                                                            Object other$aidsHastasiIleIliski = other.getAidsHastasiIleIliski();
                                                                            if (this$aidsHastasiIleIliski == null
                                                                                ? other$aidsHastasiIleIliski == null
                                                                                : this$aidsHastasiIleIliski.equals(other$aidsHastasiIleIliski)) {
                                                                                Object this$kanAlanKisiIleIliski = this.getKanAlanKisiIleIliski();
                                                                                Object other$kanAlanKisiIleIliski = other.getKanAlanKisiIleIliski();
                                                                                if (this$kanAlanKisiIleIliski == null
                                                                                    ? other$kanAlanKisiIleIliski == null
                                                                                    : this$kanAlanKisiIleIliski.equals(other$kanAlanKisiIleIliski)) {
                                                                                    Object this$uyusturucuKullanimi = this.getUyusturucuKullanimi();
                                                                                    Object other$uyusturucuKullanimi = other.getUyusturucuKullanimi();
                                                                                    if (this$uyusturucuKullanimi == null
                                                                                        ? other$uyusturucuKullanimi == null
                                                                                        : this$uyusturucuKullanimi.equals(other$uyusturucuKullanimi)) {
                                                                                        Object this$hormonIlacKullanimi = this.getHormonIlacKullanimi();
                                                                                        Object other$hormonIlacKullanimi = other.getHormonIlacKullanimi();
                                                                                        if (this$hormonIlacKullanimi == null
                                                                                            ? other$hormonIlacKullanimi == null
                                                                                            : this$hormonIlacKullanimi.equals(other$hormonIlacKullanimi)) {
                                                                                            Object this$ameliyatEndoskopi = this.getAmeliyatEndoskopi();
                                                                                            Object other$ameliyatEndoskopi = other.getAmeliyatEndoskopi();
                                                                                            if (this$ameliyatEndoskopi == null
                                                                                                ? other$ameliyatEndoskopi == null
                                                                                                : this$ameliyatEndoskopi.equals(other$ameliyatEndoskopi)) {
                                                                                                Object this$kalpAkcigerHastalik = this.getKalpAkcigerHastalik();
                                                                                                Object other$kalpAkcigerHastalik = other.getKalpAkcigerHastalik();
                                                                                                if (this$kalpAkcigerHastalik == null
                                                                                                    ? other$kalpAkcigerHastalik == null
                                                                                                    : this$kalpAkcigerHastalik.equals(other$kalpAkcigerHastalik)
                                                                                                    )
                                                                                                 {
                                                                                                    Object this$nobetEpilepsiFelc = this.getNobetEpilepsiFelc();
                                                                                                    Object other$nobetEpilepsiFelc = other.getNobetEpilepsiFelc();
                                                                                                    if (this$nobetEpilepsiFelc == null
                                                                                                        ? other$nobetEpilepsiFelc == null
                                                                                                        : this$nobetEpilepsiFelc.equals(other$nobetEpilepsiFelc)
                                                                                                        )
                                                                                                     {
                                                                                                        Object this$kanserTedavisi = this.getKanserTedavisi();
                                                                                                        Object other$kanserTedavisi = other.getKanserTedavisi();
                                                                                                        if (this$kanserTedavisi == null
                                                                                                            ? other$kanserTedavisi == null
                                                                                                            : this$kanserTedavisi.equals(other$kanserTedavisi)) {
                                                                                                            Object this$sekerRomatizma = this.getSekerRomatizma();
                                                                                                            Object other$sekerRomatizma = other.getSekerRomatizma();
                                                                                                            if (this$sekerRomatizma == null
                                                                                                                ? other$sekerRomatizma == null
                                                                                                                : this$sekerRomatizma.equals(
                                                                                                                    other$sekerRomatizma
                                                                                                                )) {
                                                                                                                Object this$kanHastaligi = this.getKanHastaligi();
                                                                                                                Object other$kanHastaligi = other.getKanHastaligi();
                                                                                                                if (this$kanHastaligi == null
                                                                                                                    ? other$kanHastaligi == null
                                                                                                                    : this$kanHastaligi.equals(
                                                                                                                        other$kanHastaligi
                                                                                                                    )) {
                                                                                                                    Object this$sıtmaTuberkuloz = this.getSıtmaTuberkuloz();
                                                                                                                    Object other$sıtmaTuberkuloz = other.getSıtmaTuberkuloz();
                                                                                                                    if (this$sıtmaTuberkuloz == null
                                                                                                                        ? other$sıtmaTuberkuloz == null
                                                                                                                        : this$sıtmaTuberkuloz.equals(
                                                                                                                            other$sıtmaTuberkuloz
                                                                                                                        )) {
                                                                                                                        Object this$hepatitTasiyicilik = this.getHepatitTasiyicilik();
                                                                                                                        Object other$hepatitTasiyicilik = other.getHepatitTasiyicilik();
                                                                                                                        if (this$hepatitTasiyicilik == null
                                                                                                                            ? other$hepatitTasiyicilik == null
                                                                                                                            : this$hepatitTasiyicilik.equals(
                                                                                                                                other$hepatitTasiyicilik
                                                                                                                            )) {
                                                                                                                            Object this$hepatitliIliski = this.getHepatitliIliski();
                                                                                                                            Object other$hepatitliIliski = other.getHepatitliIliski();
                                                                                                                            if (this$hepatitliIliski == null
                                                                                                                                ? other$hepatitliIliski == null
                                                                                                                                : this$hepatitliIliski.equals(
                                                                                                                                    other$hepatitliIliski
                                                                                                                                )) {
                                                                                                                                Object this$toksoplazma = this.getToksoplazma();
                                                                                                                                Object other$toksoplazma = other.getToksoplazma();
                                                                                                                                if (this$toksoplazma == null
                                                                                                                                    ? other$toksoplazma == null
                                                                                                                                    : this$toksoplazma.equals(
                                                                                                                                        other$toksoplazma
                                                                                                                                    )) {
                                                                                                                                    Object this$belirliUlkelerdeBulundunuzMu = this.getBelirliUlkelerdeBulundunuzMu();
                                                                                                                                    Object other$belirliUlkelerdeBulundunuzMu = other.getBelirliUlkelerdeBulundunuzMu();
                                                                                                                                    if (this$belirliUlkelerdeBulundunuzMu
                                                                                                                                            == null
                                                                                                                                        ? other$belirliUlkelerdeBulundunuzMu
                                                                                                                                            == null
                                                                                                                                        : this$belirliUlkelerdeBulundunuzMu.equals(
                                                                                                                                            other$belirliUlkelerdeBulundunuzMu
                                                                                                                                        )) {
                                                                                                                                        Object this$ingiltereKuzayIrlanda = this.getIngiltereKuzayIrlanda();
                                                                                                                                        Object other$ingiltereKuzayIrlanda = other.getIngiltereKuzayIrlanda();
                                                                                                                                        if (this$ingiltereKuzayIrlanda
                                                                                                                                                == null
                                                                                                                                            ? other$ingiltereKuzayIrlanda
                                                                                                                                                == null
                                                                                                                                            : this$ingiltereKuzayIrlanda.equals(
                                                                                                                                                other$ingiltereKuzayIrlanda
                                                                                                                                            )) {
                                                                                                                                            Object this$digerUlkeler = this.getDigerUlkeler();
                                                                                                                                            Object other$digerUlkeler = other.getDigerUlkeler();
                                                                                                                                            if (this$digerUlkeler
                                                                                                                                                    == null
                                                                                                                                                ? other$digerUlkeler
                                                                                                                                                    == null
                                                                                                                                                : this$digerUlkeler.equals(
                                                                                                                                                    other$digerUlkeler
                                                                                                                                                )) {
                                                                                                                                                Object this$deliDanaHastaligi = this.getDeliDanaHastaligi();
                                                                                                                                                Object other$deliDanaHastaligi = other.getDeliDanaHastaligi();
                                                                                                                                                if (this$deliDanaHastaligi
                                                                                                                                                        == null
                                                                                                                                                    ? other$deliDanaHastaligi
                                                                                                                                                        == null
                                                                                                                                                    : this$deliDanaHastaligi.equals(
                                                                                                                                                        other$deliDanaHastaligi
                                                                                                                                                    )) {
                                                                                                                                                    Object this$beyinZariKorneaNakli = this.getBeyinZariKorneaNakli();
                                                                                                                                                    Object other$beyinZariKorneaNakli = other.getBeyinZariKorneaNakli();
                                                                                                                                                    if (this$beyinZariKorneaNakli
                                                                                                                                                            == null
                                                                                                                                                        ? other$beyinZariKorneaNakli
                                                                                                                                                            == null
                                                                                                                                                        : this$beyinZariKorneaNakli.equals(
                                                                                                                                                            other$beyinZariKorneaNakli
                                                                                                                                                        )) {
                                                                                                                                                        Object this$kanOrganNakli = this.getKanOrganNakli();
                                                                                                                                                        Object other$kanOrganNakli = other.getKanOrganNakli();
                                                                                                                                                        if (this$kanOrganNakli
                                                                                                                                                                == null
                                                                                                                                                            ? other$kanOrganNakli
                                                                                                                                                                == null
                                                                                                                                                            : this$kanOrganNakli.equals(
                                                                                                                                                                other$kanOrganNakli
                                                                                                                                                            )) {
                                                                                                                                                            Object this$baskasininKaniIleTemas = this.getBaskasininKaniIleTemas();
                                                                                                                                                            Object other$baskasininKaniIleTemas = other.getBaskasininKaniIleTemas();
                                                                                                                                                            if (this$baskasininKaniIleTemas
                                                                                                                                                                    == null
                                                                                                                                                                ? other$baskasininKaniIleTemas
                                                                                                                                                                    == null
                                                                                                                                                                : this$baskasininKaniIleTemas.equals(
                                                                                                                                                                    other$baskasininKaniIleTemas
                                                                                                                                                                )
                                                                                                                                                                )
                                                                                                                                                             {
                                                                                                                                                                Object this$dovmeEstetikMudahale = this.getDovmeEstetikMudahale();
                                                                                                                                                                Object other$dovmeEstetikMudahale = other.getDovmeEstetikMudahale();
                                                                                                                                                                if (this$dovmeEstetikMudahale
                                                                                                                                                                        == null
                                                                                                                                                                    ? other$dovmeEstetikMudahale
                                                                                                                                                                        == null
                                                                                                                                                                    : this$dovmeEstetikMudahale.equals(
                                                                                                                                                                        other$dovmeEstetikMudahale
                                                                                                                                                                    )
                                                                                                                                                                    )
                                                                                                                                                                 {
                                                                                                                                                                    Object this$kuduzAsisi = this.getKuduzAsisi();
                                                                                                                                                                    Object other$kuduzAsisi = other.getKuduzAsisi();
                                                                                                                                                                    if (this$kuduzAsisi
                                                                                                                                                                            == null
                                                                                                                                                                        ? other$kuduzAsisi
                                                                                                                                                                            == null
                                                                                                                                                                        : this$kuduzAsisi.equals(
                                                                                                                                                                            other$kuduzAsisi
                                                                                                                                                                        )
                                                                                                                                                                        )
                                                                                                                                                                     {
                                                                                                                                                                        Object this$tutukluluk = this.getTutukluluk();
                                                                                                                                                                        Object other$tutukluluk = other.getTutukluluk();
                                                                                                                                                                        if (this$tutukluluk
                                                                                                                                                                                == null
                                                                                                                                                                            ? other$tutukluluk
                                                                                                                                                                                == null
                                                                                                                                                                            : this$tutukluluk.equals(
                                                                                                                                                                                other$tutukluluk
                                                                                                                                                                            )
                                                                                                                                                                            )
                                                                                                                                                                         {
                                                                                                                                                                            Object this$kanBagisiErkek = this.getKanBagisiErkek();
                                                                                                                                                                            Object other$kanBagisiErkek = other.getKanBagisiErkek();
                                                                                                                                                                            if (this$kanBagisiErkek
                                                                                                                                                                                    == null
                                                                                                                                                                                ? other$kanBagisiErkek
                                                                                                                                                                                    == null
                                                                                                                                                                                : this$kanBagisiErkek.equals(
                                                                                                                                                                                    other$kanBagisiErkek
                                                                                                                                                                                )
                                                                                                                                                                                )
                                                                                                                                                                             {
                                                                                                                                                                                Object this$erkekErkegeIliskiKadinHamilelik = this.getErkekErkegeIliskiKadinHamilelik();
                                                                                                                                                                                Object other$erkekErkegeIliskiKadinHamilelik = other.getErkekErkegeIliskiKadinHamilelik();
                                                                                                                                                                                return this$erkekErkegeIliskiKadinHamilelik
                                                                                                                                                                                        == null
                                                                                                                                                                                    ? other$erkekErkegeIliskiKadinHamilelik
                                                                                                                                                                                        == null
                                                                                                                                                                                    : this$erkekErkegeIliskiKadinHamilelik.equals(
                                                                                                                                                                                        other$erkekErkegeIliskiKadinHamilelik
                                                                                                                                                                                    );
                                                                                                                                                                            } else {
                                                                                                                                                                                return false;
                                                                                                                                                                            }
                                                                                                                                                                        } else {
                                                                                                                                                                            return false;
                                                                                                                                                                        }
                                                                                                                                                                    } else {
                                                                                                                                                                        return false;
                                                                                                                                                                    }
                                                                                                                                                                } else {
                                                                                                                                                                    return false;
                                                                                                                                                                }
                                                                                                                                                            } else {
                                                                                                                                                                return false;
                                                                                                                                                            }
                                                                                                                                                        } else {
                                                                                                                                                            return false;
                                                                                                                                                        }
                                                                                                                                                    } else {
                                                                                                                                                        return false;
                                                                                                                                                    }
                                                                                                                                                } else {
                                                                                                                                                    return false;
                                                                                                                                                }
                                                                                                                                            } else {
                                                                                                                                                return false;
                                                                                                                                            }
                                                                                                                                        } else {
                                                                                                                                            return false;
                                                                                                                                        }
                                                                                                                                    } else {
                                                                                                                                        return false;
                                                                                                                                    }
                                                                                                                                } else {
                                                                                                                                    return false;
                                                                                                                                }
                                                                                                                            } else {
                                                                                                                                return false;
                                                                                                                            }
                                                                                                                        } else {
                                                                                                                            return false;
                                                                                                                        }
                                                                                                                    } else {
                                                                                                                        return false;
                                                                                                                    }
                                                                                                                } else {
                                                                                                                    return false;
                                                                                                                }
                                                                                                            } else {
                                                                                                                return false;
                                                                                                            }
                                                                                                        } else {
                                                                                                            return false;
                                                                                                        }
                                                                                                    } else {
                                                                                                        return false;
                                                                                                    }
                                                                                                } else {
                                                                                                    return false;
                                                                                                }
                                                                                            } else {
                                                                                                return false;
                                                                                            }
                                                                                        } else {
                                                                                            return false;
                                                                                        }
                                                                                    } else {
                                                                                        return false;
                                                                                    }
                                                                                } else {
                                                                                    return false;
                                                                                }
                                                                            } else {
                                                                                return false;
                                                                            }
                                                                        } else {
                                                                            return false;
                                                                        }
                                                                    } else {
                                                                        return false;
                                                                    }
                                                                } else {
                                                                    return false;
                                                                }
                                                            } else {
                                                                return false;
                                                            }
                                                        } else {
                                                            return false;
                                                        }
                                                    } else {
                                                        return false;
                                                    }
                                                } else {
                                                    return false;
                                                }
                                            } else {
                                                return false;
                                            }
                                        } else {
                                            return false;
                                        }
                                    } else {
                                        return false;
                                    }
                                } else {
                                    return false;
                                }
                            } else {
                                return false;
                            }
                        } else {
                            return false;
                        }
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }
    }

    @Generated
    protected boolean canEqual(final Object other) {
        return other instanceof DonationFormRequestDTO;
    }

    @Generated
    @Override
    public int hashCode() {
        int PRIME = 59;
        int result = 1;
        Object $onamFormuOkundumu = this.getOnamFormuOkundumu();
        result = result * 59 + ($onamFormuOkundumu == null ? 43 : $onamFormuOkundumu.hashCode());
        Object $saglikliMi = this.getSaglikliMi();
        result = result * 59 + ($saglikliMi == null ? 43 : $saglikliMi.hashCode());
        Object $tehlikeliHobiVarMi = this.getTehlikeliHobiVarMi();
        result = result * 59 + ($tehlikeliHobiVarMi == null ? 43 : $tehlikeliHobiVarMi.hashCode());
        Object $dahaOnceGeriCevrildinizMi = this.getDahaOnceGeriCevrildinizMi();
        result = result * 59 + ($dahaOnceGeriCevrildinizMi == null ? 43 : $dahaOnceGeriCevrildinizMi.hashCode());
        Object $ilacKullaniyorMusunuz = this.getIlacKullaniyorMusunuz();
        result = result * 59 + ($ilacKullaniyorMusunuz == null ? 43 : $ilacKullaniyorMusunuz.hashCode());
        Object $enfeksiyonIlacAlimi = this.getEnfeksiyonIlacAlimi();
        result = result * 59 + ($enfeksiyonIlacAlimi == null ? 43 : $enfeksiyonIlacAlimi.hashCode());
        Object $agriKesiciAlimi = this.getAgriKesiciAlimi();
        result = result * 59 + ($agriKesiciAlimi == null ? 43 : $agriKesiciAlimi.hashCode());
        Object $alerjiTedavisi = this.getAlerjiTedavisi();
        result = result * 59 + ($alerjiTedavisi == null ? 43 : $alerjiTedavisi.hashCode());
        Object $digerIlacKullanimi = this.getDigerIlacKullanimi();
        result = result * 59 + ($digerIlacKullanimi == null ? 43 : $digerIlacKullanimi.hashCode());
        Object $disTedavisi = this.getDisTedavisi();
        result = result * 59 + ($disTedavisi == null ? 43 : $disTedavisi.hashCode());
        Object $ishal = this.getIshal();
        result = result * 59 + ($ishal == null ? 43 : $ishal.hashCode());
        Object $asiOlunduMu = this.getAsiOlunduMu();
        result = result * 59 + ($asiOlunduMu == null ? 43 : $asiOlunduMu.hashCode());
        Object $kronikHastalik = this.getKronikHastalik();
        result = result * 59 + ($kronikHastalik == null ? 43 : $kronikHastalik.hashCode());
        Object $paraKarsiligiIliski = this.getParaKarsiligiIliski();
        result = result * 59 + ($paraKarsiligiIliski == null ? 43 : $paraKarsiligiIliski.hashCode());
        Object $frengiGonore = this.getFrengiGonore();
        result = result * 59 + ($frengiGonore == null ? 43 : $frengiGonore.hashCode());
        Object $aidsHastaligi = this.getAidsHastaligi();
        result = result * 59 + ($aidsHastaligi == null ? 43 : $aidsHastaligi.hashCode());
        Object $aidsHastasiIleIliski = this.getAidsHastasiIleIliski();
        result = result * 59 + ($aidsHastasiIleIliski == null ? 43 : $aidsHastasiIleIliski.hashCode());
        Object $kanAlanKisiIleIliski = this.getKanAlanKisiIleIliski();
        result = result * 59 + ($kanAlanKisiIleIliski == null ? 43 : $kanAlanKisiIleIliski.hashCode());
        Object $uyusturucuKullanimi = this.getUyusturucuKullanimi();
        result = result * 59 + ($uyusturucuKullanimi == null ? 43 : $uyusturucuKullanimi.hashCode());
        Object $hormonIlacKullanimi = this.getHormonIlacKullanimi();
        result = result * 59 + ($hormonIlacKullanimi == null ? 43 : $hormonIlacKullanimi.hashCode());
        Object $ameliyatEndoskopi = this.getAmeliyatEndoskopi();
        result = result * 59 + ($ameliyatEndoskopi == null ? 43 : $ameliyatEndoskopi.hashCode());
        Object $kalpAkcigerHastalik = this.getKalpAkcigerHastalik();
        result = result * 59 + ($kalpAkcigerHastalik == null ? 43 : $kalpAkcigerHastalik.hashCode());
        Object $nobetEpilepsiFelc = this.getNobetEpilepsiFelc();
        result = result * 59 + ($nobetEpilepsiFelc == null ? 43 : $nobetEpilepsiFelc.hashCode());
        Object $kanserTedavisi = this.getKanserTedavisi();
        result = result * 59 + ($kanserTedavisi == null ? 43 : $kanserTedavisi.hashCode());
        Object $sekerRomatizma = this.getSekerRomatizma();
        result = result * 59 + ($sekerRomatizma == null ? 43 : $sekerRomatizma.hashCode());
        Object $kanHastaligi = this.getKanHastaligi();
        result = result * 59 + ($kanHastaligi == null ? 43 : $kanHastaligi.hashCode());
        Object $sıtmaTuberkuloz = this.getSıtmaTuberkuloz();
        result = result * 59 + ($sıtmaTuberkuloz == null ? 43 : $sıtmaTuberkuloz.hashCode());
        Object $hepatitTasiyicilik = this.getHepatitTasiyicilik();
        result = result * 59 + ($hepatitTasiyicilik == null ? 43 : $hepatitTasiyicilik.hashCode());
        Object $hepatitliIliski = this.getHepatitliIliski();
        result = result * 59 + ($hepatitliIliski == null ? 43 : $hepatitliIliski.hashCode());
        Object $toksoplazma = this.getToksoplazma();
        result = result * 59 + ($toksoplazma == null ? 43 : $toksoplazma.hashCode());
        Object $belirliUlkelerdeBulundunuzMu = this.getBelirliUlkelerdeBulundunuzMu();
        result = result * 59 + ($belirliUlkelerdeBulundunuzMu == null ? 43 : $belirliUlkelerdeBulundunuzMu.hashCode());
        Object $ingiltereKuzayIrlanda = this.getIngiltereKuzayIrlanda();
        result = result * 59 + ($ingiltereKuzayIrlanda == null ? 43 : $ingiltereKuzayIrlanda.hashCode());
        Object $digerUlkeler = this.getDigerUlkeler();
        result = result * 59 + ($digerUlkeler == null ? 43 : $digerUlkeler.hashCode());
        Object $deliDanaHastaligi = this.getDeliDanaHastaligi();
        result = result * 59 + ($deliDanaHastaligi == null ? 43 : $deliDanaHastaligi.hashCode());
        Object $beyinZariKorneaNakli = this.getBeyinZariKorneaNakli();
        result = result * 59 + ($beyinZariKorneaNakli == null ? 43 : $beyinZariKorneaNakli.hashCode());
        Object $kanOrganNakli = this.getKanOrganNakli();
        result = result * 59 + ($kanOrganNakli == null ? 43 : $kanOrganNakli.hashCode());
        Object $baskasininKaniIleTemas = this.getBaskasininKaniIleTemas();
        result = result * 59 + ($baskasininKaniIleTemas == null ? 43 : $baskasininKaniIleTemas.hashCode());
        Object $dovmeEstetikMudahale = this.getDovmeEstetikMudahale();
        result = result * 59 + ($dovmeEstetikMudahale == null ? 43 : $dovmeEstetikMudahale.hashCode());
        Object $kuduzAsisi = this.getKuduzAsisi();
        result = result * 59 + ($kuduzAsisi == null ? 43 : $kuduzAsisi.hashCode());
        Object $tutukluluk = this.getTutukluluk();
        result = result * 59 + ($tutukluluk == null ? 43 : $tutukluluk.hashCode());
        Object $kanBagisiErkek = this.getKanBagisiErkek();
        result = result * 59 + ($kanBagisiErkek == null ? 43 : $kanBagisiErkek.hashCode());
        Object $erkekErkegeIliskiKadinHamilelik = this.getErkekErkegeIliskiKadinHamilelik();
        return result * 59 + ($erkekErkegeIliskiKadinHamilelik == null ? 43 : $erkekErkegeIliskiKadinHamilelik.hashCode());
    }

    @Generated
    @Override
    public String toString() {
        return "DonationFormRequestDTO(onamFormuOkundumu="
            + this.getOnamFormuOkundumu()
            + ", saglikliMi="
            + this.getSaglikliMi()
            + ", tehlikeliHobiVarMi="
            + this.getTehlikeliHobiVarMi()
            + ", dahaOnceGeriCevrildinizMi="
            + this.getDahaOnceGeriCevrildinizMi()
            + ", ilacKullaniyorMusunuz="
            + this.getIlacKullaniyorMusunuz()
            + ", enfeksiyonIlacAlimi="
            + this.getEnfeksiyonIlacAlimi()
            + ", agriKesiciAlimi="
            + this.getAgriKesiciAlimi()
            + ", alerjiTedavisi="
            + this.getAlerjiTedavisi()
            + ", digerIlacKullanimi="
            + this.getDigerIlacKullanimi()
            + ", disTedavisi="
            + this.getDisTedavisi()
            + ", ishal="
            + this.getIshal()
            + ", asiOlunduMu="
            + this.getAsiOlunduMu()
            + ", kronikHastalik="
            + this.getKronikHastalik()
            + ", paraKarsiligiIliski="
            + this.getParaKarsiligiIliski()
            + ", frengiGonore="
            + this.getFrengiGonore()
            + ", aidsHastaligi="
            + this.getAidsHastaligi()
            + ", aidsHastasiIleIliski="
            + this.getAidsHastasiIleIliski()
            + ", kanAlanKisiIleIliski="
            + this.getKanAlanKisiIleIliski()
            + ", uyusturucuKullanimi="
            + this.getUyusturucuKullanimi()
            + ", hormonIlacKullanimi="
            + this.getHormonIlacKullanimi()
            + ", ameliyatEndoskopi="
            + this.getAmeliyatEndoskopi()
            + ", kalpAkcigerHastalik="
            + this.getKalpAkcigerHastalik()
            + ", nobetEpilepsiFelc="
            + this.getNobetEpilepsiFelc()
            + ", kanserTedavisi="
            + this.getKanserTedavisi()
            + ", sekerRomatizma="
            + this.getSekerRomatizma()
            + ", kanHastaligi="
            + this.getKanHastaligi()
            + ", sıtmaTuberkuloz="
            + this.getSıtmaTuberkuloz()
            + ", hepatitTasiyicilik="
            + this.getHepatitTasiyicilik()
            + ", hepatitliIliski="
            + this.getHepatitliIliski()
            + ", toksoplazma="
            + this.getToksoplazma()
            + ", belirliUlkelerdeBulundunuzMu="
            + this.getBelirliUlkelerdeBulundunuzMu()
            + ", ingiltereKuzayIrlanda="
            + this.getIngiltereKuzayIrlanda()
            + ", digerUlkeler="
            + this.getDigerUlkeler()
            + ", deliDanaHastaligi="
            + this.getDeliDanaHastaligi()
            + ", beyinZariKorneaNakli="
            + this.getBeyinZariKorneaNakli()
            + ", kanOrganNakli="
            + this.getKanOrganNakli()
            + ", baskasininKaniIleTemas="
            + this.getBaskasininKaniIleTemas()
            + ", dovmeEstetikMudahale="
            + this.getDovmeEstetikMudahale()
            + ", kuduzAsisi="
            + this.getKuduzAsisi()
            + ", tutukluluk="
            + this.getTutukluluk()
            + ", kanBagisiErkek="
            + this.getKanBagisiErkek()
            + ", erkekErkegeIliskiKadinHamilelik="
            + this.getErkekErkegeIliskiKadinHamilelik()
            + ")";
    }
}
