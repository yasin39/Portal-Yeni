using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Portal.Base; // For BasePage, logging, DB utils

namespace Portal.ModulPersonel
{
    public partial class OgrenimEkle : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Yetki kontrolü: Personel modülü için 100 yetkisi gerekli
                if (!CheckPermission(Sabitler.Personel))
                {
                    return; // BasePage zaten redirect eder
                }

                btnOgrenimSil.Visible = false;
                imgPersonel.Visible = false;
            }
        }

        // Refactored common search method (TC or Sicil)
        protected void AramaYap(object sender, EventArgs e)
        {
            try
            {
                string aramaDegeri = string.Empty;
                string aramaAlani = string.Empty;

                if (sender == btnTcAra)
                {
                    aramaDegeri = txtTc.Text.Trim();
                    aramaAlani = Sabitler.TcKimlikNo;
                }
                else if (sender == btnSicilAra)
                {
                    aramaDegeri = txtSicil.Text.Trim();
                    aramaAlani = Sabitler.SicilNo;
                }

                if (string.IsNullOrEmpty(aramaDegeri))
                {
                    ShowError("Lütfen TC Kimlik No veya Sicil No giriniz.");
                    return;
                }

                // Parameterized query for search
                string query = $@"
                    SELECT SicilNo, TcKimlikNo, Adi, Soyad, Resim 
                    FROM personel 
                    WHERE {aramaAlani} = @AramaDegeri";

                var parameters = CreateParameters(("@AramaDegeri", aramaDegeri));

                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count == 0)
                {
                    ShowError("Personel bulunamadı. Lütfen bilgileri kontrol ediniz.");
                    return;
                }

                DataRow row = dt.Rows[0];
                txtSicil.Text = row[Sabitler.SicilNo].ToString();
                txtTc.Text = row[Sabitler.TcKimlikNo].ToString();
                lblAdSoyad.Text = $"{row[Sabitler.Adi]} {row[Sabitler.Soyad]}";
                imgPersonel.ImageUrl = row["Resim"].ToString();
                imgPersonel.Visible = true;

                OgrenimGetir(); // Load ogrenim records

                LogInfo($"Personel arama başarılı: {lblAdSoyad.Text} ({aramaDegeri})");
            }
            catch (Exception ex)
            {
                LogError("Arama hatası", ex);
                ShowError("Arama sırasında bir hata oluştu. Lütfen tekrar deneyiniz.");
            }
        }

        // Load ogrenim records for person (parameterized)
        private void OgrenimGetir()
        {
            if (string.IsNullOrEmpty(txtTc.Text))
                return;

            try
            {
                string query = @"
                    SELECT id, Ogr_Durumu, Okul, Bolum, Mezuniyet_Tarihi 
                    FROM personel_ogrenim 
                    WHERE TC_No = @TcNo 
                    ORDER BY Mezuniyet_Tarihi ASC";

                var parameters = CreateParameters(("@TcNo", txtTc.Text));

                DataTable dt = ExecuteDataTable(query, parameters);
                GridViewOgrenim.DataSource = dt;
                GridViewOgrenim.DataBind();
            }
            catch (Exception ex)
            {
                LogError("Öğrenim yükleme hatası", ex);
                ShowError("Öğrenim kayıtları yüklenirken hata oluştu.");
            }
        }

        protected void GridViewOgrenim_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int selectedId = Convert.ToInt32(GridViewOgrenim.SelectedDataKey.Value);
                GridViewRow selectedRow = GridViewOgrenim.SelectedRow;

                ddlOgrenimDurumu.SelectedValue = selectedRow.Cells[1].Text; // Ogr_Durumu
                txtOkul.Text = selectedRow.Cells[2].Text; // Okul
                txtBolum.Text = selectedRow.Cells[3].Text; // Bolum
                txtMezuniyetTarihi.Text = selectedRow.Cells[4].Text; // Tarih (format handled in BoundField)

                btnOgrenimSil.Visible = true;
                btnOgrenimSil.CommandArgument = selectedId.ToString(); // For delete

                LogInfo($"Öğrenim seçildi: ID {selectedId}");
            }
            catch (Exception ex)
            {
                LogError("Satır seçimi hatası", ex);
                ShowError("Seçim sırasında hata oluştu.");
            }
        }

        protected void EgitimEkle(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                string query = @"
                    INSERT INTO personel_ogrenim (TC_No, Ogr_Durumu, Okul, Bolum, Mezuniyet_Tarihi) 
                    VALUES (@TcNo, @OgrDurumu, @Okul, @Bolum, @MezuniyetTarihi)";

                var parameters = CreateParameters(
                    ("@TcNo", txtTc.Text),
                    ("@OgrDurumu", ddlOgrenimDurumu.SelectedValue),
                    ("@Okul", txtOkul.Text),
                    ("@Bolum", txtBolum.Text),
                    ("@MezuniyetTarihi", string.IsNullOrEmpty(txtMezuniyetTarihi.Text) ? (object)DBNull.Value : txtMezuniyetTarihi.Text)
                );

                ExecuteNonQuery(query, parameters);

                OgrenimGetir(); // Refresh grid
                ClearInputs();

                LogInfo($"Öğrenim eklendi: {ddlOgrenimDurumu.SelectedItem.Text} - {txtTc.Text}");
                ShowToast("Öğrenim kaydı başarıyla eklendi.", "success");                                
            }
            catch (Exception ex)
            {
                LogError("Öğrenim ekleme hatası", ex);
                ShowError("Kayıt eklenirken hata oluştu. Lütfen bilgileri kontrol ediniz.");
            }
        }

        protected void OgrenimSil(object sender, EventArgs e)
        {
            if (GridViewOgrenim.SelectedIndex < 0)
            {
                ShowError("Silinecek kayıt seçilmedi.");
                return;
            }

            try
            {
                int id = Convert.ToInt32(GridViewOgrenim.SelectedDataKey.Value);

                string query = "DELETE FROM personel_ogrenim WHERE id = @Id";

                var parameters = CreateParameters(("@Id", id));

                ExecuteNonQuery(query, parameters);

                OgrenimGetir(); // Refresh
                ClearInputs();
                btnOgrenimSil.Visible = false;

                LogInfo($"Öğrenim silindi: ID {id}");
                ShowToast("Öğrenim kaydı başarıyla silindi.", "success");                                
            }
            catch (Exception ex)
            {
                LogError("Öğrenim silme hatası", ex);
                ShowError("Kayıt silinirken hata oluştu.");
            }
        }

        // Helper: Validate inputs
        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(txtTc.Text))
            {
                ShowError("TC Kimlik No zorunludur.");
                return false;
            }
            if (string.IsNullOrEmpty(ddlOgrenimDurumu.SelectedValue) || string.IsNullOrEmpty(txtOkul.Text))
            {
                ShowError("Öğrenim Durumu ve Okul bilgileri zorunludur.");
                return false;
            }
            return true;
        }

        // Helper: Clear form after ops
        private void ClearInputs()
        {
            ddlOgrenimDurumu.SelectedIndex = 0;
            txtOkul.Text = string.Empty;
            txtBolum.Text = string.Empty;
            txtMezuniyetTarihi.Text = string.Empty;
        }        
        

        private void ShowSuccess(string message)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "success", $"alert('{message}');", true);
        }
    }
}