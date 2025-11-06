<%@ Page Title="Öğrenim Ekle/Düzenle" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="OgrenimEkle.aspx.cs" Inherits="Portal.ModulPersonel.OgrenimEkle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <div class="card-header bg-primary text-white">
            <h5 class="mb-0"><i class="fas fa-graduation-cap me-2"></i>Eğitim Durumu Yönetimi</h5>
        </div>
        <div class="card-body">
            <div class="row mb-3">
                <div class="col-md-3">
                    <label class="form-label">Sicil No</label>
                    <div class="input-group">
                        <asp:TextBox ID="txtSicil" runat="server" CssClass="form-control" placeholder="Sicil No Giriniz"></asp:TextBox>
                        <asp:Button ID="btnSicilAra" runat="server" Text="Bul" CssClass="btn btn-outline-primary" OnClick="AramaYap" />
                    </div>
                </div>
                <div class="col-md-3">
                    <label class="form-label">TC Kimlik No</label>
                    <div class="input-group">
                        <asp:TextBox ID="txtTc" runat="server" CssClass="form-control" placeholder="TC No Giriniz"></asp:TextBox>
                        <asp:Button ID="btnTcAra" runat="server" Text="Bul" CssClass="btn btn-outline-primary" OnClick="AramaYap" />
                    </div>
                </div>
                <div class="col-md-6">
                    <label class="form-label">Adı Soyadı</label>
                    <asp:Label ID="lblAdSoyad" runat="server" CssClass="form-control-plaintext" Text="-"></asp:Label>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-3">
                    <label class="form-label">Öğrenim Durumu</label>
                    <asp:DropDownList ID="ddlOgrenimDurumu" runat="server" CssClass="form-select">
                        <asp:ListItem Value="">Seçiniz...</asp:ListItem>
                        <asp:ListItem>İlköğretim</asp:ListItem>
                        <asp:ListItem>Lise</asp:ListItem>
                        <asp:ListItem>Yüksekokul</asp:ListItem>
                        <asp:ListItem>Lisans</asp:ListItem>
                        <asp:ListItem>Yüksek Lisans</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Mezun Olunan Okul/Fakülte</label>
                    <asp:TextBox ID="txtOkul" runat="server" CssClass="form-control" placeholder="Örn: Gazi Üniversitesi"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Bölüm</label>
                    <asp:TextBox ID="txtBolum" runat="server" CssClass="form-control" placeholder="Örn: Kamu Yönetimi"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Mezuniyet Tarihi</label>
                    <asp:TextBox ID="txtMezuniyetTarihi" runat="server" CssClass="form-control fp-date" placeholder="GG/AA/YYYY"></asp:TextBox>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-12">
                    <asp:Button ID="btnOgrenimEkle" runat="server" Text="Öğrenim Ekle" CssClass="btn btn-primary me-2" OnClick="EgitimEkle" />
                    <asp:Button ID="btnOgrenimSil" runat="server" Text="Öğrenim Sil" CssClass="btn btn-danger" OnClick="OgrenimSil" Visible="false" />
                </div>
            </div>

            <div class="row">
                <div class="col-12 text-center mb-3">
                    <asp:Image ID="imgPersonel" runat="server" Height="100px" Width="100px" CssClass="rounded-circle border" Visible="false" />
                    <br />
                </div>
                <div class="col-12">
                    <asp:GridView ID="GridViewOgrenim" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False"
                        OnSelectedIndexChanged="GridViewOgrenim_SelectedIndexChanged" DataKeyNames="id"
                        EmptyDataText="Bu personele ait öğrenim kaydı bulunamadı.">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                            <asp:BoundField DataField="Ogr_Durumu" HeaderText="Öğrenim Durumu" />
                            <asp:BoundField DataField="Okul" HeaderText="Okul/Fakülte" />
                            <asp:BoundField DataField="Bolum" HeaderText="Bölüm" />
                            <asp:BoundField DataField="Mezuniyet_Tarihi" HeaderText="Mezuniyet Tarihi" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:CommandField ButtonType="Button" ShowSelectButton="true" SelectText="Seç" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>