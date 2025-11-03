<%@ Page Title="CİMER İstatistikleri" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="Istatistik.aspx.cs" Inherits="Portal.ModulCimer.Istatistik" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <style>        

        /* Cimer özel stat-card override */
        .stat-card {
            background: white;
            border-radius: 10px;
            padding: 1.5rem;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
            transition: all 0.3s ease;
            margin-bottom: 1.5rem;
        }

        .stat-card:hover {
            box-shadow: 0 4px 16px rgba(75, 123, 236, 0.15);
            transform: translateY(-2px);
        }

        .stat-title {
            color: #6b7280;
            font-size: 0.9rem;
            font-weight: 500;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <!--  Enhanced breadcrumb -->
    <li class="breadcrumb-item">
        <i class="fas fa-comments me-1"></i>CİMER
    </li>
    <li class="breadcrumb-item active" aria-current="page">İstatistikler</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <!--  Main Statistics Card -->
        <div class="row">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header">
                        <div>
                            <i class="fas fa-chart-bar"></i>
                            <span>CİMER Başvuru İstatistikleri</span>
                        </div>
                    </div>
                    <div class="card-body">
                        <!--  Info badge -->
                        <div class="info-badge">
                            <i class="fas fa-info-circle"></i>
                            CİMER başvurularının yıllık istatistiksel dağılımını görüntülemektesiniz.
                        </div>

                        <!--  Statistics Cards Grid -->
                        <div class="row mb-4">
                            <!--  Devam Eden Başvurular -->
                            <div class="col-lg-3 col-md-6">
                                <div class="card stat-card border-primary">
                                    <div class="stat-icon text-primary">
                                        <i class="fas fa-spinner"></i>
                                    </div>
                                    <div class="stat-number">
                                        <asp:Label ID="lblDevamEden" runat="server" Text="0"></asp:Label>
                                    </div>
                                    <div class="stat-title">Süreci Devam Eden</div>
                                </div>
                            </div>

                            <!--  Cevaplanan Başvurular -->
                            <div class="col-lg-3 col-md-6">
                                <div class="card stat-card border-success">
                                    <div class="stat-icon text-success">
                                        <i class="fas fa-check-circle"></i>
                                    </div>
                                    <div class="stat-number success">
                                        <asp:Label ID="lblCevaplanan" runat="server" Text="0"></asp:Label>
                                    </div>
                                    <div class="stat-title">Cevaplanan Başvurular</div>
                                </div>
                            </div>

                            <!--  2. Kez Cevaplanan -->
                            <div class="col-lg-3 col-md-6">
                                <div class="card stat-card border-warning">
                                    <div class="stat-icon text-warning">
                                        <i class="fas fa-redo"></i>
                                    </div>
                                    <div class="stat-number warning">
                                        <asp:Label ID="lblIkinciKez" runat="server" Text="0"></asp:Label>
                                    </div>
                                    <div class="stat-title">2. Kez Cevaplanan</div>
                                </div>
                            </div>

                            <!--  Toplam Başvurular -->
                            <div class="col-lg-3 col-md-6">
                                <div class="card stat-card border-info">
                                    <div class="stat-icon text-info">
                                        <i class="fas fa-chart-line"></i>
                                    </div>
                                    <div class="stat-number info">
                                        <asp:Label ID="lblToplam" runat="server" Text="0"></asp:Label>
                                    </div>
                                    <div class="stat-title">Yıllık Toplam Başvuru</div>
                                </div>
                            </div>
                        </div>

                        <!--  Year Selection Section -->
                        <div class="form-section">
                            <div class="section-title">
                                <i class="fas fa-calendar-alt"></i>
                                <span>Yıl Filtresi</span>
                            </div>

                            <div class="row">
                                <div class="col-md-4">
                                    <label for="<%= ddlYil.ClientID %>" class="form-label">Yıl Seçimi</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-calendar"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlYil" runat="server" CssClass="form-select" 
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlYil_SelectedIndexChanged">
                                            <%-- Years will be populated in code --%>
                                        </asp:DropDownList>
                                    </div>
                                    <small class="text-muted mt-1 d-block">
                                        <i class="fas fa-lightbulb me-1"></i>Yıl seçerek grafik verilerini güncelleyebilirsiniz
                                    </small>
                                </div>
                            </div>
                        </div>

                        <!--  Chart Section -->
                        <div class="form-section">
                            <div class="section-title">
                                <i class="fas fa-chart-column"></i>
                                <span>Firmalara Göre Şikayet Dağılımı</span>
                            </div>

                            <div class="chart-container">
                                <asp:Chart ID="chartSirketDagilim" runat="server" Width="1300" Height="500px" Palette="SeaGreen">
                                    <Series>
                                        <asp:Series Name="Series1" ChartType="Column" ChartArea="ChartArea1" 
                                                    IsValueShownAsLabel="true" LabelAngle="0" Font="Microsoft Sans Serif, 10pt">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1">
                                            <AxisX Title="Şikayet Edilen Firmalar" IsLabelAutoFit="false">
                                            </AxisX>
                                            <AxisY Title="Şikayet Sayısı">
                                            </AxisY>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                    <Titles>
                                        <asp:Title Text="Şirketlere Göre Şikayet Dağılımı" Font="Microsoft Sans Serif, 14pt, style=Bold" />
                                    </Titles>
                                </asp:Chart>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <%-- Hidden GridViews for data binding if needed --%>
    <asp:GridView ID="gvFirmaDetay" runat="server" Visible="false" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="Sikayet_Edilen_Firma" HeaderText="Firma Adı" />
            <asp:BoundField DataField="Sayi" HeaderText="Sayı" />
        </Columns>
    </asp:GridView>

    <!--  Additional Scripts for enhanced UX -->
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            // Animate stat cards on scroll
            const statCards = document.querySelectorAll('.stat-card');
            
            const observer = new IntersectionObserver((entries) => {
                entries.forEach((entry, index) => {
                    if (entry.isIntersecting) {
                        setTimeout(() => {
                            entry.target.style.opacity = '1';
                            entry.target.style.transform = 'translateY(0)';
                        }, index * 100);
                    }
                });
            }, { threshold: 0.1 });

            statCards.forEach(card => {
                card.style.opacity = '0';
                card.style.transform = 'translateY(20px)';
                card.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
                observer.observe(card);
            });

            // Auto-hide alerts after 5 seconds
            const alerts = document.querySelectorAll('.alert:not(.alert-danger)');
            alerts.forEach(alert => {
                setTimeout(() => {
                    const bsAlert = new bootstrap.Alert(alert);
                    bsAlert.close();
                }, 5000);
            });
        });
    </script>
</asp:Content>
