<%@ Page Title="CİMER Devam Eden Başvurular" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="DevamEden.aspx.cs" Inherits="Portal.ModulCimer.DevamEden" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <!--  Enhanced breadcrumb -->
    <li class="breadcrumb-item">
        <i class="fas fa-comments me-1"></i>CİMER
    </li>
    <li class="breadcrumb-item active" aria-current="page">Devam Eden Başvurular</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <!--  Main GridView Card -->
        <div class="row">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header">
                        <div>
                            <i class="fas fa-hourglass-half"></i>
                            <span>CİMER Süreci Devam Eden Başvurular</span>
                        </div>
                    </div>
                    <div class="card-body">
                        <!--  Info badge -->
                        <div class="info-badge">
                            <i class="fas fa-info-circle"></i>
                            Süreci devam eden başvuruları görmektesiniz. 15 günü aşan başvurular kırmızı ile vurgulanmıştır.
                        </div>

                        <!--  Action bar -->
                        <div class="action-buttons">
                            <div class="d-flex justify-content-end">
                                <asp:Button ID="btnExportToExcel" runat="server" Text="📊 Excel'e Aktar"
                                    CssClass="btn btn-success" OnClick="btnExportToExcel_Click" />
                            </div>
                        </div>

                        <!--  GridView with enhanced table styling -->
                        <div class="grid-container">
                            <asp:GridView ID="GridViewDevamEden" runat="server"
                                CssClass="table table-striped table-hover mb-0"
                                AutoGenerateColumns="False"
                                DataKeyNames="id"
                                OnSelectedIndexChanged="GridViewDevamEden_SelectedIndexChanged"
                                EmptyDataText="Devam eden başvuru bulunmamaktadır."
                                AllowPaging="False"
                                ShowHeaderWhenEmpty="True">
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" ButtonType="Button" SelectText="Seç"
                                        ControlStyle-CssClass="btn btn-sm btn-primary" />
                                    <asp:BoundField DataField="id" HeaderText="No" ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="Basvuru_No" HeaderText="Başvuru No" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Basvuru_Tarihi" HeaderText="CİMER Başvuru Tarihi"
                                        DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" ItemStyle-Width="140px" />
                                    <asp:BoundField DataField="Kayit_Tarihi" HeaderText="Kayıt Tarihi"
                                        DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="Bekleme_Durumu" HeaderText="Bekleme Durumu" ItemStyle-Width="120px" />
                                    <asp:TemplateField HeaderText="Süre (Gün)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSure" runat="server"
                                                Text='<%# Eval("Sure") %>'
                                                CssClass='<%# Convert.ToInt32(Eval("Sure")) > 15 ? "duration-danger" : (Convert.ToInt32(Eval("Sure")) > 10 ? "duration-warning" : "duration-normal") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-center" />
                                        <ItemStyle CssClass="text-center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Basvuru_Metni" HeaderText="Başvuru Metni"
                                        ItemStyle-Width="300px" />
                                    <asp:HyperLinkField DataNavigateUrlFields="Basvuru_Ek"
                                        DataTextField="Basvuru_Ek"
                                        DataTextFormatString="📎 İndir"
                                        HeaderText="Ek"
                                        Target="_blank"
                                        ControlStyle-CssClass="badge bg-info text-decoration-none" />
                                    <asp:BoundField DataField="Yapilan_İslem" HeaderText="Yapılan İşlem" />
                                    <asp:BoundField DataField="Son_Yapilan_islem" HeaderText="Son İşlem" />
                                    <asp:BoundField DataField="Son_Kullanici" HeaderText="Son Kullanıcı" />
                                    <asp:BoundField DataField="TC_No" HeaderText="TC Kimlik" Visible="False" />
                                    <asp:BoundField DataField="Adi_Soyadi" HeaderText="Adı Soyadı" Visible="False" />
                                    <asp:BoundField DataField="Tel_No" HeaderText="Cep Telefonu" Visible="False" />
                                    <asp:BoundField DataField="Mail" HeaderText="Mail Adresi" Visible="False" />
                                    <asp:BoundField DataField="Adres" HeaderText="Adres" Visible="False" />
                                    <asp:BoundField DataField="Sikayet_Edilen_Firma" HeaderText="Şikayete Konu Firma" />
                                    <asp:BoundField DataField="Guncelleyen_Kullanici" HeaderText="İşlem Yapan Kullanıcı" />

                                </Columns>
                                <EmptyDataRowStyle CssClass="text-center text-muted p-4" />
                                <RowStyle CssClass="align-middle" />
                                <SelectedRowStyle CssClass="table-active fw-semibold" />
                                <PagerStyle CssClass="pagination justify-content-center mt-3" />
                            </asp:GridView>
                        </div>

                        <!--  Duration Legend -->
                        <div class="warning-badge-duration">
                            <i class="fas fa-exclamation-triangle"></i>
                            <strong>Süre Gösterimi:</strong>
                            <span class="duration-normal ms-2">● 0-10 gün (Normal)</span>
                            <span class="duration-warning ms-2">● 11-15 gün (Dikkat)</span>
                            <span class="duration-danger ms-2">● 15+ gün (Acil)</span>
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

        <!--  History Panel (Enhanced) -->
        <asp:Panel ID="PanelGecmis" runat="server" Visible="False" CssClass="row mt-4 history-panel">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header info">
                        <div>
                            <i class="fas fa-history"></i>
                            <span>Başvuru Evrak Geçmişi</span>
                        </div>
                        <button type="button" class="btn btn-sm btn-outline-light"
                            onclick="document.getElementById('<%= PanelGecmis.ClientID %>').style.display='none';">
                            ✕ Kapat
                        </button>
                    </div>
                    <div class="card-body">
                        <div class="info-badge mb-3">
                            <i class="fas fa-info-circle"></i>
                            Bu başvurunun tüm hareket geçmişini görüntülemektesiniz.
                        </div>

                        <div class="table-responsive">
                            <asp:Label ID="lblGecmisTablo" runat="server"></asp:Label>
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

            // Smooth scroll to history panel when visible
            const historyPanel = document.querySelector('.history-panel');
            if (historyPanel && historyPanel.style.display !== 'none') {
                historyPanel.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        });
    </script>
</asp:Content>
