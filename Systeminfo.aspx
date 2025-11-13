<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemInfo.aspx.cs" Inherits="Portal.SystemInfo" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="tr">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Sistem Bilgileri - Portal Yönetim</title>

    <!-- Bootstrap 5.3.8 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />

    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            background: linear-gradient(135deg, #1e1e1e 0%, #2d2d2d 100%);
            color: #e0e0e0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            min-height: 100vh;
            padding: 20px;
        }

        .dashboard-header {
            background: linear-gradient(135deg, #2d2d2d 0%, #1a1a1a 100%);
            border-radius: 15px;
            padding: 30px;
            margin-bottom: 30px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
            border: 1px solid #3a3a3a;
        }

            .dashboard-header h1 {
                color: #ffffff;
                font-size: 2.5rem;
                font-weight: 700;
                margin-bottom: 10px;
                text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.5);
            }

            .dashboard-header p {
                color: #b0b0b0;
                font-size: 1rem;
                margin: 0;
            }

        .info-card {
            background: linear-gradient(135deg, #2d2d2d 0%, #1f1f1f 100%);
            border-radius: 15px;
            padding: 25px;
            margin-bottom: 20px;
            box-shadow: 0 8px 25px rgba(0, 0, 0, 0.4);
            border: 1px solid #3a3a3a;
        }

            .info-card h3 {
                color: #ffffff;
                font-size: 1.3rem;
                margin-bottom: 20px;
                font-weight: 600;
                display: flex;
                align-items: center;
            }

                .info-card h3 i {
                    margin-right: 12px;
                    width: 40px;
                    height: 40px;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    border-radius: 10px;
                    font-size: 1.2rem;
                }

        .icon-server {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        }

        .icon-pool {
            background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
        }

        .icon-disk {
            background: linear-gradient(135deg, #ffa726 0%, #fb8c00 100%);
        }

        .icon-memory {
            background: linear-gradient(135deg, #f85032 0%, #e73827 100%);
        }

        .icon-cpu {
            background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
        }

        .info-row {
            display: flex;
            justify-content: space-between;
            padding: 15px 0;
            border-bottom: 1px solid #3a3a3a;
        }

            .info-row:last-child {
                border-bottom: none;
            }

        .info-label {
            color: #b0b0b0;
            font-size: 0.95rem;
            font-weight: 500;
        }

        .info-value {
            color: #ffffff;
            font-size: 0.95rem;
            font-weight: 600;
            text-align: right;
        }

        .status-badge {
            display: inline-block;
            padding: 5px 15px;
            border-radius: 20px;
            font-size: 0.85rem;
            font-weight: 600;
        }

        .status-running {
            background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
            color: white;
        }

        .status-stopped {
            background: linear-gradient(135deg, #f85032 0%, #e73827 100%);
            color: white;
        }

        .progress-container {
            margin-top: 10px;
        }

        .progress {
            height: 25px;
            background-color: #1a1a1a;
            border-radius: 12px;
            overflow: hidden;
        }

        .progress-bar {
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 0.85rem;
            font-weight: 600;
            transition: width 0.6s ease;
        }

        .progress-bar-success {
            background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
        }

        .progress-bar-warning {
            background: linear-gradient(135deg, #ffa726 0%, #fb8c00 100%);
        }

        .progress-bar-danger {
            background: linear-gradient(135deg, #f85032 0%, #e73827 100%);
        }

        .disk-item {
            background: rgba(255, 255, 255, 0.03);
            border-radius: 12px;
            padding: 20px;
            margin-bottom: 15px;
        }

            .disk-item:last-child {
                margin-bottom: 0;
            }

        .disk-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 12px;
        }

        .disk-name {
            color: #667eea;
            font-size: 1.2rem;
            font-weight: 700;
        }

        .disk-stats {
            color: #b0b0b0;
            font-size: 0.9rem;
        }

        .btn-refresh {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border: none;
            color: white;
            padding: 12px 30px;
            border-radius: 10px;
            font-weight: 600;
            transition: all 0.3s ease;
            box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
        }

            .btn-refresh:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 20px rgba(102, 126, 234, 0.6);
                background: linear-gradient(135deg, #764ba2 0%, #667eea 100%);
                color: white;
            }

        .uptime-badge {
            background: rgba(67, 233, 123, 0.2);
            color: #43e97b;
            padding: 8px 16px;
            border-radius: 20px;
            font-size: 0.9rem;
            font-weight: 600;
            border: 1px solid #43e97b;
        }

        @media (max-width: 768px) {
            .dashboard-header h1 {
                font-size: 1.8rem;
            }

            .disk-header {
                flex-direction: column;
                align-items: flex-start;
            }

            .disk-stats {
                margin-top: 8px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <!-- Header -->
        <div class="dashboard-header">
            <div class="d-flex justify-content-between align-items-center flex-wrap">
                <div>
                    <h1><i class="fas fa-server me-3"></i>Sistem Bilgileri</h1>
                    <p><i class="far fa-clock me-2"></i>Son Güncelleme: <%= DateTime.Now.ToString("dd.MM.yyyy HH:mm") %></p>
                </div>
                <asp:Button ID="btnRefresh" runat="server" Text="🔄 Yenile" CssClass="btn-refresh" OnClick="btnRefresh_Click" />
            </div>
        </div>

        <div class="row">
            <!-- Server Information -->
            <div class="col-lg-6">
                <div class="info-card">
                    <h3>
                        <i class="fas fa-server icon-server"></i>
                        Sunucu Bilgileri
                    </h3>

                    <div class="info-row">
                        <span class="info-label"><i class="fab fa-windows me-2"></i>İşletim Sistemi</span>
                        <span class="info-value">
                            <asp:Label ID="lblOperatingSystem" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>

                    <div class="info-row">
                        <span class="info-label"><i class="fas fa-code me-2"></i>.NET Framework</span>
                        <span class="info-value">
                            <asp:Label ID="lblDotNetVersion" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>

                    <div class="info-row">
                        <span class="info-label"><i class="fas fa-globe me-2"></i>IIS Versiyonu</span>
                        <span class="info-value">
                            <asp:Label ID="lblIISVersion" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>

                    <div class="info-row">
                        <span class="info-label"><i class="fas fa-network-wired me-2"></i>Sunucu Adı</span>
                        <span class="info-value">
                            <asp:Label ID="lblServerName" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>

                    <div class="info-row">
                        <span class="info-label"><i class="fas fa-desktop me-2"></i>Makine Adı</span>
                        <span class="info-value">
                            <asp:Label ID="lblMachineName" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>

                    <div class="info-row">
                        <span class="info-label"><i class="fas fa-clock me-2"></i>Sunucu Çalışma Süresi</span>
                        <span class="info-value">
                            <span class="uptime-badge">
                                <asp:Label ID="lblServerUptime" runat="server" Text="0 gün"></asp:Label>
                            </span>
                        </span>
                    </div>
                </div>
            </div>

            <!-- Application Pool Information -->
            <div class="col-lg-6">
                <div class="info-card">
                    <h3>
                        <i class="fas fa-layer-group icon-pool"></i>
                        Application Pool Bilgileri
                    </h3>

                    <div class="info-row">
                        <span class="info-label"><i class="fas fa-tag me-2"></i>App Pool Adı</span>
                        <span class="info-value">
                            <asp:Label ID="lblAppPoolName" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>

                    <div class="info-row">
                        <span class="info-label"><i class="fas fa-heartbeat me-2"></i>Durum</span>
                        <span class="info-value">
                            <span class="status-badge status-running">
                                <i class="fas fa-check-circle me-1"></i>
                                <asp:Label ID="lblAppPoolStatus" runat="server" Text="Running"></asp:Label>
                            </span>
                        </span>
                    </div>

                    <div class="info-row">
                        <span class="info-label"><i class="fas fa-code-branch me-2"></i>Runtime Version</span>
                        <span class="info-value">
                            <asp:Label ID="lblRuntimeVersion" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>

                    <div class="info-row">
                        <span class="info-label"><i class="fas fa-route me-2"></i>Pipeline Mode</span>
                        <span class="info-value">
                            <asp:Label ID="lblPipelineMode" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>
                </div>

                <!-- CPU Information -->
                <div class="info-card">
                    <h3>
                        <i class="fas fa-microchip icon-cpu"></i>
                        İşlemci Bilgileri
                    </h3>

                    <div class="info-row">
                        <span class="info-label"><i class="fas fa-brain me-2"></i>İşlemci</span>
                        <span class="info-value">
                            <asp:Label ID="lblProcessorName" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>

                    <div class="info-row">
                        <span class="info-label"><i class="fas fa-layer-group me-2"></i>Çekirdek Sayısı</span>
                        <span class="info-value">
                            <asp:Label ID="lblProcessorCores" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>

                    <div class="info-row">
                        <span class="info-label"><i class="fas fa-cogs me-2"></i>Mimari</span>
                        <span class="info-value">
                            <asp:Label ID="lblArchitecture" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <!-- Memory Information -->
            <div class="col-lg-6">
                <div class="info-card">
                    <h3>
                        <i class="fas fa-memory icon-memory"></i>
                        Bellek Kullanımı
                    </h3>

                    <div class="info-row">
                        <span class="info-label"><i class="fas fa-hdd me-2"></i>Toplam Bellek</span>
                        <span class="info-value">
                            <asp:Label ID="lblTotalMemory" runat="server" Text="0 GB"></asp:Label>
                        </span>
                    </div>

                    <div class="info-row">
                        <span class="info-label"><i class="fas fa-chart-pie me-2"></i>Kullanılan</span>
                        <span class="info-value">
                            <asp:Label ID="lblUsedMemory" runat="server" Text="0 GB"></asp:Label>
                        </span>
                    </div>

                    <div class="info-row">
                        <span class="info-label"><i class="fas fa-inbox me-2"></i>Boş</span>
                        <span class="info-value">
                            <asp:Label ID="lblFreeMemory" runat="server" Text="0 GB"></asp:Label>
                        </span>
                    </div>

                    <div class="progress-container">
                        <div class="progress">
                            <div class="progress-bar progress-bar-danger" role="progressbar" style="width: 0%;" id="memoryProgress" runat="server">
                                <asp:Label ID="lblMemoryPercent" runat="server" Text="0%"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Disk Usage -->
            <div class="col-lg-6">
                <div class="info-card">
                    <h3>
                        <i class="fas fa-hard-drive icon-disk"></i>
                        Disk Kullanımı
                    </h3>

                    <asp:Repeater ID="rptDisks" runat="server">
                        <ItemTemplate>
                            <div class="disk-item">
                                <div class="disk-header">
                                    <div class="disk-name">
                                        <i class="fas fa-hdd me-2"></i><%# Eval("DriveName") %>
                                    </div>
                                    <div class="disk-stats">
                                        <%# Eval("UsedSpaceGB") %> GB / <%# Eval("TotalSpaceGB") %> GB
                                       
                                        <span style="color: #808080; margin-left: 10px;">(Boş: <%# Eval("FreeSpaceGB") %> GB)
                                        </span>
                                    </div>
                                </div>
                                <div class="progress">
                                    <div class="progress-bar <%# ((int)Eval("UsagePercent") < 70) ? "progress-bar-success" : ((int)Eval("UsagePercent") < 85) ? "progress-bar-warning" : "progress-bar-danger" %>"
                                        role="progressbar"
                                        style="width: <%# Eval("UsagePercent") %>%;">
                                    </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>

    </form>

    <!-- Bootstrap 5.3.8 JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
