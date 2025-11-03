<%@ Page Title="Takipteki Firmalar" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="TakiptekiFirmalar.aspx.cs" Inherits="Portal.ModulBelgeTakip.TakiptekiFirmalar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="~/wwwroot/css/BELGETAKIPMODUL.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">
        <div class="card">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h3 class="card-title mb-0">
                    <i class="fas fa-clipboard-list me-2"></i>Takipteki Firmalar - Belge Takip Sistemi
                </h3>
                <asp:Label ID="lblKayitSayisi" runat="server" CssClass="badge bg-primary"></asp:Label>
            </div>
            
            <div class="card-body">
                <!-- Filtre Bölümü -->
                <div class="filter-section">
                    <div class="row g-3 align-items-end">
                        <div class="col-md-3">
                            <label for="DdlBelgeTuru" class="form-label">
                                <i class="fas fa-file-alt me-1"></i>Belge Türü
                            </label>
                            <asp:DropDownList ID="DdlBelgeTuru" runat="server" CssClass="form-select"
                                AutoPostBack="true" OnSelectedIndexChanged="DdlBelgeTuru_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        
                        <div class="col-md-3">
                            <label for="DdlIl" class="form-label">
                                <i class="fas fa-map-marker-alt me-1"></i>İl
                            </label>
                            <asp:DropDownList ID="DdlIl" runat="server" CssClass="form-select"
                                AutoPostBack="true" OnSelectedIndexChanged="DdlIl_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        
                        <div class="col-md-3">
                            <label for="DdlIlce" class="form-label">
                                <i class="fas fa-map-pin me-1"></i>İlçe
                            </label>
                            <asp:DropDownList ID="DdlIlce" runat="server" CssClass="form-select"
                                AutoPostBack="true" OnSelectedIndexChanged="DdlIlce_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        
                        <div class="col-md-3">
                            <asp:Button ID="btnYenile" runat="server" 
                                CssClass="btn btn-outline-secondary w-100" 
                                OnClick="btnYenile_Click" 
                                CausesValidation="false"
                                Text="🔄 Yenile" />
                        </div>
                    </div>
                </div>

                <!-- GridView Bölümü -->
                <div class="table-responsive" style="min-width: 1200px;">
                    <asp:GridView ID="TakiptekiFirmalarGrid" runat="server"
                        CssClass="table table-striped table-bordered table-hover"
                        AutoGenerateColumns="false"
                        AllowPaging="false"
                        AllowSorting="true"
                        DataKeyNames="ID"
                        OnRowDataBound="TakiptekiFirmalarGrid_RowDataBound"
                        OnSorting="TakiptekiFirmalarGrid_Sorting"
                        EmptyDataText="Görüntülenecek kayıt bulunamadı.">
                        <Columns>
                            <asp:BoundField DataField="BELGE_TURU" HeaderText="Belge Türü" SortExpression="BELGE_TURU" />
                            <asp:BoundField DataField="IL" HeaderText="İl" SortExpression="IL" />
                            <asp:BoundField DataField="ILCE" HeaderText="İlçe" SortExpression="ILCE" />
                            <asp:BoundField DataField="ADRES" HeaderText="Adres" SortExpression="ADRES" />
                            <asp:BoundField DataField="FIRMA_ADI" HeaderText="Firma Adı" SortExpression="FIRMA_ADI" />
                            <asp:BoundField DataField="VERGI_NUMARASI" HeaderText="Vergi No" SortExpression="VERGI_NUMARASI" />
                            <asp:BoundField DataField="DENETIM_TARIHI" HeaderText="Denetim Tarihi" 
                                DataFormatString="{0:dd.MM.yyyy}" SortExpression="DENETIM_TARIHI" />
                            
                            <asp:TemplateField HeaderText="Tebliğ Tarihi" SortExpression="SONCEZA_TEBLIG_TARIHI">
                                <ItemTemplate>
                                    <asp:Label ID="lblTebligTarihi" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Muafiyet Durumu" SortExpression="SONCEZA_TEBLIG_TARIHI">
                                <ItemTemplate>
                                    <asp:Label ID="lblMuafiyetDurumu" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Ceza Sayısı" SortExpression="CEZA_SAYISI">
                                <ItemTemplate>
                                    <asp:Label ID="lblCezaSayisi" runat="server" CssClass="badge bg-danger" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Belge Durumu" SortExpression="BELGE_ALDIMI">
                                <ItemTemplate>
                                    <span class='<%# Convert.ToBoolean(Eval("BELGE_ALDIMI")) ? "highlight-green" : "highlight-red" %>'>
                                        <i class='fas <%# Convert.ToBoolean(Eval("BELGE_ALDIMI")) ? "fa-check-circle" : "fa-times-circle" %> me-1'></i>
                                        <%# Convert.ToBoolean(Eval("BELGE_ALDIMI")) ? "Belge Aldı" : "Belge Almadı" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        
                        <EmptyDataTemplate>
                            <div class="alert alert-info text-center">
                                <i class="fas fa-info-circle me-2"></i>
                                Görüntülenecek kayıt bulunamadı.
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>

                <!-- Export Butonları -->
                <asp:Panel ID="PanelExport" runat="server" CssClass="export-panel text-end">
                    <asp:Button ID="btnExcelAktar" runat="server"
                        OnClick="btnExcelAktar_Click" 
                        CausesValidation="false" 
                        CssClass="btn btn-success"
                        Text="📊 Excel'e Aktar" />
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
