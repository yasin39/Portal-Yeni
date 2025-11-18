<%@ Page Title="Taşıt Denetim Raporu" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="TasitRapor.aspx.cs" Inherits="Portal.ModulDenetim.TasitRapor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css" rel="stylesheet" />
    <style>
        .form-card {
            background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
            border-radius: 15px;
            padding: 25px;
            margin-bottom: 20px;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
        }

        .card-header-custom {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            color: white;
            padding: 15px 20px;
            border-radius: 10px 10px 0 0;
            margin: -25px -25px 20px -25px;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .form-label {
            font-weight: 600;
            color: #2E5B9A;
            margin-bottom: 8px;
        }

        .form-control, .form-select {
            border: 2px solid #e1e8ed;
            border-radius: 8px;
            padding: 10px 15px;
            transition: all 0.3s;
        }

            .form-control:focus, .form-select:focus {
                border-color: #4B7BEC;
                box-shadow: 0 0 0 0.2rem rgba(75, 123, 236, 0.25);
            }

        .btn-custom {
            padding: 10px 20px;
            border-radius: 8px;
            font-weight: 600;
            transition: all 0.3s;
            border: none;
        }

        .btn-primary-custom {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            color: white;
        }

            .btn-primary-custom:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 12px rgba(75, 123, 236, 0.4);
            }

        .btn-success-custom {
            background: linear-gradient(135deg, #1ecb97 0%, #0d8f6b 100%);
            color: white;
        }

        .btn-warning-custom {
            background: linear-gradient(135deg, #f7b731 0%, #d89d0d 100%);
            color: white;
        }

        .btn-danger-custom {
            background: linear-gradient(135deg, #ee5253 0%, #d63031 100%);
            color: white;
        }

        .btn-secondary-custom {
            background: linear-gradient(135deg, #95a5a6 0%, #7f8c8d 100%);
            color: white;
        }

        .search-card {
            background: white;
            border-radius: 15px;
            padding: 20px;
            margin-bottom: 20px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.08);
        }

        .grid-container {
            background: white;
            border-radius: 15px;
            padding: 20px;
            margin-top: 20px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.08);
        }

        .validation-message {
            color: #ee5253;
            font-size: 0.875rem;
            margin-top: 5px;
        }

        .info-badge {
            background: #e3f2fd;
            color: #1976d2;
            padding: 8px 15px;
            border-radius: 6px;
            font-size: 0.9rem;
            display: inline-block;
            margin-top: 10px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">

        <!-- Denetim Bilgileri Formu -->
        <div class="form-card">
            <div class="card-header-custom">
                <i class="fas fa-clipboard-check fa-2x"></i>
                <div>
                    <h4 class="mb-0">Taşıt Denetim Raporlama</h4>                    
                </div>
            </div>

            <div class="row g-3">
                <!-- Plaka Bilgileri -->
                <div class="col-md-6">
                    <label class="form-label">Plaka No </label>
                    <asp:TextBox ID="Plaka" runat="server" CssClass="form-control text-uppercase" placeholder="Örn: 06UAB1989" MaxLength="20"></asp:TextBox>
                </div>

                <div class="col-md-6">
                    <label class="form-label">Taşıt/Yetki Belgesi Unvan </label>
                    <asp:TextBox ID="Unvan" runat="server" CssClass="form-control" placeholder="Firma unvanı" MaxLength="250"></asp:TextBox>
                </div>

                <!-- Unvan -->
                <div class="col-md-12">
                </div>


                <!-- Yetki Belgesi -->
                <div class="col-md-6">
                    <label class="form-label">Yetki Belgesi</label>
                    <asp:DropDownList ID="YetkiBelgesi" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>

                <!-- Denetim Türü -->
                <div class="col-md-6">
                    <label class="form-label">Denetim Türü </label>
                    <asp:DropDownList ID="DenetimTuru" runat="server" CssClass="form-select">
                        <asp:ListItem Value="">Seçiniz</asp:ListItem>
                        <asp:ListItem Value="Eşya Taşımacılığı">Eşya Taşımacılığı</asp:ListItem>
                        <asp:ListItem Value="Yolcu Taşımacılığı">Yolcu Taşımacılığı</asp:ListItem>
                        <asp:ListItem Value="Tehlikeli Madde">Tehlikeli Madde</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <!-- İl -->
                <div class="col-md-6">
                    <label class="form-label">Denetim İl </label>
                    <asp:DropDownList ID="Il" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="Il_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <!-- İlçe -->
                <div class="col-md-6">
                    <label class="form-label">Denetim İlçe </label>
                    <asp:DropDownList ID="Ilce" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>

                <!-- Denetleyen Personel -->
                <div class="col-md-6">
                    <label class="form-label">Denetleyen Personel </label>
                    <asp:DropDownList ID="Personel" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>

                <!-- Ceza Durumu -->
                <div class="col-md-6">
                    <label class="form-label">Ceza Durumu </label>
                    <asp:DropDownList ID="CezaDurumu" runat="server" CssClass="form-select">
                        <asp:ListItem Value="">Seçiniz</asp:ListItem>
                        <asp:ListItem Value="Para Cezası">Para Cezası</asp:ListItem>
                        <asp:ListItem Value="Para + Uyarı Cezası">Para + Uyarı Cezası</asp:ListItem>
                        <asp:ListItem Value="Uyarı Cezası">Uyarı Cezası</asp:ListItem>
                        <asp:ListItem Value="Yok">Yok</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <!-- Validation Summary -->
                <div class="col-md-12">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger"
                        ValidationGroup="kayit" DisplayMode="BulletList" HeaderText="Lütfen aşağıdaki alanları kontrol ediniz:" />
                </div>

                <!-- Butonlar -->
                <div class="col-md-12">
                    <div class="d-flex gap-2 flex-wrap">
                        <asp:Button ID="AraBtn" runat="server" Text="🔍 Bul" CssClass="btn btn-custom btn-success-custom"
                            ValidationGroup="kayit" OnClick="AraBtn_Click" />

                        <asp:Button ID="PdfIndir" runat="server" Text="📄 PDF İndir" CssClass="btn btn-custom btn-primary-custom"
                            OnClick="PdfIndir_Click" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>

        <!-- GridView Rapor Kartı -->
        <div class="grid-container">
            <div class="d-flex justify-content-between align-items-center mb-3">
                <h5><i class="fas fa-table text-primary"></i>Denetim Kayıtları</h5>
                <asp:Button ID="ExcelBtn" runat="server" Text="📊 Excel" CssClass="btn btn-custom btn-success-custom btn-sm"
                    OnClick="ExcelBtn_Click" CausesValidation="false" />
            </div>

            <asp:GridView ID="DenetimGrid" runat="server" CssClass="table table-striped table-hover"
                AutoGenerateColumns="false" AllowPaging="true" PageSize="20"
                OnPageIndexChanging="DenetimGrid_PageIndexChanging"
                EmptyDataText="Kayıt bulunamadı">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="No" />
                    <asp:BoundField DataField="Plaka" HeaderText="Plaka" />
                    <asp:BoundField DataField="Plaka2" HeaderText="Yarı Römork" />
                    <asp:BoundField DataField="Unvan" HeaderText="Unvan" />
                    <asp:BoundField DataField="DenetimYeri" HeaderText="Denetim Yeri" />
                    <asp:BoundField DataField="YetkiBelgesi" HeaderText="Yetki Belgesi" />
                    <asp:BoundField DataField="DenetimTuru" HeaderText="Denetim Türü" />
                    <asp:BoundField DataField="DenetimTarihi" HeaderText="Denetim Tarihi" DataFormatString="{0:dd.MM.yyyy}" />
                    <asp:BoundField DataField="il" HeaderText="İl" />
                    <asp:BoundField DataField="ilce" HeaderText="İlçe" />
                    <asp:BoundField DataField="Personel1" HeaderText="Personel" />
                    <asp:BoundField DataField="CezaDurumu" HeaderText="Ceza Durumu" />
                </Columns>
                <PagerStyle CssClass="pagination" HorizontalAlign="Center" />
            </asp:GridView>
        </div>

    </div>

    <!-- Flatpickr JS -->
    <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
    <script src="https://npmcdn.com/flatpickr/dist/l10n/tr.js"></script>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            flatpickr('.flatpickr-datetime', {
                enableTime: true,
                dateFormat: "d.m.Y H:i",
                time_24hr: true,
                locale: "tr"
            });
        });
    </script>
</asp:Content>
