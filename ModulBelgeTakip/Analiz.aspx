<%@ Page Title="Analiz ve İstatistik" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="Analiz.aspx.cs" Inherits="Portal.ModulBelgeTakip.Analiz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="~/wwwroot/css/BELGETAKIPMODUL.css" rel="stylesheet" />
    <style>
        .stat-card {
            background: linear-gradient(135deg, #ffffff 0%, #f8f9fa 100%);
            border-radius: 10px;
            padding: 1.5rem;
            text-align: center;
            border-left: 4px solid #4B7BEC;
            box-shadow: 0 2px 8px rgba(75, 123, 236, 0.08);
            transition: all 0.3s ease;
            height: 100%;
        }

        .stat-card:hover {
            transform: translateY(-3px);
            box-shadow: 0 4px 12px rgba(75, 123, 236, 0.15);
        }

        .stat-icon {
            font-size: 2.5rem;
            color: #4B7BEC;
            margin-bottom: 1rem;
        }

        .stat-value {
            font-size: 2rem;
            font-weight: 700;
            color: #2E5B9A;
            margin: 0.5rem 0;
        }

        .stat-label {
            font-size: 0.9rem;
            color: #6b7280;
            font-weight: 500;
        }

        .analysis-card {
            background: #ffffff;
            border-radius: 10px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
            margin-bottom: 1.5rem;
            overflow: hidden;
        }

        .analysis-card-header {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            color: white;
            padding: 1rem 1.5rem;
            font-weight: 600;
            font-size: 1.1rem;
        }

        .analysis-card-body {
            padding: 1.5rem;
        }

        .filter-card {
            background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
            border-radius: 10px;
            padding: 1.5rem;
            margin-bottom: 1.5rem;
            border-left: 4px solid #2E5B9A;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">
        <div class="row mb-4">
            <div class="col-12">
                <h3 class="text-primary-custom">
                    <i class="fas fa-chart-line me-2"></i>Analiz ve İstatistikler
                </h3>
            </div>
        </div>

        <div class="row mb-4">
            <div class="col-md-3">
                <div class="stat-card">
                    <div class="stat-icon">
                        <i class="fas fa-building"></i>
                    </div>
                    <div class="stat-value">
                        <asp:Label ID="lblToplamFirma" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stat-label">Toplam Firma</div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="stat-card">
                    <div class="stat-icon">
                        <i class="fas fa-check-circle text-success"></i>
                    </div>
                    <div class="stat-value text-success">
                        <asp:Label ID="lblTamamlanan" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stat-label">Belgeli Firma</div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="stat-card">
                    <div class="stat-icon">
                        <i class="fas fa-exclamation-circle text-danger"></i>
                    </div>
                    <div class="stat-value text-danger">
                        <asp:Label ID="lblBekleyen" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stat-label">Belgesiz Firma</div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="stat-card">
                    <div class="stat-icon">
                        <i class="fas fa-calendar-check"></i>
                    </div>
                    <div class="stat-value">
                        <asp:Label ID="lblAylikDenetim" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stat-label">Son Ay Denetim</div>
                </div>
            </div>
        </div>

        <div class="row mb-4">
            <div class="col-12">
                <div class="filter-card">
                    <div class="row align-items-end">
                        <div class="col-md-4">
                            <label class="form-label">
                                <i class="fas fa-calendar me-1"></i>Yıl Seçimi
                            </label>
                            <asp:DropDownList ID="ddlYil" runat="server" CssClass="form-select" 
                                AutoPostBack="true" OnSelectedIndexChanged="ddlYil_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">
                                <i class="fas fa-map-marker-alt me-1"></i>İl Seçimi
                            </label>
                            <asp:DropDownList ID="ddlIl" runat="server" CssClass="form-select" 
                                AutoPostBack="true" OnSelectedIndexChanged="ddlIl_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <asp:Button ID="btnFiltrele" runat="server" CssClass="btn btn-primary w-100" 
                                Text="🔍 Filtrele" OnClick="btnFiltrele_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mb-4">
            <div class="col-md-4">
                <div class="stat-card">
                    <div class="stat-icon">
                        <i class="fas fa-clipboard-check"></i>
                    </div>
                    <div class="stat-value">
                        <asp:Label ID="lblToplamDenetim" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stat-label">Denetim Yapılan Firma</div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="stat-card">
                    <div class="stat-icon">
                        <i class="fas fa-certificate text-success"></i>
                    </div>
                    <div class="stat-value text-success">
                        <asp:Label ID="lblBelgeliFirma" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stat-label">Belgeli Firma (Filtreli)</div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="stat-card">
                    <div class="stat-icon">
                        <i class="fas fa-ban text-danger"></i>
                    </div>
                    <div class="stat-value text-danger">
                        <asp:Label ID="lblBelgesizFirma" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stat-label">Belgesiz Firma (Filtreli)</div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6 mb-4">
                <div class="analysis-card">
                    <div class="analysis-card-header">
                        <i class="fas fa-certificate me-2"></i>Belge Türü Dağılımı
                    </div>
                    <div class="analysis-card-body">
                        <asp:GridView ID="BelgeDagilimGrid" runat="server" CssClass="table table-striped table-hover" 
                            AutoGenerateColumns="false" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="BELGE_AD" HeaderText="Belge Türü" />
                                <asp:BoundField DataField="Belgeli" HeaderText="Belgeli" />
                                <asp:BoundField DataField="Belgesiz" HeaderText="Belgesiz" />
                            </Columns>
                            <HeaderStyle CssClass="table-header" />
                            <RowStyle CssClass="table-row" />
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class="col-md-6 mb-4">
                <div class="analysis-card">
                    <div class="analysis-card-header">
                        <i class="fas fa-map-marked-alt me-2"></i>İl Bazında Dağılım (İlk 10)
                    </div>
                    <div class="analysis-card-body">
                        <asp:GridView ID="SehirDagilimGrid" runat="server" CssClass="table table-striped table-hover" 
                            AutoGenerateColumns="false" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="IL_AD" HeaderText="İl" />
                                <asp:BoundField DataField="FirmaSayisi" HeaderText="Firma Sayısı" />
                            </Columns>
                            <HeaderStyle CssClass="table-header" />
                            <RowStyle CssClass="table-row" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6 mb-4">
                <div class="analysis-card">
                    <div class="analysis-card-header">
                        <i class="fas fa-layer-group me-2"></i>Kategori Dağılımı
                    </div>
                    <div class="analysis-card-body">
                        <asp:GridView ID="KategoriDagilimGrid" runat="server" CssClass="table table-striped table-hover" 
                            AutoGenerateColumns="false" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="KATEGORI_AD" HeaderText="Kategori" />
                                <asp:BoundField DataField="ToplamFirma" HeaderText="Firma Sayısı" />
                                <asp:BoundField DataField="DenetimSayisi" HeaderText="Denetim Sayısı" />
                            </Columns>
                            <HeaderStyle CssClass="table-header" />
                            <RowStyle CssClass="table-row" />
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class="col-md-6 mb-4">
                <div class="analysis-card">
                    <div class="analysis-card-header">
                        <i class="fas fa-chart-bar me-2"></i>Aylık Denetim Trendleri (Son 6 Ay)
                    </div>
                    <div class="analysis-card-body">
                        <asp:GridView ID="AylikDenetimGrid" runat="server" CssClass="table table-striped table-hover" 
                            AutoGenerateColumns="false" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="Ay" HeaderText="Ay" />
                                <asp:BoundField DataField="DenetimSayisi" HeaderText="Denetim Sayısı" />
                            </Columns>
                            <HeaderStyle CssClass="table-header" />
                            <RowStyle CssClass="table-row" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-12 mb-4">
                <div class="analysis-card">
                    <div class="analysis-card-header">
                        <i class="fas fa-table me-2"></i>
                        <asp:Label ID="lblAylikDenetimBaslik" runat="server" Text="Aylık Denetim Analizi"></asp:Label>
                    </div>
                    <div class="analysis-card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="FirmaTipiAylikGrid" runat="server" CssClass="table table-striped table-hover table-sm" 
                                AutoGenerateColumns="false" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="DenetimTuru" HeaderText="Denetim Türü" />
                                    <asp:BoundField DataField="OCAK" HeaderText="Ocak" />
                                    <asp:BoundField DataField="SUBAT" HeaderText="Şubat" />
                                    <asp:BoundField DataField="MART" HeaderText="Mart" />
                                    <asp:BoundField DataField="NISAN" HeaderText="Nisan" />
                                    <asp:BoundField DataField="MAYIS" HeaderText="Mayıs" />
                                    <asp:BoundField DataField="HAZIRAN" HeaderText="Haziran" />
                                    <asp:BoundField DataField="TEMMUZ" HeaderText="Temmuz" />
                                    <asp:BoundField DataField="AGUSTOS" HeaderText="Ağustos" />
                                    <asp:BoundField DataField="EYLUL" HeaderText="Eylül" />
                                    <asp:BoundField DataField="EKIM" HeaderText="Ekim" />
                                    <asp:BoundField DataField="KASIM" HeaderText="Kasım" />
                                    <asp:BoundField DataField="ARALIK" HeaderText="Aralık" />
                                    <asp:BoundField DataField="YILLIKTOPLAM" HeaderText="Yıllık Toplam" />
                                </Columns>
                                <HeaderStyle CssClass="table-header" />
                                <RowStyle CssClass="table-row" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>