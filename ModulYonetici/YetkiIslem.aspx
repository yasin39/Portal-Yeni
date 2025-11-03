<%@ Page Title="Kullanıcı Yetki İşlemleri" Language="C#" MasterPageFile="~/AnaV2.Master" 
    AutoEventWireup="true" CodeBehind="YetkiIslem.aspx.cs" 
    Inherits="Portal.ModulYonetici.YetkiIslem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .yetki-card {
            background: linear-gradient(135deg, #ffffff 0%, #f8f9fa 100%);
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
            border: 1px solid #e9ecef;
        }

        .yetki-card-header {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            color: white;
            padding: 1rem 1.5rem;
            border-radius: 12px 12px 0 0;
            font-weight: 600;
            font-size: 1.1rem;
        }

        .form-label-custom {
            font-weight: 600;
            color: #2C3E50;
            margin-bottom: 0.5rem;
            font-size: 0.95rem;
        }

        .grid-card {
            background: white;
            border-radius: 12px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
            overflow: hidden;
        }

        .grid-header {
            background: linear-gradient(135deg, #2E5B9A 0%, #4B7BEC 100%);
            color: white;
            padding: 1rem 1.5rem;
            font-weight: 600;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .badge-count {
            background: rgba(255, 255, 255, 0.2);
            padding: 0.3rem 0.8rem;
            border-radius: 20px;
            font-size: 0.85rem;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid py-4">
        
        <!-- Başlık Bölümü -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <h2 class="mb-1" style="color: #2E5B9A; font-weight: 700;">
                            <i class="fas fa-user-shield me-2"></i>Kullanıcı Yetki Yönetimi
                        </h2>
                        <p class="text-muted mb-0">Kullanıcılara yetki ekleme ve mevcut yetkileri yönetme</p>
                    </div>
                </div>
            </div>
        </div>

        <!-- Yetki Ekleme Formu -->
        <div class="row mb-4">
            <div class="col-lg-8 col-xl-6 mx-auto">
                <div class="yetki-card">
                    <div class="yetki-card-header">
                        <i class="fas fa-plus-circle me-2"></i>Yeni Yetki Ekle
                    </div>
                    <div class="card-body p-4">
                        <div class="row g-3">
                            <!-- Kullanıcı Seçimi -->
                            <div class="col-md-6">
                                <label class="form-label-custom">
                                    <i class="fas fa-user me-2" style="color: #4B7BEC;"></i>Kullanıcı
                                </label>
                                <asp:DropDownList ID="ddlKullanici" runat="server" 
                                    CssClass="form-select form-select-lg" 
                                    AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlKullanici_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>

                            <!-- Sicil No -->
                            <div class="col-md-6">
                                <label class="form-label-custom">
                                    <i class="fas fa-id-card me-2" style="color: #4B7BEC;"></i>Sicil No
                                </label>
                                <asp:TextBox ID="txtSicilNo" runat="server" 
                                    CssClass="form-control form-control-lg" 
                                    ReadOnly="true"
                                    placeholder="Kullanıcı seçilmedi">
                                </asp:TextBox>
                            </div>

                            <!-- Yetki Seçimi -->
                            <div class="col-12">
                                <label class="form-label-custom">
                                    <i class="fas fa-key me-2" style="color: #4B7BEC;"></i>Yetki
                                </label>
                                <asp:DropDownList ID="ddlYetki" runat="server" 
                                    CssClass="form-select form-select-lg">
                                </asp:DropDownList>
                            </div>

                            <!-- Ekle Butonu -->
                            <div class="col-12 text-center mt-4">
                                <asp:Button ID="btnYetkiEkle" runat="server" 
                                    CssClass="btn btn-primary btn-lg px-5" 
                                    Text="✓ Yetki Ekle"
                                    OnClick="btnYetkiEkle_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Mevcut Yetkiler GridView -->
        <div class="row">
            <div class="col-12">
                <div class="grid-card">
                    <div class="grid-header">
                        <div>
                            <i class="fas fa-list-ul me-2"></i>Mevcut Yetkiler
                            <asp:Label ID="lblKayitSayisi" runat="server" 
                                CssClass="badge-count ms-2" Text="0 kayıt"></asp:Label>
                        </div>
                        <div>
                            <asp:Button ID="btnTopluSil" runat="server" 
                                CssClass="btn btn-danger btn-sm" 
                                Text="🗑️ Seçilenleri Sil"
                                OnClick="btnTopluSil_Click"
                                OnClientClick="return confirmTopluSilme();" />
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="YetkilerGrid" runat="server" 
                            CssClass="table table-hover table-striped mb-0"
                            AutoGenerateColumns="False"
                            DataKeyNames="id"
                            OnSelectedIndexChanged="YetkilerGrid_SelectedIndexChanged"
                            EmptyDataText="Henüz yetki kaydı bulunmamaktadır."
                            GridLines="None">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-CssClass="text-center">
                                    <HeaderTemplate>
                                        <input type="checkbox" id="chkTumunuSec" onclick="toggleAllCheckboxes(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSec" runat="server" CssClass="form-check-input" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="id" HeaderText="ID" 
                                    ItemStyle-CssClass="fw-bold" 
                                    HeaderStyle-CssClass="bg-light" />

                                <asp:BoundField DataField="Sicil_No" HeaderText="Sicil No" 
                                    HeaderStyle-CssClass="bg-light" />

                                <asp:BoundField DataField="Yetki" HeaderText="Yetki" 
                                    HeaderStyle-CssClass="bg-light" />

                                <asp:BoundField DataField="Yetki_No" HeaderText="Yetki No" 
                                    ItemStyle-CssClass="text-center"
                                    HeaderStyle-CssClass="bg-light text-center" />

                                <asp:TemplateField HeaderText="İşlemler" 
                                    HeaderStyle-CssClass="bg-light text-center"
                                    ItemStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSil" runat="server" 
                                            CommandName="Select"
                                            CssClass="btn btn-danger btn-sm" 
                                            Text="🗑️ Sil"
                                            OnClientClick="return confirm('Bu yetkiyi silmek istediğinizden emin misiniz?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#2E5B9A" ForeColor="White" 
                                Font-Bold="True" CssClass="text-center" />
                            <RowStyle CssClass="align-middle" />
                            <AlternatingRowStyle BackColor="#F8F9FA" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <!-- JavaScript -->
    <script type="text/javascript">
        function toggleAllCheckboxes(source) {
            var checkboxes = document.querySelectorAll('[id*=chkSec]');
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i] !== source) {
                    checkboxes[i].checked = source.checked;
                }
            }
        }

        function confirmTopluSilme() {
            var checkboxes = document.querySelectorAll('[id*=chkSec]:checked');
            var checkedCount = 0;
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].id.indexOf('chkTumunuSec') === -1) {
                    checkedCount++;
                }
            }

            if (checkedCount === 0) {
                alert('Lütfen silmek istediğiniz yetkileri seçiniz.');
                return false;
            }

            if (!confirm('Seçili ' + checkedCount + ' adet yetkiyi silmek istediğinizden emin misiniz?')) {
                return false;
            }

            return confirm('SON ONAY: Bu işlem geri alınamaz! Devam etmek istiyor musunuz?');
        }
    </script>
</asp:Content>
