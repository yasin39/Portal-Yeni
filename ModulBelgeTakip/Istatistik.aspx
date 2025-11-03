<%@ Page Title="İstatistikler" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" 
    CodeBehind="Istatistik.aspx.cs" Inherits="Portal.ModulBelgeTakip.Istatistik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="~/wwwroot/css/BELGETAKIPMODUL.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js@3.9.1/dist/chart.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid wide-container">
        
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h3 class="text-primary-custom mb-0">
                <i class="fas fa-chart-bar me-2"></i>Belge Takip İstatistikleri
            </h3>
        </div>

        <div class="row mb-4">
            <div class="col-lg-3 col-md-6 mb-3">
                <div class="card border-0 shadow-custom-sm h-100">
                    <div class="card-body bg-gradient-primary">
                        <div class="d-flex justify-content-between align-items-center">
                            <div class="text-white">
                                <h6 class="mb-2 text-white-50 fw-normal">Toplam Firma</h6>
                                <h2 class="mb-0 fw-bold">
                                    <asp:Label ID="lblToplamFirma" runat="server" Text="0"></asp:Label>
                                </h2>
                            </div>
                            <div class="text-white icon-xl icon-faded">
                                <i class="fas fa-building"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-3 col-md-6 mb-3">
                <div class="card border-0 shadow-custom-sm h-100">
                    <div class="card-body bg-gradient-success">
                        <div class="d-flex justify-content-between align-items-center">
                            <div class="text-white">
                                <h6 class="mb-2 text-white-50 fw-normal">Belgeli Firma</h6>
                                <h2 class="mb-0 fw-bold">
                                    <asp:Label ID="lblBelgeliFirma" runat="server" Text="0"></asp:Label>
                                </h2>
                            </div>
                            <div class="text-white icon-xl icon-faded">
                                <i class="fas fa-certificate"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-3 col-md-6 mb-3">
                <div class="card border-0 shadow-custom-sm h-100">
                    <div class="card-body bg-gradient-warning">
                        <div class="d-flex justify-content-between align-items-center">
                            <div class="text-white">
                                <h6 class="mb-2 text-white-50 fw-normal">Belgesiz Firma</h6>
                                <h2 class="mb-0 fw-bold">
                                    <asp:Label ID="lblBelgesizFirma" runat="server" Text="0"></asp:Label>
                                </h2>
                            </div>
                            <div class="text-white icon-xl icon-faded">
                                <i class="fas fa-exclamation-triangle"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-3 col-md-6 mb-3">
                <div class="card border-0 shadow-custom-sm h-100">
                    <div class="card-body bg-gradient-purple">
                        <div class="d-flex justify-content-between align-items-center">
                            <div class="text-white">
                                <h6 class="mb-2 text-white-50 fw-normal">Toplam Denetim</h6>
                                <h2 class="mb-0 fw-bold">
                                    <asp:Label ID="lblToplamDenetim" runat="server" Text="0"></asp:Label>
                                </h2>
                            </div>
                            <div class="text-white icon-xl icon-faded">
                                <i class="fas fa-clipboard-check"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mb-4">
            <div class="col-lg-6 mb-4">
                <div class="card shadow-custom-sm">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fas fa-map-marker-alt me-2"></i>İllere Göre Dağılım</h5>
                    </div>
                    <div class="card-body">
                        <canvas id="ilGrafik" style="max-height: 400px;"></canvas>
                    </div>
                </div>
            </div>

            <div class="col-lg-6 mb-4">
                <div class="card shadow-custom-sm">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fas fa-file-certificate me-2"></i>Belge Türlerine Göre Dağılım</h5>
                    </div>
                    <div class="card-body">
                        <canvas id="belgeGrafik" style="max-height: 400px;"></canvas>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mb-4">
            <div class="col-12">
                <div class="card shadow-custom-sm">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fas fa-calendar-alt me-2"></i>Aylık Denetim Sayıları</h5>
                    </div>
                    <div class="card-body">
                        <canvas id="aylikGrafik" style="max-height: 400px;"></canvas>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdnIlData" runat="server" />
        <asp:HiddenField ID="hdnBelgeData" runat="server" />
        <asp:HiddenField ID="hdnAylikData" runat="server" />
    </div>

    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            const renkPaleti = {
                primary: '#4B7BEC',
                secondary: '#2E5B9A',
                success: '#10b981',
                warning: '#f59e0b',
                danger: '#ef4444',
                info: '#3b82f6',
                purple: '#8b5cf6',
                colors: [
                    'rgba(75, 123, 236, 0.7)',
                    'rgba(46, 91, 154, 0.7)',
                    'rgba(16, 185, 129, 0.7)',
                    'rgba(245, 158, 11, 0.7)',
                    'rgba(139, 92, 246, 0.7)',
                    'rgba(239, 68, 68, 0.7)',
                    'rgba(59, 130, 246, 0.7)'
                ]
            };

            try {
                var ilData = JSON.parse(document.getElementById('<%= hdnIlData.ClientID %>').value);
                new Chart(document.getElementById('ilGrafik'), {
                    type: 'bar',
                    data: {
                        labels: ilData.map(x => x.Il),
                        datasets: [{
                            label: 'Firma Sayısı',
                            data: ilData.map(x => x.Sayi),
                            backgroundColor: renkPaleti.primary,
                            borderColor: renkPaleti.secondary,
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            legend: { display: false }
                        },
                        scales: {
                            y: {
                                beginAtZero: true,
                                ticks: { precision: 0 }
                            }
                        }
                    }
                });
            } catch (e) {
                console.error('İl grafiği yüklenemedi:', e);
            }

            try {
                var belgeData = JSON.parse(document.getElementById('<%= hdnBelgeData.ClientID %>').value);
                new Chart(document.getElementById('belgeGrafik'), {
                    type: 'bar',
                    data: {
                        labels: belgeData.map(x => x.BelgeTuru),
                        datasets: [
                            {
                                label: 'Belgeli',
                                data: belgeData.map(x => x.Belgeli),
                                backgroundColor: 'rgba(16, 185, 129, 0.7)',
                                borderColor: '#10b981',
                                borderWidth: 1
                            },
                            {
                                label: 'Belgesiz',
                                data: belgeData.map(x => x.Belgesiz),
                                backgroundColor: 'rgba(239, 68, 68, 0.7)',
                                borderColor: '#ef4444',
                                borderWidth: 1
                            }
                        ]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        scales: {
                            y: {
                                beginAtZero: true,
                                stacked: true,
                                ticks: { precision: 0 }
                            },
                            x: { stacked: true }
                        }
                    }
                });
            } catch (e) {
                console.error('Belge grafiği yüklenemedi:', e);
            }

            try {
                var aylikData = JSON.parse(document.getElementById('<%= hdnAylikData.ClientID %>').value);
                const tekilAylar = [...new Set(aylikData.map(x => x.Ay))].sort();
                const belgeTurleri = [...new Set(aylikData.map(x => x.BelgeTuru))];

                const datasets = belgeTurleri.map((tur, index) => ({
                    label: tur,
                    data: tekilAylar.map(ay => {
                        const kayit = aylikData.find(x => x.Ay === ay && x.BelgeTuru === tur);
                        return kayit ? kayit.ToplamDenetim : 0;
                    }),
                    borderColor: renkPaleti.colors[index % renkPaleti.colors.length],
                    backgroundColor: renkPaleti.colors[index % renkPaleti.colors.length],
                    borderWidth: 2,
                    tension: 0.3,
                    fill: false
                }));

                new Chart(document.getElementById('aylikGrafik'), {
                    type: 'line',
                    data: {
                        labels: tekilAylar,
                        datasets: datasets
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            legend: {
                                position: 'top'
                            }
                        },
                        scales: {
                            y: {
                                beginAtZero: true,
                                ticks: { precision: 0 }
                            }
                        }
                    }
                });
            } catch (e) {
                console.error('Aylık grafik yüklenemedi:', e);
            }
        });
    </script>
</asp:Content>