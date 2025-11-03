<%@ Page Title="Taşıt Denetim Girişi" Language="C#" MasterPageFile="~/AnaV2.Master" 
    AutoEventWireup="true" CodeBehind="TasitGiris.aspx.cs" 
    Inherits="ModulDenetim.TasitGiris" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-section {
            background: white;
            border-radius: 10px;
            padding: 25px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            margin-bottom: 20px;
        }
        .section-title {
            color: #2E5B9A;
            font-weight: 600;
            margin-bottom: 20px;
            padding-bottom: 10px;
            border-bottom: 2px solid #4B7BEC;
        }
        .required-field::after {
            content: " *";
            color: #dc3545;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item"><a href="/Anasayfa.aspx">Ana Sayfa</a></li>
    <li class="breadcrumb-item">Denetim Takip</li>
    <li class="breadcrumb-item active">Taşıt Denetim Girişi</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-12">
            <div class="card shadow-custom-md">
                <div class="card-header">
                    <h5 class="mb-0">
                        <i class="fas fa-truck me-2"></i>Taşıt Denetim Girişi
                    </h5>
                </div>
                <div class="card-body">
                    
                    <!-- Kayıt Arama Bölümü -->
                    <div class="form-section">
                        <h6 class="section-title">
                            <i class="fas fa-search me-2"></i>Kayıt Arama
                        </h6>
                        <div class="row align-items-end">
                            <div class="col-md-4">
                                <label class="form-label">Kayıt No</label>
                                <asp:TextBox ID="txtKayitNo" runat="server" CssClass="form-control" 
                                    TextMode="Number" placeholder="Kayıt aramak için kayıt numarası giriniz...">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:Button ID="btnBul" runat="server" Text="🔍 Bul" 
                                    CssClass="btn btn-primary" OnClick="btnBul_Click" 
                                    CausesValidation="false" />
                                <asp:Label ID="lblBulunanKayit" runat="server" CssClass="ms-2 text-danger"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <!-- Taşıt Bilgileri Formu -->
                    <div class="form-section">
                        <h6 class="section-title">
                            <i class="fas fa-car me-2"></i>Taşıt Bilgileri
                        </h6>
                        
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label class="form-label required-field">Plaka No</label>
                                <asp:TextBox ID="txtPlaka" runat="server" CssClass="form-control text-uppercase" 
                                    placeholder="Örn: 06UAB1989" MaxLength="20">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPlaka" runat="server" 
                                    ControlToValidate="txtPlaka" ErrorMessage="Plaka alanı zorunludur." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Yarı Römork Plakası (varsa)</label>
                                <asp:TextBox ID="txtPlaka2" runat="server" CssClass="form-control text-uppercase" 
                                    placeholder="Yarı römork plakası" MaxLength="20">
                                </asp:TextBox>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-12">
                                <label class="form-label required-field">Taşıt/Yetki Belgesi Unvan</label>
                                <asp:TextBox ID="txtUnvan" runat="server" CssClass="form-control" 
                                    placeholder="Firma unvanı" MaxLength="250">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvUnvan" runat="server" 
                                    ControlToValidate="txtUnvan" ErrorMessage="Unvan alanı zorunludur." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label class="form-label required-field">Denetim Yeri</label>
                                <asp:DropDownList ID="ddlDenetimYeri" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvDenetimYeri" runat="server" 
                                    ControlToValidate="ddlDenetimYeri" InitialValue="" 
                                    ErrorMessage="Denetim yeri seçimi zorunludur." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Yetki Belgesi</label>
                                <asp:DropDownList ID="ddlYetkiBelgesi" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label class="form-label required-field">Denetim Türü</label>
                                <asp:DropDownList ID="ddlDenetimTuru" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="">Seçiniz...</asp:ListItem>
                                    <asp:ListItem>Eşya Taşımacılığı</asp:ListItem>
                                    <asp:ListItem>Yolcu Taşımacılığı</asp:ListItem>
                                    <asp:ListItem>Tehlikeli Madde</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvDenetimTuru" runat="server" 
                                    ControlToValidate="ddlDenetimTuru" InitialValue="" 
                                    ErrorMessage="Denetim türü seçimi zorunludur." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label required-field">Denetim Tarihi</label>
                                <asp:TextBox ID="txtDenetimTarihi" runat="server" CssClass="form-control" 
                                    TextMode="DateTimeLocal">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDenetimTarihi" runat="server" 
                                    ControlToValidate="txtDenetimTarihi" ErrorMessage="Denetim tarihi zorunludur." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label class="form-label required-field">Denetim İl</label>
                                <asp:DropDownList ID="ddlIl" runat="server" CssClass="form-select" 
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlIl_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvIl" runat="server" 
                                    ControlToValidate="ddlIl" InitialValue="" 
                                    ErrorMessage="İl seçimi zorunludur." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label required-field">Denetim İlçe</label>
                                <asp:DropDownList ID="ddlIlce" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvIlce" runat="server" 
                                    ControlToValidate="ddlIlce" InitialValue="" 
                                    ErrorMessage="İlçe seçimi zorunludur." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label class="form-label required-field">Denetleyen Personel</label>
                                <asp:DropDownList ID="ddlPersonel" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPersonel" runat="server" 
                                    ControlToValidate="ddlPersonel" InitialValue="" 
                                    ErrorMessage="Personel seçimi zorunludur." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label required-field">Ceza Durumu</label>
                                <asp:DropDownList ID="ddlCezaDurumu" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="">Seçiniz...</asp:ListItem>
                                    <asp:ListItem>Para Cezası</asp:ListItem>
                                    <asp:ListItem>Para + Uyarı Cezası</asp:ListItem>
                                    <asp:ListItem>Uyarı Cezası</asp:ListItem>
                                    <asp:ListItem>Yok</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvCezaDurumu" runat="server" 
                                    ControlToValidate="ddlCezaDurumu" InitialValue="" 
                                    ErrorMessage="Ceza durumu seçimi zorunludur." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-12">
                                <label class="form-label">Açıklama</label>
                                <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" 
                                    TextMode="MultiLine" Rows="4" placeholder="Denetim ile ilgili açıklama...">
                                </asp:TextBox>
                            </div>
                        </div>

                        <!-- Mesaj Gösterimi -->
                        <div class="row mb-3">
                            <div class="col-md-12">
                                <asp:Label ID="lblMesaj" runat="server" CssClass="text-danger fw-bold"></asp:Label>
                            </div>
                        </div>

                        <!-- İşlem Butonları -->
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnKaydet" runat="server" Text="💾 Kaydet" 
                                    CssClass="btn btn-primary btn-lg" OnClick="btnKaydet_Click" 
                                    ValidationGroup="kayit" />
                                
                                <asp:Button ID="btnGuncelle" runat="server" Text="✏️ Güncelle" 
                                    CssClass="btn btn-warning btn-lg" OnClick="btnGuncelle_Click" 
                                    ValidationGroup="kayit" Visible="false" />
                                
                                <asp:Button ID="btnVazgec" runat="server" Text="❌ Vazgeç" 
                                    CssClass="btn btn-secondary btn-lg" OnClick="btnVazgec_Click" 
                                    CausesValidation="false" Visible="false" />
                                
                                <asp:Button ID="btnSil" runat="server" Text="🗑️ Sil" 
                                    CssClass="btn btn-danger btn-lg" OnClick="btnSil_Click" 
                                    CausesValidation="false" Visible="false" 
                                    OnClientClick="return confirm('Bu kaydı silmek istediğinizden emin misiniz?');" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>