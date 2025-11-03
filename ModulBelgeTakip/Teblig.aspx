<%@ Page Title="Tebliğ Tarihi Kayıt" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="Teblig.aspx.cs" Inherits="Portal.ModulBelgeTakip.Teblig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="~/wwwroot/css/BelgeTakipModul.css" rel="stylesheet" />
    
    <style>
        .info-card {
            background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
            border-left: 4px solid #4B7BEC;
            padding: 1rem;
            border-radius: 8px;
            margin-bottom: 1rem;
        }
        
        .info-card label {
            font-weight: 600;
            color: #2C3E50;
            margin-bottom: 0.25rem;
            display: block;
            font-size: 0.85rem;
        }
        
        .info-card .value {
            color: #4B7BEC;
            font-weight: 500;
            font-size: 1rem;
        }
        
        .tarih-panel {
            background: white;
            padding: 1.5rem;
            border-radius: 10px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            border-top: 3px solid #4B7BEC;
        }
        
        .flatpickr-input {
            cursor: pointer;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid py-4">
        
        <div class="card shadow-sm">
            <div class="card-header">
                <h5 class="card-title mb-0">
                    <i class="fas fa-file-signature me-2"></i>Tebliğ Tarihi Kayıt
                </h5>
            </div>
            <div class="card-body">
                
                <div class="filter-section mb-4">
                    <div class="row align-items-end">
                        <div class="col-md-6">
                            <label for="<%= TxtVergiNo.ClientID %>" class="form-label">
                                <i class="fas fa-building me-1"></i>Vergi Numarası
                            </label>
                            <asp:TextBox ID="TxtVergiNo" runat="server" CssClass="form-control" 
                                placeholder="Vergi numarası giriniz" MaxLength="10">
                            </asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <asp:Button ID="BtnFirmaGetir" runat="server" CssClass="btn btn-primary" 
                                OnClick="BtnFirmaGetir_Click" Text="🔍 Firma Getir">
                            </asp:Button>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12">
                        <asp:GridView ID="GvDenetim" runat="server" CssClass="table table-striped table-hover" 
                            AutoGenerateColumns="False" 
                            DataKeyNames="DenetimID,TEBLIG_DURUMU"
                            OnSelectedIndexChanged="GvDenetim_SelectedIndexChanged"
                            EmptyDataText="Kayıt bulunamadı.">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" SelectText="Seç" 
                                    HeaderText="İşlem" ButtonType="Button">
                                    <ControlStyle CssClass="btn btn-sm btn-outline-primary" />
                                </asp:CommandField>
                                <asp:BoundField DataField="FIRMA_ADI" HeaderText="Firma Adı" />
                                <asp:BoundField DataField="VERGI_NUMARASI" HeaderText="Vergi No" />
                                <asp:BoundField DataField="BELGE_TURU" HeaderText="Belge Türü" />
                                <asp:BoundField DataField="DENETIM_TARIHI" HeaderText="Denetim Tarihi" 
                                    DataFormatString="{0:dd.MM.yyyy}" />
                                <asp:BoundField DataField="MAKBUZ_NO" HeaderText="Makbuz No" />
                                <asp:TemplateField HeaderText="Tebliğ Durumu">
                                    <ItemTemplate>
                                        <span class='<%# Eval("TEBLIG_DURUMU").ToString() == "Tebliğ Edildi" ? "highlight-green" : "highlight-red" %>'>
                                            <%# Eval("TEBLIG_DURUMU") %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TEBLIG_TARIHI" HeaderText="Tebliğ Tarihi" 
                                    DataFormatString="{0:dd.MM.yyyy}" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

                <asp:Panel ID="PnlDenetim" runat="server" Visible="false" CssClass="mt-4">
                    <div class="tarih-panel">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="info-card">
                                    <label>Seçili Denetim ID</label>
                                    <div class="value">
                                        <asp:Label ID="LblDenetimID" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-md-6">
                                <label for="<%= TxtTebligTarihi.ClientID %>" class="form-label">
                                    <i class="fas fa-calendar-alt me-1"></i>Tebliğ Tarihi <span class="text-danger">*</span>
                                </label>
                                <asp:TextBox ID="TxtTebligTarihi" runat="server" CssClass="form-control" 
                                    placeholder="Tarih seçiniz">
                                </asp:TextBox>
                                <asp:HiddenField ID="HdnSelectedTebligDate" runat="server" />
                                
                                <asp:RequiredFieldValidator ID="rfvTebligTarihi" runat="server" 
                                    ControlToValidate="HdnSelectedTebligDate"
                                    ErrorMessage="Tebliğ tarihi zorunludur." 
                                    CssClass="text-danger small mt-1 d-block"
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                                
                                <asp:CustomValidator ID="cvTebligTarih" runat="server" 
                                    ControlToValidate="HdnSelectedTebligDate"
                                    OnServerValidate="CustomValidatorTebligTarih_ServerValidate"
                                    CssClass="text-danger small mt-1 d-block"
                                    Display="Dynamic">
                                </asp:CustomValidator>
                            </div>
                            
                            <div class="col-md-6 d-flex align-items-end">
                                <asp:Button ID="BtnKaydet" runat="server" CssClass="btn btn-success" 
                                    OnClick="BtnKaydet_Click" Text="💾 Kaydet">
                                </asp:Button>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

            </div>
        </div>

    </div>
    
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function() {
            
            const tarihInput = document.getElementById('<%= TxtTebligTarihi.ClientID %>');
            const hiddenField = document.getElementById('<%= HdnSelectedTebligDate.ClientID %>');

            if (tarihInput) {
                flatpickr(tarihInput, {
                    locale: "tr",
                    dateFormat: "d.m.Y",
                    altInput: false,
                    allowInput: false,
                    disable: [
                        function(date) {
                            return (date.getDay() === 0 || date.getDay() === 6);
                        }
                    ],
                    onChange: function(selectedDates, dateStr, instance) {
                        if (selectedDates.length > 0) {
                            hiddenField.value = dateStr;
                        } else {
                            hiddenField.value = '';
                        }
                    }
                });
            }
        });

        function showNotification(type, message) {
            const toast = {
                success: { icon: 'fa-check-circle', bg: 'success' },
                danger: { icon: 'fa-exclamation-circle', bg: 'danger' },
                warning: { icon: 'fa-exclamation-triangle', bg: 'warning' },
                info: { icon: 'fa-info-circle', bg: 'info' }
            };

            const config = toast[type] || toast.info;
            
            const toastHtml = `
                <div class="toast align-items-center text-white bg-${config.bg} border-0" role="alert" aria-live="assertive" aria-atomic="true">
                    <div class="d-flex">
                        <div class="toast-body">
                            <i class="fas ${config.icon} me-2"></i>${message}
                        </div>
                        <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                    </div>
                </div>`;
            
            const container = document.getElementById('toastContainer') || (() => {
                const div = document.createElement('div');
                div.id = 'toastContainer';
                div.className = 'toast-container position-fixed top-0 end-0 p-3';
                div.style.zIndex = '9999';
                document.body.appendChild(div);
                return div;
            })();
            
            container.insertAdjacentHTML('beforeend', toastHtml);
            
            const toastElement = container.lastElementChild;
            const bsToast = new bootstrap.Toast(toastElement, { autohide: true, delay: 3000 });
            bsToast.show();
            
            toastElement.addEventListener('hidden.bs.toast', () => toastElement.remove());
        }
    </script>
</asp:Content>