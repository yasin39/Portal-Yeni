using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Portal.Base; // BasePage için gerekli
using iTextSharp.text; // PDF işlemleri için gerekli
using iTextSharp.text.pdf; // PDF işlemleri için gerekli
using System.Web.UI; // ScriptManager için gerekli

namespace Portal.ModulAraclar
{
    public partial class PdfBirlestir : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnBirlestir.OnClientClick = string.Format(
                    "this.disabled=true; this.value='{0}'; {1};",
                    "İşleniyor...",
                    Page.ClientScript.GetPostBackEventReference(btnBirlestir, "")
                );
            }
        }

        /// <summary>
        /// Seçilen PDF dosyalarını (en fazla 3 adet) birleştirir ve kullanıcıya indirtir.
        /// </summary>
        protected void btnBirlestir_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Dosyaları Al ve Filtrele
                if (!FileUploadControl.HasFiles)
                {
                    ShowToast("Lütfen birleştirmek için dosya seçin.", "warning");
                    ResetButtonScript();
                    return;
                }

                // === GÜNCELLEME: Filtreleme yöntemi dosya uzantısına göre değiştirildi ===
                // Dosya uzantısının ".pdf" (büyük/küçük harf duyarsız) olup olmadığını kontrol et
                var pdfFiles = FileUploadControl.PostedFiles
                    .Where(f => f.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) && f.ContentLength > 0)
                    .ToList();
                // === GÜNCELLEME SONU ===


                // 2. Doğrulama (GÜNCELLENDİ)
                if (pdfFiles.Count < 2)
                {
                    ShowToast("Lütfen birleştirmek için en az 2 geçerli PDF dosyası seçin.", "warning");
                    ResetButtonScript();
                    return;
                }

                // === YENİ KURAL: Maksimum 3 dosya limiti ===
                if (pdfFiles.Count > 3)
                {
                    ShowToast("Güvenlik nedeniyle en fazla 3 adet PDF dosyası birleştirebilirsiniz.", "warning");
                    ResetButtonScript();
                    return;
                }
                // === KURAL SONU ===


                // 3. iTextSharp ile PDF'leri Birleştirme
                using (MemoryStream ms = new MemoryStream())
                {
                    using (Document document = new Document())
                    {
                        using (PdfCopy copy = new PdfCopy(document, ms))
                        {
                            document.Open();

                            foreach (var pdfFile in pdfFiles)
                            {
                                using (PdfReader reader = new PdfReader(pdfFile.InputStream))
                                {
                                    copy.AddDocument(reader);
                                }
                            }
                        }
                    }

                    // 4. Birleştirilmiş Dosyayı Kullanıcıya Gönder
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=BirlestirilmisDosya.pdf");
                    Response.BinaryWrite(ms.ToArray());
                    Response.Flush();
                    Response.End();
                }
            }
            catch (iTextSharp.text.exceptions.InvalidPdfException pdfEx)
            {
                // PDF okuma hatası
                LogError("PDF Birleştirme - Geçersiz PDF formatı.", pdfEx);
                ShowToast("Seçtiğiniz dosyalardan biri bozuk veya geçerli bir PDF değil.", "danger");
                ResetButtonScript();
            }
            catch (Exception ex)
            {
                // Diğer genel hatalar
                LogError("PDF Birleştirme Hatası", ex);
                ShowToast("Dosyalar birleştirilirken beklenmedik bir hata oluştu.", "danger");
                ResetButtonScript();
            }
        }

        /// <summary>
        /// Bir hata veya uyarı durumunda, devre dışı bırakılan butonu
        /// istemci tarafında (client-side) tekrar aktif eder.
        /// </summary>
        private void ResetButtonScript()
        {
            pnlClientScript.Visible = true;
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "ResetBtnScript",
                "enableMergeButton();",
                true);
        }
    }
}