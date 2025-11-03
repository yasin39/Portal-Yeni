<%@ Page Title="Personel İzin Girişi" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="personelizinekle.aspx.cs" Inherits="Portal.ModulPersonel.IzinEkle" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-section { margin-bottom: 20px; }
        .form-group { margin-bottom: 15px; }
        .form-row { display: flex; flex-wrap: wrap; gap: 20px; }
        .form-col { flex: 1; min-width: 200px; }
        .btn-group { margin-top: 20px; }
        .personel-image { max-width: 100px; max-height: 130px; margin: 10px 0; }
        .grid-container { margin-top: 20px; }
        .alert { padding: 10px; margin-bottom: 15px; border-radius: 4px; }
        .alert-danger { color: #721c24; background-color: #f8d7da; border-color: #f5c6cb; }
        .alert-info { color: #0c5460; background-color: #d1ecf1; border-color: #bee5eb; }
    </style>
</asp:Content>

<asp:Content ID="BreadcrumbContent" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <!-- Breadcrumb içeriği buraya gelecek -->
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <asp:Panel ID="MesajPaneli" runat="server" CssClass="alert" Visible="false">
        <asp:Label ID="MesajLabel" runat="server"></asp:Label>
    </asp:Panel>
    
    <div class="form-section">
        <div class="card">
            <div class="card-header">
                <h5>Personel Bilgileri</h5>
            </div>
            <div class="card-body">
                <div class="form-row">
                    <div class="form-col">
                        <div class="form-group">
                            <label>Sicil No</label>
                            <asp:TextBox ID="SicilNoMetinKutusu" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="SicilNoMetinKutusu_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="SicilNoGerekliValidator" runat="server" ControlToValidate="SicilNoMetinKutusu" ErrorMessage="Sicil No boş olamaz." CssClass="text-danger" ValidationGroup="Kayit" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="form-group">
                            <label>TC Kimlik No</label>
                            <asp:TextBox ID="TcKimlikNoMetinKutusu" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                
                <div class="form-row">
                    <div class="form-col">
                        <div class="form-group">
                            <label>Adı Soyadı</label>
                            <asp:TextBox ID="AdiSoyadiMetinKutusu" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="form-group">
                            <label>Unvan</label>
                            <asp:TextBox ID="UnvanMetinKutusu" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                
                <div class="form-row">
                    <div class="form-col">
                        <div class="form-group">
                            <label>Çalıştığı Birim</label>
                            <asp:TextBox ID="BirimMetinKutusu" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="form-group">
                            <label>Statü</label>
                            <asp:TextBox ID="StatuMetinKutusu" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="form-section">
        <div class="card">
            <div class="card-header">
                <h5>İzin Bilgileri</h5>
            </div>
            <div class="card-body">
                <div class="form-row">
                    <div class="form-col">
                        <div class="form-group">
                            <label>Devreden İzin Gün</label>
                            <asp:TextBox ID="DevredenIzinMetinKutusu" runat="server" CssClass="form-control" TextMode="Number" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="form-group">
                            <label>Cari Yıl Kalan İzni</label>
                            <asp:TextBox ID="CariIzinMetinKutusu" runat="server" CssClass="form-control" TextMode="Number" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                
                <div class="form-row">
                    <div class="form-col">
                        <div class="form-group">
                            <label>Toplam İzin Süresi</label>
                            <asp:TextBox ID="ToplamYillikIzinMetinKutusu" runat="server" CssClass="form-control" TextMode="Number" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="form-group">
                            <label>İzin Türü</label>
                            <asp:DropDownList ID="IzinTuruAciklamaListesi" runat="server" CssClass="form-select">
                                <asp:ListItem Value="">Seçiniz...</asp:ListItem>
                                <asp:ListItem Value="Yıllık İzin">Yıllık İzin</asp:ListItem>
                                <asp:ListItem Value="İdari İzin">İdari İzin</asp:ListItem>
                                <asp:ListItem Value="Rapor">Rapor</asp:ListItem>
                                <asp:ListItem Value="Mazeret İzni">Mazeret İzni</asp:ListItem>
                                <asp:ListItem Value="Fazla Mesai İzni">Fazla Mesai İzni</asp:ListItem>
                                <asp:ListItem Value="Hafta Tatili İzni">Hafta Tatili İzni</asp:ListItem>
                                <asp:ListItem Value="Saatlik izin">Saatlik İzin</asp:ListItem>
                                <asp:ListItem Value="Hastane İzni">Hastane İzni</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="IzinTuruGerekliValidator" runat="server" ControlToValidate="IzinTuruAciklamaListesi" ErrorMessage="İzin Türü seçiniz." CssClass="text-danger" InitialValue="" ValidationGroup="Kayit" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                
                <div class="form-row">
                    <div class="form-col">
                        <div class="form-group">
                            <label>İzne Başlama Tarihi</label>
                            <asp:TextBox ID="IzneBaslamaTarihiMetinKutusu" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="IzneBaslamaTarihiGerekliValidator" runat="server" ControlToValidate="IzneBaslamaTarihiMetinKutusu" ErrorMessage="İzne Başlama Tarihi seçiniz." CssClass="text-danger" ValidationGroup="Kayit" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="form-group">
                            <label>Kullanılacak İzin Süresi</label>
                            <asp:TextBox ID="IzinSuresiMetinKutusu" runat="server" CssClass="form-control" TextMode="Number" AutoPostBack="true" OnTextChanged="IzinSuresiMetinKutusu_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="IzinSuresiGerekliValidator" runat="server" ControlToValidate="IzinSuresiMetinKutusu" ErrorMessage="İzin Süresi boş olamaz." CssClass="text-danger" ValidationGroup="Kayit" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                
                <div class="form-row">
                    <div class="form-col">
                        <div class="form-group">
                            <label>İzin Bitiş Tarihi</label>
                            <asp:TextBox ID="IzinBitisTarihiMetinKutusu" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="form-group">
                            <label>Göreve Başlama Tarihi</label>
                            <asp:TextBox ID="GoreveBaslamaTarihiMetinKutusu" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                
                <div class="form-row">
                    <div class="form-col">
                        <div class="form-group">
                            <label>Açıklama</label>
                            <asp:TextBox ID="AciklamaMetinKutusu" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="btn-group">
        <asp:Button ID="EkleButonu" runat="server" Text="Ekle" CssClass="btn btn-primary" OnClick="EkleButonu_Click" ValidationGroup="Kayit" />
        <asp:Button ID="GuncelleButonu" runat="server" Text="Güncelle" CssClass="btn btn-warning" OnClick="GuncelleButonu_Click" Visible="false" ValidationGroup="Kayit" />
        <asp:Button ID="VazgecButonu" runat="server" Text="Vazgeç" CssClass="btn btn-secondary" OnClick="VazgecButonu_Click" Visible="false" />
        <asp:Button ID="SilButonu" runat="server" Text="İzin Sil" CssClass="btn btn-danger" OnClick="SilButonu_Click" Visible="false" />
    </div>
    
    <div class="form-section">
        <div class="card">
            <div class="card-body text-center">
                <asp:Image ID="PersonelResim" runat="server" CssClass="personel-image" Visible="false" />
                <br />
                <asp:Label ID="AdiSoyadiLabel" runat="server" CssClass="fw-bold" Visible="false"></asp:Label>
            </div>
        </div>
    </div>
    
    <div class="form-section">
        <rsweb:ReportViewer ID="RaporGörüntüleyici" runat="server" Visible="false" Height="400px" Width="100%"></rsweb:ReportViewer>
    </div>
    
    <div class="grid-container">
        <asp:GridView ID="IzinGridView" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="false" OnSelectedIndexChanged="IzinGridView_SelectedIndexChanged" DataKeyNames="id" Visible="false">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                <asp:BoundField DataField="SicilNo" HeaderText="Sicil No" />
                <asp:BoundField DataField="Adi_Soyadi" HeaderText="Adı Soyadı" />
                <asp:BoundField DataField="Statu" HeaderText="Statü" />
                <asp:BoundField DataField="Devreden_izin" HeaderText="Devreden İzin" DataFormatString="{0:F1}" />
                <asp:BoundField DataField="Cari_izin" HeaderText="Cari İzin" DataFormatString="{0:F1}" />
                <asp:BoundField DataField="izin_turu" HeaderText="İzin Türü" />
                <asp:BoundField DataField="izin_Suresi" HeaderText="İzin Süresi" DataFormatString="{0:F1}" />
                <asp:BoundField DataField="izne_Baslama_Tarihi" HeaderText="Başlangıç Tarihi" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="izin_Bitis_Tarihi" HeaderText="Bitiş Tarihi" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="Goreve_Baslama_Tarihi" HeaderText="Göreve Dönüş" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" />
                <asp:BoundField DataField="Kayit_Tarihi" HeaderText="Kayıt Tarihi" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                <asp:CommandField ShowSelectButton="true" ButtonType="Button" SelectText="Seç" />
            </Columns>
            <EmptyDataTemplate>
                <p class="text-muted">Henüz izin kaydı bulunmamaktadır.</p>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
    
</asp:Content>