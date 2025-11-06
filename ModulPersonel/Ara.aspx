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
                                <asp:TextBox ID="txtTarih" runat="server" CssClass="form-control fp-date" placeholder="GG/AA/YYYY"></asp:TextBox>
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
                        <asp:GridView ID="gvPersonelAra" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover mb-0" 
                            ShowHeaderWhenEmpty="True" EmptyDataText="Arama kriterlerine uygun kayıt bulunamadı." 
                            OnRowCommand="gvPersonelAra_RowCommand" DataKeyNames="Personelid">
                            <Columns>                                
                                <asp:TemplateField HeaderText="İşlem">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnGuncelle" runat="server" CssClass="btn btn-sm btn-primary" 
                                            CommandName="Guncelle" CommandArgument='<%# Eval("Personelid") %>' ToolTip="Güncelle">
                                            <i class="fas fa-edit"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
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

    <!-- ==> Modal Popup - Personel Güncelleme -->
    <div class="modal fade" id="modalGuncelle" tabindex="-1" aria-labelledby="modalGuncelleLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="modalGuncelleLabel"><i class="fas fa-user-edit me-2"></i>Personel Bilgilerini Güncelle</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Kapat"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfPersonelId" runat="server" />
                    
                    <div class="row g-3">
                        <!-- Kişisel Bilgiler -->
                        <div class="col-12">
                            <h6 class="border-bottom pb-2 mb-3"><i class="fas fa-user me-2"></i>Kişisel Bilgiler</h6>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">TC Kimlik No <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtModalTcNo" runat="server" CssClass="form-control" MaxLength="11"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">Adı <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtModalAdi" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">Soyadı <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtModalSoyadi" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        
                        <!-- Görev Bilgileri -->
                        <div class="col-12">
                            <h6 class="border-bottom pb-2 mb-3 mt-3"><i class="fas fa-briefcase me-2"></i>Görev Bilgileri</h6>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Sicil No</label>
                            <asp:TextBox ID="txtModalSicilNo" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Unvan</label>
                            <asp:DropDownList ID="ddlModalUnvan" runat="server" CssClass="form-select"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Mesleki Unvan</label>
                            <asp:TextBox ID="txtModalMeslekiUnvan" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Kadro Derece</label>
                            <asp:TextBox ID="txtModalKadroDerece" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        
                        <!-- İletişim Bilgileri -->
                        <div class="col-12">
                            <h6 class="border-bottom pb-2 mb-3 mt-3"><i class="fas fa-phone me-2"></i>İletişim Bilgileri</h6>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">Cep Telefonu</label>
                            <asp:TextBox ID="txtModalCepTel" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">E-Posta</label>
                            <asp:TextBox ID="txtModalMail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">Ev Telefonu</label>
                            <asp:TextBox ID="txtModalEvTel" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-12">
                            <label class="form-label">Adres</label>
                            <asp:TextBox ID="txtModalAdres" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                        <i class="fas fa-times me-1"></i>Kapat
                    </button>
                    <asp:Button ID="btnModalKaydet" runat="server" Text="Kaydet" CssClass="btn btn-primary" OnClick="btnModalKaydet_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- ==> Modal açma için JavaScript -->
    <script type="text/javascript">
        function openModal() {
            var modal = new bootstrap.Modal(document.getElementById('modalGuncelle'));
            modal.show();
        }
    </script>
</asp:Content>