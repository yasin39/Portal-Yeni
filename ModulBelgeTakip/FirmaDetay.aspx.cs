using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulBelgeTakip
{
    public partial class FirmaDetay : BasePage
    {
        #region Properties

        private int FirmaID
        {
            get
            {
                if (ViewState["FirmaID"] != null)
                    return (int)ViewState["FirmaID"];
                return 0;
            }
            set { ViewState["FirmaID"] = value; }
        }

        #endregion

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // ==> Yetki kontrolü
                //if (!CheckPermission(Sabitler.BelgeTakip))
                //{
                //    return; // BasePage zaten yönlendirir
                //}

                // QueryString'den Firma ID'sini al
                if (Request.QueryString["ID"] != null && int.TryParse(Request.QueryString["ID"], out int firmaId))
                {
                    FirmaID = firmaId;
                    FirmaDetaylariniYukle(firmaId);
                    DenetimGecmisiniYukle(firmaId);
                    LogInfo($"Firma detay sayfası açıldı. Firma ID: {firmaId}");
                }
                else
                {
                    HataMesajiGoster("Geçersiz firma ID'si.");
                    Response.Redirect("TumFirmalar.aspx");
                }
            }
        }

        #endregion

        #region Firma Detayları Yükleme

        private void FirmaDetaylariniYukle(int firmaId)
        {
            try
            {
                string query = @"
                    SELECT F.*, I.IL_AD, IC.ILCE_AD, B.BELGE_AD, FT.FIRMA_TIP_AD, K.KATEGORI_AD
                    FROM FIRMALAR F
                    INNER JOIN ILLER I ON F.IL = I.IL_ID
                    INNER JOIN ILCELER IC ON F.ILCE = IC.ILCE_ID
                    INNER JOIN BELGELER B ON F.BELGE_TIPI = B.ID
                    INNER JOIN FIRMA_TIP FT ON F.FIRMA_TIPI = FT.FIRMA_TIP_ID
                    LEFT JOIN KATEGORILER K ON F.KATEGORI_TIPI = K.ID
                    WHERE F.ID = @FirmaID";

                var parametreler = CreateParameters(("@FirmaID", firmaId));
                DataTable dt = ExecuteDataTable(query, parametreler);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    FirmaBilgileriniDoldur(row);
                }
                else
                {
                    HataMesajiGoster("Firma bilgileri bulunamadı.");
                    LogError($"Firma bulunamadı. ID: {firmaId}");
                }
            }
            catch (Exception ex)
            {
                LogError("Firma detayları yükleme hatası", ex);
                HataMesajiGoster("Firma bilgileri yüklenirken bir hata oluştu.");
            }
        }

        private void FirmaBilgileriniDoldur(DataRow row)
        {
            try
            {
                // Firma Bilgileri
                lblVergiNo.Text = row["VERGI_NUMARASI"]?.ToString() ?? "-";
                lblFirmaAdi.Text = row["FIRMA_ADI"]?.ToString() ?? "-";
                lblIlIlce.Text = $"{row["IL_AD"]} / {row["ILCE_AD"]}";
                lblAdres.Text = row["FIRMA_ADRESI"]?.ToString() ?? "-";
                lblFirmaTipi.Text = row["FIRMA_TIP_AD"]?.ToString() ?? "-";
                lblKategori.Text = row["KATEGORI_AD"]?.ToString() ?? "Kategorisiz";

                // Belge Bilgileri
                lblBelgeTuru.Text = row["BELGE_AD"]?.ToString() ?? "-";

                // Belge durumunu renkli göster
                bool belgeAldimi = Convert.ToBoolean(row["BELGE_ALDIMI"]);
                lblBelgeDurumu.Text = belgeAldimi ? "✓ Belgeli" : "✗ Belgesiz";
                lblBelgeDurumu.CssClass = belgeAldimi ? "status-success" : "status-danger";

                lblBelgeNo.Text = row["BELGE_NUMARASI"]?.ToString() ?? "-";

                // Tarihleri kontrol et ve formatla
                lblBelgeAlmaTarihi.Text = row["BELGE_ALMA_TARIHI"] != DBNull.Value
                    ? FormatDateTurkish(Convert.ToDateTime(row["BELGE_ALMA_TARIHI"]))
                    : "-";

                lblSonCezaTebligTarihi.Text = row["SONCEZA_TEBLIG_TARIHI"] != DBNull.Value
                    ? FormatDateTurkish(Convert.ToDateTime(row["SONCEZA_TEBLIG_TARIHI"]))
                    : "-";
            }
            catch (Exception ex)
            {
                LogError("Firma bilgileri doldurma hatası", ex);
                HataMesajiGoster("Firma bilgileri işlenirken hata oluştu.");
            }
        }

        #endregion

        #region Denetim Geçmişi Yükleme

        private void DenetimGecmisiniYukle(int firmaId)
        {
            try
            {
                string query = @"
                    SELECT 
                        DENETIM_TARIHI,
                        CASE 
                            WHEN DENETIM_TIPI = 'IlkDenetim' THEN 'İlk Denetim'
                            WHEN DENETIM_TIPI = 'TekrarEden' THEN 'Uzaktan Denetim'
                            ELSE DENETIM_TIPI
                        END as DENETIM_TIPI,
                        MAKBUZ_NO,
                        CASE 
                            WHEN DENETIM_TIPI = 'TekrarEden' THEN 
                                ISNULL(P.ADSOYAD, CEZAKESEN_PERSONEL)
                            ELSE D_Y_PERSONEL 
                        END as PERSONEL,
                        TEBLIG_TARIHI
                    FROM DENETIMLER D
                    LEFT JOIN DenetimPersonel P ON D.CEZAKESEN_PERSONEL = P.ID
                    WHERE FIRMA_ID = @FirmaID
                    ORDER BY DENETIM_TARIHI DESC";

                var parametreler = CreateParameters(("@FirmaID", firmaId));
                DataTable dt = ExecuteDataTable(query, parametreler);

                gvDenetimGecmisi.DataSource = dt;
                gvDenetimGecmisi.DataBind();

                if (dt.Rows.Count > 0)
                {
                    BilgiMesajiGoster($"Toplam {dt.Rows.Count} denetim kaydı bulundu.");
                }
            }
            catch (Exception ex)
            {
                LogError("Denetim geçmişi yükleme hatası", ex);
                HataMesajiGoster("Denetim geçmişi yüklenirken bir hata oluştu.");
            }
        }

        #endregion

        #region GridView Events

        protected void gvDenetimGecmisi_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDenetimGecmisi.PageIndex = e.NewPageIndex;
                DenetimGecmisiniYukle(FirmaID);
            }
            catch (Exception ex)
            {
                LogError("Sayfa değiştirme hatası", ex);
                HataMesajiGoster("Sayfa değiştirme işlemi sırasında bir hata oluştu.");
            }
        }

        #endregion

        #region Button Events

        protected void btnGeriDon_Click(object sender, EventArgs e)
        {
            Response.Redirect("TumFirmalar.aspx");
        }

        protected void btnListeyeDon_Click(object sender, EventArgs e)
        {
            Response.Redirect("TumFirmalar.aspx");
        }

        protected void btnExcelAktar_Click(object sender, EventArgs e)
        {
            try
            {
                // Excel export için veri hazırla
                string query = @"
                    SELECT
                        F.VERGI_NUMARASI as 'Vergi No',
                        F.FIRMA_ADI as 'Firma Adı',
                        I.IL_AD + ' / ' + IC.ILCE_AD as 'İl/İlçe',
                        F.FIRMA_ADRESI as 'Adres',
                        FT.FIRMA_TIP_AD as 'Faaliyet Türü',
                        B.BELGE_AD as 'Belge Türü',
                        CASE WHEN F.BELGE_ALDIMI = 1 THEN 'Belgeli' ELSE 'Belgesiz' END as 'Belge Durumu',
                        F.BELGE_NUMARASI as 'Belge No',
                        CONVERT(VARCHAR, F.BELGE_ALMA_TARIHI, 103) as 'Belge Alma Tarihi',
                        CONVERT(VARCHAR, D.DENETIM_TARIHI, 103) as 'Denetim Tarihi',
                        CASE
                            WHEN D.DENETIM_TIPI = 'IlkDenetim' THEN 'İlk Denetim'
                            WHEN D.DENETIM_TIPI = 'TekrarEden' THEN 'Uzaktan Denetim'
                            ELSE D.DENETIM_TIPI
                        END as 'Denetim Tipi',
                        D.MAKBUZ_NO as 'Makbuz No',
                        D.D_Y_PERSONEL as 'Personel',
                        CONVERT(VARCHAR, D.TEBLIG_TARIHI, 103) as 'Tebliğ Tarihi'
                    FROM FIRMALAR F
                    INNER JOIN ILLER I ON F.IL = I.IL_ID
                    INNER JOIN ILCELER IC ON F.ILCE = IC.ILCE_ID
                    INNER JOIN BELGELER B ON F.BELGE_TIPI = B.ID
                    INNER JOIN FIRMA_TIP FT ON F.FIRMA_TIPI = FT.FIRMA_TIP_ID
                    LEFT JOIN DENETIMLER D ON F.ID = D.FIRMA_ID
                    WHERE F.ID = @FirmaID
                    ORDER BY D.DENETIM_TARIHI DESC";

                var parametreler = CreateParameters(("@FirmaID", FirmaID));
                DataTable dt = ExecuteDataTable(query, parametreler);

                if (dt.Rows.Count == 0)
                {
                    ShowToast("Export edilecek veri bulunamadı.", "warning");
                    return;
                }

                // Geçici GridView oluştur ve export et
                GridView gvTemp = new GridView();
                gvTemp.DataSource = dt;
                gvTemp.DataBind();

                ExportGridViewToExcel(gvTemp, $"FirmaDetay_{lblVergiNo.Text}_{DateTime.Now:yyyyMMddHHmmss}.xls");

                LogInfo($"Firma detayı Excel'e aktarıldı. Firma ID: {FirmaID}");
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowError("Excel'e aktarım sırasında bir hata oluştu.");
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // GridView'i form dışında render etmek için gerekli
        }

        #endregion

        #region Mesaj Yönetimi

        private void HataMesajiGoster(string mesaj)
        {
            lblHata.Text = mesaj;
            lblHata.Visible = true;
            lblBilgi.Visible = false;
            pnlMesajlar.Visible = true;
        }

        private void BilgiMesajiGoster(string mesaj)
        {
            lblBilgi.Text = mesaj;
            lblBilgi.Visible = true;
            lblHata.Visible = false;
            pnlMesajlar.Visible = true;
        }

        #endregion
    }
}