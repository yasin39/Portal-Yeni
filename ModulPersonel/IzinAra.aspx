<%@ Page Title="Personel İzin Arama" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true"
    CodeBehind="IzinAra.aspx.cs" Inherits="Portal.ModulPersonel.IzinAra" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/BELGETAKIPMODUL.css" rel="stylesheet" />
    <style>
        /* Sadece bu sayfaya özel stiller - Ortak stiller Common-Components.css'e taşındı */

        .personel-card {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            border-radius: 12px;
            padding: 1.5rem;
            color: white;
            box-shadow: 0 4px 12px rgba(75, 123, 236, 0.3);
        }

            .personel-card .personel-image {
                width: 100px;
                height: 100px;
                border-radius: 50%;
                border: 4px solid rgba(255, 255, 255, 0.3);
                object-fit: cover;
            }

            .personel-card .izin-badge {
                background: rgba(255, 255, 255, 0.2);
                padding: 0.5rem 1rem;
                border-radius: 8px;
                font-size: 0.9rem;
                font-weight: 600;
            }

        .section-title {
            color: #2E5B9A;
            font-weight: 700;
            margin-bottom: 1rem;
            display: flex;
            align-items: center;
            gap: 0.5rem;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">

        <!-- Başlık -->
        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h2 class="mb-1">🏖️ Personel İzin Arama</h2>
                <p class="text-muted mb-0">İzin bilgilerini görüntüleyin ve yönetin</p>
            </div>
        </div>

        <!-- Bugünkü İzinliler İstatistik Kartları -->
        <div class="row g-3 mb-4">
            <div class="col-md-3">
                <div class="stat-card total">
                    <i class="fas fa-users stat-icon"></i>
                    <asp:Label ID="lblToplamIzinli" runat="server" CssClass="stat-number" Text="0"></asp:Label>
                    <p class="stat-label mb-0">Toplam İzinli</p>
                </div>
            </div>
            <div class="col-md-3">
                <div class="stat-card yillik">
                    <i class="fas fa-umbrella-beach stat-icon"></i>
                    <asp:Label ID="lblYillikIzin" runat="server" CssClass="stat-number" Text="0"></asp:Label>
                    <p class="stat-label mb-0">Yıllık İzinde</p>
                </div>
            </div>
            <div class="col-md-3">
                <div class="stat-card rapor">
                    <i class="fas fa-notes-medical stat-icon"></i>
                    <asp:Label ID="lblRaporlu" runat="server" CssClass="stat-number" Text="0"></asp:Label>
                    <p class="stat-label mb-0">Raporlu</p>
                </div>
            </div>
            <div class="col-md-3">
                <div class="stat-card diger">
                    <i class="fas fa-clock stat-icon"></i>
                    <asp:Label ID="lblDigerIzin" runat="server" CssClass="stat-number" Text="0"></asp:Label>
                    <p class="stat-label mb-0">Saatlik/Mazeret</p>
                </div>
            </div>
        </div>

        <!-- Bugün İzinde Olanlar -->
        <div class="card mb-4">
            <div class="card-header">
                <h5 class="card-title mb-0">
                    <i class="fas fa-calendar-day me-2"></i>Bugün İzinde Olanlar
                    <span class="badge bg-primary ms-2">
                        <asp:Label ID="lblBugunSayisi" runat="server" Text="0"></asp:Label>
                    </span>
                </h5>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="BugunIzinlilerGrid" runat="server" CssClass="table modern-table"
                        AutoGenerateColumns="False" ShowFooter="True" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="Sicil_No" HeaderText="Sicil No" />
                            <asp:BoundField DataField="Adi_Soyadi" HeaderText="Ad Soyad" />
                            <asp:BoundField DataField="GorevYaptigiBirim" HeaderText="Birim" />
                            <asp:BoundField DataField="Statu" HeaderText="Statü" />
                            <asp:BoundField DataField="izin_turu" HeaderText="İzin Türü" />
                            <asp:BoundField DataField="izin_Suresi" HeaderText="Süre (Gün)" />
                            <asp:BoundField DataField="izne_Baslama_Tarihi" HeaderText="Başlangıç" DataFormatString="{0:dd.MM.yyyy}" />
                            <asp:BoundField DataField="izin_Bitis_Tarihi" HeaderText="Bitiş" DataFormatString="{0:dd.MM.yyyy}" />
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="text-center py-4 text-muted">
                                <i class="fas fa-info-circle fa-2x mb-2"></i>
                                <p>Bugün izinde olan personel bulunmamaktadır.</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Personel Detaylı Arama -->
        <div class="row">
            <div class="col-md-4 mb-4">
                <!-- Personel Bilgi Kartı -->
                <div class="personel-card" id="pnlPersonelBilgi" runat="server" visible="false">
                    <div class="text-center mb-3">
                        <asp:Image ID="imgPersonel" runat="server" CssClass="personel-image" />
                    </div>
                    <div class="text-center mb-3">
                        <h5 class="mb-1">
                            <asp:Label ID="lblAdSoyad" runat="server" Text=""></asp:Label>
                        </h5>
                        <p class="mb-1">
                            <small>Sicil No: <asp:Label ID="lblSicilNo" runat="server" Text=""></asp:Label></small>
                        </p>
                        <p class="mb-0">
                            <small>
                                <asp:Label ID="lblStatu" runat="server" Text=""></asp:Label>
                            </small>
                        </p>
                    </div>
                    <div class="row g-2">
                        <div class="col-6">
                            <div class="izin-badge text-center">
                                <div style="font-size: 1.2rem; font-weight: 700;">
                                    <asp:Label ID="lblToplamYillik" runat="server" Text="0"></asp:Label>
                                </div>
                                <small>Yıllık İzin</small>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="izin-badge text-center">
                                <div style="font-size: 1.2rem; font-weight: 700;">
                                    <asp:Label ID="lblToplamRapor" runat="server" Text="0"></asp:Label>
                                </div>
                                <small>Rapor</small>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="izin-badge text-center">
                                <div style="font-size: 1.2rem; font-weight: 700;">
                                    <asp:Label ID="lblToplamSaatlik" runat="server" Text="0"></asp:Label>
                                </div>
                                <small>Saatlik</small>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="izin-badge text-center">
                                <div style="font-size: 1.2rem; font-weight: 700;">
                                    <asp:Label ID="lblToplamMazeret" runat="server" Text="0"></asp:Label>
                                </div>
                                <small>Mazeret</small>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Arama Kartı -->
                <div class="card">
                    <div class="card-header">
                        <h5 class="card-title mb-0">
                            <i class="fas fa-search me-2"></i>Detaylı Arama
                        </h5>
                    </div>
                    <div class="card-body">
                        <div class="mb-3">
                            <label class="form-label">Sicil No</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtSicilNo" runat="server" CssClass="form-control" 
                                    placeholder="Sicil numarası giriniz" AutoPostBack="true"
                                    OnTextChanged="txtSicilNo_TextChanged"></asp:TextBox>
                                <asp:Button ID="btnSicilAra" runat="server" CssClass="btn btn-primary"
                                    Text="🔍" OnClick="btnSicilAra_Click" />
                            </div>
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Ad Soyad</label>
                            <asp:TextBox ID="txtAdSoyad" runat="server" CssClass="form-control"
                                placeholder="Ad veya soyad giriniz"></asp:TextBox>
                        </div>

                        <div class="mb-3">
                            <label class="form-label">İzin Türü</label>
                            <asp:DropDownList ID="ddlIzinTuru" runat="server" CssClass="form-select">
                                <asp:ListItem Value="">Hepsi</asp:ListItem>
                                <asp:ListItem Value="Yıllık İzin">Yıllık İzin</asp:ListItem>
                                <asp:ListItem Value="Rapor">Rapor</asp:ListItem>
                                <asp:ListItem Value="Saatlik izin">Saatlik İzin</asp:ListItem>
                                <asp:ListItem Value="Mazeret İzni">Mazeret İzni</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Başlangıç Tarihi</label>
                            <asp:TextBox ID="txtBaslangicTarihi" runat="server" CssClass="form-control"
                                TextMode="Date"></asp:TextBox>
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Bitiş Tarihi</label>
                            <asp:TextBox ID="txtBitisTarihi" runat="server" CssClass="form-control"
                                TextMode="Date"></asp:TextBox>
                        </div>

                        <div class="d-grid gap-2">
                            <asp:Button ID="btnAra" runat="server" CssClass="btn btn-primary"
                                Text="🔍 Ara" OnClick="btnAra_Click" />
                            <asp:Button ID="btnTemizle" runat="server" CssClass="btn btn-outline-secondary"
                                Text="🔄 Temizle" OnClick="btnTemizle_Click" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <!-- Arama Sonuçları -->
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h5 class="card-title mb-0">
                            <i class="fas fa-list me-2"></i>Arama Sonuçları
                            <span class="badge bg-primary ms-2">
                                <asp:Label ID="lblAramaSayisi" runat="server" Text="0"></asp:Label>
                            </span>
                        </h5>
                        <asp:Button ID="btnExcelAktar" runat="server" CssClass="btn btn-success btn-sm"
                            Text="📊 Excel'e Aktar" OnClick="btnExcelAktar_Click" />
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="AramaSonuclariGrid" runat="server" CssClass="table modern-table"
                                AutoGenerateColumns="False" GridLines="None" AllowPaging="True" PageSize="10"
                                OnPageIndexChanging="AramaSonuclariGrid_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="Sicil_No" HeaderText="Sicil No" />
                                    <asp:BoundField DataField="Adi_Soyadi" HeaderText="Ad Soyad" />
                                    <asp:BoundField DataField="Statu" HeaderText="Statü" />
                                    <asp:BoundField DataField="izin_turu" HeaderText="İzin Türü" />
                                    <asp:BoundField DataField="izin_Suresi" HeaderText="Süre (Gün)" />
                                    <asp:BoundField DataField="izne_Baslama_Tarihi" HeaderText="Başlangıç" DataFormatString="{0:dd.MM.yyyy}" />
                                    <asp:BoundField DataField="izin_Bitis_Tarihi" HeaderText="Bitiş" DataFormatString="{0:dd.MM.yyyy}" />
                                    <asp:BoundField DataField="Kayit_Tarihi" HeaderText="Kayıt Tarihi" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                                </Columns>
                                <PagerStyle CssClass="pagination" HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <div class="text-center py-4 text-muted">
                                        <i class="fas fa-search fa-2x mb-2"></i>
                                        <p>Arama kriteri giriniz veya sonuç bulunamadı.</p>
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>