using Portal.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.ModulTehlikeliMadde
{
    public partial class FirmaRapor : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckPermission(800))
                return;

            if (!IsPostBack)
            {
                LoadInitialData();
            }
        }

        private void LoadInitialData()
        {
            Helpers.LoadProvinces(ddlIl, "Hepsi");
            LoadFaaliyetAlanlari();

            ddlIlce.Items.Clear();
            ddlIlce.Items.Insert(0, new ListItem("Hepsi", "Hepsi"));
        }

        private void LoadFaaliyetAlanlari()
        {
            try
            {
                string query = "SELECT FaaliyetAdi FROM tmfaaliyetalanlari ORDER BY FaaliyetAdi ASC";
                DataTable dt = ExecuteDataTable(query);

                ddlFaaliyetAlani.Items.Clear();
                ddlFaaliyetAlani.Items.Insert(0, new ListItem("Hepsi", "Hepsi"));

                foreach (DataRow row in dt.Rows)
                {
                    ddlFaaliyetAlani.Items.Add(new ListItem(row["FaaliyetAdi"].ToString()));
                }
            }
            catch (Exception ex)
            {
                LogError("LoadFaaliyetAlanlari hatası", ex);
                ShowToast("Faaliyet alanları yüklenirken hata oluştu.", "error");
            }
        }

        protected void ddlIl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlIl.SelectedValue != "Hepsi" && !string.IsNullOrEmpty(ddlIl.SelectedValue))
            {
                Helpers.LoadDistricts(ddlIlce, ddlIl.SelectedValue, "Hepsi");
            }
            else
            {
                ddlIlce.Items.Clear();
                ddlIlce.Items.Insert(0, new ListItem("Hepsi", "Hepsi"));
            }
        }

        protected void btnAra_Click(object sender, EventArgs e)
        {
            try
            {
                string query = BuildQuery();
                DataTable dt = ExecuteDataTable(query);

                FirmalarGrid.DataSource = dt;
                FirmalarGrid.DataBind();

                if (dt.Rows.Count > 0)
                {
                    FirmalarGrid.FooterRow.Cells[0].Text = "Toplam";
                    FirmalarGrid.FooterRow.Cells[0].ColumnSpan = 2;
                    FirmalarGrid.FooterRow.Cells[1].Visible = false;
                    FirmalarGrid.FooterRow.Cells[2].Text = $"{dt.Rows.Count} kayıt";
                    FirmalarGrid.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Left;

                    ShowToast($"{dt.Rows.Count} kayıt bulundu.", "success");
                }
                else
                {
                    ShowToast("Arama kriterlerine uygun kayıt bulunamadı.", "info");
                }
            }
            catch (Exception ex)
            {
                LogError("btnAra_Click hatası", ex);
                ShowToast("Arama sırasında hata oluştu.", "error");
            }
        }

        protected void btnPdfRapor_Click(object sender, EventArgs e)
        {
            if (FirmalarGrid.Rows.Count == 0)
            {
                ShowToast("PDF oluşturmak için önce arama yapınız.", "warning");
                return;
            }

            try
            {
                ExportGridViewToPdf(FirmalarGrid, "TehlikeliMaddeFirmalari.pdf", "II. Bölge Müdürlüğü - TMFB Listesi");
            }
            catch (Exception ex)
            {
                LogError("btnPdfRapor_Click hatası", ex);
                ShowToast("PDF oluşturulurken hata oluştu.", "error");
            }
        }

        protected void btnExcelAktar_Click(object sender, EventArgs e)
        {
            if (FirmalarGrid.Rows.Count == 0)
            {
                ShowToast("Excel aktarmak için önce arama yapınız.", "warning");
                return;
            }

            try
            {
                ExportGridViewToExcel(FirmalarGrid, "TehlikeliMaddeFirmalari.xls");
            }
            catch (Exception ex)
            {
                LogError("btnExcelAktar_Click hatası", ex);
                ShowToast("Excel aktarımı sırasında hata oluştu.", "error");
            }
        }

        protected void btnTemizle_Click(object sender, EventArgs e)
        {
            txtUnet.Text = string.Empty;
            txtUnvan.Text = string.Empty;

            ResetDropDownLists(ddlStatu, ddlFaaliyetAlani, ddlIl, ddlIlce, ddlDurum);

            FirmalarGrid.DataSource = null;
            FirmalarGrid.DataBind();

            ShowToast("Filtreler temizlendi.", "info");
        }

        private string BuildQuery()
        {
            List<string> whereKosullari = new List<string> { "1=1" };

            if (!string.IsNullOrWhiteSpace(txtUnet.Text))
            {
                whereKosullari.Add($"Unet = '{txtUnet.Text.Trim()}'");
            }

            if (!string.IsNullOrWhiteSpace(txtUnvan.Text))
            {
                whereKosullari.Add($"Unvan LIKE '%{txtUnvan.Text.Trim()}%'");
            }

            if (ddlStatu.SelectedValue != "Hepsi")
            {
                whereKosullari.Add($"Statu = '{ddlStatu.SelectedValue}'");
            }

            if (ddlFaaliyetAlani.SelectedValue != "Hepsi")
            {
                whereKosullari.Add($"FaaliyetTuru = '{ddlFaaliyetAlani.SelectedValue}'");
            }

            // Eğer seçilen değer "Hepsi" değilse ve boş değilse filtreyi ekle
            if (ddlIl.SelectedValue != "Hepsi" && !string.IsNullOrWhiteSpace(ddlIl.SelectedValue))
            {
                // .Trim() kullanarak olası boşluk uyumsuzluklarını önleriz
                whereKosullari.Add($"il = '{ddlIl.SelectedValue.Trim()}'");
            }


            if (ddlIlce.SelectedValue != null && ddlIlce.SelectedValue != "Hepsi" && ddlIlce.SelectedValue.Trim() != "")
            {
                whereKosullari.Add($"ilce = '{ddlIlce.SelectedValue.Trim()}'");
            }
            // Hiçbir şey eklemezsek → ilçe filtresi olmadan tüm kayıtlar gelir

            if (ddlDurum.SelectedValue != "Hepsi")
            {
                string durumKodu = ddlDurum.SelectedValue == "Aktif" ? "Geçerli" : "İptal";
                whereKosullari.Add($"Durum = '{durumKodu}'");
            }

            string whereClause = string.Join(" AND ", whereKosullari);
            return $"SELECT Unet, Unvan, Adres, ilce, il, Statu, BelgeTuru, BelgeSeriNo, FaaliyetTuru FROM tmistatistik WHERE {whereClause} ORDER BY Unet ASC";
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
    }
}