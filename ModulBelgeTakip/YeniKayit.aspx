<%@ Page Title="Yeni Firma Kaydı" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="YeniKayit.aspx.cs" Inherits="Portal.ModulBelgeTakip.YeniKayit" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="~/wwwroot/css/BELGETAKIPMODUL.css" />    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">
        <div class="card">
            <div class="card-header">
                <h3 class="card-title mb-0">
                    <i class="fas fa-building me-2"></i>Yeni Firma Kaydı ve Denetim
                </h3>
            </div>

            <div class="card-body">
                <!-- Firma Bilgileri Bölümü -->
                <div class="form-section">
                    <h4 class="section-title" style="color: #2E5B9A; border-bottom: 2px solid #4B7BEC; padding-bottom: 8px; margin-bottom: 20px;">
                        <i class="fas fa-building me-2"></i>Firma Bilgileri
                    </h4>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="mb-3">
                                <label class="form-label">
                                    <i class="fas fa-hashtag me-1"></i>Vergi Numarası <span style="color: #dc2626;">*</span>
                                </label>
                                <asp:TextBox ID="txtVergiNo" runat="server" CssClass="form-control"
                                    placeholder="10 veya 11 haneli vergi numarası" MaxLength="11"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvVergiNo" runat="server"
                                    ControlToValidate="txtVergiNo" ErrorMessage="Vergi numarası zorunludur."
                                    CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvVergiNo" runat="server"
                                    ControlToValidate="txtVergiNo"
                                    OnServerValidate="cvVergiNo_ServerValidate"
                                    CssClass="text-danger small" Display="Dynamic"></asp:CustomValidator>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="mb-3">
                                <label class="form-label">
                                    <i class="fas fa-building me-1"></i>Firma Adı <span style="color: #dc2626;">*</span>
                                </label>
                                <asp:TextBox ID="txtFirmaAdi" runat="server" CssClass="form-control"
                                    placeholder="Firma adını giriniz"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFirmaAdi" runat="server"
                                    ControlToValidate="txtFirmaAdi" ErrorMessage="Firma adı zorunludur."
                                    CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-4">
                            <div class="mb-3">
                                <label class="form-label">
                                    <i class="fas fa-map-marker-alt me-1"></i>İl <span style="color: #dc2626;">*</span>
                                </label>
                                <asp:DropDownList ID="ddlIl" runat="server" CssClass="form-select"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlIl_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvIl" runat="server"
                                    ControlToValidate="ddlIl" InitialValue="-1" ErrorMessage="İl seçimi zorunludur."
                                    CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="mb-3">
                                <label class="form-label">
                                    <i class="fas fa-map-marker me-1"></i>İlçe <span style="color: #dc2626;">*</span>
                                </label>
                                <asp:DropDownList ID="ddlIlce" runat="server" CssClass="form-select" Enabled="false">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvIlce" runat="server"
                                    ControlToValidate="ddlIlce" InitialValue="-1" ErrorMessage="İlçe seçimi zorunludur."
                                    CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="mb-3">
                                <label class="form-label">
                                    <i class="fas fa-industry me-1"></i>Firma Tipi <span style="color: #dc2626;">*</span>
                                </label>
                                <asp:DropDownList ID="ddlFirmaTipi" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvFirmaTipi" runat="server"
                                    ControlToValidate="ddlFirmaTipi" InitialValue="-1" ErrorMessage="Firma tipi seçimi zorunludur."
                                    CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12">
                            <div class="mb-3">
                                <label class="form-label">
                                    <i class="fas fa-address-card me-1"></i>Firma Adresi <span style="color: #dc2626;">*</span>
                                </label>
                                <asp:TextBox ID="txtAdres" runat="server" CssClass="form-control"
                                    TextMode="MultiLine" Rows="3" placeholder="Firma adresini giriniz"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAdres" runat="server"
                                    ControlToValidate="txtAdres" ErrorMessage="Firma adresi zorunludur."
                                    CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Denetim Bilgileri Bölümü -->
                <div class="form-section mt-4">
                    <h4 class="section-title" style="color: #2E5B9A; border-bottom: 2px solid #4B7BEC; padding-bottom: 8px; margin-bottom: 20px;">
                        <i class="fas fa-clipboard-check me-2"></i>Denetim Bilgileri
                    </h4>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="mb-3">
                                <label class="form-label">
                                    <i class="fas fa-user me-1"></i>Denetim Personeli 1 <span style="color: #dc2626;">*</span>
                                </label>
                                <asp:DropDownList ID="ddlPersonel1" runat="server" CssClass="form-select"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlPersonel1_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPersonel1" runat="server"
                                    ControlToValidate="ddlPersonel1" InitialValue="-1" ErrorMessage="Personel seçimi zorunludur."
                                    CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="mb-3">
                                <label class="form-label">
                                    <i class="fas fa-user me-1"></i>Denetim Personeli 2 <span style="color: #dc2626;">*</span>
                                </label>
                                <asp:DropDownList ID="ddlPersonel2" runat="server" CssClass="form-select"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlPersonel2_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPersonel2" runat="server"
                                    ControlToValidate="ddlPersonel2" InitialValue="-1" ErrorMessage="Personel seçimi zorunludur."
                                    CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-4">
                            <div class="mb-3">
                                <label class="form-label">
                                    <i class="fas fa-file-alt me-1"></i>Belge Türü <span style="color: #dc2626;">*</span>
                                </label>
                                <asp:DropDownList ID="ddlBelgeTuru" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvBelgeTuru" runat="server"
                                    ControlToValidate="ddlBelgeTuru" InitialValue="-1" ErrorMessage="Belge türü seçimi zorunludur."
                                    CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="mb-3">
                                <label class="form-label">
                                    <i class="fas fa-tags me-1"></i>Kategori <span style="color: #dc2626;">*</span>
                                </label>
                                <asp:DropDownList ID="ddlKategori" runat="server" CssClass="form-select"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlKategori_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvKategori" runat="server"
                                    ControlToValidate="ddlKategori" InitialValue="-1" ErrorMessage="Kategori seçimi zorunludur."
                                    CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="mb-3">
                                <asp:Label ID="lblCezaMakbuzNo" runat="server" CssClass="form-label" Enabled="false">
                                    <i class="fas fa-receipt me-1"></i>Ceza Makbuz No
                                </asp:Label>
                                <asp:TextBox ID="txtCezaMakbuzNo" runat="server" CssClass="form-control"
                                    Enabled="false" placeholder="Makbuz numarasını giriniz"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMakbuz" runat="server"
                                    ControlToValidate="txtCezaMakbuzNo" ErrorMessage="Ceza makbuz numarası zorunludur."
                                    CssClass="text-danger small" Display="Dynamic" Enabled="false"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="mb-3">
                                <label class="form-label">
                                    <i class="fas fa-calendar-alt me-1"></i>Denetim Tarihi <span style="color: #dc2626;">*</span>
                                </label>
                                <asp:TextBox ID="txtDenetimTarihi" runat="server" CssClass="form-control"
                                    placeholder="Tarih seçiniz (gg.aa.yyyy)"></asp:TextBox>
                                <asp:HiddenField ID="hdnDenetimTarihi" runat="server" />
                                <asp:CustomValidator ID="cvDenetimTarihi" runat="server"
                                    ErrorMessage="Denetim tarihi zorunludur ve gelecekte olamaz."
                                    Display="Dynamic"
                                    CssClass="text-danger small"
                                    OnServerValidate="cvDenetimTarihi_ServerValidate"></asp:CustomValidator>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Butonlar -->
                <div class="row mt-4">
                    <div class="col-12">
                        <div class="d-flex justify-content-end gap-2">
                            <asp:Button ID="btnTemizle" runat="server" CssClass="btn btn-secondary"
                                Text="🗑️ Temizle" OnClick="btnTemizle_Click" CausesValidation="false" />
                            <asp:Button ID="btnKaydet" runat="server" CssClass="btn btn-primary"
                                Text="💾 Kaydet" OnClick="btnKaydet_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            // Flatpickr Date Picker
            flatpickr("#<%= txtDenetimTarihi.ClientID %>", {
                dateFormat: "d.m.Y",
                locale: "tr",
                maxDate: "today",
                allowInput: false,
                onChange: function (selectedDates, dateStr, instance) {
                    document.getElementById("<%= hdnDenetimTarihi.ClientID %>").value = dateStr;
                }
            });
        });
    </script>
</asp:Content>
