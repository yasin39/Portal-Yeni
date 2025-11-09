
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulCimer
{
    public partial class DevamEden : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(606)) // CİMER Devam Eden Yetkisi
                    return;

                BasvurulariListele();
            }
        }

        /// <summary>
        /// Devam eden CİMER başvurularını GridView'e yükler.
        /// Onay_Durumu '3' olmayan kayıtları getirir.
        /// </summary>
        private void BasvurulariListele()
        {
            string query = @"
                SELECT 
                    id, Basvuru_No, Basvuru_Tarihi, TC_No, Adi_Soyadi, Tel_No, Mail, 
                    Adres, Basvuru_Metni, Basvuru_Ek, Yapilan_İslem, Son_Yapilan_islem, 
                    Son_Kullanici, Sikayet_Edilen_Firma, Guncelleyen_Kullanici, 
                    Kayit_Tarihi, Bekleme_Durumu,
                    DATEDIFF(DAY, Kayit_Tarihi, GETDATE()) AS Sure
                FROM cimer_basvurular 
                WHERE Onay_Durumu != '3' 
                ORDER BY id DESC";

            DataTable dtBasvurular = ExecuteDataTable(query);

            // Süre hesaplaması ve renklendirme GridView'de TemplateField ile yapılıyor

            GridViewDevamEden.DataSource = dtBasvurular;
            GridViewDevamEden.DataBind();

            // Footer'a toplam sayıyı ekle (ilk sütuna)
            if (GridViewDevamEden.FooterRow != null)
            {
                GridViewDevamEden.FooterRow.Cells[0].Text = $"Toplam: {dtBasvurular.Rows.Count}";
                GridViewDevamEden.FooterRow.HorizontalAlign = HorizontalAlign.Center;
            }
        }

        /// <summary>
        /// Seçilen başvurunun geçmiş hareketlerini tablo olarak gösterir.
        /// </summary>
        private void BasvuruGecmisiniGetir()
        {
            try
            {                
                if (GridViewDevamEden.SelectedIndex < 0)
                {
                    ShowToast("Lütfen bir başvuru seçiniz.", "warning");
                    return;
                }

                // ==> FIX: id yerine Basvuru_No kullan (Hareket tablosu Basvuru_id kolonu Basvuru_No ile eşleşiyor)
                int selectedIndex = GridViewDevamEden.SelectedIndex;
                string basvuruNo = GridViewDevamEden.Rows[selectedIndex].Cells[2].Text; // Basvuru_No kolonu (index 2)                

                string query = @"
                    SELECT Sevk_Eden, Teslim_Alan, Tarih, Aciklama, islem_Aciklama 
                    FROM cimer_basvuru_hareketleri 
                    WHERE Basvuru_id = @BasvuruId 
                    ORDER BY id DESC";

                var parameters = CreateParameters(
                    ("@BasvuruId", basvuruNo)
                );

                DataTable dtGecmis = ExecuteDataTable(query, parameters);

                // HTML tablo oluştur
                string htmlTablo = "<table class='table table-bordered table-striped table-condensed history-table'>";
                htmlTablo += "<thead><tr>";
                htmlTablo += "<th>Sevk Eden</th><th>Teslim Alan</th><th>Tarih</th><th>Açıklama</th><th>İşlem Açıklaması</th>";
                htmlTablo += "</tr></thead><tbody>";

                foreach (DataRow row in dtGecmis.Rows)
                {
                    htmlTablo += "<tr>";
                    htmlTablo += $"<td>{row["Sevk_Eden"]}</td>";
                    htmlTablo += $"<td>{row["Teslim_Alan"]}</td>";
                    htmlTablo += $"<td>{FormatDateTimeTurkish(Convert.ToDateTime(row["Tarih"]))}</td>";
                    htmlTablo += $"<td>{row["Aciklama"]}</td>";
                    htmlTablo += $"<td>{row["islem_Aciklama"]}</td>";
                    htmlTablo += "</tr>";
                }

                htmlTablo += "</tbody></table>";

                if (dtGecmis.Rows.Count == 0)
                {
                    htmlTablo = "<p class='text-muted text-center'>Bu başvuru için henüz hareket kaydı bulunmamaktadır.</p>";
                }

                lblGecmisTablo.Text = htmlTablo;
                PanelGecmis.Visible = true;

                ShowToast($"Hareket geçmişi yüklendi. ({dtGecmis.Rows.Count} kayıt)", "info");
            }
            catch (Exception ex)
            {
                LogError("BasvuruGecmisiniGetir", ex);
                ShowToast($"Geçmiş yüklenirken hata: {ex.Message}", "danger");
            }
        }


        protected void btnGecmisKapat_Click(object sender, EventArgs e)
        {
            PanelGecmis.Visible = false;
        }

        /// <summary>
        /// GridView'de satır seçildiğinde geçmiş bölümünü gösterir.
        /// </summary>
        protected void GridViewDevamEden_SelectedIndexChanged(object sender, EventArgs e)
        {
            BasvuruGecmisiniGetir();
        }

        /// <summary>
        /// GridView'i Excel'e aktarır.
        /// </summary>
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ExportGridViewToExcel(GridViewDevamEden, "CimerDevamEdenBasvurular.xls");
        }

        // GridView render için override
        public override void VerifyRenderingInServerForm(Control control)
        {
            // Boş bırak, GridView render için
        }
    }
}
