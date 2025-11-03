<%@ Page Title="Şifre Değiştir" Language="C#" MasterPageFile="~/AnaV2.Master" 
    AutoEventWireup="true" CodeBehind="SifreDegistir.aspx.cs" 
    Inherits="Portal.SifreDegistir" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card shadow">
                    <div class="card-header bg-warning text-white">
                        <h5 class="mb-0">
                            <i class="fas fa-key me-2"></i>Şifre Değiştirme Zorunluluğu
                        </h5>
                    </div>
                    <div class="card-body">
                        <div class="alert alert-info">
                            <i class="fas fa-info-circle me-2"></i>
                            Güvenliğiniz için lütfen şifrenizi değiştiriniz.
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Mevcut Şifre</label>
                            <asp:TextBox ID="txtMevcutSifre" runat="server" 
                                CssClass="form-control" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvMevcut" runat="server" 
                                ControlToValidate="txtMevcutSifre" 
                                ErrorMessage="Mevcut şifre gereklidir" 
                                CssClass="text-danger small" Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Yeni Şifre</label>
                            <asp:TextBox ID="txtYeniSifre" runat="server" 
                                CssClass="form-control" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvYeni" runat="server" 
                                ControlToValidate="txtYeniSifre" 
                                ErrorMessage="Yeni şifre gereklidir" 
                                CssClass="text-danger small" Display="Dynamic">
                            </asp:RequiredFieldValidator>
                            <small class="text-muted">En az 6 karakter olmalıdır</small>
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Yeni Şifre (Tekrar)</label>
                            <asp:TextBox ID="txtYeniSifreTekrar" runat="server" 
                                CssClass="form-control" TextMode="Password"></asp:TextBox>
                            <asp:CompareValidator ID="cvSifre" runat="server" 
                                ControlToValidate="txtYeniSifreTekrar" 
                                ControlToCompare="txtYeniSifre" 
                                ErrorMessage="Şifreler eşleşmiyor" 
                                CssClass="text-danger small" Display="Dynamic">
                            </asp:CompareValidator>
                        </div>

                        <div class="d-grid">
                            <asp:Button ID="btnDegistir" runat="server" Text="🔒 Şifremi Değiştir" 
                                CssClass="btn btn-warning" OnClick="btnDegistir_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>