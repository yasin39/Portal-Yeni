<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DatabaseHealth.aspx.cs" Inherits="Portal.DatabaseHealth" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="tr">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Database Sağlık İzleme - Portal Yönetim</title>

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

        .stat-card {
            background: linear-gradient(135deg, #2d2d2d 0%, #1f1f1f 100%);
            border-radius: 15px;
            padding: 25px;
            margin-bottom: 20px;
            box-shadow: 0 8px 25px rgba(0, 0, 0, 0.4);
            border: 1px solid #3a3a3a;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
        }

        .stat-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 12px 35px rgba(0, 0, 0, 0.6);
        }

        .stat-card-icon {
            width: 60px;
            height: 60px;
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.8rem;
            margin-bottom: 15px;
        }

        .stat-card-icon.primary {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
        }

        .stat-card-icon.success {
            background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
            color: white;
        }

        .stat-card-icon.warning {
            background: linear-gradient(135deg, #ffa726 0%, #fb8c00 100%);
            color: white;
        }

        .stat-card-icon.danger {
            background: linear-gradient(135deg, #f85032 0%, #e73827 100%);
            color: white;
        }

        .stat-card-title {
            font-size: 0.9rem;
            color: #b0b0b0;
            text-transform: uppercase;
            letter-spacing: 1px;
            margin-bottom: 8px;
        }

        .stat-card-value {
            font-size: 2.5rem;
            font-weight: 700;
            color: #ffffff;
            line-height: 1;
        }

        .stat-card-subtitle {
            font-size: 0.85rem;
            color: #808080;
            margin-top: 8px;
        }

        .chart-card {
            background: linear-gradient(135deg, #2d2d2d 0%, #1f1f1f 100%);
            border-radius: 15px;
            padding: 25px;
            margin-bottom: 20px;
            box-shadow: 0 8px 25px rgba(0, 0, 0, 0.4);
            border: 1px solid #3a3a3a;
        }

        .chart-card h3 {
            color: #ffffff;
            font-size: 1.3rem;
            margin-bottom: 20px;
            font-weight: 600;
        }

        .table-card {
            background: linear-gradient(135deg, #2d2d2d 0%, #1f1f1f 100%);
            border-radius: 15px;
            padding: 25px;
            margin-bottom: 20px;
            box-shadow: 0 8px 25px rgba(0, 0, 0, 0.4);
            border: 1px solid #3a3a3a;
        }

        .table-card h3 {
            color: #ffffff;
            font-size: 1.3rem;
            margin-bottom: 20px;
            font-weight: 600;
        }

        .custom-table {
            width: 100%;
            color: #e0e0e0;
        }

        .custom-table thead th {
            background: #1a1a1a;
            color: #b0b0b0;
            text-transform: uppercase;
            font-size: 0.85rem;
            letter-spacing: 1px;
            padding: 15px;
            border: none;
            font-weight: 600;
        }

        .custom-table tbody td {
            padding: 15px;
            border-bottom: 1px solid #3a3a3a;
            vertical-align: middle;
        }

        .custom-table tbody tr:last-child td {
            border-bottom: none;
        }

        .custom-table tbody tr:hover {
            background: rgba(255, 255, 255, 0.05);
        }

        .badge-status {
            padding: 5px 12px;
            border-radius: 20px;
            font-weight: 600;
            font-size: 0.85rem;
        }

        .badge-good {
            background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
            color: white;
        }

        .badge-warning {
            background: linear-gradient(135deg, #ffa726 0%, #fb8c00 100%);
            color: white;
        }

        .badge-danger {
            background: linear-gradient(135deg, #f85032 0%, #e73827 100%);
            color: white;
        }

        .badge-info {
            background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
            color: white;
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

        .query-text {
            background: #1a1a1a;
            padding: 10px;
            border-radius: 8px;
            font-family: 'Courier New', monospace;
            font-size: 0.85rem;
            color: #ffa726;
            max-height: 80px;
            overflow-y: auto;
            white-space: pre-wrap;
            word-wrap: break-word;
        }

        .timestamp {
            color: #808080;
            font-size: 0.85rem;
        }

        .table-name {
            color: #667eea;
            font-weight: 600;
        }

        .size-badge {
            background: rgba(102, 126, 234, 0.2);
            color: #667eea;
            padding: 5px 12px;
            border-radius: 15px;
            font-size: 0.85rem;
            font-weight: 600;
        }

        .index-recommendation {
            background: rgba(255, 167, 38, 0.1);
            border-left: 3px solid #ffa726;
            padding: 12px;
            border-radius: 8px;
            margin-bottom: 10px;
        }

        .index-recommendation strong {
            color: #ffa726;
        }

        @media (max-width: 768px) {
            .dashboard-header h1 {
                font-size: 1.8rem;
            }

            .stat-card-value {
                font-size: 2rem;
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
                    <h1><i class="fas fa-database me-3"></i>Database Sağlık İzleme</h1>
                    <p><i class="far fa-clock me-2"></i>Son Güncelleme: <%= DateTime.Now.ToString("dd.MM.yyyy HH:mm") %></p>
                </div>
                <asp:Button ID="btnRefresh" runat="server" Text="🔄 Yenile" CssClass="btn-refresh" OnClick="btnRefresh_Click" />
            </div>
        </div>

        <!-- Stat Cards -->
        <div class="row">
            <!-- Toplam Tablo Sayısı -->
            <div class="col-lg-3 col-md-6">
                <div class="stat-card">
                    <div class="stat-card-icon primary">
                        <i class="fas fa-table"></i>
                    </div>
                    <div class="stat-card-title">Toplam Tablo</div>
                    <div class="stat-card-value">
                        <asp:Label ID="lblTotalTables" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stat-card-subtitle">User Tables</div>
                </div>
            </div>

            <!-- Database Boyutu -->
            <div class="col-lg-3 col-md-6">
                <div class="stat-card">
                    <div class="stat-card-icon success">
                        <i class="fas fa-hdd"></i>
                    </div>
                    <div class="stat-card-title">Database Boyutu</div>
                    <div class="stat-card-value">
                        <asp:Label ID="lblDatabaseSize" runat="server" Text="0"></asp:Label>
                        <span style="font-size: 1.2rem; color: #b0b0b0;">GB</span>
                    </div>
                    <div class="stat-card-subtitle">Veri + Index</div>
                </div>
            </div>

            <!-- Aktif Bağlantı -->
            <div class="col-lg-3 col-md-6">
                <div class="stat-card">
                    <div class="stat-card-icon warning">
                        <i class="fas fa-plug"></i>
                    </div>
                    <div class="stat-card-title">Aktif Bağlantı</div>
                    <div class="stat-card-value">
                        <asp:Label ID="lblActiveConnections" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stat-card-subtitle">Şu anda</div>
                </div>
            </div>

            <!-- Yavaş Query -->
            <div class="col-lg-3 col-md-6">
                <div class="stat-card">
                    <div class="stat-card-icon danger">
                        <i class="fas fa-exclamation-triangle"></i>
                    </div>
                    <div class="stat-card-title">Yavaş Query (>1s)</div>
                    <div class="stat-card-value">
                        <asp:Label ID="lblSlowQueries" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stat-card-subtitle">Son 24 saat</div>
                </div>
            </div>
        </div>

        <!-- Charts -->
        <div class="row">
            <!-- Bar Chart - En Büyük Tablolar -->
            <div class="col-lg-8">
                <div class="chart-card">
                    <h3><i class="fas fa-chart-bar me-2"></i>En Büyük 15 Tablo (MB)</h3>
                    <canvas id="tableSizeChart" height="80"></canvas>
                </div>
            </div>

            <!-- Connection Pool Durumu -->
            <div class="col-lg-4">
                <div class="table-card">
                    <h3><i class="fas fa-network-wired me-2"></i>Connection Pool</h3>
                    <div class="custom-table">
                        <div class="info-row" style="display: flex; justify-content: space-between; padding: 15px 0; border-bottom: 1px solid #3a3a3a;">
                            <span style="color: #b0b0b0;">Toplam Bağlantı</span>
                            <span style="color: #fff; font-weight: 600;">
                                <asp:Label ID="lblTotalConnections" runat="server" Text="0"></asp:Label>
                            </span>
                        </div>
                        <div class="info-row" style="display: flex; justify-content: space-between; padding: 15px 0; border-bottom: 1px solid #3a3a3a;">
                            <span style="color: #b0b0b0;">Aktif</span>
                            <span style="color: #43e97b; font-weight: 600;">
                                <asp:Label ID="lblActiveConn" runat="server" Text="0"></asp:Label>
                            </span>
                        </div>
                        <div class="info-row" style="display: flex; justify-content: space-between; padding: 15px 0;">
                            <span style="color: #b0b0b0;">Boşta (Idle)</span>
                            <span style="color: #ffa726; font-weight: 600;">
                                <asp:Label ID="lblIdleConn" runat="server" Text="0"></asp:Label>
                            </span>
                        </div>
                    </div>
                </div>

                <!-- Backup Durumu -->
                <div class="table-card">
                    <h3><i class="fas fa-save me-2"></i>Son Backup</h3>
                    <div class="custom-table">
                        <div class="info-row" style="display: flex; justify-content: space-between; padding: 15px 0; border-bottom: 1px solid #3a3a3a;">
                            <span style="color: #b0b0b0;">Tarih</span>
                            <span style="color: #fff; font-weight: 600;">
                                <asp:Label ID="lblLastBackupDate" runat="server" Text="-"></asp:Label>
                            </span>
                        </div>
                        <div class="info-row" style="display: flex; justify-content: space-between; padding: 15px 0; border-bottom: 1px solid #3a3a3a;">
                            <span style="color: #b0b0b0;">Tür</span>
                            <span style="color: #667eea; font-weight: 600;">
                                <asp:Label ID="lblLastBackupType" runat="server" Text="-"></asp:Label>
                            </span>
                        </div>
                        <div class="info-row" style="display: flex; justify-content: space-between; padding: 15px 0;">
                            <span style="color: #b0b0b0;">Boyut</span>
                            <span style="color: #ffa726; font-weight: 600;">
                                <asp:Label ID="lblLastBackupSize" runat="server" Text="-"></asp:Label>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Tables -->
        <div class="row">
            <!-- En Büyük Tablolar -->
            <div class="col-lg-6">
                <div class="table-card">
                    <h3><i class="fas fa-table me-2"></i>En Büyük 20 Tablo</h3>
                    <div style="max-height: 600px; overflow-y: auto;">
                        <table class="custom-table">
                            <thead>
                                <tr>
                                    <th style="width: 50px;">#</th>
                                    <th>Tablo Adı</th>
                                    <th style="width: 100px; text-align: right;">Satır</th>
                                    <th style="width: 100px; text-align: right;">Boyut</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptLargestTables" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="color: #808080;"><%# Container.ItemIndex + 1 %></td>
                                            <td class="table-name"><%# Eval("TableName") %></td>
                                            <td style="text-align: right; color: #b0b0b0;">
                                                <%# String.Format("{0:N0}", Eval("RowCount")) %>
                                            </td>
                                            <td style="text-align: right;">
                                                <span class="size-badge"><%# Eval("TotalSizeMB") %> MB</span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

            <!-- Index Analizi -->
            <div class="col-lg-6">
                <div class="table-card">
                    <h3><i class="fas fa-search me-2"></i>Index Analizi</h3>
                    
                    <!-- Kullanılmayan Indexler -->
                    <h5 style="color: #f85032; margin-bottom: 15px;">
                        <i class="fas fa-times-circle me-2"></i>Kullanılmayan Indexler
                    </h5>
                    <div style="max-height: 250px; overflow-y: auto; margin-bottom: 20px;">
                        <asp:Repeater ID="rptUnusedIndexes" runat="server">
                            <ItemTemplate>
                                <div style="background: rgba(248, 80, 50, 0.1); border-left: 3px solid #f85032; padding: 10px; border-radius: 8px; margin-bottom: 8px;">
                                    <strong style="color: #f85032;"><%# Eval("TableName") %></strong>
                                    <div style="color: #b0b0b0; font-size: 0.85rem; margin-top: 5px;">
                                        Index: <%# Eval("IndexName") %>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <!-- Eksik Index Önerileri -->
                    <h5 style="color: #ffa726; margin-bottom: 15px;">
                        <i class="fas fa-lightbulb me-2"></i>Eksik Index Önerileri
                    </h5>
                    <div style="max-height: 250px; overflow-y: auto;">
                        <asp:Repeater ID="rptMissingIndexes" runat="server">
                            <ItemTemplate>
                                <div class="index-recommendation">
                                    <strong><%# Eval("TableName") %></strong>
                                    <div style="color: #b0b0b0; font-size: 0.85rem; margin-top: 5px;">
                                        Kolon: <%# Eval("EqualityColumns") %>
                                        <span style="color: #808080; margin-left: 10px;">Etki: <%# Eval("ImprovementPercent") %>%</span>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>

        <!-- Yavaş Query'ler ve Backup Geçmişi -->
        <div class="row">
            <!-- Yavaş Query'ler -->
            <div class="col-lg-6">
                <div class="table-card">
                    <h3><i class="fas fa-hourglass-half me-2"></i>Yavaş Query'ler (>1 saniye)</h3>
                    <div style="max-height: 500px; overflow-y: auto;">
                        <asp:Repeater ID="rptSlowQueries" runat="server">
                            <ItemTemplate>
                                <div style="border-bottom: 1px solid #3a3a3a; padding: 15px 0;">
                                    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 10px;">
                                        <span class="badge-danger"><%# Eval("ExecutionTimeMs") %> ms</span>
                                        <span class="timestamp"><%# Eval("ExecutionCount") %> kez çalıştı</span>
                                    </div>
                                    <div class="query-text"><%# Eval("QueryText") %></div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>

            <!-- Backup Geçmişi -->
            <div class="col-lg-6">
                <div class="table-card">
                    <h3><i class="fas fa-history me-2"></i>Backup Geçmişi (Son 10)</h3>
                    <table class="custom-table">
                        <thead>
                            <tr>
                                <th>Tarih/Saat</th>
                                <th>Tür</th>
                                <th style="text-align: right;">Boyut</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptBackupHistory" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td class="timestamp">
                                            <%# ((DateTime)Eval("BackupDate")).ToString("dd.MM.yyyy HH:mm") %>
                                        </td>
                                        <td>
                                            <span class="badge-info"><%# Eval("BackupType") %></span>
                                        </td>
                                        <td style="text-align: right; color: #ffa726; font-weight: 600;">
                                            <%# Eval("BackupSizeMB") %> MB
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <!-- Hidden Fields for Chart Data -->
        <asp:HiddenField ID="hdnTableSizeData" runat="server" />

    </form>

    <!-- Bootstrap 5.3.8 JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    <!-- Chart.js 4.x -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.0/dist/chart.umd.min.js"></script>

    <script type="text/javascript">
        // Chart.js konfigürasyonu
        Chart.defaults.color = '#b0b0b0';
        Chart.defaults.borderColor = '#3a3a3a';

        // Tablo Boyutu Chart (Bar)
        var tableSizeData = JSON.parse(document.getElementById('<%= hdnTableSizeData.ClientID %>').value || '{}');

        if (tableSizeData.labels) {
            var ctxTableSize = document.getElementById('tableSizeChart').getContext('2d');
            new Chart(ctxTableSize, {
                type: 'bar',
                data: tableSizeData,
                options: {
                    responsive: true,
                    maintainAspectRatio: true,
                    indexAxis: 'y',
                    plugins: {
                        legend: {
                            display: false
                        }
                    },
                    scales: {
                        x: {
                            beginAtZero: true,
                            ticks: {
                                color: '#b0b0b0',
                                callback: function (value) {
                                    return value + ' MB';
                                }
                            },
                            grid: { color: '#3a3a3a' }
                        },
                        y: {
                            ticks: { color: '#b0b0b0' },
                            grid: { color: '#3a3a3a' }
                        }
                    }
                }
            });
        }
    </script>
</body>
</html>
