using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulDenetim
{
    public partial class IsletmeGiris : BasePage
    {
        private int KayitId
        {
            get { return ViewState["KayitId"] != null ? (int)ViewState["KayitId"] : 0; }
            set { ViewState["KayitId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.DENETIM_ISLETME))
                {
                    return;
                }

                SayfayiHazirla();
                SilmeYetkisiniKontrolEt();
            }
        }

        #region Sayfa Hazırlama

        private void SayfayiHazirla()
        {
            IlleriYukle();
            PersonelleriYukle();
            YetkiBelgeleriniYukle();
            FormuTemizle();
        }

        private void SilmeYetkisiniKontrolEt()
        {
            try
            {
                string sicilNo = Session["Sicil"]?.ToString();
                if (string.IsNullOrEmpty(sicilNo)) return;

                string query = @"SELECT COUNT(*) FROM yetki 
                                WHERE Sicil_No = @SicilNo AND Yetki_No = @YetkiNo";

                var parameters = CreateParameters(
                    ("@SicilNo", sicilNo),
                    ("@YetkiNo", Sabitler.DENETIM_ISLETME_SILME)
                );

                DataTable dt = ExecuteDataTable(query, parameters);
                int yetkiSayisi = Convert.ToInt32(dt.Rows[0][0]);

                if (yetkiSayisi > 0)
                {
                    btnSil.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LogError("Silme yetkisi kontrolü hatası", ex);
            }
        }

        #endregion

        #region Dropdown Yükleme

        private void IlleriYukle()
        {
            try
            {
                Helpers.LoadProvinces(ddlIl);
                // ==> İlçe başlangıçta disabled
                ddlIlce.Enabled = false;
                ddlIlce.Items.Clear();
                ddlIlce.Items.Insert(0, new ListItem("İlçe Seçiniz", ""));
            }
            catch (Exception ex)
            {
                LogError("İller yüklenirken hata", ex);
                ShowToast("İller yüklenirken hata oluştu.", "danger");
            }
        }

        private void IlceleriYukle(string ilAdi)
        {
            try
            {
                Helpers.LoadDistricts(ddlIlce, ilAdi);
            }
            catch (Exception ex)
            {
                LogError("İlçeler yüklenirken hata", ex);
                ShowToast("İlçeler yüklenirken hata oluştu.", "danger");
            }
        }

        private void PersonelleriYukle()
        {
            try
            {
                Helpers.LoadActivePersonnel(ddlPersonel1);
                Helpers.LoadActivePersonnel(ddlPersonel2);
            }
            catch (Exception ex)
            {
                LogError("Personeller yüklenirken hata", ex);
                ShowToast("Personeller yüklenirken hata oluştu.", "danger");
            }
        }

        private void YetkiBelgeleriniYukle()
        {
            try
            {
                string query = @"SELECT Belge_Adi FROM yetki_belgeleri 
                                ORDER BY Belge_Adi ASC";

                DataTable dt = ExecuteDataTable(query);

                ddlYetkiBelgesi.Items.Clear();
                ddlYetkiBelgesi.Items.Insert(0, new ListItem("Yetki Belgesi Seçiniz", ""));

                foreach (DataRow row in dt.Rows)
                {
                    ddlYetkiBelgesi.Items.Add(new ListItem(row["Belge_Adi"].ToString()));
                }
            }
            catch (Exception ex)
            {
                LogError("Yetki belgeleri yüklenirken hata", ex);
                ShowToast("Yetki belgeleri yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Button Events

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                string kayitKullanici = CurrentUserName ?? "Bilinmiyor";

                string query = @"
                    INSERT INTO denetimisletme 
                    (VergiNo, Unvan, Adres, YetkiBelgesi, DenetimTuru, DenetimTarihi, 
                     il, ilce, Personel1, Personel2, CezaDurumu, Aciklama, 
                     KayitTarihi, KayitKullanici)
                    VALUES 
                    (@VergiNo, @Unvan, @Adres, @YetkiBelgesi, @DenetimTuru, @DenetimTarihi, 
                     @Il, @Ilce, @Personel1, @Personel2, @CezaDurumu, @Aciklama, 
                     @KayitTarihi, @KayitKullanici)";

                var parameters = CreateParameters(
                    ("@VergiNo", txtVergiNo.Text),
                    ("@Unvan", txtUnvan.Text),
                    ("@Adres", txtAdres.Text),
                    ("@YetkiBelgesi", ddlYetkiBelgesi.SelectedValue),
                    ("@DenetimTuru", ddlDenetimTuru.SelectedValue),
                    ("@DenetimTarihi", ParseTarih(txtDenetimTarihi.Text)),
                    ("@Il", ddlIl.SelectedValue),
                    ("@Ilce", ddlIlce.SelectedValue),
                    ("@Personel1", ddlPersonel1.SelectedValue),
                    ("@Personel2", ddlPersonel2.SelectedValue),
                    ("@CezaDurumu", ddlCezaDurumu.SelectedValue),
                    ("@Aciklama", txtAciklama.Text),
                    ("@KayitTarihi", DateTime.Now),
                    ("@KayitKullanici", kayitKullanici)
                );

                int etkilenenSatir = ExecuteNonQuery(query, parameters);

                if (etkilenenSatir > 0)
                {
                    LogInfo($"İşletme denetim kaydı eklendi. Vergi No: {txtVergiNo.Text}, Kullanıcı: {kayitKullanici}");
                    ShowToast("Denetim kaydı başarıyla eklendi.", "success");
                    FormuTemizle();
                }
                else
                {
                    ShowToast("Kayıt eklenirken bir hata oluştu.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("İşletme denetim kaydı eklenirken hata", ex);
                ShowToast("Kayıt eklenirken bir hata oluştu.", "danger");
            }
        }

        protected void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || KayitId == 0) return;

            try
            {
                string guncellemeKullanici = CurrentUserName ?? "Bilinmiyor";

                string query = @"
                    UPDATE denetimisletme 
                    SET VergiNo = @VergiNo, 
                        Unvan = @Unvan, 
                        Adres = @Adres, 
                        YetkiBelgesi = @YetkiBelgesi, 
                        DenetimTuru = @DenetimTuru, 
                        DenetimTarihi = @DenetimTarihi, 
                        il = @Il, 
                        ilce = @Ilce, 
                        Personel1 = @Personel1, 
                        Personel2 = @Personel2, 
                        CezaDurumu = @CezaDurumu, 
                        Aciklama = @Aciklama, 
                        GuncellemeTarihi = @GuncellemeTarihi, 
                        GuncellemeKullanici = @GuncellemeKullanici
                    WHERE id = @KayitId";

                var parameters = CreateParameters(
                    ("@VergiNo", txtVergiNo.Text),
                    ("@Unvan", txtUnvan.Text),
                    ("@Adres", txtAdres.Text),
                    ("@YetkiBelgesi", ddlYetkiBelgesi.SelectedValue),
                    ("@DenetimTuru", ddlDenetimTuru.SelectedValue),
                    ("@DenetimTarihi", ParseTarih(txtDenetimTarihi.Text)),
                    ("@Il", ddlIl.SelectedValue),
                    ("@Ilce", ddlIlce.SelectedValue),
                    ("@Personel1", ddlPersonel1.SelectedValue),
                    ("@Personel2", ddlPersonel2.SelectedValue),
                    ("@CezaDurumu", ddlCezaDurumu.SelectedValue),
                    ("@Aciklama", txtAciklama.Text),
                    ("@GuncellemeTarihi", DateTime.Now),
                    ("@GuncellemeKullanici", guncellemeKullanici),
                    ("@KayitId", KayitId)
                );

                int etkilenenSatir = ExecuteNonQuery(query, parameters);

                if (etkilenenSatir > 0)
                {
                    LogInfo($"İşletme denetim kaydı güncellendi. ID: {KayitId}, Kullanıcı: {guncellemeKullanici}");
                    ShowToast("Denetim kaydı başarıyla güncellendi.", "success");
                    FormuTemizle();
                    GuncellemModundanCik();
                }
                else
                {
                    ShowToast("Kayıt güncellenirken bir hata oluştu.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("İşletme denetim kaydı güncellenirken hata", ex);
                ShowToast("Kayıt güncellenirken bir hata oluştu.", "danger");
            }
        }

        protected void btnBul_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKayitNo.Text))
            {
                ShowToast("Lütfen kayıt numarası giriniz.", "warning");
                return;
            }

            try
            {
                string query = @"SELECT * FROM denetimisletme WHERE id = @KayitId";
                var parameters = CreateParameters(("@KayitId", txtKayitNo.Text));
                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    KayitId = Convert.ToInt32(row["id"]);
                    txtVergiNo.Text = row["VergiNo"].ToString();
                    txtUnvan.Text = row["Unvan"].ToString();
                    txtAdres.Text = row["Adres"].ToString();

                    // Yetki Belgesi
                    SetSafeDropDownValue(ddlYetkiBelgesi, row["YetkiBelgesi"].ToString());

                    // Denetim Türü
                    SetSafeDropDownValue(ddlDenetimTuru, row["DenetimTuru"].ToString());

                    // Denetim Tarihi
                    if (row["DenetimTarihi"] != DBNull.Value)
                    {
                        DateTime denetimTarihi = Convert.ToDateTime(row["DenetimTarihi"]);
                        txtDenetimTarihi.Text = denetimTarihi.ToString("dd.MM.yyyy HH: mm");
                    }

                    // İl ve İlçe
                    SetSafeDropDownValue(ddlIl, row["il"].ToString());
                    IlceleriYukle(ddlIl.SelectedValue);
                    SetSafeDropDownValue(ddlIlce, row["ilce"].ToString());

                    // Personeller
                    SetSafeDropDownValue(ddlPersonel1, row["Personel1"].ToString());
                    SetSafeDropDownValue(ddlPersonel2, row["Personel2"].ToString());

                    // Ceza Durumu
                    SetSafeDropDownValue(ddlCezaDurumu, row["CezaDurumu"].ToString());

                    txtAciklama.Text = row["Aciklama"].ToString();

                    GuncellemModunaGec();
                    ShowToast("Kayıt bulundu ve yüklendi.", "info");
                    LogInfo($"Kayıt bulundu. ID: {KayitId}");
                }
                else
                {
                    ShowToast("Kayıt bulunamadı.", "warning");
                }
            }
            catch (Exception ex)
            {
                LogError("Kayıt aranırken hata", ex);
                ShowToast("Kayıt aranırken bir hata oluştu.", "danger");
            }
        }

        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            FormuTemizle();
            GuncellemModundanCik();
            ShowToast("İşlem iptal edildi.", "info");
        }

        protected void btnSil_Click(object sender, EventArgs e)
        {
            if (KayitId == 0)
            {
                ShowToast("Silinecek kayıt bulunamadı.", "warning");
                return;
            }

            try
            {
                string query = @"DELETE FROM denetimisletme WHERE id = @KayitId";
                var parameters = CreateParameters(("@KayitId", KayitId));

                int etkilenenSatir = ExecuteNonQuery(query, parameters);

                if (etkilenenSatir > 0)
                {
                    string kullanici = CurrentUserName ?? "Bilinmiyor";
                    LogInfo($"İşletme denetim kaydı silindi. ID: {KayitId}, Kullanıcı: {kullanici}");
                    ShowToast("Denetim kaydı başarıyla silindi.", "success");
                    FormuTemizle();
                    GuncellemModundanCik();
                }
                else
                {
                    ShowToast("Kayıt silinirken bir hata oluştu.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("İşletme denetim kaydı silinirken hata", ex);
                ShowToast("Kayıt silinirken bir hata oluştu.", "danger");
            }
        }

        protected void ddlIl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlIl.SelectedValue))
            {
                IlceleriYukle(ddlIl.SelectedValue);
                ddlIlce.Enabled = true;
            }
            else
            {
                ddlIlce.Items.Clear();
                ddlIlce.Items.Insert(0, new ListItem("İlçe Seçiniz", ""));
                ddlIlce.Enabled = false;
            }
        }

        #endregion

        #region Helper Methods


        private void FormuTemizle()
        {
            KayitId = 0;
            ClearFormControls(txtKayitNo, txtVergiNo, txtUnvan, txtAdres, txtDenetimTarihi, txtAciklama,
                             ddlYetkiBelgesi, ddlDenetimTuru, ddlIl, ddlPersonel1, ddlPersonel2, ddlCezaDurumu);

            // İlçe dropdown'u özel olarak temizle
            ddlIlce.Items.Clear();
            ddlIlce.Items.Insert(0, new ListItem("İlçe Seçiniz", ""));
            ddlIlce.Enabled = false;
        }

        private void GuncellemModunaGec()
        {
            btnKaydet.Visible = false;
            btnGuncelle.Visible = true;
            btnVazgec.Visible = true;
            txtKayitNo.ReadOnly = true;
        }

        private void GuncellemModundanCik()
        {
            btnKaydet.Visible = true;
            btnGuncelle.Visible = false;
            btnVazgec.Visible = false;
            btnSil.Visible = false;
            txtKayitNo.ReadOnly = false;

            SilmeYetkisiniKontrolEt();
        }

        #endregion
    }
}