using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;
// using Portal.SQL; // ==> Bu satır kaldırıldı

namespace Portal.ModulBelgeTakip
{
    /// <summary>
    /// Firma belge kayıt sayfası
    /// Vergi numarası ile firma arayıp, belge bilgilerini kaydetme işlemlerini yapar
    /// </summary>
    public partial class BelgeKayit : BasePage
    {
        #region SQL Sorguları

        /// <summary>
        /// Vergi numarasına göre belge almamış firmaları getirir
        /// (BelgeSorgu.cs dosyasından taşındı)
        /// </summary>
        private const string SqlGetFirmaByVergiNo = @"
            SELECT 
                F.ID, 
                F.FIRMA_ADI, 
                F.VERGI_NUMARASI, 
                F.FIRMA_ADRESI, 
                B.BELGE_AD, 
                F.BELGE_ALDIMI
            FROM FIRMALAR F 
            LEFT JOIN BELGELER B ON F.BELGE_TIPI = B.ID 
            WHERE F.VERGI_NUMARASI LIKE @VERGINO + '%' 
                AND F.BELGE_ALDIMI = 0
               ";

        /// <summary>
        /// Firma belge bilgilerini günceller
        /// BELGE_ALMA_TARIHI, BELGE_NUMARASI ve BELGE_ALDIMI alanlarını set eder
        /// (BelgeSorgu.cs dosyasından taşındı)
        /// </summary>
        private const string SqlUpdateBelgeBilgileri = @"
            UPDATE FIRMALAR 
            SET 
                BELGE_ALMA_TARIHI = @BELGETARIH, 
                BELGE_NUMARASI = @BELGENO,
                BELGE_ALDIMI = 1
            WHERE ID = @ID 
                AND BELGE_ALDIMI = 0";

        #endregion

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (!CheckPermission(Sabitler.BELGE_TAKIP_FIRMALAR))
                //{
                //    return; // BasePage otomatik redirect yapar
                //}

                InitializePage();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sayfa ilk yüklendiğinde çalışır
        /// </summary>
        private void InitializePage()
        {
            PanelBelge.Visible = false;
            TxtVergiNo.Focus();
        }

        /// <summary>
        /// Vergi numarası validasyonu
        /// </summary>
        private bool ValidateVergiNo()
        {
            if (string.IsNullOrWhiteSpace(TxtVergiNo.Text))
            {
                ShowToast("Lütfen vergi numarası giriniz.", "warning");
                return false;
            }

            string VergiNo = TxtVergiNo.Text.Trim();
            int Uzunluk = VergiNo.Length;

            if (Uzunluk != 10 && Uzunluk != 11)
            {
                ShowToast("Vergi numarası 10 veya 11 haneli olmalıdır.", "warning");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Belge kayıt validasyonu
        /// </summary>
        private bool ValidateBelgeKayit()
        {
            if (string.IsNullOrEmpty(lblID.Text))
            {
                ShowToast("Lütfen önce firma seçiniz.", "warning");
                return false;
            }

            if (string.IsNullOrEmpty(hdnSelectedDate.Value))
            {
                ShowToast("Lütfen belge tarihini seçiniz.", "warning");
                return false;
            }

            DateTime TempTarih;
            if (!DateTime.TryParseExact(hdnSelectedDate.Value, "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out TempTarih))
            {
                ShowToast("Belge tarihi formatı geçersiz.", "danger");
                return false;
            }

            if (TempTarih > DateTime.Now.Date)
            {
                ShowToast("Belge tarihi gelecek bir tarih olamaz.", "warning");
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtBelge1.Text) || string.IsNullOrWhiteSpace(TxtBelge2.Text))
            {
                ShowToast("Lütfen belge numarasını tam olarak giriniz.", "warning");
                return false;
            }

            if (TxtBelge1.Text.Length != 2)
            {
                ShowToast("Belge numarasının ilk kısmı 2 haneli olmalıdır.", "warning");
                return false;
            }

            if (TxtBelge2.Text.Length != 6)
            {
                ShowToast("Belge numarasının ikinci kısmı 6 haneli olmalıdır.", "warning");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Firma seçimi sonrası belge form alanlarını temizler
        /// </summary>
        private void ClearBelgeForm()
        {
            ClearFormControls(txtBelgeTarihi, TxtBelge1, TxtBelge2);
            hdnSelectedDate.Value = string.Empty;
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Vergi numarasına göre firma bilgilerini getirir
        /// </summary>
        protected void BtnFirmaGetir_Click(object sender, EventArgs e)
        {
            PanelBelge.Visible = false;

            if (!ValidateVergiNo())
            {
                return;
            }

            try
            {
                var Parametreler = CreateParameters(("@VERGINO", TxtVergiNo.Text.Trim()));

                // ==> Sorgu, yerel değişkenden çağrıldı
                DataTable FirmaTablosu = ExecuteDataTable(SqlGetFirmaByVergiNo, Parametreler);

                GvFirma.DataSource = FirmaTablosu;
                GvFirma.DataBind();

                if (FirmaTablosu.Rows.Count == 0)
                {
                    ShowToast("Vergi numarası ile eşleşen belgesiz firma bulunamadı.", "info");
                    LogInfo($"Firma arama - Sonuç yok: {TxtVergiNo.Text.Trim()}");
                }
                else
                {
                    ShowToast($"{FirmaTablosu.Rows.Count} adet firma bulundu.", "success");
                }
            }
            catch (Exception ex)
            {
                LogError("Firma arama hatası", ex);
                ShowToast("Firma arama sırasında bir hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Belge bilgilerini kaydeder
        /// </summary>
        protected void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (!ValidateBelgeKayit())
            {
                return;
            }

            try
            {
                DateTime BelgeTarihi;
                if (!DateTime.TryParseExact(hdnSelectedDate.Value, "dd.MM.yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out BelgeTarihi))
                {
                    ShowToast("Tarih formatı geçersiz.", "danger");
                    return;
                }

                string BelgeNo = $"{TxtBelge1.Text.Trim()}.{TxtBelge2.Text.Trim()}";
                int FirmaId = Convert.ToInt32(lblID.Text);
                string KullaniciAdi = Session["KullaniciAdi"]?.ToString() ?? "System";

                var Parametreler = CreateParameters(
                    ("@BELGETARIH", BelgeTarihi),
                    ("@BELGENO", BelgeNo),
                    ("@ID", FirmaId),
                    ("@KULLANICI", KullaniciAdi)
                );

                // ==> Sorgu, yerel değişkenden çağrıldı
                int EtkilenenSatir = ExecuteNonQuery(SqlUpdateBelgeBilgileri, Parametreler);

                if (EtkilenenSatir > 0)
                {
                    // ==> Başarılı kayıt sonrası sadece belge formu temizleniyor
                    ClearBelgeForm();

                    ShowToast($"Belge kaydı başarıyla tamamlandı! | Belge No: {BelgeNo} | Tarih: {BelgeTarihi:dd.MM.yyyy}", "success");
                    LogInfo($"Belge kaydı başarılı - Firma: {lblFirmaAdi.Text}, Belge No: {BelgeNo}");

                    // GridView'i güncelle
                    BtnFirmaGetir_Click(sender, e);
                }
                else
                {
                    ShowToast("Belge kaydı başarısız oldu. Firma zaten belgeli olabilir.", "warning");
                }
            }
            catch (Exception ex)
            {
                // ==> BasePage LogError ve ShowToast kullanılıyor
                LogError("Belge kayıt hatası", ex);
                ShowToast("Belge kayıt işlemi sırasında bir hata oluştu: " + ex.Message, "danger");
            }
        }

        /// <summary>
        /// Tarih validasyonu - Server-side
        /// </summary>
        protected void CustomValidatorTarih_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(hdnSelectedDate.Value))
            {
                args.IsValid = false;
                return;
            }

            DateTime TempTarih;
            if (!DateTime.TryParseExact(hdnSelectedDate.Value, "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out TempTarih))
            {
                args.IsValid = false;
                return;
            }

            args.IsValid = TempTarih <= DateTime.Now.Date;
        }

        #endregion

        #region GridView Events

        /// <summary>
        /// GridView'da firma seçildiğinde çalışır
        /// </summary>
        protected void GvFirma_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (GvFirma.SelectedIndex == -1)
                {
                    return;
                }

                GridViewRow SeciliSatir = GvFirma.SelectedRow;

                if (SeciliSatir != null)
                {
                    lblFirmaAdi.Text = SeciliSatir.Cells[1].Text;
                    lblAdres.Text = SeciliSatir.Cells[3].Text;
                    lblBelgeTuru.Text = SeciliSatir.Cells[4].Text;
                    lblID.Text = GvFirma.SelectedDataKey.Value.ToString();

                    PanelBelge.Visible = true;
                    ClearBelgeForm();

                    ShowToast($"Firma seçildi: {lblFirmaAdi.Text}", "info");
                }
            }
            catch (Exception ex)
            {
                // ==> BasePage LogError ve ShowToast kullanılıyor
                LogError("Firma seçim hatası", ex);
                ShowToast("Firma seçimi sırasında hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// GridView sayfalama işlemi
        /// </summary>
        protected void GvFirma_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GvFirma.PageIndex = e.NewPageIndex;
                BtnFirmaGetir_Click(sender, e);
            }
            catch (Exception ex)
            {
                // ==> BasePage LogError ve ShowToast kullanılıyor
                LogError("Sayfa değiştirme hatası", ex);
                ShowToast("Sayfa değiştirme sırasında hata oluştu.", "danger");
            }
        }

        #endregion
    }
}