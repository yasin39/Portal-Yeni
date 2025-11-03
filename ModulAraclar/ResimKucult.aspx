<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResimKucult.aspx.cs" 
    Inherits="Portal.ModulAraclar.ResimKucult" MasterPageFile="~/AnaV2.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <%-- Gerekirse bu sayfaya özel CSS stilleri --%>
</asp:Content>

<asp:Content ID="BreadcrumbContent" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <%-- Master Page'deki Breadcrumb alanını doldurur --%>
    <li class="breadcrumb-item"><a href="/Anasayfa.aspx">Ana Sayfa</a></li>
    <li class="breadcrumb-item">Çeşitli Araçlar</li>
    <li class="breadcrumb-item active" aria-current="page">Resim Boyutu Küçültme</li>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="card shadow-sm">
        <div class="card-header bg-light">
            <h5 class="mb-0">
                <i class="fas fa-compress-arrows-alt me-2"></i>Resim Boyutu Küçültme Aracı
            </h5>
        </div>
        
        <div class="card-body">
            
            <div class="alert alert-info">
                <h6 class="alert-heading">Nasıl Kullanılır?</h6>
                <p class="mb-0">Boyutunu küçültmek istediğiniz bir resim dosyasını (.jpg, .png, .gif, .bmp) seçin. Ardından, uygulanacak küçültme oranını seçin ve butona basın.</p>
            </div>

            <div class="row g-3 mt-3 align-items-end">
                <div class="col-md-5">
                    <label for="<%=FileUploadControl.ClientID%>" class="form-label"><strong>1. Resim Dosyası Seçin:</strong></label>
                    <%-- Sadece tek bir dosya yüklenmesine izin verilir --%>
                    <asp:FileUpload ID="FileUploadControl" runat="server" 
                        CssClass="form-control" 
                        accept="image/*" />
                </div>
                
                <div class="col-md-4">
                    <label for="<%=ddlOran.ClientID%>" class="form-label"><strong>2. Küçültme Oranı Seçin:</strong></label>
                    <asp:DropDownList ID="ddlOran" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>

                <div class="col-md-3">
                    <asp:Button ID="btnKucult" runat="server" 
                        Text="Resmi Küçült" 
                        OnClick="btnKucult_Click" 
                        CssClass="btn btn-primary w-100" />
                        
                    <%-- İşlem sırasında butonun devre dışı kalması için script paneli (PdfBirlestir'deki gibi) --%>
                    <asp:Panel ID="pnlClientScript" runat="server" Visible="false">
                        <script type="text/javascript">
                            // Butonun tekrar aktif edilmesi için
                            function enableMergeButton() {
                                var btn = document.getElementById('<%= btnKucult.ClientID %>');
                                if (btn) {
                                    btn.disabled = false;
                                    btn.value = 'Resmi Küçült';
                                }
                            }
                        </script>
                    </asp:Panel>
                </div>
            </div>

        </div>
    </div>
</asp:Content>