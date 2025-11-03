using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulYonetici
{
    public partial class Duyurular : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.DUYURU_YONETIMI))
                    return;

                LoadDuyurular();
                SetFormModeInsert(btnKaydet, btnGuncelle, btnSil, btnVazgec);
            }
        }

        private void LoadDuyurular()
        {
            try
            {
                string query = @"SELECT id, Baslama_Tarihi, Bitis_Tarihi, Dosya, Durum, Duyuru, 
                                Kullanici, Kayit_Tarihi 
                                FROM duyuru 
                                ORDER BY id DESC";

                DataTable dt = ExecuteDataTable(query);
                DuyurularGrid.DataSource = dt;
                DuyurularGrid.DataBind();
            }
            catch (Exception ex)
            {
                LogError("LoadDuyurular hatası", ex);
                ShowToast("Duyurular yüklenirken hata oluştu.", "error");
            }
        }

        protected void DuyurularGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow SeciliSatir = DuyurularGrid.SelectedRow;

                string DuyuruId = GetGridViewCellTextSafe(SeciliSatir, 0);
                string BaslamaTarihi = GetGridViewCellTextSafe(SeciliSatir, 1);
                string BitisTarihi = GetGridViewCellTextSafe(SeciliSatir, 2);
                string Durum = GetGridViewCellTextSafe(SeciliSatir, 3);
                string DuyuruMetni = GetGridViewCellTextSafe(SeciliSatir, 4);

                string query = "SELECT Dosya FROM duyuru WHERE id = @Id";
                var parameters = CreateParameters(("@Id", DuyuruId));
                DataTable dt = ExecuteDataTable(query, parameters);

                string MevcutDosya = dt.Rows.Count > 0 ? dt.Rows[0]["Dosya"]?.ToString() : string.Empty;

                if (!string.IsNullOrEmpty(BaslamaTarihi) && DateTime.TryParse(BaslamaTarihi, out DateTime baslama))
                    txtBaslamaTarihi.Text = FormatDateTimeHtml(baslama);

                if (!string.IsNullOrEmpty(BitisTarihi) && DateTime.TryParse(BitisTarihi, out DateTime bitis))
                    txtBitisTarihi.Text = FormatDateTimeHtml(bitis);

                ddlDurum.SelectedValue = Durum;
                txtDuyuru.Text = DuyuruMetni;
                hfMevcutDosya.Value = MevcutDosya;

                SetFormModeUpdate(btnKaydet, btnGuncelle, btnSil, btnVazgec);
            }
            catch (Exception ex)
            {
                LogError("DuyurularGrid_SelectedIndexChanged hatası", ex);
                ShowToast("Kayıt seçilirken hata oluştu.", "error");
            }
        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                string DosyaYolu = HandleFileUpload();

                string query = @"INSERT INTO duyuru 
                                (Baslama_Tarihi, Bitis_Tarihi, Dosya, Durum, Duyuru, Kullanici, Kayit_Tarihi) 
                                VALUES (@BaslamaTarihi, @BitisTarihi, @Dosya, @Durum, @Duyuru, @Kullanici, @KayitTarihi)";

                var parameters = CreateParameters(
                    ("@BaslamaTarihi", txtBaslamaTarihi.Text),
                    ("@BitisTarihi", txtBitisTarihi.Text),
                    ("@Dosya", DosyaYolu),
                    ("@Durum", ddlDurum.SelectedValue),
                    ("@Duyuru", txtDuyuru.Text.Trim()),
                    ("@Kullanici", CurrentUserName),
                    ("@KayitTarihi", DateTime.Now)
                );

                ExecuteNonQuery(query, parameters);

                ShowToast("Duyuru başarıyla kaydedildi.", "success");
                ClearForm();
                LoadDuyurular();
            }
            catch (Exception ex)
            {
                LogError("btnKaydet_Click hatası", ex);
                ShowToast("Kayıt sırasında hata oluştu.", "error");
            }
        }

        protected void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || DuyurularGrid.SelectedIndex == -1)
                return;

            try
            {
                string DuyuruId = GetGridViewCellTextSafe(DuyurularGrid.SelectedRow, 0);
                string DosyaYolu = HandleFileUpload();

                if (string.IsNullOrEmpty(DosyaYolu))
                    DosyaYolu = hfMevcutDosya.Value;

                string query = @"UPDATE duyuru SET 
                                Baslama_Tarihi = @BaslamaTarihi,
                                Bitis_Tarihi = @BitisTarihi,
                                Dosya = @Dosya,
                                Durum = @Durum,
                                Duyuru = @Duyuru,
                                Guncelleme_Tarihi = @GuncellemeTarihi,
                                Guncelleyen_Kullanici = @GuncelleyenKullanici
                                WHERE id = @Id";

                var parameters = CreateParameters(
                    ("@BaslamaTarihi", txtBaslamaTarihi.Text),
                    ("@BitisTarihi", txtBitisTarihi.Text),
                    ("@Dosya", DosyaYolu),
                    ("@Durum", ddlDurum.SelectedValue),
                    ("@Duyuru", txtDuyuru.Text.Trim()),
                    ("@GuncellemeTarihi", DateTime.Now),
                    ("@GuncelleyenKullanici", CurrentUserName),
                    ("@Id", DuyuruId)
                );

                ExecuteNonQuery(query, parameters);

                ShowToast("Duyuru başarıyla güncellendi.", "success");
                ClearForm();
                LoadDuyurular();
                SetFormModeInsert(btnKaydet, btnGuncelle, btnSil, btnVazgec);
            }
            catch (Exception ex)
            {
                LogError("btnGuncelle_Click hatası", ex);
                ShowToast("Güncelleme sırasında hata oluştu.", "error");
            }
        }

        protected void btnSil_Click(object sender, EventArgs e)
        {
            if (DuyurularGrid.SelectedIndex == -1)
                return;

            try
            {
                string DuyuruId = GetGridViewCellTextSafe(DuyurularGrid.SelectedRow, 0);

                string query = "DELETE FROM duyuru WHERE id = @Id";
                var parameters = CreateParameters(("@Id", DuyuruId));

                ExecuteNonQuery(query, parameters);

                ShowToast("Duyuru başarıyla silindi.", "success");
                ClearForm();
                LoadDuyurular();
                SetFormModeInsert(btnKaydet, btnGuncelle, btnSil, btnVazgec);
            }
            catch (Exception ex)
            {
                LogError("btnSil_Click hatası", ex);
                ShowToast("Silme işlemi sırasında hata oluştu.", "error");
            }
        }

        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetFormModeInsert(btnKaydet, btnGuncelle, btnSil, btnVazgec);
            DuyurularGrid.SelectedIndex = -1;
        }

        private string HandleFileUpload()
        {
            if (!fuDosya.HasFile)
                return string.Empty;

            try
            {
                string KlasorAdi = DateTime.Now.ToString("dd-MM-yyyy");
                string KlasorYolu = Server.MapPath($"~/duyuru/{KlasorAdi}");

                if (!Directory.Exists(KlasorYolu))
                    Directory.CreateDirectory(KlasorYolu);

                string DosyaAdi = $"{DateTime.Now:HHmmss}_{fuDosya.FileName}";
                string TamYol = Path.Combine(KlasorYolu, DosyaAdi);

                fuDosya.SaveAs(TamYol);

                return $"~/duyuru/{KlasorAdi}/{DosyaAdi}";
            }
            catch (Exception ex)
            {
                LogError("HandleFileUpload hatası", ex);
                ShowToast("Dosya yüklenirken hata oluştu.", "error");
                return string.Empty;
            }
        }

        private void ClearForm()
        {
            ClearFormControls(txtBaslamaTarihi, txtBitisTarihi, txtDuyuru, ddlDurum);
            hfMevcutDosya.Value = string.Empty;
        }
    }
}