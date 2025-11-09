<%@ Page Title="CİMER Başvuru Takip" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="Takip.aspx.cs" Inherits="Portal.ModulCimer.Takip" EnableEventValidation="false" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">    
    
</asp:Content>

<asp:Content ID="ContentBreadcrumb" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <!--  Enhanced breadcrumb -->
    <li class="breadcrumb-item">
        <i class="fas fa-comments me-1"></i>CİMER
    </li>
    <li class="breadcrumb-item active" aria-current="page">Başvuru Takip ve Raporlama</li>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <!--  Filter Panel (Enhanced) -->
        <div class="row">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header">
                        <div>
                            <i class="fas fa-search"></i>
                            <span>CİMER Başvuru Arama ve Filtreleme</span>
                        </div>
                    </div>
                    <div class="card-body">
                        <!--  Info badge -->
                        <div class="info-badge">
                            <i class="fas fa-info-circle"></i>
                            Arama kriterlerinizi girerek başvuruları filtreleyebilir ve detaylı raporlama yapabilirsiniz.
                        </div>

                        <div class="form-section">
                            <div class="section-title">
                                <i class="fas fa-filter"></i>
                                <span>Arama Kriterleri</span>
                            </div>

                            <div class="row g-3">
                                <!--  Başvuru No -->
                                <div class="col-md-3">
                                    <label class="form-label">Başvuru No</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-hashtag"></i>
                                        </span>
                                        <asp:TextBox ID="txtBasvuruNo" runat="server" CssClass="form-control" 
                                            placeholder="Başvuru No giriniz"></asp:TextBox>
                                    </div>
                                </div>

                                <!--  Adı Soyadı -->
                                <div class="col-md-3">
                                    <label class="form-label">Adı Soyadı</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-user"></i>
                                        </span>
                                        <asp:TextBox ID="txtAdiSoyadi" runat="server" CssClass="form-control" 
                                            placeholder="Adı Soyadı giriniz"></asp:TextBox>
                                    </div>
                                </div>

                                <!--  Firma -->
                                <div class="col-md-3">
                                    <label class="form-label">Şikayete Konu Firma</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-building"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlFirmalar" runat="server" CssClass="form-select" 
                                            AppendDataBoundItems="true">
                                            <asp:ListItem Value="" Text="-- Hepsi --" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <!--  Durum -->
                                <div class="col-md-3">
                                    <label class="form-label">Durum</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-flag-checkered"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-select">
                                            <asp:ListItem Value="" Text="-- Hepsi --" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="Yeni Kayıt Açıldı">Yeni Kayıt Açıldı</asp:ListItem>
                                            <asp:ListItem Value="inceleniyor">İnceleniyor</asp:ListItem>
                                            <asp:ListItem Value="Sonuçlandı">Sonuçlandı</asp:ListItem>
                                            <asp:ListItem Value="Cevap Verildi">Cevap Verildi</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--  Action Buttons -->
                        <div class="action-buttons">
                            <div class="d-flex flex-wrap gap-2 justify-content-center">
                                <asp:Button ID="btnFiltrele" runat="server" 
                                    Text="🔍 Filtrele" 
                                    CssClass="btn btn-primary" 
                                    OnClick="btnFiltrele_Click" />
                                <asp:Button ID="btnHareket" runat="server" 
                                    Text="📜 Evrak Geçmişi" 
                                    CssClass="btn btn-info" 
                                    OnClick="btnHareket_Click" 
                                    Visible="false" />
                                <asp:Button ID="btnExcelAktar" runat="server" 
                                    Text="📊 Excel'e Aktar" 
                                    CssClass="btn btn-success" 
                                    OnClick="btnExcelAktar_Click" />
                                <asp:Button ID="btnTemizle" runat="server" 
                                    CssClass="btn btn-outline-secondary" 
                                    OnClick="btnTemizle_Click" 
                                    CausesValidation="false" 
                                    Text="🗑️ Temizle" />
                            </div>
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

        <!--  GridView Panel (Enhanced) -->
        <div class="row">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header">
                        <div>
                            <i class="fas fa-list-alt"></i>
                            <span>CİMER Başvuruları Listesi</span>
                        </div>
                    </div>
                    <div class="card-body">
                        <!--  Results badge -->
                        <asp:Label ID="lblToplamKayit" runat="server" 
                            CssClass="stats-badge" 
                            Visible="false">
                        </asp:Label>

                        <!--  GridView with enhanced table styling -->
                        <div class="grid-container mt-3">
                            <asp:GridView ID="gvCimerBasvurular" runat="server" 
                                CssClass="table table-striped table-hover mb-0" 
                                AutoGenerateColumns="False" 
                                AllowPaging="True" 
                                PageSize="20" 
                                OnPageIndexChanging="gvCimerBasvurular_PageIndexChanging"
                                OnSelectedIndexChanged="gvCimerBasvurular_SelectedIndexChanged"
                                EmptyDataText="Arama kriterlerinize uygun kayıt bulunamadı."
                                ShowHeaderWhenEmpty="True">
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" ButtonType="Button" SelectText="Seç" 
                                        ControlStyle-CssClass="btn btn-sm btn-primary" />
                                    <asp:BoundField DataField="id" HeaderText="No" 
                                        ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" 
                                        ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="Basvuru_No" HeaderText="Başvuru No" 
                                        ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Basvuru_Metni" HeaderText="Başvuru Metni" 
                                        ItemStyle-Width="300px" HtmlEncode="False" />
                                    <asp:TemplateField HeaderText="Ek Dosya">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlEk" runat="server" 
                                                Text="📎 İndir" 
                                                NavigateUrl='<%# Eval("Basvuru_Ek") %>' 
                                                Target="_blank" 
                                                CssClass="badge bg-info text-decoration-none" 
                                                Visible='<%# !string.IsNullOrEmpty(Eval("Basvuru_Ek").ToString()) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Yapilan_İslem" HeaderText="Yapılan İşlem" />
                                    <asp:BoundField DataField="Sikayet_Konusu" HeaderText="Şikayet Konusu" />
                                    <asp:BoundField DataField="Sikayet_Edilen_Firma" HeaderText="Şikayete Konu Firma" />
                                    <asp:BoundField DataField="Kayit_Kullanici" HeaderText="Kayıt Kullanıcı" 
                                        ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="Kayit_Tarihi" HeaderText="Kayıt Tarihi" 
                                        DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-Width="140px" />
                                    <asp:BoundField DataField="Guncelleyen_Kullanici" HeaderText="İşlem Yapan" 
                                        ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="Guncelleme_Tarihi" HeaderText="Cevaplama Tarihi" 
                                        DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-Width="140px" />
                                    <asp:BoundField DataField="Sonuc" HeaderText="Durum" 
                                        ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="Basvuru_Tarihi" HeaderText="CİMER Başvuru Tarihi" 
                                        DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="140px" />
                                    <asp:BoundField DataField="Son_Yapilan_islem" HeaderText="Son İşlem" />
                                    <asp:BoundField DataField="TC_No" HeaderText="TC Kimlik" 
                                        ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Adi_Soyadi" HeaderText="Adı Soyadı" />
                                    <asp:BoundField DataField="Tel_No" HeaderText="Telefon" 
                                        ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="Mail" HeaderText="E-posta" />
                                    <asp:BoundField DataField="Adres" HeaderText="Adres" 
                                        ItemStyle-Width="200px" />
                                </Columns>
                                <EmptyDataRowStyle CssClass="text-center text-muted p-4" />
                                <PagerStyle CssClass="pagination justify-content-center mt-3" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--  History Panel (Enhanced) -->
        <asp:Panel ID="pnlGecmis" runat="server" CssClass="row history-panel">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header info">
                        <div>
                            <i class="fas fa-history"></i>
                            <span>Evrak Geçmişi</span>
                        </div>
                        <asp:Button ID="btnGecmisKapat" runat="server" 
                            Text="✕ Kapat" 
                            CssClass="btn btn-sm btn-outline-light" 
                            OnClick="btnGecmisKapat_Click" />
                    </div>
                    <div class="card-body">
                        <div class="info-badge mb-3">
                            <i class="fas fa-info-circle"></i>
                            Bu başvurunun tüm hareket geçmişini görüntülemektesiniz.
                        </div>

                        <div class="table-responsive">
                            <asp:Literal ID="litGecmisTablo" runat="server"></asp:Literal>
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
            const historyPanel = document.getElementById('<%= pnlGecmis.ClientID %>');
            if (historyPanel && historyPanel.style.display !== 'none') {
                historyPanel.classList.add('show');
                historyPanel.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }

            // Auto-focus first input
            const firstInput = document.getElementById('<%= txtBasvuruNo.ClientID %>');
            if (firstInput) {
                firstInput.focus();
            }
        });
    </script>
</asp:Content>
