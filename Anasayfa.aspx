<%@ Page Title="Ana Sayfa" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="Anasayfa.aspx.cs" Inherits="Portal.Anasayfa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">    
    <!--  Welcome Banner Section -->
    <div class="welcome-banner mb-4">
        <div class="d-flex align-items-center">
            <div class="welcome-icon">
                <i class="fas fa-home"></i>
            </div>
            <div>
                <h4 class="mb-1 fw-bold">Hoş Geldiniz!</h4>
                <p class="mb-0 welcome-subtitle">Ankara II. Bölge Portalı - Ana Sayfa</p>
            </div>
        </div>
    </div>

    <!--  Top Row: Personal Info, News, Weather -->
    <div class="row g-4 mb-4">
        <!--  Personal Information and Announcements -->
        <div class="col-lg-4">
            <div class="modern-card">
                <div class="card-header-modern bg-gradient-primary">
                    <div class="d-flex align-items-center">
                        <div class="header-icon">
                            <i class="fas fa-user-circle"></i>
                        </div>
                        <div>
                            <h6 class="mb-0 fw-bold">Kişisel Bilgilerim</h6>
                            <small class="opacity-90">Hesap ve İzin Bilgileri</small>
                        </div>
                    </div>
                </div>
                <div class="card-body-modern">
                    <!--  Personal Info -->
                    <div class="info-box mb-3">
                        <div class="info-label">
                            <i class="fas fa-user me-2"></i>Adı Soyadı
                        </div>
                        <div class="info-value">
                            <asp:Label ID="lblPersonelAdi" runat="server" CssClass="fw-bold text-primary" Text=""></asp:Label>
                        </div>
                    </div>

                    <!--  Leave Information -->
                    <div class="leave-summary mb-3">
                        <h6 class="section-subtitle">
                            <i class="fas fa-calendar-check me-2"></i>Cari Yılda Kullandığım İzinler
                        </h6>
                        <div class="row g-2">
                            <div class="col-6">
                                <div class="leave-item">
                                    <div class="leave-type">Yıllık İzin</div>
                                    <asp:Label ID="lblYillikIzin" runat="server" CssClass="leave-badge badge-danger"></asp:Label>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="leave-item">
                                    <div class="leave-type">Rapor</div>
                                    <asp:Label ID="lblRaporIzin" runat="server" CssClass="leave-badge badge-danger"></asp:Label>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="leave-item">
                                    <div class="leave-type">Saatlik</div>
                                    <asp:Label ID="lblSaatlikIzin" runat="server" CssClass="leave-badge badge-danger"></asp:Label>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="leave-item">
                                    <div class="leave-type">Mazeret</div>
                                    <asp:Label ID="lblMazeretIzin" runat="server" CssClass="leave-badge badge-danger"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <hr class="divider-line">

                    <!--  Announcements -->
                    <div class="announcements-section">
                        <h6 class="section-subtitle">
                            <i class="fas fa-bullhorn me-2"></i>Güncel Duyurular
                        </h6>
                        <div class="announcements-list">
                            <asp:Repeater ID="rptDuyurular" runat="server">
                                <ItemTemplate>
                                    <div class="announcement-item">
                                        <a href='<%# Eval("Dosya") != null && !string.IsNullOrEmpty(Eval("Dosya").ToString()) ? Eval("Dosya").ToString().Remove(0,2) : "#" %>' class="announcement-link">
                                            <div class="announcement-content">
                                                <div class="announcement-icon">
                                                    <i class="fas fa-file-alt"></i>
                                                </div>
                                                <div class="announcement-text">
                                                    <asp:Label ID="lblDuyuru" runat="server" Text='<%# Eval("Duyuru") %>' CssClass="announcement-title"></asp:Label>
                                                    <div class="announcement-date">
                                                        <i class="far fa-calendar-alt me-1"></i><%# Eval("Kayit_Tarihi", "{0:dd/MM/yyyy}") %>
                                                    </div>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <asp:Label ID="lblNoDuyuru" runat="server" Text="Duyuru bulunamadı." Visible="false" CssClass="empty-message"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <!--  Middle Column: Resmi Linkler + Kurum İçi Sistemler -->
        <div class="col-lg-4">
            <!--  Resmi Linkler Section -->
            <div class="modern-card mb-4">
                <div class="card-header-modern bg-gradient-info">
                    <div class="d-flex align-items-center">
                        <div class="header-icon">
                            <i class="fas fa-link"></i>
                        </div>
                        <div>
                            <h6 class="mb-0 fw-bold">Resmi Linkler</h6>
                            <small class="opacity-90">Hızlı Erişim</small>
                        </div>
                    </div>
                </div>
                <div class="card-body-modern">
                    <div class="official-links-container">
                        <!--  Cumhurbaşkanlığı -->
                        <a href="https://www.tccb.gov.tr/" target="_blank" class="official-link-item">
                            <div class="link-logo">
                                <img src="<%= ResolveUrl("~/wwwroot/images/cumhurbaskanligi.png") %>" alt="Cumhurbaşkanlığı">
                            </div>
                            <div class="link-text">
                                <span class="link-title">Cumhurbaşkanlığı</span>
                                <small class="link-url">tccb.gov.tr</small>
                            </div>
                            <i class="fas fa-external-link-alt link-icon"></i>
                        </a>

                        <!--  Resmi Gazete -->
                        <a href="https://www.resmigazete.gov.tr/" target="_blank" class="official-link-item">
                            <div class="link-logo">
                                <img src="<%= ResolveUrl("~/wwwroot/images/resmigazete.png") %>" alt="Resmi Gazete">
                            </div>
                            <div class="link-text">
                                <span class="link-title">Resmi Gazete</span>
                                <small class="link-url">resmigazete.gov.tr</small>
                            </div>
                            <i class="fas fa-external-link-alt link-icon"></i>
                        </a>

                        <!--  Ulaştırma ve Altyapı Bakanlığı -->
                        <a href="https://www.uab.gov.tr/" target="_blank" class="official-link-item">
                            <div class="link-logo">
                                <img src="<%= ResolveUrl("~/wwwroot/images/ulastirma-bakanligi.png") %>" alt="Ulaştırma Bakanlığı">
                            </div>
                            <div class="link-text">
                                <span class="link-title">Ulaştırma ve Altyapı Bakanlığı</span>
                                <small class="link-url">uab.gov.tr</small>
                            </div>
                            <i class="fas fa-external-link-alt link-icon"></i>
                        </a>

                        <!--  UHDGM -->
                        <a href="https://uhdgm.uab.gov.tr/" target="_blank" class="official-link-item">
                            <div class="link-logo">
                                <img src="<%= ResolveUrl("~/wwwroot/images/uhdgm.png") %>" alt="UHDGM">
                            </div>
                            <div class="link-text">
                                <span class="link-title">Ulaştırma Hizmetleri Düzenleme Genel Müdürlüğü</span>
                                <small class="link-url">uhdgm.uab.gov.tr</small>
                            </div>
                            <i class="fas fa-external-link-alt link-icon"></i>
                        </a>
                    </div>
                </div>
            </div>

            <!--  Kurum İçi Sistemler Section -->
            <div class="modern-card">
                <div class="card-header-modern bg-gradient-info">
                    <div class="d-flex align-items-center">
                        <div class="header-icon">
                            <i class="fas fa-cogs"></i>
                        </div>
                        <div>
                            <h6 class="mb-0 fw-bold">Kurum İçi Sistemler</h6>
                            <small class="opacity-90">Dahili Uygulamalar</small>
                        </div>
                    </div>
                </div>
                <div class="card-body-modern">
                    <div class="official-links-container">
                        <!--  U-NET Otomasyon -->
                        <a href="https://unet.uab.gov.tr" target="_blank" class="official-link-item">
                            <div class="link-logo">
                                <img src="<%= ResolveUrl("~/wwwroot/images/unet.png") %>" alt="U-NET">
                            </div>
                            <div class="link-text">
                                <span class="link-title">U-NET Otomasyon</span>
                                <small class="link-url">unet.uab.gov.tr</small>
                            </div>
                            <i class="fas fa-external-link-alt link-icon"></i>
                        </a>

                        <!--  Belge Yönetim Sistemi -->
                        <a href="https://belgenet.uab.gov.tr" target="_blank" class="official-link-item">
                            <div class="link-logo">
                                <img src="<%= ResolveUrl("~/wwwroot/images/belgenet.jpg") %>" alt="BelgeNet">
                            </div>
                            <div class="link-text">
                                <span class="link-title">Belge Yönetim Sistemi</span>
                                <small class="link-url">belgenet.uab.gov.tr</small>
                            </div>
                            <i class="fas fa-external-link-alt link-icon"></i>
                        </a>

                        <!--  Kurum Email -->
                        <a href="https://mail.uab.gov.tr" target="_blank" class="official-link-item">
                            <div class="link-logo">
                                <img src="<%= ResolveUrl("~/wwwroot/images/mail.png") %>" alt="Email">
                            </div>
                            <div class="link-text">
                                <span class="link-title">Kurum Email</span>
                                <small class="link-url">mail.uab.gov.tr</small>
                            </div>
                            <i class="fas fa-external-link-alt link-icon"></i>
                        </a>
                    </div>
                </div>
            </div>
        </div>

        <!--  Weather Section -->
        <div class="col-lg-4">
            <%-- Hava Durumu Widget --%>
            <div class="card shadow-sm">
                <div class="card-header bg-primary text-white">
                    <h5 class="mb-0">
                        <i class="fas fa-cloud-sun me-2"></i>Hava Durumu
                    </h5>
                </div>
                <div class="card-body">
                    <%-- Şehir Seçimi --%>
                    <div class="mb-3">
                        <label for="ddlCity" class="form-label">Şehir Seçin:</label>
                        <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-select"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>

                    <%-- Güncel Hava Durumu --%>
                    <div id="currentWeather" class="mb-4">
                        <div class="row align-items-center">
                            <div class="col-md-6 text-center">
                                <img id="weatherIcon" src="" alt="Hava Durumu" style="width: 80px;">
                                <h2 id="temperature" class="mb-0">--°C</h2>
                                <p id="description" class="text-muted mb-0">-</p>
                            </div>
                            <div class="col-md-6">
                                <div class="weather-details">
                                    <p class="mb-2">
                                        <i class="fas fa-tint text-info me-2"></i>
                                        <strong>Nem:</strong> <span id="humidity">--%</span>
                                    </p>
                                    <p class="mb-2">
                                        <i class="fas fa-wind text-secondary me-2"></i>
                                        <strong>Rüzgar:</strong> <span id="windSpeed">-- km/s</span>
                                    </p>
                                    <p class="mb-2">
                                        <i class="fas fa-compress-arrows-alt text-warning me-2"></i>
                                        <strong>Basınç:</strong> <span id="pressure">-- hPa</span>
                                    </p>
                                    <p class="mb-0">
                                        <i class="fas fa-eye text-primary me-2"></i>
                                        <strong>Görüş:</strong> <span id="visibility">-- km</span>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <hr>

                    <%-- 5 Günlük Tahmin --%>
                    <h6 class="mb-3">5 Günlük Tahmin</h6>
                    <div id="forecast" class="row row-cols-5 g-2">
                        <%-- JavaScript ile doldurulacak --%>
                    </div>

                    <%-- Loading Spinner --%>
                    <div id="weatherLoading" class="text-center d-none">
                        <div class="spinner-border text-primary" role="status">
                            <span class="visually-hidden">Yükleniyor...</span>
                        </div>
                        <p class="mt-2 text-muted">Hava durumu bilgisi yükleniyor...</p>
                    </div>
                </div>
            </div>

            <%-- Hidden Field (JSON verisi için) --%>
            <asp:HiddenField ID="hfWeatherData" runat="server" />
        </div>
    </div>

        <!--  Middle Row: Inspection Duties and Birthdays -->
        <div class="row g-4 mb-4">
            <!--  Inspection Duties -->
            <div class="col-lg-8">
                <div class="modern-card">
                    <div class="card-header-modern bg-gradient-inspection">
                        <div class="d-flex align-items-center">
                            <div class="header-icon">
                                <i class="fas fa-map-marker-alt"></i>
                            </div>
                            <div>
                                <h6 class="mb-0 fw-bold">Denetim Görevleri</h6>
                                <small class="opacity-90">Personellerimiz Nerede</small>
                            </div>
                        </div>
                    </div>
                    <div class="card-body-modern p-0">
                        <div class="table-responsive">
                            <asp:GridView ID="grdDenetimGorevleri" runat="server"
                                AutoGenerateColumns="False"
                                CssClass="modern-table mb-0"
                                ShowFooter="true"
                                EmptyDataText="Aktif denetim görevi bulunmamaktadır.">
                                <Columns>
                                    <asp:BoundField DataField="AdiSoyadi" HeaderText="Adı Soyadı" />
                                    <asp:BoundField DataField="il" HeaderText="İl" />
                                    <asp:BoundField DataField="Digeriller" HeaderText="Diğer İller" />
                                    <asp:BoundField DataField="BaslamaTarihi" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Başlama" />
                                    <asp:BoundField DataField="BitisTarihi" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Bitiş" />
                                    <asp:BoundField DataField="Aciklama" HeaderText="Görev Tanımı" />
                                </Columns>
                                <EmptyDataRowStyle CssClass="empty-row" />
                                <FooterStyle CssClass="table-footer" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>

            <!--  Birthdays (sağda ve daha küçük) -->
            <div class="col-lg-4">
                <div class="modern-card h-100">
                    <div class="card-header-modern bg-gradient-birthday">
                        <div class="d-flex align-items-center">
                            <div class="header-icon">
                                <i class="fas fa-birthday-cake"></i>
                            </div>
                            <div>
                                <h6 class="mb-0 fw-bold">Bugün Doğum Günü Olanlar</h6>
                            </div>
                        </div>
                    </div>
                    <div class="card-body-modern">
                        <div class="birthdays-container-vertical">
                            <asp:Repeater ID="rptDogumGunleri" runat="server">
                                <ItemTemplate>
                                    <div class="birthday-item-vertical">
                                        <div class="birthday-icon">
                                            <i class="fas fa-gift"></i>
                                        </div>
                                        <div class="birthday-name">
                                            <%# Eval("Adi") %> <%# Eval("Soyad") %>
                                        </div>
                                        <div class="birthday-decoration">
                                            🎂
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <asp:Label ID="lblNoDogumGunu" runat="server"
                            Text="Bugün doğum günü olan personel bulunmamaktadır."
                            Visible="false"
                            CssClass="empty-message"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <!--  Bottom Row: Aktif İzinler (tek başına altta) -->
        <div class="row g-4">
            <div class="col-12">
                <div class="modern-card">
                    <div class="card-header-modern bg-gradient-success">
                        <div class="d-flex align-items-center justify-content-between w-100">
                            <div class="d-flex align-items-center">
                                <div class="header-icon">
                                    <i class="fas fa-calendar-check"></i>
                                </div>
                                <div>
                                    <h6 class="mb-0 fw-bold">Aktif İzinler</h6>
                                    <small class="opacity-90">Şu Anda İzinde Olan Personeller</small>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-body-modern p-0">
                        <div class="table-responsive">
                            <asp:GridView ID="grdAktifIzinler" runat="server"
                                AutoGenerateColumns="False"
                                CssClass="modern-table mb-0"
                                ShowFooter="true"
                                EmptyDataText="Aktif izin bulunmamaktadır.">
                                <Columns>
                                    <asp:BoundField DataField="Adi_Soyadi" HeaderText="Adı Soyadı" />
                                    <asp:BoundField DataField="izin_turu" HeaderText="İzin Türü" />
                                    <asp:TemplateField HeaderText="Süre (Saat)">
                                        <ItemTemplate>
                                            <%# FormatDurationForGrid(Eval("izin_Suresi")) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Goreve_Baslama_Tarihi" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Göreve Dönüş" />
                                </Columns>
                                <EmptyDataRowStyle CssClass="empty-row" />
                                <FooterStyle CssClass="table-footer" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--  Hidden GridView for Data -->
        <asp:GridView ID="grdKullanilanIzinler" runat="server" AutoGenerateColumns="False" Visible="false">
            <Columns>
                <asp:BoundField DataField="Sicil_No" HeaderText="Sicil No" />
                <asp:BoundField DataField="Toplam_Yillik" HeaderText="Yıllık" NullDisplayText="0" />
                <asp:BoundField DataField="Toplam_Rapor" HeaderText="Rapor" NullDisplayText="0" />
                <asp:BoundField DataField="Toplam_Saatlik" HeaderText="Saatlik" NullDisplayText="0" />
                <asp:BoundField DataField="Toplam_Mazeret" HeaderText="Mazeret" NullDisplayText="0" />
            </Columns>
        </asp:GridView>

        <%-- JavaScript dosyasını dahil et --%>
        <script src="<%= ResolveUrl("~/wwwroot/js/weather-widget.js") %>" type="text/javascript"></script>

        <%-- Widget'ı başlat --%>
        <script type="text/javascript">
            //  Sayfa yüklendiğinde widget'ı başlat
            document.addEventListener('DOMContentLoaded', function () {
                initWeatherWidget('<%= hfWeatherData.ClientID %>');
            });
        </script>
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="head">
</asp:Content>
