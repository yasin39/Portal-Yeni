using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulCimer
{
    public partial class Cevapladiklarim : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.CIMER_PERSONEL))
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
                string query = @"
                    SELECT * FROM cimer_basvurular 
                    WHERE Guncelleyen_Kullanici = @KullaniciAdi 
                    ORDER BY id DESC";

                var parameters = CreateParameters(
                    ("@KullaniciAdi", CurrentUserName)
                );

                DataTable dt = ExecuteDataTable(query, parameters);

                GridView1.DataSource = dt;
                GridView1.DataBind();

                LogInfo($"Cevapladıklarım listesi yüklendi. Kullanıcı: {CurrentUserName}, Kayıt sayısı: {dt.Rows.Count}");
            }
            catch (Exception ex)
            {
                LogError("Listeleme hatası", ex);
                ShowErrorAndRedirect("Veri yüklenirken bir hata oluştu. Lütfen yöneticiyle iletişime geçiniz.", "~/Anasayfa.aspx");
            }
        }

        private void EvrakGecmis()
        {
            try
            {
                if (GridView1.SelectedRow == null)
                {
                    gecmisTableContainer.InnerHtml = "<p class='text-danger'>Lütfen bir başvuru seçin.</p>";
                    return;
                }

                string basvuruNo = GridView1.SelectedRow.Cells[2].Text; // 2. sütun (Basvuru_No)

                gecmisTableContainer.InnerHtml = Helpers.BuildDocumentHistoryHtml(basvuruNo);
                LogInfo($"Evrak geçmişi gösterildi: Başvuru {basvuruNo}");
            }
            catch (Exception ex)
            {
                LogError("Evrak geçmişi yükleme hatası", ex);
                gecmisTableContainer.InnerHtml = "<p class='text-danger'>Evrak geçmişi yüklenirken bir hata oluştu.</p>";
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            gecmisBolumu.Visible = true;
            EvrakGecmis();
            LogInfo($"Evrak geçmişi görüntülendi. Başvuru ID: {GridView1.SelectedDataKey.Value}");
        }

        protected void GecmisKapat_Click(object sender, EventArgs e)
        {
            gecmisBolumu.Visible = false;
        }

        protected void ExcelAktar_Click(object sender, EventArgs e)
        {
            ExportGridViewToExcel(GridView1, "CimerCevapladiklarim.xls");
            LogInfo($"Excel export tamamlandı. Kullanıcı: {CurrentUserName}");
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // GridView'i form dışında render etmek için gerekli
        }
    }
}