<%@ Page Title="CİMER Takip Edilen Başvurular" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="Incelenen.aspx.cs" Inherits="Portal.ModulCimer.Incelenen" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="ContentBreadcrumb" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <!--  Enhanced breadcrumb -->
    <li class="breadcrumb-item">
        <i class="fas fa-comments me-1"></i>CİMER
    </li>
    <li class="breadcrumb-item active" aria-current="page">Takip Edilen Başvurular</li>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <!--  Main GridView Card -->
        <div class="row">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header">
                        <div>
                            <i class="fas fa-bookmark"></i>
                            <span>Takibimdeki CİMER Başvuruları</span>
                        </div>
                    </div>
                    <div class="card-body">
                        <!--  Info badge -->
                        <div class="info-badge">
                            <i class="fas fa-info-circle"></i>
                            Bekleme durumu "Evet" olan ve sizin takibinizde bulunan başvuruları görmektesiniz.
                       
                        </div>

                        <!--  Action bar -->
                        <div class="action-buttons mb-3">
                            <div class="d-flex justify-content-end">
                                <asp:Button ID="btnExcel" runat="server" Text="📊 Excel'e Aktar"
                                    CssClass="btn btn-success" OnClick="btnExcel_Click" />
                            </div>
                        </div>

                        <!--  GridView with enhanced table styling -->
                        <div class="grid-container">
                            <asp:GridView ID="GridViewBasvurular" runat="server" CssClass="table table-striped table-hover mb-0"
                                AutoGenerateColumns="False" DataKeyNames="id"
                                OnPageIndexChanging="GridViewBasvurular_PageIndexChanging"
                                OnSelectedIndexChanged="GridViewBasvurular_SelectedIndexChanged"
                                EmptyDataText="Takip ettiğiniz başvuru bulunmamaktadır."
                                AllowPaging="True" PageSize="10">
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" ButtonType="Button" SelectText="Seç"
                                        ControlStyle-CssClass="btn btn-sm btn-primary" />
                                    <asp:BoundField DataField="id" HeaderText="No" ItemStyle-CssClass="hidden-column"
                                        HeaderStyle-CssClass="hidden-column" />
                                    <asp:BoundField DataField="Basvuru_No" HeaderText="Başvuru No" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Basvuru_Tarihi" HeaderText="Başvuru Tarihi"
                                        DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="TC_No" HeaderText="TC Kimlik No"
                                        ItemStyle-CssClass="hidden-column" HeaderStyle-CssClass="hidden-column" />
                                    <asp:BoundField DataField="Adi_Soyadi" HeaderText="Adı Soyadı"
                                        ItemStyle-CssClass="hidden-column" HeaderStyle-CssClass="hidden-column" />
                                    <asp:BoundField DataField="Tel_No" HeaderText="Telefon"
                                        ItemStyle-CssClass="hidden-column" HeaderStyle-CssClass="hidden-column" />
                                    <asp:BoundField DataField="Mail" HeaderText="E-posta"
                                        ItemStyle-CssClass="hidden-column" HeaderStyle-CssClass="hidden-column" />
                                    <asp:BoundField DataField="Adres" HeaderText="Adres"
                                        ItemStyle-CssClass="hidden-column" HeaderStyle-CssClass="hidden-column" />
                                    <asp:BoundField DataField="Basvuru_Metni" HeaderText="Başvuru Metni"
                                        ItemStyle-Width="300px" />
                                    <asp:HyperLinkField DataTextField="Basvuru_Ek" HeaderText="Ek Dosya"
                                        DataNavigateUrlFields="Basvuru_Ek" Target="_blank"
                                        DataTextFormatString="📎 İndir"
                                        ControlStyle-CssClass="badge bg-info text-decoration-none" />
                                    <asp:BoundField DataField="Yapilan_İslem" HeaderText="Yapılan İşlem" />
                                    <asp:BoundField DataField="Son_Yapilan_islem" HeaderText="Son İşlem" />
                                    <asp:BoundField DataField="Sikayet_Edilen_Firma" HeaderText="Şikayet Edilen Firma" />
                                    <asp:BoundField DataField="Guncelleyen_Kullanici" HeaderText="İşlem Yapan" />
                                    <asp:BoundField DataField="Guncelleme_Tarihi" HeaderText="İşlem Tarihi"
                                        DataFormatString="{0:dd/MM/yyyy HH:mm}" />
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

        <!--  Detail Panel (Enhanced) -->
        <asp:Panel ID="pnlDetay" runat="server" CssClass="row detail-panel">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header success">
                        <div>
                            <i class="fas fa-edit"></i>
                            <span>Başvuru Detayları ve Güncelleme</span>
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
                                    <div class="form-group">
                                        <label class="form-label">Başvuru No</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-hashtag"></i>
                                            </span>
                                            <asp:TextBox ID="txtBasvuruNo" runat="server" CssClass="form-control"
                                                ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <!--  Firma Ünvanı -->
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="form-label">Şikayet Edilen Firma</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-building"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlFirmalar" runat="server" CssClass="form-select">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <!--  Başvuru Metni -->
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label class="form-label">Başvuru Metni</label>
                                        <div class="input-group">
                                            <span class="input-group-text align-items-start pt-2">
                                                <i class="fas fa-file-alt"></i>
                                            </span>
                                            <asp:TextBox ID="txtBasvuruMetni" runat="server" CssClass="form-control"
                                                TextMode="MultiLine" Rows="4" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <!--  Yapılan İşlem -->
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label class="form-label">Yapılan İşlem</label>
                                        <div class="input-group">
                                            <span class="input-group-text align-items-start pt-2">
                                                <i class="fas fa-tasks"></i>
                                            </span>
                                            <asp:TextBox ID="txtYapilanIslem" runat="server" CssClass="form-control"
                                                TextMode="MultiLine" Rows="4" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-section">
                            <div class="section-title">
                                <i class="fas fa-cogs"></i>
                                <span>Güncelleme Bilgileri</span>
                            </div>

                            <div class="row g-3">
                                <!--  Son Yapılan İşlem -->
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label class="form-label">Son Yapılan İşlem</label>
                                        <div class="input-group">
                                            <span class="input-group-text align-items-start pt-2">
                                                <i class="fas fa-check-circle"></i>
                                            </span>
                                            <asp:TextBox ID="txtSonYapilanIslem" runat="server" CssClass="form-control"
                                                TextMode="MultiLine" Rows="4"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <!--  Açıklama -->
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label class="form-label">Açıklama</label>
                                        <div class="input-group">
                                            <span class="input-group-text align-items-start pt-2">
                                                <i class="fas fa-comment-dots"></i>
                                            </span>
                                            <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control"
                                                TextMode="MultiLine" Rows="3"
                                                placeholder="Güncelleme ile ilgili açıklama girebilirsiniz..."></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <!--  Onay Kullanıcısı -->
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="form-label required-field">Onay Kullanıcısı</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-user-check"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlOnayKullanici" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="" Text="-- Seçiniz --"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvOnayKullanici" runat="server"
                                            ControlToValidate="ddlOnayKullanici"
                                            ErrorMessage="Onay kullanıcısını seçiniz."
                                            CssClass="text-danger small d-block mt-1"
                                            Display="Dynamic" ValidationGroup="vgGuncelle" />
                                    </div>
                                </div>

                                <!--  Durum -->
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="form-label">Durum</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-flag-checkered"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="Yeni Kayıt Açıldı">Yeni Kayıt Açıldı</asp:ListItem>
                                                <asp:ListItem Value="İnceleniyor">İnceleniyor</asp:ListItem>
                                                <asp:ListItem Value="Sonuçlandı">Sonuçlandı</asp:ListItem>
                                                <asp:ListItem Value="Cevap Verildi">Cevap Verildi</asp:ListItem>
                                                <asp:ListItem Value="Süreç Devam Ediyor">Süreç Devam Ediyor</asp:ListItem>
                                                <asp:ListItem Value="Tekrar Cevap Verildi" Selected="True">Tekrar Cevap Verildi</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--  Action Buttons -->
                        <div class="action-buttons">
                            <div class="d-flex flex-wrap gap-2 justify-content-end">
                                <asp:Button ID="btnTarihce" runat="server" Text="📜 Evrak Geçmişi"
                                    CssClass="btn btn-secondary" OnClick="btnTarihce_Click" />
                                <asp:Button ID="btnKapat" runat="server" Text="✕ Kapat"
                                    CssClass="btn btn-outline-secondary" OnClick="btnKapat_Click" />
                                <asp:Button ID="btnKaydet" runat="server" Text="💾 Kaydet ve Onaya Gönder"
                                    CssClass="btn btn-success" OnClick="btnKaydet_Click"
                                    ValidationGroup="vgGuncelle" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!--  History Panel (Enhanced) -->
        <asp:Panel ID="pnlTarihce" runat="server" CssClass="row history-panel">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header info">
                        <div>
                            <i class="fas fa-history"></i>
                            <span>Evrak Geçmişi</span>
                        </div>
                        <asp:Button ID="btnTarihceKapat" runat="server" Text="✕ Kapat"
                            CssClass="btn btn-sm btn-outline-light" OnClick="btnTarihceKapat_Click" />
                    </div>
                    <div class="card-body">
                        <div class="info-badge mb-3">
                            <i class="fas fa-info-circle"></i>
                            Bu başvurunun tüm hareket geçmişini görüntülemektesiniz.
                       
                        </div>

                        <div class="table-responsive">
                            <asp:Literal ID="litTarihce" runat="server"></asp:Literal>
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

            // Smooth scroll to detail panel when visible
            const detailPanel = document.querySelector('.detail-panel.show');
            if (detailPanel) {
                detailPanel.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }

            // Smooth scroll to history panel when visible
            const historyPanel = document.querySelector('.history-panel.show');
            if (historyPanel) {
                historyPanel.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        });
    </script>
</asp:Content>
