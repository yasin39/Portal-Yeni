<%@ Page Title="CİMER Cevapladıklarım" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="Cevapladiklarim.aspx.cs" Inherits="Portal.ModulCimer.Cevapladiklarim" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <!--  Enhanced breadcrumb -->
    <li class="breadcrumb-item">
        <i class="fas fa-comments me-1"></i>CİMER
    </li>
    <li class="breadcrumb-item active" aria-current="page">Cevapladıklarım</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <!--  Main GridView Card -->
        <div class="row">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header">
                        <div>
                            <i class="fas fa-edit"></i>
                            <span>CİMER Cevapladığım Başvurular</span>
                        </div>
                    </div>
                    <div class="card-body">
                        <!--  Info badge -->
                        <div class="info-badge">
                            <i class="fas fa-info-circle"></i>
                            Cevaplamış olduğunuz tüm başvuruları görmektesiniz. Detaylı inceleme için "Detay" butonunu kullanınız.
                        </div>

                        <!--  Action bar -->
                        <div class="action-buttons mb-3">
                            <div class="d-flex justify-content-end">
                                <asp:Button ID="btnExcelAktar" runat="server" Text="📊 Excel'e Aktar"
                                    CssClass="btn btn-success" OnClick="ExcelAktar_Click" />
                            </div>
                        </div>

                        <!--  GridView with enhanced table styling -->
                        <div class="grid-container">
                            <asp:GridView ID="GridView1" runat="server"
                                CssClass="table table-striped table-hover mb-0"
                                AutoGenerateColumns="False"
                                OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                AllowPaging="True" PageSize="20"
                                EmptyDataText="Henüz cevapladığınız başvuru bulunmamaktadır."
                                DataKeyNames="id">
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" ButtonType="Button" SelectText="📋 Detay"
                                        ControlStyle-CssClass="btn btn-sm btn-primary" />
                                    <asp:BoundField DataField="id" HeaderText="No" ItemStyle-Width="50px"
                                        ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Basvuru_No" HeaderText="Başvuru No" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Basvuru_Tarihi" HeaderText="Başvuru Tarihi"
                                        DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="Basvuru_Metni" HeaderText="Başvuru Metni"
                                        ItemStyle-Width="300px" />
                                    <asp:HyperLinkField DataNavigateUrlFields="Basvuru_Ek" DataTextField="Basvuru_Ek"
                                        DataTextFormatString="📎 İndir" HeaderText="Ek" Target="_blank"
                                        ItemStyle-CssClass="text-center"
                                        ControlStyle-CssClass="badge bg-info text-decoration-none" />
                                    <asp:BoundField DataField="Yapilan_İslem" HeaderText="Yapılan İşlem" />
                                    <asp:BoundField DataField="TC_No" HeaderText="TC Kimlik" Visible="False" />
                                    <asp:BoundField DataField="Adi_Soyadi" HeaderText="Adı Soyadı" />
                                    <asp:BoundField DataField="Tel_No" HeaderText="Cep Telefonu" Visible="False" />
                                    <asp:BoundField DataField="Mail" HeaderText="Mail Adresi" Visible="False" />
                                    <asp:BoundField DataField="Adres" HeaderText="Adres" Visible="False" />
                                    <asp:BoundField DataField="Sikayet_Konusu" HeaderText="Konu" Visible="False" />
                                    <asp:BoundField DataField="Sikayet_Edilen_Firma" HeaderText="Şikayete Konu Firma" />
                                    <asp:BoundField DataField="Kayit_Kullanici" HeaderText="Kayıt Kullanıcı" />
                                    <asp:BoundField DataField="Kayit_Tarihi" HeaderText="Kayıt Tarihi"
                                        DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-Width="140px" />
                                    <asp:BoundField DataField="Guncelleyen_Kullanici" HeaderText="İşlem Yapan Kullanıcı" />
                                    <asp:BoundField DataField="Guncelleme_Tarihi" HeaderText="Cevaplama Tarihi"
                                        DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-Width="140px" />
                                    <asp:BoundField DataField="Sonuc" HeaderText="Durum" />
                                    <asp:BoundField DataField="Son_Yapilan_islem" HeaderText="Son İşlem" />
                                </Columns>
                                <EmptyDataRowStyle CssClass="text-center text-muted p-4" />
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
        <div id="gecmisBolumu" runat="server" visible="false" class="row mt-4 history-panel">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header info">
                        <div>
                            <i class="fas fa-history"></i>
                            <span>Evrak Geçmişi</span>
                        </div>
                        <asp:Button ID="btnGecmisKapat" runat="server" Text="✕ Kapat"
                            CssClass="btn btn-sm btn-outline-light" OnClick="GecmisKapat_Click" />
                    </div>
                    <div class="card-body">
                        <div class="info-badge mb-3">
                            <i class="fas fa-info-circle"></i>
                            Bu başvurunun tüm hareket geçmişini görüntülemektesiniz.
                        </div>

                        <asp:Label ID="lblTable" runat="server" CssClass="d-none"></asp:Label>
                        <div id="gecmisTableContainer" runat="server" class="table-responsive"></div>
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