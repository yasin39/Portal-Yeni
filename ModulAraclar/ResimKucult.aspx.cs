using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base; // BasePage'den türetmek için gerekli

namespace Portal.ModulAraclar
{
    public partial class ResimKucult : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Küçültme oranlarını DropDownList'e yükle
                PopulateRatios();

                // İşlem sırasında butonu devre dışı bırakmak için
                btnKucult.OnClientClick = string.Format(
                    "this.disabled=true; this.value='{0}'; {1};",
                    "İşleniyor...",
                    Page.ClientScript.GetPostBackEventReference(btnKucult, "")
                );
            }
        }

        /// <summary>
        /// Küçültme oranı seçeneklerini DropDownList'e ekler.
        /// </summary>
        private void PopulateRatios()
        {
            ddlOran.Items.Clear();
            ddlOran.Items.Add(new ListItem("Orjinal Boyutun %75'i (Az Küçült)", "0.75"));
            ddlOran.Items.Add(new ListItem("Orjinal Boyutun %50'si (Orta Küçült)", "0.50"));
            ddlOran.Items.Add(new ListItem("Orjinal Boyutun %25'i (Çok Küçült)", "0.25"));

            // Varsayılan olarak %50 seçili gelsin
            ddlOran.SelectedValue = "0.50";
        }

        /// <summary>
        /// Resmi küçültme işlemini başlatır.
        /// </summary>
        protected void btnKucult_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Dosya Yüklendi mi?
                if (!FileUploadControl.HasFile || FileUploadControl.PostedFile.ContentLength == 0)
                {
                    ShowToast("Lütfen bir resim dosyası seçin.", "warning");
                    ResetButtonScript();
                    return;
                }

                HttpPostedFile file = FileUploadControl.PostedFile;
                string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                // 2. Geçerli Resim Formatı mı? (Uzantıya göre)
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ShowToast("Lütfen geçerli bir resim dosyası (.jpg, .png, .bmp, .gif) seçin.", "warning");
                    ResetButtonScript();
                    return;
                }

                // 3. Oranı Al
                double ratio = Convert.ToDouble(ddlOran.SelectedValue);

                // 4. Resmi Yeniden Boyutlandır
                byte[] resizedImageBytes = ResizeImage(file.InputStream, ratio, fileExtension);

                // 5. Küçültülmüş Resmi Kullanıcıya İndir
                Response.Clear();
                Response.ContentType = file.ContentType; // Orjinal dosya tipini koru
                Response.AddHeader("content-disposition", $"attachment;filename=Kucultulmus_{file.FileName}");
                Response.BinaryWrite(resizedImageBytes);
                Response.End();

            }
            catch (OutOfMemoryException)
            {
                // System.Drawing.Image.FromStream, dosya resim değilse bu hatayı fırlatır.
                LogError("Resim Küçültme - Geçersiz resim formatı veya bozuk dosya.", null);
                ShowToast("Seçtiğiniz dosya bozuk veya geçerli bir resim formatında değil.", "danger");
                ResetButtonScript();
            }
            catch (Exception ex)
            {
                LogError("Resim Küçültme Hatası", ex);
                ShowToast("Resim küçültülürken beklenmedik bir hata oluştu.", "danger");
                ResetButtonScript();
            }
        }

        /// <summary>
        /// Bir resmi hafızada (in-memory) yeniden boyutlandıran ana metot.
        /// </summary>
        /// <param name="imageStream">Orjinal resmin stream'i</param>
        /// <param name="ratio">Küçültme oranı (örn: 0.50)</param>
        /// <param name="fileExtension">Dosya uzantısı (formatı belirlemek için)</param>
        /// <returns>Yeniden boyutlandırılmış resmin byte dizisi</returns>
        private byte[] ResizeImage(Stream imageStream, double ratio, string fileExtension)
        {
            using (var originalImage = System.Drawing.Image.FromStream(imageStream))
            {
                // 1. Yeni boyutları hesapla
                int newWidth = (int)(originalImage.Width * ratio);
                int newHeight = (int)(originalImage.Height * ratio);

                // 2. Yeni bir boş Bitmap (resim tuvali) oluştur
                using (var newBitmap = new Bitmap(newWidth, newHeight))
                {
                    // 3. Yüksek kalitede çizim yapmak için Graphics nesnesi oluştur
                    using (var g = Graphics.FromImage(newBitmap))
                    {
                        // En iyi küçültme kalitesi için ayarlar
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.CompositingQuality = CompositingQuality.HighQuality;

                        // Orjinal resmi yeni boyutlarda bu tuvale çiz
                        g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                    }

                    // 4. Yeni resmi hafızada bir MemoryStream'e kaydet
                    using (var ms = new MemoryStream())
                    {
                        ImageFormat format = GetImageFormat(fileExtension);

                        // EĞER JPEG İSE: Kaliteyi manuel olarak ayarlayarak optimize et
                        if (format == ImageFormat.Jpeg)
                        {
                            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
                            EncoderParameters encoderParams = new EncoderParameters(1);
                            // %85 kalite (iyi denge)
                            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 85L);
                            newBitmap.Save(ms, jpegCodec, encoderParams);
                        }
                        else
                        {
                            // PNG, GIF, BMP ise formatı koruyarak kaydet
                            newBitmap.Save(ms, format);
                        }

                        return ms.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Dosya uzantısına göre iTextSharp ImageFormat nesnesini döndürür.
        /// </summary>
        private ImageFormat GetImageFormat(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".png":
                    return ImageFormat.Png;
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".gif":
                    return ImageFormat.Gif;
                case ".jpg":
                case ".jpeg":
                default:
                    return ImageFormat.Jpeg;
            }
        }

        /// <summary>
        /// Belirli bir MIME tipine sahip resim kodlayıcısını (encoder) bulur.
        /// (JPEG kalitesini ayarlamak için gereklidir)
        /// </summary>
        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            return ImageCodecInfo.GetImageEncoders().FirstOrDefault(codec => codec.MimeType == mimeType);
        }

        /// <summary>
        /// Hata durumunda butonu tekrar aktif eden script'i tetikler.
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