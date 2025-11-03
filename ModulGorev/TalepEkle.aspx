<%@ Page Title="Görev Talep Sistemi" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" 
    CodeBehind="TalepEkle.aspx.cs" Inherits="Portal.ModulGorev.TalepEkle" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="~/wwwroot/css/BELGETAKIPMODUL.css" rel="stylesheet" />
    <script type="text/javascript">
        function YazdirmaPaneli() {
            var panel = document.getElementById("<%=pnlYazdir.ClientID %>");
            var printWindow = window.open('', '', 'height=600,width=900');
            printWindow.document.write('<html><head><title>Görev Talep Formu</title>');
            printWindow.document.write('<style>body { font-family: Arial; padding: 20px; } table { width: 100%; border-collapse: collapse; } td { padding: 8px; border: 1px solid #ddd; }</style>');
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

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">
        
        <asp:Panel ID="pnlYazdir" runat="server">
            <!-- Form Kartı -->
            <div class="card mb-4">
                <div class="card-header">
                    <h5 class="card-title mb-0">
                        <i class="fas fa-tasks me-2"></i>Görev Talep Formu
                    </h5>
                </div>
                <div class="card-body">
                    <div class="row g-3">
                        <!-- İlgili Şube Müdürlüğü -->
                        <div class="col-md-6">
                            <label class="form-label">İlgili Şube Müdürlüğü</label>
                            <asp:DropDownList ID="ddlSubeMudurlugu" runat="server" CssClass="form-select">
                            </asp:DropDownList>
                        </div>

                        <!-- Görev Gidilecek İl -->
                        <div class="col-md-6">
                            <label class="form-label">Görev Gidilecek İl</label>
                            <asp:DropDownList ID="ddlIl" runat="server" CssClass="form-select">
                                <asp:ListItem Value="">Seçiniz</asp:ListItem>
                                <asp:ListItem>Ankara</asp:ListItem>
                                <asp:ListItem>Konya</asp:ListItem>
                                <asp:ListItem>Eskişehir</asp:ListItem>
                                <asp:ListItem>Kayseri</asp:ListItem>
                                <asp:ListItem>Kırıkkale</asp:ListItem>
                                <asp:ListItem>Kırşehir</asp:ListItem>
                                <asp:ListItem>Nevşehir</asp:ListItem>
                                <asp:ListItem>Aksaray</asp:ListItem>
                                <asp:ListItem>Çankırı</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <!-- İlçe -->
                        <div class="col-md-6">
                            <label class="form-label">İlçe</label>
                            <asp:TextBox ID="txtIlce" runat="server" CssClass="form-control" 
                                placeholder="İlçe adını giriniz"></asp:TextBox>
                        </div>

                        <!-- Gidilmesi Gereken Son Tarih -->
                        <div class="col-md-6">
                            <label class="form-label">Gidilmesi Gereken Son Tarih <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtSonTarih" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvSonTarih" runat="server" 
                                ControlToValidate="txtSonTarih" ErrorMessage="Tarih seçiniz" 
                                CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                        </div>

                        <!-- Görev Türü -->
                        <div class="col-md-6">
                            <label class="form-label">Görev Türü <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlGorevTuru" runat="server" CssClass="form-select">
                                <asp:ListItem Value="">Seçiniz</asp:ListItem>
                                <asp:ListItem>Yetki Belgesi</asp:ListItem>
                                <asp:ListItem>Denetim</asp:ListItem>
                                <asp:ListItem>AFAD</asp:ListItem>
                                <asp:ListItem>UKOME Toplantıları</asp:ListItem>
                                <asp:ListItem>Kurum Görüşmeleri</asp:ListItem>
                                <asp:ListItem>Diğer</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvGorevTuru" runat="server" 
                                ControlToValidate="ddlGorevTuru" InitialValue="" 
                                ErrorMessage="Görev türü seçiniz" 
                                CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                        </div>

                        <!-- Görevlendirilecek Personel Sayısı -->
                        <div class="col-md-6">
                            <label class="form-label">Görevlendirilecek Personel Sayısı</label>
                            <asp:TextBox ID="txtPersonelSayisi" runat="server" CssClass="form-control" 
                                TextMode="Number" placeholder="Personel sayısı"></asp:TextBox>
                        </div>

                        <!-- Tahmini İş Süreci -->
                        <div class="col-md-6">
                            <label class="form-label">Tahmini İş Süresi (Gün)</label>
                            <asp:TextBox ID="txtIsSuresi" runat="server" CssClass="form-control" 
                                TextMode="Number" placeholder="Gün cinsinden"></asp:TextBox>
                        </div>

                        <!-- İvedilik -->
                        <div class="col-md-6">
                            <label class="form-label">İvedilik Durumu</label>
                            <asp:DropDownList ID="ddlIvedilik" runat="server" CssClass="form-select">
                                <asp:ListItem Selected="True">Normal</asp:ListItem>
                                <asp:ListItem>Günlü</asp:ListItem>
                                <asp:ListItem>Çok İvedi</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <!-- Firma/Kurum Unvanı -->
                        <div class="col-md-12">
                            <label class="form-label">Firma/Kurum Unvanı</label>
                            <asp:TextBox ID="txtUnvan" runat="server" CssClass="form-control" 
                                TextMode="MultiLine" Rows="3" placeholder="Firma veya kurum unvanını giriniz"></asp:TextBox>
                        </div>

                        <!-- Adres -->
                        <div class="col-md-12">
                            <label class="form-label">Adres (Açık Adres/Varsa Telefon) <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtAdres" runat="server" CssClass="form-control" 
                                TextMode="MultiLine" Rows="3" placeholder="Detaylı adres bilgisini giriniz"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAdres" runat="server" 
                                ControlToValidate="txtAdres" ErrorMessage="Adres giriniz" 
                                CssClass="text-danger small" Display="Dynamic" ValidationGroup="kayit">*</asp:RequiredFieldValidator>
                        </div>

                        <!-- Açıklama -->
                        <div class="col-md-12">
                            <label class="form-label">Açıklama (Gidilme Nedeni)</label>
                            <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" 
                                TextMode="MultiLine" Rows="3" placeholder="Görev açıklamasını giriniz"></asp:TextBox>
                        </div>
                    </div>

                    <!-- Validation Summary -->
                    <asp:ValidationSummary ID="valSummary" runat="server" 
                        CssClass="alert alert-danger mt-3" DisplayMode="BulletList" 
                        HeaderText="Lütfen aşağıdaki alanları doldurunuz:" 
                        ValidationGroup="kayit" ShowSummary="true" ShowMessageBox="false" />

                    <!-- Action Buttons -->
                    <div class="action-buttons mt-4">
                        <asp:Button ID="btnEkle" runat="server" Text="✅ Talep Ekle" 
                            CssClass="btn btn-primary me-2" OnClick="btnEkle_Click" ValidationGroup="kayit" />
                        <asp:Button ID="btnGuncelle" runat="server" Text="🔄 Güncelle" 
                            CssClass="btn btn-success me-2" OnClick="btnGuncelle_Click" 
                            ValidationGroup="kayit" Visible="false" />
                        <asp:Button ID="btnSil" runat="server" Text="❌ Sil" 
                            CssClass="btn btn-danger me-2" OnClick="btnSil_Click" 
                            OnClientClick="return confirm('Bu talebi silmek istediğinizden emin misiniz?');" 
                            CausesValidation="false" Visible="false" />
                        <asp:Button ID="btnVazgec" runat="server" Text="↩️ Vazgeç" 
                            CssClass="btn btn-outline-secondary me-2" OnClick="btnVazgec_Click" 
                            CausesValidation="false" Visible="false" />
                        <asp:Button ID="btnYazdir" runat="server" Text="🖨️ Yazdır" 
                            CssClass="btn btn-info" OnClientClick="return YazdirmaPaneli();" 
                            CausesValidation="false" />
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!-- Taleplerim Listesi -->
        <div class="card">
            <div class="card-header">
                <h5 class="card-title mb-0">
                    <i class="fas fa-list me-2"></i>Aktif Taleplerim
                    <asp:Label ID="lblKayitSayisi" runat="server" CssClass="badge bg-primary ms-2"></asp:Label>
                </h5>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="TaleplerGrid" runat="server" CssClass="table table-striped table-hover" 
                        AutoGenerateColumns="False" OnSelectedIndexChanged="TaleplerGrid_SelectedIndexChanged"
                        EmptyDataText="Henüz aktif talebiniz bulunmamaktadır.">
                        <Columns>
                            <asp:BoundField DataField="Talep_id" HeaderText="Talep No" />
                            <asp:BoundField DataField="Durum" HeaderText="Durum" />
                            <asp:BoundField DataField="Gorev_Turu" HeaderText="Görev Türü" />
                            <asp:BoundField DataField="Gorev_il" HeaderText="İl" />
                            <asp:BoundField DataField="Gorev_ilce" HeaderText="İlçe" />
                            <asp:BoundField DataField="Gidilecen_Son_Tarih" HeaderText="Son Tarih" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="sube_mudurlugu" HeaderText="Birim" />
                            <asp:BoundField DataField="Personel_Sayisi" HeaderText="Personel Sayısı" />
                            <asp:BoundField DataField="sure" HeaderText="İş Süresi (Gün)" />
                            <asp:BoundField DataField="ivedilik" HeaderText="İvedilik" />
                            <asp:BoundField DataField="Unvan" HeaderText="Firma/Kurum" />
                            <asp:BoundField DataField="Adres" HeaderText="Adres" />
                            <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" />
                            <asp:BoundField DataField="Kayit_Tarihi" HeaderText="Kayıt Tarihi" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:CommandField ShowSelectButton="True" ButtonType="Button" SelectText="📝 Seç" />
                        </Columns>
                        <HeaderStyle CssClass="table-dark" />
                    </asp:GridView>
                </div>

                <!-- Excel Export -->
                <div class="export-panel text-end mt-3">
                    <asp:Button ID="btnExcelAktar" runat="server" Text="📊 Excel'e Aktar" 
                        CssClass="btn btn-success" OnClick="btnExcelAktar_Click" CausesValidation="false" />
                </div>
            </div>
        </div>

    </div>
</asp:Content>