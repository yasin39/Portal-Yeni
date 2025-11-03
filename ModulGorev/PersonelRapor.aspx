<%@ Page Title="Personel Görev Rapor" Language="C#" MasterPageFile="~/AnaV2.Master" 
    AutoEventWireup="true" CodeBehind="PersonelRapor.aspx.cs" 
    Inherits="Portal.ModulGorev.PersonelRapor" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/BELGETAKIPMODUL.css" rel="stylesheet" />       
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">

        <!-- Başlık -->
        <div class="row mb-4">
            <div class="col-12">
                <h3 class="text-primary-custom mb-0">
                    <i class="fas fa-user-clock me-2"></i>Personel Görev Rapor
                </h3>
                <p class="text-muted mb-0">Personel görevlendirme kayıtlarını görüntüleyin ve raporlayın</p>
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
                    <div class="col-md-3">
                        <label class="form-label">Personel</label>
                        <asp:DropDownList ID="ddlPersonel" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">İl</label>
                        <asp:DropDownList ID="ddlIl" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Başlangıç Tarihi</label>
                        <asp:TextBox ID="txtBaslangicTarihi" runat="server" CssClass="form-control" 
                            TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Bitiş Tarihi</label>
                        <asp:TextBox ID="txtBitisTarihi" runat="server" CssClass="form-control" 
                            TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="col-md-12 d-flex align-items-end gap-2">
                        <asp:Button ID="btnAra" runat="server" Text="🔍 Ara" 
                            CssClass="btn btn-primary" OnClick="btnAra_Click" />
                        <asp:Button ID="btnTumunuListele" runat="server" Text="📜 Tümünü Listele"
                            CssClass="btn btn-outline-secondary" OnClick="btnTumunuListele_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Sonuç Bilgisi -->
        <asp:Label ID="lblSonucBilgisi" runat="server" CssClass="badge bg-primary mb-3" 
            Visible="false"></asp:Label>

        <!-- Görev Listesi -->
        <div class="card mb-4">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h5 class="card-title mb-0">
                    <i class="fas fa-list me-2"></i>Görev Kayıtları
                    <asp:Label ID="lblKayitSayisi" runat="server" CssClass="badge bg-light text-dark ms-2"></asp:Label>
                </h5>
                <div>
                    <asp:Button ID="btnExcelAktar" runat="server" Text="📊 Excel'e Aktar"
                        CssClass="btn btn-success btn-sm me-2" OnClick="btnExcelAktar_Click" />                   
                </div>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="GorevlerGrid" runat="server" CssClass="table table-striped table-hover"
                        AutoGenerateColumns="False" ShowFooter="True">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="No" />
                            <asp:BoundField DataField="AdiSoyadi" HeaderText="Adı Soyadı" />
                            <asp:BoundField DataField="BaslamaTarihi" HeaderText="Başlama Tarihi" 
                                DataFormatString="{0:dd.MM.yyyy}" />
                            <asp:BoundField DataField="GorevlendirmeSuresi" HeaderText="Görev Süresi" />
                            <asp:BoundField DataField="BitisTarihi" HeaderText="Bitiş Tarihi" 
                                DataFormatString="{0:dd.MM.yyyy}" />
                            <asp:BoundField DataField="il" HeaderText="İl" />
                            <asp:BoundField DataField="Digeriller" HeaderText="Diğer İller" />
                            <asp:BoundField DataField="GorevTanimi" HeaderText="Açıklama" />
                            <asp:BoundField DataField="KayitKullanici" HeaderText="Kullanıcı" />
                            <asp:BoundField DataField="KayitTarihi" HeaderText="Kayıt Tarihi" 
                                DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                            <asp:BoundField DataField="GuncelleyenKullanici" HeaderText="Güncelleyen" />
                            <asp:BoundField DataField="GuncellemeTarihi" HeaderText="Güncelleme Tarihi" 
                                DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                        </Columns>
                        <HeaderStyle CssClass="table-header" />
                        <FooterStyle CssClass="table-footer" />
                        <PagerStyle CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
            </div>
        </div>

    </div>
  
</asp:Content>