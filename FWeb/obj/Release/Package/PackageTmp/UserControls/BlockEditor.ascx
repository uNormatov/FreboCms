<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlockEditor.ascx.cs"
    Inherits="FWeb.UserControls.BlockEditor" %>
<div class="width-40 fltlft">
    <fieldset class="adminform">
        <legend>Block Details</legend>
        <ul class="adminformlist">
            <li>
                <asp:Label runat="server" ID="llbl1" AssociatedControlID="txtName" CssClass="jform_name-lbl"
                    Text="Name *"></asp:Label>
                <asp:TextBox runat="server" ID="txtName" CssClass="required" Width="200"></asp:TextBox>
            </li>
            <li>
                <asp:Label runat="server" ID="lblStructureType" AssociatedControlID="drlStructureItems"
                    CssClass="jform_name-lbl" Text="Page *"></asp:Label>
                <asp:DropDownList runat="server" ID="drlStructureItems" CssClass="required" Width="200"
                    DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="drlStructureItems_OnSelectedIndexChanged"
                    AutoPostBack="true" />
                <br />
            </li>
            <li>
                <asp:Label runat="server" ID="Label5" AssociatedControlID="drlWebPart" CssClass="jform_name-lbl"
                    Text="Web Part *"></asp:Label>
                <asp:DropDownList runat="server" ID="drlWebPart" CssClass="required" Width="200"
                    DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="drlWebPart_OnSelectedIndexChanged"
                    AutoPostBack="true" AppendDataBoundItems="true">
                    <asp:ListItem Value="" Text="Please Select"></asp:ListItem>
                </asp:DropDownList>
                <br />
            </li>
            <li>
                <asp:Label runat="server" ID="Label2" AssociatedControlID="drlOrder" CssClass="jform_name-lbl"
                    Text="Order *"></asp:Label>
                <asp:DropDownList runat="server" ID="drlOrder" CssClass="required" Width="200" DataTextField="Name"
                    DataValueField="Id">
                    <asp:ListItem Text="1" Value="1" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                    <asp:ListItem Text="6" Value="6"></asp:ListItem>
                    <asp:ListItem Text="7" Value="7"></asp:ListItem>
                    <asp:ListItem Text="8" Value="8"></asp:ListItem>
                    <asp:ListItem Text="9" Value="9"></asp:ListItem>
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="11" Value="11"></asp:ListItem>
                    <asp:ListItem Text="12" Value="12"></asp:ListItem>
                    <asp:ListItem Text="13" Value="13"></asp:ListItem>
                    <asp:ListItem Text="14" Value="14"></asp:ListItem>
                    <asp:ListItem Text="15" Value="15"></asp:ListItem>
                </asp:DropDownList>
                <br />
            </li>
        </ul>
    </fieldset>
</div>
<div class="width-60 fltlft">
    <fieldset class="adminform">
        <legend>Webpart Details</legend>
        <ul class="adminformlist">
            <li>
                <asp:Label runat="server" ID="Label1" AssociatedControlID="drlWebPartZoneName" CssClass="jform_name-lbl"
                    Text="WebPart Zone *"></asp:Label>
                <asp:DropDownList runat="server" ID="drlWebPartZoneName" CssClass="required" Width="200"
                    DataTextField="Name" DataValueField="Name" />
            </li>
            <li>
                <asp:Panel runat="server" ID="pnlWebPart">
                </asp:Panel>
            </li>
        </ul>
    </fieldset>
</div>
<div class="clr">
</div>
