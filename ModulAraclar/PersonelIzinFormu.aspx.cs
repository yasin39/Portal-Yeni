using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using Portal.Base; // BasePage'i kullanmak için
using iTextSharp.text; // PDF için
using iTextSharp.text.pdf; // PDF için
using System.IO; // PDF Fontları için

namespace Portal.ModulAraclar
{
    public partial class PersonelIzinFormu : BasePage // BasePage'den türetildi
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPermission(1001); // 1001 yetki kodu kontrolü (eski koddaki gibi)
                pnlPersonelDetay.Visible = false;
                pnlGecmisIzinler.Visible = false;
            }
        }

        protected void sicildengetirbuton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(sicil.Text))
            {
                PersonelBilgileriniGetir(sicil.Text.Trim());
            }
        }

        protected void sicil_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(sicil.Text))
            {
                PersonelBilgileriniGetir(sicil.Text.Trim());
            }
        }

        private void PersonelBilgileriniGetir(string sicilNo)
        {
            string query = "SELECT * FROM personel WHERE SicilNo = @SicilNo";
            var parameters = CreateParameters(("@SicilNo", sicilNo));

            try
            {
                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    tc.Text = row["TcKimlikNo"].ToString();
                    adisoyadi.Text = $"{row["Adi"]} {row["Soyad"]}";
                    unvan.Text = row["Unvan"].ToString();
                    statu.Text = row["Statu"].ToString();
                    birim.Text = row["GorevYaptigiBirim"].ToString();

                    // === DEĞİŞİKLİK 2 (CS): Resim atama satırı kaldırıldı ===
                    // Image1.ImageUrl = row["Resim"].ToString(); 
                    // === DEĞİŞİKLİK 2 SONU ===

                    pnlPersonelDetay.Visible = true;
                    IzinleriGetir(sicilNo); // Personelin geçmiş izinlerini yükle
                }
                else
                {
                    ShowToast("Personel bulunamadı.", "warning");
                    pnlPersonelDetay.Visible = false;
                    pnlGecmisIzinler.Visible = false;
                }
            }
            catch (Exception ex)
            {
                LogError("PersonelBilgileriniGetir Hatası", ex);
                ShowToast("Personel bilgileri getirilirken hata oluştu.", "danger");
            }
        }

        // IzinleriGetir metodunda değişiklik yok...
        private void IzinleriGetir(string sicilNo)
        {
            string query = "SELECT id, izin_turu, Aciklama, izne_Baslama_Tarihi, ibaslamasaat, izin_Bitis_Tarihi, ibitissaat, Kayit_Kullanici, Kayit_Tarihi " +
                           "FROM personel_izin WHERE Sicil_No = @SicilNo ORDER BY id DESC";
            var parameters = CreateParameters(("@SicilNo", sicilNo));

            try
            {
                DataTable dt = ExecuteDataTable(query, parameters);
                GridView2.DataSource = dt;
                GridView2.DataBind();
                pnlGecmisIzinler.Visible = dt.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                LogError("IzinleriGetir Hatası", ex);
                ShowToast("Geçmiş izinler getirilirken hata oluştu.", "danger");
            }
        }

        // IzinCakismasiVar metodunda değişiklik yok...
        private bool IzinCakismasiVar(DateTime baslama, DateTime bitis)
        {
            string query = @"SELECT COUNT(*) FROM personel_izin 
                             WHERE Sicil_No = @SicilNo 
                             AND (
                                 (@Baslama BETWEEN izne_Baslama_Tarihi AND izin_Bitis_Tarihi) OR
                                 (@Bitis BETWEEN izne_Baslama_Tarihi AND izin_Bitis_Tarihi) OR
                                 (izne_Baslama_Tarihi BETWEEN @Baslama AND @Bitis)
                             )";

            var parameters = CreateParameters(
                ("@SicilNo", sicil.Text),
                ("@Baslama", baslama),
                ("@Bitis", bitis)
            );

            try
            {
                object result = ExecuteScalar(query, parameters);
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                LogError("IzinCakismasiVar Hatası", ex);
                ShowToast("İzin çakışma kontrolü sırasında hata oluştu.", "danger");
                return true; // Hata durumunda çakışma var say (güvenlik)
            }
        }

        // eklebuton_Click metodunda değişiklik yok...
        protected void eklebuton_Click(object sender, EventArgs e)
        {
            DateTime baslamaTarihi;
            DateTime bitisTarihi;

            if (!DateTime.TryParseExact(iznebaslamatarihi.Text, "d.M.yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out baslamaTarihi) ||
                !DateTime.TryParseExact(izinbitistarihi.Text, "d.M.yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out bitisTarihi))
            {
                ShowToast("Tarih formatı geçersiz. (Örn: 01.01.2025 10:30)", "warning");
                return;
            }

            if (bitisTarihi <= baslamaTarihi)
            {
                ShowToast("Bitiş tarihi, başlama tarihinden sonra olmalıdır.", "warning");
                return;
            }

            // 1. İzin Çakışmasını Kontrol Et
            if (IzinCakismasiVar(baslamaTarihi, bitisTarihi))
            {
                ShowToast("Seçilen tarihlerde personelin zaten tanımlı bir izni bulunmaktadır.", "danger");
                return;
            }

            // 2. İzni Veritabanına Kaydet
            string query = @"INSERT INTO personel_izin 
                               (Sicil_No, Tc_No, Adi_Soyadi, Statu, izin_Turu, 
                                izne_Baslama_Tarihi, izin_Bitis_Tarihi, Aciklama, Kayit_Tarihi, Kayit_Kullanici, 
                                ibaslamasaat, ibitissaat) 
                             VALUES 
                               (@Sicil_No, @Tc_No, @Adi_Soyadi, @Statu, @izin_Turu, 
                                @izne_Baslama_Tarihi, @izin_Bitis_Tarihi, @Aciklama, @Kayit_Tarihi, @Kayit_Kullanici, 
                                @ibaslamasaat, @ibitissaat)";

            var parameters = CreateParameters(
                ("@Sicil_No", sicil.Text),
                ("@Tc_No", tc.Text),
                ("@Adi_Soyadi", adisoyadi.Text),
                ("@Statu", statu.Text),
                ("@izin_Turu", izinturu.SelectedValue),
                ("@izne_Baslama_Tarihi", baslamaTarihi.Date),
                ("@izin_Bitis_Tarihi", bitisTarihi.Date),
                ("@Aciklama", aciklama.Text),
                ("@Kayit_Tarihi", DateTime.Now),
                ("@Kayit_Kullanici", CurrentUserName), // BasePage'den alındı
                ("@ibaslamasaat", baslamaTarihi.TimeOfDay),
                ("@ibitissaat", bitisTarihi.TimeOfDay)
            );

            try
            {
                int yeniIzinId = ExecuteInsertWithIdentity(query, parameters);

                if (yeniIzinId > 0)
                {
                    // 3. PDF Oluştur
                    GenerateCustomPdf(yeniIzinId, baslamaTarihi, bitisTarihi);
                }
                else
                {
                    ShowToast("İzin kaydı oluşturulurken bir hata oluştu.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("eklebuton_Click Hatası", ex);
                ShowToast($"Kayıt sırasında kritik hata: {ex.Message}", "danger");
            }
        }

        protected void temizlebuton_Click(object sender, EventArgs e)
        {
            Response.Redirect("PersonelIzinFormu.aspx");
        }

        /// <summary>
        /// İstenen formata göre (Personel İzin Formu - PDF Çıktı.PNG) iTextSharp kullanarak PDF oluşturur.
        /// </summary>
        private void GenerateCustomPdf(int izinId, DateTime baslama, DateTime bitis)
        {
            // 1. Gerekli tüm verileri (Personel, Müdür) topla
            string personelAdi = adisoyadi.Text;
            string personelSicil = sicil.Text;
            string personelStatu = statu.Text;
            string personelUnvan = unvan.Text;
            string personelBirim = birim.Text;

            // Müdür bilgisini çek (eski koddaki gibi Session'dan)
            string amirSicil = CurrentUserSicil.Length > 2 ? CurrentUserSicil.Substring(2) : CurrentUserSicil;
            string amirAdi = "";
            string amirUnvan = "";

            try
            {
                DataTable dtAmir = ExecuteDataTable("SELECT (Adi + ' ' + Soyad) AS AdSoyad, Unvan FROM personel WHERE SicilNo = @SicilNo", CreateParameters(("@SicilNo", amirSicil)));
                if (dtAmir.Rows.Count > 0)
                {
                    amirAdi = dtAmir.Rows[0]["AdSoyad"].ToString();
                    amirUnvan = dtAmir.Rows[0]["Unvan"].ToString();
                }
            }
            catch (Exception ex)
            {
                LogError("Amir bilgisi çekilirken hata oluştu.", ex);
                amirAdi = "Hata"; // Hata durumunda PDF'e yansıt
            }


            // 2. PDF Dokümanını Hazırla
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename=PersonelIzin_{personelSicil}_{izinId}.pdf");

            Document document = new Document(PageSize.A4, 30f, 30f, 30f, 30f);
            PdfWriter writer = null; // Hata düzeltmesi için 'try' dışına alındı

            try
            {
                writer = PdfWriter.GetInstance(document, Response.OutputStream);
                document.Open();

                // 3. Fontları Tanımla (BasePage'deki gibi)
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                BaseFont bfArial = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font fontHeader = new Font(bfArial, 12, Font.BOLD);
                Font fontBold = new Font(bfArial, 10, Font.BOLD);
                Font fontNormal = new Font(bfArial, 10, Font.NORMAL);
                Font fontSmall = new Font(bfArial, 9, Font.NORMAL, BaseColor.GRAY);

                // 4. Başlık Bölümü
                Paragraph title = new Paragraph("T.C.\nULAŞTIRMA VE ALTYAPI BAKANLIĞI\nII. Bölge Müdürlüğü\nSaatlik İzin Formu", fontHeader);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 15f;
                document.Add(title);

                Paragraph formNo = new Paragraph($"No: {izinId}", fontNormal);
                formNo.Alignment = Element.ALIGN_RIGHT;
                document.Add(formNo);

                // 5. Ana Bilgi Tablosu
                PdfPTable table = new PdfPTable(2); // 2 Sütunlu
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 35f, 65f }); // Sütun genişlikleri (%35 - %65)
                table.SpacingBefore = 10f;
                table.SpacingAfter = 20f;

                // Helper metot (iç içe)
                Action<string, string> AddRow = (key, value) =>
                {
                    PdfPCell keyCell = new PdfPCell(new Phrase(key, fontBold));
                    keyCell.Padding = 8f;
                    keyCell.BorderWidth = 1f;
                    keyCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(keyCell);

                    PdfPCell valueCell = new PdfPCell(new Phrase(value, fontNormal));
                    valueCell.Padding = 8f;
                    valueCell.BorderWidth = 1f;
                    valueCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(valueCell);
                };

                AddRow("Adı Soyadı / Sicil No", $"{personelAdi} / {personelSicil}");
                AddRow("Statüsü / Kadrosu", $"{personelStatu} / {personelUnvan}");
                AddRow("Birimi", personelBirim);
                AddRow("İzin Türü", izinturu.SelectedValue);
                AddRow("İzin Sebebi Açıklama", aciklama.Text);
                AddRow("İzin Başlama / Bitiş Tarihi", $"{baslama:dd.MM.yyyy -- HH:mm} / {bitis:dd.MM.yyyy -- HH:mm}");

                document.Add(table);

                // 6. İmza Bölümü (Talep Eden / Birim Amiri)
                PdfPTable signTable = new PdfPTable(2);
                signTable.WidthPercentage = 100;
                signTable.SetWidths(new float[] { 50f, 50f });
                signTable.SpacingAfter = 20f;

                PdfPCell talepEdenCell = new PdfPCell(new Phrase($"TALEP EDEN\n\n\n{personelAdi}\n{DateTime.Now:dd/MM/yyyy}", fontNormal));
                talepEdenCell.HorizontalAlignment = Element.ALIGN_CENTER;
                talepEdenCell.VerticalAlignment = Element.ALIGN_TOP;
                talepEdenCell.Border = Rectangle.BOX;
                talepEdenCell.MinimumHeight = 150f;
                talepEdenCell.Padding = 10f;
                signTable.AddCell(talepEdenCell);

                PdfPCell birimAmiriCell = new PdfPCell(new Phrase($"BİRİM AMİRİ\n\n\n{amirAdi}\n{amirUnvan}\n{DateTime.Now:dd/MM/yyyy}", fontNormal));
                birimAmiriCell.HorizontalAlignment = Element.ALIGN_CENTER;
                birimAmiriCell.VerticalAlignment = Element.ALIGN_TOP;
                birimAmiriCell.Border = Rectangle.BOX;
                birimAmiriCell.MinimumHeight = 150f;
                birimAmiriCell.Padding = 10f;
                signTable.AddCell(birimAmiriCell);

                document.Add(signTable);

                // 7. Uygunluk Bölümü
                PdfPTable uygunTable = new PdfPTable(1);
                uygunTable.WidthPercentage = 100;

                // === DEĞİŞİKLİK 1 (CS): Tarih formatı "..../MM/yyyy" olarak güncellendi ===
                string uygunTarih = $"..../{DateTime.Now:MM/yyyy}";
                PdfPCell uygunCell = new PdfPCell(new Phrase($"UYGUNDUR\n\n\n{uygunTarih}", fontNormal));
                // === DEĞİŞİKLİK 1 SONU ===

                uygunCell.HorizontalAlignment = Element.ALIGN_CENTER;
                uygunCell.VerticalAlignment = Element.ALIGN_TOP;
                uygunCell.Border = Rectangle.BOX;
                uygunCell.MinimumHeight = 100f;
                uygunCell.Padding = 10f;
                uygunTable.AddCell(uygunCell);

                document.Add(uygunTable);

                // 8. Alt Bilgi (Tarih)
                Paragraph footer = new Paragraph($"Oluşturma Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm:ss}", fontSmall);
                footer.Alignment = Element.ALIGN_RIGHT;
                document.Add(footer);

            }
            catch (Exception ex)
            {
                LogError("GenerateCustomPdf Hatası", ex);
                // Hata oluşursa PDF oluşturmayı durdur, kullanıcıya toast göster
                ShowToast("PDF oluşturulurken kritik bir hata oluştu.", "danger");
            }
            finally
            {
                if (document.IsOpen())
                {
                    document.Close();
                }

                if (writer != null) // Hata düzeltmesi
                {
                    writer.Close();
                }
                Response.End(); // PDF'i kullanıcıya gönder
            }
        }
    }
}