<%@ Page Title="İzin Ekle" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true"
    CodeBehind="IzinEkle.aspx.cs" Inherits="Portal.ModulPersonel.IzinEkle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .izin-form-container {
            background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
            border-radius: 12px;
            padding: 25px;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
        }

        .personel-info-card {
            background: white;
            border-left: 4px solid #4B7BEC;
            padding: 20px;
            margin-bottom: 25px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
        }

        .izin-hesap-badge {
            display: inline-block;
            padding: 8px 15px;
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            color: white;
            border-radius: 20px;
            font-weight: 600;
            margin-right: 10px;
        }

        .flatpickr-input {
            cursor: pointer;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">
        <!-- Sayfa Başlığı -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="page-header-modern">
                    <h4 class="page-title">
                        <i class="fas fa-calendar-plus text-primary me-2"></i>
                        Personel İzin Ekleme / Düzenleme
                    </h4>
                    <p class="page-subtitle">Personel izin kayıtlarını bu ekrandan ekleyebilir ve düzenleyebilirsiniz</p>
                </div>
            </div>
        </div>

        <div class="izin-form-container">
            <!-- Sicil No Arama -->
            <div class="row filter-row-bg mb-4">
                <div class="col-md-6">
                    <label class="form-label-enhanced">
                        <i class="fas fa-id-card icon-primary"></i>Sicil No
                    </label>
                    <div class="filter-input-group">
                        <asp:TextBox ID="txtSicilNo" runat="server" CssClass="form-control"
                            placeholder="Sicil No Giriniz" AutoPostBack="true"
                            OnTextChanged="txtSicilNo_TextChanged"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSicilNo" runat="server"
                            ControlToValidate="txtSicilNo" ErrorMessage="Sicil No zorunludur"
                            ForeColor="Red" Display="Dynamic" ValidationGroup="kayit">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-md-6 d-flex align-items-end">
                    <asp:Button ID="btnPersonelBul" runat="server"
                        CssClass="btn btn-search-modern"
                        Text="🔍 Personel Bul"
                        OnClick="btnPersonelBul_Click"
                        CausesValidation="false" />
                </div>
            </div>

            <!-- Personel Bilgileri Card -->
            <asp:Panel ID="pnlPersonelBilgi" runat="server" Visible="false">
                <div class="personel-info-card">
                    <div class="row">
                        <div class="col-md-2 text-center">

                            <asp:Label ID="lblPersonelAd" runat="server"
                                CssClass="fw-bold text-primary mt-2 d-block"
                                Text="-"></asp:Label>
                        </div>

                        <div class="col-md-10">
                            <div class="row g-3">
                                <div class="col-md-6">
                                    <label class="form-label-enhanced">
                                        <i class="fas fa-fingerprint icon-primary"></i>TC Kimlik No
                                    </label>
                                    <asp:TextBox ID="txtTcKimlikNo" runat="server"
                                        CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="col-md-6">
                                    <label class="form-label-enhanced">
                                        <i class="fas fa-user-tie icon-primary"></i>Ünvan
                                    </label>
                                    <asp:TextBox ID="txtUnvan" runat="server"
                                        CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="col-md-6">
                                    <label class="form-label-enhanced">
                                        <i class="fas fa-building icon-primary"></i>Çalıştığı Birim
                                    </label>
                                    <asp:TextBox ID="txtBirim" runat="server"
                                        CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="col-md-6">
                                    <label class="form-label-enhanced">
                                        <i class="fas fa-user-check icon-primary"></i>Statü
                                    </label>
                                    <asp:TextBox ID="txtStatu" runat="server"
                                        CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <!-- İzin Bilgileri -->
                            <div class="row mt-3">
                                <div class="col-12">
                                    <span class="izin-hesap-badge">📊 Devreden İzin: 
                                        <asp:Label ID="lblDevredenIzin" runat="server" Text="0"></asp:Label>
                                        Gün
                                    </span>
                                    <span class="izin-hesap-badge">📅 Cari Yıl İzin: 
                                        <asp:Label ID="lblCariIzin" runat="server" Text="0"></asp:Label>
                                        Gün
                                    </span>
                                    <span class="izin-hesap-badge">✅ Toplam İzin: 
                                        <asp:Label ID="lblToplamIzin" runat="server" Text="0"></asp:Label>
                                        Gün
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- İzin Detayları -->
            <asp:Panel ID="pnlIzinDetay" runat="server" Visible="false">
                <div class="row g-3 mb-3">
                    <div class="col-md-6">
                        <label class="form-label-enhanced">
                            <i class="fas fa-clipboard-list icon-primary"></i>İzin Türü
                        </label>
                        <asp:DropDownList ID="ddlIzinTuru" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">-- İzin Türü Seçiniz --</asp:ListItem>
                            <asp:ListItem>Yıllık İzin</asp:ListItem>
                            <asp:ListItem>İdari İzin</asp:ListItem>
                            <asp:ListItem>Rapor</asp:ListItem>
                            <asp:ListItem>Mazeret İzni</asp:ListItem>
                            <asp:ListItem>Fazla Mesai İzni</asp:ListItem>
                            <asp:ListItem>Hafta Tatili İzni</asp:ListItem>
                            <asp:ListItem>Saatlik İzin</asp:ListItem>
                            <asp:ListItem>Hastane İzni</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvIzinTuru" runat="server"
                            ControlToValidate="ddlIzinTuru" ErrorMessage="İzin türü seçiniz"
                            ForeColor="Red" Display="Dynamic" ValidationGroup="kayit">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label-enhanced">
                            <i class="fas fa-calendar-day icon-primary"></i>İzne Başlama Tarihi
                        </label>
                        <asp:TextBox ID="txtIzneBaslamaTarihi" runat="server"
                            CssClass="form-control flatpickr-date fp-date" placeholder="GG/AA/YYYY"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvBaslamaTarihi" runat="server"
                            ControlToValidate="txtIzneBaslamaTarihi"
                            ErrorMessage="Başlama tarihi seçiniz"
                            ForeColor="Red" Display="Dynamic" ValidationGroup="kayit">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label-enhanced">
                            <i class="fas fa-clock icon-primary"></i>İzin Süresi (Gün)
                        </label>
                        <asp:TextBox ID="txtIzinSuresi" runat="server"
                            CssClass="form-control" placeholder="Örn: 5"
                            AutoPostBack="true" OnTextChanged="txtIzinSuresi_TextChanged"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvIzinSuresi" runat="server"
                            ControlToValidate="txtIzinSuresi"
                            ErrorMessage="İzin süresi giriniz"
                            ForeColor="Red" Display="Dynamic" ValidationGroup="kayit">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label-enhanced">
                            <i class="fas fa-calendar-check icon-primary"></i>İzin Bitiş Tarihi
                        </label>
                        <asp:TextBox ID="txtIzinBitisTarihi" runat="server"
                            CssClass="form-control flatpickr-date" ReadOnly="true"></asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label-enhanced">
                            <i class="fas fa-calendar-alt icon-primary"></i>Göreve Başlama Tarihi
                        </label>
                        <asp:TextBox ID="txtGoreveBaslamaTarihi" runat="server"
                            CssClass="form-control flatpickr-date" ReadOnly="true"></asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label-enhanced">
                            <i class="fas fa-comment-alt icon-primary"></i>Açıklama
                        </label>
                        <asp:TextBox ID="txtAciklama" runat="server"
                            CssClass="form-control" placeholder="İzin açıklaması (opsiyonel)"></asp:TextBox>
                    </div>
                </div>

                <!-- Mesaj Alanı -->
                <div class="row mb-3">
                    <div class="col-12">
                        <asp:Label ID="lblMesaj" runat="server"
                            CssClass="alert alert-danger d-block"
                            Visible="false"></asp:Label>
                    </div>
                </div>

                <!-- Butonlar -->
                <div class="action-bar-wrapper">
                    <asp:Button ID="btnKaydet" runat="server"
                        CssClass="btn btn-success btn-lg me-2"
                        Text="✅ Kaydet"
                        OnClick="btnKaydet_Click"
                        ValidationGroup="kayit" />

                    <asp:Button ID="btnGuncelle" runat="server"
                        CssClass="btn btn-primary btn-lg me-2"
                        Text="🔄 Güncelle"
                        OnClick="btnGuncelle_Click"
                        ValidationGroup="kayit"
                        Visible="false" />

                    <asp:Button ID="btnSil" runat="server"
                        CssClass="btn btn-danger btn-lg me-2"
                        Text="🗑️ Sil"
                        OnClick="btnSil_Click"
                        CausesValidation="false"
                        Visible="false"
                        OnClientClick="return confirm('İzin kaydını silmek istediğinize emin misiniz?');" />

                    <asp:Button ID="btnVazgec" runat="server"
                        CssClass="btn btn-secondary btn-lg me-2"
                        Text="❌ Vazgeç"
                        OnClick="btnVazgec_Click"
                        CausesValidation="false"
                        Visible="false" />

                    <asp:Button ID="btnYeniKayit" runat="server"
                        CssClass="btn btn-info btn-lg"
                        Text="➕ Yeni Kayıt"
                        OnClick="btnYeniKayit_Click"
                        CausesValidation="false"
                        Visible="false" />

                    <asp:Button ID="btnTemizle" runat="server"
                        CssClass="btn btn-warning btn-lg"
                        Text="🧹 Temizle"
                        OnClick="btnTemizle_Click"
                        CausesValidation="false" />
                </div>
            </asp:Panel>
        </div>

        <!-- İzin Geçmişi GridView -->
        <asp:Panel ID="pnlIzinGecmis" runat="server" Visible="false" CssClass="mt-4">
            <div class="card shadow-sm">
                <div class="card-header bg-gradient-primary text-white">
                    <h5 class="mb-0">
                        <i class="fas fa-history me-2"></i>İzin Geçmişi
                    </h5>
                </div>
                <div class="card-body">
                    <!-- Excel Export -->
                    <div class="mb-3">
                        <asp:Button ID="btnExcelExport" runat="server"
                            CssClass="btn btn-excel-modern"
                            Text="📊 Excel'e Aktar"
                            OnClick="btnExcelExport_Click"
                            CausesValidation="false" />
                    </div>

                    <asp:GridView ID="gvIzinler" runat="server"
                        CssClass="table table-hover table-bordered table-striped"
                        AutoGenerateColumns="False"
                        OnSelectedIndexChanged="gvIzinler_SelectedIndexChanged"
                        HeaderStyle-CssClass="grid-header-modern"
                        EmptyDataText="Henüz izin kaydı bulunmamaktadır" DataKeyNames="id">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                            <asp:BoundField DataField="Sicil_No" HeaderText="Sicil No" ItemStyle-CssClass="text-primary-bold" />
                            <asp:BoundField DataField="Adi_Soyadi" HeaderText="Adı Soyadı" />
                            <asp:BoundField DataField="Statu" HeaderText="Statü" />
                            <asp:BoundField DataField="Devreden_izin" HeaderText="Devreden" DataFormatString="{0:N1}" />
                            <asp:BoundField DataField="Cari_izin" HeaderText="Cari Yıl" DataFormatString="{0:N1}" />
                            <asp:BoundField DataField="izin_turu" HeaderText="İzin Türü" />
                            <asp:BoundField DataField="izin_Suresi" HeaderText="Süre (Gün)" DataFormatString="{0:N1}" />
                            <asp:BoundField DataField="izne_Baslama_Tarihi" HeaderText="Başlama" DataFormatString="{0:dd.MM.yyyy}" />
                            <asp:BoundField DataField="izin_Bitis_Tarihi" HeaderText="Bitiş" DataFormatString="{0:dd.MM.yyyy}" />
                            <asp:BoundField DataField="Goreve_Baslama_Tarihi" HeaderText="Göreve Dönüş" DataFormatString="{0:dd.MM.yyyy}" />
                            <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" />
                            <asp:BoundField DataField="Kayit_Tarihi" HeaderText="Kayıt Tarihi" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                            <asp:BoundField DataField="Kayit_Kullanici" HeaderText="Kayıt Kullanıcı" />
                            <asp:ButtonField ButtonType="Button" Text="✏️ Seç" CommandName="Select"
                                HeaderText="İşlem" ControlStyle-CssClass="btn btn-sm btn-primary" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
