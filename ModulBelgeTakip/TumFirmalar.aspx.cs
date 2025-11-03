using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulBelgeTakip
{
    public partial class TumFirmalar : BasePage
    {
        #region ViewState Properties

        private string SortExpression
        {
            get { return ViewState["SortExpression"] as string ?? "ID"; }
            set { ViewState["SortExpression"] = value; }
        }

        private string SortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
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

                SayfayiBaslat();
            }
        }

        #endregion

        #region Sayfa Başlatma

        private void SayfayiBaslat()
        {
            try
            {
                BelgeTurleriniYukle();
                YillariYukle();
                SortExpression = "ID";
                SortDirection = "DESC";
                FirmalariYukle();
                MesajlariTemizle();

                LogInfo("Belge Takip - Tüm Firmalar sayfası yüklendi.");
            }
            catch (Exception ex)
            {
                LogError("Sayfa başlatma hatası", ex);
                ShowError("Sayfa yüklenirken bir hata oluştu. Lütfen tekrar deneyiniz.");
            }
        }

        #endregion

        #region Dropdown Yükleme

        private void BelgeTurleriniYukle()
        {
            try
            {
                // ==> SQL sorgusu kod içinde
                string query = @"
                    SELECT ID, BELGE_AD 
                    FROM BELGELER 
                    WHERE IsActive = 1 
                    ORDER BY BELGE_AD";

                DataTable dt = ExecuteDataTable(query);

                ddlBelgeTuru.DataSource = dt;
                ddlBelgeTuru.DataTextField = "BELGE_AD";
                ddlBelgeTuru.DataValueField = "ID";
                ddlBelgeTuru.DataBind();
                ddlBelgeTuru.Items.Insert(0, new ListItem("Tüm Belge Türleri", "0"));
            }
            catch (Exception ex)
            {
                LogError("Belge türleri yükleme hatası", ex);
                throw new Exception("Belge türleri yüklenirken hata oluştu.", ex);
            }
        }

        /// <summary>
        /// DropDownList'i 2024 yılından mevcut yıla kadar olan yıllarla doldurur.
        /// </summary>
        private void YillariYukle()
        {
            try
            {                
                ddlYil.Items.Clear();

                // 1. "Tüm Yıllar" seçeneğini en başa ekle                
                ddlYil.Items.Insert(0, new ListItem("Tüm Yıllar", "0"));

                // 2. Yılları dinamik olarak oluştur
                int baslangicYili = 2024;
                int mevcutYil = DateTime.Now.Year;

                // Yılları mevcut yıldan başlayıp 2024'e kadar geriye doğru ekle
                // (Genellikle en yeni yılın en üstte olması tercih edilir)
                for (int yil = mevcutYil; yil >= baslangicYili; yil--)
                {
                    string yilStr = yil.ToString();
                    ddlYil.Items.Add(new ListItem(yilStr, yilStr));
                }

                // 3. Varsayılan olarak mevcut yılı seç
                // Orijinal kodunuzdaki SetSafeDropDownValue metodunu koruyoruz.
                // Bu metot, listede o değer yoksa hata vermeden işlemi atlar.
                SetSafeDropDownValue(ddlYil, mevcutYil.ToString());
            }
            catch (Exception ex)
            {                
                LogError("Yıllar yükleme hatası (Dinamik)", ex);
                throw new Exception("Yıllar yüklenirken hata oluştu.", ex);
            }
        }
      

        #endregion

        #region Firma Verileri Yükleme

        private void FirmalariYukle()
        {
            try
            {
                string query = SorguOlustur();
                DataTable dt = ExecuteDataTable(query, ParametreleriEkle());

                gvFirmalar.DataSource = dt;
                gvFirmalar.DataBind();

                lblKayitSayisi.Text = $"{dt.Rows.Count} Kayıt";
                
                pnlExport.Visible = dt.Rows.Count > 0;

                if (dt.Rows.Count > 0)
                {
                    BilgiMesajiGoster($"Toplam {dt.Rows.Count} kayıt listelendi.");
                }
                else
                {
                    BilgiMesajiGoster("Filtrelere uygun kayıt bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                LogError("Firma verileri yükleme hatası", ex);
                HataMesajiGoster("Firma verileri yüklenirken hata oluştu. Lütfen tekrar deneyiniz.");
            }
        }

        private string SorguOlustur()
        {
            // ==> SQL sorgusu kod içinde - FirmaSorgu.GetFirmaListesi'nden alındı
            string temelSorgu = @"
                SELECT F.ID, I.IL_AD as IL, IC.ILCE_AD as ILCE, 
                       F.FIRMA_ADI, F.VERGI_NUMARASI,
                       F.FIRMA_ADRESI, F.BELGE_ALDIMI,
                       F.BELGE_ALMA_TARIHI, F.BELGE_NUMARASI,
                       F.SONCEZA_TEBLIG_TARIHI, B.BELGE_AD,
                       FT.FIRMA_TIP_AD as FIRMA_TIPI,
                       F.KATEGORI_TIPI,
                       (SELECT COUNT(*) FROM DENETIMLER D 
                        WHERE D.FIRMA_ID = F.ID 
                        AND D.MAKBUZ_NO IS NOT NULL AND D.MAKBUZ_NO != '') as CEZA_SAYISI
                FROM FIRMALAR F
                INNER JOIN ILLER I ON F.IL = I.IL_ID
                INNER JOIN ILCELER IC ON F.ILCE = IC.ILCE_ID
                INNER JOIN BELGELER B ON F.BELGE_TIPI = B.ID
                INNER JOIN FIRMA_TIP FT ON F.FIRMA_TIPI = FT.FIRMA_TIP_ID";

            string whereKosulu = WhereKosuluOlustur();

            if (!string.IsNullOrEmpty(whereKosulu))
            {
                temelSorgu += " WHERE " + whereKosulu;
            }

            // Sıralama ekle
            string orderBy = $" ORDER BY {SortExpression} {SortDirection}";
            return temelSorgu + orderBy;
        }

        private string WhereKosuluOlustur()
        {
            var kosullar = new List<string>();

            if (!string.IsNullOrEmpty(txtVergiNo.Text.Trim()))
            {
                kosullar.Add("F.VERGI_NUMARASI LIKE @VergiNo");
            }

            if (ddlBelgeTuru.SelectedValue != "0")
            {
                kosullar.Add("F.BELGE_TIPI = @BelgeTipi");
            }

            if (ddlYil.SelectedValue != "0")
            {
                kosullar.Add("EXISTS (SELECT 1 FROM DENETIMLER D WHERE D.FIRMA_ID = F.ID AND YEAR(D.DENETIM_TARIHI) = @Yil)");
            }

            return string.Join(" AND ", kosullar);
        }

        private List<SqlParameter> ParametreleriEkle()
        {
            var parametreler = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(txtVergiNo.Text.Trim()))
            {
                parametreler.Add(CreateParameter("@VergiNo", $"%{txtVergiNo.Text.Trim()}%"));
            }

            if (ddlBelgeTuru.SelectedValue != "0")
            {
                parametreler.Add(CreateParameter("@BelgeTipi", ddlBelgeTuru.SelectedValue));
            }

            if (ddlYil.SelectedValue != "0")
            {
                parametreler.Add(CreateParameter("@Yil", ddlYil.SelectedValue));
            }

            return parametreler;
        }

        #endregion

        #region GridView Events

        protected void gvFirmalar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    bool belgeDurumu = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "BELGE_ALDIMI"));
                    int cezaSayisi = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "CEZA_SAYISI"));

                    // Belge almayan firmaları vurgula
                    if (!belgeDurumu)
                    {
                        e.Row.CssClass += " table-warning";
                    }

                    // Ceza sayısını vurgula
                    Label lblCezaSayisi = e.Row.FindControl("lblCezaSayisi") as Label;
                    if (lblCezaSayisi != null && cezaSayisi > 0)
                    {
                        lblCezaSayisi.CssClass = "highlight-red";
                    }
                }
                catch (Exception ex)
                {
                    LogError("GridView satır işleme hatası", ex);
                }
            }
        }

        protected void gvFirmalar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFirmalar.PageIndex = e.NewPageIndex;
            FirmalariYukle();
        }

        protected void gvFirmalar_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Aynı sütuna tıklandıysa yön değiştir
            if (e.SortExpression == SortExpression)
            {
                SortDirection = SortDirection == "ASC" ? "DESC" : "ASC";
            }
            else
            {
                // Yeni sütun, ASC olarak başlat
                SortExpression = e.SortExpression;
                SortDirection = "ASC";
            }

            FirmalariYukle();
        }

        protected void gvFirmalar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string firmaId = gvFirmalar.SelectedDataKey.Value.ToString();

                Response.Redirect($"FirmaDetay.aspx?ID={HttpUtility.UrlEncode(firmaId)}");
                LogInfo($"Firma seçildi: ID={firmaId}");
            }
            catch (Exception ex)
            {
                LogError("Firma seçme hatası", ex);
                ShowError("Firma detayına gidilemedi.");
            }
        }

        #endregion

        #region Filter Events

        protected void ddlBelgeTuru_SelectedIndexChanged(object sender, EventArgs e)
        {
            FirmalariYukle();
        }

        protected void ddlYil_SelectedIndexChanged(object sender, EventArgs e)
        {
            FirmalariYukle();
        }

        protected void btnFiltrele_Click(object sender, EventArgs e)
        {
            // Vergi no validasyonu
            if (!string.IsNullOrEmpty(txtVergiNo.Text) &&
                !Regex.IsMatch(txtVergiNo.Text, @"^\d*$"))
            {
                HataMesajiGoster("Vergi numarası sadece sayısal değerler içermelidir.");
                txtVergiNo.Text = string.Empty;
                return;
            }

            FirmalariYukle();
        }

        protected void btnTemizle_Click(object sender, EventArgs e)
        {
            txtVergiNo.Text = string.Empty;
            ddlBelgeTuru.SelectedIndex = 0;

            // Mevcut yılı seç
            string mevcutYil = DateTime.Now.Year.ToString();
            SetSafeDropDownValue(ddlYil, mevcutYil);

            FirmalariYukle();
        }

        #endregion

        #region Excel Export

        protected void btnExcelAktar_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvFirmalar.Rows.Count == 0)
                {
                    ShowToast("Export edilecek veri bulunamadı.", "warning");
                    return;
                }

                // GridView'i sayfalama olmadan yeniden yükle
                gvFirmalar.AllowPaging = false;
                FirmalariYukle();

                // İncele butonunu gizle (son sütun)
                int lastColIndex = gvFirmalar.Columns.Count - 1;
                bool wasLastColVisible = gvFirmalar.Columns[lastColIndex].Visible;
                gvFirmalar.Columns[lastColIndex].Visible = false;

                string filename = $"BelgeTakip_Firmalar_{DateTime.Now:yyyyMMddHHmmss}.xls";
                ExportGridViewToExcel(gvFirmalar, filename);

                LogInfo("Firma listesi Excel'e aktarıldı.");
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowToast("Excel'e aktarım sırasında bir hata oluştu.", "danger");
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

        private void MesajlariTemizle()
        {
            lblHata.Visible = false;
            lblBilgi.Visible = false;
            pnlMesajlar.Visible = false;
        }

        #endregion
    }
}