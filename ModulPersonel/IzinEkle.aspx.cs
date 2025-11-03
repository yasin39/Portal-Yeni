using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Portal.ModulPersonel
{
    public partial class IzinEkle : BasePage
    {
        // ViewState'te saklanan SeciliIzinId
        private int SeciliIzinId
        {
            get { return ViewState["SeciliIzinId"] != null ? (int)ViewState["SeciliIzinId"] : 0; }
            set { ViewState["SeciliIzinId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPermission(100);
            }
        }

        protected void txtSicilNo_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSicilNo.Text.Trim()))
            {
                LoadPersonelInfo();
            }
        }

        protected void btnPersonelBul_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSicilNo.Text.Trim()))
            {
                LoadPersonelInfo();
            }
            else
            {
                ShowToast("Lütfen sicil no giriniz", "warning");
            }
        }

        private void LoadPersonelInfo()
        {
            try
            {
                string query = @"SELECT TcKimlikNo, Adi, Soyad, Unvan, GorevYaptigiBirim, 
                                Statu, Devredenizin, cariyilizni, toplamizin, Resim 
                                FROM personel 
                                WHERE SicilNo = @SicilNo";

                var parameters = CreateParameters(("@SicilNo", txtSicilNo.Text.Trim()));
                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    txtTcKimlikNo.Text = row["TcKimlikNo"].ToString();
                    string adiSoyadi = $"{row["Adi"]} {row["Soyad"]}";
                    lblPersonelAd.Text = adiSoyadi;
                    txtUnvan.Text = row["Unvan"].ToString();
                    txtBirim.Text = row["GorevYaptigiBirim"].ToString();
                    txtStatu.Text = row["Statu"].ToString();

                    lblDevredenIzin.Text = row["Devredenizin"]?.ToString() ?? "0";
                    lblCariIzin.Text = row["cariyilizni"]?.ToString() ?? "0";

                    int devredenIzin = Convert.ToInt32(lblDevredenIzin.Text);
                    int cariIzin = Convert.ToInt32(lblCariIzin.Text);
                    lblToplamIzin.Text = (devredenIzin + cariIzin).ToString();

                    string resimYolu = row["Resim"]?.ToString();
                    if (!string.IsNullOrEmpty(resimYolu))
                    {
                        imgPersonel.ImageUrl = resimYolu;
                    }

                    pnlPersonelBilgi.Visible = true;
                    pnlIzinDetay.Visible = true;
                    pnlIzinGecmis.Visible = true;

                    LoadIzinGecmisi();
                    SetFormModeInsert(btnKaydet, btnGuncelle, btnSil, btnVazgec);
                    btnYeniKayit.Visible = false;
                }
                else
                {
                    ShowToast("Personel bulunamadı", "error");
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                LogError("LoadPersonelInfo hatası", ex);
                ShowToast("Personel bilgileri yüklenirken hata oluştu", "error");
            }
        }

        private void LoadIzinGecmisi()
        {
            try
            {
                string query = @"SELECT * FROM personel_izin 
                                WHERE Sicil_No = @SicilNo 
                                ORDER BY id DESC";

                var parameters = CreateParameters(("@SicilNo", txtSicilNo.Text.Trim()));
                DataTable dt = ExecuteDataTable(query, parameters);

                gvIzinler.DataSource = dt;
                gvIzinler.DataBind();
            }
            catch (Exception ex)
            {
                LogError("LoadIzinGecmisi hatası", ex);
                ShowToast("İzin geçmişi yüklenirken hata oluştu", "error");
            }
        }

        protected void txtIzinSuresi_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtIzneBaslamaTarihi.Text) &&
                !string.IsNullOrEmpty(txtIzinSuresi.Text))
            {
                CalculateDates();
            }
        }

        private void CalculateDates()
        {
            try
            {
                DateTime baslamaTarihi = DateTime.ParseExact(txtIzneBaslamaTarihi.Text, "dd/MM/yyyy",
    System.Globalization.CultureInfo.InvariantCulture);
                double izinSuresi = Convert.ToDouble(txtIzinSuresi.Text.Replace(',', '.'));
                string statu = txtStatu.Text;

                if (statu == "Memur")
                {
                    DateTime bitisTarihi = baslamaTarihi.AddDays(izinSuresi).AddDays(-1);
                    DateTime goreveBaslama = baslamaTarihi.AddDays(izinSuresi);

                    txtIzinBitisTarihi.Text = FormatDateTurkish(bitisTarihi);
                    txtGoreveBaslamaTarihi.Text = FormatDateTurkish(goreveBaslama);
                }
                else
                {
                    int pazarSayisi = CalculateSundayCount(baslamaTarihi, izinSuresi);

                    DateTime bitisTarihi = baslamaTarihi.AddDays(izinSuresi + pazarSayisi).AddDays(-1);
                    DateTime goreveBaslama = baslamaTarihi.AddDays(izinSuresi + pazarSayisi);

                    txtIzinBitisTarihi.Text = FormatDateTurkish(bitisTarihi);
                    txtGoreveBaslamaTarihi.Text = FormatDateTurkish(goreveBaslama);
                }
            }
            catch (Exception ex)
            {
                LogError("CalculateDates hatası", ex);
            }
        }

        private int CalculateSundayCount(DateTime baslangic, double izinSuresi)
        {
            int pazarSayisi = (int)izinSuresi / 7;
            DateTime bitis = baslangic.AddDays(izinSuresi + pazarSayisi);
            int gunSayisi = 0;

            DateTime temp = baslangic;
            while (temp <= bitis)
            {
                if (temp.DayOfWeek == DayOfWeek.Sunday)
                {
                    gunSayisi++;
                }
                temp = temp.AddDays(1);
            }

            return gunSayisi;
        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                if (!CheckIzinConflict())
                {
                    lblMesaj.Visible = true;
                    lblMesaj.Text = "Personelin seçilen tarihlerde zaten tanımlı izni bulunmaktadır. Farklı tarih seçiniz.";
                    return;
                }

                string izinTuru = ddlIzinTuru.SelectedValue;
                DateTime baslamaTarihi = DateTime.ParseExact(txtIzneBaslamaTarihi.Text, "dd/MM/yyyy",
    System.Globalization.CultureInfo.InvariantCulture);
                double izinSuresi = Convert.ToDouble(txtIzinSuresi.Text.Replace(',', '.'));

                DateTime bitisTarihi = Convert.ToDateTime(txtIzinBitisTarihi.Text);
                DateTime goreveBaslama = Convert.ToDateTime(txtGoreveBaslamaTarihi.Text);

                string query = @"INSERT INTO personel_izin 
                                (Sicil_No, Tc_No, Adi_Soyadi, Statu, Devreden_izin, Cari_izin, 
                                izin_turu, izin_Suresi, izne_Baslama_Tarihi, izin_Bitis_Tarihi, 
                                Goreve_Baslama_Tarihi, Aciklama, Kayit_Tarihi, Kayit_Kullanici)
                                VALUES 
                                (@SicilNo, @TcNo, @AdiSoyadi, @Statu, @DevredenIzin, @CariIzin, 
                                @IzinTuru, @IzinSuresi, @BaslamaTarihi, @BitisTarihi, 
                                @GoreveBaslama, @Aciklama, @KayitTarihi, @KayitKullanici)";

                var parameters = CreateParameters(
                    ("@SicilNo", txtSicilNo.Text.Trim()),
                    ("@TcNo", txtTcKimlikNo.Text),
                    ("@AdiSoyadi", lblPersonelAd.Text),
                    ("@Statu", txtStatu.Text),
                    ("@DevredenIzin", lblDevredenIzin.Text),
                    ("@CariIzin", lblCariIzin.Text),
                    ("@IzinTuru", izinTuru),
                    ("@IzinSuresi", izinSuresi),
                    ("@BaslamaTarihi", baslamaTarihi),
                    ("@BitisTarihi", bitisTarihi),
                    ("@GoreveBaslama", goreveBaslama),
                    ("@Aciklama", txtAciklama.Text.Trim()),
                    ("@KayitTarihi", DateTime.Now),
                    ("@KayitKullanici", CurrentUserName)
                );

                ExecuteNonQuery(query, parameters);

                if (izinTuru == "Yıllık İzin")
                {
                    UpdateYillikIzin(izinSuresi);
                }

                if (izinTuru != "Yıllık İzin")
                {
                    GeneratePdfReport();
                }

                ShowToast("İzin kaydı başarıyla eklendi", "success");
                LoadIzinGecmisi();
                ClearFormControls(txtIzinSuresi, txtIzneBaslamaTarihi, txtIzinBitisTarihi,
                    txtGoreveBaslamaTarihi, txtAciklama);
                ResetDropDownLists(ddlIzinTuru);
                lblMesaj.Visible = false;
            }
            catch (Exception ex)
            {
                LogError("btnKaydet_Click hatası", ex);
                ShowToast("İzin kaydı eklenirken hata oluştu", "error");
            }
        }

        private bool CheckIzinConflict()
        {
            try
            {
                DateTime baslamaTarihi = DateTime.ParseExact(txtIzneBaslamaTarihi.Text, "dd/MM/yyyy",
    System.Globalization.CultureInfo.InvariantCulture);
                DateTime bitisTarihi = DateTime.ParseExact(txtIzinBitisTarihi.Text.Replace(".", "/"), "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture);

                string query = @"SELECT COUNT(*) AS IzinSayisi 
                                FROM personel_izin 
                                WHERE Sicil_No = @SicilNo 
                                AND id != @IzinId
                                AND (
                                    (izne_Baslama_Tarihi BETWEEN @BaslamaTarihi AND @BitisTarihi) 
                                    OR (izin_Bitis_Tarihi BETWEEN @BaslamaTarihi AND @BitisTarihi)
                                )";

                var parameters = CreateParameters(
                    ("@SicilNo", txtSicilNo.Text.Trim()),
                    ("@IzinId", SeciliIzinId),
                    ("@BaslamaTarihi", baslamaTarihi),
                    ("@BitisTarihi", bitisTarihi)
                );

                DataTable dt = ExecuteDataTable(query, parameters);
                int izinSayisi = Convert.ToInt32(dt.Rows[0]["IzinSayisi"]);

                return izinSayisi == 0;
            }
            catch (Exception ex)
            {
                LogError("CheckIzinConflict hatası", ex);
                return false;
            }
        }

        private void UpdateYillikIzin(double izinSuresi)
        {
            try
            {
                int devredenIzin = Convert.ToInt32(lblDevredenIzin.Text);
                int cariIzin = Convert.ToInt32(lblCariIzin.Text);
                int toplamIzin = devredenIzin + cariIzin;
                int kullanilanIzin = (int)izinSuresi;

                if (kullanilanIzin > toplamIzin)
                {
                    ShowToast("Toplam izinden fazla izin kullanılamaz", "error");
                    return;
                }

                int yeniDevredenIzin = devredenIzin;
                int yeniCariIzin = cariIzin;

                if (devredenIzin >= kullanilanIzin)
                {
                    yeniDevredenIzin = devredenIzin - kullanilanIzin;
                }
                else
                {
                    int kalan = kullanilanIzin - devredenIzin;
                    yeniDevredenIzin = 0;
                    yeniCariIzin = cariIzin - kalan;
                }

                int yeniToplamIzin = yeniDevredenIzin + yeniCariIzin;

                string query = @"UPDATE personel 
                                SET Devredenizin = @DevredenIzin, 
                                    cariyilizni = @CariIzin, 
                                    toplamizin = @ToplamIzin 
                                WHERE TcKimlikNo = @TcNo";

                var parameters = CreateParameters(
                    ("@DevredenIzin", yeniDevredenIzin),
                    ("@CariIzin", yeniCariIzin),
                    ("@ToplamIzin", yeniToplamIzin),
                    ("@TcNo", txtTcKimlikNo.Text)
                );

                ExecuteNonQuery(query, parameters);

                lblDevredenIzin.Text = yeniDevredenIzin.ToString();
                lblCariIzin.Text = yeniCariIzin.ToString();
                lblToplamIzin.Text = yeniToplamIzin.ToString();
            }
            catch (Exception ex)
            {
                LogError("UpdateYillikIzin hatası", ex);
                throw;
            }
        }

        protected void gvIzinler_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // 1. ID'yi DataKeys'ten al (Bu satır bir önceki düzeltmeden)
                int id = (int)gvIzinler.DataKeys[gvIzinler.SelectedRow.RowIndex].Value;
                if (id == 0)
                {
                    ShowToast("Geçersiz kayıt seçimi", "error");
                    return;
                }

                SeciliIzinId = id;

                // 2. Veriyi HTML'den okumak yerine veritabanından çek (En Güvenli Yöntem)
                // btnSil_Click metodunuzdaki sorguyu burada da kullanıyoruz
                string query = "SELECT * FROM personel_izin WHERE id = @IzinId";
                var parameters = CreateParameters(("@IzinId", SeciliIzinId));
                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count == 0)
                {
                    ShowToast("Seçilen izin detayı bulunamadı", "error");
                    ResetForm();
                    return;
                }

                DataRow row = dt.Rows[0];

                // 3. Değerleri DataRow'dan ata (String parsing yok)
                string izinTuru = row["izin_turu"].ToString();
                if (!string.IsNullOrEmpty(izinTuru))
                {
                    try
                    {
                        ddlIzinTuru.SelectedValue = izinTuru;
                    }
                    catch
                    {
                        ddlIzinTuru.SelectedIndex = 0; // Değer listede yoksa başa al
                    }
                }

                // İzin süresi
                txtIzinSuresi.Text = row["izin_Suresi"] != DBNull.Value
                    ? Convert.ToDouble(row["izin_Suresi"]).ToString().Replace(".", ",")
                    : "";

                // Tarihleri doğrudan DateTime nesnesi olarak al
                DateTime baslamaTarihi = (DateTime)row["izne_Baslama_Tarihi"];
                DateTime bitisTarihi = (DateTime)row["izin_Bitis_Tarihi"];
                DateTime goreveBaslama = (DateTime)row["Goreve_Baslama_Tarihi"];

                // Textbox'ları kodun geri kalanının (btnGuncelle_Click) beklediği formata ayarla
                // btnGuncelle_Click metodu "dd/MM/yyyy" formatı bekliyor
                txtIzneBaslamaTarihi.Text = baslamaTarihi.ToString("dd/MM/yyyy");
                txtIzinBitisTarihi.Text = bitisTarihi.ToString("dd/MM/yyyy");
                txtGoreveBaslamaTarihi.Text = goreveBaslama.ToString("dd/MM/yyyy");

                // Açıklama
                txtAciklama.Text = row["Aciklama"].ToString();

                // Form modunu güncelle
                SetFormModeUpdate(btnKaydet, btnGuncelle, btnSil, btnVazgec);
                btnYeniKayit.Visible = true;

                // İzin silme yetkisi kontrolü
                bool izinSilmeYetkisi = false;
                try
                {
                    izinSilmeYetkisi = CheckPermission(199);
                }
                catch
                {
                    izinSilmeYetkisi = false;
                }

                btnSil.Visible = izinSilmeYetkisi;
                lblMesaj.Visible = false;
            }
            catch (Exception ex)
            {
                LogError("gvIzinler_SelectedIndexChanged hatası", ex);
                ShowToast("İzin seçilirken kritik bir hata oluştu", "error");
            }
        }

        protected void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || SeciliIzinId == 0) return;

            try
            {
                if (!CheckIzinConflict())
                {
                    lblMesaj.Visible = true;
                    lblMesaj.Text = "Personelin seçilen tarihlerde zaten tanımlı izni bulunmaktadır. Farklı tarih seçiniz.";
                    return;
                }

                GridViewRow row = gvIzinler.SelectedRow;
                string izinTuru = GetGridViewCellTextSafe(row, 6);

                DateTime baslamaTarihi = DateTime.ParseExact(txtIzneBaslamaTarihi.Text, "dd/MM/yyyy",
    System.Globalization.CultureInfo.InvariantCulture);
                DateTime bitisTarihi = DateTime.ParseExact(txtIzinBitisTarihi.Text.Replace(".", "/"), "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture);
                DateTime goreveBaslama = DateTime.ParseExact(txtGoreveBaslamaTarihi.Text.Replace(".", "/"), "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture);
                double izinSuresi = Convert.ToDouble(txtIzinSuresi.Text.Replace(',', '.'));

                string query = @"UPDATE personel_izin 
                                SET Adi_Soyadi = @AdiSoyadi, 
                                    Devreden_izin = @DevredenIzin, 
                                    Cari_izin = @CariIzin, 
                                    izin_turu = @IzinTuru, 
                                    izin_Suresi = @IzinSuresi, 
                                    izne_Baslama_Tarihi = @BaslamaTarihi, 
                                    izin_Bitis_Tarihi = @BitisTarihi, 
                                    Goreve_Baslama_Tarihi = @GoreveBaslama, 
                                    Aciklama = @Aciklama, 
                                    Son_Guncelleme_Tarihi = @GuncellemeTarihi, 
                                    Son_Guncelleyen_Kullanici = @GuncelleyenKullanici 
                                WHERE id = @IzinId";

                var parameters = CreateParameters(
                    ("@AdiSoyadi", lblPersonelAd.Text),
                    ("@DevredenIzin", lblDevredenIzin.Text),
                    ("@CariIzin", lblCariIzin.Text),
                    ("@IzinTuru", ddlIzinTuru.SelectedValue),
                    ("@IzinSuresi", izinSuresi),
                    ("@BaslamaTarihi", baslamaTarihi),
                    ("@BitisTarihi", bitisTarihi),
                    ("@GoreveBaslama", goreveBaslama),
                    ("@Aciklama", txtAciklama.Text.Trim()),
                    ("@GuncellemeTarihi", DateTime.Now),
                    ("@GuncelleyenKullanici", CurrentUserName),
                    ("@IzinId", SeciliIzinId)
                );

                ExecuteNonQuery(query, parameters);

                if (izinTuru != "Yıllık İzin")
                {
                    GeneratePdfReport();
                }

                ShowToast("İzin kaydı başarıyla güncellendi", "success");
                LoadIzinGecmisi();
                ResetForm();
            }
            catch (Exception ex)
            {
                LogError("btnGuncelle_Click hatası", ex);
                ShowToast("İzin kaydı güncellenirken hata oluştu", "error");
            }
        }

        protected void btnSil_Click(object sender, EventArgs e)
        {
            if (SeciliIzinId == 0)
            {
                ShowToast("Lütfen silinecek izni seçiniz", "warning");
                return;
            }

            try
            {
                string query = "SELECT * FROM personel_izin WHERE id = @IzinId";
                var parameters = CreateParameters(("@IzinId", SeciliIzinId));
                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count == 0)
                {
                    ShowToast("Silinecek izin bulunamadı", "error");
                    return;
                }

                DataRow row = dt.Rows[0];
                string izinTuru = row["izin_turu"].ToString();
                double izinSuresi = Convert.ToDouble(row["izin_Suresi"]);

                if (izinTuru == "Yıllık İzin")
                {
                    RestoreYillikIzin(izinSuresi);
                }

                string deleteQuery = "DELETE FROM personel_izin WHERE id = @IzinId";
                var deleteParameters = CreateParameters(("@IzinId", SeciliIzinId));
                ExecuteNonQuery(deleteQuery, deleteParameters);

                ShowToast("İzin kaydı başarıyla silindi", "success");
                LoadIzinGecmisi();
                ResetForm();
            }
            catch (Exception ex)
            {
                LogError("btnSil_Click hatası", ex);
                ShowToast("İzin kaydı silinirken hata oluştu", "error");
            }
        }

        private void RestoreYillikIzin(double izinSuresi)
        {
            try
            {
                int kullanilanIzin = (int)izinSuresi;

                string query = @"UPDATE personel 
                                SET cariyilizni = cariyilizni + @IzinSuresi, 
                                    toplamizin = toplamizin + @IzinSuresi 
                                WHERE SicilNo = @SicilNo";

                var parameters = CreateParameters(
                    ("@IzinSuresi", kullanilanIzin),
                    ("@SicilNo", txtSicilNo.Text.Trim())
                );

                ExecuteNonQuery(query, parameters);

                int cariIzin = Convert.ToInt32(lblCariIzin.Text);
                int toplamIzin = Convert.ToInt32(lblToplamIzin.Text);

                lblCariIzin.Text = (cariIzin + kullanilanIzin).ToString();
                lblToplamIzin.Text = (toplamIzin + kullanilanIzin).ToString();
            }
            catch (Exception ex)
            {
                LogError("RestoreYillikIzin hatası", ex);
                throw;
            }
        }

        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        protected void btnYeniKayit_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            SeciliIzinId = 0;
            ClearFormControls(txtIzinSuresi, txtIzneBaslamaTarihi, txtIzinBitisTarihi,
                txtGoreveBaslamaTarihi, txtAciklama);
            ResetDropDownLists(ddlIzinTuru);
            SetFormModeInsert(btnKaydet, btnGuncelle, btnSil, btnVazgec);
            btnYeniKayit.Visible = false;
            lblMesaj.Visible = false;

            if (gvIzinler.SelectedIndex >= 0)
            {
                gvIzinler.SelectedIndex = -1;
            }
        }

        private void ClearForm()
        {
            pnlPersonelBilgi.Visible = false;
            pnlIzinDetay.Visible = false;
            pnlIzinGecmis.Visible = false;
            txtSicilNo.Text = string.Empty;
            txtTcKimlikNo.Text = string.Empty;
            lblPersonelAd.Text = "-";
            txtUnvan.Text = string.Empty;
            txtBirim.Text = string.Empty;
            txtStatu.Text = string.Empty;
            lblDevredenIzin.Text = "0";
            lblCariIzin.Text = "0";
            lblToplamIzin.Text = "0";
            imgPersonel.ImageUrl = "~/wwwroot/Images/default-avatar.png";
            ResetForm();
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = $"Personel_Izin_Gecmisi_{txtSicilNo.Text}_{DateTime.Now:yyyyMMdd}.xlsx";
                ExportGridViewToExcel(gvIzinler, fileName);
            }
            catch (Exception ex)
            {
                LogError("btnExcelExport_Click hatası", ex);
                ShowToast("Excel dışa aktarılırken hata oluştu", "error");
            }
        }

        private void GeneratePdfReport()
        {
            try
            {
                Document document = new Document(PageSize.A4, 50, 50, 50, 50);
                MemoryStream memoryStream = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

                document.Open();

                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, "windows-1254", BaseFont.EMBEDDED);
                Font titleFont = new Font(bf, 14, Font.BOLD);
                Font headerFont = new Font(bf, 12, Font.BOLD);
                Font normalFont = new Font(bf, 10, Font.NORMAL);
                Font cellFont = new Font(bf, 9, Font.NORMAL);

                Paragraph title1 = new Paragraph("T.C.", titleFont);
                title1.Alignment = Element.ALIGN_CENTER;
                document.Add(title1);

                Paragraph title2 = new Paragraph("ULAŞTIRMA ve ALTYAPI BAKANLIĞI", titleFont);
                title2.Alignment = Element.ALIGN_CENTER;
                document.Add(title2);

                Paragraph title3 = new Paragraph("II. Bölge Müdürlüğü", titleFont);
                title3.Alignment = Element.ALIGN_CENTER;
                document.Add(title3);

                string izinTuru = ddlIzinTuru.SelectedValue;
                Paragraph title4 = new Paragraph($"{izinTuru} Formu", headerFont);
                title4.Alignment = Element.ALIGN_CENTER;
                title4.SpacingAfter = 20f;
                document.Add(title4);

                Paragraph noInfo = new Paragraph($"No : {DateTime.Now:yyMMddHHmmss}", normalFont);
                noInfo.Alignment = Element.ALIGN_RIGHT;
                noInfo.SpacingAfter = 10f;
                document.Add(noInfo);

                PdfPTable mainTable = new PdfPTable(2);
                mainTable.WidthPercentage = 100;
                mainTable.SetWidths(new float[] { 30, 70 });

                AddTableCell(mainTable, "Adı Soyadı / Sicil No", cellFont, true);
                AddTableCell(mainTable, $"{lblPersonelAd.Text} / {txtSicilNo.Text}", cellFont, false);

                AddTableCell(mainTable, "Statüsü / Kadrosu", cellFont, true);
                AddTableCell(mainTable, $"{txtStatu.Text} / {txtUnvan.Text}", cellFont, false);

                AddTableCell(mainTable, "Birimi", cellFont, true);
                AddTableCell(mainTable, txtBirim.Text, cellFont, false);

                AddTableCell(mainTable, "İzin Türü", cellFont, true);
                AddTableCell(mainTable, izinTuru, cellFont, false);

                AddTableCell(mainTable, "İzin Sebebi Açıklama", cellFont, true);
                AddTableCell(mainTable, txtAciklama.Text, cellFont, false);

                AddTableCell(mainTable, "İzin Başlama / Bitiş Tarihi", cellFont, true);
                AddTableCell(mainTable, $"{txtIzinBitisTarihi.Text} -- / {txtGoreveBaslamaTarihi.Text} --", cellFont, false);

                document.Add(mainTable);

                document.Add(new Paragraph(" ", normalFont));

                PdfPTable signTable = new PdfPTable(2);
                signTable.WidthPercentage = 100;
                signTable.SetWidths(new float[] { 50, 50 });

                PdfPCell talepEdenCell = new PdfPCell(new Phrase($"TALEP EDEN\n\n{CurrentUserName}\n\n.../{DateTime.Now:MM/yyyy}", cellFont));
                talepEdenCell.HorizontalAlignment = Element.ALIGN_CENTER;
                talepEdenCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                talepEdenCell.MinimumHeight = 80f;
                signTable.AddCell(talepEdenCell);

                PdfPCell birimAmiriCell = new PdfPCell(new Phrase($"BİRİM AMİRİ\n\n\n\n{DateTime.Now:dd.MM.yyyy}", cellFont));
                birimAmiriCell.HorizontalAlignment = Element.ALIGN_CENTER;
                birimAmiriCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                birimAmiriCell.MinimumHeight = 80f;
                signTable.AddCell(birimAmiriCell);

                document.Add(signTable);

                PdfPTable uygundurTable = new PdfPTable(1);
                uygundurTable.WidthPercentage = 100;

                PdfPCell uygundurCell = new PdfPCell(new Phrase($"UYGUNDUR\n\n\n\n.../{DateTime.Now:MM/yyyy}", cellFont));
                uygundurCell.HorizontalAlignment = Element.ALIGN_CENTER;
                uygundurCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                uygundurCell.MinimumHeight = 80f;
                uygundurTable.AddCell(uygundurCell);

                document.Add(uygundurTable);

                Paragraph footer = new Paragraph($"{DateTime.Now:dd.MM.yyyy HH:mm:ss}", new Font(bf, 8, Font.NORMAL));
                footer.Alignment = Element.ALIGN_RIGHT;
                footer.SpacingBefore = 20f;
                document.Add(footer);

                document.Close();

                byte[] pdfBytes = memoryStream.ToArray();
                string fileName = $"Izin_Formu_{txtSicilNo.Text}_{DateTime.Now:yyyyMMddHHmmss}.pdf";

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                Response.BinaryWrite(pdfBytes);
                Response.End();
            }
            catch (Exception ex)
            {
                LogError("GeneratePdfReport hatası", ex);
                ShowToast("PDF oluşturulurken hata oluştu", "error");
            }
        }

        private void AddTableCell(PdfPTable table, string text, Font font, bool isHeader)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Padding = 5;

            if (isHeader)
            {
                cell.BackgroundColor = new BaseColor(220, 220, 220);
            }

            table.AddCell(cell);
        }
    }
}