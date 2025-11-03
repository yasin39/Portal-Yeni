<%@ Page Title="Personel Dağılım Tablosu" Language="C#" MasterPageFile="~/AnaV2.Master" 
    AutoEventWireup="true" CodeBehind="Raporlama.aspx.cs" 
    Inherits="ModulPersonel.PersonelRaporla" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../PERSONELMODUL.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.0/dist/chart.umd.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        
        <!-- Stat Cards -->
        <div class="row mb-4">
            <div class="col-xl-2 col-md-4 col-sm-6 mb-3">
                <div class="card border-0 shadow-sm bg-gradient-primary">
                    <div class="card-body text-white text-center">
                        <i class="fas fa-user-tie fa-2x mb-2"></i>
                        <h6 class="mb-2 fs-sm">Kadrolu Aktif Çalışan</h6>
                        <h2 class="mb-0 fw-bold">
                            <asp:Label ID="lblKadroluAktif" runat="server" Text="0"></asp:Label>
                        </h2>
                    </div>
                </div>
            </div>

            <div class="col-xl-2 col-md-4 col-sm-6 mb-3">
                <div class="card border-0 shadow-sm bg-gradient-success">
                    <div class="card-body text-white text-center">
                        <i class="fas fa-user-check fa-2x mb-2"></i>
                        <h6 class="mb-2 fs-sm">Geçici Görevli Aktif</h6>
                        <h2 class="mb-0 fw-bold">
                            <asp:Label ID="lblGeciciAktif" runat="server" Text="0"></asp:Label>
                        </h2>
                    </div>
                </div>
            </div>

            <div class="col-xl-2 col-md-4 col-sm-6 mb-3">
                <div class="card border-0 shadow-sm bg-gradient-gray">
                    <div class="card-body text-white text-center">
                        <i class="fas fa-user-friends fa-2x mb-2"></i>
                        <h6 class="mb-2 fs-sm">Firma + TYP Personeli</h6>
                        <h2 class="mb-0 fw-bold">
                            <asp:Label ID="lblFirmaTYP" runat="server" Text="0"></asp:Label>
                        </h2>
                    </div>
                </div>
            </div>

            <div class="col-xl-2 col-md-4 col-sm-6 mb-3">
                <div class="card border-0 shadow-sm bg-gradient-warning">
                    <div class="card-body text-white text-center">
                        <i class="fas fa-users fa-2x mb-2"></i>
                        <h6 class="mb-2 fs-sm">Toplam Aktif Çalışan</h6>
                        <h2 class="mb-0 fw-bold">
                            <asp:Label ID="lblToplamAktif" runat="server" Text="0"></asp:Label>
                        </h2>
                    </div>
                </div>
            </div>

            <div class="col-xl-2 col-md-4 col-sm-6 mb-3">
                <div class="card border-0 shadow-sm bg-gradient-purple">
                    <div class="card-body text-white text-center">
                        <i class="fas fa-exchange-alt fa-2x mb-2"></i>
                        <h6 class="mb-2 fs-sm">Kadrolu Geçici Görevde</h6>
                        <h2 class="mb-0 fw-bold">
                            <asp:Label ID="lblKadroluGecici" runat="server" Text="0"></asp:Label>
                        </h2>
                    </div>
                </div>
            </div>

            <div class="col-xl-2 col-md-4 col-sm-6 mb-3">
                <div class="card border-0 shadow-sm bg-gradient-danger">
                    <div class="card-body text-white text-center">
                        <i class="fas fa-user-graduate fa-2x mb-2"></i>
                        <h6 class="mb-2 fs-sm">Toplam Personel Sayısı</h6>
                        <h2 class="mb-0 fw-bold">
                            <asp:Label ID="lblToplamPersonel" runat="server" Text="0"></asp:Label>
                        </h2>
                    </div>
                </div>
            </div>
        </div>

        <!-- Personel Dağılım Tablosu -->
        <div class="card shadow-sm mb-4">
            <div class="card-header">
                <h5 class="card-title mb-0">
                    <i class="fas fa-table me-2"></i>Personel Dağılım Tablosu
                </h5>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="PersonelDagılımGrid" runat="server" 
                        CssClass="table table-striped table-hover table-bordered" 
                        AutoGenerateColumns="False" 
                        ShowFooter="True"
                        GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="Unvan" HeaderText="Unvan">
                                <ItemStyle Font-Bold="True" Width="140px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Toplam_Kadrolu" HeaderText="Bölge Müdürlüğü Kadrolu (a)" NullDisplayText="0">
                                <ItemStyle Font-Bold="True" Width="90px" HorizontalAlign="Center" CssClass="table-warning" />
                                <FooterStyle HorizontalAlign="Center" CssClass="table-warning" />
                            </asp:BoundField>
                            <asp:BoundField DataField="KadroluGiden" HeaderText="Geçici Görevle Giden (b)" NullDisplayText="0">
                                <ItemStyle Width="90px" Font-Bold="True" HorizontalAlign="Center" CssClass="table-warning" />
                                <FooterStyle HorizontalAlign="Center" CssClass="table-warning" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Kadrolu" HeaderText="Kadrolu Aktif Çalışan (c=a-b)" NullDisplayText="0">
                                <ItemStyle Font-Bold="True" Width="90px" HorizontalAlign="Center" CssClass="table-warning" />
                                <FooterStyle HorizontalAlign="Center" CssClass="table-warning" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Gecici_Gelen" HeaderText="Geçici Görevle Gelen (d)" NullDisplayText="0">
                                <ItemStyle Width="90px" Font-Bold="True" HorizontalAlign="Center" />
                                <FooterStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Firma" HeaderText="Firma Personeli (f)" NullDisplayText="0">
                                <ItemStyle Width="90px" Font-Bold="True" HorizontalAlign="Center" />
                                <FooterStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TYP" HeaderText="İşkur İşçi (TYP) (h)" NullDisplayText="0">
                                <ItemStyle Width="90px" Font-Bold="True" HorizontalAlign="Center" />
                                <FooterStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Toplam_Aktif" HeaderText="Toplam Aktif (g=c+d+f+h)" NullDisplayText="0">
                                <ItemStyle Font-Bold="True" ForeColor="#dc2626" HorizontalAlign="Center" Width="90px" />
                                <FooterStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="#dc2626" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#4B7BEC" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#2E5B9A" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                </div>

                <div class="text-end mt-3">
                    <asp:Button ID="btnExcelExport" runat="server" 
                        CssClass="btn btn-success" 
                        Text="📊 Excel'e Aktar" 
                        OnClick="btnExcelExport_Click" />
                </div>
            </div>
        </div>

        <!-- Grafikler -->
        <div class="row">
            <!-- Kadro Dağılımı -->
            <div class="col-lg-6 mb-4">
                <div class="card shadow-sm">
                    <div class="card-header">
                        <h5 class="card-title mb-0">
                            <i class="fas fa-chart-bar me-2"></i>Personel Kadro Dağılımı
                        </h5>
                    </div>
                    <div class="card-body">
                        <canvas id="kadroChart" height="300"></canvas>
                    </div>
                </div>
            </div>

            <!-- Birim Dağılımı -->
            <div class="col-lg-6 mb-4">
                <div class="card shadow-sm">
                    <div class="card-header">
                        <h5 class="card-title mb-0">
                            <i class="fas fa-building me-2"></i>Birim Dağılımı
                        </h5>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="BirimDagilimGrid" runat="server" 
                                CssClass="table table-striped table-hover" 
                                AutoGenerateColumns="True"
                                GridLines="None">
                                <HeaderStyle BackColor="#2E5B9A" Font-Bold="True" ForeColor="White" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Sendika Dağılımı -->
            <div class="col-lg-6 mb-4">
                <div class="card shadow-sm">
                    <div class="card-header">
                        <h5 class="card-title mb-0">
                            <i class="fas fa-chart-pie me-2"></i>Sendika Dağılımı
                        </h5>
                    </div>
                    <div class="card-body">
                        <canvas id="sendikaChart" height="250"></canvas>
                    </div>
                </div>
            </div>

            <!-- Engel Durumu Dağılımı -->
            <div class="col-lg-6 mb-4">
                <div class="card shadow-sm">
                    <div class="card-header">
                        <h5 class="card-title mb-0">
                            <i class="fas fa-chart-pie me-2"></i>Engel Durumu Dağılımı
                        </h5>
                    </div>
                    <div class="card-body">
                        <canvas id="engelChart" height="250"></canvas>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <!-- Hidden Fields for Chart Data -->
    <asp:HiddenField ID="hdnKadroData" runat="server" />
    <asp:HiddenField ID="hdnSendikaData" runat="server" />
    <asp:HiddenField ID="hdnEngelData" runat="server" />

    <script>
        document.addEventListener('DOMContentLoaded', function() {
            // Kadro Chart
            var kadroData = JSON.parse(document.getElementById('<%= hdnKadroData.ClientID %>').value);
            var kadroCtx = document.getElementById('kadroChart').getContext('2d');
            new Chart(kadroCtx, {
                type: 'bar',
                data: {
                    labels: kadroData.labels,
                    datasets: [{
                        label: 'Personel Sayısı',
                        data: kadroData.values,
                        backgroundColor: 'rgba(75, 123, 236, 0.8)',
                        borderColor: 'rgba(46, 91, 154, 1)',
                        borderWidth: 2
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: false
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            ticks: {
                                stepSize: 1
                            }
                        }
                    }
                }
            });

            // Sendika Chart
            var sendikaData = JSON.parse(document.getElementById('<%= hdnSendikaData.ClientID %>').value);
            var sendikaCtx = document.getElementById('sendikaChart').getContext('2d');
            new Chart(sendikaCtx, {
                type: 'doughnut',
                data: {
                    labels: sendikaData.labels,
                    datasets: [{
                        data: sendikaData.values,
                        backgroundColor: [
                            'rgba(75, 123, 236, 0.8)',
                            'rgba(16, 185, 129, 0.8)',
                            'rgba(245, 158, 11, 0.8)',
                            'rgba(239, 68, 68, 0.8)',
                            'rgba(139, 92, 246, 0.8)',
                            'rgba(107, 114, 128, 0.8)'
                        ],
                        borderWidth: 2
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            position: 'bottom'
                        }
                    }
                }
            });

            // Engel Chart
            var engelData = JSON.parse(document.getElementById('<%= hdnEngelData.ClientID %>').value);
            var engelCtx = document.getElementById('engelChart').getContext('2d');
            new Chart(engelCtx, {
                type: 'doughnut',
                data: {
                    labels: engelData.labels,
                    datasets: [{
                        data: engelData.values,
                        backgroundColor: [
                            'rgba(46, 91, 154, 0.8)',
                            'rgba(75, 123, 236, 0.8)',
                            'rgba(107, 178, 252, 0.8)'
                        ],
                        borderWidth: 2
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            position: 'bottom'
                        }
                    }
                }
            });
        });
    </script>
</asp:Content>