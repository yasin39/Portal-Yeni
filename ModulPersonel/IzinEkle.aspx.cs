using Microsoft.Reporting.WebForms;
using Portal.Base;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Portal.ModulPersonel
{
    public partial class IzinEkle : BasePage
    {
        private string secilenSicilNo;
        private int secilenIzinId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.Personel))
                {
                    return;
                }
                IzinleriGetir();
                MesajPaneli.Visible = false;
            }
        }
        protected void SicilNoMetinKutusu_TextChanged(object sender, EventArgs e)
        {
            string sicilNo = SicilNoMetinKutusu.Text.Trim();
            if (string.IsNullOrEmpty(sicilNo))
            {
                TemizlePersonelBilgileri();
                return;
            }
            PersoneliSicilNoIleYukle(sicilNo);
            IzinleriGetir();
        }
        private void PersoneliSicilNoIleYukle(string sicilNo)
        {
            try
            {
                string sorgu = "SELECT * FROM personel WHERE Sicil_No = @SicilNo";
                var parametreler = CreateParameters(("@SicilNo", sicilNo));
                DataTable dt = ExecuteDataTable(sorgu, parametreler);
                if (dt.Rows.Count == 0)
                {
                    ShowError("Sicil No ile personel bulunamadı.");
                    TemizlePersonelBilgileri();
                    return;
                }
                DataRow satir = dt.Rows[0];
                TcKimlikNoMetinKutusu.Text = satir[Sabitler.TcKimlikNo].ToString();
                AdiSoyadiMetinKutusu.Text = satir[Sabitler.Adi].ToString() + " " + satir[Sabitler.Soyad].ToString();
                AdiSoyadiLabel.Text = AdiSoyadiMetinKutusu.Text;
                AdiSoyadiLabel.Visible = true;
                UnvanMetinKutusu.Text = satir[Sabitler.Unvan].ToString();
                BirimMetinKutusu.Text = satir[Sabitler.GorevYaptigiBirim].ToString();
                StatuMetinKutusu.Text = satir[Sabitler.Statu].ToString();
                DevredenIzinMetinKutusu.Text = satir[Sabitler.DevredenIzin].ToString();
                CariIzinMetinKutusu.Text = satir[Sabitler.CariIzin].ToString();
                int devreden = string.IsNullOrEmpty(DevredenIzinMetinKutusu.Text) ? 0 : Convert.ToInt32(DevredenIzinMetinKutusu.Text);
                int cari = string.IsNullOrEmpty(CariIzinMetinKutusu.Text) ? 0 : Convert.ToInt32(CariIzinMetinKutusu.Text);
                ToplamYillikIzinMetinKutusu.Text = (devreden + cari).ToString();
                string resimYolu = satir["Resim"].ToString();
                if (!string.IsNullOrEmpty(resimYolu))
                {
                    PersonelResim.ImageUrl = resimYolu;
                    PersonelResim.Visible = true;
                }
                else
                {
                    PersonelResim.Visible = false;
                }
                secilenSicilNo = sicilNo;
                IzinGridView.Visible = true;
            }
            catch (Exception ex)
            {
                LogError("Personel yükleme hatası", ex);
                ShowError("Personel bilgileri yüklenirken hata oluştu.");
            }
        }
        private void TemizlePersonelBilgileri()
        {
            TcKimlikNoMetinKutusu.Text = "";
            AdiSoyadiMetinKutusu.Text = "";
            UnvanMetinKutusu.Text = "";
            BirimMetinKutusu.Text = "";
            StatuMetinKutusu.Text = "";
            DevredenIzinMetinKutusu.Text = "";
            CariIzinMetinKutusu.Text = "";
            ToplamYillikIzinMetinKutusu.Text = "";
            PersonelResim.Visible = false;
            AdiSoyadiLabel.Visible = false;
            IzinGridView.Visible = false;
            RaporGörüntüleyici.Visible = false;
        }
        protected void IzinSuresiMetinKutusu_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(IzneBaslamaTarihiMetinKutusu.Text) || string.IsNullOrEmpty(IzinSuresiMetinKutusu.Text))
                return;
            TarihleriHesapla();
        }
        private void TarihleriHesapla()
        {
            if (!DateTime.TryParse(IzneBaslamaTarihiMetinKutusu.Text, out DateTime baslamaTarihi) ||
            !double.TryParse(IzinSuresiMetinKutusu.Text.Replace(',', '.'), out double sure))
            {
                return;
            }
            DateTime bitisTarihi;
            if (StatuMetinKutusu.Text == "Memur")
            {
                bitisTarihi = baslamaTarihi.AddDays(sure).AddDays(-1);
                GoreveBaslamaTarihiMetinKutusu.Text = FormatDateTurkish(baslamaTarihi.AddDays(sure));
            }
            else
            {
                // Pazar günlerini ekle
                int pazarSayisi = 0;
                DateTime tempTarih = baslamaTarihi;
                for (int i = 0; i < sure; i++)
                {
                    if (tempTarih.DayOfWeek == DayOfWeek.Sunday)
                        pazarSayisi++;
                    tempTarih = tempTarih.AddDays(1);
                }
                bitisTarihi = baslamaTarihi.AddDays(sure + pazarSayisi - 1);
                GoreveBaslamaTarihiMetinKutusu.Text = FormatDateTurkish(baslamaTarihi.AddDays(sure + pazarSayisi));
            }
            IzinBitisTarihiMetinKutusu.Text = FormatDateTurkish(bitisTarihi);
        }
        protected void EkleButonu_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || string.IsNullOrEmpty(secilenSicilNo))
            {
                return;
            }
            try
            {
                // Çakışma kontrolü
                if (IzinCarpismaKontrolEt())
                {
                    MesajLabel.Text = "Seçilen tarihlerde zaten tanımlı izin bulunmaktadır. Farklı tarih seçiniz.";
                    MesajPaneli.CssClass = "alert alert-danger";
                    MesajPaneli.Visible = true;
                    return;
                }
                IzinKaydet(false);
                ShowSuccessAndRedirect("Kayıt başarıyla eklendi.", "personelizinekle.aspx");
            }
            catch (Exception ex)
            {
                LogError("İzin ekleme hatası", ex);
                ShowError("İzin eklenirken hata oluştu.");
            }
        }
        protected void GuncelleButonu_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || secilenIzinId == 0)
            {
                return;
            }
            try
            {
                IzinKaydet(true);
                ShowSuccessAndRedirect("Kayıt başarıyla güncellendi.", "personelizinekle.aspx");
            }
            catch (Exception ex)
            {
                LogError("İzin güncelleme hatası", ex);
                ShowError("İzin güncellenirken hata oluştu.");
            }
        }
        private void IzinKaydet(bool guncelleModu)
        {
            if (!DateTime.TryParse(IzneBaslamaTarihiMetinKutusu.Text, out DateTime baslamaTarihi) ||
            !DateTime.TryParse(IzinBitisTarihiMetinKutusu.Text, out DateTime bitisTarihi))
            {
                throw new Exception("Geçersiz tarih formatı.");
            }
            int sure = Convert.ToInt32(IzinSuresiMetinKutusu.Text);
            string izinTuru = IzinTuruAciklamaListesi.SelectedValue;
            string aciklama = AciklamaMetinKutusu.Text;
            if (guncelleModu)
            {
                string sorgu = @"
UPDATE personel_izin
SET Adi_Soyadi = @AdiSoyadi, Devreden_izin = @DevredenIzin, Cari_izin = @CariIzin,
izin_turu = @IzinTuru, izin_Suresi = @IzinSuresi, izne_Baslama_Tarihi = @BaslamaTarihi,
izin_Bitis_Tarihi = @BitisTarihi, Goreve_Baslama_Tarihi = @DonusTarihi, Aciklama = @Aciklama,
Son_Guncelleme_Tarihi = @GuncellemeTarihi, Son_Guncelleyen_Kullanici = @GuncelleyenKullanici
WHERE id = @IzinId";
                var parametreler = CreateParameters(
                ("@AdiSoyadi", AdiSoyadiMetinKutusu.Text),
                ("@DevredenIzin", DevredenIzinMetinKutusu.Text),
                ("@CariIzin", CariIzinMetinKutusu.Text),
                ("@IzinTuru", izinTuru),
                ("@IzinSuresi", sure),
                ("@BaslamaTarihi", baslamaTarihi),
                ("@BitisTarihi", bitisTarihi),
                ("@DonusTarihi", DateTime.Parse(GoreveBaslamaTarihiMetinKutusu.Text)),
                ("@Aciklama", aciklama),
                ("@GuncellemeTarihi", DateTime.Now),
                ("@GuncelleyenKullanici", CurrentUserName),
                ("@IzinId", secilenIzinId)
                );
                ExecuteNonQuery(sorgu, parametreler);
            }
            else
            {
                string sorgu = @"
INSERT INTO personel_izin (SicilNo, Adi_Soyadi, Statu, Devreden_izin, Cari_izin, izin_turu,
izin_Suresi, izne_Baslama_Tarihi, izin_Bitis_Tarihi, Goreve_Baslama_Tarihi, Aciklama,
Kayit_Tarihi, Kayit_Kullanici)
VALUES (@SicilNo, @AdiSoyadi, @Statu, @DevredenIzin, @CariIzin, @IzinTuru, @IzinSuresi,
@BaslamaTarihi, @BitisTarihi, @DonusTarihi, @Aciklama, @KayitTarihi, @KayitKullanici)";
                var parametreler = CreateParameters(
                ("@SicilNo", secilenSicilNo),
                ("@AdiSoyadi", AdiSoyadiMetinKutusu.Text),
                ("@Statu", StatuMetinKutusu.Text),
                ("@DevredenIzin", DevredenIzinMetinKutusu.Text),
                ("@CariIzin", CariIzinMetinKutusu.Text),
                ("@IzinTuru", izinTuru),
                ("@IzinSuresi", sure),
                ("@BaslamaTarihi", baslamaTarihi),
                ("@BitisTarihi", bitisTarihi),
                ("@DonusTarihi", DateTime.Parse(GoreveBaslamaTarihiMetinKutusu.Text)),
                ("@Aciklama", aciklama),
                ("@KayitTarihi", DateTime.Now),
                ("@KayitKullanici", CurrentUserName)
                );
                ExecuteInsertWithIdentity(sorgu, parametreler);
                // Yıllık izin ise personel izinlerini güncelle
                if (izinTuru == "Yıllık İzin")
                {
                    IzinleriGuncelle();
                }
            }
            if (izinTuru != "Yıllık İzin")
            {
                RaporOlustur();
            }
        }
        private void IzinleriGuncelle()
        {
            int devreden = Convert.ToInt32(DevredenIzinMetinKutusu.Text);
            int cari = Convert.ToInt32(CariIzinMetinKutusu.Text);
            int sure = Convert.ToInt32(IzinSuresiMetinKutusu.Text);
            int yeniDevreden = devreden;
            int yeniCari = cari;
            if (sure <= devreden)
            {
                yeniDevreden = devreden - sure;
                yeniCari = cari + yeniDevreden; // Kalan devreden cariye ekle? Mantık orijinale göre
            }
            else
            {
                int artan = sure - devreden;
                yeniDevreden = 0;
                yeniCari = cari - artan;
            }
            string sorgu = @"
UPDATE personel
SET Devredenizin = @YeniDevreden, cariyilizni = @YeniCari, toplamizin = @ToplamIzin
WHERE SicilNo = @SicilNo";
            var parametreler = CreateParameters(
            ("@YeniDevreden", yeniDevreden),
            ("@YeniCari", yeniCari),
            ("@ToplamIzin", yeniDevreden + yeniCari),
            ("@SicilNo", secilenSicilNo)
            );
            ExecuteNonQuery(sorgu, parametreler);
        }
        private bool IzinCarpismaKontrolEt()
        {
            string sorgu = @"
SELECT COUNT(*) FROM personel_izin
WHERE SicilNo = @SicilNo
AND ((izne_Baslama_Tarihi BETWEEN @BaslamaTarihi AND @BitisTarihi)
OR (izin_Bitis_Tarihi BETWEEN @BaslamaTarihi AND @BitisTarihi))";
            var parametreler = CreateParameters(
            ("@SicilNo", secilenSicilNo),
            ("@BaslamaTarihi", DateTime.Parse(IzneBaslamaTarihiMetinKutusu.Text)),
            ("@BitisTarihi", DateTime.Parse(IzinBitisTarihiMetinKutusu.Text))
            );
            int carpismaSayisi = Convert.ToInt32(ExecuteScalar(sorgu, parametreler));
            return carpismaSayisi > 0;
        }
        protected void SilButonu_Click(object sender, EventArgs e)
        {
            if (!CheckPermission(Sabitler.IzinSilme))
            {
                return;
            }
            if (secilenIzinId == 0)
            {
                MesajLabel.Text = "Silinmesi istenen izni seçiniz.";
                MesajPaneli.CssClass = "alert alert-danger";
                MesajPaneli.Visible = true;
                return;
            }
            try
            {
                // Yıllık izin ise izinleri geri ekle
                DataTable dt = ExecuteDataTable("SELECT izin_turu, izin_Suresi FROM personel_izin WHERE id = @Id",
                CreateParameters(("@Id", secilenIzinId)));
                if (dt.Rows.Count > 0)
                {
                    string izinTuru = dt.Rows[0]["izin_turu"].ToString();
                    int sure = Convert.ToInt32(dt.Rows[0]["izin_Suresi"]);
                    if (izinTuru == "Yıllık İzin")
                    {
                        string guncelleSorgu = "UPDATE personel SET cariyilizni = cariyilizni + @Sure, toplamizin = toplamizin + @Sure WHERE SicilNo = @SicilNo";
                        var parametreler = CreateParameters(("@Sure", sure), ("@SicilNo", IzinGridView.SelectedRow.Cells[1].Text));
                        ExecuteNonQuery(guncelleSorgu, parametreler);
                    }
                    string silSorgu = "DELETE FROM personel_izin WHERE id = @Id";
                    ExecuteNonQuery(silSorgu, CreateParameters(("@Id", secilenIzinId)));
                }
                IzinleriGetir();
                ShowSuccessAndRedirect("İzin başarıyla silindi.", "personelizinekle.aspx");
            }
            catch (Exception ex)
            {
                LogError("İzin silme hatası", ex);
                ShowError("İzin silinirken hata oluştu.");
            }
        }
        protected void VazgecButonu_Click(object sender, EventArgs e)
        {
            TemizleIzinFormu();
            EkleModunaDon();
        }
        private void TemizleIzinFormu()
        {
            IzinTuruAciklamaListesi.SelectedIndex = 0;
            IzneBaslamaTarihiMetinKutusu.Text = "";
            IzinSuresiMetinKutusu.Text = "";
            IzinBitisTarihiMetinKutusu.Text = "";
            GoreveBaslamaTarihiMetinKutusu.Text = "";
            AciklamaMetinKutusu.Text = "";
        }
        private void EkleModunaDon()
        {
            EkleButonu.Visible = true;
            GuncelleButonu.Visible = false;
            VazgecButonu.Visible = false;
            SilButonu.Visible = false;
        }
        protected void IzinGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            secilenIzinId = Convert.ToInt32(IzinGridView.SelectedDataKey.Value);
            GridViewRow satir = IzinGridView.SelectedRow;
            // Formu seçili izinle doldur
            IzinTuruAciklamaListesi.SelectedValue = satir.Cells[6].Text; // İzin Türü
            IzneBaslamaTarihiMetinKutusu.Text = DateTime.Parse(satir.Cells[8].Text).ToString("yyyy-MM-dd");
            IzinSuresiMetinKutusu.Text = satir.Cells[7].Text;
            IzinBitisTarihiMetinKutusu.Text = satir.Cells[9].Text;
            GoreveBaslamaTarihiMetinKutusu.Text = satir.Cells[10].Text;
            AciklamaMetinKutusu.Text = satir.Cells[11].Text;
            // Mod değiştir
            EkleButonu.Visible = false;
            GuncelleButonu.Visible = true;
            VazgecButonu.Visible = true;
            SilButonu.Visible = CheckPermission(Sabitler.IzinSilme);
        }
        private void IzinleriGetir()
        {
            if (string.IsNullOrEmpty(secilenSicilNo))
            {
                IzinGridView.Visible = false;
                return;
            }
            string sorgu = "SELECT * FROM personel_izin WHERE SicilNo = @SicilNo ORDER BY id DESC";
            var parametreler = CreateParameters(("@SicilNo", secilenSicilNo));
            DataTable dt = ExecuteDataTable(sorgu, parametreler);
            IzinGridView.DataSource = dt;
            IzinGridView.DataBind();
            IzinGridView.Visible = dt.Rows.Count > 0;
        }
        private void RaporOlustur()
        {
            try
            {
                string raporSorgu = "SELECT TOP 1 * FROM personel_izin ORDER BY id DESC";
                string personelSorgu = "SELECT * FROM personel WHERE SicilNo = @SicilNo";
                string mudurSorgu = "SELECT * FROM personel WHERE SicilNo = '99999999x'";
                DataTable raporDt = ExecuteDataTable(raporSorgu);
                DataTable personelDt = ExecuteDataTable(personelSorgu, CreateParameters(("@SicilNo", secilenSicilNo)));
                DataTable mudurDt = ExecuteDataTable(mudurSorgu);
                RaporGörüntüleyici.Reset();
                RaporGörüntüleyici.ProcessingMode = ProcessingMode.Local;
                RaporGörüntüleyici.LocalReport.ReportPath = Server.MapPath("~/personelizin.rdlc");
                RaporGörüntüleyici.LocalReport.DataSources.Clear();
                RaporGörüntüleyici.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", raporDt));
                RaporGörüntüleyici.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", mudurDt));
                RaporGörüntüleyici.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", personelDt));
                RaporGörüntüleyici.LocalReport.Refresh();
                RaporGörüntüleyici.Visible = true;
            }
            catch (Exception ex)
            {
                LogError("Rapor oluşturma hatası", ex);
                ShowError("Rapor oluşturulamadı.");
            }
        }
    }
}