<%@ Page Title="Görev Talep Rapor" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true"
    CodeBehind="TalepRapor.aspx.cs" Inherits="Portal.ModulGorev.TalepRapor" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/BELGETAKIPMODUL.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.0/dist/chart.umd.min.js"></script>
    <style>
        /* Sadece bu sayfaya özel stiller - Ortak stiller Common-Components.css'e taşındı */

        .update-panel {
            background: linear-gradient(135deg, #fff9e6 0%, #fff3cd 100%);
            border-left: 4px solid #ffc107;
            border-radius: 8px;
            padding: 1.5rem;
            margin-top: 1rem;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">

        <!-- Başlık -->
        <div class="row mb-4">
            <div class="col-12">
                <h3 class="text-primary-custom mb-0">
                    <i class="fas fa-chart-bar me-2"></i>Görev Talep Rapor
                </h3>
                <p class="text-muted mb-0">Görev taleplerinizi görüntüleyin ve yönetin</p>
            </div>
        </div>

        <!-- İstatistik Kartları -->
        <div class="row mb-4">
            <div class="col-md-4">
                <div class="stat-card total">
                    <div class="d-flex align-items-center">
                        <i class="fas fa-tasks fa-3x text-primary-custom me-3"></i>
                        <div>
                            <p class="stat-number text-primary-custom">
                                <asp:Label ID="lblToplamGorev" runat="server" Text="0"></asp:Label>
                            </p>
                            <p class="stat-label mb-0">Toplam Görev</p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="stat-card active">
                    <div class="d-flex align-items-center">
                        <i class="fas fa-check-circle fa-3x text-success me-3"></i>
                        <div>
                            <p class="stat-number text-success">
                                <asp:Label ID="lblAktifGorev" runat="server" Text="0"></asp:Label>
                            </p>
                            <p class="stat-label mb-0">Aktif Görev</p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="stat-card passive">
                    <div class="stat-card passive">
                        <div class="d-flex align-items-center">
                            <i class="fas fa-archive fa-3x text-secondary me-3"></i>
                            <div>
                                <p class="stat-number text-secondary">
                                    <asp:Label ID="lblPasifGorev" runat="server" Text="0"></asp:Label>
                                </p>
                                <p class="stat-label mb-0">Tamamlanan Görev</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Filtre Bölümü -->
        <div class="card mb-4">
            <div class="card-header">
                <h5 class="card-title mb-0">
                    <i class="fas fa-filter me-2"></i>Filtreleme Seçenekleri
                </h5>
            </div>
            <div class="card-body">
                <div class="row g-3">
                    <div class="col-md-4">
                        <label class="form-label">İl Seçiniz</label>
                        <asp:DropDownList ID="ddlIl" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <label class="form-label">Durum</label>
                        <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                            <asp:ListItem Value="Aktif">Aktif</asp:ListItem>
                            <asp:ListItem Value="Pasif">Pasif</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4 d-flex align-items-end">
                        <asp:Button ID="btnAra" runat="server" Text="🔍 Ara" CssClass="btn btn-primary me-2"
                            OnClick="btnAra_Click" />
                        <asp:Button ID="btnTumunuListele" runat="server" Text="📜 Tümünü Listele"
                            CssClass="btn btn-outline-secondary" OnClick="btnTumunuListele_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Görev Listesi -->
        <div class="card mb-4">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h5 class="card-title mb-0">
                    <i class="fas fa-list me-2"></i>Görev Listesi
                </h5>
                <asp:Button ID="btnExcelAktar" runat="server" Text="📊 Excel'e Aktar"
                    CssClass="btn btn-success btn-sm" OnClick="btnExcelAktar_Click" />
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="GorevlerGrid" runat="server" CssClass="table table-striped table-hover"
                        AutoGenerateColumns="False" OnSelectedIndexChanged="GorevlerGrid_SelectedIndexChanged">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" SelectText="Seç" ButtonType="Button"
                                ControlStyle-CssClass="btn btn-sm btn-primary">
                                <ControlStyle CssClass="btn btn-sm btn-primary"></ControlStyle>
                            </asp:CommandField>
                            <asp:BoundField DataField="Talep_id" HeaderText="Talep ID" />
                            <asp:BoundField DataField="Gorev_Turu" HeaderText="Görev Türü" />
                            <asp:BoundField DataField="Gorev_il" HeaderText="İl" />
                            <asp:BoundField DataField="Gorev_ilce" HeaderText="İlçe" />
                            <asp:BoundField DataField="Personel_Sayisi" HeaderText="Personel Sayısı" />
                            <asp:BoundField DataField="sure" HeaderText="Süre" />
                            <asp:BoundField DataField="ivedilik" HeaderText="İvedilik" />
                            <asp:BoundField DataField="Gidilecen_Son_Tarih" HeaderText="Son Tarih" DataFormatString="{0:dd.MM.yyyy}" />
                            <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" />
                            <asp:BoundField DataField="Durum" HeaderText="Durum" />
                            <asp:BoundField DataField="Kayit_Tarihi" HeaderText="Kayıt Tarihi" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                            <asp:BoundField DataField="Kullanici" HeaderText="Kullanıcı" />
                        </Columns>
                        <HeaderStyle CssClass="table-header" />
                        <PagerStyle CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Görev Güncelleme Paneli -->
        <asp:Panel ID="pnlGorevGuncelle" runat="server" Visible="false">
            <div class="update-panel">
                <h5 class="mb-3">
                    <i class="fas fa-edit me-2"></i>Görev Güncelleme
                </h5>
                <div class="row g-3">
                    <div class="col-md-12">
                        <label class="form-label"><strong>Talep ID:</strong></label>
                        <asp:Label ID="lblTalepId" runat="server" CssClass="form-control-plaintext fw-bold"></asp:Label>
                    </div>
                    <div class="col-md-12">
                        <label class="form-label"><strong>Açıklama:</strong></label>
                        <asp:Label ID="lblAciklama" runat="server" CssClass="form-control-plaintext"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Göreve Çıkış Tarihi</label>
                        <asp:TextBox ID="txtGoreveCikisTarihi" runat="server" CssClass="form-control"
                            TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Giden Personel</label>
                        <asp:TextBox ID="txtGidenPersonel" runat="server" CssClass="form-control"
                            placeholder="Personel adı"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Gidiş Türü</label>
                        <asp:DropDownList ID="ddlGidisTuru" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">Seçiniz</asp:ListItem>
                            <asp:ListItem Value="Uçak">Uçak</asp:ListItem>
                            <asp:ListItem Value="Otobüs">Otobüs</asp:ListItem>
                            <asp:ListItem Value="Tren">Tren</asp:ListItem>
                            <asp:ListItem Value="Araç">Araç</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Görev Süresi (Gün)</label>
                        <asp:TextBox ID="txtGorevSuresi" runat="server" CssClass="form-control"
                            placeholder="Gün sayısı" TextMode="Number"></asp:TextBox>
                    </div>
                    <div class="col-md-12">
                        <asp:Button ID="btnGuncelle" runat="server" Text="✔️ Güncelle"
                            CssClass="btn btn-success me-2" OnClick="btnGuncelle_Click" />
                        <asp:Button ID="btnVazgec" runat="server" Text="❌ Vazgeç"
                            CssClass="btn btn-secondary" OnClick="btnVazgec_Click" />
                    </div>
                </div>
            </div>
        </asp:Panel>
        <!-- Hidden field for chart data -->
        <asp:HiddenField ID="hfGrafikVerisi" runat="server" />
        <!-- Grafik Bölümü -->
        <div class="row mt-4">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h5 class="card-title mb-0">
                            <i class="fas fa-chart-pie me-2"></i>İllere Göre Görev Dağılımı
                        </h5>
                    </div>
                    <div class="card-body">
                        <div class="chart-container">
                            <canvas id="gorevGrafik"></canvas>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>


    <!-- Chart.js Initialization -->
    <script type="text/javascript">
        var chartDataJSON = document.getElementById('<%= hfGrafikVerisi.ClientID %>').value;

        if (chartDataJSON) {
            var chartData = JSON.parse(chartDataJSON);

            if (chartData && chartData.labels && chartData.values) {
                var ctx = document.getElementById('gorevGrafik').getContext('2d');
                var myChart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: chartData.labels,
                        datasets: [{
                            label: 'Görev Sayısı',
                            data: chartData.values,
                            backgroundColor: 'rgba(75, 123, 236, 0.7)',
                            borderColor: 'rgba(46, 91, 154, 1)',
                            borderWidth: 2,
                            borderRadius: 6
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            legend: {
                                display: true,
                                position: 'top',
                                labels: {
                                    font: { size: 14, weight: 'bold' },
                                    color: '#2C3E50'
                                }
                            },
                            tooltip: {
                                backgroundColor: 'rgba(46, 91, 154, 0.9)',
                                titleFont: { size: 14 },
                                bodyFont: { size: 13 },
                                padding: 12,
                                cornerRadius: 6
                            }
                        },
                        scales: {
                            y: {
                                beginAtZero: true,
                                ticks: {
                                    stepSize: 1,
                                    font: { size: 12 },
                                    color: '#6b7280'
                                },
                                grid: { color: 'rgba(0, 0, 0, 0.05)' }
                            },
                            x: {
                                ticks: {
                                    font: { size: 12 },
                                    color: '#6b7280'
                                },
                                grid: { display: false }
                            }
                        }
                    }
                });
            }
        }
    </script>
</asp:Content>
