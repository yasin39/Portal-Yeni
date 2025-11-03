<%@ Page Title="Personel Listesi" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true"
    CodeBehind="Liste.aspx.cs" Inherits="Portal.ModulPersonel.Liste" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/BELGETAKIPMODUL.css" rel="stylesheet" />
    <link href="/Content/PERSONELMODUL.css" rel="stylesheet" />
    <style>
        /* Sadece bu sayfaya özel stiller - Geri kalan tüm stiller Common-Components.css'e taşındı */

        .filter-card {
            background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
            border-radius: 10px;
            padding: 1.5rem;
            margin-bottom: 1.5rem;
        }

        .badge-durum {
            padding: 0.35rem 0.75rem;
            border-radius: 6px;
            font-weight: 600;
            font-size: 0.85rem;
        }

        .badge-aktif {
            background: #d1fae5;
            color: #065f46;
        }

        .badge-pasif {
            background: #fee2e2;
            color: #991b1b;
        }

        .modal-header-custom {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            color: white;
        }

        .info-group {
            background: #f8f9fa;
            padding: 1rem;
            border-radius: 8px;
            margin-bottom: 1rem;
        }

            .info-group h6 {
                color: #4B7BEC;
                font-weight: 600;
                margin-bottom: 0.75rem;
                border-bottom: 2px solid #4B7BEC;
                padding-bottom: 0.5rem;
            }

        .info-row {
            display: flex;
            padding: 0.5rem 0;
            border-bottom: 1px solid #e9ecef;
        }

            .info-row:last-child {
                border-bottom: none;
            }

        .info-label {
            font-weight: 600;
            color: #6b7280;
            min-width: 180px;
        }

        .info-value {
            color: #2d3436;
        }

        .personel-foto {
            width: 120px;
            height: 150px;
            object-fit: cover;
            border-radius: 8px;
            border: 3px solid #4B7BEC;
        }

        .no-foto {
            width: 120px;
            height: 150px;
            background: linear-gradient(135deg, #e9ecef 0%, #dee2e6 100%);
            border-radius: 8px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 3rem;
            color: #6b7280;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">

        <!-- Başlık -->
        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h2 class="mb-1">👥 Personel Listesi</h2>
                <p class="text-muted mb-0">Personel kayıtlarını görüntüleyin ve yönetin</p>
            </div>
        </div>

        <!-- İstatistik Kartları -->
        <div class="row g-3 mb-4">
            <div class="col-md-4">
                <div class="stat-card total position-relative">
                    <i class="fas fa-users stat-icon icon-subtle"></i>
                    <asp:Label ID="lblToplamPersonel" runat="server" CssClass="stat-number" Text="0"></asp:Label>
                    <p class="stat-label mb-0">Toplam Personel</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="stat-card active position-relative">
                    <i class="fas fa-user-check stat-icon icon-subtle"></i>
                    <asp:Label ID="lblAktifPersonel" runat="server" CssClass="stat-number text-success" Text="0"></asp:Label>
                    <p class="stat-label mb-0">Aktif Personel</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="stat-card passive position-relative">
                    <i class="fas fa-user-times stat-icon icon-subtle"></i>
                    <asp:Label ID="lblPasifPersonel" runat="server" CssClass="stat-number text-secondary" Text="0"></asp:Label>
                    <p class="stat-label mb-0">Pasif Personel</p>
                </div>
            </div>
        </div>

        <!-- Filtre Bölümü -->
        <div class="filter-card">
            <h5 class="mb-3">
                <i class="fas fa-filter me-2"></i>Filtreleme Seçenekleri
            </h5>
            <div class="row g-3">
                <div class="col-md-3">
                    <label class="form-label">Unvan</label>
                    <asp:DropDownList ID="ddlUnvan" runat="server" CssClass="form-select">
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Mesleki Unvan</label>
                    <asp:DropDownList ID="ddlMeslekiUnvan" runat="server" CssClass="form-select">
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Statü</label>
                    <asp:DropDownList ID="ddlStatu" runat="server" CssClass="form-select">
                        <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                        <asp:ListItem Value="Memur">Memur</asp:ListItem>
                        <asp:ListItem Value="İşçi">İşçi</asp:ListItem>
                        <asp:ListItem Value="Sözleşmeli">Sözleşmeli</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Çalışma Durumu</label>
                    <asp:DropDownList ID="ddlCalismaDurumu" runat="server" CssClass="form-select">
                        <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                        <asp:ListItem Value="Çalışıyor">Çalışıyor</asp:ListItem>
                        <asp:ListItem Value="Geçici Görev">Geçici Görev</asp:ListItem>
                        <asp:ListItem Value="İzinli">İzinli</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Birim</label>
                    <asp:DropDownList ID="ddlBirim" runat="server" CssClass="form-select">
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Cinsiyet</label>
                    <asp:DropDownList ID="ddlCinsiyet" runat="server" CssClass="form-select">
                        <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                        <asp:ListItem Value="Erkek">Erkek</asp:ListItem>
                        <asp:ListItem Value="Kadın">Kadın</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Kan Grubu</label>
                    <asp:DropDownList ID="ddlKanGrubu" runat="server" CssClass="form-select">
                        <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                        <asp:ListItem Value="A Rh+">A Rh+</asp:ListItem>
                        <asp:ListItem Value="A Rh-">A Rh-</asp:ListItem>
                        <asp:ListItem Value="B Rh+">B Rh+</asp:ListItem>
                        <asp:ListItem Value="B Rh-">B Rh-</asp:ListItem>
                        <asp:ListItem Value="AB Rh+">AB Rh+</asp:ListItem>
                        <asp:ListItem Value="AB Rh-">AB Rh-</asp:ListItem>
                        <asp:ListItem Value="0 Rh+">0 Rh+</asp:ListItem>
                        <asp:ListItem Value="0 Rh-">0 Rh-</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Medeni Hal</label>
                    <asp:DropDownList ID="ddlMedeniHal" runat="server" CssClass="form-select">
                        <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                        <asp:ListItem Value="Evli">Evli</asp:ListItem>
                        <asp:ListItem Value="Bekar">Bekar</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Sendika</label>
                    <asp:DropDownList ID="ddlSendika" runat="server" CssClass="form-select">
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Öğrenim Durumu</label>
                    <asp:DropDownList ID="ddlOgrenimDurumu" runat="server" CssClass="form-select">
                        <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                        <asp:ListItem Value="İlkokul">İlkokul</asp:ListItem>
                        <asp:ListItem Value="Ortaokul">Ortaokul</asp:ListItem>
                        <asp:ListItem Value="Lise">Lise</asp:ListItem>
                        <asp:ListItem Value="Ön Lisans">Ön Lisans</asp:ListItem>
                        <asp:ListItem Value="Lisans">Lisans</asp:ListItem>
                        <asp:ListItem Value="Yüksek Lisans">Yüksek Lisans</asp:ListItem>
                        <asp:ListItem Value="Doktora">Doktora</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Durum</label>
                    <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-select">
                        <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                        <asp:ListItem Value="Aktif" Selected="True">Aktif</asp:ListItem>
                        <asp:ListItem Value="Pasif">Pasif</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3 d-flex align-items-end gap-2">
                    <asp:Button ID="btnAra" runat="server" Text="🔍 Ara" CssClass="btn btn-primary"
                        OnClick="btnAra_Click" />
                    <asp:Button ID="btnTumunuListele" runat="server" Text="📜 Tümünü Listele"
                        CssClass="btn btn-outline-secondary" OnClick="btnTumunuListele_Click" />
                </div>
            </div>
        </div>

        <!-- Personel Listesi -->
        <div class="card mb-4">
            <div class="card-header d-flex justify-content-between align-items-center bg-gradient-primary text-white">
                <h5 class="card-title mb-0">
                    <i class="fas fa-list me-2"></i>Personel Listesi
                    <asp:Label ID="lblKayitSayisi" runat="server" CssClass="badge bg-light text-dark ms-2" Text="0 kayıt"></asp:Label>
                </h5>
                <asp:Button ID="btnExcelAktar" runat="server" Text="📊 Excel'e Aktar" 
                    CssClass="btn btn-light btn-sm" OnClick="btnExcelAktar_Click" />
            </div>
            <div class="card-body p-0">
                <div class="table-responsive">
                    <asp:GridView ID="PersonellerGrid" runat="server" CssClass="table table-hover modern-table mb-0"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="25"
                        OnPageIndexChanging="PersonellerGrid_PageIndexChanging"
                        OnRowCommand="PersonellerGrid_RowCommand"
                        EmptyDataText="Kayıt bulunamadı.">
                        <Columns>
                            <asp:BoundField DataField="SicilNo" HeaderText="Sicil No" />
                            <asp:BoundField DataField="Adi" HeaderText="Adı" />
                            <asp:BoundField DataField="Soyad" HeaderText="Soyadı" />
                            <asp:BoundField DataField="TcKimlikNo" HeaderText="TC Kimlik No" />
                            <asp:BoundField DataField="Unvan" HeaderText="Unvan" />
                            <asp:BoundField DataField="MeslekiUnvan" HeaderText="Mesleki Unvan" />
                            <asp:BoundField DataField="GorevYaptigiBirim" HeaderText="Birim" />
                            <asp:BoundField DataField="CalismaDurumu" HeaderText="Çalışma Durumu" />
                            <asp:TemplateField HeaderText="Durum">
                                <ItemTemplate>
                                    <span class='badge-durum <%# Eval("Durum").ToString() == "Aktif" ? "badge-aktif" : "badge-pasif" %>'>
                                        <%# Eval("Durum") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="İşlem">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDetay" runat="server" CssClass="btn btn-sm btn-primary"
                                        CommandName="DetayGoster" CommandArgument='<%# Eval("Personelid") %>'
                                        ToolTip="Detay Görüntüle">
                                        <i class="fas fa-eye"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
            </div>
        </div>

    </div>

    <!-- Modal - Personel Detay -->
    <div class="modal fade" id="modalPersonelDetay" tabindex="-1" aria-labelledby="modalPersonelDetayLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header modal-header-custom">
                    <h5 class="modal-title" id="modalPersonelDetayLabel">
                        <i class="fas fa-user-circle me-2"></i>Personel Detay Bilgileri
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <!-- Sol Kolon - Foto ve Temel Bilgiler -->
                        <div class="col-md-4">
                            <div class="text-center mb-3">
                                <asp:Image ID="imgPersonelFoto" runat="server" CssClass="personel-foto" Visible="false" />
                                <div id="divNoFoto" runat="server" class="no-foto mx-auto" visible="false">
                                    <i class="fas fa-user"></i>
                                </div>
                            </div>
                            <div class="info-group">
                                <h6><i class="fas fa-id-card me-2"></i>Kimlik Bilgileri</h6>
                                <div class="info-row">
                                    <span class="info-label">TC Kimlik No:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayTcNo" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Sicil No:</span>
                                    <span class="info-value"><asp:Label ID="lblDetaySicilNo" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Adı Soyadı:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayAdSoyad" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Doğum Yeri:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayDogumYeri" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Doğum Tarihi:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayDogumTarihi" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Cinsiyet:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayCinsiyet" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Kan Grubu:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayKanGrubu" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Medeni Hal:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayMedeniHal" runat="server"></asp:Label></span>
                                </div>
                            </div>
                        </div>

                        <!-- Orta Kolon - Görev Bilgileri -->
                        <div class="col-md-4">
                            <div class="info-group">
                                <h6><i class="fas fa-briefcase me-2"></i>Görev Bilgileri</h6>
                                <div class="info-row">
                                    <span class="info-label">Unvan:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayUnvan" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Mesleki Unvan:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayMeslekiUnvan" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Statü:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayStatu" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Görev Yaptığı Birim:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayBirim" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Çalışma Durumu:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayCalismaDurumu" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">İlk İşe Giriş:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayIlkIseGiris" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Kuruma Başlama:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayKurumaBaslama" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Sendika:</span>
                                    <span class="info-value"><asp:Label ID="lblDetaySendika" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Durum:</span>
                                    <span class="info-value">
                                        <asp:Label ID="lblDetayDurum" runat="server"></asp:Label>
                                    </span>
                                </div>
                            </div>

                            <div class="info-group">
                                <h6><i class="fas fa-graduation-cap me-2"></i>Öğrenim Durumu</h6>
                                <div class="info-row">
                                    <span class="info-label">Öğrenim:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayOgrenim" runat="server"></asp:Label></span>
                                </div>
                            </div>
                        </div>

                        <!-- Sağ Kolon - İletişim Bilgileri -->
                        <div class="col-md-4">
                            <div class="info-group">
                                <h6><i class="fas fa-phone me-2"></i>İletişim Bilgileri</h6>
                                <div class="info-row">
                                    <span class="info-label">Cep Telefonu:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayCepTel" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Ev Telefonu:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayEvTel" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Dahili:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayDahili" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">E-posta:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayMail" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Adres:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayAdres" runat="server"></asp:Label></span>
                                </div>
                            </div>

                            <div class="info-group">
                                <h6><i class="fas fa-exclamation-circle me-2"></i>Acil Durum</h6>
                                <div class="info-row">
                                    <span class="info-label">Kişi:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayAcilKisi" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Telefon:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayAcilTel" runat="server"></asp:Label></span>
                                </div>
                            </div>

                            <div class="info-group">
                                <h6><i class="fas fa-calendar-alt me-2"></i>İzin Bilgileri</h6>
                                <div class="info-row">
                                    <span class="info-label">Devreden İzin:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayDevredenIzin" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Cari Yıl İzin:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayCariYilIzin" runat="server"></asp:Label></span>
                                </div>
                                <div class="info-row">
                                    <span class="info-label">Toplam İzin:</span>
                                    <span class="info-value"><asp:Label ID="lblDetayToplamIzin" runat="server"></asp:Label></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                        <i class="fas fa-times me-1"></i>Kapat
                    </button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>