<%@ Page Title="CİMER Başvuru Kaydı" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="Kayit.aspx.cs" Inherits="Portal.ModulCimer.Kayit" EnableEventValidation="false" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <style>
        /*==> Sayfa özel stiller - minimal tutuldu */
        .search-section {
            background: linear-gradient(135deg, #e0f2fe 0%, #bae6fd 100%);
            padding: 1.5rem;
            border-radius: 10px;
            margin-bottom: 1.5rem;
            border-left: 4px solid #0891b2;
            box-shadow: 0 2px 8px rgba(8, 145, 178, 0.12);
        }

        .file-upload-area {
            border: 2px dashed #d1d5db;
            padding: 2rem;
            text-align: center;
            border-radius: 8px;
            background: linear-gradient(135deg, #f8f9fa 0%, #ffffff 100%);
            transition: all 0.3s ease;
        }

        .file-upload-area:hover {
            border-color: #4B7BEC;
            background: linear-gradient(135deg, #dbeafe 0%, #ffffff 100%);
        }

        .file-upload-icon {
            font-size: 3rem;
            color: #4B7BEC;
            margin-bottom: 1rem;
        }

        /*==> EKLEME: Section aralıkları */
        .mb-section {
            margin-bottom: 2rem;
        }
    </style>
</asp:Content>

<%--==> EKLEME: Breadcrumb'a ikon eklendi --%>
<asp:Content ID="ContentBreadcrumb" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item">
        <i class="fas fa-home me-1"></i>
        <a href="/Anasayfa.aspx">Ana Sayfa</a>
    </li>
    <li class="breadcrumb-item">
        <i class="fas fa-comments me-1"></i>CİMER İşlemleri
    </li>
    <li class="breadcrumb-item active" aria-current="page">Başvuru Kayıt</li>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-12">
                <div class="card panel-card">
                    <div class="panel-header">
                        <div>
                            <i class="fas fa-edit"></i>
                            <span>CİMER Başvuru Ekle / Güncelle</span>
                        </div>
                    </div>

                    <div class="card-body">
                        <div class="info-badge">
                            <i class="fas fa-info-circle"></i>
                            Bu ekrandan yeni CİMER başvurusu ekleyebilir veya mevcut başvuruları güncelleyebilirsiniz. Zorunlu alanlar (<span class="text-danger">*</span>) ile işaretlenmiştir.
                        </div>

                        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-info mt-3">
                        </asp:Panel>

                        <asp:ValidationSummary ID="vsSummary" runat="server" 
                            CssClass="alert alert-danger alert-dismissible fade show mt-3" 
                            HeaderText="⚠️ Lütfen aşağıdaki alanları düzeltin:" 
                            ValidationGroup="kayit" />

                        <%--==> BÖLÜM 1: BAŞVURU ARAMA --%>
                        <div class="search-section mb-section">
                            <div class="row align-items-end g-3">
                                <div class="col-md-8">
                                    <label class="form-label fw-bold">
                                        <i class="fas fa-search me-2"></i>Başvuru Numarası ile Ara
                                    </label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-hashtag"></i>
                                        </span>
                                        <asp:TextBox ID="txtApplicationNumber" runat="server" 
                                            CssClass="form-control" 
                                            placeholder="Başvuru numarasını girin" 
                                            TextMode="Number"></asp:TextBox>
                                        <asp:Button ID="btnSearch" runat="server" 
                                            Text="🔍 Bul" 
                                            CssClass="btn btn-info" 
                                            OnClick="btnSearch_Click" 
                                            CausesValidation="false" />
                                    </div>
                                    <small class="text-muted">
                                        <i class="fas fa-lightbulb me-1"></i>
                                        Mevcut bir başvuruyu güncellemek için başvuru numarasını girin ve "Bul" butonuna tıklayın.
                                    </small>
                                    <asp:HiddenField ID="hfId" runat="server" />
                                </div>
                            </div>
                        </div>

                        <%--==> BÖLÜM 2: BAŞVURU BİLGİLERİ --%>
                        <div class="form-section mb-section">
                            <div class="section-title">
                                <i class="fas fa-file-alt"></i>
                                <span>Başvuru Bilgileri</span>
                            </div>

                            <div class="row g-3">
                                <%--==> Başvuru Tarihi --%>
                                <div class="col-md-12">
                                    <label class="form-label">Başvuru Tarihi <span class="text-danger">*</span></label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-calendar-alt"></i>
                                        </span>
                                        <asp:TextBox ID="txtApplicationDate" runat="server" 
                                            CssClass="form-control fp-date" 
                                            placeholder="Başvuru Tarihi" 
                                            TextMode="Date"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvApplicationDate" runat="server" 
                                        ControlToValidate="txtApplicationDate" 
                                        ErrorMessage="Başvuru Tarihi zorunludur." 
                                        CssClass="text-danger small" 
                                        Display="Dynamic" 
                                        ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>

                        <%--==> BÖLÜM 3: BAŞVURUCU BİLGİLERİ --%>
                        <div class="form-section mb-section">
                            <div class="section-title">
                                <i class="fas fa-user"></i>
                                <span>Başvurucu Bilgileri</span>
                            </div>

                            <div class="row g-3">                               
                                <div class="col-md-6">
                                    <label class="form-label">TC Kimlik No</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-id-card"></i>
                                        </span>
                                        <asp:TextBox ID="txtTcNumber" runat="server" 
                                            CssClass="form-control" 
                                            placeholder="TC Kimlik No" 
                                            TextMode="Number" 
                                            MaxLength="11"></asp:TextBox>
                                    </div>
                                </div>                                
                                <div class="col-md-6">
                                    <label class="form-label">Adı Soyadı <span class="text-danger">*</span></label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-user-circle"></i>
                                        </span>
                                        <asp:TextBox ID="txtFullName" runat="server" 
                                            CssClass="form-control" 
                                            placeholder="Ad ve Soyad"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" 
                                        ControlToValidate="txtFullName" 
                                        ErrorMessage="Adı Soyadı zorunludur." 
                                        CssClass="text-danger small" 
                                        Display="Dynamic" 
                                        ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                                </div>

                                <%--==> E-posta --%>
                                <div class="col-md-6">
                                    <label class="form-label">E-posta Adresi</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-envelope"></i>
                                        </span>
                                        <asp:TextBox ID="txtEmail" runat="server" 
                                            CssClass="form-control" 
                                            placeholder="ornek@email.com" 
                                            TextMode="Email"></asp:TextBox>
                                    </div>
                                </div>

                                <%--==> Telefon --%>
                                <div class="col-md-6">
                                    <label class="form-label">Telefon Numarası</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-phone"></i>
                                        </span>
                                        <asp:TextBox ID="txtPhone" runat="server" 
                                            CssClass="form-control" 
                                            placeholder="0555 555 5555"></asp:TextBox>
                                    </div>
                                </div>

                                <%--==> Adres --%>
                                <div class="col-12">
                                    <label class="form-label">Adres</label>
                                    <div class="input-group">
                                        <span class="input-group-text align-items-start pt-2">
                                            <i class="fas fa-map-marker-alt"></i>
                                        </span>
                                        <asp:TextBox ID="txtAddress" runat="server" 
                                            CssClass="form-control" 
                                            placeholder="Adres bilgisi" 
                                            TextMode="MultiLine" 
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%--==> BÖLÜM 4: BAŞVURU DETAYLARI --%>
                        <div class="form-section mb-section">
                            <div class="section-title">
                                <i class="fas fa-clipboard-list"></i>
                                <span>Başvuru Detayları</span>
                            </div>

                            <div class="row g-3">
                                <%--==> Şikayet Türü --%>
                                <div class="col-md-6">
                                    <label class="form-label">Şikayet Türü <span class="text-danger">*</span></label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-tags"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlComplaintType" runat="server" 
                                            CssClass="form-select">
                                        </asp:DropDownList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvComplaintType" runat="server" 
                                        ControlToValidate="ddlComplaintType" 
                                        ErrorMessage="Şikayet Türü seçiniz." 
                                        CssClass="text-danger small" 
                                        Display="Dynamic" 
                                        ValidationGroup="kayit" 
                                        InitialValue="">*</asp:RequiredFieldValidator>
                                </div>

                                <%--==> Şikayete Konu Firma --%>
                                <div class="col-md-6">
                                    <label class="form-label">Şikayete Konu Firma <span class="text-danger">*</span></label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-building"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlCompany" runat="server" 
                                            CssClass="form-select" 
                                            AppendDataBoundItems="true">
                                            <asp:ListItem Value="" Text="Seçiniz..."></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvCompany" runat="server" 
                                        ControlToValidate="ddlCompany" 
                                        ErrorMessage="Firma seçiniz." 
                                        CssClass="text-danger small" 
                                        Display="Dynamic" 
                                        ValidationGroup="kayit" 
                                        InitialValue="">*</asp:RequiredFieldValidator>
                                </div>

                                <%--==> Sevk Edilecek Kullanıcı --%>
                                <div class="col-md-12">
                                    <label class="form-label">Sevk Edilecek Kullanıcı <span class="text-danger">*</span></label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-user-tie"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlAssignedUser" runat="server" 
                                            CssClass="form-select" 
                                            AppendDataBoundItems="true">
                                            <asp:ListItem Value="" Text="Seçiniz..."></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvAssignedUser" runat="server" 
                                        ControlToValidate="ddlAssignedUser" 
                                        ErrorMessage="Kullanıcı seçiniz." 
                                        CssClass="text-danger small" 
                                        Display="Dynamic" 
                                        ValidationGroup="kayit" 
                                        InitialValue="">*</asp:RequiredFieldValidator>
                                </div>

                                <%--==> Başvuru Metni --%>
                                <div class="col-12">
                                    <label class="form-label">Başvuru Metni <span class="text-danger">*</span></label>
                                    <div class="input-group">
                                        <span class="input-group-text align-items-start pt-2">
                                            <i class="fas fa-comment-dots"></i>
                                        </span>
                                        <asp:TextBox ID="txtApplicationText" runat="server" 
                                            CssClass="form-control" 
                                            placeholder="Başvuru içeriğini buraya yazınız..." 
                                            TextMode="MultiLine" 
                                            Rows="5"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvApplicationText" runat="server" 
                                        ControlToValidate="txtApplicationText" 
                                        ErrorMessage="Başvuru Metni zorunludur." 
                                        CssClass="text-danger small" 
                                        Display="Dynamic" 
                                        ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>

                        <%--==> BÖLÜM 5: DOSYA YÜKLEME --%>
                        <div class="form-section mb-section">
                            <div class="section-title">
                                <i class="fas fa-paperclip"></i>
                                <span>Başvuru Eki</span>
                            </div>

                            <div class="file-upload-area">
                                <div class="file-upload-icon">
                                    <i class="fas fa-cloud-upload-alt"></i>
                                </div>
                                <asp:FileUpload ID="fuAttachment" runat="server" CssClass="form-control" />
                                <small class="text-muted d-block mt-3">
                                    <i class="fas fa-info-circle me-1"></i>
                                    Desteklenen formatlar: <strong>PDF, DOC, DOCX, JPG, JPEG, PNG, ZIP</strong>
                                    <br />
                                    Maksimum dosya boyutu: <strong>10 MB</strong>
                                </small>
                                <asp:HiddenField ID="hfAttachmentPath" runat="server" />
                            </div>
                        </div>

                        <%--==> BÖLÜM 6: İŞLEM DURUMU --%>
                        <div class="form-section mb-section">
                            <div class="section-title">
                                <i class="fas fa-tasks"></i>
                                <span>İşlem Durumu</span>
                            </div>

                            <div class="row g-3">
                                <%--==> Başvuru Durumu --%>
                                <div class="col-md-6">
                                    <label class="form-label">Başvuru Durumu</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-info-circle"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                                            <asp:ListItem Value="Yeni Kayıt Açıldı">Yeni Kayıt Açıldı</asp:ListItem>
                                            <asp:ListItem Value="İnceleniyor">İnceleniyor</asp:ListItem>
                                            <asp:ListItem Value="Sonuçlandı">Sonuçlandı</asp:ListItem>
                                            <asp:ListItem Value="Cevap Verildi">Cevap Verildi</asp:ListItem>
                                            <asp:ListItem Value="Süreç Devam Ediyor">Süreç Devam Ediyor</asp:ListItem>
                                            <asp:ListItem Value="Tekrar Cevap Verildi">Tekrar Cevap Verildi</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <%--==> Onay Durumu --%>
                                <div class="col-md-6">
                                    <label class="form-label">Onay Durumu</label>
                                    <div class="input-group">
                                        <span class="input-group-text">
                                            <i class="fas fa-check-circle"></i>
                                        </span>
                                        <asp:DropDownList ID="ddlApprovalStatus" runat="server" CssClass="form-select">
                                            <asp:ListItem Value="0">Havale</asp:ListItem>
                                            <asp:ListItem Value="1">Onay Bekliyor</asp:ListItem>
                                            <asp:ListItem Value="2">Bitti</asp:ListItem>
                                            <asp:ListItem Value="3">Kapatıldı</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%--==> BÖLÜM 7: İŞLEM BUTONLARI --%>
                        <%--==> EKLEME: action-buttons sınıfı kullanıldı (CIMERMODUL.css) --%>
                        <div class="action-buttons">
                            <div class="d-flex flex-wrap gap-2 justify-content-end align-items-center">
                                <%--==> DEĞİŞİKLİK: Button'lara emoji eklendi (Text property) --%>
                                <asp:Button ID="btnSave" runat="server" 
                                    Text="💾 Ekle" 
                                    CssClass="btn btn-primary" 
                                    OnClick="btnSave_Click" 
                                    ValidationGroup="kayit" />
                                
                                <asp:Button ID="btnUpdate" runat="server" 
                                    Text="✏️ Güncelle" 
                                    CssClass="btn btn-success" 
                                    OnClick="btnUpdate_Click" 
                                    ValidationGroup="kayit" 
                                    Visible="false" />
                                
                                <asp:Button ID="btnCancel" runat="server" 
                                    Text="↩️ Vazgeç" 
                                    CssClass="btn btn-secondary" 
                                    OnClick="btnCancel_Click" 
                                    Visible="false" 
                                    CausesValidation="false" />
                            </div>

                            <%--==> EKLEME: Privacy notice --%>
                            <div class="privacy-notice mt-3">
                                <i class="fas fa-shield-alt"></i>
                                <strong>Gizlilik Notu:</strong> Girilen tüm bilgiler KVKK kapsamında güvenli bir şekilde saklanmaktadır.
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--==> EKLEME: JavaScript iyileştirmeleri --%>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            // TC No validation (11 digits only)
            const tcNo = document.getElementById('<%= txtTcNumber.ClientID %>');
            if (tcNo) {
                tcNo.addEventListener('input', function (e) {
                    this.value = this.value.replace(/[^0-9]/g, '').substring(0, 11);
                });
            }

            // Phone number formatting
            const phone = document.getElementById('<%= txtPhone.ClientID %>');
            if (phone) {
                phone.addEventListener('input', function (e) {
                    this.value = this.value.replace(/[^0-9]/g, '');
                });
            }

            // Auto-hide success messages after 5 seconds
            const alerts = document.querySelectorAll('.alert-success');
            alerts.forEach(alert => {
                setTimeout(() => {
                    alert.style.display = 'none';
                }, 5000);
            });

            // File upload preview
            const fileUpload = document.getElementById('<%= fuAttachment.ClientID %>');
            if (fileUpload) {
                fileUpload.addEventListener('change', function (e) {
                    const fileName = this.files[0]?.name || '';
                    if (fileName) {
                        console.log('Seçilen dosya: ' + fileName);
                    }
                });
            }
        });
    </script>
</asp:Content>
