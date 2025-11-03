<%@ Page Title="Bilgisayar Adları Yönetimi" Language="C#" MasterPageFile="~/AnaV2.Master" 
    AutoEventWireup="true" CodeBehind="BilgisayarAdlari.aspx.cs" 
    Inherits="ModulYonetici.BilgisayarAdlari" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-section {
            background: white;
            border-radius: 10px;
            padding: 25px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            margin-bottom: 20px;
        }
        .section-title {
            color: #2E5B9A;
            font-weight: 600;
            margin-bottom: 20px;
            padding-bottom: 10px;
            border-bottom: 2px solid #4B7BEC;
        }
        .required-field::after {
            content: " *";
            color: #dc3545;
        }
        .grid-container {
            max-height: 500px;
            overflow-y: auto;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item"><a href="/Anasayfa.aspx">Ana Sayfa</a></li>
    <li class="breadcrumb-item">Yönetici</li>
    <li class="breadcrumb-item active">Bilgisayar Adları</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-12">
            <div class="card shadow-custom-md">
                <div class="card-header">
                    <h5 class="mb-0">
                        <i class="fas fa-desktop me-2"></i>Bilgisayar Adları Yönetimi
                    </h5>
                </div>
                <div class="card-body">
                    
                    <!-- Arama Bölümü -->
                    <div class="form-section">
                        <h6 class="section-title">
                            <i class="fas fa-search me-2"></i>Kayıt Arama
                        </h6>
                        <div class="row align-items-end">
                            <div class="col-md-3">
                                <label class="form-label">Domain No</label>
                                <asp:TextBox ID="txtAramaDomain" runat="server" CssClass="form-control text-uppercase" 
                                    placeholder="Örn: ANKB001">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label class="form-label">Kişi Adı</label>
                                <asp:TextBox ID="txtAramaKisi" runat="server" CssClass="form-control" 
                                    placeholder="Kişi adı ile ara...">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-5">
                                <asp:Button ID="btnAra" runat="server" Text="🔍 Ara" 
                                    CssClass="btn btn-primary" OnClick="btnAra_Click" 
                                    CausesValidation="false" />
                                <asp:Button ID="btnTumunuListele" runat="server" Text="📜 Tümünü Listele" 
                                    CssClass="btn btn-secondary ms-2" OnClick="btnTumunuListele_Click" 
                                    CausesValidation="false" />
                            </div>
                        </div>
                    </div>

                    <!-- Form Bölümü -->
                    <div class="form-section">
                        <h6 class="section-title">
                            <i class="fas fa-keyboard me-2"></i>Kayıt Bilgileri
                        </h6>
                        
                        <div class="row mb-3">
                            <div class="col-md-3">
                                <label class="form-label required-field">Domain No</label>
                                <asp:TextBox ID="txtDomainNo" runat="server" CssClass="form-control text-uppercase" 
                                    placeholder="ANKB001" MaxLength="50">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDomain" runat="server" 
                                    ControlToValidate="txtDomainNo" ErrorMessage="Domain No zorunludur." 
                                    CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-5">
                                <label class="form-label">Kişi Adı</label>
                                <asp:TextBox ID="txtKisiAdi" runat="server" CssClass="form-control" 
                                    placeholder="Ad Soyad" MaxLength="150">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label class="form-label">Bilgisayar Tipi</label>
                                <asp:DropDownList ID="ddlBilgisayarTipi" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-3">
                                <label class="form-label">Dahili Telefon</label>
                                <asp:TextBox ID="txtDahiliNo" runat="server" CssClass="form-control" 
                                    placeholder="1234" MaxLength="20">
                                </asp:TextBox>
                            </div>
                        </div>

                        <!-- İşlem Butonları -->
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnKaydet" runat="server" Text="💾 Kaydet" 
                                    CssClass="btn btn-primary btn-lg" OnClick="btnKaydet_Click" 
                                    ValidationGroup="kayit" />
                                
                                <asp:Button ID="btnGuncelle" runat="server" Text="✏️ Güncelle" 
                                    CssClass="btn btn-warning btn-lg" OnClick="btnGuncelle_Click" 
                                    ValidationGroup="kayit" Visible="false" />
                                
                                <asp:Button ID="btnSil" runat="server" Text="🗑️ Sil" 
                                    CssClass="btn btn-danger btn-lg" OnClick="btnSil_Click" 
                                    CausesValidation="false" Visible="false" 
                                    OnClientClick="return confirm('Bu kaydı silmek istediğinizden emin misiniz?');" />
                                
                                <asp:Button ID="btnVazgec" runat="server" Text="❌ Vazgeç" 
                                    CssClass="btn btn-secondary btn-lg" OnClick="btnVazgec_Click" 
                                    CausesValidation="false" Visible="false" />
                            </div>
                        </div>

                    </div>

                    <!-- GridView Bölümü -->
                    <div class="form-section">
                        <h6 class="section-title">
                            <i class="fas fa-list me-2"></i>Kayıt Listesi
                            <span class="badge bg-primary ms-2">
                                <asp:Literal ID="litToplamKayit" runat="server"></asp:Literal>
                            </span>
                        </h6>
                        
                        <div class="row mb-3">
                            <div class="col-md-12">
                                <asp:Button ID="btnExcelAktar" runat="server" Text="📥 Excel'e Aktar" 
                                    CssClass="btn btn-success" OnClick="btnExcelAktar_Click" 
                                    CausesValidation="false" />
                                
                                <asp:Button ID="btnExcelYukle" runat="server" Text="📤 Excel'den Yükle" 
                                    CssClass="btn btn-info ms-2" OnClick="btnExcelYukle_Click" 
                                    CausesValidation="false" />
                                
                                <asp:FileUpload ID="fuExcel" runat="server" CssClass="d-none" 
                                    accept=".xlsx,.xls" />
                            </div>
                        </div>

                        <div class="grid-container">
                            <asp:GridView ID="BilgisayarlarGrid" runat="server" 
                                CssClass="table table-striped table-hover modern-table" 
                                AutoGenerateColumns="false" 
                                OnRowCommand="BilgisayarlarGrid_RowCommand"
                                DataKeyNames="id"
                                EmptyDataText="Kayıt bulunamadı.">
                                <Columns>
                                    <asp:BoundField DataField="domain_no" HeaderText="Domain No" />
                                    <asp:BoundField DataField="kisi_adi" HeaderText="Kişi Adı" />
                                    <asp:BoundField DataField="bilgisayar_tipi" HeaderText="Bilgisayar Tipi" />
                                    <asp:BoundField DataField="dahili_no" HeaderText="Dahili No" />
                                    <asp:BoundField DataField="kayit_tarihi" HeaderText="Kayıt Tarihi" 
                                        DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                                    <asp:BoundField DataField="kayit_kullanici" HeaderText="Kaydeden" />
                                    <asp:TemplateField HeaderText="İşlem">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnSec" runat="server" 
                                                CommandName="Sec" CommandArgument='<%# Eval("id") %>'
                                                CssClass="btn btn-sm btn-primary" CausesValidation="false">
                                                <i class="fas fa-edit"></i> Seç
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <!-- Excel Yükleme Modal -->
    <script type="text/javascript">
        function TriggerFileUpload() {
            document.getElementById('<%= fuExcel.ClientID %>').click();
        }
    </script>
</asp:Content>