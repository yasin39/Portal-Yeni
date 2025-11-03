<%@ Page Title="Denetim İstatistikleri" Language="C#" MasterPageFile="~/AnaV2.Master" 
    AutoEventWireup="true" CodeBehind="Istatistik.aspx.cs" 
    Inherits="Portal.ModulDenetim.Istatistik" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <style>
    /* Sadece bu sayfaya özel stiller - Ortak stiller Common-Components.css'e taşındı */

    .stat-widget .row > div {
        background: rgba(0,0,0,0.15);
        padding: 1rem;
        border-radius: 8px;
        margin: 0.25rem;
    }

    .stat-widget small {
        font-size: 0.85rem;
        font-weight: 500;
        display: block;
        margin-top: 0.5rem;
    }

    .summary-box {
        background: white;
        border-radius: 10px;
        padding: 1.5rem;
        box-shadow: 0 2px 8px rgba(0,0,0,0.08);
        margin-bottom: 1.5rem;
    }

    .summary-text {
        font-size: 1.2rem;
        font-weight: 600;
        color: #2E5B9A;
    }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        
        <div class="row mb-3">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0">
                            <i class="fas fa-chart-bar me-2"></i>Denetim İstatistikleri
                        </h5>
                    </div>
                    <div class="card-body">
                        <div class="row align-items-center">
                            <div class="col-md-3">
                                <label class="form-label">Raporlama Yılı:</label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlYil" runat="server" CssClass="form-select" 
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlYil_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-3 col-md-6">
                <div class="stat-widget widget-blue">
                    <div class="text-center">
                        <div class="stat-icon">
                            <i class="fas fa-bus"></i>
                        </div>
                        <div class="stat-label">Yolcu Taşımacılığı</div>
                        <hr class="hr-light">
                        <div class="row">
                            <div class="col-6">
                                <i class="fas fa-building mb-2" class="icon-md"></i>
                                <div><asp:Label ID="lblYolcuIsletme" runat="server" Text="0"></asp:Label></div>
                                <small>İşletme</small>
                            </div>
                            <div class="col-6">
                                <i class="fas fa-bus mb-2" class="icon-md"></i>
                                <div><asp:Label ID="lblYolcuArac" runat="server" Text="0"></asp:Label></div>
                                <small>Taşıt</small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-3 col-md-6">
                <div class="stat-widget widget-pink">
                    <div class="text-center">
                        <div class="stat-icon">
                            <i class="fas fa-box"></i>
                        </div>
                        <div class="stat-label">Eşya Taşımacılığı</div>
                        <hr class="hr-light">
                        <div class="row">
                            <div class="col-6">
                                <i class="fas fa-warehouse mb-2" class="icon-md"></i>
                                <div><asp:Label ID="lblEsyaIsletme" runat="server" Text="0"></asp:Label></div>
                                <small>İşletme</small>
                            </div>
                            <div class="col-6">
                                <i class="fas fa-truck mb-2" class="icon-md"></i>
                                <div><asp:Label ID="lblEsyaArac" runat="server" Text="0"></asp:Label></div>
                                <small>Taşıt</small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-3 col-md-6">
                <div class="stat-widget widget-red">
                    <div class="text-center">
                        <div class="stat-icon">
                            <i class="fas fa-fire"></i>
                        </div>
                        <div class="stat-label">Tehlikeli Madde</div>
                        <hr class="hr-light">
                        <div class="row">
                            <div class="col-6">
                                <i class="fas fa-flask mb-2" class="icon-md"></i>
                                <div><asp:Label ID="lblTmIsletme" runat="server" Text="0"></asp:Label></div>
                                <small>İşletme</small>
                            </div>
                            <div class="col-6">
                                <i class="fas fa-fire mb-2" class="icon-md"></i>
                                <div><asp:Label ID="lblTmArac" runat="server" Text="0"></asp:Label></div>
                                <small>Taşıt</small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-3 col-md-6">
                <div class="stat-widget widget-yellow">
                    <div class="text-center">
                        <div class="stat-icon">
                            <i class="fas fa-desktop"></i>
                        </div>
                        <div class="stat-label">Uzak Denetim</div>
                        <hr class="hr-light">
                        <div class="row">
                            <div class="col-6">
                                <i class="fas fa-tv mb-2" class="icon-md"></i>
                                <div><asp:Label ID="lblUzakArac" runat="server" Text="0"></asp:Label></div>
                                <small>YKDİ Araç</small>
                            </div>
                            <div class="col-6">
                                <i class="fas fa-camera mb-2" class="icon-md"></i>
                                <div><asp:Label ID="lblUzakCeza" runat="server" Text="0"></asp:Label></div>
                                <small>Ceza</small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6">
                <div class="summary-box">
                    <div class="text-center">
                        <i class="fas fa-building text-primary mb-2" class="icon-lg"></i>
                        <div class="summary-text">
                            <asp:Label ID="lblToplamIsletme" runat="server" Text="Toplam İşletme Denetimi: 0"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="summary-box">
                    <div class="text-center">
                        <i class="fas fa-car text-primary mb-2" class="icon-lg"></i>
                        <div class="summary-text">
                            <asp:Label ID="lblToplamArac" runat="server" Text="Toplam Araç Denetimi: 0"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h5 class="mb-0">
                            <i class="fas fa-table me-2"></i>Aylık İşletme Denetimi
                        </h5>
                        <asp:Button ID="btnExcelIsletme" runat="server" CssClass="btn btn-success btn-sm" 
                            Text="📊 Excel'e Aktar" OnClick="btnExcelIsletme_Click" />
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="gvAylikIsletme" runat="server" CssClass="table table-striped table-hover" 
                                AutoGenerateColumns="true">
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mt-3">
            <div class="col-12">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h5 class="mb-0">
                            <i class="fas fa-table me-2"></i>Aylık Taşıt Denetimi
                        </h5>
                        <asp:Button ID="btnExcelTasit" runat="server" CssClass="btn btn-success btn-sm" 
                            Text="📊 Excel'e Aktar" OnClick="btnExcelTasit_Click" />
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="gvAylikTasit" runat="server" CssClass="table table-striped table-hover" 
                                AutoGenerateColumns="true">
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mt-3">
            <div class="col-lg-6">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0">
                            <i class="fas fa-chart-pie me-2"></i>İşletme Denetimi İllere Göre Dağılımı
                        </h5>
                    </div>
                    <div class="card-body text-center">
                        <asp:Chart ID="ChartIsletmeIl" runat="server" Width="600px" Height="500px">
                            <Series>
                                <asp:Series Name="IsletmeIl" ChartType="Pie" 
                                    Label="#VALX - #VAL (#PERCENT{P})" 
                                    IsValueShownAsLabel="True">
                                    <SmartLabelStyle Enabled="True" />
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <Area3DStyle Enable3D="True" />
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </div>
                </div>
            </div>

            <div class="col-lg-6">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0">
                            <i class="fas fa-chart-pie me-2"></i>Araç Denetimi İllere Göre Dağılımı
                        </h5>
                    </div>
                    <div class="card-body text-center">
                        <asp:Chart ID="ChartAracIl" runat="server" Width="600px" Height="500px">
                            <Series>
                                <asp:Series Name="AracIl" ChartType="Pie" 
                                    Label="#VALX - #VAL (#PERCENT{P})" 
                                    IsValueShownAsLabel="True">
                                    <SmartLabelStyle Enabled="True" />
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <Area3DStyle Enable3D="True" />
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mt-3">
            <div class="col-lg-6">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0">
                            <i class="fas fa-chart-bar me-2"></i>İşletme Denetim Türü Dağılımı
                        </h5>
                    </div>
                    <div class="card-body text-center">
                        <asp:Chart ID="ChartIsletmeTur" runat="server" Width="800px" Height="600px" Palette="SeaGreen">
                            <Series>
                                <asp:Series Name="IsletmeTur" ChartType="Column" 
                                    Label="#VALX - #VAL" 
                                    IsValueShownAsLabel="True"
                                    CustomProperties="DrawingStyle=Cylinder">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <AxisX Enabled="False" />
                                    <Area3DStyle Enable3D="True" Inclination="20" />
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </div>
                </div>
            </div>

            <div class="col-lg-6">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0">
                            <i class="fas fa-chart-bar me-2"></i>Taşıt Denetim Türü Dağılımı
                        </h5>
                    </div>
                    <div class="card-body text-center">
                        <asp:Chart ID="ChartTasitTur" runat="server" Width="800px" Height="600px" Palette="SeaGreen">
                            <Series>
                                <asp:Series Name="TasitTur" ChartType="Column" 
                                    Label="#VALX - #VAL" 
                                    IsValueShownAsLabel="True"
                                    CustomProperties="DrawingStyle=Cylinder">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <AxisX Enabled="False" />
                                    <Area3DStyle Enable3D="True" Inclination="20" />
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mt-3">
            <div class="col-lg-6">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0">
                            <i class="fas fa-users me-2"></i>İşletme Denetimlerinin Personele Göre Dağılımı
                        </h5>
                    </div>
                    <div class="card-body text-center">
                        <asp:Chart ID="ChartIsletmePersonel" runat="server" Width="800px" Height="700px" Palette="SeaGreen">
                            <Series>
                                <asp:Series Name="IsletmePersonel" ChartType="Column" 
                                    Label="#VALX - #VAL" 
                                    IsValueShownAsLabel="True"
                                    CustomProperties="DrawingStyle=Cylinder">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <AxisX Enabled="False" />
                                    <Area3DStyle Enable3D="True" Inclination="20" />
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </div>
                </div>
            </div>

            <div class="col-lg-6">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0">
                            <i class="fas fa-users me-2"></i>Taşıt Denetimlerinin Personele Göre Dağılımı
                        </h5>
                    </div>
                    <div class="card-body text-center">
                        <asp:Chart ID="ChartTasitPersonel" runat="server" Width="800px" Height="700px" Palette="SeaGreen">
                            <Series>
                                <asp:Series Name="TasitPersonel" ChartType="Column" 
                                    Label="#VALX - #VAL" 
                                    IsValueShownAsLabel="True"
                                    CustomProperties="DrawingStyle=Cylinder">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <AxisX Enabled="False" />
                                    <Area3DStyle Enable3D="True" Inclination="20" />
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>