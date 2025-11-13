using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using Portal.Base;

namespace Portal
{
    public partial class PerformanceMonitor : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Yetki kontrolü: Sadece Admin görebilir (yetkiNo: 100)
            
            if (!CheckPermission(100))
            {
                return;
            }

            if (!IsPostBack)
            {
                LoadPageDropDown();
                SetDefaultDateRange();
                LoadPerformanceData();
            }
        }

        #region Model Sınıfları

        /// <summary>
        /// Performans istatistikleri model
        /// </summary>
        public class PerformanceStatistics
        {
            public int AvgLoadTime { get; set; }
            public string SlowestPage { get; set; }
            public int SlowestPageTime { get; set; }
            public int TotalQueries { get; set; }
            public decimal AvgMemory { get; set; }
        }

        /// <summary>
        /// Haftalık trend verisi model
        /// </summary>
        public class WeeklyTrendData
        {
            public List<string> Dates { get; set; }
            public List<int> AvgLoadTimes { get; set; }

            public WeeklyTrendData()
            {
                Dates = new List<string>();
                AvgLoadTimes = new List<int>();
            }
        }

        /// <summary>
        /// Sayfa performans karşılaştırması model
        /// </summary>
        public class PagePerformance
        {
            public string PageName { get; set; }
            public int AvgLoadTime { get; set; }
        }

        /// <summary>
        /// Performans log kaydı model
        /// </summary>
        public class PerformanceLogEntry
        {
            public DateTime CreatedDate { get; set; }
            public string PageName { get; set; }
            public int LoadTimeMs { get; set; }
            public int SqlQueryCount { get; set; }
            public decimal MemoryUsageMB { get; set; }
            public string UserName { get; set; }
        }

        #endregion

        #region Veri Yükleme

        /// <summary>
        /// Performans verilerini yükler
        /// </summary>
        private void LoadPerformanceData()
        {
            try
            {
                // 1. İstatistik kartları
                var stats = GetPerformanceStatistics();
                lblAvgLoadTime.Text = stats.AvgLoadTime.ToString();
                lblSlowestPage.Text = stats.SlowestPage;
                lblSlowestPageTime.Text = stats.SlowestPageTime + " ms";
                lblTotalQueries.Text = stats.TotalQueries.ToString();
                lblAvgMemory.Text = stats.AvgMemory.ToString("0.00");

                // 2. Haftalık trend (Chart.js için JSON)
                var weeklyTrend = GetWeeklyTrend();
                hdnWeeklyTrendData.Value = SerializeWeeklyTrend(weeklyTrend);

                // 3. Sayfa karşılaştırması (Bar Chart için JSON)
                var pageComparison = GetTopPages(5);
                hdnPageComparisonData.Value = SerializePageComparison(pageComparison);

                // 4. En yavaş 10 sayfa
                rptSlowestPages.DataSource = GetTopPages(10);
                rptSlowestPages.DataBind();

                // 5. Son 20 performans kaydı
                rptRecentLogs.DataSource = GetRecentLogs(20);
                rptRecentLogs.DataBind();
            }
            catch (Exception ex)
            {
                BasePage.LogError("LoadPerformanceData hatası", ex);
                ShowToast("Veriler yüklenirken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Performans istatistiklerini getirir
        /// </summary>
        private PerformanceStatistics GetPerformanceStatistics()
        {
            var stats = new PerformanceStatistics();

            try
            {
                DateTime startDate = string.IsNullOrEmpty(txtStartDate.Text)
                    ? DateTime.Today.AddDays(-1)
                    : DateTime.Parse(txtStartDate.Text);

                DateTime endDate = string.IsNullOrEmpty(txtEndDate.Text)
                    ? DateTime.Today.AddDays(1)
                    : DateTime.Parse(txtEndDate.Text).AddDays(1);

                string pageFilter = ddlPage.SelectedValue;
                string userFilter = txtUser.Text.Trim();

                string query = @"
                    SELECT 
                        AVG(LoadTimeMs) AS AvgLoadTime,
                        SUM(SqlQueryCount) AS TotalQueries,
                        AVG(MemoryUsageMB) AS AvgMemory
                    FROM PerformanceLogs
                    WHERE CreatedDate >= @StartDate AND CreatedDate < @EndDate";

                if (!string.IsNullOrEmpty(pageFilter))
                    query += " AND PageName = @PageName";

                if (!string.IsNullOrEmpty(userFilter))
                    query += " AND (UserSicil LIKE @UserFilter OR UserName LIKE @UserFilter)";

                var parameters = BasePage.CreateParameters(
                    ("@StartDate", startDate),
                    ("@EndDate", endDate)
                );

                if (!string.IsNullOrEmpty(pageFilter))
                    parameters.Add(BasePage.CreateParameter("@PageName", pageFilter));

                if (!string.IsNullOrEmpty(userFilter))
                    parameters.Add(BasePage.CreateParameter("@UserFilter", "%" + userFilter + "%"));

                DataTable dt = BasePage.ExecuteDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    stats.AvgLoadTime = dt.Rows[0]["AvgLoadTime"] != DBNull.Value
                        ? Convert.ToInt32(dt.Rows[0]["AvgLoadTime"])
                        : 0;

                    stats.TotalQueries = dt.Rows[0]["TotalQueries"] != DBNull.Value
                        ? Convert.ToInt32(dt.Rows[0]["TotalQueries"])
                        : 0;

                    stats.AvgMemory = dt.Rows[0]["AvgMemory"] != DBNull.Value
                        ? Convert.ToDecimal(dt.Rows[0]["AvgMemory"])
                        : 0;
                }

                // En yavaş sayfa
                string slowestQuery = @"
                    SELECT TOP 1 PageName, AVG(LoadTimeMs) AS AvgTime
                    FROM PerformanceLogs
                    WHERE CreatedDate >= @StartDate AND CreatedDate < @EndDate
                    GROUP BY PageName
                    ORDER BY AvgTime DESC";

                DataTable dtSlowest = BasePage.ExecuteDataTable(slowestQuery, parameters);

                if (dtSlowest.Rows.Count > 0)
                {
                    stats.SlowestPage = dtSlowest.Rows[0]["PageName"].ToString();
                    stats.SlowestPageTime = Convert.ToInt32(dtSlowest.Rows[0]["AvgTime"]);
                }
                else
                {
                    stats.SlowestPage = "-";
                    stats.SlowestPageTime = 0;
                }
            }
            catch (Exception ex)
            {
                BasePage.LogError("GetPerformanceStatistics hatası", ex);
            }

            return stats;
        }

        /// <summary>
        /// Son 7 günlük performans trend verilerini getirir
        /// </summary>
        private WeeklyTrendData GetWeeklyTrend()
        {
            var trendData = new WeeklyTrendData();

            try
            {
                for (int i = 6; i >= 0; i--)
                {
                    DateTime date = DateTime.Today.AddDays(-i);
                    DateTime nextDate = date.AddDays(1);

                    string query = @"
                        SELECT AVG(LoadTimeMs) AS AvgLoadTime
                        FROM PerformanceLogs
                        WHERE CreatedDate >= @StartDate AND CreatedDate < @EndDate";

                    var parameters = BasePage.CreateParameters(
                        ("@StartDate", date),
                        ("@EndDate", nextDate)
                    );

                    DataTable dt = BasePage.ExecuteDataTable(query, parameters);

                    trendData.Dates.Add(date.ToString("dd.MM"));

                    if (dt.Rows.Count > 0 && dt.Rows[0]["AvgLoadTime"] != DBNull.Value)
                    {
                        trendData.AvgLoadTimes.Add(Convert.ToInt32(dt.Rows[0]["AvgLoadTime"]));
                    }
                    else
                    {
                        trendData.AvgLoadTimes.Add(0);
                    }
                }
            }
            catch (Exception ex)
            {
                BasePage.LogError("GetWeeklyTrend hatası", ex);
            }

            return trendData;
        }

        /// <summary>
        /// En yavaş N sayfa getirir
        /// </summary>
        private List<PagePerformance> GetTopPages(int topCount)
        {
            var pages = new List<PagePerformance>();

            try
            {
                DateTime startDate = string.IsNullOrEmpty(txtStartDate.Text)
                    ? DateTime.Today.AddDays(-7)
                    : DateTime.Parse(txtStartDate.Text);

                DateTime endDate = string.IsNullOrEmpty(txtEndDate.Text)
                    ? DateTime.Today.AddDays(1)
                    : DateTime.Parse(txtEndDate.Text).AddDays(1);

                string query = $@"
                    SELECT TOP {topCount} PageName, AVG(LoadTimeMs) AS AvgLoadTime
                    FROM PerformanceLogs
                    WHERE CreatedDate >= @StartDate AND CreatedDate < @EndDate
                    GROUP BY PageName
                    ORDER BY AvgLoadTime DESC";

                var parameters = BasePage.CreateParameters(
                    ("@StartDate", startDate),
                    ("@EndDate", endDate)
                );

                DataTable dt = BasePage.ExecuteDataTable(query, parameters);

                foreach (DataRow row in dt.Rows)
                {
                    pages.Add(new PagePerformance
                    {
                        PageName = row["PageName"].ToString(),
                        AvgLoadTime = Convert.ToInt32(row["AvgLoadTime"])
                    });
                }
            }
            catch (Exception ex)
            {
                BasePage.LogError("GetTopPages hatası", ex);
            }

            return pages;
        }

        /// <summary>
        /// Son N performans kaydını getirir
        /// </summary>
        private List<PerformanceLogEntry> GetRecentLogs(int count)
        {
            var logs = new List<PerformanceLogEntry>();

            try
            {
                DateTime startDate = string.IsNullOrEmpty(txtStartDate.Text)
                    ? DateTime.Today.AddDays(-7)
                    : DateTime.Parse(txtStartDate.Text);

                DateTime endDate = string.IsNullOrEmpty(txtEndDate.Text)
                    ? DateTime.Today.AddDays(1)
                    : DateTime.Parse(txtEndDate.Text).AddDays(1);

                string pageFilter = ddlPage.SelectedValue;
                string userFilter = txtUser.Text.Trim();

                string query = $@"
                    SELECT TOP {count} 
                        CreatedDate, PageName, LoadTimeMs, SqlQueryCount, MemoryUsageMB, UserName
                    FROM PerformanceLogs
                    WHERE CreatedDate >= @StartDate AND CreatedDate < @EndDate";

                if (!string.IsNullOrEmpty(pageFilter))
                    query += " AND PageName = @PageName";

                if (!string.IsNullOrEmpty(userFilter))
                    query += " AND (UserSicil LIKE @UserFilter OR UserName LIKE @UserFilter)";

                query += " ORDER BY CreatedDate DESC";

                var parameters = BasePage.CreateParameters(
                    ("@StartDate", startDate),
                    ("@EndDate", endDate)
                );

                if (!string.IsNullOrEmpty(pageFilter))
                    parameters.Add(BasePage.CreateParameter("@PageName", pageFilter));

                if (!string.IsNullOrEmpty(userFilter))
                    parameters.Add(BasePage.CreateParameter("@UserFilter", "%" + userFilter + "%"));

                DataTable dt = BasePage.ExecuteDataTable(query, parameters);

                foreach (DataRow row in dt.Rows)
                {
                    logs.Add(new PerformanceLogEntry
                    {
                        CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                        PageName = row["PageName"].ToString(),
                        LoadTimeMs = Convert.ToInt32(row["LoadTimeMs"]),
                        SqlQueryCount = Convert.ToInt32(row["SqlQueryCount"]),
                        MemoryUsageMB = Convert.ToDecimal(row["MemoryUsageMB"]),
                        UserName = row["UserName"]?.ToString() ?? "-"
                    });
                }
            }
            catch (Exception ex)
            {
                BasePage.LogError("GetRecentLogs hatası", ex);
            }

            return logs;
        }

        #endregion

        #region Yardımcı Metodlar

        /// <summary>
        /// Sayfa dropdown'unu doldurur
        /// </summary>
        private void LoadPageDropDown()
        {
            try
            {
                string query = @"
                    SELECT DISTINCT PageName
                    FROM PerformanceLogs
                    ORDER BY PageName";

                DataTable dt = BasePage.ExecuteDataTable(query);

                ddlPage.Items.Clear();
                ddlPage.Items.Add(new System.Web.UI.WebControls.ListItem("Tümü", ""));

                foreach (DataRow row in dt.Rows)
                {
                    string pageName = row["PageName"].ToString();
                    ddlPage.Items.Add(new System.Web.UI.WebControls.ListItem(pageName, pageName));
                }
            }
            catch (Exception ex)
            {
                BasePage.LogError("LoadPageDropDown hatası", ex);
            }
        }

        /// <summary>
        /// Varsayılan tarih aralığını ayarlar (Son 7 gün)
        /// </summary>
        private void SetDefaultDateRange()
        {
            txtStartDate.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
            txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Haftalık trend verisini JSON'a çevirir (Chart.js için)
        /// </summary>
        private string SerializeWeeklyTrend(WeeklyTrendData data)
        {
            try
            {
                var json = new
                {
                    labels = data.Dates,
                    datasets = new[]
                    {
                        new {
                            label = "Ortalama Yükleme Süresi (ms)",
                            data = data.AvgLoadTimes,
                            borderColor = "#667eea",
                            backgroundColor = "rgba(102, 126, 234, 0.1)",
                            tension = 0.4,
                            fill = true
                        }
                    }
                };

                return Newtonsoft.Json.JsonConvert.SerializeObject(json);
            }
            catch
            {
                return "{}";
            }
        }

        /// <summary>
        /// Sayfa karşılaştırmasını JSON'a çevirir (Bar Chart için)
        /// </summary>
        private string SerializePageComparison(List<PagePerformance> pages)
        {
            try
            {
                var labels = pages.Select(p => p.PageName.Replace("/", "").Replace(".aspx", "")).ToList();
                var data = pages.Select(p => p.AvgLoadTime).ToList();

                var json = new
                {
                    labels = labels,
                    datasets = new[]
                    {
                        new {
                            label = "Ortalama Süre (ms)",
                            data = data,
                            backgroundColor = new[] { "#f85032", "#ff6b6b", "#ffa726", "#43e97b", "#667eea" }
                        }
                    }
                };

                return Newtonsoft.Json.JsonConvert.SerializeObject(json);
            }
            catch
            {
                return "{}";
            }
        }

        /// <summary>
        /// Performans badge'i döndürür (Hızlı/Orta/Yavaş)
        /// </summary>
        public string GetPerformanceBadge(int loadTimeMs)
        {
            if (loadTimeMs < 100)
                return "<span class='badge-fast'>Hızlı</span>";
            else if (loadTimeMs < 500)
                return "<span class='badge-medium'>Orta</span>";
            else
                return "<span class='badge-slow'>Yavaş</span>";
        }

       

        #endregion

        #region Button Events

        /// <summary>
        /// Yenile butonu
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPerformanceData();
            ShowToast("Veriler yenilendi.", "success");
        }

        /// <summary>
        /// Filtrele butonu
        /// </summary>
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadPerformanceData();
            ShowToast("Filtre uygulandı.", "info");
        }

        /// <summary>
        /// Filtreyi temizle butonu
        /// </summary>
        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            SetDefaultDateRange();
            ddlPage.SelectedIndex = 0;
            txtUser.Text = string.Empty;
            LoadPerformanceData();
            ShowToast("Filtre temizlendi.", "info");
        }

        #endregion
    }
}