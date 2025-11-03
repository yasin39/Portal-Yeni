<%@ Page Title="Personel Görevlendirme" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true"
    CodeBehind="Gorevlendirme.aspx.cs" Inherits="Portal.ModulGorev.Gorevlendirme" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/BELGETAKIPMODUL.css" rel="stylesheet" />
    <style>
        /* Sadece bu sayfaya özel stiller - Ortak stiller Common-Components.css'e taşındı */

        .form-panel {
            background: linear-gradient(135deg, #e8f4ff 0%, #f0f8ff 100%);
            border-left: 4px solid #4B7BEC;
            border-radius: 8px;
            padding: 1.5rem;
            margin-top: 1rem;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">

        <!-- Başlık -->
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2 class="mb-0">
                <i class="fas fa-user-tie me-2 icon-primary"></i>Personel Görevlendirme
            </h2>
        </div>

        <!-- İstatistik Kartları -->
        <div class="row mb-4">
            <div class="col-md-4">
                <div class="stat-card total">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h3 class="stat-number">
                                <asp:Label ID="lblToplamGorev" runat="server" Text="0"></asp:Label>
                            </h3>
                            <p class="stat-label mb-0">Toplam Görevlendirme</p>
                        </div>
                        <div>
                            <i class="fas fa-clipboard-list icon-xl icon-primary icon-faded"></i>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="stat-card active">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h3 class="stat-number text-success">
                                <asp:Label ID="lblAktifGorev" runat="server" Text="0"></asp:Label>
                            </h3>
                            <p class="stat-label mb-0">Aktif Görevler</p>
                        </div>
                        <div>
                            <i class="fas fa-users icon-xl icon-success icon-faded"></i>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="stat-card completed">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h3 class="stat-number text-muted">
                                <asp:Label ID="lblTamamlananGorev" runat="server" Text="0"></asp:Label>
                            </h3>
                            <p class="stat-label mb-0">Tamamlanan Görevler</p>
                        </div>
                        <div>
                            <i class="fas fa-check-circle icon-xl icon-secondary icon-faded"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Filtre Bölümü -->
        <div class="card mb-4">
            <div class="card-header">
                <h5 class="card-title mb-0">
                    <i class="fas fa-filter me-2"></i>Filtreleme ve Arama
                </h5>
            </div>
            <div class="card-body">
                <div class="row g-3">
                    <div class="col-md-3">
                        <label class="form-label">Personel</label>
                        <asp:DropDownList ID="ddlFiltrePersonel" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">Tümü</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">İl</label>
                        <asp:DropDownList ID="ddlFiltreIl" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">Tümü</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="form-label">Başlangıç Tarihi</label>
                        <asp:TextBox ID="txtFiltreBaslangic" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="form-label">Bitiş Tarihi</label>
                        <asp:TextBox ID="txtFiltreBitis" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="col-md-2 d-flex align-items-end">
                        <div class="btn-action-group w-100">
                            <asp:Button ID="btnFiltrele" runat="server" Text="🔍 Ara" CssClass="btn btn-primary flex-fill"
                                OnClick="btnFiltrele_Click" CausesValidation="false" />
                            <asp:Button ID="btnTemizle" runat="server" Text="🔄 Temizle" CssClass="btn btn-outline-secondary flex-fill"
                                OnClick="btnTemizle_Click" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Görevlendirme Listesi -->
        <div class="card mb-4">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h5 class="card-title mb-0">
                    <i class="fas fa-list me-2"></i>Görevlendirme Listesi
                    <asp:Label ID="lblKayitSayisi" runat="server" CssClass="badge bg-primary ms-2"></asp:Label>
                </h5>
                <asp:Button ID="btnExcelAktar" runat="server" Text="📊 Excel'e Aktar"
                    CssClass="btn btn-success btn-sm" OnClick="btnExcelAktar_Click" CausesValidation="false" />
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="GorevlendirmeGrid" runat="server" CssClass="table table-striped table-hover"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="15"
                        OnPageIndexChanging="GorevlendirmeGrid_PageIndexChanging"
                        OnSelectedIndexChanged="GorevlendirmeGrid_SelectedIndexChanged"
                        EmptyDataText="Kayıt bulunamadı.">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" SelectText="Seç" ButtonType="Button"
                                ControlStyle-CssClass="btn btn-sm btn-primary">
                                <ControlStyle CssClass="btn btn-sm btn-primary"></ControlStyle>
                            </asp:CommandField>
                            <asp:BoundField DataField="id" HeaderText="ID" />
                            <asp:BoundField DataField="AdiSoyadi" HeaderText="Personel" />
                            <asp:BoundField DataField="BaslamaTarihi" HeaderText="Başlama Tarihi" DataFormatString="{0:dd.MM.yyyy}" />
                            <asp:BoundField DataField="GorevlendirmeSuresi" HeaderText="Süre (Gün)" />
                            <asp:BoundField DataField="BitisTarihi" HeaderText="Bitiş Tarihi" DataFormatString="{0:dd.MM.yyyy}" />
                            <asp:BoundField DataField="il" HeaderText="İl" />
                            <asp:BoundField DataField="Digeriller" HeaderText="Diğer İller" />
                            <asp:BoundField DataField="GorevTanimi" HeaderText="Görev Tanımı" />
                            <asp:BoundField DataField="KayitTarihi" HeaderText="Kayıt Tarihi" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                            <asp:BoundField DataField="KayitKullanici" HeaderText="Kaydeden" />
                        </Columns>
                        <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
                        <EmptyDataRowStyle CssClass="text-center text-muted p-4" />
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Form Paneli -->
        <asp:Panel ID="pnlForm" runat="server" Visible="true">
            <div class="form-panel">
                <h5 class="mb-3">
                    <i class="fas fa-edit me-2"></i>
                    <asp:Label ID="lblFormBaslik" runat="server" Text="Yeni Görevlendirme Ekle"></asp:Label>
                </h5>

                <div class="row g-3">
                    <div class="col-md-4">
                        <label class="form-label">Personel <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlPersonel" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">Seçiniz</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvPersonel" runat="server" ControlToValidate="ddlPersonel"
                            ErrorMessage="Personel seçimi zorunludur" ForeColor="Red" Display="Dynamic" InitialValue="" />
                    </div>

                    <div class="col-md-4">
                        <label class="form-label">İl <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlIl" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">Seçiniz</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvIl" runat="server" ControlToValidate="ddlIl"
                            ErrorMessage="İl seçimi zorunludur" ForeColor="Red" Display="Dynamic" InitialValue="" />
                    </div>

                    <div class="col-md-4">
                        <label class="form-label">Diğer İller</label>
                        <asp:TextBox ID="txtDigerIller" runat="server" CssClass="form-control"
                            placeholder="Varsa diğer illeri yazınız"></asp:TextBox>
                    </div>

                    <div class="col-md-4">
                        <label class="form-label">Başlama Tarihi <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtBaslamaTarihi" runat="server" CssClass="form-control"
                            TextMode="Date"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvBaslamaTarihi" runat="server" ControlToValidate="txtBaslamaTarihi"
                            ErrorMessage="Başlama tarihi zorunludur" ForeColor="Red" Display="Dynamic" />
                    </div>

                    <div class="col-md-4">
                        <label class="form-label">Görevlendirme Süresi (Gün) <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtSure" runat="server" CssClass="form-control"
                            TextMode="Number" placeholder="Örn: 5"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSure" runat="server" ControlToValidate="txtSure"
                            ErrorMessage="Süre zorunludur" ForeColor="Red" Display="Dynamic" />
                        <asp:RangeValidator ID="rvSure" runat="server" ControlToValidate="txtSure"
                            MinimumValue="1" MaximumValue="365" Type="Integer"
                            ErrorMessage="Süre 1-365 gün arasında olmalıdır" ForeColor="Red" Display="Dynamic" />
                    </div>

                    <div class="col-md-4">
                        <label class="form-label">Bitiş Tarihi <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtBitisTarihi" runat="server" CssClass="form-control"
                            TextMode="Date"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvBitisTarihi" runat="server" ControlToValidate="txtBitisTarihi"
                            ErrorMessage="Bitiş tarihi zorunludur" ForeColor="Red" Display="Dynamic" />
                    </div>

                    <div class="col-md-12">
                        <label class="form-label">Görev Tanımı <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtGorevTanimi" runat="server" CssClass="form-control"
                            TextMode="MultiLine" Rows="3" placeholder="Görev detaylarını yazınız"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvGorevTanimi" runat="server" ControlToValidate="txtGorevTanimi"
                            ErrorMessage="Görev tanımı zorunludur" ForeColor="Red" Display="Dynamic" />
                    </div>

                    <div class="col-md-12">
                        <asp:HiddenField ID="hfKayitID" runat="server" Value="0" />
                        <div class="btn-action-group">
                            <asp:Button ID="btnKaydet" runat="server" Text="💾 Kaydet" CssClass="btn btn-success"
                                OnClick="btnKaydet_Click" />
                            <asp:Button ID="btnGuncelle" runat="server" Text="✏️ Güncelle" CssClass="btn btn-warning"
                                OnClick="btnGuncelle_Click" Visible="false" />
                            <asp:Button ID="btnSil" runat="server" Text="🗑️ Sil" CssClass="btn btn-danger"
                                OnClick="btnSil_Click" Visible="false"
                                OnClientClick="return confirm('Bu kaydı silmek istediğinizden emin misiniz?');" />
                            <asp:Button ID="btnVazgec" runat="server" Text="❌ Vazgeç" CssClass="btn btn-secondary"
                                OnClick="btnVazgec_Click" Visible="false" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

    </div>
</asp:Content>