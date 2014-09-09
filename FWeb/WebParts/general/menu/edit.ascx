<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="edit.ascx.cs" Inherits="FWeb.WebParts.general.menu.edit" %>
<ul class="adminformlist">
    <li>
        <asp:Label runat="server" ID="Label2" AssociatedControlID="drlMenu" CssClass="jform_name-lbl"
            Text="Menu *"></asp:Label>
        <asp:DropDownList runat="server" ID="drlMenu" CssClass="required" Width="200" DataTextField="Name"
            DataValueField="Id" />
        <br />
    </li>
    <li>
        <asp:Label runat="server" ID="Label1" AssociatedControlID="txtContainerCss" CssClass="jform_name-lbl"
            Text="Container Css"></asp:Label>
        <asp:TextBox runat="server" ID="txtContainerCss"></asp:TextBox>
        <br />
    </li>
    <li>
        <asp:Label runat="server" ID="Label3" AssociatedControlID="txtItemCss" CssClass="jform_name-lbl"
            Text="Item Css"></asp:Label>
        <asp:TextBox runat="server" ID="txtItemCss"></asp:TextBox>
        <br />
    </li>
    <li>
        <asp:Label runat="server" ID="Label4" AssociatedControlID="txtSelectedItemCss" CssClass="jform_name-lbl"
            Text="Active Item Css"></asp:Label>
        <asp:TextBox runat="server" ID="txtSelectedItemCss"></asp:TextBox>
        <br />
    </li>
     <li>
        <asp:Label runat="server" ID="Label5" AssociatedControlID="txtChildContainerCss" CssClass="jform_name-lbl"
            Text="Child Container Css"></asp:Label>
        <asp:TextBox runat="server" ID="txtChildContainerCss"></asp:TextBox>
        <br />
    </li>
     <li>
        <asp:Label runat="server" ID="Label6" AssociatedControlID="txtChildItemCss" CssClass="jform_name-lbl"
            Text="Child Item Css"></asp:Label>
        <asp:TextBox runat="server" ID="txtChildItemCss"></asp:TextBox>
        <br />
    </li>
</ul>
