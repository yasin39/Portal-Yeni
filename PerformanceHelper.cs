using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Configuration;

namespace Portal
{
    /// <summary>
    /// Performans izleme yardımcı sınıfı
    /// </summary>
    public static class PerformanceHelper
    {
        // Request bazlı performans verileri için key'ler
        private const string START_TIME_KEY = "PerformanceStartTime";
        private const string QUERY_COUNT_KEY = "SqlQueryCount";
        private const string PAGE_NAME_KEY = "PerformancePageName";

        /// <summary>
        /// Performans izleme aktif mi? (Web.config'den okunur)
        /// </summary>
        public static bool IsEnabled
        {
            get
            {
                string value = ConfigurationManager.AppSettings["PerformanceMonitoring"];
                return value != null && value.ToLower() == "true";
            }
        }

        /// <summary>
        /// Sayfa yüklenmeye başladığında çağrılır
        /// </summary>
        /// <param name="pageName">Sayfa adı (örn: Dashboard.aspx)</param>
        public static void StartMonitoring(string pageName)
        {
            if (!IsEnabled) return;

            try
            {
                HttpContext.Current.Items[START_TIME_KEY] = Stopwatch.GetTimestamp();
                HttpContext.Current.Items[QUERY_COUNT_KEY] = 0;
                HttpContext.Current.Items[PAGE_NAME_KEY] = pageName;
            }
            catch
            {
                // Sessizce geç - monitoring hatası uygulamayı bozmasın
            }
        }

        /// <summary>
        /// Sayfa yüklenmesi bittiğinde çağrılır ve veritabanına kaydeder
        /// </summary>
        /// <param name="userSicil">Kullanıcı sicil no</param>
        /// <param name="userName">Kullanıcı adı</param>
        public static void EndMonitoring(string userSicil, string userName)
        {
            if (!IsEnabled) return;

            try
            {
                var startTime = HttpContext.Current.Items[START_TIME_KEY];
                var pageName = HttpContext.Current.Items[PAGE_NAME_KEY];
                var queryCount = HttpContext.Current.Items[QUERY_COUNT_KEY];

                if (startTime == null || pageName == null) return;

                // Süre hesaplama (milisaniye)
                long startTicks = (long)startTime;
                long endTicks = Stopwatch.GetTimestamp();
                double elapsedMs = ((endTicks - startTicks) * 1000.0) / Stopwatch.Frequency;

                // Memory kullanımı (MB)
                decimal memoryMB = (decimal)GC.GetTotalMemory(false) / (1024 * 1024);

                // Veritabanına kaydet
                SavePerformanceLog(
                    pageName.ToString(),
                    (int)Math.Round(elapsedMs),
                    (int)(queryCount ?? 0),
                    Math.Round(memoryMB, 2),
                    userSicil,
                    userName
                );
            }
            catch
            {
                // Sessizce geç - monitoring hatası uygulamayı bozmasın
            }
        }

        /// <summary>
        /// SQL query sayacını artırır (her ExecuteXXX metodunda çağrılır)
        /// </summary>
        public static void IncrementQueryCount()
        {
            if (!IsEnabled) return;

            try
            {
                var count = HttpContext.Current.Items[QUERY_COUNT_KEY];
                HttpContext.Current.Items[QUERY_COUNT_KEY] = (int)(count ?? 0) + 1;
            }
            catch
            {
                // Sessizce geç
            }
        }

        /// <summary>
        /// Performans verisini veritabanına kaydeder
        /// </summary>
        private static void SavePerformanceLog(string pageName, int loadTimeMs, int sqlQueryCount, decimal memoryUsageMB, string userSicil, string userName)
        {
            try
            {
                string query = @"
                    INSERT INTO PerformanceLogs 
                    (PageName, LoadTimeMs, SqlQueryCount, MemoryUsageMB, UserSicil, UserName, CreatedDate)
                    VALUES 
                    (@PageName, @LoadTimeMs, @SqlQueryCount, @MemoryUsageMB, @UserSicil, @UserName, GETDATE())";

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@PageName", pageName ?? "Unknown"),
                    new SqlParameter("@LoadTimeMs", loadTimeMs),
                    new SqlParameter("@SqlQueryCount", sqlQueryCount),
                    new SqlParameter("@MemoryUsageMB", memoryUsageMB),
                    new SqlParameter("@UserSicil", (object)userSicil ?? DBNull.Value),
                    new SqlParameter("@UserName", (object)userName ?? DBNull.Value)
                };

                // BasePage.ExecuteNonQuery metodunu kullan
                Portal.Base.BasePage.ExecuteNonQuery(query, parameters);
            }
            catch
            {
                // Hata olsa bile sessizce geç - monitoring hatası uygulamayı bozmasın
            }
        }

        #region System Information Helper Methods

        /// <summary>
        /// Sistem bilgileri model sınıfı
        /// </summary>
        public class SystemInfo
        {
            public string OperatingSystem { get; set; }
            public string DotNetVersion { get; set; }
            public string IISVersion { get; set; }
            public string ServerName { get; set; }
            public string MachineName { get; set; }
            public string ProcessorName { get; set; }
            public int ProcessorCores { get; set; }
            public string Architecture { get; set; }
            public TimeSpan ServerUptime { get; set; }
        }

        /// <summary>
        /// Disk bilgileri model sınıfı
        /// </summary>
        public class DiskInfo
        {
            public string DriveName { get; set; }
            public long TotalSpaceGB { get; set; }
            public long UsedSpaceGB { get; set; }
            public long FreeSpaceGB { get; set; }
            public int UsagePercent { get; set; }
        }

        /// <summary>
        /// Bellek bilgileri model sınıfı
        /// </summary>
        public class MemoryInfo
        {
            public long TotalMemoryGB { get; set; }
            public long UsedMemoryGB { get; set; }
            public long FreeMemoryGB { get; set; }
            public int UsagePercent { get; set; }
        }

        /// <summary>
        /// Application Pool bilgileri model sınıfı
        /// </summary>
        public class AppPoolInfo
        {
            public string AppPoolName { get; set; }
            public string Status { get; set; }
            public string RuntimeVersion { get; set; }
            public string PipelineMode { get; set; }
        }

        /// <summary>
        /// Sistem bilgilerini getirir
        /// </summary>
        public static SystemInfo GetSystemInfo()
        {
            var info = new SystemInfo();

            try
            {
                // Operating System
                info.OperatingSystem = Environment.OSVersion.ToString();

                // .NET Version
                info.DotNetVersion = Environment.Version.ToString();

                // IIS Version
                info.IISVersion = "IIS " + HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"];

                // Server Name
                info.ServerName = Environment.MachineName;

                // Machine Name
                info.MachineName = Environment.MachineName;

                // Processor Name
                info.ProcessorName = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") ?? "Unknown";

                // Processor Cores
                info.ProcessorCores = Environment.ProcessorCount;

                // Architecture
                info.Architecture = Environment.Is64BitOperatingSystem ? "x64" : "x86";

                // Server Uptime
                info.ServerUptime = TimeSpan.FromMilliseconds(Environment.TickCount);
            }
            catch
            {
                // Hata olursa varsayılan değerler kalır
            }

            return info;
        }

        /// <summary>
        /// Disk kullanım bilgilerini getirir
        /// </summary>
        public static List<DiskInfo> GetDiskInfo()
        {
            var disks = new List<DiskInfo>();

            try
            {
                var drives = System.IO.DriveInfo.GetDrives();

                foreach (var drive in drives)
                {
                    if (drive.IsReady && drive.DriveType == System.IO.DriveType.Fixed)
                    {
                        long totalGB = drive.TotalSize / (1024 * 1024 * 1024);
                        long freeGB = drive.AvailableFreeSpace / (1024 * 1024 * 1024);
                        long usedGB = totalGB - freeGB;
                        int usagePercent = totalGB > 0 ? (int)((usedGB * 100) / totalGB) : 0;

                        disks.Add(new DiskInfo
                        {
                            DriveName = drive.Name,
                            TotalSpaceGB = totalGB,
                            UsedSpaceGB = usedGB,
                            FreeSpaceGB = freeGB,
                            UsagePercent = usagePercent
                        });
                    }
                }
            }
            catch
            {
                // Hata olursa boş liste döner
            }

            return disks;
        }

        /// <summary>
        /// Bellek kullanım bilgilerini getirir
        /// </summary>
        public static MemoryInfo GetMemoryInfo()
        {
            var memInfo = new MemoryInfo();

            try
            {
                // GC'den process memory bilgisi
                long totalMemoryBytes = GC.GetTotalMemory(false);
                long totalMemoryMB = totalMemoryBytes / (1024 * 1024);

                // Basit bir tahmin (gerçek fiziksel bellek için WMI gerekir)
                memInfo.TotalMemoryGB = 16; // Varsayılan
                memInfo.UsedMemoryGB = totalMemoryMB / 1024;
                memInfo.FreeMemoryGB = memInfo.TotalMemoryGB - memInfo.UsedMemoryGB;
                memInfo.UsagePercent = memInfo.TotalMemoryGB > 0
                    ? (int)((memInfo.UsedMemoryGB * 100) / memInfo.TotalMemoryGB)
                    : 0;
            }
            catch
            {
                // Hata olursa varsayılan değerler
            }

            return memInfo;
        }

        /// <summary>
        /// Application Pool bilgilerini getirir
        /// </summary>
        public static AppPoolInfo GetAppPoolInfo()
        {
            var appPoolInfo = new AppPoolInfo();

            try
            {
                // Application Pool adı
                appPoolInfo.AppPoolName = System.Web.Hosting.HostingEnvironment.ApplicationHost.GetSiteName();

                // Status (Her zaman çalışıyor olarak varsayıyoruz)
                appPoolInfo.Status = "Running";

                // Runtime Version
                appPoolInfo.RuntimeVersion = Environment.Version.ToString();

                // Pipeline Mode (basitleştirilmiş)
                appPoolInfo.PipelineMode = "Integrated";
            }
            catch
            {
                appPoolInfo.AppPoolName = "Unknown";
                appPoolInfo.Status = "Unknown";
                appPoolInfo.RuntimeVersion = "Unknown";
                appPoolInfo.PipelineMode = "Unknown";
            }

            return appPoolInfo;
        }

        #endregion
    }
}