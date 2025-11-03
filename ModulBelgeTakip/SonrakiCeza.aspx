<%@ Page Title="Sonraki Ceza Kaydı" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="SonrakiCeza.aspx.cs" Inherits="Portal.ModulBelgeTakip.SonrakiCeza" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        /* Modern Card Styling */
        .search-card {
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(75, 123, 236, 0.1);
            border: none;
            margin-bottom: 1.5rem;
        }

            .search-card .card-header {
                background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
                color: white;
                border-radius: 12px 12px 0 0 !important;
                padding: 1rem 1.5rem;
                font-weight: 600;
                font-size: 1.1rem;
            }

            .search-card .card-body {
                padding: 1.5rem;
            }

        /* GridView Modern Styling */
        .table-custom {
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
        }

            .table-custom thead th {
                background: linear-gradient(135deg, #2E5B9A 0%, #4B7BEC 100%);
                color: white;
                font-weight: 600;
                text-transform: uppercase;
                font-size: 0.85rem;
                letter-spacing: 0.5px;
                border: none;
                padding: 14px 12px;
            }

            .table-custom tbody tr {
                transition: all 0.2s ease;
            }

                .table-custom tbody tr:hover {
                    background-color: rgba(75, 123, 236, 0.08);
                    transform: scale(1.01);
                }

            .table-custom tbody td {
                vertical-align: middle;
                padding: 12px;
            }

        /* Form Controls */
        .form-control-custom {
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            padding: 0.6rem 1rem;
            transition: all 0.3s ease;
        }

            .form-control-custom:focus {
                border-color: #4B7BEC;
                box-shadow: 0 0 0 0.2rem rgba(75, 123, 236, 0.15);
            }

        /* Button Styling */
        .btn-custom-primary {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            border: none;
            color: white;
            font-weight: 500;
            padding: 0.6rem 1.5rem;
            border-radius: 8px;
            transition: all 0.3s ease;
        }

            .btn-custom-primary:hover {
                background: linear-gradient(135deg, #3B6BD3 0%, #1E4B8A 100%);
                transform: translateY(-2px);
                box-shadow: 0 4px 12px rgba(75, 123, 236, 0.3);
            }

        /* Selected Firma Panel */
        .selected-firma-panel {
            background: linear-gradient(135deg, #E8F4FD 0%, #D6E9F8 100%);
            border-left: 4px solid #4B7BEC;
            padding: 1rem;
            border-radius: 8px;
            margin-bottom: 1.5rem;
        }

        /* Validation */
        .validation-error {
            color: #dc3545;
            font-size: 0.875rem;
            margin-top: 0.25rem;
            display: block;
        }

        /* Info Badge */
        .info-badge {
            background: #4B7BEC;
            color: white;
            padding: 0.5rem 1rem;
            border-radius: 6px;
            font-weight: 600;
            display: inline-block;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item"><a href="/Anasayfa.aspx">Ana Sayfa</a></li>
    <li class="breadcrumb-item">Belge Takip</li>
    <li class="breadcrumb-item active">Sonraki Ceza Kaydı</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <!-- Başlık -->
        <div class="row mb-4">
            <div class="col-12">
                <h2 class="text-primary-custom">
                    <i class="fas fa-clipboard-list me-2"></i>Sonraki Ceza Kaydı
                </h2>
                <p class="text-muted">Firmaya ait tekrar eden denetim ve ceza kaydı oluşturun</p>
            </div>
        </div>

        <!-- Firma Arama Kartı -->
        <div class="row">
            <div class="col-12">
                <div class="card search-card">
                    <div class="card-header">
                        <i class="fas fa-search me-2"></i>Firma Arama
                   
                    </div>
                    <div class="card-body">
                        <div class="row align-items-end">
                            <div class="col-md-6">
                                <label for="<%= TxtVergiNo.ClientID %>" class="form-label">
                                    <i class="fas fa-building me-1 text-primary-custom"></i>Vergi Numarası
                               
                                </label>
                                <asp:TextBox ID="TxtVergiNo" runat="server" CssClass="form-control form-control-custom"
                                    placeholder="Vergi numarası giriniz" MaxLength="11"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <asp:Button ID="BtnAra" runat="server" Text="🔍 Ara" CssClass="btn btn-custom-primary"
                                    OnClick="BtnAra_Click" />
                            </div>
                        </div>

                        <!-- Firma Grid -->
                        <div class="row mt-4" id="divFirmaGrid" runat="server" visible="true">
                            <div class="col-12">
                                <asp:GridView ID="FirmaGrid" runat="server" CssClass="table table-hover table-custom"
                                    AutoGenerateColumns="False" DataKeyNames="ID"
                                    OnSelectedIndexChanged="FirmaGrid_SelectedIndexChanged"
                                    EmptyDataText="Firma bulunamadı. Lütfen vergi numarası ile arama yapınız."
                                    GridLines="None">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                        <asp:BoundField DataField="FIRMA_ADI" HeaderText="Firma Adı" />
                                        <asp:BoundField DataField="VERGI_NUMARASI" HeaderText="Vergi No" />
                                        <asp:BoundField DataField="FIRMA_ADRESI" HeaderText="Adres" />
                                        <asp:CommandField ShowSelectButton="True" SelectText="Seç"
                                            HeaderText="İşlem" ButtonType="Link">
                                            <ControlStyle CssClass="btn btn-outline-primary btn-sm" />
                                        </asp:CommandField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="text-center text-muted py-4" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Denetim Bilgileri Paneli -->
        <asp:Panel ID="PanelDenetim" runat="server" Visible="false">
            <!-- Seçili Firma Bilgisi -->
            <div class="row mb-3">
                <div class="col-12">
                    <div class="selected-firma-panel">
                        <h5 class="mb-2">
                            <i class="fas fa-check-circle text-success me-2"></i>Seçili Firma
                        </h5>
                        <div class="row">
                            <div class="col-md-6">
                                <strong>Firma Adı:</strong>
                                <asp:Label ID="LblFirmaAdi" runat="server" CssClass="ms-2"></asp:Label>
                            </div>
                            <div class="col-md-6">
                                <strong>Firma ID:</strong>
                                <asp:Label ID="LblFirmaID" runat="server" CssClass="ms-2"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Denetim Formu -->
            <div class="row">
                <div class="col-12">
                    <div class="card search-card">
                        <div class="card-header">
                            <i class="fas fa-file-signature me-2"></i>Denetim Bilgileri
                       
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <!-- Personel Seçimi -->
                                <div class="col-md-6 mb-3">
                                    <label for="<%= DdlPersonel.ClientID %>" class="form-label">
                                        <i class="fas fa-user-tie me-1 text-primary-custom"></i>Ceza Kesen Personel
                                       
                                        <span class="text-danger">*</span>
                                    </label>
                                    <asp:DropDownList ID="DdlPersonel" runat="server" CssClass="form-select form-control-custom"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvPersonel" runat="server"
                                        ControlToValidate="DdlPersonel" InitialValue="-1"
                                        ErrorMessage="Personel seçimi zorunludur"
                                        CssClass="validation-error" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>

                                <!-- Belge Türü -->
                                <div class="col-md-6 mb-3">
                                    <label for="<%= DdlBelgeTuru.ClientID %>" class="form-label">
                                        <i class="fas fa-file-alt me-1 text-primary-custom"></i>Belge Türü
                                       
                                        <span class="text-danger">*</span>
                                    </label>
                                    <asp:DropDownList ID="DdlBelgeTuru" runat="server" CssClass="form-select form-control-custom"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvBelgeTuru" runat="server"
                                        ControlToValidate="DdlBelgeTuru" InitialValue="-1"
                                        ErrorMessage="Belge türü seçimi zorunludur"
                                        CssClass="validation-error" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>

                                <!-- Denetim Tarihi -->
                                <div class="col-md-6 mb-3">
                                    <label for="<%= TxtDate.ClientID %>" class="form-label">
                                        <i class="fas fa-calendar-alt me-1 text-primary-custom"></i>Denetim Tarihi
                                       
                                        <span class="text-danger">*</span>
                                    </label>
                                    <asp:TextBox ID="TxtDate" runat="server" CssClass="form-control form-control-custom"
                                        placeholder="Tarih seçiniz"></asp:TextBox>
                                    <asp:HiddenField ID="HdnSelectedDate" runat="server" />
                                    <asp:CustomValidator ID="CustomValidatorTarih" runat="server"
                                        ControlToValidate="TxtDate"
                                        OnServerValidate="CustomValidatorTarih_ServerValidate"
                                        ErrorMessage="Denetim tarihi zorunludur"
                                        CssClass="validation-error" Display="Dynamic"></asp:CustomValidator>
                                </div>

                                <!-- Ceza Makbuz No -->
                                <div class="col-md-6 mb-3">
                                    <label for="<%= TxtCezaMakbuzNo.ClientID %>" class="form-label">
                                        <i class="fas fa-receipt me-1 text-primary-custom"></i>Ceza Makbuz No
                                       
                                        <span class="text-danger">*</span>
                                    </label>
                                    <asp:TextBox ID="TxtCezaMakbuzNo" runat="server" CssClass="form-control form-control-custom"
                                        placeholder="Makbuz numarası giriniz" MaxLength="6"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvMakbuz" runat="server"
                                        ControlToValidate="TxtCezaMakbuzNo"
                                        ErrorMessage="Ceza makbuz numarası zorunludur"
                                        CssClass="validation-error" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <!-- Kaydet Butonu -->
                            <div class="row mt-3">
                                <div class="col-12 text-end">
                                    <asp:Button ID="BtnKaydet" runat="server" Text="💾 Kaydet"
                                        CssClass="btn btn-custom-primary btn-lg" OnClick="BtnKaydet_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div> 
<%--    <script src="../wwwroot/js/flatpickr.js"></script>
    <script src="../wwwroot/js/tr.js"></script>--%>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            // Flatpickr Türkçe tarih seçici
            flatpickr('#<%= TxtDate.ClientID %>', {
                dateFormat: 'd.m.Y',
                locale: 'tr',
                maxDate: 'today',
                altInput: true,
                altFormat: 'd F Y',
                onChange: function (selectedDates, dateStr, instance) {
                    if (selectedDates.length > 0) {
                        // Hidden field'a ISO formatında kaydet
                        document.getElementById('<%= HdnSelectedDate.ClientID %>').value =
                            selectedDates[0].toLocaleDateString('tr-TR');
                    }
                }
            });
        });
    </script>
</asp:Content>
