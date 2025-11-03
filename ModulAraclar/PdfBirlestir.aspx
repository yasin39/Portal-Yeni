<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PdfBirlestir.aspx.cs" 
    Inherits="Portal.ModulAraclar.PdfBirlestir" MasterPageFile="~/AnaV2.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <%-- Gerekirse bu sayfaya özel CSS stilleri --%>
</asp:Content>

<asp:Content ID="BreadcrumbContent" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <%-- Master Page'deki Breadcrumb alanını doldurur [cite: 85] --%>
    <li class="breadcrumb-item"><a href="/Anasayfa.aspx">Ana Sayfa</a></li>
    <li class="breadcrumb-item">Çeşitli Araçlar</li>
    <li class="breadcrumb-item active" aria-current="page">PDF Birleştirici</li>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <%-- Portalın geneliyle uyumlu kart yapısı --%>
    <div class="card shadow-sm">
        <div class="card-header bg-light">
            <h5 class="mb-0">
                <i class="fas fa-file-pdf me-2 text-danger"></i>PDF Birleştirme Aracı
            </h5>
        </div>
        
        <div class="card-body">
            
            <div class="alert alert-info">
                <h6 class="alert-heading">Nasıl Kullanılır?</h6>
                <p class="mb-0">Lütfen birleştirmek istediğiniz PDF dosyalarını seçin (Ctrl tuşuna basılı tutarak birden fazla dosya seçebilirsiniz). Dosyalar, seçim sırasına göre birleştirilecektir. En az 2, en fazla 3 dosya seçmelisiniz.</p>
            </div>

            <div class="row mt-4">
                <div class="col-md-8">
                    <label for="<%=FileUploadControl.ClientID%>" class="form-label"><strong>Birleştirilecek PDF Dosyaları:</strong></label>
                    
                    <%-- AllowMultiple="true" özelliği, birden fazla dosya seçilmesini sağlar --%>
                    <asp:FileUpload ID="FileUploadControl" runat="server" 
                        AllowMultiple="true" 
                        CssClass="form-control" 
                        accept="application/pdf" />
                </div>
                
                <div class="col-md-4 d-flex align-items-end">
                    <asp:Button ID="btnBirlestir" runat="server" 
                        Text="Seçili PDF'leri Birleştir" 
                        OnClick="btnBirlestir_Click" 
                        CssClass="btn btn-primary w-100" />
                        
                    <%-- Kullanıcıya işlem yapıldığını göstermek için küçük bir script --%>
                    <asp:Panel ID="pnlClientScript" runat="server" Visible="false">
                        <script type="text/javascript">
                            // Butonun tekrar aktif edilmesi için
                            function enableMergeButton() {
                                var btn = document.getElementById('<%= btnBirlestir.ClientID %>');
                                if (btn) {
                                    btn.disabled = false;
                                    btn.value = 'Seçili PDF\'leri Birleştir';
                                }
                            }
                        </script>
                    </asp:Panel>
                </div>
            </div>

        </div>
    </div>
</asp:Content>