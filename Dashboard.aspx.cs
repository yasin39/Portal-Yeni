using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Portal.Base;

namespace Portal
{
    public partial class Dashboard : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Yetki kontrolü: Sadece 117 yetkisine sahip ADMIN görebilir
            if (!CheckPermission(100))
            {
                return;
            }

            if (!IsPostBack)
            {
                LoadDashboardData();
            }
        }

        /// <summary>
        /// Dashboard verilerini yükler
        /// </summary>
        private void LoadDashboardData()
        {
            try
            {
                // 1. Günlük istatistikler
                var dailyStats = GetDailyLogStatistics();
                lblTotalCount.Text = dailyStats.TotalCount.ToString();
                lblErrorCount.Text = dailyStats.ErrorCount.ToString();
                lblWarningCount.Text = dailyStats.WarningCount.ToString();
                lblSuccessRate.Text = dailyStats.SuccessRate.ToString("0.00") + "%";

                // 2. Haftalık trend verisi (Chart.js için JSON)
                var weeklyTrend = GetWeeklyLogTrend();
                hdnWeeklyTrendData.Value = SerializeWeeklyTrend(weeklyTrend);

                // 3. Log seviye dağılımı (Pie Chart için JSON)
                hdnLogDistributionData.Value = SerializeLogDistribution(dailyStats);

                // 4. En çok hata veren modüller
                rptTopModules.DataSource = GetTopErrorModules(5);
                rptTopModules.DataBind();

                // 5. Son 10 hata kaydı
                rptRecentErrors.DataSource = GetRecentErrors(10);
                rptRecentErrors.DataBind();
            }
            catch (Exception ex)
            {
                LogError("LoadDashboardData hatası", ex);
                ShowToast("Dashboard verileri yüklenirken hata oluştu.", "danger");
            }
        }

        #region Yardımcı Metodlar

        /// <summary>
        /// Günlük log istatistiklerini model sınıfı
        /// </summary>
        public class DailyLogStatistics
        {
            public int TotalCount { get; set; }
            public int ErrorCount { get; set; }
            public int WarningCount { get; set; }
            public int InfoCount { get; set; }
            public decimal SuccessRate { get; set; }
        }

        /// <summary>
        /// Haftalık trend verisi model sınıfı
        /// </summary>
        public class WeeklyTrendData
        {
            public List<string> Dates { get; set; }
            public List<int> ErrorCounts { get; set; }
            public List<int> WarningCounts { get; set; }
            public List<int> InfoCounts { get; set; }

            public WeeklyTrendData()
            {
                Dates = new List<string>();
                ErrorCounts = new List<int>();
                WarningCounts = new List<int>();
                InfoCounts = new List<int>();
            }
        }

        /// <summary>
        /// Modül hata istatistiği model sınıfı
        /// </summary>
        public class ModuleErrorStats
        {
            public string ModuleName { get; set; }
            public int ErrorCount { get; set; }
        }

        /// <summary>
        /// Son hata kaydı model sınıfı
        /// </summary>
        public class RecentError
        {
            public DateTime Timestamp { get; set; }
            public string Module { get; set; }
            public string Message { get; set; }
            public string ShortMessage
            {
                get
                {
                    if (string.IsNullOrEmpty(Message)) return "";
                    return Message.Length > 100 ? Message.Substring(0, 100) + "..." : Message;
                }
            }
        }

        /// <summary>
        /// Bugünün log istatistiklerini getirir
        /// </summary>
        private DailyLogStatistics GetDailyLogStatistics()
        {
            DailyLogStatistics stats = new DailyLogStatistics();

            try
            {
                DateTime today = DateTime.Today;
                List<LogEntry> todayLogs = GetLogEntries(filterDate: today);

                stats.TotalCount = todayLogs.Count;
                stats.ErrorCount = todayLogs.Count(x => x.Level == "ERROR");
                stats.WarningCount = todayLogs.Count(x => x.Level == "WARNING");
                stats.InfoCount = todayLogs.Count(x => x.Level == "INFO");

                if (stats.TotalCount > 0)
                {
                    stats.SuccessRate = Math.Round((decimal)(stats.InfoCount * 100) / stats.TotalCount, 2);
                }
            }
            catch (Exception ex)
            {
                LogError("GetDailyLogStatistics hatası", ex);
            }

            return stats;
        }

        /// <summary>
        /// Son 7 günlük log trend verilerini getirir
        /// </summary>
        private WeeklyTrendData GetWeeklyLogTrend()
        {
            WeeklyTrendData trendData = new WeeklyTrendData();

            try
            {
                for (int i = 6; i >= 0; i--)
                {
                    DateTime date = DateTime.Today.AddDays(-i);
                    List<LogEntry> dayLogs = GetLogEntries(filterDate: date);

                    trendData.Dates.Add(date.ToString("dd.MM"));
                    trendData.ErrorCounts.Add(dayLogs.Count(x => x.Level == "ERROR"));
                    trendData.WarningCounts.Add(dayLogs.Count(x => x.Level == "WARNING"));
                    trendData.InfoCounts.Add(dayLogs.Count(x => x.Level == "INFO"));
                }
            }
            catch (Exception ex)
            {
                LogError("GetWeeklyLogTrend hatası", ex);
            }

            return trendData;
        }

        /// <summary>
        /// StackTrace'den modül adını çıkarır
        /// Örnek: "at Portal.Cimer.CimerList.Page_Load() in C:\...\CimerList.aspx.cs:line 45"
        /// Sonuç: "Portal.Cimer.CimerList"
        /// </summary>
        private string ExtractModuleFromStackTrace(string stackTrace)
        {
            if (string.IsNullOrEmpty(stackTrace))
                return "Bilinmeyen Modül";

            try
            {
                // "at " ile başlayan kısmı bul
                int atIndex = stackTrace.IndexOf("at ");
                if (atIndex == -1) return "Bilinmeyen Modül";

                string afterAt = stackTrace.Substring(atIndex + 3).Trim();

                // Parantez veya "in" kelimesine kadar olan kısmı al
                int parenIndex = afterAt.IndexOf('(');
                if (parenIndex > 0)
                {
                    string fullMethod = afterAt.Substring(0, parenIndex).Trim();

                    // Son nokta işaretine kadar olan kısmı al (metod adını çıkar)
                    int lastDotIndex = fullMethod.LastIndexOf('.');
                    if (lastDotIndex > 0)
                    {
                        return fullMethod.Substring(0, lastDotIndex);
                    }

                    return fullMethod;
                }

                return "Bilinmeyen Modül";
            }
            catch
            {
                return "Bilinmeyen Modül";
            }
        }

        /// <summary>
        /// En çok hata veren modülleri getirir
        /// </summary>
        private List<ModuleErrorStats> GetTopErrorModules(int topCount)
        {
            List<ModuleErrorStats> moduleStats = new List<ModuleErrorStats>();

            try
            {
                // Tüm ERROR loglarını al
                List<LogEntry> errorLogs = GetLogEntries(filterLevel: "ERROR");

                // StackTrace (Last) satırlarından modül bilgilerini çıkar
                var moduleGroups = errorLogs
                    .Where(x => x.Message.Contains("StackTrace (Last):"))
                    .Select(x =>
                    {
                        // StackTrace (Last): satırını bul
                        int stackTraceIndex = x.Message.IndexOf("StackTrace (Last):");
                        if (stackTraceIndex >= 0)
                        {
                            string stackTracePart = x.Message.Substring(stackTraceIndex);
                            return ExtractModuleFromStackTrace(stackTracePart);
                        }
                        return "Bilinmeyen Modül";
                    })
                    .GroupBy(x => x)
                    .Select(g => new ModuleErrorStats
                    {
                        ModuleName = g.Key,
                        ErrorCount = g.Count()
                    })
                    .OrderByDescending(x => x.ErrorCount)
                    .Take(topCount)
                    .ToList();

                moduleStats = moduleGroups;
            }
            catch (Exception ex)
            {
                LogError("GetTopErrorModules hatası", ex);
            }

            return moduleStats;
        }

        /// <summary>
        /// Son N adet hata kaydını getirir
        /// </summary>
        private List<RecentError> GetRecentErrors(int count)
        {
            List<RecentError> recentErrors = new List<RecentError>();

            try
            {
                List<LogEntry> errorLogs = GetLogEntries(filterLevel: "ERROR");

                recentErrors = errorLogs
                    .OrderByDescending(x => x.Timestamp)
                    .Take(count)
                    .Select(x => new RecentError
                    {
                        Timestamp = x.Timestamp,
                        Module = ExtractModuleFromMessage(x.Message),
                        Message = x.Message
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                LogError("GetRecentErrors hatası", ex);
            }

            return recentErrors;
        }

        /// <summary>
        /// Log mesajından modül adını çıkarır (StackTrace varsa)
        /// </summary>
        private string ExtractModuleFromMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return "Bilinmeyen";

            try
            {
                if (message.Contains("StackTrace (Last):"))
                {
                    int stackTraceIndex = message.IndexOf("StackTrace (Last):");
                    string stackTracePart = message.Substring(stackTraceIndex);
                    return ExtractModuleFromStackTrace(stackTracePart);
                }

                // StackTrace yoksa mesajın ilk 30 karakterini al
                return message.Length > 30 ? message.Substring(0, 30) : message;
            }
            catch
            {
                return "Bilinmeyen";
            }
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
                            label = "Hatalar",
                            data = data.ErrorCounts,
                            borderColor = "#dc3545",
                            backgroundColor = "rgba(220, 53, 69, 0.1)",
                            tension = 0.4
                        },
                        new {
                            label = "Uyarılar",
                            data = data.WarningCounts,
                            borderColor = "#ffc107",
                            backgroundColor = "rgba(255, 193, 7, 0.1)",
                            tension = 0.4
                        },
                        new {
                            label = "Bilgi",
                            data = data.InfoCounts,
                            borderColor = "#0dcaf0",
                            backgroundColor = "rgba(13, 202, 240, 0.1)",
                            tension = 0.4
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
        /// Log dağılımını JSON'a çevirir (Pie Chart için)
        /// </summary>
        private string SerializeLogDistribution(DailyLogStatistics stats)
        {
            try
            {
                var json = new
                {
                    labels = new[] { "Hatalar", "Uyarılar", "Bilgi" },
                    datasets = new[]
                    {
                        new {
                            data = new[] { stats.ErrorCount, stats.WarningCount, stats.InfoCount },
                            backgroundColor = new[] { "#dc3545", "#ffc107", "#0dcaf0" },
                            borderWidth = 0
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

        #endregion

        #region Button Events

        /// <summary>
        /// Yenile butonu
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
            ShowToast("Dashboard verileri yenilendi.", "success");
        }

        #endregion
    }
}