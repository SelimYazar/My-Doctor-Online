<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" 
         CodeBehind="Login.aspx.cs" Inherits="_152120191023_WebBasedTechnologies_Hw4.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent1" runat="server">
    <h2 class="text-center mb-3 text-warning">Giriş Yap</h2>
    <asp:TextBox ID="txtUsername" runat="server"
        CssClass="form-control mb-3  text-primary border border-primary w-50 fw-bold "
        Placeholder="Kullanıcı Adı"></asp:TextBox>

    <asp:TextBox ID="txtPassword" runat="server"
        CssClass="form-control mb-3  text-primary border border-primary w-50 fw-bold "
        TextMode="Password" Placeholder="Şifre"></asp:TextBox>

    <asp:Button ID="btnLogin" runat="server"
        Text="Giriş"
        CssClass="btn btn-primary btn-lg w-40 mx-auto d-block mb-3"
        OnClick="btnLogin_Click" />

    <div class="text-center">
        <a href="Register.aspx" class="btn btn-warning btn-sm rounded text-white mb-3">
            Hemen Kaydol
        </a>
    </div>

    <asp:Label ID="lblMessage" runat="server"
        CssClass="d-block text-center" ForeColor="Red"></asp:Label>
  <script src="Scripts/jquery-1.6.4.min.js"></script>

    <script>
        $(document).ready(function () {
            $("body").addClass("login-page");
        });

    </script>
</asp:Content>
