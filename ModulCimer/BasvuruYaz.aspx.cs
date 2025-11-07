using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using Portal.Base;
using System.Web.Configuration;

namespace Portal.ModulCimer
{
    public partial class BasvuruYaz : BasePage
    {        

        // Public property'ler - .aspx sayfasında <%= %> ile kullanılacak
        public string BasvuruNo { get; set; } = "";
        public string BasvuruTarihi { get; set; } = "";
        public string IlgiliFirma { get; set; } = "Diğer";
        public string SikayetMetni { get; set; } = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.CIMER_RAPORLAMA))
                {
                    Response.Redirect("~/Anasayfa.aspx", true);
                }
            }
        }

        protected void btnGetir_Click(object sender, EventArgs e)
        {
            string basvuruNoGiris = txtBasvuruNo.Text.Trim();

            if (string.IsNullOrEmpty(basvuruNoGiris))
            {
                ShowToast("Lütfen başvuru numarası giriniz.", "warning");
                return;
            }

            System.Threading.Thread.Sleep(100);

            string sql = @"
            SELECT 
                Basvuru_No, 
                CONVERT(varchar, Basvuru_Tarihi, 104) AS Basvuru_Tarihi_Format,
                ISNULL(Sikayet_Edilen_Firma, 'Diğer') AS Sikayet_Edilen_Firma,
                ISNULL(Basvuru_Metni, '') AS Basvuru_Metni
            FROM cimer_basvurular 
            WHERE Basvuru_No = @BasvuruNo";

            var parameters = CreateParameters(
                ("@BasvuruNo", basvuruNoGiris)
            );

            try
            {
                DataTable dt = ExecuteDataTable(sql, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    // Property'leri doldur
                    BasvuruNo = row["Basvuru_No"].ToString();
                    BasvuruTarihi = row["Basvuru_Tarihi_Format"].ToString();
                    IlgiliFirma = row["Sikayet_Edilen_Firma"].ToString();
                    SikayetMetni = row["Basvuru_Metni"].ToString()
                        .Replace("\r\n", "<br />")
                        .Replace("\n", "<br />")
                        .Replace("\r", "<br />");

                    pnlRapor.Visible = true;
                    ShowToast("Rapor yüklendi.", "success");
                }
                else
                {
                    ShowToast("Başvuru bulunamadı.", "danger");
                    pnlRapor.Visible = false;

                    // Hata durumunda property'leri sıfırla
                    BasvuruNo = "";
                    BasvuruTarihi = "";
                    IlgiliFirma = "Diğer";
                    SikayetMetni = "";
                }
            }
            catch (Exception ex)
            {
                LogError("CİMER Rapor Hatası", ex);
                ShowToast("Veritabanı hatası oluştu.", "danger");

                // Hata durumunda sıfırla
                BasvuruNo = "";
                BasvuruTarihi = "";
                IlgiliFirma = "Diğer";
                SikayetMetni = "";
            }
        }
    }
}