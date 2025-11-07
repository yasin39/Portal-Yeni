<%@ Page Title="Beklemede Olan Başvurular" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="Beklemede.aspx.cs" Inherits="Portal.ModulCimer.Beklemede" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <!--  Enhanced breadcrumb -->
    <li class="breadcrumb-item">
        <i class="fas fa-comments me-1"></i>CİMER
    </li>
    <li class="breadcrumb-item active" aria-current="page">Beklemede Olan Başvurular</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <!--  Main GridView Card -->
        <div class="row">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header">
                        <div>
                            <i class="fas fa-clock"></i>
                            <span>CİMER Beklemede Olan Başvurular</span>
                        </div>
                    </div>
                    <div class="card-body">
                        <!--  Info badge with stats -->
                        <div class="info-badge">
                            <i class="fas fa-info-circle"></i>
                            Bekleme durumunda olan başvuruları görmektesiniz. Bu başvurular için ek işlem veya yanıt beklenmektedir.
                        </div>

                        <!--  Warning badge -->
                        <div class="warning-badge">
                            <i class="fas fa-exclamation-triangle"></i>
                            <strong>DİKKAT:</strong> Bekleme süresi uzun olan başvurular için işlem yapılması önerilir.
                        </div>

                        <!--  Action bar -->
                        <div class="action-buttons mb-3">
                            <div class="d-flex justify-content-end">
                                <asp:Button ID="exceleaktar" runat="server"
                                    Text="📊 Excel'e Aktar"
                                    CssClass="btn btn-success"
                                    OnClick="exceleaktar_Click" />
                            </div>
                        </div>

                        <!--  GridView with enhanced table styling -->
                        <div class="grid-container">
                            <asp:GridView ID="GridView1" runat="server"
                                CssClass="table table-striped table-hover mb-0"
                                AutoGenerateColumns="False"
                                OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                ShowFooter="True"
                                AllowSorting="True"
                                EmptyDataText="Beklemede olan başvuru bulunamadı."
                                DataKeyNames="id">
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" ButtonType="Button" SelectText="Seç"
                                        ControlStyle-CssClass="btn btn-sm btn-primary" />
                                    <asp:BoundField DataField="id" HeaderText="No" ItemStyle-Width="50px"
                                        ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Basvuru_No" HeaderText="Başvuru No" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Basvuru_Tarihi" HeaderText="CİMER Başvuru Tarihi"
                                        DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="140px"
                                        ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Bekleme_Durumu" HeaderText="Bekleme Durumu"
                                        ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Basvuru_Metni" HeaderText="Başvuru Metni"
                                        ItemStyle-Width="400px" />
                                    <asp:HyperLinkField DataNavigateUrlFields="Basvuru_Ek" DataTextField="Basvuru_Ek"
                                        DataTextFormatString="📎 İndir" HeaderText="Ek" Target="_blank"
                                        ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"
                                        ControlStyle-CssClass="badge bg-info text-decoration-none" />
                                    <asp:BoundField DataField="Yapilan_İslem" HeaderText="Yapılan İşlem" />
                                    <asp:BoundField DataField="Son_Yapilan_islem" HeaderText="Son İşlem" />
                                    <asp:BoundField DataField="TC_No" HeaderText="TC Kimlik" Visible="False" />
                                    <asp:BoundField DataField="Adi_Soyadi" HeaderText="Adı Soyadı" Visible="False" />
                                    <asp:BoundField DataField="Tel_No" HeaderText="Cep Telefonu" Visible="False" />
                                    <asp:BoundField DataField="Mail" HeaderText="Mail Adresi" Visible="False" />
                                    <asp:BoundField DataField="Adres" HeaderText="Adres" Visible="False" />
                                    <asp:BoundField DataField="Sikayet_Edilen_Firma" HeaderText="Şikayete Konu Firma" />
                                    <asp:BoundField DataField="Guncelleyen_Kullanici" HeaderText="İşlem Yapan Kullanıcı" />

                                </Columns>
                                <EmptyDataRowStyle CssClass="text-center text-muted p-4" />
                                <FooterStyle CssClass="bg-dark text-white font-weight-bold" />
                                <PagerStyle CssClass="pagination justify-content-center mt-3" />
                                <SelectedRowStyle CssClass="table-active" />
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

        <!--  History Panel (Enhanced) -->
        <div id="gecmisbolumu" runat="server" visible="false" class="row mt-4 history-panel">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header info">
                        <div>
                            <i class="fas fa-history"></i>
                            <span>Evrak Geçmişi</span>
                        </div>
                        <button id="gecmiskapat" runat="server" class="btn btn-sm btn-outline-light"
                            type="button" onclick="document.getElementById('<%= gecmisbolumu.ClientID %>').style.display='none';">
                            ✕ Kapat
                        </button>
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
        </div>
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
