<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="edit.ascx.cs" Inherits="FWeb.WebParts.general.commentbox.edit" %>
<ul class="adminformlist">
    <li>
        <asp:Label runat="server" ID="Label2" AssociatedControlID="drlContentType" CssClass="jform_name-lbl"
            Text="Content Type*"></asp:Label>
        <asp:DropDownList runat="server" ID="drlContentType" AutoPostBack="false" />
    </li>
    <li>
        <asp:Label runat="server" ID="llbl1" AssociatedControlID="drlParameterType" CssClass="jform_name-lbl"
            Text="Parameter Type*"></asp:Label>
        <asp:DropDownList runat="server" ID="drlParameterType">
            <asp:ListItem Value="1">Query String</asp:ListItem>
            <asp:ListItem Value="2">Seo Teamplate</asp:ListItem>
            <asp:ListItem Value="3">Cookie</asp:ListItem>
            <asp:ListItem Value="4">User Name</asp:ListItem>
        </asp:DropDownList>
    </li>
    <li>
        <asp:Label runat="server" ID="Label1" AssociatedControlID="txtParameterName" CssClass="jform_name-lbl"
            Text="Parameter Type*"></asp:Label>
        <asp:TextBox runat="server" ID="txtParameterName"></asp:TextBox>
    </li>
    <li>
        <asp:CheckBox runat="server" ID="chbxAnonym" Text="Allow anonym comments" />
    </li>
</ul>
<div class="clr">
</div>
