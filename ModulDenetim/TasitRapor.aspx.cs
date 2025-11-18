using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Portal.Base;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using ListItem = System.Web.UI.WebControls.ListItem;

namespace Portal.ModulDenetim
{
    public partial class TasitRapor : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                if (!CheckPermission(200))
                {
                    return;
                }

                DropdownlariDoldur();
                GridDoldur();
            }
        }

        #region Dropdown Doldurma

        private void DropdownlariDoldur()
        {
            try
            {                
                Helpers.LoadActivePersonnel(Personel, "Personel Seçiniz");
                
                Helpers.LoadProvinces(Il, "İl Seçiniz");
                
                YetkiBelgesiDoldur();
                                
            }
            catch (Exception ex)
            {
                LogError("DropdownlariDoldur hatası", ex);
                ShowToast("Dropdown listeler yüklenirken hata oluştu.", "danger");
            }
        }

        private void YetkiBelgesiDoldur()
        {
            string query = @"SELECT Belge_Adi FROM yetki_belgeleri ORDER BY Belge_Adi ASC";
            DataTable dt = ExecuteDataTable(query);

            YetkiBelgesi.Items.Clear();
            YetkiBelgesi.Items.Insert(0, new ListItem("Seçiniz", ""));

            foreach (DataRow row in dt.Rows)
            {
                YetkiBelgesi.Items.Add(new ListItem(row["Belge_Adi"].ToString()));
            }
        }

 

        #endregion

        #region İl/İlçe İşlemleri

        protected void Il_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Il.SelectedValue))
            {                
                Helpers.LoadDistricts(Ilce, Il.SelectedValue, "İlçe Seçiniz");
            }
            else
            {
                Ilce.Items.Clear();
                Ilce.Items.Insert(0, new ListItem("İlçe Seçiniz", ""));
            }
        }

        #endregion

        #region GridView İşlemleri

        private void GridDoldur(string filtre = "")
        {
            try
            {
                string query = @"SELECT id, Plaka, Plaka2, Unvan, DenetimYeri, YetkiBelgesi, 
                                DenetimTuru, DenetimTarihi, il, ilce, Personel1, CezaDurumu, Aciklama
                                FROM denetimtasit 
                                WHERE 1=1";

                var parameters = new System.Collections.Generic.List<SqlParameter>();

                if (!string.IsNullOrEmpty(filtre))
                {
                    query += " AND (Plaka LIKE @Filtre OR Unvan LIKE @Filtre)";
                    parameters.Add(CreateParameter("@Filtre", "%" + filtre + "%"));
                }

                query += " ORDER BY id DESC";

                DataTable dt = ExecuteDataTable(query, parameters);
                DenetimGrid.DataSource = dt;
                DenetimGrid.DataBind();
            }
            catch (Exception ex)
            {
                LogError("GridDoldur hatası", ex);
                ShowToast("Grid yüklenirken hata oluştu.", "danger");
            }
        }

        protected void DenetimGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DenetimGrid.PageIndex = e.NewPageIndex;
            GridDoldur();
        }

        #endregion

      

        #region Excel Export

        protected void ExcelBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DenetimGrid.AllowPaging = false;
                GridDoldur(); // Grid'i yenile
                ExportGridViewToExcel(DenetimGrid, "TasitDenetimRaporu.xls", 300);               
                DenetimGrid.AllowPaging = true;
                GridDoldur();
            }
            catch (Exception ex)
            {
                LogError("ExcelBtn_Click hatası", ex);
                ShowToast("Excel dışa aktarılırken hata oluştu.", "danger");
            }
        }

        #endregion

        #region PDF Export

        protected void PdfIndir_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"SELECT id, Plaka, Plaka2, Unvan, DenetimYeri, YetkiBelgesi, 
                                DenetimTuru, DenetimTarihi, il, ilce, Personel1, CezaDurumu, Aciklama
                                FROM denetimtasit 
                                ORDER BY id DESC";

                DataTable dt = ExecuteDataTable(query);

                if (dt.Rows.Count == 0)
                {
                    ShowToast("PDF oluşturulacak veri bulunamadı.", "warning");
                    return;
                }

                PdfOlustur(dt);
            }
            catch (Exception ex)
            {
                LogError("PdfIndir_Click hatası", ex);
                ShowToast("PDF oluşturulurken hata oluştu.", "danger");
            }
        }

        private void PdfOlustur(DataTable dt)
        {
            Document document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
            MemoryStream ms = new MemoryStream();

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Türkçe karakter desteği için font
                BaseFont bf;

                try
                {
                    // Windows sistem fontunu kullanmayı dene
                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                catch
                {
                    // Başarısız olursa built-in Helvetica kullan
                    bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                }

                Font titleFont = new Font(bf, 16, Font.BOLD);
                Font headerFont = new Font(bf, 10, Font.BOLD);
                Font cellFont = new Font(bf, 8, Font.NORMAL);

                // Başlık
                Paragraph title = new Paragraph("II. Bolge Mudurlugu\nTasit Denetim Raporu", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 20;
                document.Add(title);

                // Tablo oluştur
                PdfPTable table = new PdfPTable(11);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 5f, 8f, 8f, 15f, 12f, 10f, 10f, 8f, 8f, 8f, 15f });

                // Başlık satırı
                string[] headers = { "No", "Plaka", "Y.Romork", "Unvan", "Denetim Yeri", "Yetki Belgesi",
                            "Denetim Turu", "Tarih", "Il", "Ilce", "Personel" };

                foreach (string header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, headerFont));
                    cell.BackgroundColor = new BaseColor(75, 123, 236);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 5;
                    table.AddCell(cell);
                }

                // Veri satırları
                foreach (DataRow row in dt.Rows)
                {
                    table.AddCell(new Phrase(row["id"].ToString(), cellFont));
                    table.AddCell(new Phrase(row["Plaka"].ToString(), cellFont));
                    table.AddCell(new Phrase(row["Plaka2"].ToString(), cellFont));
                    table.AddCell(new Phrase(row["Unvan"].ToString(), cellFont));
                    table.AddCell(new Phrase(row["DenetimYeri"].ToString(), cellFont));
                    table.AddCell(new Phrase(row["YetkiBelgesi"].ToString(), cellFont));
                    table.AddCell(new Phrase(row["DenetimTuru"].ToString(), cellFont));

                    DateTime tarih = Convert.ToDateTime(row["DenetimTarihi"]);
                    table.AddCell(new Phrase(tarih.ToString("dd.MM.yyyy"), cellFont));

                    table.AddCell(new Phrase(row["il"].ToString(), cellFont));
                    table.AddCell(new Phrase(row["ilce"].ToString(), cellFont));
                    table.AddCell(new Phrase(row["Personel1"].ToString(), cellFont));
                }

                document.Add(table);

                // Toplam satırı
                Paragraph toplam = new Paragraph($"\nTOPLAM: {dt.Rows.Count} kayit", headerFont);
                toplam.Alignment = Element.ALIGN_RIGHT;
                document.Add(toplam);

                document.Close();

                // PDF'i indir
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=TasitDenetimRaporu.pdf");
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
            catch (Exception ex)
            {
                if (document.IsOpen())
                {
                    document.Close();
                }
                LogError("PdfOlustur hatası", ex);
                ShowToast("PDF oluşturulurken hata oluştu: " + ex.Message, "error");
            }
            finally
            {
                ms.Close();
            }
        }

        #endregion

        protected void AraBtn_Click(object sender, EventArgs e)
        {

        }
    }
}