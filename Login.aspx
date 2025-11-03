<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Portal.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Ankara Portal - Giriş</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />

    <style>
        body {
            background: linear-gradient(135deg, #1a2a44 0%, #0d1929 100%);
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        .login-container {
            max-width: 450px;
            width: 100%;
            padding: 20px;
        }

        .login-card {
            background: white;
            border-radius: 15px;
            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.2);
            overflow: hidden;
        }

        .login-header {
            background: linear-gradient(135deg, #1a2a44 0%, #2563eb 100%);
            color: white;
            padding: 40px 30px;
            text-align: center;
        }

            .login-header h2 {
                margin: 0 0 10px 0;
                font-size: 28px;
                font-weight: 600;
            }

            .login-header p {
                margin: 0;
                opacity: 0.9;
                font-size: 14px;
            }

        .login-body {
            padding: 40px 30px;
        }

        .form-group {
            margin-bottom: 25px;
        }

        .form-label {
            font-weight: 600;
            color: #374151;
            margin-bottom: 8px;
            font-size: 14px;
        }

        .input-group {
            position: relative;
        }

        .input-group-icon {
            position: absolute;
            left: 15px;
            top: 50%;
            transform: translateY(-50%);
            color: #6b7280;
            z-index: 10;
        }

        .form-control {
            padding: 12px 15px 12px 45px;
            border: 2px solid #e5e7eb;
            border-radius: 8px;
            font-size: 15px;
            transition: all 0.3s ease;
        }

            .form-control:focus {
                border-color: #2563eb;
                box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.1);
            }

        .btn-login {
            width: 100%;
            padding: 14px;
            background: linear-gradient(135deg, #2563eb 0%, #1d4ed8 100%);
            border: none;
            border-radius: 8px;
            color: white;
            font-weight: 600;
            font-size: 16px;
            transition: all 0.3s ease;
            margin-top: 10px;
        }

            .btn-login:hover {
                transform: translateY(-2px);
                box-shadow: 0 8px 20px rgba(37, 99, 235, 0.4);
            }

        .btn-clear {
            width: 100%;
            padding: 14px;
            background: #f3f4f6;
            border: 2px solid #e5e7eb;
            border-radius: 8px;
            color: #374151;
            font-weight: 600;
            font-size: 16px;
            transition: all 0.3s ease;
            margin-top: 10px;
        }

            .btn-clear:hover {
                background: #e5e7eb;
                border-color: #d1d5db;
            }

        .footer-text {
            text-align: center;
            margin-top: 30px;
            color: white;
            font-size: 13px;
        }

        .alert {
            border-radius: 8px;
            padding: 12px 15px;
            margin-bottom: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="login-card">
                <div class="login-header">
                    <h2>Ankara Portal</h2>
                    <p>Kurumsal Yönetim Sistemi</p>
                </div>

                <div class="login-body">
                    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-danger">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </asp:Panel>

                    <div class="form-group">
                        <label class="form-label">Sicil No</label>
                        <div class="input-group">
                            <span class="fa-solid fa-user input-group-icon"></span>
                            <asp:TextBox ID="txtSicilNo" runat="server" CssClass="form-control"
                                placeholder="Sicil numaranızı giriniz" MaxLength="50"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvSicilNo" runat="server"
                            ControlToValidate="txtSicilNo"
                            ErrorMessage="Sicil No zorunludur"
                            CssClass="text-danger small"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group">
                        <label class="form-label">Parola</label>
                        <div class="input-group">
                            <span class="fa-solid fa-lock input-group-icon"></span>
                            <asp:TextBox ID="txtParola" runat="server" CssClass="form-control"
                                TextMode="Password"
                                placeholder="Parolanızı giriniz" MaxLength="50"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvParola" runat="server"
                            ControlToValidate="txtParola"
                            ErrorMessage="Parola zorunludur"
                            CssClass="text-danger small"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>

                    <asp:Button ID="btnGiris" runat="server" Text="Giriş Yap"
                        CssClass="btn-login" OnClick="btnGiris_Click" />

                    <asp:Button ID="btnTemizle" runat="server" Text="Temizle"
                        CssClass="btn-clear" OnClick="btnTemizle_Click" CausesValidation="false" />
                </div>
            </div>

            <div class="footer-text">
                <p>&copy; <%= DateTime.Now.Year %> T.C. Ulaştırma ve Altyapı Bakanlığı Ankara II.Bölge Müdürlüğü </p>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
