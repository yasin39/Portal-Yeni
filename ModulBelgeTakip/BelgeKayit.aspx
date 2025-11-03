<%@ Page Title="Firma Belge Kayıt" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" 
    CodeBehind="BelgeKayit.aspx.cs" Inherits="Portal.ModulBelgeTakip.BelgeKayit" EnableEventValidation="false" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">    
    <link href="~/wwwroot/css/BELGETAKIPMODUL.css" rel="stylesheet" />

    <style>
        /* Belge form özel stilleri */
        .belge-input-group {
            display: flex;
            gap: 10px;
            align-items: center;
        }

        .belge-input-group .form-control {
            flex: 1;
        }

        .belge-separator {
            font-size: 1.5rem;
            font-weight: 700;
            color: #6b7280;
        }

        .firma-info-card {
            background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
            padding: 1.25rem;
            border-radius: 10px;
            border-left: 4px solid #4B7BEC;
            margin-bottom: 1.5rem;
        }

        .info-label {
            font-weight: 600;
            color: #4B7BEC;
            font-size: 0.85rem;
            margin-bottom: 0.25rem;
        }

        .info-value {
            font-size: 1.05rem;
            color: #2C3E50;
            font-weight: 500;
        }
    </style>
</asp:Content>

<asp:Content ID="ContentBreadcrumb" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item">
        <i class="fas fa-file-certificate me-1"></i>Firma Belge İşlemleri
    </li>
    <li class="breadcrumb-item active" aria-current="page">Belge Kaydet</li>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        
        <%-- Firma Arama Bölümü --%>
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h5 class="card-title mb-0">
                            <i class="fas fa-search me-2"></i>Firma Arama
                        </h5>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label for="TxtVergiNo" class="form-label">
                                        <i class="fas fa-hashtag me-1"></i>Vergi Numarası
                                    </label>
                                    <asp:TextBox ID="TxtVergiNo" runat="server" CssClass="form-control" 
                                        placeholder="Vergi numarasını giriniz (10 veya 11 hane)" 
                                        MaxLength="11" autocomplete="off">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvVergiNo" runat="server" 
                                        ControlToValidate="TxtVergiNo" 
                                        ErrorMessage="Vergi numarası zorunludur" 
                                        CssClass="text-danger" 
                                        Display="Dynamic"
                                        ValidationGroup="FirmaAra">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revVergiNo" runat="server" 
                                        ControlToValidate="TxtVergiNo" 
                                        ValidationExpression="^\d{10,11}$" 
                                        ErrorMessage="Vergi numarası 10 veya 11 haneli sayı olmalıdır" 
                                        CssClass="text-danger" 
                                        Display="Dynamic"
                                        ValidationGroup="FirmaAra">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="col-md-6 d-flex align-items-end">
                                <div class="mb-3 w-100">
                                    <asp:Button ID="BtnFirmaGetir" runat="server" 
                                        Text="🔍 Firma Ara" 
                                        CssClass="btn btn-primary w-25" 
                                        OnClick="BtnFirmaGetir_Click"
                                        ValidationGroup="FirmaAra" />
                                </div>
                            </div>
                        </div>

                        <%-- Firma Listesi GridView --%>
                        <div class="row mt-3">
                            <div class="col-12">
                                <asp:GridView ID="GvFirma" runat="server" 
                                    CssClass="table table-striped table-hover" 
                                    AutoGenerateColumns="False"
                                    DataKeyNames="ID"
                                    AllowPaging="True"
                                    PageSize="10"
                                    OnSelectedIndexChanged="GvFirma_SelectedIndexChanged"
                                    OnPageIndexChanging="GvFirma_PageIndexChanging"
                                    EmptyDataText="Vergi numarası ile eşleşen firma bulunamadı.">
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" SelectText="Seç" 
                                            ButtonType="Button" 
                                            ControlStyle-CssClass="btn btn-sm btn-primary" />
                                        <asp:BoundField DataField="FIRMA_ADI" HeaderText="Firma Adı" />
                                        <asp:BoundField DataField="VERGI_NUMARASI" HeaderText="Vergi No" />
                                        <asp:BoundField DataField="FIRMA_ADRESI" HeaderText="Adres" />
                                        <asp:BoundField DataField="BELGE_AD" HeaderText="Belge Türü" />
                                        <asp:TemplateField HeaderText="Belge Durumu">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBelgeDurumu" runat="server" 
                                                    CssClass='<%# Convert.ToBoolean(Eval("BELGE_ALDIMI")) ? "highlight-green" : "highlight-red" %>'
                                                    Text='<%# Convert.ToBoolean(Eval("BELGE_ALDIMI")) ? "✓ Belgeli" : "✗ Belgesiz" %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%-- Belge Kayıt Formu --%>
        <asp:Panel ID="PanelBelge" runat="server" Visible="false">
            <div class="row mt-4">
                <div class="col-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="card-title mb-0">
                                <i class="fas fa-file-signature me-2"></i>Belge Kayıt İşlemi
                            </h5>
                        </div>
                        <div class="card-body">
                            
                            <%-- Seçili Firma Bilgileri --%>
                            <div class="firma-info-card">
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="info-label">Firma Adı</div>
                                        <div class="info-value">
                                            <asp:Label ID="lblFirmaAdi" runat="server" Text="-"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="info-label">Adres</div>
                                        <div class="info-value">
                                            <asp:Label ID="lblAdres" runat="server" Text="-"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="info-label">Belge Türü</div>
                                        <div class="info-value">
                                            <asp:Label ID="lblBelgeTuru" runat="server" Text="-"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <asp:Label ID="lblID" runat="server" Visible="false"></asp:Label>
                            </div>

                            <%-- Belge Bilgileri Form --%>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtBelgeTarihi" class="form-label">
                                            <i class="fas fa-calendar me-1"></i>Belge Tarihi
                                        </label>
                                        <asp:TextBox ID="txtBelgeTarihi" runat="server" 
                                            CssClass="form-control" 
                                            placeholder="GG.AA.YYYY" 
                                            autocomplete="off">
                                        </asp:TextBox>
                                        <asp:HiddenField ID="hdnSelectedDate" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvBelgeTarihi" runat="server" 
                                            ControlToValidate="txtBelgeTarihi" 
                                            ErrorMessage="Belge tarihi zorunludur" 
                                            CssClass="text-danger" 
                                            Display="Dynamic"
                                            ValidationGroup="BelgeKaydet">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvTarih" runat="server" 
                                            ControlToValidate="txtBelgeTarihi"
                                            ErrorMessage="Belge tarihi gelecek bir tarih olamaz" 
                                            CssClass="text-danger" 
                                            Display="Dynamic"
                                            OnServerValidate="CustomValidatorTarih_ServerValidate"
                                            ValidationGroup="BelgeKaydet">
                                        </asp:CustomValidator>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label class="form-label">
                                            <i class="fas fa-file-alt me-1"></i>Belge Numarası
                                        </label>
                                        <div class="belge-input-group">
                                            <asp:TextBox ID="TxtBelge1" runat="server" 
                                                CssClass="form-control" 
                                                placeholder="XX" 
                                                MaxLength="2"
                                                autocomplete="off">
                                            </asp:TextBox>
                                            <span class="belge-separator">.</span>
                                            <asp:TextBox ID="TxtBelge2" runat="server" 
                                                CssClass="form-control" 
                                                placeholder="XXXXXX" 
                                                MaxLength="6"
                                                autocomplete="off">
                                            </asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvBelge1" runat="server" 
                                            ControlToValidate="TxtBelge1" 
                                            ErrorMessage="Belge numarası zorunludur" 
                                            CssClass="text-danger" 
                                            Display="Dynamic"
                                            ValidationGroup="BelgeKaydet">
                                        </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvBelge2" runat="server" 
                                            ControlToValidate="TxtBelge2" 
                                            ErrorMessage="Belge numarası tam olmalıdır" 
                                            CssClass="text-danger" 
                                            Display="Dynamic"
                                            ValidationGroup="BelgeKaydet">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revBelge1" runat="server" 
                                            ControlToValidate="TxtBelge1" 
                                            ValidationExpression="^\d{2}$" 
                                            ErrorMessage="İlk kısım 2 haneli sayı olmalıdır" 
                                            CssClass="text-danger" 
                                            Display="Dynamic"
                                            ValidationGroup="BelgeKaydet">
                                        </asp:RegularExpressionValidator>
                                        <asp:RegularExpressionValidator ID="revBelge2" runat="server" 
                                            ControlToValidate="TxtBelge2" 
                                            ValidationExpression="^\d{6}$" 
                                            ErrorMessage="İkinci kısım 6 haneli sayı olmalıdır" 
                                            CssClass="text-danger" 
                                            Display="Dynamic"
                                            ValidationGroup="BelgeKaydet">
                                        </asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>

                            <%-- Kaydet Butonu --%>
                            <div class="row mt-3">
                                <div class="col-12">
                                    <asp:Button ID="BtnKaydet" runat="server" 
                                        Text="💾 Belge Kaydını Tamamla" 
                                        CssClass="btn btn-success btn-lg w-100" 
                                        OnClick="BtnKaydet_Click"
                                        ValidationGroup="BelgeKaydet" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

    </div>

    <%-- Flatpickr Date Picker Script --%>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css">
    <link rel="stylesheet" href="https://npmcdn.com/flatpickr/dist/themes/material_blue.css">
    <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
    <script src="https://npmcdn.com/flatpickr/dist/l10n/tr.js"></script>

    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            // Tarih picker
            const tarihInput = document.getElementById('<%= txtBelgeTarihi.ClientID %>');
            const hdnDate = document.getElementById('<%= hdnSelectedDate.ClientID %>');

            if (tarihInput) {
                flatpickr(tarihInput, {
                    locale: 'tr',
                    dateFormat: 'd.m.Y',
                    maxDate: 'today',
                    allowInput: true,
                    onChange: function (selectedDates, dateStr, instance) {
                        hdnDate.value = dateStr;
                    }
                });
            }

            // Vergi No input - sadece rakam
            const vergiNoInput = document.getElementById('<%= TxtVergiNo.ClientID %>');
            if (vergiNoInput) {
                vergiNoInput.addEventListener('input', function (e) {
                    this.value = this.value.replace(/[^0-9]/g, '');
                });
            }

            // Belge numarası inputları - sadece rakam
            const belge1 = document.getElementById('<%= TxtBelge1.ClientID %>');
            const belge2 = document.getElementById('<%= TxtBelge2.ClientID %>');

            if (belge1) {
                belge1.addEventListener('input', function (e) {
                    this.value = this.value.replace(/[^0-9]/g, '');
                    if (this.value.length === 2) {
                        belge2.focus();
                    }
                });
            }

            if (belge2) {
                belge2.addEventListener('input', function (e) {
                    this.value = this.value.replace(/[^0-9]/g, '');
                });
            }
        });
    </script>
</asp:Content>
