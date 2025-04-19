<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="_152120191023_WebBasedTechnologies_Hw4.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="text-center mb-3 text-warning">Profil Sayfası</h2>

    <!-- Profil resmi gösterimi (rounded-pill) -->
    <asp:Image ID="imgProfile" runat="server" CssClass="rounded-pill mb-3" Width="150px" Height="150px" />

    <!-- Profil resmi yükleme -->
    <asp:FileUpload ID="fuProfilePic" runat="server" CssClass="btn btn-primary btn-lg w-50 mx-auto d-block mb-3" />
    <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary btn-lg w-40 mx-auto d-block mb-3" Text="Resim Yükle" OnClick="btnUpload_Click" />
    
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

    <hr />

    <!-- Kullanıcı Bilgilerini Düzenleme Alanı -->
    <div class="mb-2">
        <label class="text-primary fw-bold">Kullanıcı Adı</label>
        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control mb-3 text-primary border border-primary w-100 fw-bold" />
    </div>
    <div class="mb-2">
        <label class="text-primary fw-bold">Şifre</label>
        <asp:TextBox ID="txtPassword" runat="server"  CssClass="form-control mb-3 text-primary border border-primary w-100 fw-bold" />
    </div>
    <div class="mb-2">
        <label class="text-primary fw-bold">Ad Soyad</label>
        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control mb-3 text-primary border border-primary w-100 fw-bold" />
    </div>
    <div class="mb-2">
        <label class="text-primary fw-bold">Email</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control mb-3 text-primary border border-primary w-100 fw-bold" />
    </div>

    <asp:Button ID="btnSave" runat="server" Text="Kaydet" CssClass="btn btn-primary btn-lg w-40 mx-auto d-block mb-3" OnClick="btnSave_Click" />

    <!-- Gizli alanlar: Eski verileri saklamak için (değişiklik kıyaslaması yaparken kullanacağız) -->
    <asp:HiddenField ID="hfOldUsername" runat="server" />
    <asp:HiddenField ID="hfOldPassword" runat="server" />
    <asp:HiddenField ID="hfOldFullName" runat="server" />
    <asp:HiddenField ID="hfOldEmail" runat="server" />
</asp:Content>
