<%@ Page Title="İşletme Denetim Raporlama" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" CodeBehind="IsletmeRapor.aspx.cs" Inherits="Portal.ModulDenetim.IsletmeRapor" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .filter-card-enhanced {
            background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%);
            border-left: 4px solid #4B7BEC;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
            margin-bottom: 1.5rem;
            overflow: hidden;
        }

        .filter-header-gradient {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            color: white;
            padding: 1.2rem 1.5rem;
            font-weight: 600;
            font-size: 1.1rem;
        }

        .result-card-enhanced {
            background: white;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
            overflow: hidden;
        }

        .result-header-gradient {
            background: linear-gradient(135deg, #2E5B9A 0%, #4B7BEC 100%);
            color: white;
            padding: 1.2rem 1.5rem;
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
            gap: 1rem;
        }

        .result-header-title {
            font-size: 1.15rem;
            font-weight: 600;
            display: flex;
            align-items: center;
            gap: 0.5rem;
        }

        .count-badge-modern {
            background: rgba(255, 255, 255, 0.25);
            padding: 0.5rem 1.2rem;
            border-radius: 25px;
            font-weight: 600;
            font-size: 0.95rem;
            backdrop-filter: blur(10px);
        }

        .form-label-modern {
            font-weight: 600;
            color: #2E5B9A;
            font-size: 0.875rem;
            margin-bottom: 0.5rem;
        }

        .button-group-modern {
            display: flex;
            gap: 0.75rem;
            flex-wrap: wrap;
            align-items: center;
        }

        @media (max-width: 768px) {
            .result-header-gradient {
                flex-direction: column;
                text-align: center;
            }

            .button-group-modern {
                width: 100%;
            }

            .button-group-modern .btn {
                flex: 1;
                min-width: 120px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item"><a href="/Anasayfa.aspx">Ana Sayfa</a></li>
    <li class="breadcrumb-item">Denetim Takip</li>
    <li class="breadcrumb-item active" aria-current="page">İşletme Denetim Raporlama</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="filter-card-enhanced">
        <div class="filter-header-gradient">
            <i class="fas fa-filter me-2"></i>Raporlama Filtreleri
        </div>
        <div class="card-body p-4">
            <div class="row g-3">

                <div class="col-md-3">
                    <label for="<%= vergino.ClientID %>" class="form-label-modern">
                        <i class="fas fa-hashtag me-1"></i>Vergi No
                    </label>
                    <asp:TextBox ID="vergino" runat="server" CssClass="form-control" TextMode="Number" placeholder="Vergi numarası giriniz"></asp:TextBox>
                </div>

                <div class="col-md-3">
                    <label for="<%= unvan.ClientID %>" class="form-label-modern">
                        <i class="fas fa-building me-1"></i>Firma Unvanı
                    </label>
                    <asp:TextBox ID="unvan" runat="server" CssClass="form-control" placeholder="Firma unvanı giriniz"></asp:TextBox>
                </div>

                <div class="col-md-3">
                    <label for="<%= yetkibelgesi.ClientID %>" class="form-label-modern">
                        <i class="fas fa-certificate me-1"></i>Belge Türü
                    </label>
                    <asp:DropDownList ID="yetkibelgesi" runat="server" CssClass="form-select" AppendDataBoundItems="True"></asp:DropDownList>
                </div>

                <div class="col-md-3">
                    <label for="<%= denetimturu.ClientID %>" class="form-label-modern">
                        <i class="fas fa-clipboard-check me-1"></i>Denetim Türü
                    </label>
                    <asp:DropDownList ID="denetimturu" runat="server" CssClass="form-select" AppendDataBoundItems="True">
                        <asp:ListItem>Hepsi</asp:ListItem>
                        <asp:ListItem>Eşya Taşımacılığı</asp:ListItem>
                        <asp:ListItem>Yolcu Taşımacılığı</asp:ListItem>
                        <asp:ListItem>Tehlikeli Madde</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-md-3">
                    <label for="<%= il.ClientID %>" class="form-label-modern">
                        <i class="fas fa-map-marker-alt me-1"></i>Denetim İl
                    </label>
                    <asp:DropDownList ID="il" runat="server" CssClass="form-select" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="il_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="col-md-3">
                    <label for="<%= ilce.ClientID %>" class="form-label-modern">
                        <i class="fas fa-map-pin me-1"></i>Denetim İlçe
                    </label>
                    <asp:DropDownList ID="ilce" runat="server" CssClass="form-select" AppendDataBoundItems="True">
                        <asp:ListItem>Hepsi</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-md-3">
                    <label for="<%= personel.ClientID %>" class="form-label-modern">
                        <i class="fas fa-user-tie me-1"></i>Denetleyen Personel
                    </label>
                    <asp:DropDownList ID="personel" runat="server" CssClass="form-select" AppendDataBoundItems="True"></asp:DropDownList>
                </div>

                <div class="col-md-3">
                    <label for="<%= cezadurumu.ClientID %>" class="form-label-modern">
                        <i class="fas fa-gavel me-1"></i>Ceza Durumu
                    </label>
                    <asp:DropDownList ID="cezadurumu" runat="server" CssClass="form-select" AppendDataBoundItems="True">
                        <asp:ListItem>Hepsi</asp:ListItem>
                        <asp:ListItem>Para Cezası</asp:ListItem>
                        <asp:ListItem>Para + Uyarı Cezası</asp:ListItem>
                        <asp:ListItem>Uyarı Cezası</asp:ListItem>
                        <asp:ListItem>Yok</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-md-3">
                    <label for="<%= ilktarih.ClientID %>" class="form-label-modern">
                        <i class="fas fa-calendar-day me-1"></i>Başlangıç Tarihi
                    </label>
                    <asp:TextBox ID="ilktarih" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>

                <div class="col-md-3">
                    <label for="<%= sontarih.ClientID %>" class="form-label-modern">
                        <i class="fas fa-calendar-check me-1"></i>Bitiş Tarihi
                    </label>
                    <asp:TextBox ID="sontarih" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>

                <div class="col-md-6 d-flex align-items-end justify-content-end">
                    <div class="button-group-modern">
                        <asp:Button ID="bulbuton" runat="server" Text="🔍 Filtrele" CssClass="btn btn-search-modern" OnClick="bulbuton_Click" />
                        <asp:Button ID="btnPdfAktar" runat="server" Text="📄 PDF Rapor Al" CssClass="btn btn-pdf-modern" OnClick="btnPdfAktar_Click" />
                        <asp:Button ID="exceleaktar" runat="server" Text="📊 Excel'e Aktar" CssClass="btn btn-excel-modern" OnClick="exceleaktar_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div class="result-card-enhanced">
        <div class="result-header-gradient">
            <div class="result-header-title">
                <i class="fas fa-table"></i>
                <span>Arama Sonuçları</span>
            </div>
            <div class="count-badge-modern">
                <i class="fas fa-database me-2"></i>Toplam Kayıt: <asp:Label ID="lblKayitSayisi" runat="server" Text="0"></asp:Label>
            </div>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="GridView1" runat="server"
                    CssClass="modern-table mb-0"
                    AutoGenerateColumns="False"
                    ShowFooter="True"
                    GridLines="None"
                    OnDataBound="GridView1_DataBound">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="Kayıt No" />
                        <asp:BoundField DataField="VergiNo" HeaderText="Vergi No" />
                        <asp:BoundField DataField="Unvan" HeaderText="Firma Unvanı" />
                        <asp:BoundField DataField="Adres" HeaderText="Adres" />
                        <asp:BoundField DataField="YetkiBelgesi" HeaderText="Belge Türü" />
                        <asp:BoundField DataField="DenetimTuru" HeaderText="Denetim Türü" />
                        <asp:BoundField DataField="DenetimTarihi" HeaderText="Denetim Tarihi" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:BoundField DataField="il" HeaderText="İl" />
                        <asp:BoundField DataField="ilce" HeaderText="İlçe" />
                        <asp:BoundField DataField="Personel1" HeaderText="Personel-1" />
                        <asp:BoundField DataField="Personel2" HeaderText="Personel-2" />
                        <asp:BoundField DataField="CezaDurumu" HeaderText="Ceza Durumu" />
                        <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" />
                        <asp:BoundField DataField="KayitKullanici" HeaderText="Kullanıcı" />
                        <asp:BoundField DataField="KayitTarihi" HeaderText="Kayıt Tarihi" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                        <asp:BoundField DataField="GuncellemeKullanici" HeaderText="Güncelleyen Kullanıcı" />
                        <asp:BoundField DataField="GuncellemeTarihi" HeaderText="Güncelleme Tarihi" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                    </Columns>

                    <HeaderStyle CssClass="grid-header-modern" />

                    <EmptyDataTemplate>
                        <div class="empty-state p-5 text-center">
                            <i class="empty-state-icon fas fa-search fa-3x text-muted mb-3"></i>
                            <h5 class="empty-state-text">Arama Sonucu Bulunamadı</h5>
                            <p class="empty-state-subtext">Lütfen filtreleme kriterlerinizi değiştirerek tekrar deneyin.</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>