<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="_152120191023_WebBasedTechnologies_Hw4.Dashboard" %>


        <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblWelcome" runat="server" CssClass="text-warning justify-content-center fw-bold"></asp:Label>
     <div class="d-flex justify-content-center align-items-center py-5">
  <!-- Profil ikonu -->
  <asp:HyperLink ID="hlProfile" runat="server"
      NavigateUrl="Profile.aspx"
      CssClass="mx-5">
    <span class="icon-container rounded-circle p-2 d-inline-block">
      <img src="Images/user.png"
           alt="Profil"
           class="img-fluid icon-warning p-3"
           style="width:8rem; height:auto;" />
    </span>
  </asp:HyperLink>

  <!-- Chat ikonu -->
  <asp:HyperLink ID="hlChat" runat="server"
      NavigateUrl="Chat.aspx"
      CssClass="mx-5">
    <span class="icon-container rounded-circle p-2 d-inline-block">
      <img src="Images/chat.png"
           alt="Chat"
           class="img-fluid icon-warning p-3"
           style="width:8rem; height:auto;" />
    </span>
  </asp:HyperLink>
</div>

</asp:Content>

    