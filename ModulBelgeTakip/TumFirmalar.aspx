<%@ Page Title="Tüm Firmalar" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" 
    CodeBehind="TumFirmalar.aspx.cs" Inherits="Portal.ModulBelgeTakip.TumFirmalar" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="~/wwwroot/Css/BELGETAKIPMODUL.css" />
</asp:Content>

<asp:Content ID="ContentBreadcrumb" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item">
        <i class="fas fa-file-alt me-1"></i>Belge Takip
    </li>
    <li class="breadcrumb-item active">Tüm Firmalar</li>
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid wide-container mt-4">
        <div class="card">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h3 class="card-title mb-0">
                    <i class="fas fa-building me-2"></i>Firma Listesi
                </h3>
                <asp:Label ID="lblKayitSayisi" runat="server" CssClass="badge bg-primary"></asp:Label>
            </div>
            
            <div class="card-body">
                <!-- Filtre Bölümü -->
                <div class="filter-section">
                    <div class="row g-3 align-items-end">
                        <div class="col-md-3">
                            <label for="txtVergiNo" class="form-label">
                                <i class="fas fa-hashtag me-1 text-primary-custom"></i>Vergi No
                            </label>
                            <asp:TextBox ID="txtVergiNo" runat="server" CssClass="form-control"
                                placeholder="Vergi No Ara" MaxLength="20"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revVergiNo" runat="server"
                                ControlToValidate="txtVergiNo" ValidationExpression="^\d*$"
                                ErrorMessage="Sadece sayısal değer giriniz" CssClass="text-danger small"
                                Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                        
                        <div class="col-md-3">
                            <label for="ddlBelgeTuru" class="form-label">
                                <i class="fas fa-certificate me-1 text-primary-custom"></i>Belge Türü
                            </label>
                            <asp:DropDownList ID="ddlBelgeTuru" runat="server" CssClass="form-select"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlBelgeTuru_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        
                        <div class="col-md-3">
                            <label for="ddlYil" class="form-label">
                                <i class="fas fa-calendar me-1 text-primary-custom"></i>Yıl
                            </label>
                            <asp:DropDownList ID="ddlYil" runat="server" CssClass="form-select"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlYil_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        
                        <div class="col-md-3">
                            <asp:Button ID="btnFiltrele" runat="server" CssClass="btn btn-primary me-2"
                                OnClick="btnFiltrele_Click" Text="🔍 Filtrele" />
                            <asp:Button ID="btnTemizle" runat="server" CssClass="btn btn-outline-secondary"
                                OnClick="btnTemizle_Click" CausesValidation="false" Text="🗑️ Temizle" />
                        </div>
                    </div>
                </div>

                <!-- Mesaj Paneli -->
                <asp:Panel ID="pnlMesajlar" runat="server" Visible="false" CssClass="mb-3">
                    <asp:Label ID="lblHata" runat="server" CssClass="alert alert-danger d-block"
                        Visible="false" EnableViewState="false"></asp:Label>
                    <asp:Label ID="lblBilgi" runat="server" CssClass="alert alert-info d-block"
                        Visible="false" EnableViewState="false"></asp:Label>
                </asp:Panel>

                <!-- GridView -->
                <div class="table-responsive">
                    <asp:GridView ID="gvFirmalar" runat="server"
                        CssClass="table table-striped table-bordered table-hover"
                        AutoGenerateColumns="false"
                        AllowPaging="true"
                        AllowSorting="true"
                        PageSize="20"
                        DataKeyNames="ID"
                        OnRowDataBound="gvFirmalar_RowDataBound"
                        OnPageIndexChanging="gvFirmalar_PageIndexChanging"
                        OnSelectedIndexChanged="gvFirmalar_SelectedIndexChanged"
                        OnSorting="gvFirmalar_Sorting"
                        EmptyDataText="Görüntülenecek kayıt bulunamadı."
                        EmptyDataRowStyle-CssClass="alert alert-info">
                        <Columns>
                            <asp:BoundField DataField="IL" HeaderText="İl" SortExpression="IL" />
                            <asp:BoundField DataField="ILCE" HeaderText="İlçe" SortExpression="ILCE" />
                            <asp:BoundField DataField="FIRMA_ADI" HeaderText="Firma Adı" SortExpression="FIRMA_ADI" />
                            <asp:BoundField DataField="VERGI_NUMARASI" HeaderText="Vergi No" SortExpression="VERGI_NUMARASI" />
                            <asp:BoundField DataField="FIRMA_ADRESI" HeaderText="Adres" SortExpression="FIRMA_ADRESI" />
                            <asp:BoundField DataField="FIRMA_TIPI" HeaderText="Faaliyet Türü" SortExpression="FIRMA_TIPI" />
                            <asp:BoundField DataField="BELGE_AD" HeaderText="Belge Türü" SortExpression="BELGE_AD" />
                            
                            <asp:TemplateField HeaderText="Belge Durumu" SortExpression="BELGE_ALDIMI">
                                <ItemTemplate>
                                    <span class='<%# Convert.ToBoolean(Eval("BELGE_ALDIMI")) ? "highlight-green" : "highlight-red" %>'>
                                        <%# Convert.ToBoolean(Eval("BELGE_ALDIMI")) ? "✓ Belge Aldı" : "✗ Belge Almadı" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField DataField="BELGE_ALMA_TARIHI" HeaderText="Belge Alma Tarihi"
                                DataFormatString="{0:dd.MM.yyyy}" SortExpression="BELGE_ALMA_TARIHI" 
                                NullDisplayText="-" />
                            
                            <asp:TemplateField HeaderText="Ceza Sayısı" SortExpression="CEZA_SAYISI">
                                <ItemTemplate>
                                    <asp:Label ID="lblCezaSayisi" runat="server" 
                                        CssClass='<%# Convert.ToInt32(Eval("CEZA_SAYISI")) > 0 ? "highlight-red" : "" %>'
                                        Text='<%# Eval("CEZA_SAYISI") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="İşlemler" ItemStyle-Width="100px" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnIncele" runat="server" CommandName="Select"
                                        CssClass="btn btn-sm btn-primary" Text="🔍 İncele" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                        <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>

                <!-- Export Butonları -->
                <asp:Panel ID="pnlExport" runat="server" CssClass="export-panel text-end" Visible="false">
                    <asp:Button ID="btnExcelAktar" runat="server" 
                        Text="📊 Excel'e Aktar"
                        OnClick="btnExcelAktar_Click" 
                        CssClass="btn btn-success" 
                        CausesValidation="false" />
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
