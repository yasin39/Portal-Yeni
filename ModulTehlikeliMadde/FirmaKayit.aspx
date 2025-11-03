<%@ Page Title="Tehlikeli Madde Firma Kayıt" Language="C#" MasterPageFile="~/AnaV2.Master"
    AutoEventWireup="true" CodeBehind="FirmaKayit.aspx.cs"
    Inherits="Portal.ModulTehlikeliMadde.FirmaKayit" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="~/wwwroot/css/BELGETAKIPMODUL.css" rel="stylesheet" />
    <style>
        .tm-primary-gradient {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
        }

        .tm-card {
            background: white;
            border-radius: 10px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
            margin-bottom: 1.5rem;
            border-left: 4px solid #4B7BEC;
        }

        .tm-form-label {
            font-weight: 600;
            color: #2E5B9A;
            margin-bottom: 0.5rem;
        }

        .tm-grid-container {
            background: white;
            border-radius: 10px;
            padding: 1.5rem;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
        }

        .btn-tm-primary {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            border: none;
            color: white;
            padding: 0.6rem 1.5rem;
            font-weight: 500;
            transition: all 0.3s ease;
        }

            .btn-tm-primary:hover {
                transform: translateY(-2px);
                box-shadow: 0 4px 12px rgba(75, 123, 236, 0.3);
                color: white;
            }

        .btn-tm-success {
            background: linear-gradient(135deg, #10b981 0%, #059669 100%);
            border: none;
            color: white;
        }

            .btn-tm-success:hover {
                transform: translateY(-2px);
                box-shadow: 0 4px 12px rgba(16, 185, 129, 0.3);
                color: white;
            }

        .btn-tm-warning {
            background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
            border: none;
            color: white;
        }

            .btn-tm-warning:hover {
                transform: translateY(-2px);
                color: white;
            }

        .btn-tm-secondary {
            background: linear-gradient(135deg, #6b7280 0%, #4b5563 100%);
            border: none;
            color: white;
        }

        .required-field::after {
            content: " *";
            color: #ef4444;
            font-weight: bold;
        }

        .info-badge {
            background: #dbeafe;
            color: #1e40af;
            padding: 0.5rem 1rem;
            border-radius: 6px;
            font-size: 0.9rem;
            border-left: 3px solid #3b82f6;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">

        <!-- Başlık ve İstatistik Kartı -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="tm-card">
                    <div class="card-header tm-primary-gradient text-white py-3">
                        <div class="d-flex justify-content-between align-items-center">
                            <h5 class="mb-0">
                                <i class="fas fa-radiation me-2"></i>Tehlikeli Madde Firma Kayıt ve İstatistik
                            </h5>
                            <asp:Label ID="lblKayitSayisi" runat="server" CssClass="badge bg-light text-dark"
                                Text="0 kayıt"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Arama Kartı -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="tm-card">
                    <div class="card-body">
                        <div class="row align-items-end">
                            <div class="col-md-4">
                                <label class="form-label tm-form-label">
                                    <i class="fas fa-search me-1"></i>Kayıt No ile Ara
                                </label>
                                <asp:TextBox ID="txtKayitNo" runat="server" CssClass="form-control"
                                    placeholder="Kayıt numarası giriniz..." TextMode="Number"></asp:TextBox>
                            </div>
                            <div class="col-md-8">
                                <asp:Button ID="btnAra" runat="server" CssClass="btn btn-tm-primary me-2"
                                    Text="🔍 Ara" OnClick="btnAra_Click" CausesValidation="false" />
                                <asp:Button ID="btnYeniKayit" runat="server" CssClass="btn btn-tm-success"
                                    Text="➕ Yeni Kayıt" OnClick="btnYeniKayit_Click" CausesValidation="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Form Kartı -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="tm-card">
                    <div class="card-header tm-primary-gradient text-white">
                        <h6 class="mb-0">
                            <i class="fas fa-edit me-2"></i>Firma Bilgileri
                        </h6>
                    </div>
                    <div class="card-body p-4">

                        <!-- Satır 1: UNET ve Firma Unvanı -->
                        <div class="row mb-3">
                            <div class="col-md-4">
                                <label class="form-label tm-form-label required-field">UNET No</label>
                                <asp:TextBox ID="txtUnetNo" runat="server" CssClass="form-control"
                                    placeholder="UNET numarası giriniz" TextMode="Number"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvUnet" runat="server"
                                    ControlToValidate="txtUnetNo" ErrorMessage="UNET numarası gereklidir."
                                    ForeColor="#ef4444" ValidationGroup="kayit" Display="Dynamic">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-8">
                                <label class="form-label tm-form-label required-field">Firma Unvanı</label>
                                <asp:TextBox ID="txtUnvan" runat="server" CssClass="form-control"
                                    placeholder="Firma unvanını giriniz"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvUnvan" runat="server"
                                    ControlToValidate="txtUnvan" ErrorMessage="Firma unvanı gereklidir."
                                    ForeColor="#ef4444" ValidationGroup="kayit" Display="Dynamic">*</asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <!-- Satır 2: Firma Adresi -->
                        <div class="row mb-3">
                            <div class="col-12">
                                <label class="form-label tm-form-label required-field">Firma Adresi</label>
                                <asp:TextBox ID="txtAdres" runat="server" CssClass="form-control"
                                    placeholder="Firma adresini giriniz" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAdres" runat="server"
                                    ControlToValidate="txtAdres" ErrorMessage="Firma adresi gereklidir."
                                    ForeColor="#ef4444" ValidationGroup="kayit" Display="Dynamic">*</asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <!-- Satır 3: Yetki Belgesi ve Belge No -->
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label class="form-label tm-form-label">Yetki Belgesi</label>
                                <asp:DropDownList ID="ddlYetkiBelgesi" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label tm-form-label required-field">Belge No</label>
                                <asp:DropDownList ID="ddlBelgeNo" runat="server" CssClass="form-select"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlBelgeNo_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvBelgeNo" runat="server"
                                    ControlToValidate="ddlBelgeNo" InitialValue=""
                                    ErrorMessage="Belge numarası seçiniz."
                                    ForeColor="#ef4444" ValidationGroup="kayit" Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:Label ID="lblBelgeUyari" runat="server" CssClass="text-danger small"
                                    Visible="false"></asp:Label>
                            </div>
                        </div>

                        <!-- Satır 4: İl ve İlçe -->
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label class="form-label tm-form-label required-field">İl</label>
                                <asp:DropDownList ID="ddlIl" runat="server" CssClass="form-select"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlIl_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvIl" runat="server"
                                    ControlToValidate="ddlIl" InitialValue=""
                                    ErrorMessage="İl seçiniz."
                                    ForeColor="#ef4444" ValidationGroup="kayit" Display="Dynamic">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label tm-form-label required-field">İlçe</label>
                                <asp:DropDownList ID="ddlIlce" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvIlce" runat="server"
                                    ControlToValidate="ddlIlce" InitialValue=""
                                    ErrorMessage="İlçe seçiniz."
                                    ForeColor="#ef4444" ValidationGroup="kayit" Display="Dynamic">*</asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <!-- Satır 5: Statü ve Faaliyet Alanı -->
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label class="form-label tm-form-label required-field">Statü</label>
                                <asp:DropDownList ID="ddlStatu" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="">Seçiniz...</asp:ListItem>
                                    <asp:ListItem Value="Ticari Kuruluş">Ticari Kuruluş</asp:ListItem>
                                    <asp:ListItem Value="STK">STK</asp:ListItem>
                                    <asp:ListItem Value="Kamu Kurumu">Kamu Kurumu</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvStatu" runat="server"
                                    ControlToValidate="ddlStatu" InitialValue=""
                                    ErrorMessage="Statü seçiniz."
                                    ForeColor="#ef4444" ValidationGroup="kayit" Display="Dynamic">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label tm-form-label required-field">Faaliyet Alanı</label>
                                <asp:DropDownList ID="ddlFaaliyetAlani" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvFaaliyet" runat="server"
                                    ControlToValidate="ddlFaaliyetAlani" InitialValue=""
                                    ErrorMessage="Faaliyet alanı seçiniz."
                                    ForeColor="#ef4444" ValidationGroup="kayit" Display="Dynamic">*</asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <!-- Satır 6: Belge Durumu -->
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label class="form-label tm-form-label">Belge Durumu</label>
                                <asp:DropDownList ID="ddlBelgeDurumu" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="Geçerli" Selected="True">Geçerli</asp:ListItem>
                                    <asp:ListItem Value="İptal">İptal</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <!-- Satır 7: Açıklama -->
                        <div class="row mb-3">
                            <div class="col-12">
                                <label class="form-label tm-form-label">Açıklama</label>
                                <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control"
                                    placeholder="Varsa açıklama giriniz..." TextMode="MultiLine" Rows="3"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Validation Summary -->
                        <div class="row mb-3">
                            <div class="col-12">
                                <asp:ValidationSummary ID="valSummary" runat="server"
                                    CssClass="alert alert-danger" ValidationGroup="kayit"
                                    HeaderText="Lütfen aşağıdaki alanları doldurunuz:" />
                            </div>
                        </div>
                        <!-- Form Kartı içinde, butonların üstüne ekle -->
                        <asp:HiddenField ID="hfSecilenKayitID" runat="server" Value="0" />
                        <!-- Butonlar -->
                        <div class="row">
                            <div class="col-12">
                                <asp:Button ID="btnKaydet" runat="server" CssClass="btn btn-tm-primary me-2"
                                    Text="💾 Kaydet" OnClick="btnKaydet_Click" ValidationGroup="kayit" />
                                <asp:Button ID="btnGuncelle" runat="server" CssClass="btn btn-tm-warning me-2"
                                    Text="✏️ Güncelle" OnClick="btnGuncelle_Click"
                                    ValidationGroup="kayit" Visible="false" />
                                <asp:Button ID="btnVazgec" runat="server" CssClass="btn btn-tm-secondary"
                                    Text="❌ Vazgeç" OnClick="btnVazgec_Click"
                                    CausesValidation="false" Visible="false" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <!-- GridView Kartı -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="tm-card">
                    <div class="card-header tm-primary-gradient text-white">
                        <div class="d-flex justify-content-between align-items-center">
                            <h6 class="mb-0">
                                <i class="fas fa-table me-2"></i>Kayıtlı Firmalar
                            </h6>
                            <asp:Button ID="btnExcelAktar" runat="server" CssClass="btn btn-sm btn-light"
                                Text="📊 Excel'e Aktar" OnClick="btnExcelAktar_Click"
                                CausesValidation="false" />
                        </div>
                    </div>
                    <div class="card-body p-0">
                        <div class="table-responsive">
                            <asp:GridView ID="gvFirmalar" runat="server"
                                CssClass="table table-hover table-striped mb-0"
                                AutoGenerateColumns="False"
                                AllowPaging="True"
                                PageSize="20"
                                OnPageIndexChanging="gvFirmalar_PageIndexChanging"
                                EmptyDataText="Kayıt bulunamadı.">
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="No"
                                        ItemStyle-CssClass="text-center"
                                        HeaderStyle-CssClass="text-center bg-primary text-white" />
                                    <asp:BoundField DataField="Unet" HeaderText="UNET"
                                        ItemStyle-CssClass="text-center"
                                        HeaderStyle-CssClass="bg-primary text-white" />
                                    <asp:BoundField DataField="Unvan" HeaderText="Firma Unvanı"
                                        HeaderStyle-CssClass="bg-primary text-white" />
                                    <asp:BoundField DataField="Adres" HeaderText="Adres"
                                        HeaderStyle-CssClass="bg-primary text-white" />
                                    <asp:BoundField DataField="il" HeaderText="İl"
                                        HeaderStyle-CssClass="bg-primary text-white" />
                                    <asp:BoundField DataField="ilce" HeaderText="İlçe"
                                        HeaderStyle-CssClass="bg-primary text-white" />
                                    <asp:BoundField DataField="BelgeTuru" HeaderText="Belge Türü"
                                        HeaderStyle-CssClass="bg-primary text-white" />
                                    <asp:BoundField DataField="BelgeSeriNo" HeaderText="Belge No"
                                        ItemStyle-CssClass="text-center"
                                        HeaderStyle-CssClass="bg-primary text-white" />
                                    <asp:BoundField DataField="Statu" HeaderText="Statü"
                                        HeaderStyle-CssClass="bg-primary text-white" />
                                    <asp:BoundField DataField="FaaliyetTuru" HeaderText="Faaliyet"
                                        HeaderStyle-CssClass="bg-primary text-white" />
                                    <asp:BoundField DataField="Durum" HeaderText="Belge Durumu"
                                        ItemStyle-CssClass="text-center"
                                        HeaderStyle-CssClass="bg-primary text-white" />
                                    <asp:BoundField DataField="KayitKullanici" HeaderText="Kaydeden"
                                        HeaderStyle-CssClass="bg-primary text-white" />
                                    <asp:BoundField DataField="KayitTarihi" HeaderText="Kayıt Tarihi"
                                        DataFormatString="{0:dd.MM.yyyy HH:mm}"
                                        ItemStyle-CssClass="text-center"
                                        HeaderStyle-CssClass="bg-primary text-white" />
                                </Columns>
                                <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
