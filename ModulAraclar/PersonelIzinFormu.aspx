<%@ Page Title="Personel İzin Formu" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="PersonelIzinFormu.aspx.cs" Inherits="Portal.ModulAraclar.PersonelIzinFormu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- Flatpickr CSS (Tarih/Saat seçici için) --%>
   <link href="../wwwroot/css/flatpickr.min.css" rel="stylesheet" />
    <style>
        /* flatpickr takviminin diğer elementlerin altında kalmasını engeller */
        .flatpickr-calendar {
            z-index: 1056 !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item"><a href="/Anasayfa.aspx">Ana Sayfa</a></li>
    <li class="breadcrumb-item"><asp:Literal ID="litAraclar" runat="server">Çeşitli Araçlar</asp:Literal></li>
    <li class="breadcrumb-item active" aria-current="page">Personel İzin Formu</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card shadow-sm-custom">
       <div class="card-header">
            <h5 class="mb-0"><i class="fas fa-file-alt me-2"></i>Personel İzin Formu (Saatlik / Hastane)</h5>
        </div>
       <div class="card-body">
            <div class="row">
                <div class="col-lg-5 col-md-12">
                    <h6 class="text-primary-custom">1. Personel Bilgileri</h6>
                    <div class="mb-3">
                        <label for="<%=sicil.ClientID%>" class="form-label">Sicil No ile Personel Bul</label>
                        <div class="input-group">
                           <asp:TextBox ID="sicil" runat="server" CssClass="form-control" AutoPostBack="True" OnTextChanged="sicil_TextChanged"></asp:TextBox>
                            <button id="bulbuton" runat="server" class="btn btn-primary" onserverclick="sicildengetirbuton_Click">
                                <i class="fas fa-search me-1"></i> Bul
                            </button>
                        </div>
                       <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="sicil" ErrorMessage="Sicil No Boş Olamaz." ForeColor="Red" ValidationGroup="izinKayit" Display="Dynamic" CssClass="d-block mt-1"></asp:RequiredFieldValidator>
                    </div>

                    <asp:Panel ID="pnlPersonelDetay" runat="server" Visible="false">
                        <%-- === DEĞİŞİKLİK 1 (ASPX): Resim alanı kaldırıldı === --%>
                        <div class="text-center mb-3">
                            <%-- <asp:Image ID="Image1" runat="server" CssClass="img-thumbnail" Width="100" Height="130" /> --%>
                            <h6 class="mt-2 mb-0">
                                <asp:Label ID="adisoyadi" runat="server" Text="-"></asp:Label>
                            </h6>
                        </div>
                        <%-- === DEĞİŞİKLİK 1 SONU === --%>
                        
                        <div class="row">
                           <div class="col-md-6 mb-3">
                                <label class="form-label">TC Kimlik No</label>
                                <asp:TextBox ID="tc" runat="server" ReadOnly="True" CssClass="form-control bg-light"></asp:TextBox>
                            </div>
                           <div class="col-md-6 mb-3">
                                <label class="form-label">Ünvanı</label>
                                <asp:TextBox ID="unvan" runat="server" ReadOnly="True" CssClass="form-control bg-light"></asp:TextBox>
                            </div>
                           <div class="col-md-6 mb-3">
                                <label class="form-label">Çalıştığı Birim</label>
                                <asp:TextBox ID="birim" runat="server" ReadOnly="True" CssClass="form-control bg-light"></asp:TextBox>
                            </div>
                           <div class="col-md-6 mb-3">
                                <label class="form-label">Statü</label>
                                <asp:TextBox ID="statu" runat="server" ReadOnly="True" CssClass="form-control bg-light"></asp:TextBox>
                            </div>
                        </div>
                    </asp:Panel>
                </div>

                <div class="col-lg-7 col-md-12">
                    <h6 class="text-primary-custom">2. İzin Detayları</h6>
                    <div class="row">
                       <div class="col-md-12 mb-3">
                            <label for="<%=izinturu.ClientID%>" class="form-label">İzin Türü</label>
                            <asp:DropDownList ID="izinturu" runat="server" CssClass="form-select">
                                <asp:ListItem Value="">Lütfen Seçiniz...</asp:ListItem>
                                <asp:ListItem>Saatlik izin</asp:ListItem>
                               <asp:ListItem>Hastane İzni</asp:ListItem>
                            </asp:DropDownList>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="izinturu" ErrorMessage="İzin Türü Seçiniz." ForeColor="Red" InitialValue="" ValidationGroup="izinKayit" Display="Dynamic" CssClass="d-block mt-1"></asp:RequiredFieldValidator>
                        </div>
                       <div class="col-md-6 mb-3">
                            <label for="<%=iznebaslamatarihi.ClientID%>" class="form-label">İzne Başlama Tarihi ve Saati</label>
                            <asp:TextBox ID="iznebaslamatarihi" runat="server" CssClass="form-control flatpickr-datetime"></asp:TextBox>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="iznebaslamatarihi" ErrorMessage="Başlama Tarihi Seçiniz."
                               ForeColor="Red" ValidationGroup="izinKayit" Display="Dynamic" CssClass="d-block mt-1"></asp:RequiredFieldValidator>
                        </div>
                       <div class="col-md-6 mb-3">
                            <label for="<%=izinbitistarihi.ClientID%>" class="form-label">İzin Bitiş Tarihi ve Saati</label>
                            <asp:TextBox ID="izinbitistarihi" runat="server" CssClass="form-control flatpickr-datetime"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="izinbitistarihi" ErrorMessage="Bitiş Tarihi Seçiniz."
                                ForeColor="Red" ValidationGroup="izinKayit" Display="Dynamic" CssClass="d-block mt-1"></asp:RequiredFieldValidator>
                        </div>
                       <div class="col-md-12 mb-3">
                            <label for="<%=aciklama.ClientID%>" class="form-label">Açıklama / İzin Sebebi</label>
                            <asp:TextBox ID="aciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="aciklama" ErrorMessage="Açıklama zorunludur."
                                ForeColor="Red" ValidationGroup="izinKayit" Display="Dynamic" CssClass="d-block mt-1"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="d-flex justify-content-end gap-2 mt-3">
                       <button ID="eklebuton" runat="server" onServerClick="eklebuton_Click"
                            class="btn btn-primary" validationgroup="izinKayit">
                            <i class="fas fa-check me-1"></i> Kaydet ve PDF Oluştur
                        </button>
                        <button ID="temizlebuton" runat="server" onServerClick="temizlebuton_Click"
                            class="btn btn-secondary" causesvalidation="false">
                            <i class="fas fa-eraser me-1"></i> Formu Temizle
                        </button>
                    </div>
                   <asp:Label ID="Label1" runat="server" ForeColor="Red" CssClass="d-block mt-2"></asp:Label>
                </div>
            </div>

            <asp:Panel ID="pnlGecmisIzinler" runat="server" Visible="false">
                <hr class="my-4" />
                <h6 class="text-primary-custom">Personelin Geçmiş İzinleri</h6>
               <div class="table-responsive">
                    <asp:GridView ID="GridView2" runat="server"
                       CssClass="table table-striped table-hover table-bordered" AutoGenerateColumns="False" BorderStyle="None" GridLines="None">
                        <Columns>
                           <asp:BoundField DataField="izin_turu" HeaderText="İzin Türü" />
                           <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" />
                           <asp:BoundField DataField="izne_Baslama_Tarihi" DataFormatString="{0:dd.MM.yyyy}" HeaderText="Baş. Tarihi" />
                           <asp:BoundField DataField="ibaslamasaat" HeaderText="Baş. Saati" />
                           <asp:BoundField DataField="izin_Bitis_Tarihi" DataFormatString="{0:dd.MM.yyyy}" HeaderText="Bitiş Tarihi" />
                            <asp:BoundField DataField="ibitissaat" HeaderText="Bitiş Saati" />
                           <asp:BoundField DataField="Kayit_Kullanici" HeaderText="Kayıt Eden" />
                            <asp:BoundField DataField="Kayit_Tarihi" HeaderText="Kayıt Tarihi" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                        </Columns>
                       <SelectedRowStyle CssClass="table-active" />
                       <FooterStyle CssClass="table-light" />
                       <PagerStyle CssClass="pagination" />
                       <HeaderStyle CssClass="table-primary" />
                    </asp:GridView>
                </div>
            </asp:Panel>

        </div>
    </div>

    <%-- flatpickr JS (Tarih/Saat seçici için) --%>
   <script src="../wwwroot/js/flatpickr.js"></script>
   <script src="../wwwroot/js/tr.js"></script>
    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            // Türkçe dil ayarını yükle
            flatpickr.localize(flatpickr.l10ns.tr);

            // Sadece Tarih seçimi
            flatpickr(".flatpickr-date", {
                dateFormat: "d.m.Y",
                allowInput: true
            });

            // Tarih ve Saat seçimi
            flatpickr(".flatpickr-datetime", {
                enableTime: true,
                dateFormat: "d.m.Y H:i",
                time_24hr: true,
                allowInput: true
            });
        });
    </script>
</asp:Content>