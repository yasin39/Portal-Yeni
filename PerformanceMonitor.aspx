<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerformanceMonitor.aspx.cs" Inherits="Portal.PerformanceMonitor" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="tr">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Performans İzleme - Portal Yönetim</title>

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

        .filter-card {
            background: linear-gradient(135deg, #2d2d2d 0%, #1f1f1f 100%);
            border-radius: 15px;
            padding: 25px;
            margin-bottom: 20px;
            box-shadow: 0 8px 25px rgba(0, 0, 0, 0.4);
            border: 1px solid #3a3a3a;
        }

            .filter-card h5 {
                color: #ffffff;
                font-size: 1.1rem;
                margin-bottom: 20px;
                font-weight: 600;
            }

        .form-control, .form-select {
            background: #1a1a1a;
            border: 1px solid #3a3a3a;
            color: #e0e0e0;
            border-radius: 8px;
            padding: 10px 15px;
        }

            .form-control:focus, .form-select:focus {
                background: #252525;
                border-color: #667eea;
                box-shadow: 0 0 0 0.2rem rgba(102, 126, 234, 0.25);
                color: #e0e0e0;
            }

            .form-control::placeholder {
                color: #808080;
            }

        .form-label {
            color: #b0b0b0;
            font-size: 0.9rem;
            margin-bottom: 8px;
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

        .badge-fast {
            background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
            color: white;
            padding: 5px 12px;
            border-radius: 20px;
            font-weight: 600;
            font-size: 0.85rem;
        }

        .badge-medium {
            background: linear-gradient(135deg, #ffa726 0%, #fb8c00 100%);
            color: white;
            padding: 5px 12px;
            border-radius: 20px;
            font-weight: 600;
            font-size: 0.85rem;
        }

        .badge-slow {
            background: linear-gradient(135deg, #f85032 0%, #e73827 100%);
            color: white;
            padding: 5px 12px;
            border-radius: 20px;
            font-weight: 600;
            font-size: 0.85rem;
        }

        .btn-action {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border: none;
            color: white;
            padding: 10px 25px;
            border-radius: 10px;
            font-weight: 600;
            transition: all 0.3s ease;
            box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
        }

            .btn-action:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 20px rgba(102, 126, 234, 0.6);
                background: linear-gradient(135deg, #764ba2 0%, #667eea 100%);
                color: white;
            }

        .btn-secondary-action {
            background: linear-gradient(135deg, #3a3a3a 0%, #2d2d2d 100%);
            border: 1px solid #4a4a4a;
            color: #e0e0e0;
            padding: 10px 25px;
            border-radius: 10px;
            font-weight: 600;
            transition: all 0.3s ease;
        }

            .btn-secondary-action:hover {
                background: linear-gradient(135deg, #4a4a4a 0%, #3a3a3a 100%);
                border-color: #5a5a5a;
                color: #ffffff;
                transform: translateY(-2px);
            }

        .timestamp {
            color: #808080;
            font-size: 0.85rem;
        }

        .page-name {
            color: #667eea;
            font-weight: 600;
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
                    <h1><i class="fas fa-tachometer-alt me-3"></i>Performans İzleme</h1>
                    <p><i class="far fa-clock me-2"></i>Son Güncelleme: <%= DateTime.Now.ToString("dd.MM.yyyy HH:mm") %></p>
                </div>
                <asp:Button ID="btnRefresh" runat="server" Text="🔄 Yenile" CssClass="btn-action" OnClick="btnRefresh_Click" />
            </div>
        </div>

        <!-- Stat Cards -->
        <div class="row">
            <!-- Ortalama Yükleme Süresi -->
            <div class="col-lg-3 col-md-6">
                <div class="stat-card">
                    <div class="stat-card-icon primary">
                        <i class="fas fa-clock"></i>
                    </div>
                    <div class="stat-card-title">Ortalama Yükleme</div>
                    <div class="stat-card-value">
                        <asp:Label ID="lblAvgLoadTime" runat="server" Text="0"></asp:Label>
                        <span style="font-size: 1.2rem; color: #b0b0b0;">ms</span>
                    </div>
                    <div class="stat-card-subtitle">Son 24 saat</div>
                </div>
            </div>

            <!-- En Yavaş Sayfa -->
            <div class="col-lg-3 col-md-6">
                <div class="stat-card">
                    <div class="stat-card-icon danger">
                        <i class="fas fa-exclamation-triangle"></i>
                    </div>
                    <div class="stat-card-title">En Yavaş Sayfa</div>
                    <div class="stat-card-value" style="font-size: 1.3rem;">
                        <asp:Label ID="lblSlowestPage" runat="server" Text="-"></asp:Label>
                    </div>
                    <div class="stat-card-subtitle">
                        <asp:Label ID="lblSlowestPageTime" runat="server" Text="0 ms"></asp:Label>
                    </div>
                </div>
            </div>

            <!-- Toplam SQL Query -->
            <div class="col-lg-3 col-md-6">
                <div class="stat-card">
                    <div class="stat-card-icon warning">
                        <i class="fas fa-database"></i>
                    </div>
                    <div class="stat-card-title">Toplam SQL Query</div>
                    <div class="stat-card-value">
                        <asp:Label ID="lblTotalQueries" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stat-card-subtitle">Bugün</div>
                </div>
            </div>

            <!-- Ortalama Hafıza -->
            <div class="col-lg-3 col-md-6">
                <div class="stat-card">
                    <div class="stat-card-icon success">
                        <i class="fas fa-memory"></i>
                    </div>
                    <div class="stat-card-title">Ortalama Hafıza</div>
                    <div class="stat-card-value">
                        <asp:Label ID="lblAvgMemory" runat="server" Text="0"></asp:Label>
                        <span style="font-size: 1.2rem; color: #b0b0b0;">MB</span>
                    </div>
                    <div class="stat-card-subtitle">Son 24 saat</div>
                </div>
            </div>
        </div>

        <!-- Filters -->
        <div class="filter-card">
            <h5><i class="fas fa-filter me-2"></i>Filtreler</h5>
            <div class="row g-3">
                <div class="col-md-3">
                    <label class="form-label">Başlangıç Tarihi</label>
                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Bitiş Tarihi</label>
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Sayfa</label>
                    <asp:DropDownList ID="ddlPage" runat="server" CssClass="form-select">
                        <asp:ListItem Value="" Text="Tümü"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Kullanıcı</label>
                    <asp:TextBox ID="txtUser" runat="server" CssClass="form-control" placeholder="Sicil veya ad..."></asp:TextBox>
                </div>
            </div>
            <div class="row mt-3">
                <div class="col-12">
                    <asp:Button ID="btnFilter" runat="server" Text="🔍 Filtrele" CssClass="btn-action me-2" OnClick="btnFilter_Click" />
                    <asp:Button ID="btnClearFilter" runat="server" Text="🧹 Temizle" CssClass="btn-secondary-action" OnClick="btnClearFilter_Click" />
                </div>
            </div>
        </div>

        <!-- Charts -->
        <div class="row">
            <!-- Line Chart - Haftalık Trend -->
            <div class="col-lg-8">
                <div class="chart-card">
                    <h3><i class="fas fa-chart-line me-2"></i>Son 7 Günlük Performans Trendi</h3>
                    <canvas id="weeklyTrendChart" height="80"></canvas>
                </div>
            </div>

            <!-- Bar Chart - Sayfa Karşılaştırması -->
            <div class="col-lg-4">
                <div class="chart-card">
                    <h3><i class="fas fa-chart-bar me-2"></i>Sayfalara Göre Ort. Süre</h3>
                    <canvas id="pageComparisonChart"></canvas>
                </div>
            </div>
        </div>

        <!-- Tables -->
        <div class="row">
            <!-- En Yavaş 10 Sayfa -->
            <div class="col-lg-6">
                <div class="table-card">
                    <h3><i class="fas fa-stopwatch me-2"></i>En Yavaş 10 Sayfa</h3>
                    <table class="custom-table">
                        <thead>
                            <tr>
                                <th style="width: 60px;">#</th>
                                <th>Sayfa</th>
                                <th style="width: 120px; text-align: right;">Ort. Süre</th>
                                <th style="width: 100px; text-align: center;">Durum</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptSlowestPages" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td style="color: #808080;"><%# Container.ItemIndex + 1 %></td>
                                        <td class="page-name"><%# Eval("PageName") %></td>
                                        <td style="text-align: right; font-weight: 600;">
                                            <%# Eval("AvgLoadTime") %> ms
                                        </td>
                                        <td style="text-align: center;">
                                            <%# 
                                        (int)Eval("AvgLoadTime") < 100 
                                         ? "<span class='badge-fast'>Hızlı</span>" 
                                         : (int)Eval("AvgLoadTime") < 500 
                                         ? "<span class='badge-medium'>Orta</span>" 
                                         : "<span class='badge-slow'>Yavaş</span>" 
    %>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>

            <!-- Son 20 Performans Kaydı -->
            <div class="col-lg-6">
                <div class="table-card">
                    <h3><i class="fas fa-history me-2"></i>Son 20 Performans Kaydı</h3>
                    <div style="max-height: 500px; overflow-y: auto;">
                        <table class="custom-table">
                            <thead>
                                <tr>
                                    <th>Tarih/Saat</th>
                                    <th>Sayfa</th>
                                    <th style="text-align: right;">Süre</th>
                                    <th style="text-align: right;">Query</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptRecentLogs" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="timestamp">
                                                <%# ((DateTime)Eval("CreatedDate")).ToString("dd.MM HH:mm") %>
                                            </td>
                                            <td>
                                                <div class="page-name"><%# Eval("PageName") %></div>
                                                <small style="color: #808080;"><%# Eval("UserName") %></small>
                                            </td>
                                            <td style="text-align: right; font-weight: 600;">
                                                <%# Eval("LoadTimeMs") %> ms
                                            </td>
                                            <td style="text-align: right; color: #ffa726;">
                                                <%# Eval("SqlQueryCount") %>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <!-- Hidden Fields for Chart Data -->
        <asp:HiddenField ID="hdnWeeklyTrendData" runat="server" />
        <asp:HiddenField ID="hdnPageComparisonData" runat="server" />

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
                            ticks: {
                                color: '#b0b0b0',
                                callback: function (value) {
                                    return value + ' ms';
                                }
                            },
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

        // Sayfa Karşılaştırması Chart (Bar)
        var pageComparisonData = JSON.parse(document.getElementById('<%= hdnPageComparisonData.ClientID %>').value || '{}');

        if (pageComparisonData.labels) {
            var ctxComparison = document.getElementById('pageComparisonChart').getContext('2d');
            new Chart(ctxComparison, {
                type: 'bar',
                data: pageComparisonData,
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
                                    return value + ' ms';
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
