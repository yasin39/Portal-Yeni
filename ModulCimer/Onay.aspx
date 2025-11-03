<%@ Page Title="CİMER Onay" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="Onay.aspx.cs" Inherits="Portal.ModulCimer.Onay" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Ana.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <style>
        .tooltip-inner {
            max-width: 500px;
            background-color: #2E5B9A;
        }
    </style>
</asp:Content>

<asp:Content ID="BreadcrumbContent" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <!--  Enhanced breadcrumb -->
    <li class="breadcrumb-item">
        <i class="fas fa-comments me-1"></i>CİMER
    </li>
    <li class="breadcrumb-item active" aria-current="page">Onay İşlemleri</li>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <!--  Main GridView Card -->
        <div class="row">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header">
                        <div>
                            <i class="fas fa-check-double"></i>
                            <span>Onay Bekleyen CİMER Başvuruları</span>
                        </div>
                    </div>
                    <div class="card-body">
                        <!--  Info badge -->
                        <div class="info-badge">
                            <i class="fas fa-info-circle"></i>
                            Onayınızı bekleyen başvuruları görmektesiniz. Detaylı inceleme için "Seç" butonunu kullanınız.
                        </div>

                        <!--  Action bar -->
                        <div class="action-buttons mb-3">
                            <div class="d-flex justify-content-end">
                                <asp:Button ID="btnExceleAktar" runat="server" Text="📊 Excel'e Aktar"
                                    CssClass="btn btn-primary" OnClick="ExceleAktar_Click" />
                            </div>
                        </div>

                        <!--  GridView with enhanced table styling -->
                        <!--  GridView with enhanced table styling -->
                        <div class="grid-container">
                            <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-hover mb-0"
                                AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                DataKeyNames="id" AllowPaging="True" PageSize="10"
                                EmptyDataText="Henüz onay bekleyen başvuru bulunmamaktadır.">
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" ButtonType="Button" SelectText="Seç"
                                        ControlStyle-CssClass="btn btn-sm btn-primary" />
                                    <asp:BoundField DataField="id" HeaderText="No" ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="Basvuru_No" HeaderText="Başvuru No" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="TC_No" HeaderText="TC Kimlik" Visible="False" />
                                    <asp:BoundField DataField="Adi_Soyadi" HeaderText="Adı Soyadı" Visible="False" />
                                    <asp:BoundField DataField="Tel_No" HeaderText="Cep Telefonu" Visible="False" />
                                    <asp:BoundField DataField="Mail" HeaderText="Mail Adresi" Visible="False" />
                                    <asp:BoundField DataField="Adres" HeaderText="Adres" Visible="False" />
                                    <asp:TemplateField HeaderText="Başvuru Metni">
                                        <ItemTemplate>
                                            <div style="max-width: 400px;">
                                                <span data-bs-toggle="tooltip"
                                                    data-bs-placement="top"
                                                    data-bs-html="true"
                                                    title='<%# GetTooltipText(Eval("Basvuru_Metni")) %>'>
                                                    <%# GetShortText(Eval("Basvuru_Metni"), 150) %>
                                                </span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-wrap" />
                                    </asp:TemplateField>

                                    <asp:HyperLinkField DataNavigateUrlFields="Basvuru_Ek" DataTextField="Basvuru_Ek"
                                        DataTextFormatString="📎 İndir" HeaderText="Ek" Target="_blank"
                                        ItemStyle-CssClass="text-decoration-none"
                                        ControlStyle-CssClass="badge bg-info text-decoration-none" />
                                    <asp:BoundField DataField="Yapilan_İslem" HeaderText="Yapılan İşlem" />
                                    <asp:BoundField DataField="Son_Yapilan_islem" HeaderText="Son İşlem" />
                                    <asp:BoundField DataField="Sikayet_Edilen_Firma" HeaderText="Şikayete Konu Firma" />
                                    <asp:BoundField DataField="Kayit_Kullanici" HeaderText="Kullanıcı" />
                                    <asp:BoundField DataField="Kayit_Tarihi" HeaderText="Kayıt Tarihi"
                                        DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="Guncelleyen_Kullanici" HeaderText="İşlem Yapan" />
                                    <asp:BoundField DataField="Guncelleme_Tarihi" HeaderText="Cevaplama Tarihi"
                                        DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="Sonuc" HeaderText="Durum" />
                                    <asp:BoundField DataField="Basvuru_Tarihi" HeaderText="Başvuru Tarihi"
                                        DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="Bekleme_Durumu" HeaderText="Bekleme Durumu" />
                                </Columns>
                                <PagerStyle CssClass="pagination justify-content-center mt-3" />
                                <EmptyDataRowStyle CssClass="text-center text-muted p-4" />
                            </asp:GridView>
                        </div>

                        <!--  Privacy notice -->
                        <div class="privacy-notice">
                            <i class="fas fa-shield-alt"></i>
                            <strong>GİZLİLİK UYARISI:</strong> Şahısların kimlik/iletişim bilgilerinin görüntülenmesi hususunda 
                            GİZLİLİK ilkesi ve Kişisel Verilerin Korunması Kanununa dikkat edilmesi gerekmektedir.
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--  Approval Panel (Enhanced) -->
        <asp:Panel ID="pnlApproval" runat="server" Visible="false" CssClass="row mt-4">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header success">
                        <div>
                            <i class="fas fa-check-circle"></i>
                            <span>Başvuru Onay İşlemleri - No: 
                                <asp:Label ID="lblBasvuruNo" runat="server" Font-Bold="true"></asp:Label>
                            </span>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="form-section">
                            <div class="section-title">
                                <i class="fas fa-file-alt"></i>
                                <span>Başvuru Bilgileri</span>
                            </div>

                            <div class="row g-3">
                                <!--  Başvuru No -->
                                <div class="col-md-6">
                                    <label class="form-label">Başvuru No</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-hashtag"></i>
                                        </span>
                                        <asp:TextBox ID="txtBasvuruNo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <!--  Süreç Durumu -->
                                <div class="col-md-6">
                                    <label class="form-label">Süreç Devam Edecek Mi</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-clock"></i>
                                        </span>
                                        <asp:TextBox ID="txtSurecDurum" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <!--  NEW: Başvuru Metni Tam Hali -->
                                <div class="col-md-12">
                                    <label class="form-label">
                                        <i class="fas fa-file-alt me-2"></i>Başvuru Metni (Tam Hali)
           
                                    </label>
                                    <div class="input-group">
                                        <span class="input-group-text align-items-start pt-2">
                                            <i class="fas fa-align-left"></i>
                                        </span>
                                        <asp:TextBox ID="txtBasvuruMetniFull" runat="server" CssClass="form-control"
                                            TextMode="MultiLine" Rows="8" ReadOnly="true"
                                            Style="background-color: #f8f9fa; resize: none;"></asp:TextBox>
                                    </div>
                                    <small class="text-muted mt-1 d-block">
                                        <i class="fas fa-info-circle me-1"></i>Başvuranın tam şikayet metni
                                    </small>
                                </div>

                                <!--  Yapılan İşlem -->
                                <div class="col-md-12">
                                    <label class="form-label">Yapılan İşlem</label>
                                    <div class="input-group">
                                        <span class="input-group-text align-items-start pt-2">
                                            <i class="fas fa-tasks"></i>
                                        </span>
                                        <asp:TextBox ID="txtYapilanIslem" runat="server" CssClass="form-control"
                                            TextMode="MultiLine" Rows="6" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <!--  Son Yapılan İşlem -->
                                <div class="col-md-12">
                                    <label class="form-label">Son Yapılan İşlem</label>
                                    <div class="input-group">
                                        <span class="input-group-text align-items-start pt-2">
                                            <i class="fas fa-check-circle"></i>
                                        </span>
                                        <asp:TextBox ID="txtSonYapilanIslem" runat="server" CssClass="form-control"
                                            TextMode="MultiLine" Rows="6" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="warning-badge">
                                        <i class="fas fa-exclamation-triangle"></i>
                                        Değiştirilmesi gerekiyorsa açıklama bölümünü doldurarak ilgili kullanıcıya iade ediniz
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-section">
                            <div class="section-title">
                                <i class="fas fa-cogs"></i>
                                <span>Onay İşlemleri</span>
                            </div>

                            <div class="row g-3">
                                <!--  Gereği İçin -->
                                <div class="col-md-6">
                                    <label class="form-label required-field">Gereği İçin</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-user-tag"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlKullanici" runat="server" CssClass="form-select">
                                        </asp:DropDownList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvKullanici" runat="server"
                                        ControlToValidate="ddlKullanici"
                                        ErrorMessage="Onay kullanıcısı seçiniz"
                                        CssClass="text-danger small d-block mt-1"
                                        Display="Dynamic"
                                        ValidationGroup="onay">
                                    </asp:RequiredFieldValidator>
                                </div>

                                <!--  Açıklama -->
                                <div class="col-md-12">
                                    <label class="form-label">Açıklama</label>
                                    <div class="input-group">
                                        <span class="input-group-text align-items-start pt-2">
                                            <i class="fas fa-comment-dots"></i>
                                        </span>
                                        <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control"
                                            TextMode="MultiLine" Rows="4"
                                            placeholder="İade işlemi için açıklama girebilirsiniz..."></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--  Action Buttons -->
                        <div class="action-buttons">
                            <div class="d-flex flex-wrap gap-2 justify-content-end">
                                <asp:Button ID="btnHareket" runat="server" Text="📜 Evrak Geçmişi"
                                    CssClass="btn btn-outline-secondary" OnClick="Hareket_Click"
                                    CausesValidation="false" />
                                <asp:Button ID="btnIade" runat="server" Text="↩️ İade Et"
                                    CssClass="btn btn-secondary" OnClick="Iade_Click"
                                    ValidationGroup="onay" />
                                <asp:Button ID="btnOnayla" runat="server" Text="✓ Onayla"
                                    CssClass="btn btn-success" OnClick="Onayla_Click"
                                    ValidationGroup="onay" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!--  History Panel (Enhanced) -->
        <asp:Panel ID="pnlHistory" runat="server" Visible="false" CssClass="row mt-4">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header info">
                        <div>
                            <i class="fas fa-history"></i>
                            <span>Evrak Geçmişi</span>
                        </div>
                        <asp:Button ID="btnGecmisKapat" runat="server" Text="✕ Kapat"
                            CssClass="btn btn-sm btn-outline-light" OnClick="GecmisKapat_Click"
                            CausesValidation="false" />
                    </div>
                    <div class="card-body">
                        <div class="info-badge mb-3">
                            <i class="fas fa-info-circle"></i>
                            Bu başvurunun tüm hareket geçmişini görüntülemektesiniz.
                        </div>

                        <div class="table-responsive">
                            <asp:Label ID="lblTable" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

    <!--  Additional Scripts for enhanced UX -->
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            // Auto-hide alerts after 5 seconds
            const alerts = document.querySelectorAll('.alert:not(.alert-danger)');
            alerts.forEach(alert => {
                setTimeout(() => {
                    const bsAlert = new bootstrap.Alert(alert);
                    bsAlert.close();
                }, 5000);
            });

            // Smooth scroll to approval panel when visible
            const approvalPanel = document.getElementById('<%= pnlApproval.ClientID %>');
            if (approvalPanel && approvalPanel.style.display !== 'none') {
                approvalPanel.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }

            // Smooth scroll to history panel when visible
            const historyPanel = document.getElementById('<%= pnlHistory.ClientID %>');
            if (historyPanel && historyPanel.style.display !== 'none') {
                historyPanel.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        });
    </script>
    <!--  Enhanced Scripts for tooltip and UX -->
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            //  Initialize Bootstrap tooltips
            var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
            var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
                return new bootstrap.Tooltip(tooltipTriggerEl, {
                    html: true,
                    trigger: 'hover'
                });
            });

            // Auto-hide alerts after 5 seconds
            const alerts = document.querySelectorAll('.alert:not(.alert-danger)');
            alerts.forEach(alert => {
                setTimeout(() => {
                    const bsAlert = new bootstrap.Alert(alert);
                    bsAlert.close();
                }, 5000);
            });

            // Smooth scroll to approval panel when visible
            const approvalPanel = document.getElementById('<%= pnlApproval.ClientID %>');
            if (approvalPanel && approvalPanel.style.display !== 'none') {
                approvalPanel.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }

            // Smooth scroll to history panel when visible
            const historyPanel = document.getElementById('<%= pnlHistory.ClientID %>');
            if (historyPanel && historyPanel.style.display !== 'none') {
                historyPanel.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        });
    </script>
</asp:Content>
