using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;
// using Portal.SQL; // Bu satır kaldırıldı, çünkü TakipSorgu sınıfı artık kullanılmıyor.

namespace Portal.ModulBelgeTakip
{
    /// <summary>
    /// Firma belge takip sayfası
    /// Belge almayan firmaların takibini yapar, muafiyet sürelerini kontrol eder
    /// </summary>
    public partial class TakiptekiFirmalar : BasePage
    {
        #region SQL Sorguları
        // Bu bölge, TakipSorgu.cs dosyasından taşınan SQL sorgularını içerir.
        // Sorguları doğrudan bu sınıfta yöneterek bağımlılığı azaltıyoruz.

        /// <summary>
        /// Firma takip listesini getirir (Belge almamış firmalar)
        /// </summary>
        private const string SqlGetFirmaTakipListesi = @"SELECT F.ID, B.BELGE_AD as BELGE_TURU, I.IL_AD as IL, 
IC.ILCE_AD as ILCE, F.FIRMA_ADRESI as ADRES, F.FIRMA_ADI, F.VERGI_NUMARASI, D.DENETIM_TARIHI, 
D.TEBLIG_TARIHI as SONCEZA_TEBLIG_TARIHI, F.BELGE_ALDIMI, F.BELGE_ALMA_TARIHI, F.BELGE_NUMARASI, 
B.BELGE_AD 
FROM FIRMALAR F INNER JOIN ILLER I ON F.IL = I.IL_ID INNER JOIN ILCELER IC ON F.ILCE = IC.ILCE_ID INNER JOIN
BELGELER B ON F.BELGE_TIPI = B.ID LEFT JOIN (SELECT FIRMA_ID, DENETIM_TARIHI, TEBLIG_TARIHI, ROW_NUMBER() OVER
(PARTITION BY FIRMA_ID ORDER BY DENETIM_TARIHI DESC) as rn FROM DENETIMLER) D ON F.ID = D.FIRMA_ID AND D.rn = 1 
WHERE (@BELGE_TIPI = 0 OR F.BELGE_TIPI = @BELGE_TIPI) AND (@IL_ID = 0 OR F.IL = @IL_ID) AND 
(@ILCE_ID = 0 OR F.ILCE = @ILCE_ID) AND B.IsActive = 1 AND F.BELGE_ALDIMI = 0 ORDER BY 
COALESCE(D.DENETIM_TARIHI, D.TEBLIG_TARIHI) DESC";

        /// <summary>
        /// Firmaya kesilmiş ceza sayısını döner
        /// </summary>
        private const string SqlGetCezaSayisi = @"SELECT COUNT(*) FROM DENETIMLER WHERE FIRMA_ID = @FirmaId AND MAKBUZ_NO IS NOT NULL AND MAKBUZ_NO != ''";

        /// <summary>
        /// Aktif il listesini getirir (belge almamış firma olan iller)
        /// </summary>
        private const string SqlGetIlListesi = @"SELECT DISTINCT I.IL_ID, I.IL_AD FROM ILLER I INNER JOIN FIRMALAR F ON I.IL_ID = F.IL WHERE F.BELGE_ALDIMI = 0 ORDER BY I.IL_AD";

        /// <summary>
        /// Seçilen ile ait ilçe listesini getirir (belge almamış firma olan ilçeler)
        /// </summary>
        private const string SqlGetIlceListesi = @"SELECT DISTINCT IC.ILCE_ID, IC.ILCE_AD FROM ILCELER IC INNER JOIN FIRMALAR F ON IC.ILCE_ID = F.ILCE WHERE IC.IL_ID = @IL_ID AND F.BELGE_ALDIMI = 0 ORDER BY IC.ILCE_AD";

        /// <summary>
        /// Aktif belge türlerini getirir
        /// </summary>
        private const string SqlGetBelgeTurleri = @"SELECT ID, BELGE_AD FROM BELGELER WHERE IsActive = 1 ORDER BY BELGE_AD";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (!CheckPermission(Sabitler.BELGE_TAKIP_FIRMALAR))
                //{
                //    return; // BasePage otomatik redirect yapar
                //}

                BelgeTurleriniYukle();
                IlleriYukle();
                FirmaVerileriniYukle();
            }
        }

        #region Dropdown Yükleme Metodları

        private void BelgeTurleriniYukle()
        {
            try
            {
                DataTable dt = ExecuteDataTable(SqlGetBelgeTurleri);

                DdlBelgeTuru.DataSource = dt;
                DdlBelgeTuru.DataTextField = "BELGE_AD";
                DdlBelgeTuru.DataValueField = "ID";
                DdlBelgeTuru.DataBind();
                DdlBelgeTuru.Items.Insert(0, new ListItem("Tümü", "0"));
            }
            catch (Exception ex)
            {             
                LogError("Belge türleri yüklenirken hata", ex);
                ShowToast("Belge türleri yüklenirken hata oluştu.", "danger");
            }
        }

        private void IlleriYukle()
        {
            try
            {
                DataTable dt = ExecuteDataTable(SqlGetIlListesi);

                DdlIl.DataSource = dt;
                DdlIl.DataTextField = "IL_AD";
                DdlIl.DataValueField = "IL_ID";
                DdlIl.DataBind();
                DdlIl.Items.Insert(0, new ListItem("Tümü", "0"));

                // İlçe dropdown'unu başlangıçta sadece "Tümü" ile doldur
                DdlIlce.Items.Clear();
                DdlIlce.Items.Insert(0, new ListItem("Tümü", "0"));
            }
            catch (Exception ex)
            {
                LogError("İller yüklenirken hata", ex);
                ShowToast("İller yüklenirken hata oluştu.", "danger");
            }
        }


        private void IlceleriYukle(int ilId)
        {
            DdlIlce.Items.Clear();
            DdlIlce.Items.Insert(0, new ListItem("Tümü", "0"));

            if (ilId == 0) return; // Tümü seçiliyse ilçe yükleme

            try
            {                
                var parameters = CreateParameters(("@IL_ID", ilId));                
                DataTable dt = ExecuteDataTable(SqlGetIlceListesi, parameters);

                DdlIlce.DataSource = dt;
                DdlIlce.DataTextField = "ILCE_AD";
                DdlIlce.DataValueField = "ILCE_ID";
                DdlIlce.DataBind();
                DdlIlce.Items.Insert(0, new ListItem("Tümü", "0"));
            }
            catch (Exception ex)
            {
                // ==> BasePage LogError ve ShowToast kullanılıyor
                LogError("İlçeler yüklenirken hata", ex);
                ShowToast("İlçeler yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Veri Yükleme ve GridView İşlemleri

        /// <summary>
        /// Firma verilerini GridView'e yükler
        /// </summary>
        private void FirmaVerileriniYukle()
        {
            try
            {
                // ==> BasePage CreateParameters ve ExecuteDataTable kullanılıyor
                var parameters = CreateParameters(
                    ("@BELGE_TIPI", DdlBelgeTuru.SelectedValue),
                    ("@IL_ID", DdlIl.SelectedValue),
                    ("@ILCE_ID", DdlIlce.SelectedValue)
                );

                // Değişiklik: TakipSorgu.GetFirmaTakipListesi yerine yerel sabit kullanıldı
                DataTable dt = ExecuteDataTable(SqlGetFirmaTakipListesi, parameters);

                TakiptekiFirmalarGrid.DataSource = dt;
                TakiptekiFirmalarGrid.DataBind();

                KayitSayisiniGuncelle(dt.Rows.Count);

                if (dt.Rows.Count > 0)
                {
                    // ==> BasePage ShowToast kullanılıyor
                    ShowToast($"{dt.Rows.Count} firma listelendi.", "success");
                }
                else
                {
                    ShowToast("Belirtilen kriterlere uygun firma bulunamadı.", "info");
                }
            }
            catch (Exception ex)
            {
                // ==> BasePage LogError ve ShowToast kullanılıyor
                LogError("Firma verileri yüklenirken hata", ex);
                ShowToast("Veriler yüklenirken hata oluştu.", "danger");
                KayitSayisiniGuncelle(0);
            }
        }

        /// <summary>
        /// GridView satırları bind edilirken çağrılır
        /// Muafiyet durumu ve ceza sayısı hesaplanır
        /// </summary>
        protected void TakiptekiFirmalarGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool belgeDurumu = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "BELGE_ALDIMI"));
                string belgeAd = DataBinder.Eval(e.Row.DataItem, "BELGE_AD").ToString().Trim();
                int firmaId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ID"));

                // Tebliğ tarihini göster
                Label lblTebligTarihi = e.Row.FindControl("lblTebligTarihi") as Label;
                if (lblTebligTarihi != null)
                {
                    object tebligObj = DataBinder.Eval(e.Row.DataItem, "SONCEZA_TEBLIG_TARIHI");
                    if (tebligObj == DBNull.Value || tebligObj == null)
                    {
                        lblTebligTarihi.Text = "-";
                    }
                    else
                    {
                        DateTime tebligTarihi = Convert.ToDateTime(tebligObj);
                        lblTebligTarihi.Text = FormatDateTurkish(tebligTarihi);
                    }
                }

                // Muafiyet durumu hesaplama
                Label lblMuafiyetDurumu = e.Row.FindControl("lblMuafiyetDurumu") as Label;
                if (lblMuafiyetDurumu != null)
                {
                    // ==> Sabitler.cs'den TMFB_BELGE_KODU ve TAKIP_SURESI_GUN kullanılıyor
                    if (belgeAd == Sabitler.TMFB_BELGE_KODU)
                    {
                        if (belgeDurumu)
                        {
                            lblMuafiyetDurumu.Text = "<i class='fas fa-check me-1'></i>Belge Alındı";
                            lblMuafiyetDurumu.CssClass = "highlight-green";
                        }
                        else
                        {
                            object tebligObj = DataBinder.Eval(e.Row.DataItem, "SONCEZA_TEBLIG_TARIHI");
                            if (tebligObj == DBNull.Value || tebligObj == null)
                            {
                                lblMuafiyetDurumu.Text = "<i class='fas fa-ban me-1'></i>Tebliğ bekleniyor";
                                lblMuafiyetDurumu.CssClass = "highlight-red";
                            }
                            else
                            {
                                DateTime sonTebligTarihi = Convert.ToDateTime(tebligObj);
                                int gunFarki = (DateTime.Now - sonTebligTarihi).Days;

                                if (gunFarki < 0)
                                {
                                    lblMuafiyetDurumu.Text = "<i class='fas fa-calendar me-1'></i>Gelecek Tarih";
                                    lblMuafiyetDurumu.CssClass = "badge bg-secondary";
                                }
                                else if (gunFarki < Sabitler.TAKIP_SURESI_GUN)
                                {
                                    int kalanGun = Sabitler.TAKIP_SURESI_GUN - gunFarki;
                                    lblMuafiyetDurumu.Text = $"<i class='fas fa-clock me-1'></i>{kalanGun} gün kaldı";
                                    lblMuafiyetDurumu.CssClass = "badge bg-warning text-dark";
                                }
                                else
                                {
                                    int gecenGun = gunFarki - Sabitler.TAKIP_SURESI_GUN;
                                    lblMuafiyetDurumu.Text = $"<i class='fas fa-exclamation-triangle me-1'></i>{gecenGun} gün geçti";
                                    lblMuafiyetDurumu.CssClass = "highlight-red";
                                }
                            }
                        }
                    }
                    else
                    {
                        lblMuafiyetDurumu.Text = "-";
                    }
                }

                // Ceza sayısını yükle
                Label lblCezaSayisi = e.Row.FindControl("lblCezaSayisi") as Label;
                if (lblCezaSayisi != null)
                {
                    CezaSayisiniYukle(firmaId, lblCezaSayisi);
                }
            }
        }

        /// <summary>
        /// Firmaya kesilmiş ceza sayısını yükler
        /// </summary>
        private void CezaSayisiniYukle(int firmaId, Label lblCezaSayisi)
        {
            try
            {
                // ==> BasePage GetConnection ve CreateParameters kullanılıyor
                using (SqlConnection conn = GetConnection())
                // Değişiklik: TakipSorgu.GetCezaSayisi yerine yerel sabit kullanıldı
                using (SqlCommand cmd = new SqlCommand(SqlGetCezaSayisi, conn))
                {
                    cmd.Parameters.AddWithValue("@FirmaId", firmaId);
                    conn.Open();
                    int cezaSayisi = (int)cmd.ExecuteScalar();

                    if (lblCezaSayisi != null)
                    {
                        lblCezaSayisi.Text = cezaSayisi.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // ==> BasePage LogError kullanılıyor
                LogError($"Ceza sayısı yüklenirken hata - FirmaId: {firmaId}", ex);
                if (lblCezaSayisi != null)
                {
                    lblCezaSayisi.Text = "0";
                }
            }
        }

        /// <summary>
        /// GridView sıralama işlemi
        /// </summary>
        protected void TakiptekiFirmalarGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            string sortDirection = "ASC";

            // ViewState'den önceki sıralama bilgisini al
            if (ViewState["SortExpression"] != null && ViewState["SortExpression"].ToString() == sortExpression)
            {
                sortDirection = ViewState["SortDirection"].ToString() == "ASC" ? "DESC" : "ASC";
            }

            // ViewState'e kaydet
            ViewState["SortExpression"] = sortExpression;
            ViewState["SortDirection"] = sortDirection;

            FirmaVerileriniSiralaVeYukle(sortExpression, sortDirection);
        }

        /// <summary>
        /// Firma verilerini sıralayarak yükler
        /// </summary>
        private void FirmaVerileriniSiralaVeYukle(string sortExpression, string sortDirection)
        {
            try
            {
                // Sorguya sıralama ekle
                // Değişiklik: TakipSorgu.GetFirmaTakipListesi yerine yerel sabit kullanıldı
                string query = SqlGetFirmaTakipListesi.Replace(
                    "ORDER BY COALESCE(D.DENETIM_TARIHI, D.TEBLIG_TARIHI) DESC",
                    $"ORDER BY {sortExpression} {sortDirection}"
                );

                // ==> BasePage CreateParameters ve ExecuteDataTable kullanılıyor
                var parameters = CreateParameters(
                    ("@BELGE_TIPI", DdlBelgeTuru.SelectedValue),
                    ("@IL_ID", DdlIl.SelectedValue),
                    ("@ILCE_ID", DdlIlce.SelectedValue)
                );

                DataTable dt = ExecuteDataTable(query, parameters);

                TakiptekiFirmalarGrid.DataSource = dt;
                TakiptekiFirmalarGrid.DataBind();

                KayitSayisiniGuncelle(dt.Rows.Count);
                // ==> BasePage ShowToast kullanılıyor
                ShowToast($"{dt.Rows.Count} firma sıralandı.", "info");
            }
            catch (Exception ex)
            {
                // ==> BasePage LogError ve ShowToast kullanılıyor
                LogError("Sıralama sırasında hata", ex);
                ShowToast("Sıralama işlemi sırasında hata oluştu.", "danger");
            }
        }

        #endregion

        #region Event Handler Metodları

        /// <summary>
        /// Belge türü değiştiğinde
        /// </summary>
        protected void DdlBelgeTuru_SelectedIndexChanged(object sender, EventArgs e)
        {
            FirmaVerileriniYukle();
        }

        /// <summary>
        /// İl değiştiğinde ilçeleri yükle ve firma listesini güncelle
        /// </summary>
        protected void DdlIl_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ilId = Convert.ToInt32(DdlIl.SelectedValue);
            IlceleriYukle(ilId);
            FirmaVerileriniYukle();
        }

        /// <summary>
        /// İlçe değiştiğinde firma listesini güncelle
        /// </summary>
        protected void DdlIlce_SelectedIndexChanged(object sender, EventArgs e)
        {
            FirmaVerileriniYukle();
        }

        /// <summary>
        /// Yenile butonu - filtreleri sıfırla ve tüm verileri yükle
        /// </summary>
        protected void btnYenile_Click(object sender, EventArgs e)
        {
            DdlBelgeTuru.SelectedIndex = 0;
            DdlIl.SelectedIndex = 0;
            DdlIlce.Items.Clear();
            DdlIlce.Items.Insert(0, new ListItem("Tümü", "0"));
            FirmaVerileriniYukle();
            // ==> BasePage ShowToast kullanılıyor
            ShowToast("Filtreler sıfırlandı ve veriler yenilendi.", "success");
        }

        /// <summary>
        /// Excel'e aktar butonu
        /// </summary>
        protected void btnExcelAktar_Click(object sender, EventArgs e)
        {
            if (TakiptekiFirmalarGrid.Rows.Count == 0)
            {
                // ==> BasePage ShowToast kullanılıyor
                ShowToast("Export edilecek veri bulunamadı.", "warning");
                return;
            }

            try
            {
                // ==> BasePage ExportGridViewToExcel metodu kullanılıyor
                ExportGridViewToExcel(TakiptekiFirmalarGrid, "TakiptekiFirmalar_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                // ==> BasePage LogInfo kullanılıyor
                LogInfo("Takipteki firmalar Excel'e aktarıldı.");
            }
            catch (Exception ex)
            {
                // ==> BasePage LogError ve ShowToast kullanılıyor
                LogError("Excel export hatası", ex);
                ShowToast("Excel dosyası oluşturulurken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Excel render için gerekli override
        /// </summary>
        public override void VerifyRenderingInServerForm(Control control)
        {
            // GridView Excel render için gerekli
        }

        #endregion

        #region UI Helper Metodları

        /// <summary>
        /// Kayıt sayısını badge'de göster
        /// </summary>
        private void KayitSayisiniGuncelle(int kayitSayisi)
        {
            if (kayitSayisi > 0)
            {
                lblKayitSayisi.Text = $"{kayitSayisi} kayıt";
                lblKayitSayisi.CssClass = "badge bg-primary";
            }
            else
            {
                lblKayitSayisi.Text = "Kayıt yok";
                lblKayitSayisi.CssClass = "badge bg-secondary";
            }
        }

        #endregion
    }
}