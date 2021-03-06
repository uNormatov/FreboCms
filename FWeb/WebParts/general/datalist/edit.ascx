﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="edit.ascx.cs" Inherits="FWeb.WebParts.general.datalist.edit" %>
<script src="/Content/js/frebo.js" type="text/javascript"></script>
<ul class="adminformlist">
    <li>
        <asp:Label runat="server" ID="Label1" AssociatedControlID="txtBeforeContainer" CssClass="jform_name-lbl"
            Text="Before Container"></asp:Label>
        <asp:TextBox TextMode="MultiLine" Rows="10" ID="txtBeforeContainer" runat="server"
            Width="300" onkeydown="return TabKeyTextArea(event, 'ctl00_MainContent_txtDocType', '    ');">
        </asp:TextBox>
    </li>
    <li>
        <asp:Label runat="server" ID="Label8" AssociatedControlID="txtRowBefore" CssClass="jform_name-lbl"
            Text="Row Before"></asp:Label>
        <asp:TextBox ID="txtRowBefore" runat="server" Width="300" onkeydown="return TabKeyTextArea(event, 'ctl00_MainContent_txtDocType', '    ');">
        </asp:TextBox>
    </li>
    <li>
        <asp:Label runat="server" ID="Label2" AssociatedControlID="txtTransformation" CssClass="jform_name-lbl"
            Text="Transformation*"></asp:Label>
        <asp:TextBox CssClass="required" ID="txtTransformation" runat="server" Width="300"
            onkeydown="return TabKeyTextArea(event, 'ctl00_MainContent_txtDocType', '    ');">
        </asp:TextBox>
        <asp:Literal runat="server" ID="ltlTransformationSelect"></asp:Literal>
    </li>
    <li>
        <asp:Label runat="server" ID="Label3" AssociatedControlID="txtAlterTransformation"
            CssClass="jform_name-lbl" Text="Alter Transformation*"></asp:Label>
        <asp:TextBox Rows="10" CssClass="required" ID="txtAlterTransformation" runat="server"
            Width="300" onkeydown="return TabKeyTextArea(event, 'ctl00_MainContent_txtDocType', '    ');">
        </asp:TextBox>
        <asp:Literal runat="server" ID="ltlAlterTransformationSelect"></asp:Literal>
    </li>
    <asp:Label runat="server" ID="Label9" AssociatedControlID="txtRowAfter" CssClass="jform_name-lbl"
        Text="Row After"></asp:Label>
    <asp:TextBox ID="txtRowAfter" runat="server" Width="300" onkeydown="return TabKeyTextArea(event, 'ctl00_MainContent_txtDocType', '    ');">
    </asp:TextBox>
    <li>
        <asp:Label runat="server" ID="Label4" AssociatedControlID="txtAfterContainer" CssClass="jform_name-lbl"
            Text="After Container"></asp:Label>
        <asp:TextBox TextMode="MultiLine" Rows="10" ID="txtAfterContainer" runat="server"
            Width="300" onkeydown="return TabKeyTextArea(event, 'ctl00_MainContent_txtDocType', '    ');">
        </asp:TextBox>
    </li>
    <li>
        <asp:Label runat="server" ID="Label5" AssociatedControlID="drlRepeatColumns" CssClass="jform_name-lbl"
            Text="Repeat Columns *"></asp:Label>
        <asp:DropDownList runat="server" ID="drlRepeatColumns">
            <asp:ListItem Value="0">Please Select</asp:ListItem>
            <asp:ListItem Value="1">1</asp:ListItem>
            <asp:ListItem Value="2">2</asp:ListItem>
            <asp:ListItem Value="3">3</asp:ListItem>
            <asp:ListItem Value="4">4</asp:ListItem>
            <asp:ListItem Value="5">5</asp:ListItem>
            <asp:ListItem Value="6">6</asp:ListItem>
            <asp:ListItem Value="7">7</asp:ListItem>
            <asp:ListItem Value="8">8</asp:ListItem>
            <asp:ListItem Value="9">9</asp:ListItem>
            <asp:ListItem Value="10">10</asp:ListItem>
        </asp:DropDownList>
    </li>
    <li>
        <asp:Label runat="server" ID="llbl1" AssociatedControlID="txtQueryName" CssClass="jform_name-lbl"
            Text="Query Name*"></asp:Label>
        <asp:TextBox Rows="18" CssClass="required" ID="txtQueryName" runat="server" Width="300"
            onkeydown="return TabKeyTextArea(event, 'ctl00_MainContent_txtDocType', '    ');">
        </asp:TextBox>
        <asp:Literal runat="server" ID="ltlQuerySelector"></asp:Literal>
    </li>
    <li>
        <fieldset style="float: left;">
            <legend>Query Paramaters</legend>
            <ul class="adminformlist" id="ulmultiple">
                <asp:Literal runat="server" ID="ltlQueryParams"></asp:Literal>
            </ul>
        </fieldset>
    </li>
</ul>
<div class="clr">
</div>
<script type="text/javascript" src="/Content/js/dataviewerparameters.js"> </script>
