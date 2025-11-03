using Portal.Base;
using System;
using System.Data;

namespace Portal.ModulBelgeTakip
{
    public partial class Analiz : BasePage
    {
        #region SQL Sorguları

        private const string GetOzetIstatistik = @"
            SELECT 
                COUNT(*) as ToplamFirma,
                SUM(CASE WHEN BELGE_ALDIMI = 1 THEN 1 ELSE 0 END) as BelgeliFirma,
                SUM(CASE WHEN BELGE_ALDIMI = 0 THEN 1 ELSE 0 END) as BelgesizFirma,
                (SELECT COUNT(*) FROM DENETIMLER 
                WHERE DENETIM_TARIHI >= DATEADD(MONTH, -1, GETDATE())) as SonAyYapilanDenetim
            FROM FIRMALAR";

        private const string GetKategoriDagilimi = @"
            WITH KategoriOzet AS (
                SELECT 
                    K.ID,
                    K.KATEGORI_AD,
                    COUNT(DISTINCT F.ID) as ToplamFirma,
                    COUNT(DISTINCT D.ID) as DenetimSayisi
                FROM KATEGORILER K
                LEFT JOIN FIRMALAR F ON K.ID = F.KATEGORI_TIPI
                LEFT JOIN DENETIMLER D ON F.ID = D.FIRMA_ID
                GROUP BY K.ID, K.KATEGORI_AD
            )
            SELECT 
                KATEGORI_AD,
                ToplamFirma,
                CASE 
                    WHEN ID = 1 THEN DenetimSayisi 
                    ELSE NULL 
                END as DenetimSayisi
            FROM KategoriOzet
            ORDER BY ID";

        private const string GetBelgeDagilimi = @"
            SELECT 
                B.BELGE_AD,
                SUM(CASE WHEN F.BELGE_ALDIMI = 1 THEN 1 ELSE 0 END) as Belgeli,
                SUM(CASE WHEN F.BELGE_ALDIMI = 0 THEN 1 ELSE 0 END) as Belgesiz,
                1 as SortOrder
            FROM FIRMALAR F
            JOIN BELGELER B ON F.BELGE_TIPI = B.ID
            GROUP BY B.BELGE_AD
            
            UNION ALL
            
            SELECT 
                'TOPLAM' as BELGE_AD,
                SUM(CASE WHEN BELGE_ALDIMI = 1 THEN 1 ELSE 0 END) as Belgeli,
                SUM(CASE WHEN BELGE_ALDIMI = 0 THEN 1 ELSE 0 END) as Belgesiz,
                2 as SortOrder
            FROM FIRMALAR
            
            ORDER BY SortOrder, BELGE_AD";

        private const string GetSehirDagilimi = @"
            WITH TopCities AS (
                SELECT TOP 10
                    I.IL_AD,
                    COUNT(*) as FirmaSayisi,
                    1 as SortOrder
                FROM FIRMALAR F
                JOIN ILLER I ON F.IL = I.IL_ID
                GROUP BY I.IL_AD
                HAVING COUNT(*) > 0
            ),
            TotalCount AS (
                SELECT 
                    'TOPLAM' as IL_AD,
                    SUM(FirmaSayisi) as FirmaSayisi,
                    2 as SortOrder
                FROM TopCities
            )
            SELECT IL_AD, FirmaSayisi, SortOrder
            FROM TopCities

            UNION ALL

            SELECT IL_AD, FirmaSayisi, SortOrder
            FROM TotalCount

            ORDER BY SortOrder, FirmaSayisi DESC, IL_AD";

        private const string GetAylikIstatistik = @"
            SELECT 
                FORMAT(DENETIM_TARIHI, 'MMMM yyyy', 'tr-TR') as Ay,
                COUNT(*) as DenetimSayisi
            FROM DENETIMLER
            WHERE DENETIM_TARIHI >= DATEADD(MONTH, -6, GETDATE())
            GROUP BY FORMAT(DENETIM_TARIHI, 'MMMM yyyy', 'tr-TR')
            ORDER BY MIN(DENETIM_TARIHI)";

        private const string GetYillar = @"
            SELECT DISTINCT YEAR(DENETIM_TARIHI) as YIL 
            FROM DENETIMLER 
            WHERE DENETIM_TARIHI IS NOT NULL
            ORDER BY YIL DESC";

        private const string GetIller = @"
            SELECT IL_ID, IL_AD 
            FROM ILLER 
            ORDER BY IL_AD";

        private const string GetFiltreliIstatistik = @"
            SELECT 
                COUNT(DISTINCT F.ID) as ToplamDenetimYapilanFirma,
                COUNT(DISTINCT CASE WHEN F.BELGE_ALDIMI = 1 THEN F.ID END) as BelgeliFirmaSayisi,
                COUNT(DISTINCT CASE WHEN F.BELGE_ALDIMI = 0 THEN F.ID END) as BelgesizFirmaSayisi
            FROM FIRMALAR F
            INNER JOIN DENETIMLER D ON F.ID = D.FIRMA_ID
            WHERE 1=1 
                {0}
                {1}";

        private const string GetFirmaTipiAylikDenetim = @"
            SELECT 
                COALESCE(FT.FIRMA_TIP_AD, 'Tanımsız') as DenetimTuru,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 1 THEN 1 ELSE 0 END) as OCAK,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 2 THEN 1 ELSE 0 END) as SUBAT,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 3 THEN 1 ELSE 0 END) as MART,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 4 THEN 1 ELSE 0 END) as NISAN,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 5 THEN 1 ELSE 0 END) as MAYIS,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 6 THEN 1 ELSE 0 END) as HAZIRAN,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 7 THEN 1 ELSE 0 END) as TEMMUZ,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 8 THEN 1 ELSE 0 END) as AGUSTOS,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 9 THEN 1 ELSE 0 END) as EYLUL,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 10 THEN 1 ELSE 0 END) as EKIM,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 11 THEN 1 ELSE 0 END) as KASIM,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 12 THEN 1 ELSE 0 END) as ARALIK,
                COUNT(*) as YILLIKTOPLAM,
                CASE 
                    WHEN COALESCE(FT.FIRMA_TIP_AD, 'Tanımsız') = 'GENEL TOPLAM' THEN 999 
                    ELSE FT.FIRMA_TIP_ID 
                END as SortOrder
            FROM DENETIMLER D
            INNER JOIN FIRMALAR F ON D.FIRMA_ID = F.ID
            LEFT JOIN FIRMA_TIP FT ON F.FIRMA_TIPI = FT.FIRMA_TIP_ID
            WHERE 1=1
                  {0}
                  {1}
            GROUP BY FT.FIRMA_TIP_ID, FT.FIRMA_TIP_AD
            
            UNION ALL
            
            SELECT 
                'GENEL TOPLAM' as DenetimTuru,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 1 THEN 1 ELSE 0 END) as OCAK,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 2 THEN 1 ELSE 0 END) as SUBAT,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 3 THEN 1 ELSE 0 END) as MART,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 4 THEN 1 ELSE 0 END) as NISAN,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 5 THEN 1 ELSE 0 END) as MAYIS,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 6 THEN 1 ELSE 0 END) as HAZIRAN,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 7 THEN 1 ELSE 0 END) as TEMMUZ,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 8 THEN 1 ELSE 0 END) as AGUSTOS,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 9 THEN 1 ELSE 0 END) as EYLUL,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 10 THEN 1 ELSE 0 END) as EKIM,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 11 THEN 1 ELSE 0 END) as KASIM,
                SUM(CASE WHEN MONTH(D.DENETIM_TARIHI) = 12 THEN 1 ELSE 0 END) as ARALIK,
                COUNT(*) as YILLIKTOPLAM,
                999 as SortOrder
            FROM DENETIMLER D
            INNER JOIN FIRMALAR F ON D.FIRMA_ID = F.ID
            WHERE 1=1
                  {0}
                  {1}
            
            ORDER BY SortOrder, DenetimTuru";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (!CheckPermission(Sabitler.BELGE_TAKIP_ANALIZ))
                //{
                //    return;
                //}

                LoadDropdowns();
                LoadDashboardData();
                LoadFilteredStats();
            }
        }

        private void LoadDropdowns()
        {
            LoadYears();
            LoadCities();
        }

        private void LoadYears()
        {
            try
            {
                DataTable dt = ExecuteDataTable(GetYillar);

                ddlYil.Items.Clear();
                ddlYil.Items.Add(new System.Web.UI.WebControls.ListItem("Tümü", "0"));

                foreach (DataRow row in dt.Rows)
                {
                    string yil = row["YIL"].ToString();
                    ddlYil.Items.Add(new System.Web.UI.WebControls.ListItem(yil, yil));
                }

                string currentYear = DateTime.Now.Year.ToString();
                SetSafeDropDownValue(ddlYil, currentYear);
            }
            catch (Exception ex)
            {
                LogError("Yıllar yüklenirken hata", ex);
                ShowToast("Yıllar yüklenirken hata oluştu.", "danger");
            }
        }

        private void LoadCities()
        {
            try
            {
                DataTable dt = ExecuteDataTable(GetIller);

                ddlIl.Items.Clear();
                ddlIl.Items.Add(new System.Web.UI.WebControls.ListItem("Tümü", "0"));

                foreach (DataRow row in dt.Rows)
                {
                    string ilId = row["IL_ID"].ToString();
                    string ilAd = row["IL_AD"].ToString().Trim();
                    ddlIl.Items.Add(new System.Web.UI.WebControls.ListItem(ilAd, ilId));
                }

                SetSafeDropDownValue(ddlIl, "0");
            }
            catch (Exception ex)
            {
                LogError("İller yüklenirken hata", ex);
                ShowToast("İller yüklenirken hata oluştu.", "danger");
            }
        }

        private void LoadFilteredStats()
        {
            try
            {
                string yilFiltresi = "";
                string ilFiltresi = "";

                if (ddlYil.SelectedValue != "0")
                {
                    yilFiltresi = $" AND YEAR(D.DENETIM_TARIHI) = {ddlYil.SelectedValue}";
                }

                if (ddlIl.SelectedValue != "0")
                {
                    ilFiltresi = $" AND F.IL = {ddlIl.SelectedValue}";
                }

                string query = string.Format(GetFiltreliIstatistik, yilFiltresi, ilFiltresi);
                DataTable dt = ExecuteDataTable(query);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblToplamDenetim.Text = row["ToplamDenetimYapilanFirma"].ToString();
                    lblBelgeliFirma.Text = row["BelgeliFirmaSayisi"].ToString();
                    lblBelgesizFirma.Text = row["BelgesizFirmaSayisi"].ToString();
                }

                LoadFirmaTipiAylikDenetim();
            }
            catch (Exception ex)
            {
                LogError("Filtreli istatistikler yüklenirken hata", ex);
                ShowToast("Filtreli istatistikler yüklenirken hata oluştu.", "danger");
            }
        }

        protected void ddlYil_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFilteredStats();
        }

        protected void ddlIl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFilteredStats();
        }

        protected void btnFiltrele_Click(object sender, EventArgs e)
        {
            LoadFilteredStats();
        }

        private void LoadDashboardData()
        {
            LoadSummaryStats();
            LoadBelgeDistribution();
            LoadCityDistribution();
            LoadMonthlyStats();
            LoadKategoriDistribution();
            LoadFirmaTipiAylikDenetim();
        }

        private void LoadKategoriDistribution()
        {
            try
            {
                DataTable dt = ExecuteDataTable(GetKategoriDagilimi);
                KategoriDagilimGrid.DataSource = dt;
                KategoriDagilimGrid.DataBind();
            }
            catch (Exception ex)
            {
                LogError("Kategori dağılımı yüklenirken hata", ex);
                ShowToast("Kategori dağılımı yüklenirken hata oluştu.", "danger");
            }
        }

        private void LoadSummaryStats()
        {
            try
            {
                DataTable dt = ExecuteDataTable(GetOzetIstatistik);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblToplamFirma.Text = row["ToplamFirma"].ToString();
                    lblTamamlanan.Text = row["BelgeliFirma"].ToString();
                    lblBekleyen.Text = row["BelgesizFirma"].ToString();
                    lblAylikDenetim.Text = row["SonAyYapilanDenetim"].ToString();
                }
            }
            catch (Exception ex)
            {
                LogError("Özet istatistikler yüklenirken hata", ex);
                ShowToast("Özet istatistikler yüklenirken hata oluştu.", "danger");
            }
        }

        private void LoadBelgeDistribution()
        {
            try
            {
                DataTable dt = ExecuteDataTable(GetBelgeDagilimi);
                BelgeDagilimGrid.DataSource = dt;
                BelgeDagilimGrid.DataBind();
            }
            catch (Exception ex)
            {
                LogError("Belge dağılımı yüklenirken hata", ex);
                ShowToast("Belge dağılımı yüklenirken hata oluştu.", "danger");
            }
        }

        private void LoadCityDistribution()
        {
            try
            {
                DataTable dt = ExecuteDataTable(GetSehirDagilimi);
                SehirDagilimGrid.DataSource = dt;
                SehirDagilimGrid.DataBind();
            }
            catch (Exception ex)
            {
                LogError("Şehir dağılımı yüklenirken hata", ex);
                ShowToast("Şehir dağılımı yüklenirken hata oluştu.", "danger");
            }
        }

        private void LoadMonthlyStats()
        {
            try
            {
                DataTable dt = ExecuteDataTable(GetAylikIstatistik);
                AylikDenetimGrid.DataSource = dt;
                AylikDenetimGrid.DataBind();
            }
            catch (Exception ex)
            {
                LogError("Aylık istatistikler yüklenirken hata", ex);
                ShowToast("Aylık istatistikler yüklenirken hata oluştu.", "danger");
            }
        }

        private void LoadFirmaTipiAylikDenetim()
        {
            try
            {
                string yilFiltresi = "";
                string selectedYear = ddlYil.SelectedValue;

                if (selectedYear != "0")
                {
                    yilFiltresi = $" AND YEAR(D.DENETIM_TARIHI) = {selectedYear}";
                }

                string ilFiltre = "";
                if (ddlIl.SelectedValue != "0")
                {
                    ilFiltre = $" AND F.IL = {ddlIl.SelectedValue}";
                }

                string query = string.Format(GetFirmaTipiAylikDenetim, yilFiltresi, ilFiltre);
                DataTable dt = ExecuteDataTable(query);
                FirmaTipiAylikGrid.DataSource = dt;
                FirmaTipiAylikGrid.DataBind();

                UpdateAylikDenetimBaslik();
            }
            catch (Exception ex)
            {
                LogError("Firma tipi aylık denetim yüklenirken hata", ex);
                ShowToast("Firma tipi aylık denetim yüklenirken hata oluştu.", "danger");
            }
        }

        private void UpdateAylikDenetimBaslik()
        {
            string baslik = "Aylık Denetim Analizi";

            string ilAdi = GetSelectedIlAdi();
            string yilBilgisi = GetSelectedYilBilgisi();

            if (!string.IsNullOrEmpty(ilAdi))
            {
                baslik += $" - {ilAdi}";
            }

            if (!string.IsNullOrEmpty(yilBilgisi))
            {
                baslik += $" ({yilBilgisi})";
            }

            lblAylikDenetimBaslik.Text = baslik;
        }

        private string GetSelectedIlAdi()
        {
            if (ddlIl.SelectedValue == "0")
            {
                return "";
            }

            return ddlIl.SelectedItem?.Text ?? "";
        }

        private string GetSelectedYilBilgisi()
        {
            if (ddlYil.SelectedValue == "0")
            {
                return "Tüm Yıllar";
            }

            return ddlYil.SelectedItem?.Text ?? "";
        }
    }
}