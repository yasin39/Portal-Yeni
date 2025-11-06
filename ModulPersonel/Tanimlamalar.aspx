<%@ Page Title="Personel Tanımlamaları" Language="C#" MasterPageFile="~/AnaV2.Master"
    AutoEventWireup="true" CodeBehind="Tanimlamalar.aspx.cs"
    Inherits="Portal.ModulPersonel.Tanimlamalar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="~/wwwroot/css/PERSONELMODUL.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">

        <!-- Sayfa Başlığı -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h4 class="mb-0">
                            <i class="fas fa-cog me-2"></i>Personel Tanımlamaları
                        </h4>
                    </div>
                    <div class="card-body">
                        <p class="text-muted mb-0">
                            <i class="fas fa-info-circle me-2"></i>
                            Kurum, sendika, ünvan ve birim tanımlamalarını bu sayfadan yönetebilirsiniz.
                       
                        </p>
                    </div>
                </div>
            </div>
        </div>

        <!-- Tab Yapısı -->
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-body">
                        <asp:HiddenField ID="hdnAktifTab" runat="server" Value="#kurum" />
                        <!-- Nav Tabs -->
                        <ul class="nav nav-tabs mb-4" id="tanimlamalarTab" role="tablist">
                            <li class="nav-item" role="presentation">
                                <button class="nav-link active" id="kurum-tab" data-bs-toggle="tab"
                                    data-bs-target="#kurum" type="button" role="tab">
                                    <i class="fas fa-building me-2"></i>Kurum
                               
                                </button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="sendika-tab" data-bs-toggle="tab"
                                    data-bs-target="#sendika" type="button" role="tab">
                                    <i class="fas fa-users me-2"></i>Sendika
                               
                                </button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="unvan-tab" data-bs-toggle="tab"
                                    data-bs-target="#unvan" type="button" role="tab">
                                    <i class="fas fa-id-badge me-2"></i>Ünvan
                               
                                </button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="birim-tab" data-bs-toggle="tab"
                                    data-bs-target="#birim" type="button" role="tab">
                                    <i class="fas fa-sitemap me-2"></i>Birim
                               
                                </button>
                            </li>
                        </ul>

                        <!-- Tab Content -->
                        <div class="tab-content" id="tanimlamalarTabContent">

                            <!-- KURUM TAB -->
                            <div class="tab-pane fade show active" id="kurum" role="tabpanel">
                                <div class="form-section">
                                    <h5 class="section-title">
                                        <i class="fas fa-building"></i>Kurum Bilgileri
                                    </h5>
                                    <div class="row">
                                        <div class="col-md-8">
                                            <div class="mb-3">
                                                <label class="form-label">Kurum Adı</label>
                                                <asp:TextBox ID="txtKurum" runat="server" CssClass="form-control"
                                                    placeholder="Kurum adını giriniz..."></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvKurum" runat="server"
                                                    ControlToValidate="txtKurum" ValidationGroup="Kurum"
                                                    ErrorMessage="Kurum adı gereklidir" CssClass="text-danger small"
                                                    Display="Dynamic" />
                                            </div>
                                        </div>
                                        <div class="col-md-4 d-flex align-items-end">
                                            <div class="mb-3">
                                                <asp:Button ID="btnKurumEkle" runat="server" CssClass="btn btn-primary me-2"
                                                    Text="💾 Ekle" OnClick="btnKurumEkle_Click" ValidationGroup="Kurum" />
                                                <asp:Button ID="btnKurumGuncelle" runat="server" CssClass="btn btn-success me-2"
                                                    Text="✏️ Güncelle" OnClick="btnKurumGuncelle_Click"
                                                    ValidationGroup="Kurum" Visible="false" />
                                                <asp:Button ID="btnKurumIptal" runat="server" CssClass="btn btn-secondary"
                                                    Text="❌ İptal" OnClick="btnKurumIptal_Click"
                                                    CausesValidation="false" Visible="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Kurum GridView -->
                                <div class="table-responsive mt-4">
                                    <asp:GridView ID="gvKurum" runat="server" CssClass="table table-hover table-bordered"
                                        AutoGenerateColumns="False" DataKeyNames="id"
                                        OnSelectedIndexChanged="gvKurum_SelectedIndexChanged"
                                        EmptyDataText="Henüz kurum kaydı bulunmamaktadır.">
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                                            <asp:BoundField DataField="Kurum_Adi" HeaderText="Kurum Adı" />
                                            <asp:TemplateField HeaderText="İşlem" ItemStyle-Width="120px" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" CommandName="Select"
                                                        CssClass="btn btn-sm btn-primary"
                                                        Text="✏️ Düzenle" CausesValidation="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="table-primary" />
                                    </asp:GridView>
                                </div>
                            </div>

                            <!-- SENDIKA TAB -->
                            <div class="tab-pane fade" id="sendika" role="tabpanel">
                                <div class="form-section">
                                    <h5 class="section-title">
                                        <i class="fas fa-users"></i>Sendika Bilgileri
                                    </h5>
                                    <div class="row">
                                        <div class="col-md-8">
                                            <div class="mb-3">
                                                <label class="form-label">Sendika Adı</label>
                                                <asp:TextBox ID="txtSendika" runat="server" CssClass="form-control"
                                                    placeholder="Sendika adını giriniz..."></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvSendika" runat="server"
                                                    ControlToValidate="txtSendika" ValidationGroup="Sendika"
                                                    ErrorMessage="Sendika adı gereklidir" CssClass="text-danger small"
                                                    Display="Dynamic" />
                                            </div>
                                        </div>
                                        <div class="col-md-4 d-flex align-items-end">
                                            <div class="mb-3">
                                                <asp:Button ID="btnSendikaEkle" runat="server" CssClass="btn btn-primary me-2"
                                                    Text="💾 Ekle" OnClick="btnSendikaEkle_Click" ValidationGroup="Sendika" />
                                                <asp:Button ID="btnSendikaGuncelle" runat="server" CssClass="btn btn-success me-2"
                                                    Text="✏️ Güncelle" OnClick="btnSendikaGuncelle_Click"
                                                    ValidationGroup="Sendika" Visible="false" />
                                                <asp:Button ID="btnSendikaIptal" runat="server" CssClass="btn btn-secondary"
                                                    Text="❌ İptal" OnClick="btnSendikaIptal_Click"
                                                    CausesValidation="false" Visible="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Sendika GridView -->
                                <div class="table-responsive mt-4">
                                    <asp:GridView ID="gvSendika" runat="server" CssClass="table table-hover table-bordered"
                                        AutoGenerateColumns="False" DataKeyNames="id"
                                        OnSelectedIndexChanged="gvSendika_SelectedIndexChanged"
                                        EmptyDataText="Henüz sendika kaydı bulunmamaktadır.">
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                                            <asp:BoundField DataField="Sendika_Adi" HeaderText="Sendika Adı" />
                                            <asp:TemplateField HeaderText="İşlem" ItemStyle-Width="120px" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" CommandName="Select"
                                                        CssClass="btn btn-sm btn-primary"
                                                        Text="✏️ Düzenle" CausesValidation="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="table-primary" />
                                    </asp:GridView>
                                </div>
                            </div>

                            <!-- ÜNVAN TAB -->
                            <div class="tab-pane fade" id="unvan" role="tabpanel">
                                <div class="form-section">
                                    <h5 class="section-title">
                                        <i class="fas fa-id-badge"></i>Ünvan Bilgileri
                                    </h5>
                                    <div class="row">
                                        <div class="col-md-8">
                                            <div class="mb-3">
                                                <label class="form-label">Ünvan</label>
                                                <asp:TextBox ID="txtUnvan" runat="server" CssClass="form-control"
                                                    placeholder="Ünvan giriniz..."></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvUnvan" runat="server"
                                                    ControlToValidate="txtUnvan" ValidationGroup="Unvan"
                                                    ErrorMessage="Ünvan gereklidir" CssClass="text-danger small"
                                                    Display="Dynamic" />
                                            </div>
                                        </div>
                                        <div class="col-md-4 d-flex align-items-end">
                                            <div class="mb-3">
                                                <asp:Button ID="btnUnvanEkle" runat="server" CssClass="btn btn-primary me-2"
                                                    Text="💾 Ekle" OnClick="btnUnvanEkle_Click" ValidationGroup="Unvan" />
                                                <asp:Button ID="btnUnvanGuncelle" runat="server" CssClass="btn btn-success me-2"
                                                    Text="✏️ Güncelle" OnClick="btnUnvanGuncelle_Click"
                                                    ValidationGroup="Unvan" Visible="false" />
                                                <asp:Button ID="btnUnvanIptal" runat="server" CssClass="btn btn-secondary"
                                                    Text="❌ İptal" OnClick="btnUnvanIptal_Click"
                                                    CausesValidation="false" Visible="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Ünvan GridView -->
                                <div class="table-responsive mt-4">
                                    <asp:GridView ID="gvUnvan" runat="server" CssClass="table table-hover table-bordered"
                                        AutoGenerateColumns="False" DataKeyNames="id"
                                        OnSelectedIndexChanged="gvUnvan_SelectedIndexChanged"
                                        EmptyDataText="Henüz ünvan kaydı bulunmamaktadır.">
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                                            <asp:BoundField DataField="Unvan" HeaderText="Ünvan" />
                                            <asp:TemplateField HeaderText="İşlem" ItemStyle-Width="120px" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" CommandName="Select"
                                                        CssClass="btn btn-sm btn-primary"
                                                        Text="✏️ Düzenle" CausesValidation="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="table-primary" />
                                    </asp:GridView>
                                </div>
                            </div>

                            <!-- BIRIM TAB -->
                            <div class="tab-pane fade" id="birim" role="tabpanel">
                                <div class="form-section">
                                    <h5 class="section-title">
                                        <i class="fas fa-sitemap"></i>Birim Bilgileri
                                    </h5>
                                    <div class="row">
                                        <div class="col-md-8">
                                            <div class="mb-3">
                                                <label class="form-label">Birim Adı</label>
                                                <asp:TextBox ID="txtBirim" runat="server" CssClass="form-control"
                                                    placeholder="Birim adını giriniz..."></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvBirim" runat="server"
                                                    ControlToValidate="txtBirim" ValidationGroup="Birim"
                                                    ErrorMessage="Birim adı gereklidir" CssClass="text-danger small"
                                                    Display="Dynamic" />
                                            </div>
                                        </div>
                                        <div class="col-md-4 d-flex align-items-end">
                                            <div class="mb-3">
                                                <asp:Button ID="btnBirimEkle" runat="server" CssClass="btn btn-primary me-2"
                                                    Text="💾 Ekle" OnClick="btnBirimEkle_Click" ValidationGroup="Birim" />
                                                <asp:Button ID="btnBirimGuncelle" runat="server" CssClass="btn btn-success me-2"
                                                    Text="✏️ Güncelle" OnClick="btnBirimGuncelle_Click"
                                                    ValidationGroup="Birim" Visible="false" />
                                                <asp:Button ID="btnBirimIptal" runat="server" CssClass="btn btn-secondary"
                                                    Text="❌ İptal" OnClick="btnBirimIptal_Click"
                                                    CausesValidation="false" Visible="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Birim GridView -->
                                <div class="table-responsive mt-4">
                                    <asp:GridView ID="gvBirim" runat="server" CssClass="table table-hover table-bordered"
                                        AutoGenerateColumns="False" DataKeyNames="Id"
                                        OnSelectedIndexChanged="gvBirim_SelectedIndexChanged"
                                        EmptyDataText="Henüz birim kaydı bulunmamaktadır.">
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                                            <asp:BoundField DataField="Sube_Adi" HeaderText="Birim Adı" />
                                            <asp:TemplateField HeaderText="İşlem" ItemStyle-Width="120px" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" CommandName="Select"
                                                        CssClass="btn btn-sm btn-primary"
                                                        Text="✏️ Düzenle" CausesValidation="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="table-primary" />
                                    </asp:GridView>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {

            // 1. Gizli alanı (hidden field) bul
            var hdnField = document.getElementById('<%= hdnAktifTab.ClientID %>');

            // 2. Sayfadaki tüm tab butonlarını seç
            var tabElms = document.querySelectorAll('button[data-bs-toggle="tab"]');

            // 3. Her bir tab butonuna 'gösterilmeden hemen önce' (show.bs.tab) olayını ekle
            tabElms.forEach(function (tabElm) {
                tabElm.addEventListener('show.bs.tab', function (event) {

                    // event.target, tıklanan <button> elementidir
                    // Butonun 'data-bs-target' özelliğini (örn: '#sendika') al
                    var targetId = event.target.getAttribute('data-bs-target');

                    // Gizli alanın değerini bu targetId ile güncelle
                    if (hdnField) {
                        hdnField.value = targetId;
                    }
                });
            });
        });
    </script>

</asp:Content>
