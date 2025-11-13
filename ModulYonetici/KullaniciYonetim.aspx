<%@ Page Title="Kullanıcı ve Yetki Yönetimi" Language="C#" MasterPageFile="~/AnaV2.Master"
    AutoEventWireup="true" CodeBehind="KullaniciYonetim.aspx.cs"
    Inherits="Portal.ModulYonetici.KullaniciYonetim" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .yonetim-container {
            background: #ffffff;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
            padding: 0;
            margin-bottom: 20px;
        }

        .page-header-custom {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            color: white;
            padding: 25px;
            border-radius: 12px 12px 0 0;
        }

        .nav-tabs-custom {
            background: #f8f9fa;
            border-bottom: 3px solid #4B7BEC;
            padding: 0 20px;
        }

        .nav-tabs-custom .nav-link {
            color: #6c757d;
            font-weight: 600;
            padding: 15px 25px;
            border: none;
            border-bottom: 3px solid transparent;
            margin-bottom: -3px;
            transition: all 0.3s;
        }

        .nav-tabs-custom .nav-link:hover {
            color: #4B7BEC;
            border-bottom-color: #4B7BEC;
        }

        .nav-tabs-custom .nav-link.active {
            color: #4B7BEC;
            background: white;
            border-bottom-color: #4B7BEC;
        }

        .tab-content-custom {
            padding: 30px;
        }

        .filter-card {
            background: #f8f9fa;
            border-left: 4px solid #4B7BEC;
            padding: 20px;
            border-radius: 8px;
            margin-bottom: 20px;
        }

        .kullanici-table {
            background: white;
        }

        .kullanici-table th {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            color: white;
            font-weight: 500;
            padding: 12px;
            border: none;
        }

        .kullanici-table td {
            padding: 12px;
            vertical-align: middle;
        }

        .kullanici-table tbody tr:hover {
            background-color: #f1f5ff;
            cursor: pointer;
        }

        .badge-custom {
            padding: 6px 12px;
            border-radius: 6px;
            font-size: 0.85rem;
            font-weight: 500;
        }

        .detail-card {
            background: #f8f9fa;
            border-radius: 8px;
            padding: 20px;
            height: 100%;
        }

        .detail-card h5 {
            color: #2E5B9A;
            border-bottom: 2px solid #4B7BEC;
            padding-bottom: 10px;
            margin-bottom: 20px;
        }

        .yetki-list {
            list-style: none;
            padding: 0;
        }

        .yetki-list li {
            padding: 10px;
            margin-bottom: 8px;
            background: white;
            border-radius: 6px;
            border-left: 3px solid #28a745;
        }

        .yetki-list li i {
            color: #28a745;
            margin-right: 8px;
        }

        .matrix-container {
            max-height: 600px;
            overflow-x: auto;
            overflow-y: auto;
            border: 1px solid #dee2e6;
            border-radius: 8px;
        }

        .matrix-table {
            min-width: 800px;
            margin-bottom: 0;
        }

        .matrix-table thead {
            position: sticky;
            top: 0;
            z-index: 100;
        }

        .matrix-table th {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            color: white;
            font-weight: 500;
            padding: 12px;
            border: 1px solid #dee2e6;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }

        .matrix-table td {
            text-align: center;
            padding: 12px;
            border: 1px solid #dee2e6;
        }

        .matrix-table tbody tr:hover {
            background-color: #f1f5ff;
        }

        .matrix-checkbox {
            width: 20px;
            height: 20px;
            cursor: pointer;
        }

        .user-name-cell {
            background: #f8f9fa !important;
            font-weight: 600;
            text-align: left !important;
            position: sticky;
            left: 0;
            z-index: 50;
            box-shadow: 2px 0 4px rgba(0,0,0,0.1);
        }

        .matrix-table thead .user-name-cell {
            z-index: 150;
        }

        .action-btn {
            padding: 6px 12px;
            margin: 0 3px;
            font-size: 0.85rem;
        }

        .info-text {
            color: #6c757d;
            font-size: 0.9rem;
            font-style: italic;
        }

        .empty-state {
            text-align: center;
            padding: 60px 20px;
            color: #6c757d;
        }

        .empty-state i {
            font-size: 4rem;
            margin-bottom: 20px;
            opacity: 0.3;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">

        <!-- Header -->
        <div class="yonetim-container">
            <div class="page-header-custom">
                <h2 class="mb-1">
                    <i class="fas fa-user-shield me-3"></i>Kullanıcı ve Yetki Yönetimi
                </h2>
                <p class="mb-0 opacity-75">Kullanıcı işlemleri, yetki atama ve toplu yönetim</p>
            </div>

            <!-- Tabs -->
            <ul class="nav nav-tabs nav-tabs-custom" id="yonetimTabs" role="tablist">
                <li class="nav-item" role="presentation">
                    <button class="nav-link active" id="liste-tab" data-bs-toggle="tab"
                        data-bs-target="#liste-content" type="button" role="tab">
                        <i class="fas fa-list me-2"></i>Kullanıcı Listesi
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link" id="detay-tab" data-bs-toggle="tab"
                        data-bs-target="#detay-content" type="button" role="tab">
                        <i class="fas fa-user-tag me-2"></i>Kullanıcı Detay
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link" id="matris-tab" data-bs-toggle="tab"
                        data-bs-target="#matris-content" type="button" role="tab">
                        <i class="fas fa-table me-2"></i>Yetki Matrisi
                    </button>
                </li>
            </ul>

            <!-- Tab Content -->
            <div class="tab-content tab-content-custom" id="yonetimTabContent">

                <!-- SEKME 1: Kullanıcı Listesi -->
                <div class="tab-pane fade show active" id="liste-content" role="tabpanel">
                    
                    <!-- Filtreleme Bölümü -->
                    <div class="filter-card">
                        <h5 class="mb-3">
                            <i class="fas fa-filter me-2"></i>Gelişmiş Arama ve Filtreleme
                        </h5>
                        <div class="row g-3">
                            <div class="col-md-3">
                                <label class="form-label">Sicil No</label>
                                <input type="text" id="txtFilterSicilNo" class="form-control"
                                    placeholder="Sicil no ara..." onkeyup="filterKullanicilar()">
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Ad Soyad</label>
                                <input type="text" id="txtFilterAdSoyad" class="form-control"
                                    placeholder="Ad soyad ara..." onkeyup="filterKullanicilar()">
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Kullanıcı Türü</label>
                                <asp:DropDownList ID="ddlFilterKullaniciTuru" runat="server"
                                    CssClass="form-select" onchange="filterKullanicilar()">
                                    <asp:ListItem Value="">Tümü</asp:ListItem>
                                    <asp:ListItem Value="Admin">Admin</asp:ListItem>
                                    <asp:ListItem Value="Personel">Personel</asp:ListItem>
                                    <asp:ListItem Value="Misafir">Misafir</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Durum</label>
                                <asp:DropDownList ID="ddlFilterDurum" runat="server"
                                    CssClass="form-select" onchange="filterKullanicilar()">
                                    <asp:ListItem Value="">Tümü</asp:ListItem>
                                    <asp:ListItem Value="Aktif">Aktif</asp:ListItem>
                                    <asp:ListItem Value="Pasif">Pasif</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="mt-3">
                            <button type="button" class="btn btn-sm btn-secondary" onclick="clearFilters()">
                                <i class="fas fa-times me-1"></i>Filtreleri Temizle
                            </button>
                            <span id="recordCount" class="ms-3 info-text"></span>
                        </div>
                    </div>

                    <!-- Kullanıcı Tablosu -->
                    <div class="table-responsive">
                        <asp:GridView ID="gvKullanicilar" runat="server" CssClass="table table-hover kullanici-table"
                            AutoGenerateColumns="False" DataKeyNames="Sicil_No"
                            EmptyDataText="Kayıt bulunamadı." GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="Sicil_No" HeaderText="Sicil No" />
                                <asp:BoundField DataField="Adi_Soyadi" HeaderText="Ad Soyad" />
                                <asp:BoundField DataField="Kullanici_Turu" HeaderText="Kullanıcı Türü" />
                                <asp:BoundField DataField="Personel_Tipi" HeaderText="Personel Tipi" />
                                <asp:BoundField DataField="Birim" HeaderText="Birim" />
                                <asp:BoundField DataField="Mail_Adresi" HeaderText="Mail Adresi" />
                                <asp:TemplateField HeaderText="Durum">
                                    <ItemTemplate>
                                        <span class='badge <%# Eval("Durum").ToString() == "Aktif" ? "bg-success" : "bg-secondary" %> badge-custom'>
                                            <%# Eval("Durum") %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="İşlemler">
                                    <ItemTemplate>
                                        <asp:Button ID="btnDetayGit" runat="server"
                                            CommandArgument='<%# Eval("Sicil_No") %>'
                                            Text="🔍 Detay" CssClass="btn btn-sm btn-info action-btn"
                                            OnClick="btnDetayGit_Click" CausesValidation="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                </div>

                <!-- SEKME 2: Kullanıcı Detay -->
                <div class="tab-pane fade" id="detay-content" role="tabpanel">
                    
                    <div class="row">
                        <!-- Sol: Kullanıcı Bilgileri ve Yetkileri -->
                        <div class="col-lg-6">
                            <div class="detail-card">
                                <h5>
                                    <i class="fas fa-user-circle me-2"></i>Kullanıcı Bilgileri
                                </h5>
                                
                                <asp:Panel ID="pnlKullaniciDetay" runat="server" Visible="false">
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">Sicil No:</label>
                                        <asp:Label ID="lblDetaySicilNo" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">Ad Soyad:</label>
                                        <asp:Label ID="lblDetayAdSoyad" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">Kullanıcı Türü:</label>
                                        <asp:Label ID="lblDetayKullaniciTuru" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">Durum:</label>
                                        <asp:Label ID="lblDetayDurum" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">Mail Adresi:</label>
                                        <asp:Label ID="lblDetayMail" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    </div>

                                    <hr />

                                    <h6 class="mb-3">
                                        <i class="fas fa-key me-2"></i>Kullanıcının Yetkileri
                                    </h6>
                                    <asp:Panel ID="pnlYetkiListesi" runat="server">
                                        <ul class="yetki-list">
                                            <asp:Repeater ID="rptKullaniciYetkileri" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <i class="fas fa-check-circle"></i>
                                                        <%# Eval("Yetki") %> <span class="text-muted">(<%# Eval("Yetki_No") %>)</span>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </asp:Panel>
                                    <asp:Label ID="lblYetkiYok" runat="server" CssClass="info-text"
                                        Text="Bu kullanıcıya henüz yetki atanmamış." Visible="false"></asp:Label>
                                </asp:Panel>

                                <div class="empty-state" runat="server" id="divDetayBos" visible="true">
                                    <i class="fas fa-user-slash"></i>
                                    <p>Detay görmek için bir kullanıcı seçin.</p>
                                </div>

                            </div>
                        </div>

                        <!-- Sağ: Yetkiye Göre Kullanıcılar -->
                        <div class="col-lg-6">
                            <div class="detail-card">
                                <h5>
                                    <i class="fas fa-users me-2"></i>Yetkiye Göre Kullanıcılar
                                </h5>

                                <div class="mb-3">
                                    <label class="form-label">Yetki Seçin:</label>
                                    <asp:DropDownList ID="ddlYetkiSec" runat="server" CssClass="form-select"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlYetkiSec_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>

                                <asp:Panel ID="pnlYetkiyeGoreKullanicilar" runat="server" Visible="false">
                                    <div class="alert alert-info">
                                        <i class="fas fa-info-circle me-2"></i>
                                        <strong>Toplam <asp:Label ID="lblYetkiKullaniciSayisi" runat="server"></asp:Label> kullanıcı</strong>
                                        bu yetkiye sahip.
                                    </div>
                                    <ul class="list-group">
                                        <asp:Repeater ID="rptYetkiyeGoreKullanicilar" runat="server">
                                            <ItemTemplate>
                                                <li class="list-group-item d-flex justify-content-between align-items-center">
                                                    <div>
                                                        <i class="fas fa-user me-2 text-primary"></i>
                                                        <strong><%# Eval("Adi_Soyadi") %></strong>
                                                        <span class="text-muted ms-2">(<%# Eval("Sicil_No") %>)</span>
                                                    </div>
                                                    <span class='badge <%# Eval("Durum").ToString() == "Aktif" ? "bg-success" : "bg-secondary" %>'>
                                                        <%# Eval("Durum") %>
                                                    </span>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </asp:Panel>

                                <div class="empty-state" runat="server" id="divYetkiyeGoreBos" visible="true">
                                    <i class="fas fa-user-lock"></i>
                                    <p>Yetki seçin ve bu yetkiye sahip kullanıcıları görün.</p>
                                </div>

                            </div>
                        </div>
                    </div>

                </div>

                <!-- SEKME 3: Yetki Matrisi -->
                <div class="tab-pane fade" id="matris-content" role="tabpanel">
                    
                    <div class="alert alert-info">
                        <i class="fas fa-info-circle me-2"></i>
                        <strong>Kullanım:</strong> Checkbox'ları işaretleyerek/kaldırarak yetki düzenleyin. 
                        Değişikliklerinizi kaydetmek için <strong>"Değişiklikleri Kaydet"</strong> butonuna tıklayın.
                    </div>

                    <asp:Panel ID="pnlMatris" runat="server">
                        <div class="matrix-container">
                            <asp:Literal ID="ltMatrisTable" runat="server"></asp:Literal>
                        </div>

                        <div class="mt-4 text-center">
                            <asp:Button ID="btnMatrisKaydet" runat="server" Text="💾 Değişiklikleri Kaydet"
                                CssClass="btn btn-primary btn-lg px-5" OnClick="btnMatrisKaydet_Click"
                                OnClientClick="return confirmMatrisChanges();" />
                            
                            <asp:Button ID="btnMatrisYenile" runat="server" Text="🔄 Yenile"
                                CssClass="btn btn-secondary btn-lg px-4 ms-2" OnClick="btnMatrisYenile_Click"
                                CausesValidation="false" />
                        </div>
                    </asp:Panel>

                    <div class="empty-state" runat="server" id="divMatrisBos" visible="true">
                        <i class="fas fa-table"></i>
                        <p>Yetki matrisi yükleniyor...</p>
                    </div>

                </div>

            </div>

        </div>

    </div>

    <!-- JavaScript -->
    <script type="text/javascript">
        // Kullanıcı listesi filtreleme (Client-side)
        function filterKullanicilar() {
            const sicilNo = document.getElementById('txtFilterSicilNo').value.toLowerCase();
            const adSoyad = document.getElementById('txtFilterAdSoyad').value.toLowerCase();
            const kullaniciTuru = document.getElementById('<%= ddlFilterKullaniciTuru.ClientID %>').value.toLowerCase();
            const durum = document.getElementById('<%= ddlFilterDurum.ClientID %>').value.toLowerCase();

            const table = document.querySelector('.kullanici-table');
            const rows = table.querySelectorAll('tbody tr');
            let visibleCount = 0;

            rows.forEach(row => {
                const cells = row.querySelectorAll('td');
                if (cells.length > 0) {
                    const rowSicilNo = cells[0].textContent.toLowerCase();
                    const rowAdSoyad = cells[1].textContent.toLowerCase();
                    const rowKullaniciTuru = cells[2].textContent.toLowerCase();
                    const rowDurum = cells[6].textContent.toLowerCase();

                    const matchSicilNo = sicilNo === '' || rowSicilNo.includes(sicilNo);
                    const matchAdSoyad = adSoyad === '' || rowAdSoyad.includes(adSoyad);
                    const matchKullaniciTuru = kullaniciTuru === '' || rowKullaniciTuru.includes(kullaniciTuru);
                    const matchDurum = durum === '' || rowDurum.includes(durum);

                    if (matchSicilNo && matchAdSoyad && matchKullaniciTuru && matchDurum) {
                        row.style.display = '';
                        visibleCount++;
                    } else {
                        row.style.display = 'none';
                    }
                }
            });

            document.getElementById('recordCount').textContent = visibleCount + ' kayıt gösteriliyor';
        }

        // Filtreleri temizle
        function clearFilters() {
            document.getElementById('txtFilterSicilNo').value = '';
            document.getElementById('txtFilterAdSoyad').value = '';
            document.getElementById('<%= ddlFilterKullaniciTuru.ClientID %>').selectedIndex = 0;
            document.getElementById('<%= ddlFilterDurum.ClientID %>').selectedIndex = 0;
            filterKullanicilar();
        }

        // Sayfa yüklendiğinde kayıt sayısını göster
        window.addEventListener('load', function () {
            filterKullanicilar();
        });

        // Yetki matrisi kaydetme için çift onay
        function confirmMatrisChanges() {
            if (!confirm('Yetki değişikliklerini kaydetmek istediğinizden emin misiniz?')) {
                return false;
            }
            if (!confirm('SON ONAY: Bu işlem geri alınamaz. Devam etmek istiyor musunuz?')) {
                return false;
            }
            return true;
        }
    </script>

</asp:Content>
