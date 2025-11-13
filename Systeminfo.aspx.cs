using Portal.Base;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Portal
{
    public partial class SystemInfo : BasePage
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
                LoadSystemInformation();
            }
        }

        /// <summary>
        /// Tüm sistem bilgilerini yükler
        /// </summary>
        private void LoadSystemInformation()
        {
            try
            {
                // 1. Server bilgileri
                LoadServerInfo();

                // 2. Application Pool bilgileri
                LoadAppPoolInfo();

                // 3. CPU bilgileri
                LoadCpuInfo();

                // 4. Memory bilgileri
                LoadMemoryInfo();

                // 5. Disk bilgileri
                LoadDiskInfo();
            }
            catch (Exception ex)
            {
                LogError("LoadSystemInformation hatası", ex);
                ShowToast("Sistem bilgileri yüklenirken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Sunucu bilgilerini yükler
        /// </summary>
        private void LoadServerInfo()
        {
            try
            {
                var systemInfo = PerformanceHelper.GetSystemInfo();

                lblOperatingSystem.Text = systemInfo.OperatingSystem;
                lblDotNetVersion.Text = systemInfo.DotNetVersion;
                lblIISVersion.Text = systemInfo.IISVersion;
                lblServerName.Text = systemInfo.ServerName;
                lblMachineName.Text = systemInfo.MachineName;

                // Server uptime formatla
                lblServerUptime.Text = FormatUptime(systemInfo.ServerUptime);
            }
            catch (Exception ex)
            {
                LogError("LoadServerInfo hatası", ex);
            }
        }

        /// <summary>
        /// Application Pool bilgilerini yükler
        /// </summary>
        private void LoadAppPoolInfo()
        {
            try
            {
                var appPoolInfo = PerformanceHelper.GetAppPoolInfo();

                lblAppPoolName.Text = appPoolInfo.AppPoolName;
                lblAppPoolStatus.Text = appPoolInfo.Status;
                lblRuntimeVersion.Text = appPoolInfo.RuntimeVersion;
                lblPipelineMode.Text = appPoolInfo.PipelineMode;
            }
            catch (Exception ex)
            {
                LogError("LoadAppPoolInfo hatası", ex);
            }
        }

        /// <summary>
        /// CPU bilgilerini yükler
        /// </summary>
        private void LoadCpuInfo()
        {
            try
            {
                var systemInfo = PerformanceHelper.GetSystemInfo();

                lblProcessorName.Text = systemInfo.ProcessorName;
                lblProcessorCores.Text = systemInfo.ProcessorCores.ToString() + " Core";
                lblArchitecture.Text = systemInfo.Architecture;
            }
            catch (Exception ex)
            {
                LogError("LoadCpuInfo hatası", ex);
            }
        }

        /// <summary>
        /// Bellek bilgilerini yükler
        /// </summary>
        private void LoadMemoryInfo()
        {
            try
            {
                var memoryInfo = PerformanceHelper.GetMemoryInfo();

                lblTotalMemory.Text = memoryInfo.TotalMemoryGB + " GB";
                lblUsedMemory.Text = memoryInfo.UsedMemoryGB + " GB";
                lblFreeMemory.Text = memoryInfo.FreeMemoryGB + " GB";
                lblMemoryPercent.Text = memoryInfo.UsagePercent + "%";

                // Progress bar stilini ayarla
                memoryProgress.Style["width"] = memoryInfo.UsagePercent + "%";
            }
            catch (Exception ex)
            {
                LogError("LoadMemoryInfo hatası", ex);
            }
        }

        /// <summary>
        /// Disk bilgilerini yükler
        /// </summary>
        private void LoadDiskInfo()
        {
            try
            {
                List<PerformanceHelper.DiskInfo> disks = PerformanceHelper.GetDiskInfo();

                rptDisks.DataSource = disks;
                rptDisks.DataBind();
            }
            catch (Exception ex)
            {
                LogError("LoadDiskInfo hatası", ex);
            }
        }

        /// <summary>
        /// Uptime'ı okunabilir formata çevirir
        /// </summary>
        private string FormatUptime(TimeSpan uptime)
        {
            if (uptime.TotalDays >= 1)
            {
                return $"{(int)uptime.TotalDays} gün {uptime.Hours} saat";
            }
            else if (uptime.TotalHours >= 1)
            {
                return $"{(int)uptime.TotalHours} saat {uptime.Minutes} dakika";
            }
            else
            {
                return $"{uptime.Minutes} dakika {uptime.Seconds} saniye";
            }
        }

        /// <summary>
        /// Disk kullanım yüzdesine göre progress bar class'ı döndürür
        /// </summary>
        protected string GetProgressBarClass(int usagePercent)
        {
            if (usagePercent < 70)
                return "progress-bar-success";
            else if (usagePercent < 85)
                return "progress-bar-warning";
            else
                return "progress-bar-danger";
        }

        /// <summary>
        /// Yenile butonu
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadSystemInformation();
            ShowToast("Sistem bilgileri yenilendi.", "success");
        }
    }
}