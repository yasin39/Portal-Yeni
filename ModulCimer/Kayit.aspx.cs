using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using Portal.Base;

namespace Portal.ModulCimer
{
    public partial class Kayit : BasePage
    {
        private const int MaxFileSizeBytes = 10 * 1024 * 1024; // 10MB
        private const string AllowedExtensions = ".pdf,.doc,.docx,.jpg,.jpeg,.png,.zip";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.CIMER_KAYIT))
                {
                    return; // BasePage redirects on failure
                }

                PopulateDropdowns();
                pnlMessage.Visible = false;
            }
        }

        private void PopulateDropdowns()
        {
            // Complaint Types (static)
            ddlComplaintType.Items.Clear();
            ddlComplaintType.Items.Add(new System.Web.UI.WebControls.ListItem("Yolcu Taşımacılığı", "Yolcu Taşımacılığı"));
            ddlComplaintType.Items.Add(new System.Web.UI.WebControls.ListItem("Eşya Taşımacılığı", "Eşya Taşımacılığı"));
            ddlComplaintType.Items.Add(new System.Web.UI.WebControls.ListItem("Tehlikeli Maddeler", "Tehlikeli Maddeler"));
            ddlComplaintType.Items.Add(new System.Web.UI.WebControls.ListItem("İdari İşler", "İdari İşler"));
            ddlComplaintType.Items.Add(new System.Web.UI.WebControls.ListItem("Diğer", "Diğer"));

            // Companies from cimer_firmalar
            string companyQuery = "SELECT Firma_Unvan FROM cimer_firmalar ORDER BY Firma_Unvan ASC";
            PopulateDropDownList(ddlCompany, companyQuery, "Firma_Unvan", "Firma_Unvan", false);

            // Users from kullanici (Active, Personel_Tipi=1)
            string userQuery = "SELECT Adi_Soyadi FROM kullanici WHERE Durum = 'Aktif' AND Personel_Tipi = '1' ORDER BY Adi_Soyadi ASC";
            PopulateDropDownList(ddlAssignedUser, userQuery, "Adi_Soyadi", "Adi_Soyadi", false);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtApplicationNumber.Text))
            {
                ShowToast("Başvuru No giriniz.", "warning");
                return;
            }

            string query = "SELECT * FROM cimer_basvurular WHERE Basvuru_No = @BasvuruNo";
            var parameters = CreateParameters(("@BasvuruNo", Convert.ToInt64(txtApplicationNumber.Text)));

            try
            {
                DataTable dt = ExecuteDataTable(query, parameters);
                if (dt.Rows.Count == 0)
                {
                    ShowToast("Başvuru bulunamadı.", "info");
                    return;
                }

                DataRow row = dt.Rows[0];
                hfId.Value = row["id"].ToString();
                txtApplicationDate.Text = row["Basvuru_Tarihi"]?.ToString() ?? "";
                txtTcNumber.Text = row["TC_No"]?.ToString() ?? "";
                txtFullName.Text = row["Adi_Soyadi"]?.ToString() ?? "";
                txtEmail.Text = row["Mail"]?.ToString() ?? "";
                txtPhone.Text = row["Tel_No"]?.ToString() ?? "";
                txtAddress.Text = row["Adres"]?.ToString() ?? "";
                txtApplicationText.Text = row["Basvuru_Metni"]?.ToString() ?? "";
                hfAttachmentPath.Value = row["Basvuru_Ek"]?.ToString() ?? "";

                // Güvenli dropdown atamaları: Değer listede yoksa varsayılan ata
                string sikayet = row["Sikayet_Konusu"]?.ToString() ?? "";
                SetSafeDropDownValue(ddlComplaintType, sikayet);

                string firma = row["Sikayet_Edilen_Firma"]?.ToString() ?? "";
                SetSafeDropDownValue(ddlCompany, firma);

                string kullanici = row["Son_Kullanici"]?.ToString() ?? "";
                SetSafeDropDownValue(ddlAssignedUser, kullanici);

                string sonuc = row["Sonuc"]?.ToString() ?? "";
                SetSafeDropDownValue(ddlStatus, sonuc);

                string onayDurumu = row["Onay_Durumu"]?.ToString() ?? "";
                SetSafeDropDownValue(ddlApprovalStatus, onayDurumu);

                btnUpdate.Visible = true;
                btnCancel.Visible = true;
                btnSave.Visible = false;

                ShowToast("Başvuru bilgileri yüklendi.", "success");
                LogInfo($"Başvuru arama başarılı: {txtApplicationNumber.Text} - Kullanıcı: {CurrentUserName}");
            }
            catch (Exception ex)
            {
                LogError("Başvuru arama hatası", ex);
                ShowToast("Arama sırasında hata oluştu.", "danger");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            if (!HandleFileUpload(txtApplicationNumber.Text)) return;

            string query = @"
                INSERT INTO cimer_basvurular (
                    Basvuru_No, Basvuru_Tarihi, TC_No, Adi_Soyadi, Tel_No, Mail, Adres, Basvuru_Metni, 
                    Basvuru_Ek, Sikayet_Konusu, Sikayet_Edilen_Firma, Son_Kullanici, Sonuc, Onay_Durumu, 
                    Kayit_Kullanici, Kayit_Tarihi
                ) VALUES (
                    @BasvuruNo, @BasvuruTarihi, @TcNo, @AdiSoyadi, @TelNo, @Mail, @Adres, @BasvuruMetni, 
                    @BasvuruEk, @SikayetKonusu, @SikayetFirma, @SonKullanici, @Sonuc, @OnayDurumu, 
                    @KayitKullanici, @KayitTarihi
                )";

            var parameters = CreateParameters(
                ("@BasvuruNo", Convert.ToInt64(txtApplicationNumber.Text)),
                ("@BasvuruTarihi", DateTime.Parse(txtApplicationDate.Text)),
                ("@TcNo", string.IsNullOrEmpty(txtTcNumber.Text) ? (object)DBNull.Value : Convert.ToInt64(txtTcNumber.Text)),
                ("@AdiSoyadi", txtFullName.Text),
                ("@TelNo", txtPhone.Text),
                ("@Mail", txtEmail.Text),
                ("@Adres", txtAddress.Text),
                ("@BasvuruMetni", txtApplicationText.Text),
                ("@BasvuruEk", hfAttachmentPath.Value),
                ("@SikayetKonusu", ddlComplaintType.SelectedValue),
                ("@SikayetFirma", ddlCompany.SelectedValue),
                ("@SonKullanici", ddlAssignedUser.SelectedValue),
                ("@Sonuc", ddlStatus.SelectedValue),
                ("@OnayDurumu", ddlApprovalStatus.SelectedValue),
                ("@KayitKullanici", CurrentUserName),
                ("@KayitTarihi", DateTime.Now)
            );

            try
            {
                using (var connection = GetOpenConnection())
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        ExecuteNonQueryWithTransaction(connection, transaction, query, parameters);

                        // Insert into hareketleri
                        Helpers.InsertCimerMovement(connection, transaction, txtApplicationNumber.Text, CurrentUserName,
                            ddlAssignedUser.SelectedValue, "", 0, "Yeni başvuru kaydedildi.");
                        transaction.Commit();

                        ShowToast("Başvuru başarıyla kaydedildi.", "success");
                        LogInfo($"Yeni CİMER kaydı: {txtApplicationNumber.Text} - {CurrentUserName}");
                        ClearForm();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Kaydetme hatası", ex);
                ShowToast("Kaydetme sırasında hata oluştu.", "danger");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || string.IsNullOrEmpty(hfId.Value)) return;

            if (!HandleFileUpload(txtApplicationNumber.Text)) return;

            string query = @"
                UPDATE cimer_basvurular SET 
                    Basvuru_Tarihi = @BasvuruTarihi, TC_No = @TcNo, Adi_Soyadi = @AdiSoyadi, Tel_No = @TelNo, 
                    Mail = @Mail, Adres = @Adres, Basvuru_Metni = @BasvuruMetni, Basvuru_Ek = @BasvuruEk, 
                    Sikayet_Konusu = @SikayetKonusu, Sikayet_Edilen_Firma = @SikayetFirma, Son_Kullanici = @SonKullanici, 
                    Sonuc = @Sonuc, Onay_Durumu = @OnayDurumu, Guncelleme_Tarihi = @GuncellemeTarihi, 
                    Guncelleyen_Kullanici = @GuncelleyenKullanici
                WHERE id = @Id";

            var parameters = CreateParameters(
                ("@Id", Convert.ToInt32(hfId.Value)),
                ("@BasvuruTarihi", DateTime.Parse(txtApplicationDate.Text)),
                ("@TcNo", string.IsNullOrEmpty(txtTcNumber.Text) ? (object)DBNull.Value : Convert.ToInt64(txtTcNumber.Text)),
                ("@AdiSoyadi", txtFullName.Text),
                ("@TelNo", txtPhone.Text),
                ("@Mail", txtEmail.Text),
                ("@Adres", txtAddress.Text),
                ("@BasvuruMetni", txtApplicationText.Text),
                ("@BasvuruEk", hfAttachmentPath.Value),
                ("@SikayetKonusu", ddlComplaintType.SelectedValue),
                ("@SikayetFirma", ddlCompany.SelectedValue),
                ("@SonKullanici", ddlAssignedUser.SelectedValue),
                ("@Sonuc", ddlStatus.SelectedValue),
                ("@OnayDurumu", ddlApprovalStatus.SelectedValue),
                ("@GuncellemeTarihi", DateTime.Now),
                ("@GuncelleyenKullanici", CurrentUserName)
            );

            try
            {
                using (var connection = GetOpenConnection())
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        ExecuteNonQueryWithTransaction(connection, transaction, query, parameters);

                        // Insert into hareketleri
                        Helpers.InsertCimerMovement(connection, transaction, txtApplicationNumber.Text, CurrentUserName,
                            ddlAssignedUser.SelectedValue, "", 0, "Başvuru güncellendi.");
                        transaction.Commit();

                        ShowToast("Başvuru başarıyla güncellendi.", "success");
                        LogInfo($"CİMER güncelleme: {txtApplicationNumber.Text} - {CurrentUserName}");
                        ClearForm();
                        btnUpdate.Visible = false;
                        btnCancel.Visible = false;
                        btnSave.Visible = true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Güncelleme hatası", ex);
                ShowToast("Güncelleme sırasında hata oluştu.", "danger");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            btnUpdate.Visible = false;
            btnCancel.Visible = false;
            btnSave.Visible = true;
            ShowToast("İşlem iptal edildi.", "info");
        }

        private bool HandleFileUpload(string applicationNumber)
        {
            if (!fuAttachment.HasFile) return true;

            // Validation
            if (fuAttachment.PostedFile.ContentLength > MaxFileSizeBytes)
            {
                ShowToast("Dosya boyutu 10MB'ı aşamaz.", "danger");
                return false;
            }

            string extension = Path.GetExtension(fuAttachment.FileName).ToLower();
            if (!AllowedExtensions.Contains(extension))
            {
                ShowToast("Desteklenmeyen dosya türü.", "danger");
                return false;
            }

            string folderPath = Server.MapPath($"~/cimer/{applicationNumber}");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = $"{applicationNumber}_{Path.GetFileNameWithoutExtension(fuAttachment.FileName)}{extension}";
            string fullPath = Path.Combine(folderPath, fileName);
            hfAttachmentPath.Value = $"~/cimer/{applicationNumber}/{fileName}";

            try
            {
                fuAttachment.SaveAs(fullPath);
                LogInfo($"Dosya yüklendi: {fileName}");
                return true;
            }
            catch (Exception ex)
            {
                LogError("Dosya yükleme hatası", ex);
                ShowToast("Dosya yüklenemedi.", "danger");
                return false;
            }
        }

        private void ClearForm()
        {
            ClearFormControls(txtApplicationNumber, txtApplicationDate, txtTcNumber, txtFullName,
                             txtEmail, txtPhone, txtAddress, txtApplicationText,
                             ddlComplaintType, ddlCompany, ddlAssignedUser, ddlStatus, ddlApprovalStatus);
            hfAttachmentPath.Value = "";
            hfId.Value = "";
        }

    }
}