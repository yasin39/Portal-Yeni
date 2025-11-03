<%@ Page Title="Kullanıcı İşlemleri" Language="C#" MasterPageFile="~/AnaV2.Master"
    AutoEventWireup="true" CodeBehind="KullaniciIslem.aspx.cs"
    Inherits="ModulYonetici.KullaniciIslem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .kullanici-container {
            background: #ffffff;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            padding: 25px;
            margin-bottom: 20px;
        }

        .form-section {
            background: #f8f9fa;
            border-left: 4px solid #4B7BEC;
            padding: 20px;
            margin-bottom: 20px;
            border-radius: 4px;
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
                padding: 10px;
                vertical-align: middle;
            }

            .kullanici-table tbody tr:hover {
                background-color: #f1f5ff;
                cursor: pointer;
            }

        .action-btn {
            padding: 5px 10px;
            margin: 0 2px;
            font-size: 0.85rem;
        }

        .badge-custom {
            padding: 5px 10px;
            border-radius: 4px;
            font-size: 0.85rem;
        }

        .page-header {
            border-bottom: 3px solid #4B7BEC;
            padding-bottom: 10px;
            margin-bottom: 25px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">

        <div class="page-header">
            <h3 class="mb-0">
                <i class="fas fa-users-cog me-2"></i>Kullanıcı İşlemleri
            </h3>
        </div>

        <!-- Form Bölümü -->
        <div class="kullanici-container">
            <div class="form-section">
                <h5 class="mb-3">
                    <i class="fas fa-user-plus me-2"></i>Kullanıcı Bilgileri
                </h5>

                <div class="row g-3">
                    <div class="col-md-4">
                        <label class="form-label">Sicil No <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtSicilNo" runat="server" CssClass="form-control"
                            placeholder="Sicil numarası" MaxLength="15"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSicilNo" runat="server"
                            ControlToValidate="txtSicilNo" ErrorMessage="Sicil No zorunludur"
                            CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-md-4">
                        <label class="form-label">Adı Soyadı <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtAdiSoyadi" runat="server" CssClass="form-control"
                            placeholder="Ad Soyad" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvAdiSoyadi" runat="server"
                            ControlToValidate="txtAdiSoyadi" ErrorMessage="Ad Soyad zorunludur"
                            CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-md-4">
                        <label class="form-label">Mail Adresi</label>
                        <asp:TextBox ID="txtMailAdresi" runat="server" CssClass="form-control"
                            placeholder="ornek@ankara.gov.tr" TextMode="Email"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revMail" runat="server"
                            ControlToValidate="txtMailAdresi"
                            ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                            ErrorMessage="Geçerli bir mail adresi giriniz"
                            CssClass="text-danger small" Display="Dynamic"></asp:RegularExpressionValidator>
                    </div>

                    <div class="col-md-3">
                        <label class="form-label">Kullanıcı Türü <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlKullaniciTuru" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">Seçiniz</asp:ListItem>
                            <asp:ListItem Value="Admin">Admin</asp:ListItem>
                            <asp:ListItem Value="Personel">Personel</asp:ListItem>
                            <asp:ListItem Value="Misafir">Misafir</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvKullaniciTuru" runat="server"
                            ControlToValidate="ddlKullaniciTuru" InitialValue=""
                            ErrorMessage="Kullanıcı türü seçiniz"
                            CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-md-3">
                        <label class="form-label">Personel Tipi</label>
                        <asp:DropDownList ID="ddlPersonelTipi" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">Seçiniz</asp:ListItem>
                            <asp:ListItem Value="0">Tip 0</asp:ListItem>
                            <asp:ListItem Value="1">Tip 1</asp:ListItem>
                            <asp:ListItem Value="2">Tip 2</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-3">
                        <label class="form-label">Birim</label>
                        <asp:TextBox ID="txtBirim" runat="server" CssClass="form-control"
                            placeholder="Birim adı" MaxLength="100"></asp:TextBox>
                    </div>

                    <div class="col-md-3">
                        <label class="form-label">Durum <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">Seçiniz</asp:ListItem>
                            <asp:ListItem Value="Aktif" Selected="True">Aktif</asp:ListItem>
                            <asp:ListItem Value="Pasif">Pasif</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvDurum" runat="server"
                            ControlToValidate="ddlDurum" InitialValue=""
                            ErrorMessage="Durum seçiniz"
                            CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-md-12" id="divParola" runat="server">
                        <label class="form-label">Parola <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtParola" runat="server" CssClass="form-control"
                            placeholder="Parola giriniz" TextMode="Password" MaxLength="50"></asp:TextBox>
                        <small class="text-muted">
                            <i class="fas fa-info-circle"></i>Yeni kullanıcı için parola zorunludur. Güncellemede boş bırakılırsa değişmez.
                        </small>
                        <asp:RequiredFieldValidator ID="rfvParola" runat="server"
                            ControlToValidate="txtParola" ErrorMessage="Parola zorunludur"
                            CssClass="text-danger small d-block" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="row mt-4">
                    <div class="col-md-12">
                        <asp:Button ID="btnKaydet" runat="server" Text="💾 Kaydet"
                            CssClass="btn btn-primary me-2" OnClick="btnKaydet_Click" />

                        <asp:Button ID="btnGuncelle" runat="server" Text="✏️ Güncelle"
                            CssClass="btn btn-warning me-2" OnClick="btnGuncelle_Click"
                            Visible="false" />

                        <asp:Button ID="btnVazgec" runat="server" Text="🚫 Vazgeç"
                            CssClass="btn btn-secondary" OnClick="btnVazgec_Click"
                            CausesValidation="false" Visible="false" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Liste Bölümü -->
        <div class="kullanici-container">
            <div class="d-flex justify-content-between align-items-center mb-3">
                <h5 class="mb-0">
                    <i class="fas fa-list me-2"></i>Kullanıcı Listesi
                </h5>
                <div>
                    <asp:Button ID="btnYenile" runat="server" Text="🔄 Yenile"
                        CssClass="btn btn-info btn-sm me-2" OnClick="btnYenile_Click"
                        CausesValidation="false" />
                </div>
            </div>

            <div class="table-responsive">
                <asp:Repeater ID="rptKullanicilar" runat="server" OnItemCommand="rptKullanicilar_ItemCommand">
                    <HeaderTemplate>
                        <table class="table table-hover kullanici-table">
                            <thead>
                                <tr>
                                    <th style="width: 100px;">Sicil No</th>
                                    <th>Adı Soyadı</th>
                                    <th style="width: 120px;">Kullanıcı Türü</th>
                                    <th style="width: 100px;">Personel Tipi</th>
                                    <th>Birim</th>
                                    <th>Mail Adresi</th>
                                    <th style="width: 80px;">Durum</th>
                                    <th style="width: 200px;" class="text-center">İşlemler</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("Sicil_No") %></td>
                            <td><strong><%# Eval("Adi_Soyadi") %></strong></td>
                            <td><%# Eval("Kullanici_Turu") %></td>
                            <td class="text-center"><%# Eval("Personel_Tipi") %></td>
                            <td><%# Eval("Birim") %></td>
                            <td>
                                <%# !string.IsNullOrEmpty(Eval("Mail_Adresi")?.ToString()) 
                                    ? "<i class='fas fa-envelope me-1'></i>" + Eval("Mail_Adresi") 
                                    : "<span class='text-muted'>-</span>" %>
                            </td>
                            <td>
                                <span class='badge <%# Eval("Durum").ToString() == "Aktif" ? "bg-success" : "bg-secondary" %> badge-custom'>
                                    <%# Eval("Durum") %>
                                </span>
                            </td>
                            <td class="text-center">
                                <asp:Button ID="btnDuzenle" runat="server"
                                    CommandName="Duzenle" CommandArgument='<%# Eval("Sicil_No") %>'
                                    Text="✏️ Düzenle" CssClass="btn btn-sm btn-primary action-btn"
                                    CausesValidation="false" />

                                <asp:Button ID="btnSifreSifirla" runat="server"
                                    CommandName="SifreSifirla" CommandArgument='<%# Eval("Sicil_No") %>'
                                    Text="🔑 Şifre Sıfırla" CssClass="btn btn-sm btn-warning action-btn"
                                    CausesValidation="false"
                                    OnClientClick="return confirm('Kullanıcı şifresi varsayılan değere sıfırlanacak. Onaylıyor musunuz?');" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>

                <asp:Label ID="lblMesaj" runat="server" CssClass="text-muted fst-italic"
                    Visible="false"></asp:Label>
            </div>
        </div>

    </div>
</asp:Content>
