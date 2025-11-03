<%@ Page Title="İşletme Denetim Girişi" Language="C#" MasterPageFile="~/AnaV2.Master" 
    AutoEventWireup="true" CodeBehind="IsletmeGiris.aspx.cs" 
    Inherits="Portal.ModulDenetim.IsletmeGiris" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="~/DENETIMMODUL.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">
        
        <!-- Başlık -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="d-flex justify-content-between align-items-center">
                    <h2 class="text-primary mb-0">
                        <i class="fas fa-building me-2"></i>İşletme Denetim Girişi
                    </h2>
                </div>
            </div>
        </div>

        <!-- Kayıt Arama Kartı -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="card denetim-filter-card">
                    <div class="card-header">
                        <i class="fas fa-search me-2"></i>Kayıt Arama
                    </div>
                    <div class="card-body">
                        <div class="row g-3 align-items-end">
                            <div class="col-md-8">
                                <label class="form-label fw-bold">Kayıt No</label>
                                <asp:TextBox ID="txtKayitNo" runat="server" CssClass="form-control" 
                                    TextMode="Number" placeholder="Kayıt numarası giriniz..."></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:Button ID="btnBul" runat="server" CssClass="btn btn-denetim-filter w-100" 
                                    Text="🔍 Bul" OnClick="btnBul_Click" CausesValidation="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Form Kartı -->
        <div class="row">
            <div class="col-12">
                <div class="card denetim-result-card">
                    <div class="denetim-result-header">
                        <h5 class="denetim-result-title">Denetim Bilgileri</h5>
                    </div>
                    <div class="card-body p-4">
                        
                        <!-- Firma Bilgileri -->
                        <div class="row mb-4">
                            <div class="col-12">
                                <h6 class="text-primary border-bottom pb-2 mb-3">
                                    <i class="fas fa-building me-2"></i>Firma Bilgileri
                                </h6>
                            </div>
                            
                            <div class="col-md-6 mb-3">
                                <label class="form-label fw-bold">Vergi No <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtVergiNo" runat="server" CssClass="form-control" 
                                    TextMode="Number" placeholder="Vergi numarası giriniz..."></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvVergiNo" runat="server" 
                                    ControlToValidate="txtVergiNo" ErrorMessage="Vergi numarası zorunludur." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label fw-bold">Yetki Belgesi <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlYetkiBelgesi" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvYetkiBelgesi" runat="server" 
                                    ControlToValidate="ddlYetkiBelgesi" InitialValue="" 
                                    ErrorMessage="Yetki belgesi seçiniz." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                            </div>

                            <div class="col-12 mb-3">
                                <label class="form-label fw-bold">Firma Unvanı <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtUnvan" runat="server" CssClass="form-control" 
                                    placeholder="Firma unvanı giriniz..."></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvUnvan" runat="server" 
                                    ControlToValidate="txtUnvan" ErrorMessage="Firma unvanı zorunludur." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                            </div>

                            <div class="col-12 mb-3">
                                <label class="form-label fw-bold">Firma Adresi <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtAdres" runat="server" CssClass="form-control" 
                                    TextMode="MultiLine" Rows="2" 
                                    placeholder="Firma adresi giriniz..."></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAdres" runat="server" 
                                    ControlToValidate="txtAdres" ErrorMessage="Firma adresi zorunludur." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <!-- Denetim Bilgileri -->
                        <div class="row mb-4">
                            <div class="col-12">
                                <h6 class="text-primary border-bottom pb-2 mb-3">
                                    <i class="fas fa-clipboard-check me-2"></i>Denetim Bilgileri
                                </h6>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label fw-bold">Denetim Türü <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlDenetimTuru" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="">Seçiniz...</asp:ListItem>
                                    <asp:ListItem Value="Eşya Taşımacılığı">Eşya Taşımacılığı</asp:ListItem>
                                    <asp:ListItem Value="Yolcu Taşımacılığı">Yolcu Taşımacılığı</asp:ListItem>
                                    <asp:ListItem Value="Tehlikeli Madde">Tehlikeli Madde</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvDenetimTuru" runat="server" 
                                    ControlToValidate="ddlDenetimTuru" InitialValue="" 
                                    ErrorMessage="Denetim türü seçiniz." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label fw-bold">Denetim Tarihi <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtDenetimTarihi" runat="server" CssClass="form-control" 
                                    TextMode="DateTimeLocal"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDenetimTarihi" runat="server" 
                                    ControlToValidate="txtDenetimTarihi" ErrorMessage="Denetim tarihi zorunludur." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label fw-bold">İl <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlIl" runat="server" CssClass="form-select" 
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlIl_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvIl" runat="server" 
                                    ControlToValidate="ddlIl" InitialValue="" 
                                    ErrorMessage="İl seçiniz." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label fw-bold">İlçe <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlIlce" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvIlce" runat="server" 
                                    ControlToValidate="ddlIlce" InitialValue="" 
                                    ErrorMessage="İlçe seçiniz." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label fw-bold">Denetim Personeli 1 <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlPersonel1" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPersonel1" runat="server" 
                                    ControlToValidate="ddlPersonel1" InitialValue="" 
                                    ErrorMessage="En az 1 personel seçmelisiniz." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label fw-bold">Denetim Personeli 2</label>
                                <asp:DropDownList ID="ddlPersonel2" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label fw-bold">Ceza Durumu <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlCezaDurumu" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="">Seçiniz...</asp:ListItem>
                                    <asp:ListItem Value="Para Cezası">Para Cezası</asp:ListItem>
                                    <asp:ListItem Value="Para + Uyarı Cezası">Para + Uyarı Cezası</asp:ListItem>
                                    <asp:ListItem Value="Uyarı Cezası">Uyarı Cezası</asp:ListItem>
                                    <asp:ListItem Value="Yok">Yok</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvCezaDurumu" runat="server" 
                                    ControlToValidate="ddlCezaDurumu" InitialValue="" 
                                    ErrorMessage="Ceza durumu seçiniz." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                            </div>

                            <div class="col-12 mb-3">
                                <label class="form-label fw-bold">Açıklama</label>
                                <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" 
                                    TextMode="MultiLine" Rows="4" 
                                    placeholder="Denetim ile ilgili açıklama giriniz..."></asp:TextBox>
                            </div>
                        </div>

                        <!-- Validation Summary -->
                        <div class="row mb-3">
                            <div class="col-12">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                    CssClass="alert alert-danger" ValidationGroup="kayit" 
                                    HeaderText="Lütfen aşağıdaki alanları kontrol ediniz:" />
                            </div>
                        </div>

                        <!-- Action Buttons -->
                        <div class="row">
                            <div class="col-12">
                                <div class="d-flex gap-2 flex-wrap">
                                    <asp:Button ID="btnKaydet" runat="server" CssClass="btn btn-success px-4" 
                                        Text="💾 Kaydet" OnClick="btnKaydet_Click" ValidationGroup="kayit" />
                                    
                                    <asp:Button ID="btnGuncelle" runat="server" CssClass="btn btn-primary px-4" 
                                        Text="✏️ Güncelle" OnClick="btnGuncelle_Click" 
                                        ValidationGroup="kayit" Visible="false" />
                                    
                                    <asp:Button ID="btnVazgec" runat="server" CssClass="btn btn-secondary px-4" 
                                        Text="↩️ Vazgeç" OnClick="btnVazgec_Click" 
                                        CausesValidation="false" Visible="false" />
                                    
                                    <asp:Button ID="btnSil" runat="server" CssClass="btn btn-danger px-4" 
                                        Text="🗑️ Sil" OnClick="btnSil_Click" 
                                        CausesValidation="false" Visible="false"
                                        OnClientClick="return confirm('Bu kaydı silmek istediğinizden emin misiniz?');" />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>