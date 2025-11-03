<%@ Page Title="Duyuru Yönetimi" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" 
    CodeBehind="Duyurular.aspx.cs" Inherits="Portal.ModulYonetici.Duyurular" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Common-Components.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid py-4">
        
        <!-- Sayfa Başlığı -->
        <div class="page-header">
            <h1 class="page-title">
                <i class="fas fa-bullhorn"></i>
                Duyuru Yönetimi
            </h1>
            <p class="page-subtitle">Sistem duyurularını ekleyebilir, düzenleyebilir ve yönetebilirsiniz</p>
        </div>

        <!-- Duyuru Listesi -->
        <div class="card-modern">
            <div class="card-modern-header">
                <h5 class="card-modern-title">
                    <i class="fas fa-list"></i> Duyuru Listesi
                </h5>
            </div>
            <div class="card-modern-body">
                <div class="table-responsive">
                    <asp:GridView ID="DuyurularGrid" runat="server" CssClass="modern-table" 
                        AutoGenerateColumns="False" 
                        DataKeyNames="id"
                        OnSelectedIndexChanged="DuyurularGrid_SelectedIndexChanged"
                        HeaderStyle-CssClass="grid-header-modern"
                        EmptyDataText="Henüz kayıtlı duyuru bulunmamaktadır.">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="ID" ItemStyle-CssClass="text-center" />
                            <asp:BoundField DataField="Baslama_Tarihi" HeaderText="Başlama" DataFormatString="{0:dd.MM.yyyy}" 
                                ItemStyle-CssClass="text-center" />
                            <asp:BoundField DataField="Bitis_Tarihi" HeaderText="Bitiş" DataFormatString="{0:dd.MM.yyyy}" 
                                ItemStyle-CssClass="text-center" />
                            <asp:BoundField DataField="Durum" HeaderText="Durum" ItemStyle-CssClass="text-center" />
                            <asp:BoundField DataField="Duyuru" HeaderText="Duyuru Metni" />
                            <asp:BoundField DataField="Kullanici" HeaderText="Oluşturan" />
                            <asp:BoundField DataField="Kayit_Tarihi" HeaderText="Kayıt Tarihi" DataFormatString="{0:dd.MM.yyyy HH:mm}" 
                                ItemStyle-CssClass="text-center" />
                            <asp:CommandField ShowSelectButton="True" SelectText="Seç" 
                                HeaderText="İşlem" ButtonType="Button" 
                                ControlStyle-CssClass="btn btn-sm btn-outline-primary" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Duyuru Form -->
        <div class="card-modern mt-4">
            <div class="card-modern-header">
                <h5 class="card-modern-title">
                    <i class="fas fa-edit"></i> Duyuru Formu
                </h5>
            </div>
            <div class="card-modern-body">
                <div class="row g-3">
                    <!-- Başlama Tarihi -->
                    <div class="col-md-6">
                        <label class="form-label-enhanced">
                            <i class="fas fa-calendar-alt icon-primary"></i> Başlama Tarihi
                            <span class="required-asterisk">*</span>
                        </label>
                        <asp:TextBox ID="txtBaslamaTarihi" runat="server" CssClass="form-control" 
                            TextMode="Date"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvBaslama" runat="server" 
                            ControlToValidate="txtBaslamaTarihi" 
                            ErrorMessage="Başlama tarihi zorunludur" 
                            CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <!-- Bitiş Tarihi -->
                    <div class="col-md-6">
                        <label class="form-label-enhanced">
                            <i class="fas fa-calendar-check icon-primary"></i> Bitiş Tarihi
                            <span class="required-asterisk">*</span>
                        </label>
                        <asp:TextBox ID="txtBitisTarihi" runat="server" CssClass="form-control" 
                            TextMode="Date"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvBitis" runat="server" 
                            ControlToValidate="txtBitisTarihi" 
                            ErrorMessage="Bitiş tarihi zorunludur" 
                            CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <!-- Durum -->
                    <div class="col-md-6">
                        <label class="form-label-enhanced">
                            <i class="fas fa-toggle-on icon-primary"></i> Durum
                            <span class="required-asterisk">*</span>
                        </label>
                        <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Aktif" Text="Aktif"></asp:ListItem>
                            <asp:ListItem Value="Pasif" Text="Pasif"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <!-- Dosya -->
                    <div class="col-md-6">
                        <label class="form-label-enhanced">
                            <i class="fas fa-paperclip icon-primary"></i> Dosya (Opsiyonel)
                        </label>
                        <asp:FileUpload ID="fuDosya" runat="server" CssClass="form-control" />
                        <asp:HiddenField ID="hfMevcutDosya" runat="server" />
                        <small class="text-muted d-block mt-1">Mevcut dosya varsa ve yeni dosya seçilmezse korunur</small>
                    </div>

                    <!-- Duyuru Metni -->
                    <div class="col-12">
                        <label class="form-label-enhanced">
                            <i class="fas fa-comment-alt icon-primary"></i> Duyuru Metni
                            <span class="required-asterisk">*</span>
                        </label>
                        <asp:TextBox ID="txtDuyuru" runat="server" CssClass="form-control" 
                            TextMode="MultiLine" Rows="5" MaxLength="4000"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDuyuru" runat="server" 
                            ControlToValidate="txtDuyuru" 
                            ErrorMessage="Duyuru metni zorunludur" 
                            CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <!-- Butonlar -->
                    <div class="col-12">
                        <div class="action-buttons-group mt-3">
                            <asp:Button ID="btnKaydet" runat="server" Text="💾 Kaydet" 
                                CssClass="btn btn-success" OnClick="btnKaydet_Click" />
                            <asp:Button ID="btnGuncelle" runat="server" Text="✏️ Güncelle" 
                                CssClass="btn btn-primary" OnClick="btnGuncelle_Click" Visible="False" />
                            <asp:Button ID="btnSil" runat="server" Text="🗑️ Sil" 
                                CssClass="btn btn-danger" OnClick="btnSil_Click" Visible="False"
                                OnClientClick="return confirm('Bu duyuruyu silmek istediğinizden emin misiniz?');" />
                            <asp:Button ID="btnVazgec" runat="server" Text="↩️ Vazgeç" 
                                CssClass="btn btn-secondary" OnClick="btnVazgec_Click" 
                                CausesValidation="False" Visible="False" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
