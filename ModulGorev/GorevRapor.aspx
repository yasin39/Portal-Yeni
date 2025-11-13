<%@ Page Title="Araç Görev Rapor" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true"
    CodeBehind="GorevRapor.aspx.cs" Inherits="Portal.ModulGorev.GorevRapor" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/BELGETAKIPMODUL.css" rel="stylesheet" />
    <style>
        /* Sadece bu sayfaya özel stiller - Ortak stiller Common-Components.css'e taşındı */

        .stat-card.km {
            border-left-color: #10b981;
        }

        .stat-card.fuel {
            border-left-color: #F59E0B;
        }

        .update-panel {
            background: linear-gradient(135deg, #fff9e6 0%, #fff3cd 100%);
            border-left: 4px solid #ffc107;
            border-radius: 8px;
            padding: 1.5rem;
            margin-top: 1.5rem;
            display: none;
        }

            .update-panel.show {
                display: block;
                animation: slideDown 0.3s ease-out;
            }

        @keyframes slideDown {
            from {
                opacity: 0;
                transform: translateY(-10px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .modern-table tbody tr.selected {
            background: #e3f2fd !important;
            border-left: 3px solid #4B7BEC;
        }

        .btn-group-custom {
            gap: 0.5rem;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">

        <!-- Başlık -->
        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h2 class="mb-1">🚗 Araç Görev Rapor</h2>
                <p class="text-muted mb-0">Araç görevlerini görüntüleyin ve yönetin</p>
            </div>
        </div>

        <!-- İstatistik Kartları -->
        <div class="row g-3 mb-4">
            <div class="col-md-4">
                <div class="stat-card total position-relative">
                    <i class="fas fa-tasks stat-icon"></i>
                    <asp:Label ID="lblToplamGorev" runat="server" CssClass="stat-number" Text="0"></asp:Label>
                    <p class="stat-label mb-0">Toplam Görev</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="stat-card km position-relative">
                    <i class="fas fa-route stat-icon"></i>
                    <asp:Label ID="lblToplamKm" runat="server" CssClass="stat-number" Text="0"></asp:Label>
                    <p class="stat-label mb-0">Toplam KM</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="stat-card fuel position-relative">
                    <i class="fas fa-gas-pump stat-icon"></i>
                    <asp:Label ID="lblToplamYakit" runat="server" CssClass="stat-number" Text="0"></asp:Label>
                    <p class="stat-label mb-0">Toplam Yakıt (Litre)</p>
                </div>
            </div>
        </div>

        <!-- Filtre Bölümü -->
        <div class="card mb-4">
            <div class="card-header">
                <h5 class="card-title mb-0">
                    <i class="fas fa-filter me-2"></i>Filtreleme Seçenekleri
                </h5>
            </div>
            <div class="card-body">
                <div class="row g-3">
                    <div class="col-md-3">
                        <label class="form-label">Araç Plakası</label>
                        <asp:DropDownList ID="ddlPlaka" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Görevlendiren Birim</label>
                        <asp:DropDownList ID="ddlSube" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="form-label">Başlangıç Tarihi</label>
                        <asp:TextBox ID="txtBaslangicTarihi" runat="server" CssClass="form-control fp-date"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="form-label">Bitiş Tarihi</label>
                        <asp:TextBox ID="txtBitisTarihi" runat="server" CssClass="form-control fp-date"></asp:TextBox>
                    </div>
                    <div class="col-md-2 d-flex align-items-end">
                        <div class="btn-group-custom d-flex w-100">
                            <asp:Button ID="btnAra" runat="server" Text="🔍 Ara" CssClass="btn btn-primary flex-fill"
                                OnClick="btnAra_Click" />
                            <asp:Button ID="btnTumunuListele" runat="server" Text="📜 Tümü"
                                CssClass="btn btn-outline-secondary" OnClick="btnTumunuListele_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Görev Listesi -->
        <div class="card mb-4">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h5 class="card-title mb-0">
                    <i class="fas fa-list me-2"></i>Görev Listesi
                    <asp:Label ID="lblKayitSayisi" runat="server" CssClass="badge bg-primary ms-2"></asp:Label>
                </h5>
                <asp:Button ID="btnExcelAktar" runat="server" Text="📊 Excel'e Aktar"
                    CssClass="btn btn-success btn-sm" OnClick="btnExcelAktar_Click" />
            </div>
            <div class="card-body p-0">
                <div class="table-responsive">
                    <asp:GridView ID="GorevlerGrid" runat="server" CssClass="table modern-table mb-0"
                        AutoGenerateColumns="False" DataKeyNames="Gorev_Id"
                        OnSelectedIndexChanged="GorevlerGrid_SelectedIndexChanged"
                        EmptyDataText="Görev kaydı bulunamadı.">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" SelectText="Seç" ButtonType="Button"
                                HeaderText="İşlem">
                                <ControlStyle CssClass="btn btn-sm btn-outline-primary" />
                            </asp:CommandField>
                            <asp:BoundField DataField="Gorev_Id" HeaderText="ID" Visible="False" />
                            <asp:BoundField DataField="Gorevli_Arac" HeaderText="Plaka" />
                            <asp:BoundField DataField="Adi_Soyadi" HeaderText="Sürücü" />
                            <asp:BoundField DataField="Goreve_Cikis_Tarihi" HeaderText="Çıkış Tarihi" />
                            <asp:BoundField DataField="Gorevden_Donus_Tarihi" HeaderText="Dönüş Tarihi" />
                            <asp:BoundField DataField="Cikis_Km" HeaderText="Çıkış KM" />
                            <asp:BoundField DataField="Donus_Km" HeaderText="Dönüş KM" />
                            <asp:BoundField DataField="Yapilan_Km" HeaderText="Yapılan KM" />
                            <asp:BoundField DataField="Ort_Yak_Tuk" HeaderText="Ort. Tüketim" />
                            <asp:BoundField DataField="Tuketilen_Yakit" HeaderText="Tüketilen Yakıt" />
                            <asp:BoundField DataField="Gorevlendiren_Birim" HeaderText="Birim" />
                            <asp:BoundField DataField="Goreve_Gidilen_Yer" HeaderText="Gidilen Yer" />
                            <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Güncelleme Paneli -->
        <asp:Panel ID="pnlGuncelleme" runat="server" CssClass="update-panel">
            <h5 class="mb-3">
                <i class="fas fa-edit me-2"></i>Görev Kaydı Düzenleme
            </h5>
            <div class="row g-3">
                <div class="col-md-3">
                    <label class="form-label">Görev ID</label>
                    <asp:TextBox ID="txtGorevId" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Araç Plakası</label>
                    <asp:DropDownList ID="ddlPlakaGuncelle" runat="server" CssClass="form-select"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlPlakaGuncelle_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Sürücü Adı Soyadı</label>
                    <asp:TextBox ID="txtAdiSoyadi" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Ort. Yakıt Tüketimi</label>
                    <asp:TextBox ID="txtOrtYakitTuk" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Çıkış Tarihi</label>
                    <asp:TextBox ID="txtCikisTarihi" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Dönüş Tarihi</label>
                    <asp:TextBox ID="txtDonusTarihi" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <label class="form-label">Çıkış KM</label>
                    <asp:TextBox ID="txtCikisKm" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <label class="form-label">Dönüş KM</label>
                    <asp:TextBox ID="txtDonusKm" runat="server" CssClass="form-control" TextMode="Number"
                        AutoPostBack="true" OnTextChanged="txtDonusKm_TextChanged"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <label class="form-label">Yapılan KM</label>
                    <asp:TextBox ID="txtYapilanKm" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Tüketilen Yakıt</label>
                    <asp:TextBox ID="txtTuketilenYakit" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Görevlendiren Birim</label>
                    <asp:DropDownList ID="ddlSubeGuncelle" runat="server" CssClass="form-select">
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Gidilen Yer</label>
                    <asp:TextBox ID="txtGidilenYer" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-12">
                    <label class="form-label">Açıklama</label>
                    <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                </div>
                <div class="col-md-12">
                    <div class="d-flex gap-2 justify-content-end">
                        <asp:Button ID="btnGuncelle" runat="server" Text="💾 Güncelle" CssClass="btn btn-primary"
                            OnClick="btnGuncelle_Click" />
                        <asp:Button ID="btnSil" runat="server" Text="🗑️ Sil" CssClass="btn btn-danger"
                            OnClick="btnSil_Click" OnClientClick="return confirm('Bu kaydı silmek istediğinizden emin misiniz?');" />
                        <asp:Button ID="btnVazgec" runat="server" Text="❌ Vazgeç" CssClass="btn btn-secondary"
                            OnClick="btnVazgec_Click" />
                    </div>
                </div>
            </div>
        </asp:Panel>

    </div>
</asp:Content>
