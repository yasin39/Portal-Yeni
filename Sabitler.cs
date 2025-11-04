using System.Collections.Generic;

namespace Portal
{
    public class Sabitler
    {
        #region Cimer Modülü Onay Durumları

        /// <summary>
        /// Havale bekliyor - Personele atanmış, işlem yapılmamış
        /// </summary>
        public const int HAVALE = 0;

        /// <summary>
        /// Onay bekliyor - Personel cevaplamış, yönetici onayı bekleniyor
        /// </summary>
        public const int ONAY_BEKLIYOR = 1;

        /// <summary>
        /// Yönetici tarafından onaylandı,Bitti 
        /// </summary>
        public const int BITTI = 2;

        /// <summary>
        /// Başvuru reddedildil Kapatıldı 
        /// </summary>
        public const int KAPATILDI = 3;

        #endregion

        #region Cimer Modülü Kullanıcı personel tiplerini temsil eden sabitler

        /// <summary>
        /// Normal Personel - Tek onay yeterli (Onay_Durumu: 1 → 2)
        /// </summary>
        public const int NORMAL_PERSONEL = 0;

        /// <summary>
        /// Onaylayıcı/Yönetici - İkinci onay gerekli (Onay_Durumu: 1 → 1, başka onaylayıcıya)
        /// </summary>
        public const int ONAYLAYICI = 1;

        #endregion

        #region Cimer Modülü Yetki Kodları

        public const int CIMER_KAYIT = 601;
        public const int CIMER_PERSONEL = 600;
        public const int CIMER_SEVK_IADE = 699;
        public const int CIMER_RAPORLAMA = 603;
        public const int CIMER_DEVAMEDEN_BASVURU = 606;
        public const int CIMER_BITEN_BASVURU = 608;

        #endregion

        #region Personel Yetki Kodları

        public const int Personel = 100;
        public const int IzinSilme = 199;

        #endregion

        #region BelgeTakip Yetki Kodları

        public const int BelgeTakip = 150;

        #endregion

        #region Personel statü ve durum değerleri
        public const string Aktif = "Aktif";
        public const string KadroluAktifCalisan = "Kadrolu Aktif Çalışan";
        public const string GeciciGorevliAktifCalisan = "Geçici Görevli Aktif Çalışan";
        public const string GeciciGorevdePasifCalisan = "Geçici Görevde Pasif Çalışan";
        public const string FirmaPersoneli = "Firma personeli";
        public const string IskurIsciTYP = "İşkur İşçi (TYP)";

        #endregion

        #region Personel Common field names and query fragments (for reuse in SQL building).
        public const string SicilNo = "SicilNo";
        public const string TcKimlikNo = "TcKimlikNo";
        public const string Adi = "Adi";
        public const string Soyad = "Soyad";
        public const string Unvan = "Unvan";
        public const string Statu = "Statu";
        public const string Durum = "Durum";
        public const string CalismaDurumu = "CalismaDurumu";
        public const string YetkiNo = "Yetki_No";
        public const string Sicil_No = "Sicil_No"; // Note: Underscore variant used in some tables
        public const string MeslekiUnvan = "MeslekiUnvan";
        public const string Sendika = "Sendika";
        public const string KanGrubu = "KanGrubu";
        public const string MedeniHali = "MedeniHali";
        public const string Cinsiyet = "Cinsiyet";
        public const string GorevYaptigiBirim = "GorevYaptigiBirim";
        public const string Ogrenim_Durumu = "Ogrenim_Durumu";
        public const string KurumaBaslamaTarihi = "KurumaBaslamaTarihi";
        public const string IstenAyrilisTarihi = "istenAyrilisTarihi";
        public const string DevredenIzin = "Devredenizin";
        public const string CariIzin = "cariyilizni";
        public const string ToplamIzin = "toplamizin";
        public const string IzinTuru = "izin_turu";
        public const string IzinSuresi = "izin_Suresi";
        public const string IzneBaslamaTarihi = "izne_Baslama_Tarihi";
        public const string IzinBitisTarihi = "izin_Bitis_Tarihi";
        public const string GoreveBaslamaTarihi = "Goreve_Baslama_Tarihi";
        public const string Aciklama = "Aciklama";
        public const string KayitTarihi = "Kayit_Tarihi";
        public const string KayitKullanici = "Kayit_Kullanici";
        public const string SonGuncellemeTarihi = "SonGuncellemeTarihi";
        public const string SonGuncellemeKullanici = "SonGuncellemeKullanici";
        #endregion

        #region BelgeTakip Modülü Yetki Kodları

        /// <summary>
        /// Belge Takip Modülü - Takipteki Firmalar Sayfası
        /// </summary>
        public const int BELGE_TAKIP_FIRMALAR = 700;
        public const int BELGE_TAKIP_ANALIZ = 701;



        #endregion

        #region BelgeTakip Modülü Genel Ayarlar

        /// <summary>
        /// Firma belge alma takip süresi (gün cinsinden)
        /// </summary>
        public const int TAKIP_SURESI_GUN = 30;

        /// <summary>
        /// TMFB Belge Kodu
        /// </summary>
        public const string TMFB_BELGE_KODU = "TMFB";

        #endregion

        #region Denetim Modülü Yetki Kodları

        /// <summary>
        /// Denetim Modülü - İstatistik Sayfası
        /// </summary>
        public const int DENETIM_ISTATISTIK = 200;

        /// <summary>
        /// Denetim Modülü - Uzaktan Denetim Raporlama Sayfası
        /// </summary>
        public const int DENETIM_UZAK_RAPOR = 200;
        /// <summary>
        /// Denetim Modülü - Taşıt Denetim Girişi
        /// </summary>
        public const int DENETIM_TASIT_GIRIS = 200;

        /// <summary>
        /// Denetim Modülü - İşletme Denetim Girişi
        /// </summary>
        public const int DENETIM_ISLETME = 200;

        /// <summary>
        /// Denetim Modülü - Uzaktan Denetim Girişi
        /// </summary>
        public const int DENETIM_UZAK_GIRIS = 201;
        public const int DENETIM_UZAK_GOREV = 200;

        /// <summary>
        /// Denetim Modülü - Taşıt Denetim Silme
        /// </summary>
        public const int DENETIM_TASIT_SILME = 299;

        /// <summary>
        /// Denetim Modülü - İşletme Denetim Silme
        /// </summary>
        public const int DENETIM_ISLETME_SILME = 299;

        #endregion

        #region Denetim Modülü Sabitler

        /// <summary>
        /// Denetim türleri
        /// </summary>
        public const string YOLCU_TASIMACILIGI = "Yolcu Taşımacılığı";
        public const string ESYA_TASIMACILIGI = "Eşya Taşımacılığı";
        public const string TEHLIKELI_MADDE = "Tehlikeli Madde";

        #endregion

        #region Denetim Modülü Durum Değerleri

        public const string ACIK = "Açık";
        public const string KAPALI = "Kapalı";

        #endregion

        #region Görev Takip Modülü Yetki Kodları

        /// <summary>
        /// Görev Takip Modülü - Talep Ekleme ve Listeleme
        /// </summary>
        public const int GOREV_TAKIP_SISTEMI = 500;

        #endregion

        #region Tehlikeli Madde Modülü Yetki Kodları

        /// <summary>
        /// Tehlikeli Madde Modülü - Firma Kayıt ve İstatistik
        /// </summary>
        public const int TEHLIKELI_MADDE_KAYIT = 800;

        /// <summary>
        /// Tehlikeli Madde Modülü - Faaliyet Alanları Tanımlama
        /// </summary>
        public const int TEHLIKELI_MADDE_TANIMLAR = 899;

        #endregion

        #region Yönetici Modülü Yetki Kodları

        /// <summary>
        /// Duyuru Yönetimi Modülü
        /// </summary>
        public const int DUYURU_YONETIMI = 900;

        public const int YONETIM_YETKI = 900;
        public const int MODUL_YONETICI_KULLANICI_ISLEM = 900;

        #endregion

        #region Arşiv Modülü Yetki Kodları

        /// <summary>
        /// Arşiv Modülü - Dosya Birleştirme
        /// </summary>
        public const int ARSIV = 300;

        #endregion

        #region Bilgisayar Tipleri
        public static readonly List<string> BilgisayarTipleri = new List<string>
{
    "All in One",
    "Hp-Elitedesk-G6",
    "Hp-G3",
    "Hp-G2",
    "Hep-G1",
    "Hp Elitedesk",
    "Hp ProDesk",
    "Laptop",
    "Acer",
    "Asus",
    "Diğer"
};
        #endregion
    }
}