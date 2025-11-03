<%@ Page Title="CİMER Biten Başvurular" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="Biten.aspx.cs" Inherits="Portal.ModulCimer.Biten" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <!--  Enhanced breadcrumb -->
    <li class="breadcrumb-item">
        <i class="fas fa-comments me-1"></i>CİMER
    </li>
    <li class="breadcrumb-item active" aria-current="page">Biten Başvurular</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <!--  Main GridView Card -->
        <div class="row">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header">
                        <div>
                            <i class="fas fa-check-circle"></i>
                            <span>CİMER Süreci Biten Başvurular</span>
                        </div>
                    </div>
                    <div class="card-body">
                        <!--  Info badge -->
                        <div class="info-badge">
                            <i class="fas fa-info-circle"></i>
                            Süreci tamamlanmış ve onaylanmış başvuruları görmektesiniz. Gerekirse kapatma işlemi yapabilirsiniz.
                        </div>

                        <!--  Action bar -->
                        <div class="action-buttons mb-3">
                            <div class="d-flex justify-content-end">
                                <asp:Button ID="btnExcelAktar" runat="server" 
                                    Text="📊 Excel'e Aktar" 
                                    CssClass="btn btn-success" 
                                    OnClick="btnExcelAktar_Click" />
                            </div>
                        </div>

                        <!--  GridView with enhanced table styling -->
                        <div class="grid-container">
                            <asp:GridView ID="gvBitenBasvurular" runat="server" 
                                CssClass="table table-striped table-hover mb-0" 
                                AutoGenerateColumns="False" 
                                OnSelectedIndexChanged="gvBitenBasvurular_SelectedIndexChanged" 
                                AllowPaging="false" 
                                EmptyDataText="Biten başvuru bulunamadı."
                                DataKeyNames="id">
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" ButtonType="Button" SelectText="Seç" 
                                                     ControlStyle-CssClass="btn btn-sm btn-primary" />
                                    <asp:BoundField DataField="id" HeaderText="ID" Visible="False" />
                                    <asp:BoundField DataField="Basvuru_No" HeaderText="Başvuru No" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Basvuru_Metni" HeaderText="Başvuru Metni" 
                                                   ItemStyle-Width="300px" HtmlEncode="false" />
                                    <asp:HyperLinkField DataTextField="Basvuru_Ek" DataNavigateUrlFields="Basvuru_Ek" 
                                                       HeaderText="Ek Dosya" DataTextFormatString="📎 İndir" Target="_blank"
                                                       ControlStyle-CssClass="badge bg-info text-decoration-none" />
                                    <asp:BoundField DataField="Yapilan_İslem" HeaderText="Yapılan İşlem" />
                                    <asp:BoundField DataField="Son_Yapilan_islem" HeaderText="Son Yapılan İşlem" />
                                    <asp:BoundField DataField="TC_No" HeaderText="TC Kimlik" Visible="False" />
                                    <asp:BoundField DataField="Adi_Soyadi" HeaderText="Adı Soyadı" />
                                    <asp:BoundField DataField="Tel_No" HeaderText="Cep Telefonu" ItemStyle-Width="120px" Visible="False" />
                                    <asp:BoundField DataField="Mail" HeaderText="Mail Adresi" Visible="False" />
                                    <asp:BoundField DataField="Adres" HeaderText="Adres" Visible="False" />
                                    <asp:BoundField DataField="Sikayet_Konusu" HeaderText="Konu" Visible="False" />
                                    <asp:BoundField DataField="Sikayet_Edilen_Firma" HeaderText="Şikayete Konu Firma" />
                                    <asp:BoundField DataField="Kayit_Kullanici" HeaderText="Kayıt Kullanıcı" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="Kayit_Tarihi" HeaderText="Kayıt Tarihi" 
                                                   DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-Width="140px" />
                                    <asp:BoundField DataField="Guncelleyen_Kullanici" HeaderText="İşlem Yapan Kullanıcı" ItemStyle-Width="150px" />
                                    <asp:BoundField DataField="Guncelleme_Tarihi" HeaderText="Cevaplama Tarihi" 
                                                   DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-Width="140px" />
                                    <asp:BoundField DataField="Sonuc" HeaderText="Durum" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="Basvuru_Tarihi" HeaderText="CİMER Başvuru Tarihi" 
                                                   DataFormatString="{0:dd/MM/yyyy}" NullDisplayText="-" ItemStyle-Width="140px" />
                                    <asp:BoundField DataField="Bekleme_Durumu" HeaderText="Bekleme Durumu" ItemStyle-Width="120px" />
                                </Columns>
                                <EmptyDataRowStyle CssClass="text-center text-muted p-4" />
                                <PagerStyle CssClass="pagination justify-content-center mt-3" />
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

        <!--  Havale Bölümü Panel (Enhanced) -->
        <asp:Panel ID="pnlHavaleBolumu" runat="server" Visible="false" CssClass="row mt-4">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header success">
                        <div>
                            <i class="fas fa-edit"></i>
                            <span>Başvuru Detayları ve Kapatma İşlemi</span>
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

                                <!--  Adı Soyadı -->
                                <div class="col-md-6">
                                    <label class="form-label">Adı Soyadı</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-user"></i>
                                        </span>
                                        <asp:TextBox ID="txtAdSoyad" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <!--  Şikayete Konu Firma -->
                                <div class="col-md-6">
                                    <label class="form-label">Şikayete Konu Firma</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-building"></i>
                                        </span>
                                        <asp:TextBox ID="txtFirma" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <!--  Mevcut Bekleme Durumu -->
                                <div class="col-md-6">
                                    <label class="form-label">Mevcut Bekleme Durumu</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-clock"></i>
                                        </span>
                                        <asp:TextBox ID="txtMevcutBekleme" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-section">
                            <div class="section-title">
                                <i class="fas fa-cogs"></i>
                                <span>Kapatma Bilgileri</span>
                            </div>

                            <div class="row g-3">
                                <!--  Durum -->
                                <div class="col-md-6">
                                    <label class="form-label">Durum</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-flag-checkered"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-select">
                                            <asp:ListItem Value="Yeni Kayıt Açıldı">Yeni Kayıt Açıldı</asp:ListItem>
                                            <asp:ListItem Value="inceleniyor">İnceleniyor</asp:ListItem>
                                            <asp:ListItem Value="Sonuçlandı">Sonuçlandı</asp:ListItem>
                                            <asp:ListItem Value="Cevap Verildi" Selected="True">Cevap Verildi</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <!--  Süreç Devam Edecek mi? -->
                                <div class="col-md-6">
                                    <label class="form-label required-field">Süreç Devam Edecek mi?</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-question-circle"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlSurecDurum" runat="server" CssClass="form-select" AppendDataBoundItems="True">
                                            <asp:ListItem Value="">-- Seçiniz --</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvSurecDurum" runat="server" 
                                        ErrorMessage="Süreç durumunu seçiniz." 
                                        CssClass="text-danger small d-block mt-1" 
                                        ControlToValidate="ddlSurecDurum" 
                                        ValidationGroup="kapatma" 
                                        Display="Dynamic" />
                                    <div class="warning-badge">
                                        <i class="fas fa-exclamation-triangle"></i>
                                        Bu seçim kaydın kapatılması için gereklidir.
                                    </div>
                                </div>

                                <!--  Açıklama -->
                                <div class="col-md-12">
                                    <label class="form-label">Açıklama</label>
                                    <div class="input-group">
                                        <span class="input-group-text align-items-start pt-2">
                                            <i class="fas fa-comment-dots"></i>
                                        </span>
                                        <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" 
                                                    TextMode="MultiLine" Rows="3" 
                                                    placeholder="Kapatma işlemi için açıklama girebilirsiniz..."></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--  Action Buttons -->
                        <div class="action-buttons">
                            <div class="d-flex flex-wrap gap-2 justify-content-end">
                                <asp:Button ID="btnEvrakGecmisi" runat="server" 
                                    Text="📜 Evrak Geçmişi" 
                                    CssClass="btn btn-info" 
                                    OnClick="btnEvrakGecmisi_Click" />
                                <asp:Button ID="btnKayitKapat" runat="server" 
                                    Text="🔒 Kaydı Kapat" 
                                    CssClass="btn btn-danger" 
                                    OnClick="btnKayitKapat_Click" 
                                    ValidationGroup="kapatma" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!--  Geçmiş Bölümü Panel (Enhanced) -->
        <asp:Panel ID="pnlGecmisBolumu" runat="server" Visible="false" CssClass="row mt-4">
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
                            <asp:Label ID="lblGecmisTable" runat="server"></asp:Label>
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

            // Smooth scroll to havale panel when visible
            const havalePanel = document.getElementById('<%= pnlHavaleBolumu.ClientID %>');
            if (havalePanel && havalePanel.style.display !== 'none') {
                havalePanel.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }

            // Smooth scroll to gecmis panel when visible
            const gecmisPanel = document.getElementById('<%= pnlGecmisBolumu.ClientID %>');
            if (gecmisPanel && gecmisPanel.style.display !== 'none') {
                gecmisPanel.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        });
    </script>
</asp:Content>