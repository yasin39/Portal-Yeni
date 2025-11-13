<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Portal.Dashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="tr">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Dashboard - Portal Yönetim</title>
    
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

        .stat-card-icon.danger {
            background: linear-gradient(135deg, #f85032 0%, #e73827 100%);
            color: white;
        }

        .stat-card-icon.warning {
            background: linear-gradient(135deg, #ffa726 0%, #fb8c00 100%);
            color: white;
        }

        .stat-card-icon.success {
            background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
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

        .badge-rank {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 5px 12px;
            border-radius: 20px;
            font-weight: 600;
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
        }

        .error-message {
            color: #ef5350;
            font-size: 0.9rem;
            background: rgba(239, 83, 80, 0.1);
            padding: 8px 12px;
            border-radius: 8px;
            border-left: 3px solid #ef5350;
        }

        .timestamp {
            color: #808080;
            font-size: 0.85rem;
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
                    <h1><i class="fas fa-chart-line me-3"></i>Dashboard</h1>
                    <p><i class="far fa-clock me-2"></i>Son Güncelleme: <%= DateTime.Now.ToString("dd.MM.yyyy HH:mm") %></p>
                </div>
                <asp:Button ID="btnRefresh" runat="server" Text="🔄 Yenile" CssClass="btn-refresh" OnClick="btnRefresh_Click" />
            </div>
        </div>

        <!-- Stat Cards -->
        <div class="row">
            <!-- Toplam Log -->
            <div class="col-lg-3 col-md-6">
                <div class="stat-card">
                    <div class="stat-card-icon primary">
                        <i class="fas fa-database"></i>
                    </div>
                    <div class="stat-card-title">Toplam Log (Bugün)</div>
                    <div class="stat-card-value">
                        <asp:Label ID="lblTotalCount" runat="server" Text="0"></asp:Label>
                    </div>
                </div>
            </div>

            <!-- Hata Sayısı -->
            <div class="col-lg-3 col-md-6">
                <div class="stat-card">
                    <div class="stat-card-icon danger">
                        <i class="fas fa-exclamation-circle"></i>
                    </div>
                    <div class="stat-card-title">Hatalar</div>
                    <div class="stat-card-value">
                        <asp:Label ID="lblErrorCount" runat="server" Text="0"></asp:Label>
                    </div>
                </div>
            </div>

            <!-- Uyarı Sayısı -->
            <div class="col-lg-3 col-md-6">
                <div class="stat-card">
                    <div class="stat-card-icon warning">
                        <i class="fas fa-exclamation-triangle"></i>
                    </div>
                    <div class="stat-card-title">Uyarılar</div>
                    <div class="stat-card-value">
                        <asp:Label ID="lblWarningCount" runat="server" Text="0"></asp:Label>
                    </div>
                </div>
            </div>

            <!-- Başarı Oranı -->
            <div class="col-lg-3 col-md-6">
                <div class="stat-card">
                    <div class="stat-card-icon success">
                        <i class="fas fa-check-circle"></i>
                    </div>
                    <div class="stat-card-title">Başarı Oranı</div>
                    <div class="stat-card-value">
                        <asp:Label ID="lblSuccessRate" runat="server" Text="0%"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <!-- Charts -->
        <div class="row">
            <!-- Line Chart - Haftalık Trend -->
            <div class="col-lg-8">
                <div class="chart-card">
                    <h3><i class="fas fa-chart-line me-2"></i>Son 7 Günlük Trend</h3>
                    <canvas id="weeklyTrendChart" height="80"></canvas>
                </div>
            </div>

            <!-- Doughnut Chart - Log Dağılımı -->
            <div class="col-lg-4">
                <div class="chart-card">
                    <h3><i class="fas fa-chart-pie me-2"></i>Log Dağılımı</h3>
                    <canvas id="logDistributionChart"></canvas>
                </div>
            </div>
        </div>

        <!-- Tables -->
        <div class="row">
            <!-- Top Error Modules -->
            <div class="col-lg-6">
                <div class="table-card">
                    <h3><i class="fas fa-fire me-2"></i>En Çok Hata Veren Modüller (Top 5)</h3>
                    <table class="custom-table">
                        <thead>
                            <tr>
                                <th style="width: 60px;">Sıra</th>
                                <th>Modül Adı</th>
                                <th style="width: 100px; text-align: right;">Hata Sayısı</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptTopModules" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <span class="badge-rank"><%# Container.ItemIndex + 1 %></span>
                                        </td>
                                        <td><%# Eval("ModuleName") %></td>
                                        <td style="text-align: right; font-weight: 600; color: #ef5350;">
                                            <%# Eval("ErrorCount") %>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>

            <!-- Recent Errors -->
            <div class="col-lg-6">
                <div class="table-card">
                    <h3><i class="fas fa-history me-2"></i>Son 10 Hata Kaydı</h3>
                    <div style="max-height: 400px; overflow-y: auto;">
                        <asp:Repeater ID="rptRecentErrors" runat="server">
                            <ItemTemplate>
                                <div style="border-bottom: 1px solid #3a3a3a; padding: 15px 0;">
                                    <div class="d-flex justify-content-between align-items-start mb-2">
                                        <strong style="color: #fff;"><%# Eval("Module") %></strong>
                                        <span class="timestamp"><%# ((DateTime)Eval("Timestamp")).ToString("dd.MM.yyyy HH:mm") %></span>
                                    </div>
                                    <div class="error-message">
                                        <%# Eval("ShortMessage") %>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>

        <!-- Hidden Fields for Chart Data -->
        <asp:HiddenField ID="hdnWeeklyTrendData" runat="server" />
        <asp:HiddenField ID="hdnLogDistributionData" runat="server" />

    </form>

    <!-- Bootstrap 5.3.8 JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    
    <!-- Chart.js 4.x -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.0/dist/chart.umd.min.js"></script>

    <script type="text/javascript">
        // Chart.js konfigürasyonu
        Chart.defaults.color = '#b0b0b0';
        Chart.defaults.borderColor = '#3a3a3a';

        // Haftalık Trend Chart (Line)
        var weeklyTrendData = JSON.parse(document.getElementById('<%= hdnWeeklyTrendData.ClientID %>').value || '{}');
        
        if (weeklyTrendData.labels) {
            var ctxWeekly = document.getElementById('weeklyTrendChart').getContext('2d');
            new Chart(ctxWeekly, {
                type: 'line',
                data: weeklyTrendData,
                options: {
                    responsive: true,
                    maintainAspectRatio: true,
                    plugins: {
                        legend: {
                            position: 'top',
                            labels: {
                                color: '#e0e0e0',
                                font: { size: 12 },
                                padding: 15
                            }
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            ticks: { color: '#b0b0b0' },
                            grid: { color: '#3a3a3a' }
                        },
                        x: {
                            ticks: { color: '#b0b0b0' },
                            grid: { color: '#3a3a3a' }
                        }
                    }
                }
            });
        }

        // Log Dağılımı Chart (Doughnut)
        var logDistributionData = JSON.parse(document.getElementById('<%= hdnLogDistributionData.ClientID %>').value || '{}');

        if (logDistributionData.labels) {
            var ctxDistribution = document.getElementById('logDistributionChart').getContext('2d');
            new Chart(ctxDistribution, {
                type: 'doughnut',
                data: logDistributionData,
                options: {
                    responsive: true,
                    maintainAspectRatio: true,
                    plugins: {
                        legend: {
                            position: 'bottom',
                            labels: {
                                color: '#e0e0e0',
                                font: { size: 12 },
                                padding: 15
                            }
                        }
                    }
                }
            });
        }
    </script>
</body>
</html>
