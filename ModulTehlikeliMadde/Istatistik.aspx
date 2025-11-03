<%@ Page Title="Tehlikeli Madde İstatistikleri" Language="C#" MasterPageFile="~/AnaV2.Master" 
    AutoEventWireup="true" CodeBehind="Istatistik.aspx.cs" 
    Inherits="Portal.ModulTehlikeliMadde.Istatistik" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.0/dist/chart.umd.min.js"></script>
    <style>
        .stat-card {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            color: white;
            border-radius: 12px;
            padding: 1.5rem;
            box-shadow: 0 4px 12px rgba(75, 123, 236, 0.2);
            margin-bottom: 1.5rem;
        }

        .stat-card h5 {
            font-weight: 600;
            margin-bottom: 1rem;
        }

        .chart-container {
            background: white;
            border-radius: 12px;
            padding: 2rem;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            margin-bottom: 1.5rem;
        }

        .table-container {
            background: white;
            border-radius: 12px;
            padding: 1.5rem;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            margin-bottom: 1.5rem;
        }

        .gridview-modern {
            width: 100%;
            border-collapse: collapse;
        }

        .gridview-modern th {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            color: white;
            padding: 0.75rem;
            text-align: center;
            font-weight: 600;
            border: none;
        }

        .gridview-modern td {
            padding: 0.75rem;
            text-align: center;
            border-bottom: 1px solid #e9ecef;
        }

        .gridview-modern tr:hover {
            background-color: #f8f9fa;
        }

        .gridview-modern tr:last-child td {
            font-weight: 600;
            background-color: #f0f4ff;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        
        <div class="row mb-3">
            <div class="col-12">
                <div class="stat-card">
                    <h5>
                        <i class="fas fa-chart-bar me-2"></i>Tehlikeli Madde Taşıma Belgesi İstatistikleri
                    </h5>
                    <div class="row align-items-center">
                        <div class="col-md-2">
                            <label class="form-label mb-0">İl Filtresi:</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlIl" runat="server" CssClass="form-select" 
                                AutoPostBack="True" OnSelectedIndexChanged="ddlIl_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-12">
                <div class="table-container">
                    <h6 class="mb-3" style="color: #2E5B9A; font-weight: 600;">
                        <i class="fas fa-table me-2"></i>Detaylı İstatistik Tablosu
                    </h6>
                    <asp:GridView ID="IstatistikGrid" runat="server" 
                        CssClass="gridview-modern" 
                        AutoGenerateColumns="True"
                        GridLines="None">
                    </asp:GridView>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-12">
                <div class="chart-container">
                    <h6 class="mb-3 text-center" style="color: #2E5B9A; font-weight: 600;">
                        <i class="fas fa-chart-pie me-2"></i>Faaliyet Türü Dağılımı
                    </h6>
                    <canvas id="grafikCanvas" style="max-height: 400px;"></canvas>
                </div>
            </div>
        </div>

    </div>

    <script>
        let grafikInstance = null;

        function CreateChart(etiketler, veriler) {
            const ctx = document.getElementById('grafikCanvas');

            if (grafikInstance) {
                grafikInstance.destroy();
            }

            const renkler = [
                'rgba(75, 123, 236, 0.8)',
                'rgba(46, 91, 154, 0.8)',
                'rgba(156, 136, 255, 0.8)',
                'rgba(52, 211, 153, 0.8)',
                'rgba(251, 146, 60, 0.8)',
                'rgba(248, 113, 113, 0.8)',
                'rgba(147, 197, 253, 0.8)',
                'rgba(167, 139, 250, 0.8)'
            ];

            grafikInstance = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: etiketler,
                    datasets: [{
                        label: 'Belge Sayısı',
                        data: veriler,
                        backgroundColor: renkler,
                        borderColor: renkler.map(c => c.replace('0.8', '1')),
                        borderWidth: 2,
                        borderRadius: 8
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: true,
                    plugins: {
                        legend: {
                            display: false
                        },
                        tooltip: {
                            backgroundColor: 'rgba(0, 0, 0, 0.8)',
                            padding: 12,
                            titleFont: { size: 14 },
                            bodyFont: { size: 13 }
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            ticks: {
                                stepSize: 1
                            }
                        },
                        x: {
                            ticks: {
                                maxRotation: 45,
                                minRotation: 45
                            }
                        }
                    }
                }
            });
        }
    </script>
</asp:Content>