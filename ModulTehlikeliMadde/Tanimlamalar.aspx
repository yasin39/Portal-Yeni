<%@ Page Title="Tehlikeli Madde - Faaliyet Tanımları" Language="C#" MasterPageFile="~/AnaV2.Master" 
    AutoEventWireup="true" CodeBehind="Tanimlamalar.aspx.cs" 
    Inherits="Portal.ModulTehlikeliMadde.Tanimlamalar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Content/Common-Components.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid py-4">
        
        <!-- Başlık -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="page-header-modern">
                    <h2 class="page-title">
                        <i class="fas fa-list-ul me-2"></i>
                        Tehlikeli Madde - Faaliyet Alanları Tanımları
                    </h2>
                    <p class="page-subtitle">Faaliyet alanlarını tanımlayın ve yönetin</p>
                </div>
            </div>
        </div>

        <div class="row">
            <!-- SOL PANEL: Form -->
            <div class="col-lg-5 col-md-12 mb-4">
                <div class="card shadow-sm">
                    <div class="card-header bg-gradient-primary text-white">
                        <h5 class="mb-0">
                            <i class="fas fa-edit me-2"></i>Faaliyet Bilgileri
                        </h5>
                    </div>
                    <div class="card-body">
                        <div class="mb-3">
                            <label for="txtFaaliyetAdi" class="form-label fw-bold">
                                <i class="fas fa-tag text-primary me-1"></i>Faaliyet Adı
                            </label>
                            <asp:TextBox ID="txtFaaliyetAdi" runat="server" 
                                CssClass="form-control" 
                                placeholder="Örn: Atık Yönetimi"
                                MaxLength="200"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFaaliyetAdi" runat="server" 
                                ControlToValidate="txtFaaliyetAdi" 
                                ErrorMessage="Faaliyet adı zorunludur" 
                                CssClass="text-danger small" 
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>

                        <div class="mb-4">
                            <label for="txtAciklama" class="form-label fw-bold">
                                <i class="fas fa-comment-dots text-primary me-1"></i>Açıklama
                            </label>
                            <asp:TextBox ID="txtAciklama" runat="server" 
                                CssClass="form-control" 
                                TextMode="MultiLine" 
                                Rows="4" 
                                placeholder="Faaliyet alanı hakkında detaylı açıklama..."></asp:TextBox>
                        </div>

                        <!-- Butonlar -->
                        <div class="d-flex gap-2 flex-wrap">
                            <asp:Button ID="btnKaydet" runat="server" 
                                Text="💾 Kaydet" 
                                CssClass="btn btn-primary flex-grow-1" 
                                OnClick="btnKaydet_Click" />
                            
                            <asp:Button ID="btnGuncelle" runat="server" 
                                Text="✏️ Güncelle" 
                                CssClass="btn btn-warning flex-grow-1" 
                                OnClick="btnGuncelle_Click" 
                                Visible="false" />
                            
                            <asp:Button ID="btnVazgec" runat="server" 
                                Text="🔄 Vazgeç" 
                                CssClass="btn btn-secondary" 
                                OnClick="btnVazgec_Click" 
                                CausesValidation="false" 
                                Visible="false" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- SAĞ PANEL: GridView -->
            <div class="col-lg-7 col-md-12">
                <div class="card shadow-sm">
                    <div class="card-header bg-gradient-info text-white d-flex justify-content-between align-items-center">
                        <h5 class="mb-0">
                            <i class="fas fa-table me-2"></i>Kayıtlı Faaliyet Alanları
                        </h5>
                        <span class="badge bg-light text-dark">
                            <asp:Label ID="lblKayitSayisi" runat="server" Text="0"></asp:Label> Kayıt
                        </span>
                    </div>
                    <div class="card-body p-0">
                        <div class="table-responsive">
                            <asp:GridView ID="FaaliyetlerGrid" runat="server" 
                                CssClass="table table-hover modern-table mb-0" 
                                AutoGenerateColumns="False" 
                                OnSelectedIndexChanged="FaaliyetlerGrid_SelectedIndexChanged"
                                AllowPaging="True" 
                                PageSize="10" 
                                OnPageIndexChanging="FaaliyetlerGrid_PageIndexChanging"
                                EmptyDataText="Henüz faaliyet alanı tanımlanmamış."
                                GridLines="None"
                                DataKeyNames="id">
                                
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                                    
                                    <asp:BoundField DataField="FaaliyetAdi" HeaderText="Faaliyet Adı" 
                                        ItemStyle-CssClass="fw-bold" />
                                    
                                    <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" 
                                        ItemStyle-CssClass="text-muted" />
                                    
                                    <asp:TemplateField HeaderText="İşlemler" ItemStyle-Width="100px" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button runat="server" 
                                                Text="✏️ Düzenle" 
                                                CssClass="btn btn-sm btn-outline-primary" 
                                                CommandName="Select" 
                                                CausesValidation="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                
                                <PagerStyle CssClass="pagination-modern" HorizontalAlign="Center" />
                                <EmptyDataRowStyle CssClass="text-center text-muted py-4" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <style>
        .bg-gradient-primary {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
        }
        .bg-gradient-info {
            background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
        }
        .page-header-modern {
            background: linear-gradient(135deg, #F5F5F0 0%, #FFFFFF 100%);
            padding: 1.5rem;
            border-radius: 8px;
            border-left: 4px solid #4B7BEC;
        }
        .page-title {
            color: #2E5B9A;
            font-weight: 700;
            font-size: 1.5rem;
            margin-bottom: 0.25rem;
        }
        .page-subtitle {
            color: #6B7280;
            margin-bottom: 0;
            font-size: 0.9rem;
        }
        .pagination-modern {
            padding: 1rem;
            background: #F8F9FA;
        }
    </style>
</asp:Content>