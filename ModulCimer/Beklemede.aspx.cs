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
    public partial class Beklemede : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.CIMER_DEVAMEDEN_BASVURU))
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
                    WHERE Bekleme_Durumu = 'Evet' 
                    ORDER BY id DESC";

                DataTable dt = ExecuteDataTable(query);

                GridView1.DataSource = dt;
                GridView1.DataBind();

                if (dt.Rows.Count > 0)
                {
                    GridView1.FooterRow.Cells[0].Text = "Toplam: " + dt.Rows.Count.ToString();
                    GridView1.FooterRow.HorizontalAlign = HorizontalAlign.Right;
                }
            }
            catch (Exception ex)
            {
                LogError("Beklemede listeleme hatası", ex);
                ShowErrorAndRedirect("Başvurular listelenirken bir hata oluştu. Lütfen yöneticiyle iletişime geçiniz.", "~/Anasayfa.aspx");
            }
        }

        private void EvrakGecmis()
        {
            try
            {
                if (GridView1.SelectedRow == null)
                {
                    return;
                }

                string basvuruId = GridView1.SelectedRow.Cells[2].Text; // Basvuru_No sütunu (index 2)

                lblTable.Text = Helpers.BuildDocumentHistoryHtml(basvuruId);
                LogInfo($"Evrak geçmişi gösterildi: Başvuru {basvuruId}");
            }
            catch (Exception ex)
            {
                LogError("Evrak geçmişi yükleme hatası", ex);
                lblTable.Text = "<div class='alert alert-danger'>Geçmiş yüklenirken bir hata oluştu.</div>";
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            gecmisbolumu.Visible = true;
            EvrakGecmis();
        }

        protected void exceleaktar_Click(object sender, EventArgs e)
        {

            ExportGridViewToExcel(GridView1, "BeklemedeBasvurular.xls");
            if (GridView1.Rows.Count == 0)
            {
                ShowToast("Export edilecek veri bulunamadı.", "warning");
                return;
            }
            try
            {
                ExportGridViewToExcel(GridView1, "BeklemedeBasvurular_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowToast("Excel dosyası oluşturulurken hata oluştu.", "danger");
            }

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // GridView export için gerekli
        }
    }
}