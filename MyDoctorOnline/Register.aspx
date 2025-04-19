<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" 
    CodeBehind="Register.aspx.cs" Inherits="_152120191023_WebBasedTechnologies_Hw4.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent1" runat="server">
    <h2 class="text-center mb-3 text-warning">Kayıt Ol</h2>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    
    <asp:TextBox ID="txtUsername" runat="server" 
        CssClass="form-control mb-3 text-primary border border-primary w-50 fw-bold" 
        Placeholder="Kullanıcı Adı"></asp:TextBox>
        
    <asp:TextBox ID="txtPassword" runat="server" 
        CssClass="form-control mb-3 text-primary border border-primary w-50 fw-bold" 
        TextMode="Password" Placeholder="Şifre"></asp:TextBox>
        
    <asp:TextBox ID="txtFullName" runat="server" 
        CssClass="form-control mb-3 text-primary border border-primary w-50 fw-bold" 
        Placeholder="Ad Soyad"></asp:TextBox>
        
    <asp:TextBox ID="txtEmail" runat="server" 
        CssClass="form-control mb-3 text-primary border border-primary w-50 fw-bold" 
        Placeholder="Email"></asp:TextBox>
    
    <asp:Label ID="lblRole" runat="server" Text="Rol Seçimi:" CssClass="d-block mb-2 fw-bold text-warning text-center"></asp:Label>
    
    <!-- Radio butonları tek bir RadioButtonList içinde, yan yana görüntülenecek -->
    <asp:RadioButtonList ID="rblRole" runat="server" CssClass="asp-RadioButtonList mb-3" 
        RepeatLayout="Flow" RepeatDirection="Horizontal">
        <asp:ListItem Value="Doctor" Attributes-CssClass="doctorChoice">Doctor</asp:ListItem>
        <asp:ListItem Value="Patient" Attributes-CssClass="patientChoice">Patient</asp:ListItem>
    </asp:RadioButtonList>
    
    <asp:Button ID="btnRegister" runat="server" 
        CssClass="btn btn-primary btn-lg w-40 mx-auto d-block mb-3" 
        Text="Kayıt Ol" OnClick="btnRegister_Click" />
    
    <a href="Login.aspx" 
       class="btn btn-warning btn-sm rounded text-white mb-3 d-block text-center">
        Zaten Hesabım Var, Giriş Yap
    </a>
    
    <script src="Scripts/jquery-1.6.4.min.js"></script>
    <script>
        $(document).ready(function () {
            $("body").addClass("login-page");
        });
    </script>
    
    
      
</asp:Content>
