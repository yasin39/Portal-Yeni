using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Portal;
using Portal.Base;


namespace ModulDenetim
{
    public partial class TasitGiris : BasePage
    {
        private int _kayitId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPermission(Sabitler.DENETIM_TASIT_GIRIS);

                YetkiKontrolSilmeButonu();
                DropDownDoldur();
            }
        }

        private void YetkiKontrolSilmeButonu()
        {
            string sicilNo = Session["Sicil"]?.ToString();
            if (string.IsNullOrEmpty(sicilNo)) return;

            string query = "SELECT COUNT(*) FROM yetki WHERE Sicil_No = @SicilNo AND Yetki_No = @YetkiNo";
            var parameters = CreateParameters(
                ("@SicilNo", sicilNo),
                ("@YetkiNo", Sabitler.DENETIM_TASIT_SILME)
            );

            int yetkiSayisi = Convert.ToInt32(ExecuteScalar(query, parameters));
            btnSil.Visible = (yetkiSayisi > 0);
        }

        private void DropDownDoldur()
        {
            ddlPersonel.Items.Clear();
            string personelQuery = @"
                SELECT Adi + ' ' + Soyad AS Kisi 
                FROM personel 
                WHERE Durum = 'Aktif' 
                  AND CalismaDurumu != 'Geçici Görevde Pasif Çalışan' 
                  AND Statu = 'Memur' 
                ORDER BY Adi ASC";
            PopulateDropDownList(ddlPersonel, personelQuery, "Kisi", "Kisi", true);

            Helpers.LoadProvinces(ddlIl, "Seçiniz...");

            ddlYetkiBelgesi.Items.Clear();
            string yetkiBelgesiQuery = "SELECT Belge_Adi FROM yetki_belgeleri ORDER BY Belge_Adi ASC";
            PopulateDropDownList(ddlYetkiBelgesi, yetkiBelgesiQuery, "Belge_Adi", "Belge_Adi", true);

            ddlDenetimYeri.Items.Clear();
            string denetimYeriQuery = "SELECT DenetimYeri FROM denetimyerleri ORDER BY DenetimYeri ASC";
            PopulateDropDownList(ddlDenetimYeri, denetimYeriQuery, "DenetimYeri", "DenetimYeri", true);
        }

        protected void ddlIl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlIl.SelectedValue))
            {
                ddlIlce.Items.Clear();
                ddlIlce.Items.Insert(0, new ListItem("Seçiniz...", ""));
                return;
            }

            Helpers.LoadDistricts(ddlIlce, ddlIl.SelectedValue, "Seçiniz...");
        }

        protected void btnBul_Click(object sender, EventArgs e)
        {
            lblBulunanKayit.Text = "";
            lblMesaj.Text = "";

            if (string.IsNullOrEmpty(txtKayitNo.Text))
            {
                ShowToast("Lütfen kayıt numarası giriniz.", "warning");
                return;
            }

            string query = "SELECT * FROM denetimtasit WHERE id = @Id";
            var parameters = CreateParameters(("@Id", txtKayitNo.Text));
            DataTable dt = ExecuteDataTable(query, parameters);

            if (dt.Rows.Count == 0)
            {
                lblBulunanKayit.Text = "Aranan kayıt bulunamadı.";
                return;
            }

            DataRow row = dt.Rows[0];
            _kayitId = Convert.ToInt32(row["id"]);

            txtPlaka.Text = row["Plaka"].ToString();
            txtPlaka2.Text = row["Plaka2"].ToString();
            txtUnvan.Text = row["Unvan"].ToString();
            SetSafeDropDownValue(ddlDenetimYeri, row["DenetimYeri"].ToString());
            SetSafeDropDownValue(ddlYetkiBelgesi, row["YetkiBelgesi"].ToString());
            SetSafeDropDownValue(ddlDenetimTuru, row["DenetimTuru"].ToString());
            txtDenetimTarihi.Text = Convert.ToDateTime(row["DenetimTarihi"]).ToString("yyyy-MM-ddTHH:mm");
            SetSafeDropDownValue(ddlIl, row["il"].ToString());

            ddlIl_SelectedIndexChanged(null, null);
            SetSafeDropDownValue(ddlIlce, row["ilce"].ToString());

            SetSafeDropDownValue(ddlPersonel, row["Personel1"].ToString());
            SetSafeDropDownValue(ddlCezaDurumu, row["CezaDurumu"].ToString());
            txtAciklama.Text = row["Aciklama"].ToString();

            txtKayitNo.ReadOnly = true;
            txtUnvan.ReadOnly = true;
            btnKaydet.Visible = false;
            btnGuncelle.Visible = true;
            btnVazgec.Visible = true;

            if (btnSil.Visible)
            {
                btnSil.Visible = true;
            }

            ShowToast("Kayıt başarıyla yüklendi.", "info");
        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            lblMesaj.Text = "";

            if (KayitVarMi())
            {
                lblMesaj.Text = "Aynı tarihte aynı plaka ile denetim kaydı zaten mevcut.";
                ShowToast("Aynı tarihte aynı plaka ile denetim kaydı zaten mevcut.", "warning");
                return;
            }

            string query = @"
                INSERT INTO denetimtasit 
                (Plaka, Plaka2, Unvan, DenetimYeri, YetkiBelgesi, DenetimTuru, 
                 DenetimTarihi, il, ilce, Personel1, CezaDurumu, Aciklama, 
                 KayitTarihi, KayitKullanici)
                VALUES 
                (@Plaka, @Plaka2, @Unvan, @DenetimYeri, @YetkiBelgesi, @DenetimTuru, 
                 @DenetimTarihi, @Il, @Ilce, @Personel, @CezaDurumu, @Aciklama, 
                 @KayitTarihi, @KayitKullanici)";

            var parameters = CreateParameters(
                ("@Plaka", txtPlaka.Text.ToUpper().Trim()),
                ("@Plaka2", txtPlaka2.Text.ToUpper().Trim()),
                ("@Unvan", txtUnvan.Text.Trim()),
                ("@DenetimYeri", ddlDenetimYeri.SelectedValue),
                ("@YetkiBelgesi", ddlYetkiBelgesi.SelectedValue),
                ("@DenetimTuru", ddlDenetimTuru.SelectedValue),
                ("@DenetimTarihi", ParseTarih(txtDenetimTarihi.Text)),
                ("@Il", ddlIl.SelectedValue),
                ("@Ilce", ddlIlce.SelectedValue),
                ("@Personel", ddlPersonel.SelectedValue),
                ("@CezaDurumu", ddlCezaDurumu.SelectedValue),
                ("@Aciklama", txtAciklama.Text.Trim()),
                ("@KayitTarihi", DateTime.Now),
                ("@KayitKullanici", CurrentUserName ?? "")
            );

            try
            {
                ExecuteNonQuery(query, parameters);
                ShowToast("Denetim kaydı başarıyla eklendi.", "success");
                LogInfo($"Taşıt denetim kaydı eklendi: {txtPlaka.Text}");
                FormuTemizle();
            }
            catch (Exception ex)
            {
                LogError("Taşıt denetim kayıt hatası", ex);
                ShowToast("Kayıt sırasında bir hata oluştu.", "danger");
            }
        }

        protected void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            lblMesaj.Text = "";

            string kontrolQuery = @"
                SELECT COUNT(*) FROM denetimtasit 
                WHERE Plaka = @Plaka 
                  AND DenetimTarihi = @DenetimTarihi 
                  AND id != @Id";

            var kontrolParams = CreateParameters(
                ("@Plaka", txtPlaka.Text.ToUpper().Trim()),
               ("@DenetimTarihi", ParseTarih(txtDenetimTarihi.Text)),
                ("@Id", txtKayitNo.Text)
            );

            int kayitSayisi = Convert.ToInt32(ExecuteScalar(kontrolQuery, kontrolParams));

            if (kayitSayisi > 0)
            {
                lblMesaj.Text = "Aynı tarihte aynı plaka ile denetim kaydı zaten mevcut.";
                ShowToast("Aynı tarihte aynı plaka ile denetim kaydı zaten mevcut.", "warning");
                return;
            }

            string query = @"
                UPDATE denetimtasit 
                SET Plaka = @Plaka, Plaka2 = @Plaka2, Unvan = @Unvan, 
                    DenetimYeri = @DenetimYeri, YetkiBelgesi = @YetkiBelgesi, 
                    DenetimTuru = @DenetimTuru, DenetimTarihi = @DenetimTarihi, 
                    il = @Il, ilce = @Ilce, Personel1 = @Personel, 
                    CezaDurumu = @CezaDurumu, Aciklama = @Aciklama, 
                    GuncellemeTarihi = @GuncellemeTarihi, 
                    GuncellemeKullanici = @GuncellemeKullanici
                WHERE id = @Id";

            var parameters = CreateParameters(
                ("@Plaka", txtPlaka.Text.ToUpper().Trim()),
                ("@Plaka2", txtPlaka2.Text.ToUpper().Trim()),
                ("@Unvan", txtUnvan.Text.Trim()),
                ("@DenetimYeri", ddlDenetimYeri.SelectedValue),
                ("@YetkiBelgesi", ddlYetkiBelgesi.SelectedValue),
                ("@DenetimTuru", ddlDenetimTuru.SelectedValue),
                ("@DenetimTarihi", ParseTarih(txtDenetimTarihi.Text)),
                ("@Il", ddlIl.SelectedValue),
                ("@Ilce", ddlIlce.SelectedValue),
                ("@Personel", ddlPersonel.SelectedValue),
                ("@CezaDurumu", ddlCezaDurumu.SelectedValue),
                ("@Aciklama", txtAciklama.Text.Trim()),
                ("@GuncellemeTarihi", DateTime.Now),
                ("@GuncellemeKullanici", CurrentUserName ?? ""),
                ("@Id", txtKayitNo.Text)
            );

            try
            {
                ExecuteNonQuery(query, parameters);
                ShowToast("Denetim kaydı başarıyla güncellendi.", "success");
                LogInfo($"Taşıt denetim kaydı güncellendi: {txtPlaka.Text}");
                FormuTemizle();
            }
            catch (Exception ex)
            {
                LogError("Taşıt denetim güncelleme hatası", ex);
                ShowToast("Güncelleme sırasında bir hata oluştu.", "danger");
            }
        }

        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            FormuTemizle();
            ShowToast("İşlemden vazgeçildi.", "info");
        }

        protected void btnSil_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKayitNo.Text))
            {
                ShowToast("Silinecek kayıt bulunamadı.", "warning");
                return;
            }

            string query = "DELETE FROM denetimtasit WHERE id = @Id";
            var parameters = CreateParameters(("@Id", txtKayitNo.Text));

            try
            {
                ExecuteNonQuery(query, parameters);
                ShowToast("Denetim kaydı başarıyla silindi.", "success");
                LogInfo($"Taşıt denetim kaydı silindi: ID={txtKayitNo.Text}");
                FormuTemizle();
            }
            catch (Exception ex)
            {
                LogError("Taşıt denetim silme hatası", ex);
                ShowToast("Silme işlemi sırasında bir hata oluştu.", "danger");
            }
        }

        private bool KayitVarMi()
        {
            string query = @"
                SELECT COUNT(*) FROM denetimtasit 
                WHERE Plaka = @Plaka AND DenetimTarihi = @DenetimTarihi";

            var parameters = CreateParameters(
                ("@Plaka", txtPlaka.Text.ToUpper().Trim()),
                ("@DenetimTarihi", ParseTarih(txtDenetimTarihi.Text))
            );

            int kayitSayisi = Convert.ToInt32(ExecuteScalar(query, parameters));
            return kayitSayisi > 0;
        }

        private void FormuTemizle()
        {
            ClearFormControls(txtKayitNo, txtPlaka, txtPlaka2, txtUnvan, txtDenetimTarihi, txtAciklama,
                             ddlDenetimYeri, ddlYetkiBelgesi, ddlDenetimTuru, ddlIl, ddlPersonel, ddlCezaDurumu);
            lblBulunanKayit.Text = "";
            lblMesaj.Text = "";

            ddlIlce.Items.Clear();
            ddlIlce.Items.Insert(0, new ListItem("Seçiniz...", ""));

            txtKayitNo.ReadOnly = false;
            txtUnvan.ReadOnly = false;

            btnKaydet.Visible = true;
            btnGuncelle.Visible = false;
            btnVazgec.Visible = false;
            btnSil.Visible = false;

            YetkiKontrolSilmeButonu();
        }
    }
}