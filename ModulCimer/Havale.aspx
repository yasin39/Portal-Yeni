<%@ Page Title="Havale İşlemleri" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="Havale.aspx.cs" Inherits="Portal.ModulCimer.Havale" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <!--  Enhanced breadcrumb -->
    <li class="breadcrumb-item">
        <i class="fas fa-comments me-1"></i>CİMER
    </li>
    <li class="breadcrumb-item active">Havale İşlemleri</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <!--  Main GridView Card -->
        <div class="row">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header">
                        <div>
                            <i class="fas fa-inbox"></i>
                            <span>Havale Gelen Başvurular</span>
                        </div>
                    </div>
                    <div class="card-body">
                        <!--  Info badge -->
                        <div class="info-badge">
                            <i class="fas fa-info-circle"></i>
                            Size havale edilmiş ve işlem bekleyen başvuruları görmektesiniz.
                        </div>

                        <!--  GridView with modern table styling -->
                        <div class="table-responsive">
                            <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-hover mb-0" 
                                          AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                          DataKeyNames="id" AllowPaging="True" PageSize="20" 
                                          EmptyDataText="Henüz havale edilmiş başvuru bulunmamaktadır.">
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" ButtonType="Button" SelectText="Seç" 
                                                     ControlStyle-CssClass="btn btn-sm btn-primary" />
                                    <asp:BoundField DataField="id" HeaderText="No" ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="Basvuru_No" HeaderText="Başvuru No" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Basvuru_Tarihi" HeaderText="Başvuru Tarihi" 
                                                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="Adi_Soyadi" HeaderText="Adı Soyadı" />
                                    <asp:BoundField DataField="Basvuru_Metni" HeaderText="Başvuru Metni" 
                                                    ItemStyle-Width="300px" ItemStyle-Height="80px" HtmlEncode="False" />
                                    <asp:HyperLinkField DataNavigateUrlFields="Basvuru_Ek" DataTextField="Basvuru_Ek" 
                                                        DataTextFormatString="Ek İndir" HeaderText="Ek" Target="_blank" 
                                                        ItemStyle-Width="100px" ControlStyle-CssClass="badge bg-info text-decoration-none" />
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

                        <!--  Action buttons -->
                        <div class="action-buttons">
                            <div class="d-flex gap-2">
                                <asp:Button ID="btnTum" runat="server" CssClass="btn btn-secondary" 
                                            OnClick="btnTum_Click" CausesValidation="false" Text="📋 Tümünü Listele">                                    
                                </asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--  Havale Panel (Enhanced) -->
        <asp:Panel ID="pnlHavale" runat="server" Visible="false" CssClass="row mt-4">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header">
                        <div>
                            <i class="fas fa-share-square"></i>
                            <span>Başvuru Havale İşlemi</span>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="form-section">
                            <div class="row g-3">
                                <!--  Başvuru No -->
                                <div class="col-md-12">
                                    <label class="form-label">Başvuru No</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-hashtag"></i>
                                        </span>
                                        <asp:TextBox ID="txtBasvuruNo" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                </div>

                                <!--  Gereği İçin -->
                                <div class="col-md-12">
                                    <label class="form-label required-field">Gereği İçin</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-user-tag"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlKullanici" runat="server" CssClass="form-select">
                                        </asp:DropDownList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvKullanici" runat="server" ControlToValidate="ddlKullanici" 
                                                                ErrorMessage="Havale edilecek kullanıcı seçiniz." CssClass="text-danger small d-block mt-1" 
                                                                Display="Dynamic" ValidationGroup="havale" />
                                </div>

                                <!--  Açıklama -->
                                <div class="col-md-12">
                                    <label class="form-label">Açıklama</label>
                                    <div class="input-group">
                                        <span class="input-group-text align-items-start pt-2">
                                            <i class="fas fa-comment-dots"></i>
                                        </span>
                                        <asp:TextBox ID="txtAciklama" runat="server" TextMode="MultiLine" Rows="3" 
                                                     CssClass="form-control" placeholder="Havale ile ilgili açıklama girebilirsiniz..." />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--  Action Buttons -->
                        <div class="action-buttons">
                            <div class="d-flex flex-wrap gap-2">
                                <asp:Button ID="btnHavaleEt" runat="server" CssClass="btn btn-success" 
                                            OnClick="btnHavaleEt_Click" ValidationGroup="havale" Text="✓ Havale Et">                                    
                                </asp:Button>
                                <asp:Button ID="btnCevapYaz" runat="server" CssClass="btn btn-success" 
                                            OnClick="btnCevapYaz_Click" CausesValidation="false" Text="✏️ Cevap Yaz">                                    
                                </asp:Button>
                                <asp:Button ID="btnIade" runat="server" CssClass="btn btn-danger" 
                                            OnClick="btnIade_Click" Visible="false" CausesValidation="false" Text="↩️ CİMER Sevk/İade">                                
                                </asp:Button>
                                <asp:Button ID="btnGecmis" runat="server" CssClass="btn btn-secondary" 
                                            OnClick="btnGecmis_Click" CausesValidation="false" Text="📜 Evrak Geçmişi">
                                </asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!--  Cevapla Panel (Enhanced) -->
        <asp:Panel ID="pnlCevapla" runat="server" Visible="false" CssClass="row mt-4">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header warning">
                        <div>
                            <i class="fas fa-edit"></i>
                            <span>Başvuru Cevap İşlemi</span>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="form-section">
                            <div class="row g-3">
                                <!--  Başvuru No -->
                                <div class="col-md-6">
                                    <label class="form-label">Başvuru No</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-hashtag"></i>
                                        </span>
                                        <asp:TextBox ID="txtBasvuruNoCevap" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                </div>

                                <!--  Şikayet Edilen Firma -->
                                <div class="col-md-6">
                                    <label class="form-label">Şikayet Edilen Firma</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-building"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlFirmalar" runat="server" CssClass="form-select">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <!--  Yapılan İşlem -->
                                <div class="col-md-12">
                                    <label class="form-label">Yapılan İşlem</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-tasks"></i>
                                        </span>
                                        <asp:TextBox ID="txtYapilanIslem" runat="server" CssClass="form-control" 
                                                     placeholder="Yapılan işlem detayını giriniz..." />
                                    </div>
                                </div>

                                <!--  Son Yapılan İşlem -->
                                <div class="col-md-12">
                                    <label class="form-label">Son Yapılan İşlem</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-check-circle"></i>
                                        </span>
                                        <asp:TextBox ID="txtSonIslem" runat="server" CssClass="form-control" 
                                                     placeholder="Son yapılan işlem detayını giriniz..." />
                                    </div>
                                </div>

                                <!--  Sonuç -->
                                <div class="col-md-6">
                                    <label class="form-label">Sonuç</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-flag-checkered"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-select">
                                            <asp:ListItem Value="Yeni Kayıt Açıldı" />
                                            <asp:ListItem Value="İnceleniyor" />
                                            <asp:ListItem Value="Sonuçlandı" Selected="True" />
                                            <asp:ListItem Value="Cevap Verildi" />
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <!--  Bekleme Durumu -->
                                <div class="col-md-6">
                                    <label class="form-label">Bekleme Durumu</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-clock"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlBekleme" runat="server" CssClass="form-select">
                                            <asp:ListItem Value="" />
                                            <asp:ListItem Value="Evet" />
                                            <asp:ListItem Value="Hayır" />
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <!--  Onaylayıcı -->
                                <div class="col-md-12">
                                    <label class="form-label">Onaylayıcı</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-user-check"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlOnaylayici" runat="server" CssClass="form-select">
                                        </asp:DropDownList>
                                    </div>
                                    <small class="text-muted mt-1 d-block">
                                        <i class="fas fa-lightbulb me-1"></i>Cevabınızın onaylanması için yönetici seçiniz
                                    </small>
                                </div>
                            </div>
                        </div>

                        <!--  Action Button -->
                        <div class="action-buttons">
                            <asp:Button ID="btnKaydetCevap" runat="server" CssClass="btn btn-success" 
                                        OnClick="btnKaydetCevap_Click" ValidationGroup="cevap" Text="💾 Kaydet">
                                
                            </asp:Button>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!--  Geçmiş Panel (Enhanced) -->
        <asp:Panel ID="pnlGecmis" runat="server" Visible="false" CssClass="row mt-4">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header secondary">
                        <div>
                            <i class="fas fa-history"></i>
                            <span>Evrak Geçmişi</span>
                        </div>
                        <asp:Button ID="btnGecmisKapat" runat="server" CssClass="btn btn-sm btn-outline-light" 
                                    OnClick="btnGecmisKapat_Click" CausesValidation="false" Text="✕ Kapat">                            
                        </asp:Button>
                    </div>
                    <div class="card-body">
                        <div class="info-badge mb-3">
                            <i class="fas fa-info-circle"></i>
                            Bu başvurunun tüm hareket geçmişini görüntülemektesiniz.
                        </div>

                        <div class="table-responsive">
                            <asp:GridView ID="GridViewGecmis" runat="server" CssClass="table table-striped table-hover mb-0" 
                                          AutoGenerateColumns="False" EmptyDataText="Geçmiş kayıt bulunmamaktadır.">
                                <Columns>
                                    <asp:BoundField DataField="Sevk_Eden" HeaderText="Sevk Eden" />
                                    <asp:BoundField DataField="Teslim_Alan" HeaderText="Teslim Alan" />
                                    <asp:BoundField DataField="Tarih" HeaderText="Tarih" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" />
                                    <asp:BoundField DataField="islem_Aciklama" HeaderText="İşlem Açıklaması" />
                                </Columns>
                                <EmptyDataRowStyle CssClass="text-center text-muted p-4" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!--  Privacy Notice (Enhanced) -->
        <div class="privacy-notice">
            <i class="fas fa-shield-alt"></i>
            <strong>GİZLİLİK UYARISI:</strong> Şahısların kimlik/iletişim bilgilerinin görüntülenmesi hususunda 
            GİZLİLİK ilkesi ve Kişisel Verilerin Korunması Kanununa dikkat edilmesi gerekmektedir.
        </div>
    </div>
</asp:Content>