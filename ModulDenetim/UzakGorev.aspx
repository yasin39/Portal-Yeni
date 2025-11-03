<%@ Page Title="Uzak Görev Takibi" Language="C#" MasterPageFile="~/AnaV2.Master" 
    AutoEventWireup="true" CodeBehind="UzakGorev.aspx.cs" 
    Inherits="Portal.ModulDenetim.UzakGorev" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="~/wwwroot/css/DENETIMMODUL.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid py-4">
        
        <!-- Başlık ve Aksiyon Butonları -->
        <div class="row mb-3">
            <div class="col-md-12">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <h4 class="mb-1">
                            <i class="fas fa-clipboard-check me-2 text-primary-custom"></i>
                            Uzak Görev Takibi
                        </h4>
                        <p class="text-muted mb-0">
                            <small>Atanan uzak denetim görevlerini takip edin ve raporlayın</small>
                        </p>
                    </div>
                    <div>
                        <asp:Label ID="lblKayitSayisi" runat="server" CssClass="badge bg-primary" Text="0 kayıt"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <!-- Aksiyon Butonları -->
        <div class="row mb-3">
            <div class="col-md-12">
                <div class="denetim-actions">
                    <asp:Button ID="btnAcikGorevler" runat="server" CssClass="btn btn-denetim-filter" 
                        Text="📋 Açık Görevler" OnClick="btnAcikGorevler_Click" />
                    
                    <asp:Button ID="btnTumGorevler" runat="server" CssClass="btn btn-outline-secondary" 
                        Text="📜 Tüm Görevler" OnClick="btnTumGorevler_Click" CausesValidation="false" />
                    
                    <asp:Button ID="btnExcelAktar" runat="server" CssClass="btn btn-denetim-excel" 
                        Text="📊 Excel'e Aktar" OnClick="btnExcelAktar_Click" CausesValidation="false" />
                </div>
            </div>
        </div>

        <!-- Grid Paneli -->
        <div class="row">
            <div class="col-md-12">
                <div class="denetim-result-card">
                    <div class="denetim-result-header">
                        <h6 class="denetim-result-title mb-0">
                            <i class="fas fa-list me-2"></i>Görev Listesi
                        </h6>
                    </div>
                    <div class="card-body p-0">
                        <div class="table-responsive">
                            <asp:GridView ID="UzakGorevGrid" runat="server" 
                                CssClass="table table-hover denetim-grid mb-0"
                                AutoGenerateColumns="False"
                                DataKeyNames="id"
                                OnSelectedIndexChanged="UzakGorevGrid_SelectedIndexChanged"
                                EmptyDataText="Kayıt bulunamadı."
                                GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                                    
                                    <asp:BoundField DataField="Tarih" HeaderText="Tarih" 
                                        DataFormatString="{0:dd.MM.yyyy}" HtmlEncode="false" />
                                    
                                    <asp:BoundField DataField="AracSayisi" HeaderText="Toplam Araç" />
                                    
                                    <asp:BoundField DataField="UygunsuzAracSayisi" HeaderText="Uygunsuz Araç" />
                                    
                                    <asp:BoundField DataField="YBOlmayanAracSayisi" HeaderText="YB Olmayan" />
                                    
                                    <asp:BoundField DataField="YBKayitliOlmayanAracSayisi" HeaderText="YB Kayıtsız" />
                                    
                                    <asp:BoundField DataField="AtananPersonel" HeaderText="Atanan Personel" />
                                    
                                    <asp:TemplateField HeaderText="Durum">
                                        <ItemTemplate>
                                            <span class='<%# Eval("Durum").ToString() == "Açık" ? "durum-badge durum-acik" : "durum-badge durum-kapali" %>'>
                                                <%# Eval("Durum") %>
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:CommandField ShowSelectButton="True" SelectText="Seç" 
                                        HeaderText="İşlem" ButtonType="Button" 
                                        ControlStyle-CssClass="btn btn-sm btn-primary" />
                                </Columns>
                                <HeaderStyle CssClass="table-header" />
                                <RowStyle CssClass="table-row" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Detay Paneli (Seçim sonrası görünür) -->
        <asp:Panel ID="PanelDetay" runat="server" Visible="false" CssClass="mt-4">
            <div class="row">
                <div class="col-md-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0">
                                <i class="fas fa-edit me-2"></i>Görev Detayı ve Raporlama
                            </h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-3">
                                <div class="col-md-4">
                                    <label class="form-label">Tarih</label>
                                    <asp:TextBox ID="txtTarih" runat="server" CssClass="form-control" 
                                        ReadOnly="true" />
                                </div>
                                
                                <div class="col-md-4">
                                    <label class="form-label">Toplam Araç Sayısı</label>
                                    <asp:TextBox ID="txtAracSayisi" runat="server" CssClass="form-control" 
                                        ReadOnly="true" />
                                </div>

                                <div class="col-md-4">
                                    <label class="form-label">Uygunsuz Araç Sayısı <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtUygunsuzArac" runat="server" CssClass="form-control" 
                                        TextMode="Number" />
                                    <asp:RequiredFieldValidator ID="rfvUygunsuzArac" runat="server" 
                                        ControlToValidate="txtUygunsuzArac" 
                                        ErrorMessage="Zorunlu alan" 
                                        CssClass="text-danger" 
                                        Display="Dynamic" />
                                </div>

                                <div class="col-md-4">
                                    <label class="form-label">YB Olmayan Araç Sayısı <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtYBOlmayanArac" runat="server" CssClass="form-control" 
                                        TextMode="Number" />
                                    <asp:RequiredFieldValidator ID="rfvYBOlmayanArac" runat="server" 
                                        ControlToValidate="txtYBOlmayanArac" 
                                        ErrorMessage="Zorunlu alan" 
                                        CssClass="text-danger" 
                                        Display="Dynamic" />
                                </div>

                                <div class="col-md-4">
                                    <label class="form-label">YB Kayıtlı Olmayan Araç Sayısı <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtYBKayitliOlmayan" runat="server" CssClass="form-control" 
                                        TextMode="Number" />
                                    <asp:RequiredFieldValidator ID="rfvYBKayitliOlmayan" runat="server" 
                                        ControlToValidate="txtYBKayitliOlmayan" 
                                        ErrorMessage="Zorunlu alan" 
                                        CssClass="text-danger" 
                                        Display="Dynamic" />
                                </div>

                                <div class="col-md-12">
                                    <label class="form-label">Açıklama</label>
                                    <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" 
                                        TextMode="MultiLine" Rows="4" />
                                </div>

                                <div class="col-md-12">
                                    <div class="d-flex gap-2 justify-content-end mt-3">
                                        <asp:Button ID="btnKaydet" runat="server" CssClass="btn btn-success" 
                                            Text="💾 Kaydet ve Kapat" OnClick="btnKaydet_Click" />
                                        
                                        <asp:Button ID="btnVazgec" runat="server" CssClass="btn btn-outline-secondary" 
                                            Text="❌ Vazgeç" OnClick="btnVazgec_Click" CausesValidation="false" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

    </div>
</asp:Content>