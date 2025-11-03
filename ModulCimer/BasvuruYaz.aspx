<%@ Page Title="" Language="C#" MasterPageFile="~/AnaV2.Master" AutoEventWireup="true" 
    CodeBehind="BasvuruYaz.aspx.cs" Inherits="Portal.ModulCimer.BasvuruYaz" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/wwwroot/css/CIMERMODUL.css") %>" rel="stylesheet" />
    <link href="<%= ResolveUrl("~/wwwroot/css/AnaV2.css") %>" rel="stylesheet" />
    <style>
        .report-container {
            width: 21cm;
            min-height: 29.7cm;
            padding: 2.5cm 2cm;
            margin: 30px auto;
            background: white;
            box-shadow: 0 0 15px rgba(0,0,0,0.1);
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            position: relative;
            font-size: 11pt;
            line-height: 1.6;
        }

        .header-line {
            border-top: 2px solid #000;
            margin: 20px 0 15px 0;
        }

        .title-center {
            text-align: center;
            font-weight: bold;
            font-size: 13pt;
            margin: 6px 0;
        }

        .info-row {
            display: flex;
            justify-content: space-between;
            margin: 12px 0;
            font-size: 11pt;
        }

        .info-label {
            font-weight: bold;
            min-width: 120px;
        }

        .complaint-title {
            font-weight: bold;
            font-size: 12pt;
            margin: 20px 0 10px 0;
        }

        .complaint-box {
            border: 1px solid #ccc;
            padding: 18px;
            min-height: 220px;
            margin-top: 10px;
            background: #f9f9f9;
            border-radius: 6px;
            font-size: 11pt;
            line-height: 1.7;
        }

        .logo-cimer {
            height: 70px;
            position: absolute;
            top: 50px;
            left: 70px;
        }

        .logo-muhur {
            height: 70px;
            position: absolute;
            top: 50px;
            right: 70px;
        }

        .datetime-top {
            position: absolute;
            top: 20px;
            right: 70px;
            font-size: 10pt;
            color: #333;
            font-weight: 500;
        }

        /* btn-pdf stilleri btn-gradient-danger class'ı ile değiştirildi (Gradients.css) */
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    

    <div class="container-fluid">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="/Anasayfa.aspx">Ana Sayfa</a></li>
                <li class="breadcrumb-item"><a href="/ModulCimer/Default.aspx">CİMER Modülü</a></li>
                <li class="breadcrumb-item active">Başvuru Raporu</li>
            </ol>
        </nav>

        <div class="form-section">
            <h4 class="section-title"><i class="fas fa-file-alt"></i> CİMER Talep/Şikayet Raporu</h4>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="form-label">Başvuru No</label>
                        <asp:TextBox ID="txtBasvuruNo" runat="server" CssClass="form-control" Placeholder="1600004717"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-3 d-flex align-items-end">
                    <asp:Button ID="btnGetir" runat="server" Text="Raporu Getir" CssClass="btn btn-primary" OnClick="btnGetir_Click" />
                </div>
            </div>
        </div>

        <asp:Panel ID="pnlRapor" runat="server" Visible="false" CssClass="form-section">
            <div class="text-end mb-3">
                <button type="button" class="btn btn-gradient-danger" onclick="exportToPDF()">
                    <i class="fas fa-file-pdf"></i> PDF Oluştur
                </button>
            </div>

            <!-- PDF İÇİN GİZLENEN ALAN -->
            <div id="reportContent" class="report-container">
                <div class="datetime-top" id="reportDateTime"></div>

                <img src="<%= ResolveUrl("~/wwwroot/Images/cimer_logo.png") %>" class="logo-cimer" alt="CİMER" />
                <img src="<%= ResolveUrl("~/wwwroot/Images/muhur.png") %>" class="logo-muhur" alt="Mühür" />

                <div style="margin-top: 100px; text-align: center;">
                    <div class="title-center">Ulaştırma ve Altyapı Bakanlığı</div>
                    <div class="title-center">II. Bölge Müdürlüğü</div>
                    <div class="title-center">Cimer Talep/Şikayet Formu</div>
                </div>

                <div class="header-line"></div>

                <div class="info-row">
                    <div>
                        <span class="info-label">Başvuru No:</span>
                        <span id="lblBasvuruNo"><%= BasvuruNo %></span>
                    </div>
                    <div>
                        <span class="info-label">Başvuru Tarihi:</span>
                        <span id="lblBasvuruTarihi"><%= BasvuruTarihi %></span>
                    </div>
                    <div>
                        <span class="info-label">İlgili Firma:</span>
                        <span id="lblFirma"><%= IlgiliFirma %></span>
                    </div>
                </div>

                <div class="header-line"></div>

                <div class="complaint-title">Talep / Şikayet:</div>
                <div class="complaint-box" id="lblSikayet"><%= SikayetMetni %></div>
            </div>
        </asp:Panel>
    </div>

    <!-- jsPDF + html2canvas -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js"></script>

    <script>
        function exportToPDF() {
            const { jsPDF } = window.jspdf;
            const element = document.getElementById('reportContent');
            const now = new Date();
            const dateStr = now.toLocaleDateString('tr-TR') + ' - ' + now.toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' });

            document.getElementById('reportDateTime').innerText = dateStr;

            html2canvas(element, {
                scale: 2,
                useCORS: true,
                allowTaint: true,
                backgroundColor: '#ffffff'
            }).then(canvas => {
                const imgData = canvas.toDataURL('image/png');
                const pdf = new jsPDF('p', 'mm', 'a4');
                const width = pdf.internal.pageSize.getWidth();
                const height = pdf.internal.pageSize.getHeight();

                pdf.addImage(imgData, 'PNG', 0, 0, width, height);
                pdf.save('Cimer_Basvuru_' + '<%= BasvuruNo %>' + '.pdf');
            }).catch(err => {
                alert('PDF oluşturulurken hata: ' + err.message);
            });
        }
    </script>
</asp:Content>