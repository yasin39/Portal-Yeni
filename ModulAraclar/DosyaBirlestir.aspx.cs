using iTextSharp.text;
using iTextSharp.text.pdf;
using Portal.Base;
using System;
using System.Data;
using System.IO;
using System.Web;

namespace Portal.ModulAraclar
{
    public partial class DosyaBirlestir : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.ARSIV))
                    return;

                LoadBelgeTurleri();
            }
        }

        private void LoadBelgeTurleri()
        {
            try
            {
                string query = "SELECT Belge_Adi FROM yetki_belgeleri ORDER BY Belge_Adi ASC";
                PopulateDropDownList(ddlBelgeTuru, query, "Belge_Adi", "Belge_Adi", true, null);
                ddlBelgeTuru.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Hepsi", "Hepsi"));
            }
            catch (Exception ex)
            {
                LogError("LoadBelgeTurleri hatası", ex);
                ShowToast("Belge türleri yüklenirken hata oluştu!", "danger");
            }
        }

        protected void btnAra_Click(object sender, EventArgs e)
        {
            try
            {
                SearchFiles();
            }
            catch (Exception ex)
            {
                LogError("btnAra_Click hatası", ex);
                ShowToast("Arama sırasında hata oluştu!", "danger");
            }
        }

        private void SearchFiles()
        {
            string UnetNumarasi = txtUnet.Text.Trim();
            string BelgeTuru = ddlBelgeTuru.SelectedValue;

            string sqlSorgu = @"SELECT Id, Unet, Unvan, Belge_Turu, Sayfa_Sayisi, Dosya 
                               FROM arsiv WHERE 1=1";

            var parametreler = CreateParameters();

            if (!string.IsNullOrEmpty(UnetNumarasi))
            {
                sqlSorgu += " AND Unet = @Unet";
                parametreler.Add(CreateParameter("@Unet", UnetNumarasi));
            }

            if (!string.IsNullOrEmpty(BelgeTuru) && BelgeTuru != "Seçiniz..." && BelgeTuru != "Hepsi")
            {
                sqlSorgu += " AND Belge_Turu = @BelgeTuru";
                parametreler.Add(CreateParameter("@BelgeTuru", BelgeTuru));
            }

            sqlSorgu += " ORDER BY Id DESC";

            DataTable dt = ExecuteDataTable(sqlSorgu, parametreler);
            DosyalarGrid.DataSource = dt;
            DosyalarGrid.DataBind();

            if (dt.Rows.Count > 0)
            {
                CalculateFooter();
                btnBirlestir.Enabled = true;
                ShowToast($"{dt.Rows.Count} dosya bulundu.", "info");
            }
            else
            {
                btnBirlestir.Enabled = false;
                ShowToast("Arama kriterlerine uygun dosya bulunamadı.", "warning");
            }

            if (dt.Rows.Count > 0)
            {
                CalculateFooter();
                btnBirlestir.Enabled = true;
                ShowToast($"{dt.Rows.Count} dosya bulundu.", "info");

                // DEBUG: İlk dosyanın yolunu logla
                if (dt.Rows.Count > 0)
                {
                    string ilkDosyaYolu = dt.Rows[0]["Dosya"].ToString();
                    LogInfo($"İlk dosya yolu (örnek): {ilkDosyaYolu}");

                    string mappedPath = Server.MapPath(ilkDosyaYolu.StartsWith("~/") ? ilkDosyaYolu : "~/" + ilkDosyaYolu.TrimStart('/'));
                    LogInfo($"Mapped path: {mappedPath}");
                    LogInfo($"Dosya var mı: {File.Exists(mappedPath)}");
                }
            }
            else
            {
                btnBirlestir.Enabled = false;
                ShowToast("Arama kriterlerine uygun dosya bulunamadı.", "warning");
            }
        }

        private void CalculateFooter()
        {
            if (DosyalarGrid.Rows.Count == 0)
                return;

            int toplamSayfa = 0;

            for (int i = 0; i < DosyalarGrid.Rows.Count; i++)
            {
                string sayfaSayisiText = GetGridViewCellTextSafe(DosyalarGrid.Rows[i], 4);
                if (int.TryParse(sayfaSayisiText, out int sayfaSayisi))
                {
                    toplamSayfa += sayfaSayisi;
                }
            }

            DosyalarGrid.FooterRow.Cells[0].Text = "Toplam";
            DosyalarGrid.FooterRow.Cells[0].ColumnSpan = 3;
            DosyalarGrid.FooterRow.Cells[1].Visible = false;
            DosyalarGrid.FooterRow.Cells[2].Visible = false;
            DosyalarGrid.FooterRow.Cells[3].Text = $"{DosyalarGrid.Rows.Count} Dosya";
            DosyalarGrid.FooterRow.Cells[4].Text = $"{toplamSayfa} Sayfa";
        }

        protected void btnBirlestir_Click(object sender, EventArgs e)
        {
            try
            {
                if (DosyalarGrid.Rows.Count == 0)
                {
                    ShowToast("Birleştirilecek dosya bulunamadı!", "warning");
                    return;
                }

                MergePDFsToResponse();
            }
            catch (Exception ex)
            {
                LogError("btnBirlestir_Click hatası", ex);
                ShowToast("PDF birleştirme sırasında hata oluştu!", "danger");
            }
        }

        private void MergePDFsToResponse()
        {
            // 1. Çıkış stream'i 'using' bloğu içinde
            using (MemoryStream outputStream = new MemoryStream())
            {
                Document document = null;
                PdfCopy copier = null; // PdfWriter yerine PdfCopy kullanıyoruz

                try
                {
                    document = new Document();
                    copier = new PdfCopy(document, outputStream);

                    // 2. Stream'in otomatik kapanmasını burada da engelliyoruz
                    copier.CloseStream = false;

                    document.Open();

                    int eklenenDosyaSayisi = 0;

                    for (int i = 0; i < DosyalarGrid.Rows.Count; i++)
                    {
                        string dosyaYolu = DosyalarGrid.DataKeys[i]["Dosya"].ToString();

                        if (string.IsNullOrEmpty(dosyaYolu))
                            continue;

                        string tamDosyaYolu = Server.MapPath(dosyaYolu);

                        if (!File.Exists(tamDosyaYolu))
                        {
                            LogWarning($"Dosya bulunamadı: {tamDosyaYolu}");
                            continue;
                        }

                        // 3. Reader'ı 'using' ile açmak artık GÜVENLİ
                        using (PdfReader reader = new PdfReader(tamDosyaYolu))
                        {
                            // 4. PdfCopy'nin doğru metodu budur.
                            // Tüm sayfaları dökümandan okur ve kopyalar.
                            copier.AddDocument(reader);
                            eklenenDosyaSayisi++;
                        }
                    }

                    if (eklenenDosyaSayisi == 0)
                    {
                        ShowToast("Birleştirilecek geçerli dosya bulunamadı.", "warning");
                        // Hata olmasa bile dökümanı düzgünce kapat
                        if (document != null && document.IsOpen()) document.Close();
                        if (copier != null) copier.Close();
                        return;
                    }

                    // 5. ÖNEMLİ: Response'a yazmadan ÖNCE dökümanı kapatmalıyız.
                    // iTextSharp son verileri stream'e bu anda yazar.
                    if (document != null && document.IsOpen())
                    {
                        document.Close();
                    }
                    if (copier != null)
                    {
                        copier.Close();
                    }

                    // 6. Döküman kapandı, stream'e tüm veriler yazıldı.
                    // 'CloseStream = false' sayesinde stream hala AÇIK.
                    string dosyaAdi = $"Birlesmis_Dosya_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", $"attachment; filename={dosyaAdi}");

                    // 7. Artık stream'i güvenle okuyabiliriz.
                    Response.BinaryWrite(outputStream.ToArray());
                    Response.Flush();

                    // 8. Hata fırlatmayan 'CompleteRequest' ile bitiriyoruz.
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                    LogInfo($"PDF birleştirme başarılı: {eklenenDosyaSayisi} dosya birleştirildi. Kullanıcı: {CurrentUserName}");
                }
                catch (Exception ex)
                {
                    // Bir hata olursa stream'leri ve dökümanı yine de kapat
                    if (document != null && document.IsOpen())
                        document.Close();

                    if (copier != null)
                        copier.Close();

                    LogError("MergePDFsToResponse metodunda hata", ex);
                    throw; // Üstteki catch bloğunun yakalaması için fırlat
                }
            } // 'outputStream' burada güvenle Dispose edilir.
        }
    }
}