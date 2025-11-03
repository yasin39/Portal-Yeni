using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulBelgeTakip
{
    public partial class Teblig : BasePage
    {       

        /// <summary>
        /// Vergi numarasına göre firma ve denetim detaylarını getirir
        /// </summary>
        private const string GetDenetimDetailsQuery = @"
        SELECT 
            D.ID as DenetimID, 
            F.ID as FirmaID, 
            F.FIRMA_ADI, 
            F.VERGI_NUMARASI, 
            F.FIRMA_ADRESI, 
            B.BELGE_AD as BELGE_TURU,
            D.DENETIM_TARIHI, 
            D.MAKBUZ_NO, 
            D.TEBLIG_TARIHI,
            CASE 
                WHEN D.TEBLIG_TARIHI IS NULL THEN 'Tebliğ Edilmedi'
                ELSE 'Tebliğ Edildi'
            END as TEBLIG_DURUMU
        FROM FIRMALAR F
        INNER JOIN DENETIMLER D ON F.ID = D.FIRMA_ID
        LEFT JOIN BELGELER B ON F.BELGE_TIPI = B.ID
        WHERE F.VERGI_NUMARASI = @VERGINO
        ORDER BY D.DENETIM_TARIHI DESC";

        /// <summary>
        /// Tebliğ tarihini günceller
        /// </summary>
        private const string UpdateTebligTarihiQuery = @"
        UPDATE DENETIMLER 
        SET TEBLIG_TARIHI = @TebligTarihi 
        WHERE ID = @DenetimID";

        // --- Orijinal Sınıf Metotları ---

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (!CheckPermission(Sabitler.BELGE_TAKIP_FIRMALAR))
                //{
                //    return;
                //}

                InitializeUI();
            }
        }

        private void InitializeUI()
        {
            ClearForm();
        }

        private void ClearForm()
        {
            ClearFormControls(TxtTebligTarihi);
            HdnSelectedTebligDate.Value = string.Empty;
            LblDenetimID.Text = string.Empty;
            PnlDenetim.Visible = false;

            ClientScript.RegisterStartupScript(this.GetType(), "clearFlatpickr",
                $"if(document.getElementById('{TxtTebligTarihi.ClientID}')._flatpickr){{ document.getElementById('{TxtTebligTarihi.ClientID}')._flatpickr.clear(); }}", true);
        }

        protected void BtnFirmaGetir_Click(object sender, EventArgs e)
        {
            ClearForm();

            if (string.IsNullOrWhiteSpace(TxtVergiNo.Text))
            {
                ShowToast("Vergi numarası boş olamaz.", "warning");
                return;
            }

            try
            {
                var parametreler = CreateParameters(("@VERGINO", TxtVergiNo.Text.Trim()));

                // DEĞİŞİKLİK: Sorgu yerel değişkenden alındı
                DataTable dt = ExecuteDataTable(GetDenetimDetailsQuery, parametreler);

                if (dt.Rows.Count == 0)
                {
                    ShowToast("Firma bulunamadı.", "warning");
                    GvDenetim.DataSource = null;
                    GvDenetim.DataBind();
                    return;
                }

                GvDenetim.DataSource = dt;
                GvDenetim.DataBind();

                ShowToast($"{dt.Rows.Count} kayıt bulundu.", "info");
            }
            catch (Exception ex)
            {
                LogError("Firma bilgileri getirilirken hata", ex);
                ShowToast("Firma bilgileri getirilirken bir hata oluştu.", "danger");
            }
        }

        protected void GvDenetim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GvDenetim.SelectedIndex != -1)
            {
                if (GvDenetim.SelectedDataKey == null)
                {
                    ShowToast("DataKey null geldi.", "danger");
                    return;
                }

                var denetimID = GvDenetim.SelectedDataKey.Values["DenetimID"];
                var tebligDurumu = GvDenetim.SelectedDataKey.Values["TEBLIG_DURUMU"];

                if (denetimID == null || string.IsNullOrEmpty(denetimID.ToString()))
                {
                    ShowToast("DenetimID alınamadı.", "danger");
                    return;
                }

                if (string.IsNullOrEmpty(tebligDurumu?.ToString()))
                {
                    ShowToast("Tebliğ durumu alınamadı.", "danger");
                    return;
                }

                if (tebligDurumu.ToString() == "Tebliğ Edildi")
                {
                    ShowToast("Bu denetim zaten tebliğ edilmiş.", "warning");
                    return;
                }

                LblDenetimID.Text = denetimID.ToString();
                PnlDenetim.Visible = true;
            }
            else
            {
                ShowToast("Hiçbir satır seçilmedi.", "danger");
            }
        }

        protected void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                CultureInfo trCulture = new CultureInfo("tr-TR");
                DateTime selectedDate = DateTime.Parse(HdnSelectedTebligDate.Value, trCulture);

                var parametreler = CreateParameters(
                    ("@TebligTarihi", selectedDate),
                    ("@DenetimID", Convert.ToInt32(LblDenetimID.Text))
                );

                // DEĞİŞİKLİK: Sorgu yerel değişkenden alındı
                int rowsAffected = ExecuteNonQuery(UpdateTebligTarihiQuery, parametreler);

                if (rowsAffected > 0)
                {
                    ShowToast("Tebliğ tarihi başarıyla kaydedildi.", "success");
                    LogInfo($"Tebliğ tarihi kaydedildi. DenetimID: {LblDenetimID.Text}, Tarih: {selectedDate:dd.MM.yyyy}");

                    PnlDenetim.Visible = false;
                    ClearForm();

                    if (!string.IsNullOrWhiteSpace(TxtVergiNo.Text))
                    {
                        BtnFirmaGetir_Click(null, null);
                    }
                }
                else
                {
                    ShowToast("Tebliğ tarihi kaydedilemedi.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Tebliğ tarihi kaydedilirken hata", ex);
                ShowToast("İşlem sırasında bir hata oluştu.", "danger");
            }
        }

        protected void CustomValidatorTebligTarih_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(HdnSelectedTebligDate.Value))
            {
                CultureInfo trCulture = new CultureInfo("tr-TR");
                if (DateTime.TryParse(HdnSelectedTebligDate.Value, trCulture, DateTimeStyles.None, out DateTime selectedDate))
                {
                    if (selectedDate.DayOfWeek == DayOfWeek.Saturday || selectedDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        args.IsValid = false;
                        ((CustomValidator)source).ErrorMessage = "Hafta sonu tarihleri seçilemez.";
                        return;
                    }

                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                    ((CustomValidator)source).ErrorMessage = "Geçersiz tarih formatı.";
                }
            }
            else
            {
                args.IsValid = false;
                ((CustomValidator)source).ErrorMessage = "Tebliğ tarihi zorunludur.";
            }
        }
    }
}