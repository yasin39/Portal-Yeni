<%@ Page Title="Firma Detay" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" 
    CodeBehind="FirmaDetay.aspx.cs" Inherits="Portal.ModulBelgeTakip.FirmaDetay" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="~/wwwroot/Css/BELGETAKIPMODUL.css" />
    <style>
        /* Detay Sayfası Özel Stiller */
        .section-title {
            color: #2E5B9A;
            font-weight: 600;
            font-size: 1.1rem;
            margin-bottom: 1rem;
            padding-bottom: 0.5rem;
            border-bottom: 2px solid #E5E7EB;
        }

        .info-table {
            margin-bottom: 0;
        }

        .info-table th {
            background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
            color: #2C3E50;
            font-weight: 600;
            width: 40%;
            padding: 12px;
            border: 1px solid #dee2e6;
        }

        .info-table td {
            padding: 12px;
            background: white;
            border: 1px solid #dee2e6;
        }

        .status-success {
            background: linear-gradient(135deg, #d1fae5 0%, #a7f3d0 100%);
            color: #065f46;
            padding: 0.35rem 0.75rem;
            border-radius: 6px;
            font-weight: 600;
            display: inline-block;
            border-left: 3px solid #10b981;
        }

        .status-danger {
            background: linear-gradient(135deg, #fee2e2 0%, #fecaca 100%);
            color: #991b1b;
            padding: 0.35rem 0.75rem;
            border-radius: 6px;
            font-weight: 600;
            display: inline-block;
            border-left: 3px solid #dc2626;
        }

        .action-buttons {
            background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
            padding: 1rem;
            border-radius: 8px;
            margin-top: 1.5rem;
        }
    </style>
</asp:Content>

<asp:Content ID="ContentBreadcrumb" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item">
        <i class="fas fa-file-alt me-1"></i>Belge Takip
    </li>
    <li class="breadcrumb-item">
        <a href="TumFirmalar.aspx">Tüm Firmalar</a>
    </li>
    <li class="breadcrumb-item active">Firma Detay</li>
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid wide-container mt-4">
        
        <!-- Firma Detay Kartı -->
        <div class="card">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h3 class="card-title mb-0">
                    <i class="fas fa-building me-2"></i>Firma Detay Bilgileri
                </h3>
                <asp:Button ID="btnGeriDon" runat="server" 
                    Text="⬅️ Geri Dön" 
                    CssClass="btn btn-outline-secondary btn-sm"
                    OnClick="btnGeriDon_Click" 
                    CausesValidation="false" />
            </div>

            <div class="card-body">
                <!-- Mesaj Paneli -->
                <asp:Panel ID="pnlMesajlar" runat="server" Visible="false" CssClass="mb-3">
                    <asp:Label ID="lblHata" runat="server" CssClass="alert alert-danger d-block"
                        Visible="false" EnableViewState="false"></asp:Label>
                    <asp:Label ID="lblBilgi" runat="server" CssClass="alert alert-info d-block"
                        Visible="false" EnableViewState="false"></asp:Label>
                </asp:Panel>

                <!-- Firma ve Belge Bilgileri -->
                <div class="row mb-4">
                    <!-- Sol Kolon - Firma Bilgileri -->
                    <div class="col-lg-6 mb-3">
                        <h5 class="section-title">
                            <i class="fas fa-info-circle me-2 text-primary-custom"></i>Firma Bilgileri
                        </h5>
                        <div class="table-responsive">
                            <table class="table table-bordered info-table">
                                <tbody>
                                    <tr>
                                        <th>
                                            <i class="fas fa-hashtag me-1 text-primary-custom"></i>Vergi No:
                                        </th>
                                        <td>
                                            <asp:Label ID="lblVergiNo" runat="server" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            <i class="fas fa-building me-1 text-primary-custom"></i>Firma Adı:
                                        </th>
                                        <td>
                                            <asp:Label ID="lblFirmaAdi" runat="server" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            <i class="fas fa-map-marker-alt me-1 text-primary-custom"></i>İl/İlçe:
                                        </th>
                                        <td>
                                            <asp:Label ID="lblIlIlce" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            <i class="fas fa-map-pin me-1 text-primary-custom"></i>Adres:
                                        </th>
                                        <td>
                                            <asp:Label ID="lblAdres" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            <i class="fas fa-industry me-1 text-primary-custom"></i>Faaliyet Türü:
                                        </th>
                                        <td>
                                            <asp:Label ID="lblFirmaTipi" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            <i class="fas fa-tags me-1 text-primary-custom"></i>Kategori:
                                        </th>
                                        <td>
                                            <asp:Label ID="lblKategori" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <!-- Sağ Kolon - Belge Bilgileri -->
                    <div class="col-lg-6 mb-3">
                        <h5 class="section-title">
                            <i class="fas fa-certificate me-2 text-primary-custom"></i>Belge Bilgileri
                        </h5>
                        <div class="table-responsive">
                            <table class="table table-bordered info-table">
                                <tbody>
                                    <tr>
                                        <th>
                                            <i class="fas fa-file-alt me-1 text-primary-custom"></i>Belge Türü:
                                        </th>
                                        <td>
                                            <asp:Label ID="lblBelgeTuru" runat="server" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            <i class="fas fa-check-circle me-1 text-primary-custom"></i>Belge Durumu:
                                        </th>
                                        <td>
                                            <asp:Label ID="lblBelgeDurumu" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            <i class="fas fa-barcode me-1 text-primary-custom"></i>Belge No:
                                        </th>
                                        <td>
                                            <asp:Label ID="lblBelgeNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            <i class="fas fa-calendar-check me-1 text-primary-custom"></i>Belge Alma Tarihi:
                                        </th>
                                        <td>
                                            <asp:Label ID="lblBelgeAlmaTarihi" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            <i class="fas fa-calendar-times me-1 text-primary-custom"></i>Son Ceza Tebliğ Tarihi:
                                        </th>
                                        <td>
                                            <asp:Label ID="lblSonCezaTebligTarihi" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <!-- Denetim Geçmişi -->
                <div class="row">
                    <div class="col-12">
                        <h5 class="section-title">
                            <i class="fas fa-history me-2 text-primary-custom"></i>Denetim Geçmişi
                        </h5>
                        <div class="table-responsive">
                            <asp:GridView ID="gvDenetimGecmisi" runat="server"
                                CssClass="table table-striped table-bordered table-hover"
                                AutoGenerateColumns="false"
                                AllowPaging="true"
                                PageSize="10"
                                OnPageIndexChanging="gvDenetimGecmisi_PageIndexChanging"
                                EmptyDataText="Denetim kaydı bulunamadı."
                                EmptyDataRowStyle-CssClass="alert alert-info">
                                <Columns>
                                    <asp:BoundField DataField="DENETIM_TARIHI" HeaderText="Denetim Tarihi"
                                        DataFormatString="{0:dd.MM.yyyy}" ItemStyle-Width="15%" 
                                        NullDisplayText="-" />
                                    <asp:BoundField DataField="DENETIM_TIPI" HeaderText="Denetim Tipi" 
                                        ItemStyle-Width="20%" />
                                    <asp:BoundField DataField="MAKBUZ_NO" HeaderText="Makbuz No" 
                                        ItemStyle-Width="20%" NullDisplayText="-" />
                                    <asp:BoundField DataField="PERSONEL" HeaderText="Denetim Personeli" 
                                        ItemStyle-Width="25%" NullDisplayText="-" />
                                    <asp:BoundField DataField="TEBLIG_TARIHI" HeaderText="Tebliğ Tarihi"
                                        DataFormatString="{0:dd.MM.yyyy}" ItemStyle-Width="20%" 
                                        NullDisplayText="-" />
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <!-- Aksiyon Butonları -->
                <div class="action-buttons text-end">
                    <asp:Button ID="btnExcelAktar" runat="server" 
                        Text="📊 Excel'e Aktar"
                        OnClick="btnExcelAktar_Click" 
                        CssClass="btn btn-success me-2" 
                        CausesValidation="false" />
                    <asp:Button ID="btnListeyeDon" runat="server" 
                        Text="📋 Listeye Dön"
                        OnClick="btnListeyeDon_Click" 
                        CssClass="btn btn-outline-secondary" 
                        CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
