using Portal.Base;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.ModulCimer
{
    public partial class Biten : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(608))
                {
                    return;
                }

                Listele();
            }
        }

        private void Listele()
        {
            try
            {
                string query = "SELECT * FROM cimer_basvurular WHERE Onay_Durumu = '2' ORDER BY id DESC";

                DataTable dt = ExecuteDataTable(query);

                gvBitenBasvurular.DataSource = dt;
                gvBitenBasvurular.DataBind();

                // Süreç durum dropdown'unu doldur
                ddlSurecDurum.Items.Add(new ListItem("Evet", "Evet"));
                ddlSurecDurum.Items.Add(new ListItem("Hayır", "Hayır"));
            }
            catch (Exception ex)
            {
                LogError("Biten başvuruları listeleme hatası", ex);
                ShowErrorAndRedirect("Veriler yüklenirken bir hata oluştu. Lütfen yöneticiyle iletişime geçiniz.", "~/Anasayfa.aspx");
            }
        }

        private void EvrakGecmisi()
        {
            try
            {
                string basvuruId = txtBasvuruNo.Text.Trim();

                if (string.IsNullOrEmpty(basvuruId))
                {
                    ShowError("Başvuru No bulunamadı.");
                    return;
                }

                lblGecmisTable.Text = Helpers.BuildDocumentHistoryHtml(basvuruId);
                LogInfo($"Evrak geçmişi gösterildi: Başvuru {basvuruId}");
            }
            catch (Exception ex)
            {
                LogError("Evrak geçmişi yükleme hatası", ex);
                lblGecmisTable.Text = "<p class='text-center text-danger py-3'>Geçmiş yüklenirken hata oluştu.</p>";
            }
        }

        protected void gvBitenBasvurular_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow selectedRow = gvBitenBasvurular.SelectedRow;
                if (selectedRow == null) return;

                // Sütun index'lerine göre değerleri ata (mevcut yapıya göre)
                txtBasvuruNo.Text = GetGridViewCellTextSafe(selectedRow, 2); // Basvuru_No
                txtAdSoyad.Text = GetGridViewCellTextSafe(selectedRow, 8); // Adi_Soyadi
                txtFirma.Text = GetGridViewCellTextSafe(selectedRow, 13); // Sikayet_Edilen_Firma
                txtMevcutBekleme.Text = GetGridViewCellTextSafe(selectedRow, 20); // Bekleme_Durumu

                pnlHavaleBolumu.Visible = true;
                pnlGecmisBolumu.Visible = false;
            }
            catch (Exception ex)
            {
                LogError("Seçim hatası", ex);
                ShowError("Başvuru seçimi sırasında hata oluştu.");
            }
        }

        protected void btnEvrakGecmisi_Click(object sender, EventArgs e)
        {
            pnlGecmisBolumu.Visible = true;
            EvrakGecmisi();
        }

        protected void btnGecmisKapat_Click(object sender, EventArgs e)
        {
            pnlGecmisBolumu.Visible = false;
        }

        protected void btnKayitKapat_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                string basvuruNo = txtBasvuruNo.Text.Trim();
                string aciklama = txtAciklama.Text.Trim();
                string durum = ddlDurum.SelectedValue;
                string surecDurum = ddlSurecDurum.SelectedValue;

                if (string.IsNullOrEmpty(basvuruNo))
                {
                    ShowError("Başvuru No bulunamadı.");
                    return;
                }

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Hareket tablosuna ekle
                            Helpers.InsertCimerMovement(conn, transaction, basvuruNo, CurrentUserName,
                                CurrentUserName, aciklama, 2, "Cevap Verildi. Kayıt Kapatıldı.");

                            // Ana tabloyu güncelle
                            string updateQuery = @"
                                UPDATE cimer_basvurular 
                                SET Onay_Durumu = '3', Sonuc = @Sonuc, Bekleme_Durumu = @BeklemeDurumu 
                                WHERE Basvuru_No = @BasvuruNo";

                            var updateParams = CreateParameters(
                                ("@Sonuc", durum),
                                ("@BeklemeDurumu", surecDurum),
                                ("@BasvuruNo", basvuruNo)
                            );

                            ExecuteNonQueryWithTransaction(conn, transaction, updateQuery, updateParams);

                            transaction.Commit();

                            LogInfo($"CİMER kaydı kapatıldı: {basvuruNo} - Kullanıcı: {CurrentUserName}");
                            ShowSuccessAndRedirect("Kayıt başarıyla kapatıldı. CİMER portalını kontrol ediniz.", "~/Modul/Biten.aspx");
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Kayıt kapatma hatası", ex);
                ShowError("Kapatma işlemi sırasında hata oluştu. Lütfen tekrar deneyiniz.");
            }
        }

        protected void btnExcelAktar_Click(object sender, EventArgs e)
        {
            if (gvBitenBasvurular.Rows.Count == 0)
            {
                ShowError("Export edilecek veri bulunamadı.");
                return;
            }

            ExportGridViewToExcel(gvBitenBasvurular, "CimerBitenBasvurular.xls");
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // GridView export için gerekli
        }
    }
}