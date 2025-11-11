<%@ Page Title="Uzaktan Denetim Kayıt" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="UzakEkle.aspx.cs" Inherits="Portal.ModulDenetim.UzakEkle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="~/wwwroot/css/DENETIMMODUL.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid py-4">

        <!-- Sayfa Başlığı -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="d-flex align-items-center">
                    <i class="fas fa-desktop fa-2x text-primary me-3"></i>
                    <div>
                        <h2 class="mb-0">Uzaktan Denetim Kayıt</h2>
                        <p class="text-muted mb-0">Uzaktan denetim kaydı ekleyin veya güncelleyin</p>
                    </div>
                </div>
            </div>
        </div>

        <!-- Kayıt Arama Kartı -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="card denetim-filter-card">
                    <div class="card-header">
                        <i class="fas fa-search me-2"></i>Kayıt Ara
                   
                    </div>
                    <div class="card-body">
                        <div class="row align-items-end">
                            <div class="col-md-4">
                                <label class="form-label fw-semibold">Kayıt ID</label>
                                <asp:TextBox ID="txtKayitBul" runat="server" CssClass="form-control"
                                    placeholder="Kayıt ID giriniz"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvKayitBul" runat="server"
                                    ControlToValidate="txtKayitBul" Display="Dynamic"
                                    ErrorMessage="Kayıt ID zorunludur" CssClass="text-danger small"
                                    ValidationGroup="AramaGrup">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4">
                                <asp:Button ID="btnBul" runat="server" CssClass="btn btn-primary px-4"
                                    OnClick="btnBul_Click" Text="🔍 Ara" ValidationGroup="AramaGrup" />
                                <asp:Button ID="btnVazgec" runat="server" CssClass="btn btn-secondary px-4 ms-2"
                                    OnClick="btnVazgec_Click" Visible="false" Text="↩️ Vazgeç" />
                            </div>
                            <div class="col-md-4 text-end">
                                <asp:Label ID="lblHata" runat="server" CssClass="text-danger fw-bold"></asp:Label>
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
                        <h5 class="denetim-result-title mb-0">
                            <i class="fas fa-edit me-2"></i>Denetim Bilgileri
                        </h5>
                    </div>
                    <div class="card-body p-4">
                        <div class="row g-3">

                            <!-- Tarih -->
                            <div class="col-md-4">
                                <label class="form-label fw-semibold">
                                    <i class="fas fa-calendar text-primary me-1"></i>Tarih
                                   
                                    <span class="text-danger">*</span>
                                </label>
                                <asp:TextBox ID="txtTarih" runat="server" CssClass="form-control fp-date"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTarih" runat="server"
                                    ControlToValidate="txtTarih" Display="Dynamic"
                                    ErrorMessage="Tarih zorunludur" CssClass="text-danger small"
                                    ValidationGroup="FormGrup"> 
                                </asp:RequiredFieldValidator>
                            </div>

                            <!-- Araç Sayısı -->
                            <div class="col-md-4">
                                <label class="form-label fw-semibold">
                                    <i class="fas fa-car text-primary me-1"></i>Araç Sayısı
                                   
                                    <span class="text-danger">*</span>
                                </label>
                                <asp:TextBox ID="txtAracSayisi" runat="server" CssClass="form-control"
                                    TextMode="Number" placeholder="0"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAracSayisi" runat="server"
                                    ControlToValidate="txtAracSayisi" Display="Dynamic"
                                    ErrorMessage="Araç sayısı zorunludur" CssClass="text-danger small"
                                    ValidationGroup="FormGrup">
                                </asp:RequiredFieldValidator>
                            </div>

                            <!-- Atanan Personel -->
                            <div class="col-md-4">
                                <label class="form-label fw-semibold">
                                    <i class="fas fa-user text-primary me-1"></i>Atanan Personel
                                   
                                    <span class="text-danger">*</span>
                                </label>
                                <asp:DropDownList ID="ddlPersonel" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPersonel" runat="server"
                                    ControlToValidate="ddlPersonel" Display="Dynamic" InitialValue=""
                                    ErrorMessage="Personel seçimi zorunludur" CssClass="text-danger small"
                                    ValidationGroup="FormGrup">
                                </asp:RequiredFieldValidator>
                            </div>

                            <!-- Uygunsuz Araç Sayısı (Readonly - sadece güncelleme modunda görünür) -->
                            <div class="col-md-4" id="divUygunsuzArac" runat="server" visible="false">
                                <label class="form-label fw-semibold">
                                    <i class="fas fa-exclamation-triangle text-warning me-1"></i>Uygunsuz Araç Sayısı
                               
                                </label>
                                <asp:TextBox ID="txtUygunsuzArac" runat="server" CssClass="form-control"
                                    ReadOnly="true" BackColor="#f8f9fa"></asp:TextBox>
                            </div>

                            <!-- Ceza Uygulanan Araç (YB Olmayan) -->
                            <div class="col-md-4" id="divCezaliArac" runat="server" visible="false">
                                <label class="form-label fw-semibold">
                                    <i class="fas fa-gavel text-danger me-1"></i>Ceza Uygulanan Araç
                               
                                </label>
                                <asp:TextBox ID="txtCezaliArac" runat="server" CssClass="form-control"
                                    ReadOnly="true" BackColor="#f8f9fa"></asp:TextBox>
                            </div>

                            <!-- YB Kayıtlı Olmayan -->
                            <div class="col-md-4" id="divYBKayitliOlmayan" runat="server" visible="false">
                                <label class="form-label fw-semibold">
                                    <i class="fas fa-clipboard-list text-danger me-1"></i>YB Kayıtlı Olmayan
                               
                                </label>
                                <asp:TextBox ID="txtYBKayitliOlmayan" runat="server" CssClass="form-control"
                                    ReadOnly="true" BackColor="#f8f9fa"></asp:TextBox>
                            </div>

                            <!-- İşlem Durumu -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">
                                    <i class="fas fa-info-circle text-primary me-1"></i>İşlem Durumu
                                   
                                    <span class="text-danger">*</span>
                                </label>
                                <asp:DropDownList ID="ddlIslemDurum" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="">Seçiniz</asp:ListItem>
                                    <asp:ListItem Value="Açık">Açık</asp:ListItem>
                                    <asp:ListItem Value="Kapalı">Kapalı</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvDurum" runat="server"
                                    ControlToValidate="ddlIslemDurum" Display="Dynamic" InitialValue=""
                                    ErrorMessage="İşlem durumu zorunludur" CssClass="text-danger small"
                                    ValidationGroup="FormGrup">
                                </asp:RequiredFieldValidator>
                            </div>

                            <!-- Açıklama -->
                            <div class="col-md-12">
                                <label class="form-label fw-semibold">
                                    <i class="fas fa-comment text-primary me-1"></i>Açıklama
                               
                                </label>
                                <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control"
                                    TextMode="MultiLine" Rows="3" placeholder="Açıklama giriniz..."></asp:TextBox>
                            </div>

                            <!-- Butonlar -->
                            <div class="col-12 mt-4">
                                <div class="d-flex gap-2">
                                    <asp:Button ID="btnKaydet" runat="server" CssClass="btn btn-success px-4"
                                        OnClick="btnKaydet_Click" Text="💾 Kaydet" ValidationGroup="FormGrup" />

                                    <asp:Button ID="btnGuncelle" runat="server" CssClass="btn btn-primary px-4"
                                        OnClick="btnGuncelle_Click" Visible="false" Text="✏️ Güncelle" ValidationGroup="FormGrup" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
