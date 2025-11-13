using Portal.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal
{
    public partial class LogViewer : BasePage
    {
        #region Log Management Classes

        /// <summary>
        /// Log entry model sınıfı
        /// </summary>
        public class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public string Level { get; set; }
            public string Message { get; set; }
            public string LevelClass { get; set; }
            public string LevelIcon { get; set; }
            public string LastStackTrace { get; set; }

            public LogEntry()
            {
                SetLevelProperties();
            }

            public void SetLevelProperties()
            {
                switch (Level)
                {
                    case "ERROR":
                        LevelClass = "danger";
                        LevelIcon = "fa-exclamation-circle";
                        break;
                    case "WARNING":
                        LevelClass = "warning";
                        LevelIcon = "fa-exclamation-triangle";
                        break;
                    case "INFO":
                        LevelClass = "info";
                        LevelIcon = "fa-info-circle";
                        break;
                    default:
                        LevelClass = "secondary";
                        LevelIcon = "fa-question-circle";
                        break;
                }
            }
        }

        /// <summary>
        /// Log istatistikleri model sınıfı
        /// </summary>
        public class LogStatistics
        {
            public int TotalCount { get; set; }
            public int ErrorCount { get; set; }
            public int WarningCount { get; set; }
            public int InfoCount { get; set; }
        }

        #endregion

        #region Log Management Methods

        /// <summary>
        /// ErrorLog.txt dosyasını okur ve log kayıtlarını liste olarak döner
        /// </summary>
        private List<LogEntry> GetLogEntries(string filterLevel = "", DateTime? filterDate = null, string searchText = "")
        {
            List<LogEntry> logEntries = new List<LogEntry>();

            try
            {
                string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(projectRoot, "ErrorLog.txt");

                if (!File.Exists(filePath))
                {
                    LogWarning("ErrorLog.txt dosyası bulunamadı.");
                    return logEntries;
                }

                string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
                LogEntry currentEntry = null;
                StringBuilder messageBuilder = new StringBuilder();

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];

                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // Yeni log satırı mı kontrol et (timestamp ile başlıyorsa)
                    if (line.StartsWith("[") && line.Contains("]"))
                    {
                        // Önceki entry'yi kaydet
                        if (currentEntry != null)
                        {
                            currentEntry.Message = messageBuilder.ToString().Trim();

                            // Filtreleme
                            if (ApplyFilters(currentEntry, filterLevel, filterDate, searchText))
                            {
                                logEntries.Add(currentEntry);
                            }
                        }

                        // Yeni entry başlat
                        currentEntry = ParseLogLine(line);
                        messageBuilder.Clear();
                    }
                    else
                    {
                        // Çok satırlı mesaj devamı
                        if (currentEntry != null)
                        {
                            messageBuilder.AppendLine(line);
                        }
                    }
                }

                // Son entry'yi kaydet
                if (currentEntry != null)
                {
                    currentEntry.Message = messageBuilder.ToString().Trim();
                    if (ApplyFilters(currentEntry, filterLevel, filterDate, searchText))
                    {
                        logEntries.Add(currentEntry);
                    }
                }

                logEntries = logEntries.OrderByDescending(x => x.Timestamp).ToList();
            }
            catch (Exception ex)
            {
                LogError("GetLogEntries hatası", ex);
            }

            return logEntries;
        }

        /// <summary>
        /// Filtreleme kontrolü yapar
        /// </summary>
        private bool ApplyFilters(LogEntry entry, string filterLevel, DateTime? filterDate, string searchText)
        {
            if (!string.IsNullOrEmpty(filterLevel) && entry.Level != filterLevel)
                return false;

            if (filterDate.HasValue && entry.Timestamp.Date != filterDate.Value.Date)
                return false;

            if (!string.IsNullOrEmpty(searchText) &&
                entry.Message.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) == -1)
                return false;

            return true;
        }

        /// <summary>
        /// Log satırını parse eder ve LogEntry objesi döner
        /// </summary>
        private LogEntry ParseLogLine(string line)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(line)) return null;

                int firstBracketEnd = line.IndexOf(']');
                if (firstBracketEnd == -1) return null;

                string dateStr = line.Substring(1, firstBracketEnd - 1).Trim();

                int secondBracketStart = line.IndexOf('[', firstBracketEnd);
                int secondBracketEnd = line.IndexOf(']', secondBracketStart);

                if (secondBracketStart == -1 || secondBracketEnd == -1) return null;

                string level = line.Substring(secondBracketStart + 1, secondBracketEnd - secondBracketStart - 1).Trim();
                string message = line.Substring(secondBracketEnd + 1).Trim();

                // Stack Trace'i ayıkla (varsa)
                string lastStackTrace = "";
                int stackTraceIndex = message.IndexOf("StackTrace (Last):");
                if (stackTraceIndex != -1)
                {
                    int stackTraceStart = stackTraceIndex + "StackTrace (Last):".Length;
                    int stackTraceEnd = message.IndexOf("\n", stackTraceStart);

                    if (stackTraceEnd == -1)
                        stackTraceEnd = message.Length;

                    lastStackTrace = message.Substring(stackTraceStart, stackTraceEnd - stackTraceStart).Trim();
                }

                LogEntry entry = new LogEntry
                {
                    Timestamp = DateTime.Parse(dateStr),
                    Level = level,
                    Message = message,
                    LastStackTrace = lastStackTrace
                };

                entry.SetLevelProperties();

                return entry;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Log istatistiklerini hesaplar
        /// </summary>
        private LogStatistics GetLogStatistics()
        {
            LogStatistics stats = new LogStatistics();

            try
            {
                List<LogEntry> allLogs = GetLogEntries();

                stats.TotalCount = allLogs.Count;
                stats.ErrorCount = allLogs.Count(x => x.Level == "ERROR");
                stats.WarningCount = allLogs.Count(x => x.Level == "WARNING");
                stats.InfoCount = allLogs.Count(x => x.Level == "INFO");
            }
            catch (Exception ex)
            {
                LogError("GetLogStatistics hatası", ex);
            }

            return stats;
        }

        /// <summary>
        /// Log dosyasını tamamen temizler
        /// </summary>
        private void ClearLogFile()
        {
            try
            {
                string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(projectRoot, "ErrorLog.txt");

                if (File.Exists(filePath))
                {
                    File.WriteAllText(filePath, string.Empty);
                    LogInfo("Log dosyası temizlendi.");
                }
            }
            catch (Exception ex)
            {
                LogError("ClearLogFile hatası", ex);
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLogData();
                LoadStatistics();
            }
        }

        /// <summary>
        /// Log verilerini yükler
        /// </summary>
        private void LoadLogData()
        {
            try
            {
                string filterLevel = ddlLevel.SelectedValue;
                DateTime? filterDate = null;

                if (!string.IsNullOrEmpty(txtDate.Text))
                {
                    filterDate = DateTime.Parse(txtDate.Text);
                }

                string searchText = txtSearch.Text.Trim();

                List<LogEntry> logs = GetLogEntries(filterLevel, filterDate, searchText);

                // Filter info label
                UpdateFilterInfoLabel(filterLevel, logs.Count);

                if (logs.Count == 0)
                {
                    pnlEmptyState.Visible = true;
                    gvLogs.Visible = false;
                }
                else
                {
                    pnlEmptyState.Visible = false;
                    gvLogs.Visible = true;
                    gvLogs.DataSource = logs;
                    gvLogs.DataBind();
                }
            }
            catch (Exception ex)
            {
                LogError("LoadLogData hatası", ex);
                ShowToast("Log verileri yüklenirken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// İstatistikleri yükler
        /// </summary>
        private void LoadStatistics()
        {
            try
            {
                LogStatistics stats = GetLogStatistics();

                lblTotalCount.Text = stats.TotalCount.ToString();
                lblErrorCount.Text = stats.ErrorCount.ToString();
                lblWarningCount.Text = stats.WarningCount.ToString();
                lblInfoCount.Text = stats.InfoCount.ToString();
            }
            catch (Exception ex)
            {
                LogError("LoadStatistics hatası", ex);
            }
        }

        /// <summary>
        /// Filtre bilgi label'ını günceller
        /// </summary>
        private void UpdateFilterInfoLabel(string filterLevel, int count)
        {
            if (!string.IsNullOrEmpty(filterLevel))
            {
                string levelText = GetLevelText(filterLevel);
                lblFilterInfo.Text = $"{levelText} ({count} kayıt)";
                lblFilterInfo.Visible = true;
            }
            else
            {
                lblFilterInfo.Visible = false;
            }
        }

        /// <summary>
        /// Seviye text'ini Türkçe'ye çevirir
        /// </summary>
        public string GetLevelText(string level)
        {
            switch (level)
            {
                case "ERROR":
                    return "Hata";
                case "WARNING":
                    return "Uyarı";
                case "INFO":
                    return "Bilgi";
                default:
                    return level;
            }
        }

        /// <summary>
        /// Filtreleme butonu
        /// </summary>
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            hdnFilterLevel.Value = ddlLevel.SelectedValue;
            LoadLogData();
            ShowToast("Filtre uygulandı.", "info");
        }

        /// <summary>
        /// Filtreyi temizle butonu
        /// </summary>
        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            ddlLevel.SelectedIndex = 0;
            txtDate.Text = string.Empty;
            txtSearch.Text = string.Empty;
            hdnFilterLevel.Value = string.Empty;
            LoadLogData();
            ShowToast("Filtre temizlendi.", "info");
        }

        /// <summary>
        /// Yenile butonu
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLogData();
            LoadStatistics();
            ShowToast("Sayfa yenilendi.", "success");
        }

        #region Stat Card Click Events

        /// <summary>
        /// Tümü kartına tıklama
        /// </summary>
        protected void lnkFilterAll_Click(object sender, EventArgs e)
        {
            ddlLevel.SelectedValue = "";
            hdnFilterLevel.Value = "";
            txtDate.Text = string.Empty;
            txtSearch.Text = string.Empty;
            LoadLogData();
            ShowToast("Tüm kayıtlar gösteriliyor.", "info");
        }

        /// <summary>
        /// Hata kartına tıklama
        /// </summary>
        protected void lnkFilterError_Click(object sender, EventArgs e)
        {
            ddlLevel.SelectedValue = "ERROR";
            hdnFilterLevel.Value = "ERROR";
            txtDate.Text = string.Empty;
            txtSearch.Text = string.Empty;
            LoadLogData();
            ShowToast("Sadece hatalar gösteriliyor.", "danger");
        }

        /// <summary>
        /// Uyarı kartına tıklama
        /// </summary>
        protected void lnkFilterWarning_Click(object sender, EventArgs e)
        {
            ddlLevel.SelectedValue = "WARNING";
            hdnFilterLevel.Value = "WARNING";
            txtDate.Text = string.Empty;
            txtSearch.Text = string.Empty;
            LoadLogData();
            ShowToast("Sadece uyarılar gösteriliyor.", "warning");
        }

        /// <summary>
        /// Bilgi kartına tıklama
        /// </summary>
        protected void lnkFilterInfo_Click(object sender, EventArgs e)
        {
            ddlLevel.SelectedValue = "INFO";
            hdnFilterLevel.Value = "INFO";
            txtDate.Text = string.Empty;
            txtSearch.Text = string.Empty;
            LoadLogData();
            ShowToast("Sadece bilgiler gösteriliyor.", "info");
        }

        #endregion

        #region Quick Filter Buttons

        /// <summary>
        /// Hızlı filtre - Tümü
        /// </summary>
        protected void btnQuickAll_Click(object sender, EventArgs e)
        {
            lnkFilterAll_Click(sender, e);
        }

        /// <summary>
        /// Hızlı filtre - Hatalar
        /// </summary>
        protected void btnQuickError_Click(object sender, EventArgs e)
        {
            lnkFilterError_Click(sender, e);
        }

        /// <summary>
        /// Hızlı filtre - Uyarılar
        /// </summary>
        protected void btnQuickWarning_Click(object sender, EventArgs e)
        {
            lnkFilterWarning_Click(sender, e);
        }

        /// <summary>
        /// Hızlı filtre - Bilgiler
        /// </summary>
        protected void btnQuickInfo_Click(object sender, EventArgs e)
        {
            lnkFilterInfo_Click(sender, e);
        }

        #endregion

        /// <summary>
        /// Excel export butonu
        /// </summary>
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvLogs.Rows.Count == 0)
                {
                    ShowToast("Export edilecek veri bulunamadı.", "warning");
                    return;
                }

                string filename = $"LogRaporu_{DateTime.Now:yyyyMMdd_HHmmss}.xls";
                ExportGridViewToExcel(gvLogs, filename, 0);
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowToast("Excel dosyası oluşturulurken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// PDF export butonu
        /// </summary>
        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvLogs.Rows.Count == 0)
                {
                    ShowToast("Export edilecek veri bulunamadı.", "warning");
                    return;
                }

                string filename = $"LogRaporu_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string baslik = "Sistem Log Raporu";
                ExportGridViewToPdf(gvLogs, filename, baslik);
            }
            catch (Exception ex)
            {
                LogError("PDF export hatası", ex);
                ShowToast("PDF dosyası oluşturulurken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Log dosyasını temizle butonu
        /// </summary>
        protected void btnClearLog_Click(object sender, EventArgs e)
        {
            try
            {
                ClearLogFile();
                LoadLogData();
                LoadStatistics();
                ShowToast("Log dosyası başarıyla temizlendi.", "success");
            }
            catch (Exception ex)
            {
                LogError("Log temizleme hatası", ex);
                ShowToast("Log dosyası temizlenirken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// GridView sayfalama
        /// </summary>
        protected void gvLogs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLogs.PageIndex = e.NewPageIndex;
            LoadLogData();
        }

        /// <summary>
        /// GridView render için gerekli override
        /// </summary>
        public override void VerifyRenderingInServerForm(Control control)
        {
            // Excel export için gerekli
        }
    }
}