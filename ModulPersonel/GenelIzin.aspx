<%@ Page Title="Genel İzin Raporu" Language="C#" MasterPageFile="~/AnaV2.Master" 
    AutoEventWireup="true" CodeBehind="GenelIzin.aspx.cs" 
    Inherits="Portal.ModulPersonel.GenelIzin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="~/wwwroot/css/PERSONELMODUL.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        
        <!-- Breadcrumb -->
        <nav aria-label="breadcrumb" class="mb-3">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="../Anasayfa.aspx"><i class="fas fa-home me-1"></i>Ana Sayfa</a></li>
                <li class="breadcrumb-item"><i class="fas fa-users me-1"></i>Personel</li>
                <li class="breadcrumb-item active" aria-current="page">Genel İzin Raporu</li>
            </ol>
        </nav>

        <!-- Başlık ve Özet Bilgi -->
        <div class="card shadow-custom-md mb-4">
            <div class="card-header">
                <div class="row align-items-center">
                    <div class="col-md-8">
                        <h5 class="mb-0">
                            <i class="fas fa-calendar-alt me-2"></i>
                            Personel Genel İzin Raporu
                        </h5>
                    </div>
                    <div class="col-md-4 text-end">
                        <asp:Label ID="LblKayitSayisi" runat="server" CssClass="badge bg-primary fs-6" Text="0 Kayıt"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="card-body">
                <div class="alert alert-info mb-0">
                    <i class="fas fa-info-circle me-2"></i>
                    Bu rapor, personellerin yıllık izin kullanım durumlarını ve kalan izin haklarını gösterir.
                </div>
            </div>
        </div>

        <!-- Filtre Kartı -->
        <div class="card shadow-custom-md mb-4">
            <div class="card-header">
                <h6 class="mb-0"><i class="fas fa-filter me-2"></i>Filtreleme ve Arama</h6>
            </div>
            <div class="card-body">
                <div class="row g-3">
                    
                    <!-- Yıl Seçimi -->
                    <div class="col-md-2">
                        <label class="form-label">
                            <i class="fas fa-calendar me-1"></i>Yıl
                        </label>
                        <asp:DropDownList ID="DdlYil" runat="server" CssClass="form-select" 
                            AutoPostBack="true" OnSelectedIndexChanged="DdlYil_SelectedIndexChanged">
                            <asp:ListItem Value="2023">2023</asp:ListItem>
                            <asp:ListItem Value="2024">2024</asp:ListItem>
                            <asp:ListItem Value="2025" Selected="True">2025</asp:ListItem>
                            <asp:ListItem Value="2026">2026</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <!-- İzin Türü -->
                    <div class="col-md-3">
                        <label class="form-label">
                            <i class="fas fa-list me-1"></i>İzin Türü
                        </label>
                        <asp:DropDownList ID="DdlIzinTuru" runat="server" CssClass="form-select">
                            <asp:ListItem Value="" Selected="True">Tüm İzin Türleri</asp:ListItem>
                            <asp:ListItem Value="Yıllık İzin">Yıllık İzin</asp:ListItem>
                            <asp:ListItem Value="Rapor">Rapor</asp:ListItem>
                            <asp:ListItem Value="Saatlik izin">Saatlik İzin</asp:ListItem>
                            <asp:ListItem Value="Mazeret İzni">Mazeret İzni</asp:ListItem>
                            <asp:ListItem Value="Hastane İzni">Hastane İzni</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <!-- Personel Arama -->
                    <div class="col-md-4">
                        <label class="form-label">
                            <i class="fas fa-search me-1"></i>Personel Ara (Ad, Soyad veya Sicil No)
                        </label>
                        <asp:TextBox ID="TxtArama" runat="server" CssClass="form-control" 
                            placeholder="Personel adı, soyadı veya sicil no girin..."></asp:TextBox>
                    </div>

                    <!-- Butonlar -->
                    <div class="col-md-3">
                        <label class="form-label d-block">&nbsp;</label>
                        <asp:Button ID="BtnAra" runat="server" CssClass="btn btn-primary me-2" 
                            OnClick="BtnAra_Click" Text="🔍 Ara" />
                        <asp:Button ID="BtnTumunuListele" runat="server" CssClass="btn btn-secondary" 
                            OnClick="BtnTumunuListele_Click" CausesValidation="false" Text="📜 Tümünü Listele" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Veri Tablosu -->
        <div class="card shadow-custom-md">
            <div class="card-header">
                <div class="row align-items-center">
                    <div class="col-md-8">
                        <h6 class="mb-0"><i class="fas fa-table me-2"></i>İzin Kullanım Raporu</h6>
                    </div>
                    <div class="col-md-4 text-end">
                        <asp:Button ID="BtnExcelAktar" runat="server" CssClass="btn btn-success btn-sm" 
                            OnClick="BtnExcelAktar_Click" Text="📊 Excel'e Aktar" />
                    </div>
                </div>
            </div>
            <div class="card-body p-0">
                <div class="table-responsive">
                    <asp:GridView ID="PersonelIzinGrid" runat="server" CssClass="table table-hover table-striped mb-0" 
                        AutoGenerateColumns="False" GridLines="None" EmptyDataText="Kayıt bulunamadı.">
                        <Columns>
                            
                           
                            

                            
                            <asp:BoundField DataField="SicilNo" HeaderText="Sicil No" 
                                ItemStyle-CssClass="align-middle" HeaderStyle-CssClass="text-center" 
                                ItemStyle-HorizontalAlign="Center" />

                            
                            <asp:TemplateField HeaderText="Ad Soyad">
                                <ItemTemplate>
                                    <strong><%# Eval("Adi") %> <%# Eval("Soyad") %></strong>
                                </ItemTemplate>
                                <ItemStyle CssClass="align-middle" />
                            </asp:TemplateField>

                           
                            <asp:BoundField DataField="Toplam_Rapor" HeaderText="Rapor" 
                                DataFormatString="{0:0.##}" ItemStyle-CssClass="align-middle text-center" 
                                HeaderStyle-CssClass="text-center">
                                <ItemStyle Width="80px" />
                            </asp:BoundField>

                            
                            <asp:BoundField DataField="Toplam_Saatlik" HeaderText="Saatlik İzin" 
                                DataFormatString="{0:0.##}" ItemStyle-CssClass="align-middle text-center" 
                                HeaderStyle-CssClass="text-center">
                                <ItemStyle Width="100px" />
                            </asp:BoundField>

                            
                            <asp:BoundField DataField="Toplam_Mazeret" HeaderText="Mazeret İzni" 
                                DataFormatString="{0:0.##}" ItemStyle-CssClass="align-middle text-center" 
                                HeaderStyle-CssClass="text-center">
                                <ItemStyle Width="110px" />
                            </asp:BoundField>

                          
                            <asp:BoundField DataField="Toplam_Hastane" HeaderText="Hastane İzni" 
                                DataFormatString="{0:0.##}" ItemStyle-CssClass="align-middle text-center" 
                                HeaderStyle-CssClass="text-center">
                                <ItemStyle Width="110px" />
                            </asp:BoundField>

                            
                            <asp:BoundField DataField="Toplam_Yillik" HeaderText="Yıllık İzin" 
                                DataFormatString="{0:0.##}" ItemStyle-CssClass="align-middle text-center" 
                                HeaderStyle-CssClass="text-center">
                                <ItemStyle Width="90px" />
                            </asp:BoundField>

                           
                            <asp:TemplateField HeaderText="Toplam Kullanılan">
                                <ItemTemplate>
                                    <span class="badge bg-warning text-dark fs-6">
                                        <%# Eval("Toplam") == DBNull.Value ? "0" : string.Format("{0:0.##}", Eval("Toplam")) %>
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="align-middle text-center" Width="130px" />
                                <HeaderStyle CssClass="text-center" />
                            </asp:TemplateField>

                           
                            <asp:BoundField DataField="Devredenizin" HeaderText="Devreden İzin" 
                                DataFormatString="{0:0.##}" ItemStyle-CssClass="align-middle text-center" 
                                HeaderStyle-CssClass="text-center">
                                <ItemStyle Width="110px" />
                            </asp:BoundField>

                           
                            <asp:BoundField DataField="cariyilizni" HeaderText="Cari Yıl İzni" 
                                DataFormatString="{0:0.##}" ItemStyle-CssClass="align-middle text-center" 
                                HeaderStyle-CssClass="text-center">
                                <ItemStyle Width="110px" />
                            </asp:BoundField>

                           
                            <asp:TemplateField HeaderText="Kalan İzin">
                                <ItemTemplate>
                                    <span class="badge bg-success fs-6">
                                        <%# Eval("Kalanizin") == DBNull.Value ? "0" : string.Format("{0:0.##}", Eval("Kalanizin")) %>
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="align-middle text-center" Width="100px" />
                                <HeaderStyle CssClass="text-center" />
                            </asp:TemplateField>

                        </Columns>
                        <HeaderStyle CssClass="table-dark" />
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- KVKK Uyarısı -->
        <div class="privacy-notice mt-3">
            <i class="fas fa-shield-alt"></i>
            <strong>KVKK Uyarısı:</strong> Bu sayfada gösterilen personel bilgileri, 6698 sayılı Kişisel Verilerin Korunması Kanunu kapsamında gizlidir. 
            Yetkisiz kullanımı ve paylaşımı yasaktır.
        </div>

    </div>
</asp:Content>