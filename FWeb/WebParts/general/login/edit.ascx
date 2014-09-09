<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="edit.ascx.cs" Inherits="FWeb.WebParts.general.login.edit" %>
<ul class="adminformlist">
    <li>
        <asp:Label runat="server" ID="Label2" AssociatedControlID="txtForm" CssClass="jform_name-lbl"
            Text="Form"></asp:Label>
        <asp:TextBox runat="server" ID="txtForm" TextMode="MultiLine" Rows="10" Columns="50"></asp:TextBox>
        <br />
    </li>
        <li>
        <asp:Label runat="server" ID="Label3" AssociatedControlID="txtErrorMessage" CssClass="jform_name-lbl"
            Text="Form"></asp:Label>
        <asp:TextBox runat="server" ID="txtErrorMessage" TextMode="MultiLine" Rows="10" Columns="50"></asp:TextBox>
        <br />
    </li>
    <li>
        <asp:Label runat="server" ID="Label1" AssociatedControlID="txtReturnUrl" CssClass="jform_name-lbl"
            Text="Redrict Url"></asp:Label>
        <asp:TextBox runat="server" ID="txtReturnUrl" Width="300"></asp:TextBox>
        <br />
    </li>
</ul>
