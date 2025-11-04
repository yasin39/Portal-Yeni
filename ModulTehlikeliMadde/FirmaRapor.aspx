<%@ Page Title="Tehlikeli Madde Firma Rapor" Language="C#" MasterPageFile="~/AnaV2.Master" 
    AutoEventWireup="true" CodeBehind="FirmaRapor.aspx.cs" 
    Inherits="Portal.ModulTehlikeliMadde.FirmaRapor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .filter-section {
            background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
            padding: 25px;
            border-radius: 10px;
            margin-bottom: 25px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
        }
        
        .filter-row {
            margin-bottom: 15px;
        }
        
        .action-buttons {
            display: flex;
            gap: 10px;
            margin-top: 20px;
            flex-wrap: wrap;
        }
        
        .grid-container {
            background: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            margin-top: 20px;
        }
        
        .page-header {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            color: white;
            padding: 20px 25px;
            border-radius: 10px;
            margin-bottom: 25px;
            box-shadow: 0 4px 12px rgba(75, 123, 236, 0.3);
        }
        
        .page-header h2 {
            margin: 0;
            font-size: 24px;
            font-weight: 600;
        }
        
        .form-label {
            font-weight: 500;
            color: #495057;
            margin-bottom: 5px;
        }
        
        .gridview-modern {
            width: 100%;
            border-collapse: collapse;
        }
        
        .gridview-modern th {
            background: linear-gradient(135deg, #4B7BEC 0%, #2E5B9A 100%);
            color: white;
            padding: 12px;
            text-align: left;
            font-weight: 500;
            border: none;
        }
        
        .gridview-modern td {
            padding: 10px 12px;
            border-bottom: 1px solid #dee2e6;
        }
        
        .gridview-modern tr:hover {
            background-color: #f8f9fa;
        }
        
        .gridview-modern tfoot td {
            background-color: #f8f9fa;
            font-weight: 600;
            padding: 12px;
            border-top: 2px solid #4B7BEC;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="page-header">
            <h2>🧪 Tehlikeli Madde Firma Raporlama</h2>
        </div>

        <div class="filter-section">
            <h5 class="mb-3">🔍 Arama Filtreleri</h5>
            
            <div class="row filter-row">
                <div class="col-md-3">
                    <label class="form-label">UNET No</label>
                    <asp:TextBox ID="txtUnet" runat="server" CssClass="form-control" 
                        placeholder="UNET giriniz"></asp:TextBox>
                </div>
                
                <div class="col-md-3">
                    <label class="form-label">Ünvan</label>
                    <asp:TextBox ID="txtUnvan" runat="server" CssClass="form-control" 
                        placeholder="Ünvan giriniz"></asp:TextBox>
                </div>
                
                <div class="col-md-3">
                    <label class="form-label">Statü</label>
                    <asp:DropDownList ID="ddlStatu" runat="server" CssClass="form-select">
                        <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                        <asp:ListItem Value="Ticari Kuruluş">Ticari Kuruluş</asp:ListItem>
                        <asp:ListItem Value="STK">STK</asp:ListItem>
                        <asp:ListItem Value="Kamu Kurumu">Kamu Kurumu</asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="col-md-3">
                    <label class="form-label">Faaliyet Alanı</label>
                    <asp:DropDownList ID="ddlFaaliyetAlani" runat="server" CssClass="form-select">
                    </asp:DropDownList>
                </div>
            </div>
            
            <div class="row filter-row">
                <div class="col-md-3">
                    <label class="form-label">İl</label>
                    <asp:DropDownList ID="ddlIl" runat="server" CssClass="form-select" 
                        AutoPostBack="true" OnSelectedIndexChanged="ddlIl_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                
                <div class="col-md-3">
                    <label class="form-label">İlçe</label>
                    <asp:DropDownList ID="ddlIlce" runat="server" CssClass="form-select">
                    </asp:DropDownList>
                </div>
                
                <div class="col-md-3">
                    <label class="form-label">Durum</label>
                    <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-select">
                        <asp:ListItem Value="Hepsi">Hepsi</asp:ListItem>
                        <asp:ListItem Value="Aktif">Aktif</asp:ListItem>
                        <asp:ListItem Value="Pasif">Pasif</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            
            <div class="action-buttons">
                <asp:Button ID="btnAra" runat="server" CssClass="btn btn-primary" 
                    Text="🔍 Ara" OnClick="btnAra_Click" />
                    
                <asp:Button ID="btnPdfRapor" runat="server" CssClass="btn btn-danger" 
                    Text="📄 PDF Rapor" OnClick="btnPdfRapor_Click" />
                    
                <asp:Button ID="btnExcelAktar" runat="server" CssClass="btn btn-success" 
                    Text="📊 Excel Aktar" OnClick="btnExcelAktar_Click" />
                    
                <asp:Button ID="btnTemizle" runat="server" CssClass="btn btn-secondary" 
                    Text="🔄 Temizle" OnClick="btnTemizle_Click" CausesValidation="false" />
            </div>
        </div>

        <div class="grid-container">
            <asp:GridView ID="FirmalarGrid" runat="server" CssClass="gridview-modern"
                AutoGenerateColumns="False" ShowFooter="True"
                EmptyDataText="Kayıt bulunamadı.">
                <Columns>
                    <asp:BoundField DataField="Unet" HeaderText="UNET" />
                    <asp:BoundField DataField="Unvan" HeaderText="Ünvan" />
                    <asp:BoundField DataField="Adres" HeaderText="Adres" />
                    <asp:BoundField DataField="ilce" HeaderText="İlçe" />
                    <asp:BoundField DataField="il" HeaderText="İl" />
                    <asp:BoundField DataField="Statu" HeaderText="Statü" />
                    <asp:BoundField DataField="BelgeTuru" HeaderText="Belge Türü" />
                    <asp:BoundField DataField="BelgeSeriNo" HeaderText="Belge Seri No" />
                    <asp:BoundField DataField="FaaliyetTuru" HeaderText="Faaliyet Türü" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>