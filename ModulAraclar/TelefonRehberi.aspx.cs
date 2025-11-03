using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Portal.Base; // BasePage'i kullanmak için bu namespace gerekli

namespace Portal.ModulAraclar
{
    // Sınıf adı dosya adıyla eşleşmeli ve BasePage'den türemeli
    public partial class TelefonRehberi : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Sayfa ilk yüklendiğinde birim listesini doldur
                PopulateDepartments();

                // Başlangıçta tüm rehberi listele
                BindGrid();
            }
        }

        /// <summary>
        /// Arama filtresindeki Birim (Şube) DropDownList'ini doldurur.
        /// </summary>
        private void PopulateDepartments()
        {
            try
            {
                // BasePage'deki PopulateDropDownList'i kullanamayız çünkü "Hepsi" seçeneği
                // veritabanından gelmiyor ve value değeri de "Hepsi" olmalı.
                string query = "SELECT Sube_Adi FROM subeler ORDER BY Sube_Adi ASC";

                // BasePage'den gelen ExecuteDataTable metodunu kullan
                DataTable dt = ExecuteDataTable(query);

                ddlBirim.DataSource = dt;
                ddlBirim.DataTextField = "Sube_Adi";
                ddlBirim.DataValueField = "Sube_Adi";
                ddlBirim.DataBind();

                // Orijinal koddaki gibi "Hepsi" seçeneğini en başa ekle
                ddlBirim.Items.Insert(0, new ListItem("Hepsi", "Hepsi"));
            }
            catch (Exception ex)
            {
                // BasePage'den gelen Loglama ve Hata gösterme metodları
                LogError("Telefon Rehberi - Birimler yüklenemedi.", ex);
                ShowToast("Birimler yüklenirken bir hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Arama butonuna tıklandığında tetiklenir.
        /// </summary>
        protected void btnAra_Click(object sender, EventArgs e)
        {
            // Arama kriterlerine göre grid'i yeniden doldur
            BindGrid();
        }

        /// <summary>
        /// Arama kriterlerine göre personel verilerini çeker ve HTML tablo olarak basar.
        /// </summary>
        private void BindGrid()
        {
            try
            {
                // GÜVENLİK: SQL Injection'ı önlemek için parametreli sorgu kullanıyoruz.
                var parameters = new List<SqlParameter>();

                // Sorgu oluşturucu
                var queryBuilder = new StringBuilder();
                queryBuilder.Append(@"SELECT Adi, Soyad, Unvan, Dahili, CepTelefonu 
                                      FROM personel 
                                      WHERE Durum = 'Aktif' 
                                      AND CalismaDurumu != 'Geçici Görevde Pasif Çalışan' 
                                      AND Dahili IS NOT NULL ");

                // Ad filtresi (LIKE ile daha esnek arama)
                if (!string.IsNullOrWhiteSpace(txtAd.Text))
                {
                    queryBuilder.Append("AND Adi LIKE @Adi ");
                    // BasePage'den gelen CreateParameter metodunu kullan
                    parameters.Add(CreateParameter("@Adi", "%" + txtAd.Text.Trim() + "%"));
                }

                // Soyad filtresi (LIKE ile daha esnek arama)
                if (!string.IsNullOrWhiteSpace(txtSoyad.Text))
                {
                    queryBuilder.Append("AND Soyad LIKE @Soyad ");
                    parameters.Add(CreateParameter("@Soyad", "%" + txtSoyad.Text.Trim() + "%"));
                }

                // Birim filtresi (Eğer "Hepsi" seçili değilse)
                if (ddlBirim.SelectedValue != "Hepsi" && !string.IsNullOrEmpty(ddlBirim.SelectedValue))
                {
                    queryBuilder.Append("AND GorevYaptigiBirim = @Birim ");
                    parameters.Add(CreateParameter("@Birim", ddlBirim.SelectedValue));
                }

                queryBuilder.Append("ORDER BY Adi ASC");

                // BasePage'den gelen ExecuteDataTable ile sorguyu çalıştır
                DataTable dt = ExecuteDataTable(queryBuilder.ToString(), parameters);

                // DataTable'ı HTML'e çevir
                lblTable.Text = GenerateHtmlTable(dt);
                lblTable.Visible = true;
            }
            catch (Exception ex)
            {
                LogError("Telefon Rehberi - Arama hatası.", ex);
                ShowToast("Veriler getirilirken bir hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Veritabanından gelen DataTable'ı Bootstrap uyumlu bir HTML tabloya dönüştürür.
        /// </summary>
        /// <param name="dt">Personel verilerini içeren DataTable</param>
        /// <returns>HTML tablo string'i</returns>
        private string GenerateHtmlTable(DataTable dt)
        {
            if (dt.Rows.Count == 0)
            {
                return "<div class='alert alert-warning'>Arama kriterlerine uygun kayıt bulunamadı.</div>";
            }

            var htmlTable = new StringBuilder();

            // AnaV2.Master'daki stillerle uyumlu Bootstrap sınıfları eklendi
            htmlTable.Append("<table class='table table-bordered table-striped table-hover table-sm' style='width:100%;'>");

            // Tablo Başlığı (Thead)
            htmlTable.Append("<thead class='table-dark'><tr>");
            foreach (DataColumn column in dt.Columns)
            {
                // XSS önlemi için başlıkları encode et
                htmlTable.Append($"<th>{HttpUtility.HtmlEncode(column.ColumnName)}</th>");
            }
            htmlTable.Append("</tr></thead>");

            // Tablo İçeriği (Tbody)
            htmlTable.Append("<tbody>");
            foreach (DataRow row in dt.Rows)
            {
                htmlTable.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    // XSS önlemi için hücre verisini encode et
                    string cellValue = row[column].ToString();
                    htmlTable.Append($"<td>{HttpUtility.HtmlEncode(cellValue)}</td>");
                }
                htmlTable.Append("</tr>");
            }
            htmlTable.Append("</tbody>");

            htmlTable.Append("</table>");

            return htmlTable.ToString();
        }
    }
}