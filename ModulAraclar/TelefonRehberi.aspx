<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TelefonRehberi.aspx.cs" 
    Inherits="Portal.ModulAraclar.TelefonRehberi" MasterPageFile="~/AnaV2.Master" %>

<%-- 
    Bu sayfa, AnaV2.Master sayfasını kullanır.
    Tüm içerik <asp:Content> etiketleri içine yerleştirilmelidir.
--%>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <%-- Bu sayfaya özel CSS stillerini buraya ekleyebilirsiniz --%>
    <style>
        /* Gerekirse ek stiller */
    </style>
</asp:Content>

<asp:Content ID="BreadcrumbContent" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <%-- Sayfanın "breadcrumb" navigasyonunu tanımlar --%>
    <li class="breadcrumb-item"><a href="/Anasayfa.aspx">Ana Sayfa</a></li>
    <li class="breadcrumb-item">Çeşitli Araçlar</li>
    <li class="breadcrumb-item active" aria-current="page">Telefon Rehberi</li>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <%-- 
        AnaV2.Master ile uyumlu olması için Bootstrap 5 card yapısı kullanıldı.
    --%>
    <div class="card shadow-sm">
        <div class="card-header bg-light">
            <h5 class="mb-0">
                <i class="fas fa-phone-alt me-2"></i>Telefon Rehberi Sorgulama
            </h5>
        </div>
        
        <div class="card-body">
            <%-- 
                Orijinal form elemanları Bootstrap 5 grid sistemi ile yeniden düzenlendi.
                ID'ler daha anlaşılır olacak şekilde (TextBox1 -> txtAd) değiştirildi.
            --%>
            <div class="row g-3 align-items-end">
                <div class="col-md-3">
                    <label for="<%=txtAd.ClientID%>" class="form-label">Adı</label>
                    <asp:TextBox ID="txtAd" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label for="<%=txtSoyad.ClientID%>" class="form-label">Soyadı</label>
                    <asp:TextBox ID="txtSoyad" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label for="<%=ddlBirim.ClientID%>" class="form-label">Birimi</label>
                    <asp:DropDownList ID="ddlBirim" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnAra" runat="server" OnClick="btnAra_Click" Text="Ara" CssClass="btn btn-primary w-100" />
                </div>
            </div>

            <hr class="my-4" />

            <%-- Sonuçların gösterileceği alan --%>
            <div class="table-responsive">
                <asp:Label ID="lblTable" runat="server" Visible="False"></asp:Label>
            </div>
        </div>

        <div class="card-footer">
            <%-- Orijinal sayfadaki alt bilgi notu --%>
             <div class="alert alert-info mb-0">
                 <strong>Bilgi:</strong> Dış Nizamiye Güvenlik : 9200 | Çay Ocağı : 9214 | Bölge Müdürlüğü Faks : 3974086 | Bakanlık Santral : 1000
             </div>
        </div>
    </div>

</asp:Content>