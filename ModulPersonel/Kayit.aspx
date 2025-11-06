<%@ Page Title="Personel Kayıt" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="Kayit.aspx.cs" Inherits="Portal.ModulPersonel.Kayit" EnableEventValidation="false" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <style>
        /*   TC Kimlik No Validation Styling (Sayfa özel) */
        .text-danger {
            color: #dc2626;
            font-size: 0.85rem;
            margin-top: 0.25rem;
        }

        .text-success {
            color: #059669;
            font-size: 0.85rem;
            margin-top: 0.25rem;
        }

        /*   Image Preview Styling */
        .img-preview {
            border: 2px solid #e5e7eb;
            border-radius: 8px;
            padding: 0.5rem;
            max-width: 200px;
            max-height: 200px;
            object-fit: cover;
        }
    </style>

    <%--   Print Panel Script (Unchanged) --%>
    <script type="text/javascript">
        function YazdirmaPaneli() {
            var panel = document.getElementById("<%=pnlYazdir.ClientID %>");
            var printWindow = window.open('', '', 'height=600,width=900');
            printWindow.document.write('<html><head><title>Personel Bilgi Formu</title>');
            printWindow.document.write('<style>body { font-family: Arial, sans-serif; } table { width: 100%; border-collapse: collapse; } th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }</style>');
            printWindow.document.write('</head><body>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
</asp:Content>

<%--   Enhanced Breadcrumb Section --%>
<asp:Content ID="ContentBreadcrumb" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item">
        <i class="fas fa-users me-1"></i>Personel İşlemleri
    </li>
    <li class="breadcrumb-item active" aria-current="page">Personel Ekle/Düzenle</li>
</asp:Content>

<%--   Enhanced Main Content Section --%>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-12">
                <%--   Main Card with Enhanced Styling --%>
                <div class="card panel-card">
                    <%--   Enhanced Card Header --%>
                    <div class="card-header-custom">
                        <i class="fas fa-user-plus"></i>
                        <h5>Personel Kayıt / Güncelleme İşlemleri</h5>
                    </div>

                    <div class="card-body">
                        <%--   Info Badge --%>
                        <div class="info-badge">
                            <i class="fas fa-info-circle"></i>
                            Personel ekleme ve düzenleme işlemlerini bu ekran üzerinden gerçekleştirebilirsiniz. Zorunlu alanlar (<span class="text-danger">*</span>) ile işaretlenmiştir.
                       
                        </div>

                        <asp:Panel ID="pnlYazdir" runat="server" CssClass="print-panel">
                            <%--   Enhanced ValidationSummary --%>
                            <asp:ValidationSummary ID="ValidationSummaryPersonel" runat="server"
                                CssClass="alert alert-danger alert-dismissible fade show"
                                HeaderText="⚠️ Lütfen aşağıdaki alanları düzeltin:"
                                ValidationGroup="PersonelKayit" />

                            <%--   SECTION 1: Kimlik Bilgileri --%>
                            <div class="form-section mb-section">
                                <div class="section-title">
                                    <i class="fas fa-id-card"></i>
                                    <span>Kimlik Bilgileri</span>
                                </div>

                                <div class="row g-3">
                                    <%--   TC Kimlik No with Input Group --%>
                                    <div class="col-md-3">
                                        <label class="form-label">TC Kimlik No <span class="text-danger">*</span></label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-id-badge"></i>
                                            </span>
                                            <asp:TextBox ID="txtTcKimlikNo" runat="server" CssClass="form-control"
                                                MaxLength="11" AutoPostBack="true"
                                                OnTextChanged="txtTcKimlikNo_TextChanged"
                                                placeholder="11 haneli TC No"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTcKimlikNo" runat="server"
                                            ControlToValidate="txtTcKimlikNo"
                                            ErrorMessage="TC Kimlik No zorunludur."
                                            CssClass="text-danger small"
                                            ValidationGroup="PersonelKayit"
                                            Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </div>

                                    <%--   Adı --%>
                                    <div class="col-md-2">
                                        <label class="form-label">Adı <span class="text-danger">*</span></label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-user"></i>
                                            </span>
                                            <asp:TextBox ID="txtAdi" runat="server" CssClass="form-control"
                                                MaxLength="50" placeholder="Adı"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvAdi" runat="server"
                                            ControlToValidate="txtAdi"
                                            ErrorMessage="Adı zorunludur."
                                            CssClass="text-danger small"
                                            ValidationGroup="PersonelKayit"
                                            Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </div>

                                    <%--   Soyadı --%>
                                    <div class="col-md-2">
                                        <label class="form-label">Soyadı <span class="text-danger">*</span></label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-user"></i>
                                            </span>
                                            <asp:TextBox ID="txtSoyad" runat="server" CssClass="form-control"
                                                MaxLength="50" placeholder="Soyadı"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvSoyad" runat="server"
                                            ControlToValidate="txtSoyad"
                                            ErrorMessage="Soyadı zorunludur."
                                            CssClass="text-danger small"
                                            ValidationGroup="PersonelKayit"
                                            Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </div>

                                    <%--   Doğum Yeri --%>
                                    <div class="col-md-2">
                                        <label class="form-label">Doğum Yeri</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-map-marker-alt"></i>
                                            </span>
                                            <asp:TextBox ID="txtDogumYeri" runat="server" CssClass="form-control"
                                                MaxLength="100" placeholder="İl"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--   Doğum Tarihi --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Doğum Tarihi</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-calendar-alt"></i>
                                            </span>
                                            <asp:TextBox ID="txtDogumTarihi" runat="server"
                                                CssClass="form-control fp-date" placeholder="GG/AA/YYYY"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row g-3 mt-2">
                                    <%--   Cinsiyet --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Cinsiyet <span class="text-danger">*</span></label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-venus-mars"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlCinsiyet" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="">-- Seçiniz --</asp:ListItem>
                                                <asp:ListItem Value="Erkek">Erkek</asp:ListItem>
                                                <asp:ListItem Value="Bayan">Bayan</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvCinsiyet" runat="server"
                                            ControlToValidate="ddlCinsiyet"
                                            ErrorMessage="Cinsiyet seçiniz."
                                            CssClass="text-danger small"
                                            ValidationGroup="PersonelKayit"
                                            Display="Dynamic"
                                            InitialValue="">*</asp:RequiredFieldValidator>
                                    </div>

                                    <%--   Çalışma Durumu --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Çalışma Durumu <span class="text-danger">*</span></label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-toggle-on"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlCalismaDurumu" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="Aktif">Aktif</asp:ListItem>
                                                <asp:ListItem Value="Pasif">Pasif</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvCalismaDurumu" runat="server"
                                            ControlToValidate="ddlCalismaDurumu"
                                            ErrorMessage="Çalışma durumu seçiniz."
                                            CssClass="text-danger small"
                                            ValidationGroup="PersonelKayit"
                                            Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </div>

                                    <%--   Bilgileri Getir Button --%>
                                    <div class="col-md-6">
                                        <label class="form-label d-block">&nbsp;</label>
                                        <asp:Button ID="btnBilgileriGetir" runat="server"
                                            Text="🔍 Bilgileri Getir"
                                            CssClass="btn btn-outline-primary"
                                            OnClick="btnBilgileriGetir_Click"
                                            CausesValidation="false" />
                                        <asp:Label ID="lblTcValidation" runat="server" CssClass="text-danger small ms-2"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <%--   SECTION 2: Mesleki Bilgiler --%>
                            <div class="form-section mb-section">
                                <div class="section-title">
                                    <i class="fas fa-briefcase"></i>
                                    <span>Mesleki Bilgiler</span>
                                </div>

                                <div class="row g-3">
                                    <%--   Sicil No --%>
                                    <div class="col-md-2">
                                        <label class="form-label">Sicil No <span class="text-danger">*</span></label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-hashtag"></i>
                                            </span>
                                            <asp:TextBox ID="txtSicilNo" runat="server" CssClass="form-control"
                                                TextMode="Number" AutoPostBack="true" placeholder="Sicil No"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvSicilNo" runat="server"
                                            ControlToValidate="txtSicilNo"
                                            ErrorMessage="Sicil No zorunludur."
                                            CssClass="text-danger small"
                                            ValidationGroup="PersonelKayit"
                                            Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </div>

                                    <%--   Personel Durumu --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Personel Durumu <span class="text-danger">*</span></label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-user-check"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="Kadrolu Aktif Çalışan">Kadrolu Aktif Çalışan</asp:ListItem>
                                                <asp:ListItem Value="Geçici Görevli Aktif Çalışan">Geçici Görevli Aktif Çalışan</asp:ListItem>
                                                <asp:ListItem Value="Geçici Görevde Pasif Çalışan">Geçici Görevde Pasif Çalışan</asp:ListItem>
                                                <asp:ListItem Value="Firma Personeli">Firma Personeli</asp:ListItem>
                                                <asp:ListItem Value="İşkur İşçi (TYP)">İşkur İşçi (TYP)</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvDurum" runat="server"
                                            ControlToValidate="ddlDurum"
                                            ErrorMessage="Personel durumu seçiniz."
                                            CssClass="text-danger small"
                                            ValidationGroup="PersonelKayit"
                                            Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </div>

                                    <%--   Ünvan --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Ünvan <span class="text-danger">*</span></label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-user-tie"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlUnvan" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">-- Seçiniz --</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvUnvan" runat="server"
                                            ControlToValidate="ddlUnvan"
                                            ErrorMessage="Ünvan seçiniz."
                                            CssClass="text-danger small"
                                            ValidationGroup="PersonelKayit"
                                            Display="Dynamic"
                                            InitialValue="">*</asp:RequiredFieldValidator>
                                    </div>

                                    <%--   Statü --%>
                                    <div class="col-md-2">
                                        <label class="form-label">Statü</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-tag"></i>
                                            </span>
                                            <asp:TextBox ID="txtStatu" runat="server" CssClass="form-control"
                                                MaxLength="50" placeholder="Statü"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--   İlk İşe Giriş Tarihi --%>
                                    <div class="col-md-2">
                                        <label class="form-label">İlk İşe Giriş Tarihi</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-calendar-check"></i>
                                            </span>
                                            <asp:TextBox ID="txtIlkIseGirisTarihi" runat="server"
                                                CssClass="form-control fp-date" placeholder="GG/AA/YYYY"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row g-3 mt-2">
                                    <%--   Kuruma Başlama Tarihi --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Kuruma Başlama Tarihi</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-calendar-alt"></i>
                                            </span>
                                            <asp:TextBox ID="txtKurumaBaslamaTarihi" runat="server"
                                                CssClass="form-control fp-date" placeholder="GG/AA/YYYY"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--   Görev Yaptığı Birim --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Görev Yaptığı Birim <span class="text-danger">*</span></label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-building"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlGorevYaptigiBirim" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">-- Seçiniz --</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvGorevYaptigiBirim" runat="server"
                                            ControlToValidate="ddlGorevYaptigiBirim"
                                            ErrorMessage="Görev birimi seçiniz."
                                            CssClass="text-danger small"
                                            ValidationGroup="PersonelKayit"
                                            Display="Dynamic"
                                            InitialValue="">*</asp:RequiredFieldValidator>
                                    </div>

                                    <%--   Kadro Derece --%>
                                    <div class="col-md-2">
                                        <label class="form-label">Kadro Derece</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-layer-group"></i>
                                            </span>
                                            <asp:TextBox ID="txtKadroDerece" runat="server" CssClass="form-control"
                                                MaxLength="20" placeholder="Derece"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--   Öğrenim Durumu --%>
                                    <div class="col-md-2">
                                        <label class="form-label">Öğrenim Durumu</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-graduation-cap"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlOgrenimDurumu" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="">-- Seçiniz --</asp:ListItem>
                                                <asp:ListItem Value="İlkokul">İlkokul</asp:ListItem>
                                                <asp:ListItem Value="Ortaokul">Ortaokul</asp:ListItem>
                                                <asp:ListItem Value="Lise">Lise</asp:ListItem>
                                                <asp:ListItem Value="Üniversite">Üniversite</asp:ListItem>
                                                <asp:ListItem Value="Lisansüstü">Lisansüstü</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <%--   Mesleki Unvan --%>
                                    <div class="col-md-2">
                                        <label class="form-label">Mesleki Unvan</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-certificate"></i>
                                            </span>
                                            <asp:TextBox ID="txtMeslekiUnvan" runat="server" CssClass="form-control"
                                                MaxLength="100" placeholder="Mesleki unvan"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row g-3 mt-2">
                                    <%--   İşten Ayrılış Tarihi --%>
                                    <div class="col-md-3">
                                        <label class="form-label">İşten Ayrılış Tarihi</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-sign-out-alt"></i>
                                            </span>
                                            <asp:TextBox ID="txtIstenAyrilisTarihi" runat="server"
                                                CssClass="form-control fp-date" placeholder="GG/AA/YYYY"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--   İşten Ayrılma Sebebi --%>
                                    <div class="col-md-3">
                                        <label class="form-label">İşten Ayrılma Sebebi</label>
                                        <div class="input-group">
                                            <span class="input-group-text align-items-start pt-2">
                                                <i class="fas fa-comment-dots"></i>
                                            </span>
                                            <asp:TextBox ID="txtIstenAyrilmaSebebi" runat="server" CssClass="form-control"
                                                MaxLength="255" TextMode="MultiLine" Rows="2"
                                                placeholder="Ayrılma sebebi"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--   Geçici Görev Başlangıç --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Geçici Görev Başlangıç</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-calendar-plus"></i>
                                            </span>
                                            <asp:TextBox ID="txtGGorevBaslangic" runat="server"
                                                CssClass="form-control fp-date" placeholder="GG/AA/YYYY"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--   Geçici Görev Bitiş --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Geçici Görev Bitiş</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-calendar-minus"></i>
                                            </span>
                                            <asp:TextBox ID="txtGGorevBitis" runat="server"
                                                CssClass="form-control fp-date" placeholder="GG/AA/YYYY"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row g-3 mt-2">
                                    <%--   Geçici Gelen Kurum --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Geçici Gelen Kurum</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-arrow-right"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlGeciciGelenKurum" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">-- Seçiniz --</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <%--   Geçici Giden Kurum --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Geçici Giden Kurum</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-arrow-left"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlGeciciGidenKurum" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">-- Seçiniz --</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <%--   Devreden İzin --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Devreden İzin (Gün)</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-calendar-week"></i>
                                            </span>
                                            <asp:TextBox ID="txtDevredenIzin" runat="server" CssClass="form-control"
                                                TextMode="Number" placeholder="0"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--   Cari Yıl İzni --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Cari Yıl İzni (Gün)</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-calendar"></i>
                                            </span>
                                            <asp:TextBox ID="txtCariIzin" runat="server" CssClass="form-control"
                                                TextMode="Number" placeholder="0"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%--   SECTION 3: İletişim Bilgileri --%>
                            <div class="form-section mb-section">
                                <div class="section-title">
                                    <i class="fas fa-phone-alt"></i>
                                    <span>İletişim Bilgileri</span>
                                </div>

                                <div class="row g-3">
                                    <%--   Cep Telefonu --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Cep Telefonu</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-mobile-alt"></i>
                                            </span>
                                            <asp:TextBox ID="txtCepTelefonu" runat="server" CssClass="form-control"
                                                MaxLength="15" placeholder="5xxxxxxxxx"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--   Mail Adresi --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Mail Adresi</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-envelope"></i>
                                            </span>
                                            <asp:TextBox ID="txtMailAdresi" runat="server" CssClass="form-control"
                                                TextMode="Email" MaxLength="100" placeholder="ornek@mail.com"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--   Ev Telefonu --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Ev Telefonu</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-phone"></i>
                                            </span>
                                            <asp:TextBox ID="txtEvTelefonu" runat="server" CssClass="form-control"
                                                MaxLength="15" placeholder="0xxxxxxxxxx"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--   Dahili Telefon --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Dahili Telefon</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-phone-square"></i>
                                            </span>
                                            <asp:TextBox ID="txtDahiliTelefon" runat="server" CssClass="form-control"
                                                MaxLength="10" placeholder="Dahili"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row g-3 mt-2">
                                    <%--   Adres --%>
                                    <div class="col-md-6">
                                        <label class="form-label">Adres</label>
                                        <div class="input-group">
                                            <span class="input-group-text align-items-start pt-2">
                                                <i class="fas fa-map-marker-alt"></i>
                                            </span>
                                            <asp:TextBox ID="txtAdres" runat="server" CssClass="form-control"
                                                MaxLength="500" TextMode="MultiLine" Rows="3"
                                                placeholder="Adres bilgisi"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--   Acil Durumda Aranacak Kişi --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Acil Durumda Aranacak Kişi</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-user-shield"></i>
                                            </span>
                                            <asp:TextBox ID="txtAcilDurumKisi" runat="server" CssClass="form-control"
                                                MaxLength="100" placeholder="Ad Soyad"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--   Acil Cep Telefonu --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Acil Cep Telefonu</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-phone-volume"></i>
                                            </span>
                                            <asp:TextBox ID="txtAcilCep" runat="server" CssClass="form-control"
                                                MaxLength="15" placeholder="5xxxxxxxxx"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%--   SECTION 4: Diğer Bilgiler --%>
                            <div class="form-section mb-section">
                                <div class="section-title">
                                    <i class="fas fa-info-circle"></i>
                                    <span>Diğer Bilgiler</span>
                                </div>

                                <div class="row g-3">
                                    <%--   Kan Grubu --%>
                                    <div class="col-md-2">
                                        <label class="form-label">Kan Grubu</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-tint"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlKanGrubu" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="">-- Seçiniz --</asp:ListItem>
                                                <asp:ListItem Value="A(+)">A(+)</asp:ListItem>
                                                <asp:ListItem Value="A(-)">A(-)</asp:ListItem>
                                                <asp:ListItem Value="B(+)">B(+)</asp:ListItem>
                                                <asp:ListItem Value="B(-)">B(-)</asp:ListItem>
                                                <asp:ListItem Value="AB(+)">AB(+)</asp:ListItem>
                                                <asp:ListItem Value="AB(-)">AB(-)</asp:ListItem>
                                                <asp:ListItem Value="0(+)">0(+)</asp:ListItem>
                                                <asp:ListItem Value="0(-)">0(-)</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <%--   Medeni Hali --%>
                                    <div class="col-md-2">
                                        <label class="form-label">Medeni Hali</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-ring"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlMedeniHali" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="">-- Seçiniz --</asp:ListItem>
                                                <asp:ListItem Value="Evli">Evli</asp:ListItem>
                                                <asp:ListItem Value="Bekar">Bekar</asp:ListItem>
                                                <asp:ListItem Value="Boşanmış">Boşanmış</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <%--   Askerlik Durumu --%>
                                    <div class="col-md-2">
                                        <label class="form-label">Askerlik Durumu</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-flag"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlAskerlikDurumu" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="">-- Seçiniz --</asp:ListItem>
                                                <asp:ListItem Value="Bitirilmiş">Bitirilmiş</asp:ListItem>
                                                <asp:ListItem Value="Tecilli">Tecilli</asp:ListItem>
                                                <asp:ListItem Value="Muaf">Muaf</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <%--   Engel Durumu --%>
                                    <div class="col-md-2">
                                        <label class="form-label">Engel Durumu</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-wheelchair"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlEngelDurumu" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="Yok">Yok</asp:ListItem>
                                                <asp:ListItem Value="Var">Var</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <%--   Bağlı Olduğu Sendika --%>
                                    <div class="col-md-2">
                                        <label class="form-label">Bağlı Olduğu Sendika</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-users"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlSendika" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">-- Seçiniz --</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <%--   Emeklilik Hak Ediş Tarihi --%>
                                    <div class="col-md-2">
                                        <label class="form-label">Emeklilik Hak Ediş Tarihi</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-calendar-check"></i>
                                            </span>
                                            <asp:TextBox ID="txtEmeklilikTarihi" runat="server"
                                                CssClass="form-control fp-date" placeholder="GG/AA/YYYY"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row g-3 mt-2">
                                    <%--   Yaşlılık Aylığı Tarihi --%>
                                    <div class="col-md-3">
                                        <label class="form-label">Yaşlılık Aylığı Tarihi</label>
                                        <div class="input-group">
                                            <span class="input-group-text">
                                                <i class="fas fa-calendar-alt"></i>
                                            </span>
                                            <asp:TextBox ID="txtYaslilikAyligiTarihi" runat="server"
                                                CssClass="form-control fp-date" placeholder="GG/AA/YYYY"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--   Engel Açıklama --%>
                                    <div class="col-md-4">
                                        <label class="form-label">Engel Açıklama</label>
                                        <div class="input-group">
                                            <span class="input-group-text align-items-start pt-2">
                                                <i class="fas fa-comment"></i>
                                            </span>
                                            <asp:TextBox ID="txtEngelAciklama" runat="server" CssClass="form-control"
                                                MaxLength="255" TextMode="MultiLine" Rows="2"
                                                placeholder="Engel durumu açıklaması"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--   Emeklilik Açıklama --%>
                                    <div class="col-md-5">
                                        <label class="form-label">Emeklilik Açıklama</label>
                                        <div class="input-group">
                                            <span class="input-group-text align-items-start pt-2">
                                                <i class="fas fa-comment-alt"></i>
                                            </span>
                                            <asp:TextBox ID="txtEmeklilikAciklama" runat="server" CssClass="form-control"
                                                MaxLength="500" TextMode="MultiLine" Rows="3"
                                                placeholder="Emeklilik durumu açıklaması"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%--   Action Buttons Section --%>
                            <div class="action-buttons">
                                <div class="d-flex flex-wrap gap-2 justify-content-between align-items-center">
                                    <div class="info-badge mb-0">
                                        <i class="fas fa-info-circle"></i>
                                        Zorunlu alanlar (<span class="text-danger">*</span>) ile işaretlenmiştir
                                   
                                    </div>
                                    <div class="d-flex flex-wrap gap-2">
                                        <asp:Button ID="btnEkle" runat="server"
                                            Text="💾 Kaydet"
                                            CssClass="btn btn-primary"
                                            OnClick="btnEkle_Click"
                                            ValidationGroup="PersonelKayit" />
                                        <asp:Button ID="btnGuncelle" runat="server"
                                            Text="✏️ Güncelle"
                                            CssClass="btn btn-success"
                                            OnClick="btnGuncelle_Click"
                                            ValidationGroup="PersonelKayit" />
                                        <asp:Button ID="btnVazgec" runat="server"
                                            Text="↩️ Vazgeç"
                                            CssClass="btn btn-secondary"
                                            OnClick="btnVazgec_Click"
                                            CausesValidation="false" />
                                        <asp:Button ID="btnYazdir" runat="server"
                                            Text="🖨️ Yazdır"
                                            CssClass="btn btn-info"
                                            OnClientClick="return YazdirmaPaneli();"
                                            CausesValidation="false" />
                                    </div>
                                </div>

                                <%--   Message Label --%>
                                <asp:Label ID="lblMesaj" runat="server" CssClass="alert mt-3 d-block" Visible="false"></asp:Label>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--   Additional Scripts for Enhanced UX --%>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            // Auto-hide success alerts after 5 seconds
            const alerts = document.querySelectorAll('.alert-success');
            alerts.forEach(alert => {
                setTimeout(() => {
                    alert.style.display = 'none';
                }, 5000);
            });

            // TC Kimlik No validation (11 digits)
            const tcNo = document.getElementById('<%= txtTcKimlikNo.ClientID %>');
            if (tcNo) {
                tcNo.addEventListener('input', function (e) {
                    this.value = this.value.replace(/[^0-9]/g, '');
                });
            }

            // Phone number validation (only digits)
            const phoneFields = [
                '<%= txtCepTelefonu.ClientID %>',
                '<%= txtEvTelefonu.ClientID %>',
                '<%= txtAcilCep.ClientID %>'
            ];

            phoneFields.forEach(fieldId => {
                const field = document.getElementById(fieldId);
                if (field) {
                    field.addEventListener('input', function (e) {
                        this.value = this.value.replace(/[^0-9]/g, '');
                    });
                }
            });
        });
    </script>
</asp:Content>
