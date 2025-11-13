<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogViewer.aspx.cs" Inherits="Portal.LogViewer" %>

<!DOCTYPE html>
<html lang="tr">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Log Görüntüleyici</title>
    
    <!-- Bootstrap 5.3.8 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css">
    
    <style>
        :root {
            --dark-bg: #1a1a1a;
            --darker-bg: #0f0f0f;
            --card-bg: #2d2d2d;
            --card-hover: #3a3a3a;
            --border-color: #404040;
            --text-primary: #e0e0e0;
            --text-secondary: #b0b0b0;
            --accent-primary: #6c757d;
            --accent-secondary: #495057;
        }
        
        body {
            background-color: var(--dark-bg);
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            color: var(--text-primary);
        }
        
        .page-header {
            background: linear-gradient(135deg, #212529 0%, #343a40 100%);
            color: var(--text-primary);
            padding: 2rem 0;
            margin-bottom: 2rem;
            box-shadow: 0 4px 12px rgba(0,0,0,0.5);
            border-bottom: 2px solid var(--border-color);
        }
        
        .page-header h1 {
            font-weight: 600;
            text-shadow: 2px 2px 4px rgba(0,0,0,0.5);
        }
        
        .stat-card {
            background: var(--card-bg);
            border: 1px solid var(--border-color);
            border-radius: 12px;
            padding: 1.5rem;
            margin-bottom: 1.5rem;
            box-shadow: 0 4px 8px rgba(0,0,0,0.4);
            transition: all 0.3s ease;
            cursor: pointer;
            position: relative;
        }
        
        .stat-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 16px rgba(0,0,0,0.6);
            background: var(--card-hover);
            border-color: #555;
        }
        
        .stat-card.active {
            transform: translateY(-5px);
            box-shadow: 0 0 0 3px rgba(108, 117, 125, 0.5);
            border-color: #6c757d;
        }
        
        .stat-card .stat-number {
            font-size: 2.5rem;
            font-weight: bold;
            margin: 0;
            color: var(--text-primary);
        }
        
        .stat-card .stat-label {
            font-size: 0.9rem;
            color: var(--text-secondary);
            margin-top: 0.5rem;
        }
        
        .stat-card.error {
            border-left: 4px solid #dc3545;
        }
        
        .stat-card.warning {
            border-left: 4px solid #ffc107;
        }
        
        .stat-card.info {
            border-left: 4px solid #0dcaf0;
        }
        
        .stat-card.total {
            border-left: 4px solid #6c757d;
        }
        
        .stat-card .badge-click {
            position: absolute;
            top: 10px;
            right: 10px;
            background: rgba(108, 117, 125, 0.3);
            padding: 0.3rem 0.6rem;
            border-radius: 15px;
            font-size: 0.75rem;
            color: var(--text-secondary);
        }
        
        .filter-panel {
            background: var(--card-bg);
            border: 1px solid var(--border-color);
            border-radius: 12px;
            padding: 1.5rem;
            margin-bottom: 1.5rem;
            box-shadow: 0 4px 8px rgba(0,0,0,0.4);
        }
        
        .quick-filters {
            background: var(--card-bg);
            border: 1px solid var(--border-color);
            border-radius: 12px;
            padding: 1rem;
            margin-bottom: 1.5rem;
            box-shadow: 0 4px 8px rgba(0,0,0,0.4);
        }
        
        .quick-filter-btn {
            border-radius: 20px;
            padding: 0.6rem 1.5rem;
            font-weight: 600;
            border: 2px solid var(--border-color);
            transition: all 0.3s;
            margin-right: 0.5rem;
            margin-bottom: 0.5rem;
            background: var(--card-bg);
            color: var(--text-primary);
        }
        
        .quick-filter-btn:hover {
            background: var(--card-hover);
            border-color: #6c757d;
        }
        
        .quick-filter-btn.active {
            transform: scale(1.05);
            box-shadow: 0 4px 12px rgba(0,0,0,0.5);
            border-color: #6c757d;
            background: #495057;
        }
        
        .log-table-container {
            background: var(--card-bg);
            border: 1px solid var(--border-color);
            border-radius: 12px;
            padding: 1.5rem;
            box-shadow: 0 4px 8px rgba(0,0,0,0.4);
        }
        
        .log-table {
            margin-top: 1rem;
        }
        
        .log-table th {
            background-color: #343a40;
            color: var(--text-primary);
            font-weight: 600;
            border: 1px solid var(--border-color);
            padding: 1rem;
        }
        
        .log-table td {
            vertical-align: middle;
            padding: 0.75rem 1rem;
            background-color: var(--card-bg);
            border: 1px solid var(--border-color);
            color: var(--text-primary);
        }
        
        .log-table tbody tr:hover td {
            background-color: var(--card-hover);
        }
        
        .badge-level {
            padding: 0.5rem 1rem;
            border-radius: 20px;
            font-weight: 600;
            font-size: 0.85rem;
        }
        
        .badge-error {
            background-color: #dc3545;
            color: white;
        }
        
        .badge-warning {
            background-color: #ffc107;
            color: #000;
        }
        
        .badge-info {
            background-color: #0dcaf0;
            color: #000;
        }
        
        .log-message {
            max-width: 400px;
            word-wrap: break-word;
            font-size: 0.9rem;
            font-family: 'Courier New', monospace;
            color: var(--text-secondary);
        }
        
        .stack-trace-cell {
            max-width: 350px;
            word-wrap: break-word;
            font-size: 0.85rem;
            font-family: 'Courier New', monospace;
            color: #ff6b6b;
        }
        
        .btn-action {
            border-radius: 20px;
            padding: 0.5rem 1.5rem;
            font-weight: 600;
            transition: all 0.3s;
            background: var(--accent-primary);
            border: 1px solid var(--border-color);
            color: var(--text-primary);
        }
        
        .btn-action:hover {
            transform: scale(1.05);
            background: var(--accent-secondary);
            color: var(--text-primary);
        }
        
        .btn-light {
            background: var(--card-bg);
            border-color: var(--border-color);
            color: var(--text-primary);
        }
        
        .btn-light:hover {
            background: var(--card-hover);
            color: var(--text-primary);
        }
        
        .empty-state {
            text-align: center;
            padding: 3rem;
            color: var(--text-secondary);
        }
        
        .empty-state i {
            font-size: 4rem;
            margin-bottom: 1rem;
            opacity: 0.3;
        }
        
        .form-control, .form-select {
            background-color: var(--darker-bg);
            border: 1px solid var(--border-color);
            color: var(--text-primary);
        }
        
        .form-control:focus, .form-select:focus {
            background-color: var(--darker-bg);
            border-color: #6c757d;
            box-shadow: 0 0 0 0.2rem rgba(108, 117, 125, 0.25);
            color: var(--text-primary);
        }
        
        .form-label {
            color: var(--text-secondary);
            font-weight: 500;
        }
        
        .badge.bg-primary {
            background-color: #495057 !important;
        }
        
        h5, h6 {
            color: var(--text-primary);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        
        <!-- Hidden fields for stat card filtering -->
        <asp:HiddenField ID="hdnFilterLevel" runat="server" />
        
        <!-- Header -->
        <div class="page-header">
            <div class="container">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <h1 class="mb-2">
                            <i class="fas fa-clipboard-list me-2"></i>Log Görüntüleyici
                        </h1>
                        <p class="mb-0 opacity-75">Sistem loglarını izleyin ve yönetin</p>
                    </div>
                    <div>
                        <asp:Button ID="btnRefresh" runat="server" Text="🔄 Yenile" CssClass="btn btn-light btn-action me-2" OnClick="btnRefresh_Click" />
                    </div>
                </div>
            </div>
        </div>
        
        <div class="container">
            <!-- Statistics Cards (Clickable) -->
            <div class="row">
                <div class="col-md-3">
                    <asp:LinkButton ID="lnkFilterAll" runat="server" OnClick="lnkFilterAll_Click" CssClass="text-decoration-none">
                        <div class="stat-card total" id="cardTotal">
                            <span class="badge-click">Tıkla</span>
                            <i class="fas fa-list fa-2x mb-3"></i>
                            <h2 class="stat-number">
                                <asp:Label ID="lblTotalCount" runat="server" Text="0"></asp:Label>
                            </h2>
                            <div class="stat-label">Tüm Kayıtlar</div>
                        </div>
                    </asp:LinkButton>
                </div>
                <div class="col-md-3">
                    <asp:LinkButton ID="lnkFilterError" runat="server" OnClick="lnkFilterError_Click" CssClass="text-decoration-none">
                        <div class="stat-card error" id="cardError">
                            <span class="badge-click">Tıkla</span>
                            <i class="fas fa-exclamation-circle fa-2x mb-3"></i>
                            <h2 class="stat-number">
                                <asp:Label ID="lblErrorCount" runat="server" Text="0"></asp:Label>
                            </h2>
                            <div class="stat-label">Hatalar</div>
                        </div>
                    </asp:LinkButton>
                </div>
                <div class="col-md-3">
                    <asp:LinkButton ID="lnkFilterWarning" runat="server" OnClick="lnkFilterWarning_Click" CssClass="text-decoration-none">
                        <div class="stat-card warning" id="cardWarning">
                            <span class="badge-click">Tıkla</span>
                            <i class="fas fa-exclamation-triangle fa-2x mb-3"></i>
                            <h2 class="stat-number">
                                <asp:Label ID="lblWarningCount" runat="server" Text="0"></asp:Label>
                            </h2>
                            <div class="stat-label">Uyarılar</div>
                        </div>
                    </asp:LinkButton>
                </div>
                <div class="col-md-3">
                    <asp:LinkButton ID="lnkFilterInfo" runat="server" OnClick="lnkFilterInfo_Click" CssClass="text-decoration-none">
                        <div class="stat-card info" id="cardInfo">
                            <span class="badge-click">Tıkla</span>
                            <i class="fas fa-info-circle fa-2x mb-3"></i>
                            <h2 class="stat-number">
                                <asp:Label ID="lblInfoCount" runat="server" Text="0"></asp:Label>
                            </h2>
                            <div class="stat-label">Bilgiler</div>
                        </div>
                    </asp:LinkButton>
                </div>
            </div>
            
            <!-- Quick Filters -->
            <div class="quick-filters">
                <div class="d-flex align-items-center">
                    <h6 class="me-3 mb-0">
                        <i class="fas fa-bolt me-2"></i>Hızlı Filtre:
                    </h6>
                    <asp:Button ID="btnQuickAll" runat="server" Text="📋 Tümü" CssClass="quick-filter-btn" OnClick="btnQuickAll_Click" />
                    <asp:Button ID="btnQuickError" runat="server" Text="❌ Hatalar" CssClass="quick-filter-btn" OnClick="btnQuickError_Click" />
                    <asp:Button ID="btnQuickWarning" runat="server" Text="⚠️ Uyarılar" CssClass="quick-filter-btn" OnClick="btnQuickWarning_Click" />
                    <asp:Button ID="btnQuickInfo" runat="server" Text="ℹ️ Bilgiler" CssClass="quick-filter-btn" OnClick="btnQuickInfo_Click" />
                </div>
            </div>
            
            <!-- Filter Panel -->
            <div class="filter-panel">
                <h5 class="mb-3">
                    <i class="fas fa-filter me-2"></i>Gelişmiş Filtre
                </h5>
                <div class="row g-3">
                    <div class="col-md-3">
                        <label class="form-label">Seviye</label>
                        <asp:DropDownList ID="ddlLevel" runat="server" CssClass="form-select">
                            <asp:ListItem Value="" Text="Tümü"></asp:ListItem>
                            <asp:ListItem Value="ERROR" Text="Hata"></asp:ListItem>
                            <asp:ListItem Value="WARNING" Text="Uyarı"></asp:ListItem>
                            <asp:ListItem Value="INFO" Text="Bilgi"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Tarih</label>
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <label class="form-label">Ara</label>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Mesajda ara..."></asp:TextBox>
                    </div>
                    <div class="col-md-2 d-flex align-items-end">
                        <asp:Button ID="btnFilter" runat="server" Text="🔍 Filtrele" CssClass="btn btn-action w-100" OnClick="btnFilter_Click" />
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-md-12">
                        <asp:Button ID="btnClearFilter" runat="server" Text="🧹 Filtreyi Temizle" CssClass="btn btn-action me-2" OnClick="btnClearFilter_Click" />
                        <asp:Button ID="btnExportExcel" runat="server" Text="📊 Excel" CssClass="btn btn-action me-2" OnClick="btnExportExcel_Click" />
                        <asp:Button ID="btnExportPdf" runat="server" Text="📄 PDF" CssClass="btn btn-action me-2" OnClick="btnExportPdf_Click" />
                        <asp:Button ID="btnClearLog" runat="server" Text="🗑️ Log Temizle" CssClass="btn btn-action" OnClick="btnClearLog_Click" OnClientClick="return confirm('Log dosyasını tamamen temizlemek istediğinize emin misiniz?');" />
                    </div>
                </div>
            </div>
            
            <!-- Log Table -->
            <div class="log-table-container">
                <h5 class="mb-3">
                    <i class="fas fa-table me-2"></i>Log Kayıtları
                    <asp:Label ID="lblFilterInfo" runat="server" CssClass="badge bg-primary ms-2" Visible="false"></asp:Label>
                </h5>
                
                <asp:Panel ID="pnlEmptyState" runat="server" Visible="false" CssClass="empty-state">
                    <i class="fas fa-inbox"></i>
                    <h4>Kayıt Bulunamadı</h4>
                    <p>Seçilen kriterlere uygun log kaydı bulunmamaktadır.</p>
                </asp:Panel>
                
                <asp:GridView ID="gvLogs" runat="server" 
                    CssClass="table table-hover log-table"
                    AutoGenerateColumns="False"
                    EmptyDataText="Log kaydı bulunamadı"
                    AllowPaging="True"
                    PageSize="20"
                    OnPageIndexChanging="gvLogs_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Tarih/Saat" ItemStyle-Width="180px">
                            <ItemTemplate>
                                <i class="far fa-clock me-2"></i>
                                <%# ((DateTime)Eval("Timestamp")).ToString("dd.MM.yyyy HH:mm:ss") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Seviye" ItemStyle-Width="110px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span class='badge badge-<%# Eval("Level").ToString().ToLower() %>'>
                                    <i class="fas <%# Eval("LevelIcon") %> me-1"></i>
                                    <%# Eval("Level").ToString() == "ERROR" ? "Hata" : Eval("Level").ToString() == "WARNING" ? "Uyarı" : Eval("Level").ToString() == "INFO" ? "Bilgi" : Eval("Level").ToString() %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Mesaj">
                            <ItemTemplate>
                                <div class="log-message">
                                    <%# Eval("Message") %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Son Stack Trace">
                            <ItemTemplate>
                                <div class="stack-trace-cell">
                                    <%# !string.IsNullOrEmpty(Eval("LastStackTrace")?.ToString()) ? Eval("LastStackTrace") : "<span style='color: #666;'>-</span>" %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    
                    <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="İlk" LastPageText="Son" />
                </asp:GridView>
            </div>
        </div>
        
        <div style="height: 3rem;"></div>
    </form>
    
    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    
    <script>
        // Active card highlighter
        function highlightCard(cardId) {
            document.querySelectorAll('.stat-card').forEach(card => {
                card.classList.remove('active');
            });
            if (cardId) {
                document.getElementById(cardId)?.classList.add('active');
            }
        }
        
        // Active button highlighter
        function highlightButton(btnId) {
            document.querySelectorAll('.quick-filter-btn').forEach(btn => {
                btn.classList.remove('active');
            });
            if (btnId) {
                document.getElementById(btnId)?.classList.add('active');
            }
        }
        
        // Check active filter on page load
        window.onload = function() {
            var filterLevel = document.getElementById('<%= hdnFilterLevel.ClientID %>').value;
            if (filterLevel === '') {
                highlightCard('cardTotal');
                highlightButton('<%= btnQuickAll.ClientID %>');
            } else if (filterLevel === 'ERROR') {
                highlightCard('cardError');
                highlightButton('<%= btnQuickError.ClientID %>');
            } else if (filterLevel === 'WARNING') {
                highlightCard('cardWarning');
                highlightButton('<%= btnQuickWarning.ClientID %>');
            } else if (filterLevel === 'INFO') {
                highlightCard('cardInfo');
                highlightButton('<%= btnQuickInfo.ClientID %>');
            }
        };
    </script>
</body>
</html>
