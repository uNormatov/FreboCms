<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="edit.ascx.cs" Inherits="FWeb.WebParts.general.breadcrumb.edit" %>
<ul class="adminformlist">
    <li>
        <asp:Label runat="server" ID="Label1" AssociatedControlID="txtSeparator" CssClass="jform_name-lbl"
            Text="Separator"></asp:Label>
        <asp:TextBox ID="txtSeparator" runat="server" Width="200">
        </asp:TextBox>
    </li>
</ul>
