<%@ Page Title="Personel Arama" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="Ara.aspx.cs" Inherits="Portal.ModulPersonel.Ara" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item"><a href="Anasayfa.aspx">Ana Sayfa</a></li>
    <li class="breadcrumb-item active" aria-current="page">Personel Arama</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header bg-primary text-white">
                    <h5 class="mb-0"><i class="fas fa-search me-2"></i>Personel Arama Kriterleri</h5>
                </div>
                <div class="card-body">
                    <asp:Panel ID="pnlSearch" runat="server" CssClass="search-form">
                        <div class="row">
                            <div class="col-md-3">
                                <label class="form-label" for="<%=txtSicilNo.ClientID%>">Sicil No</label>
                                <asp:TextBox ID="txtSicilNo" runat="server" CssClass="form-control" placeholder="Sicil No giriniz"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label" for="<%=txtTcKimlikNo.ClientID%>">TC Kimlik No</label>
                                <asp:TextBox ID="txtTcKimlikNo" runat="server" CssClass="form-control" TextMode="Number" placeholder="TC Kimlik No giriniz"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label" for="<%=txtAdi.ClientID%>">Adı</label>
                                <asp:TextBox ID="txtAdi" runat="server" CssClass="form-control" placeholder="Ad giriniz"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label" for="<%=txtSoyad.ClientID%>">Soyadı</label>
                                <asp:TextBox ID="txtSoyad" runat="server" CssClass="form-control" placeholder="Soyad giriniz"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <label class="form-label" for="<%=ddlUnvan.ClientID%>">Unvan</label>
                                <asp:DropDownList ID="ddlUnvan" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                    <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label" for="<%=ddlMeslekiUnvan.ClientID%>">Mesleki Unvan</label>
                                <asp:DropDownList ID="ddlMeslekiUnvan" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                    <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label" for="<%=ddlStatu.ClientID%>">Statü</label>
                                <asp:DropDownList ID="ddlStatu" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                    <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label" for="<%=ddlCalismaDurumu.ClientID%>">Çalışma Durumu</label>
                                <asp:DropDownList ID="ddlCalismaDurumu" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                    <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <label class="form-label" for="<%=ddlOgrenim.ClientID%>">Öğrenim Durumu</label>
                                <asp:DropDownList ID="ddlOgrenim" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                    <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label" for="<%=ddlKanGrubu.ClientID%>">Kan Grubu</label>
                                <asp:DropDownList ID="ddlKanGrubu" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                    <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label" for="<%=ddlMedeniHal.ClientID%>">Medeni Hal</label>
                                <asp:DropDownList ID="ddlMedeniHal" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                    <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label" for="<%=ddlCinsiyet.ClientID%>">Cinsiyet</label>
                                <asp:DropDownList ID="ddlCinsiyet" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                    <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <label class="form-label" for="<%=ddlSendika.ClientID%>">Sendika</label>
                                <asp:DropDownList ID="ddlSendika" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                    <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label" for="<%=ddlBirim.ClientID%>">Birim</label>
                                <asp:DropDownList ID="ddlBirim" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                    <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label" for="<%=ddlDurum.ClientID%>">Durum</label>
                                <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                    <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label" for="<%=txtTarih.ClientID%>">Tarih (Kurumda Çalıştığı Tarih)</label>
                                <asp:TextBox ID="txtTarih" runat="server" CssClass="form-control" TextMode="Date" placeholder="GG/AA/YYYY"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12 text-end">
                                <asp:Button ID="btnAra" runat="server" Text="Ara" CssClass="btn btn-search me-2" OnClick="btnAra_Click" />
                                <asp:Button ID="btnTemizle" runat="server" Text="Temizle" CssClass="btn btn-secondary" OnClick="btnTemizle_Click" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-12">
            <div class="card stat-label d-flex justify-content-between align-items-center">
                <span>Aktif Personel Sayısı: <asp:Label ID="lblToplamSayi" runat="server" CssClass="badge bg-primary ms-2"></asp:Label></span>
                <asp:Button ID="btnExport" runat="server" Text="Excel'e Aktar" CssClass="btn btn-outline-primary" OnClick="btnExport_Click" />
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header bg-info text-white">
                    <h5 class="mb-0"><i class="fas fa-list me-2"></i>Arama Sonuçları <span class="badge bg-light text-dark ms-2">Bulunan: <asp:Label ID="lblBulunanSayi" runat="server"></asp:Label></span></h5>
                </div>
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <asp:GridView ID="gvPersonelAra" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover mb-0" ShowHeaderWhenEmpty="True" EmptyDataText="Arama kriterlerine uygun kayıt bulunamadı.">
                            <Columns>                                
                                <asp:BoundField DataField="TcKimlikNo" HeaderText="TC No" />
                                <asp:BoundField DataField="Adi" HeaderText="Adı" />
                                <asp:BoundField DataField="Soyad" HeaderText="Soyadı" />
                                <asp:BoundField DataField="Unvan" HeaderText="Unvanı" />
                                <asp:BoundField DataField="KadroDerece" HeaderText="Kadro Derece" />
                                <asp:BoundField DataField="MeslekiUnvan" HeaderText="Mesleki Unvanı" />
                                <asp:BoundField DataField="Ogrenim_Durumu" HeaderText="Öğrenim Durumu" />
                                <asp:BoundField DataField="Statu" HeaderText="Statü" />
                                <asp:BoundField DataField="SicilNo" HeaderText="Sicil No" />
                                <asp:BoundField DataField="GorevYaptigiBirim" HeaderText="Birimi" />
                                <asp:BoundField DataField="CalismaDurumu" HeaderText="Çalışma Durumu" />
                                <asp:BoundField DataField="GeciciGelenPersonelKurumu" HeaderText="Geç. Gelen Per. Kurumu" />
                                <asp:BoundField DataField="GeciciGidenPersonelKurumu" HeaderText="Geç. Gittiği Kurum" />
                                <asp:BoundField DataField="DogumYeri" HeaderText="Doğum Yeri" />
                                <asp:BoundField DataField="DogumTarihi" HeaderText="Doğum Tarihi" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="ilkisegiristarihi" HeaderText="İlk İşe Giriş Tarihi" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="KurumaBaslamaTarihi" HeaderText="Kuruma Başlangıç Tarihi" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="istenAyrilisTarihi" HeaderText="İşten Ayrılış Tarihi" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="istenAyrilmaSebebi" HeaderText="İşten Ayrılış Sebebi" />
                                <asp:BoundField DataField="GGorevBaslangicTarihi" HeaderText="Geç. Görev Baş. Tarihi" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="GGorevBitisTarihi" HeaderText="Geç. Görev Bit. Tarihi" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="CepTelefonu" HeaderText="Cep Telefonu" />
                                <asp:BoundField DataField="MailAdresi" HeaderText="E-Posta" />
                                <asp:BoundField DataField="EvTelefonu" HeaderText="Ev Telefonu" />
                                <asp:BoundField DataField="Adres" HeaderText="Adres" />
                                <asp:BoundField DataField="AcilDurumdaAranacakKisi" HeaderText="Yakını" />
                                <asp:BoundField DataField="AcilCep" HeaderText="Acil Durumda Aranacak Cep" />
                                <asp:BoundField DataField="KanGrubu" HeaderText="Kan Grubu" />
                                <asp:BoundField DataField="MedeniHali" HeaderText="Medeni Hali" />
                                <asp:BoundField DataField="AskerlikDurumu" HeaderText="Askerlik Durumu" />
                                <asp:BoundField DataField="Sendika" HeaderText="Sendika" />
                                <asp:BoundField DataField="EngelDurumu" HeaderText="Engel Durumu" />
                                <asp:BoundField DataField="EngelAciklama" HeaderText="Engel Açıklama" />
                                <asp:BoundField DataField="Durum" HeaderText="Durum" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>