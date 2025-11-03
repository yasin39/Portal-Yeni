<%@ Page Title="Uzaktan Denetim Raporları" Language="C#" MasterPageFile="~/AnaV2.Master" 
    AutoEventWireup="true" CodeBehind="UzakRapor.aspx.cs" 
    Inherits="Portal.ModulDenetim.UzakRapor" EnableEventValidation="false" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <link href="~/wwwroot/css/DENETIMMODUL.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="ContentBreadcrumb" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item">
        <i class="fas fa-clipboard-check me-1"></i>Denetim Modülü
    </li>
    <li class="breadcrumb-item active">Uzaktan Denetim Raporları</li>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        
        <!-- Filtre Kartı -->
        <div class="card denetim-filter-card">
            <div class="card-header">
                <i class="fas fa-filter me-2"></i>Arama ve Filtreleme
            </div>
            <div class="card-body">
                <div class="row g-3">
                    <!-- Personel Seçimi -->
                    <div class="col-md-3">
                        <label class="form-label">Personel</label>
                        <asp:DropDownList ID="ddlPersonel" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                    </div>

                    <!-- Durum Seçimi -->
                    <div class="col-md-2">
                        <label class="form-label">Durum</label>
                        <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                            <asp:ListItem Value="Açık">Açık</asp:ListItem>
                            <asp:ListItem Value="Kapalı">Kapalı</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <!-- Başlangıç Tarihi -->
                    <div class="col-md-2">
                        <label class="form-label">Başlangıç Tarihi</label>
                        <asp:TextBox ID="txtBaslangicTarihi" runat="server" 
                            CssClass="form-control" TextMode="Date">
                        </asp:TextBox>
                    </div>

                    <!-- Bitiş Tarihi -->
                    <div class="col-md-2">
                        <label class="form-label">Bitiş Tarihi</label>
                        <asp:TextBox ID="txtBitisTarihi" runat="server" 
                            CssClass="form-control" TextMode="Date">
                        </asp:TextBox>
                    </div>

                    <!-- Butonlar -->
                    <div class="col-md-3 d-flex align-items-end gap-2">
                        <asp:Button ID="btnAra" runat="server" 
                            Text="🔍 Ara" 
                            CssClass="btn btn-primary" 
                            OnClick="btnAra_Click" />
                        <asp:Button ID="btnTumunuListele" runat="server" 
                            Text="📜 Tümü" 
                            CssClass="btn btn-outline-secondary" 
                            OnClick="btnTumunuListele_Click" 
                            CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Sonuç Bilgisi -->
        <asp:Label ID="lblSonucBilgisi" runat="server" 
            CssClass="badge bg-info mb-3" 
            Visible="false">
        </asp:Label>

        <!-- Sonuç Kartı -->
        <div class="card denetim-result-card">
            <div class="denetim-result-header">
                <h5 class="denetim-result-title">
                    <i class="fas fa-list me-2"></i>Denetim Kayıtları
                </h5>
                <div class="d-flex gap-2 align-items-center">
                    <span class="denetim-count-badge">
                        <asp:Label ID="lblKayitSayisi" runat="server" Text="0 kayıt"></asp:Label>
                    </span>
                    <asp:Button ID="btnExcelAktar" runat="server" 
                        Text="📊 Excel" 
                        CssClass="btn btn-success" 
                        OnClick="btnExcelAktar_Click" />
                </div>
            </div>
            <div class="card-body p-0">
                <div class="table-responsive">
                    <asp:GridView ID="gvDenetimler" runat="server" 
                        CssClass="table table-hover denetim-grid mb-0"
                        AutoGenerateColumns="False"
                        EmptyDataText="Kayıt bulunamadı.">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                            
                            <asp:BoundField DataField="Tarih" HeaderText="Tarih" 
                                DataFormatString="{0:dd.MM.yyyy}" />
                            
                            <asp:BoundField DataField="AracSayisi" HeaderText="Araç Sayısı" />
                            
                            <asp:BoundField DataField="UygunsuzAracSayisi" HeaderText="Uygunsuz Araç" />
                            
                            <asp:BoundField DataField="YBOlmayanAracSayisi" HeaderText="YB Olmayan" />
                            
                            <asp:BoundField DataField="YBKayitliOlmayanAracSayisi" HeaderText="YB Kayıtlı Olmayan" />
                            
                            <asp:BoundField DataField="AtananPersonel" HeaderText="Atanan Personel" />
                            
                            <asp:TemplateField HeaderText="Durum">
                                <ItemTemplate>
                                    <span class='<%# Eval("Durum").ToString() == "Açık" ? "durum-badge durum-acik" : "durum-badge durum-kapali" %>'>
                                        <%# Eval("Durum") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

    </div>
</asp:Content>