<%@ Page Title="Dosya Birleştir" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" 
    CodeBehind="DosyaBirlestir.aspx.cs" Inherits="Portal.ModulAraclar.DosyaBirlestir" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item"><a href="/Anasayfa.aspx">Ana Sayfa</a></li>
    <li class="breadcrumb-item"><a href="#">Çeşitli Araçlar</a></li>
    <li class="breadcrumb-item active" aria-current="page">Dosya Birleştir</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card shadow-custom-md">
        <div class="card-header">
            <h5 class="mb-0">
                <i class="fas fa-file-pdf me-2"></i>Arşiv Dosya Birleştir
            </h5>
        </div>
        <div class="card-body">
            <div class="row g-3 mb-4">
                <div class="col-md-4">
                    <label for="txtUnet" class="form-label">UNET Numarası</label>
                    <asp:TextBox ID="txtUnet" runat="server" CssClass="form-control" 
                        placeholder="UNET numarası giriniz" TextMode="Number"></asp:TextBox>
                </div>
                
                <div class="col-md-4">
                    <label for="ddlBelgeTuru" class="form-label">Belge Türü</label>
                    <asp:DropDownList ID="ddlBelgeTuru" runat="server" CssClass="form-select">
                    </asp:DropDownList>
                </div>
                
                <div class="col-md-4 d-flex align-items-end">
                    <asp:Button ID="btnAra" runat="server" CssClass="btn btn-primary me-2" 
                        OnClick="btnAra_Click" Text="🔍 Ara" />
                    <asp:Button ID="btnBirlestir" runat="server" CssClass="btn btn-success" 
                        OnClick="btnBirlestir_Click" Text="📄 Birleştir" Enabled="false" />
                </div>
            </div>

            <div class="alert alert-info">
                <i class="fas fa-info-circle me-2"></i>
                <strong>Bilgi:</strong> UNET numarası veya belge türü seçerek dosyaları arayabilirsiniz. 
                Birleştirmek istediğiniz dosyalar listelendikten sonra "Birleştir" butonuna tıklayın.
            </div>

            <div class="table-responsive">
                <asp:GridView ID="DosyalarGrid" runat="server" CssClass="table table-striped table-hover" 
                    AutoGenerateColumns="False" ShowFooter="True" EmptyDataText="Kayıt bulunamadı." DataKeyNames="Id, Dosya">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" />
                        <asp:BoundField DataField="Unet" HeaderText="UNET" />
                        <asp:BoundField DataField="Unvan" HeaderText="Ünvan" />
                        <asp:BoundField DataField="Belge_Turu" HeaderText="Belge Türü" />
                        <asp:BoundField DataField="Sayfa_Sayisi" HeaderText="Sayfa Sayısı" />                        
                    </Columns>
                    <FooterStyle CssClass="table-dark" Font-Bold="true" />
                    <HeaderStyle CssClass="table-dark" />
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>