<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="edit.ascx.cs" Inherits="FWeb.WebParts.general.pager.edit" %>
<script src="/Content/js/frebo.js" type="text/javascript"></script>
<ul class="adminformlist">
    <li>
        <asp:Label runat="server" ID="llbl1" AssociatedControlID="txtQueryName" CssClass="jform_name-lbl"
            Text="Total Query Name*"></asp:Label>
        <asp:TextBox Rows="18" CssClass="required" ID="txtQueryName" runat="server" Width="300"
            onkeydown="return TabKeyTextArea(event, 'ctl00_MainContent_txtDocType', '    ');">
        </asp:TextBox>
        <asp:Literal runat="server" ID="ltlQuerySelector"></asp:Literal>
    </li>
    <li>
        <asp:Label runat="server" ID="Label1" AssociatedControlID="txtPageSize" CssClass="jform_name-lbl"
            Text="Page Size*"></asp:Label>
        <asp:TextBox Rows="18" CssClass="required" ID="txtPageSize" runat="server" Width="300"
            onkeydown="return TabKeyTextArea(event, 'ctl00_MainContent_txtDocType', '    ');">
        </asp:TextBox>
    </li>
    <li>
        <asp:Label runat="server" ID="Label3" AssociatedControlID="txtPageIndexKeyword" CssClass="jform_name-lbl"
            Text="Page Index Keyword*"></asp:Label>
        <asp:TextBox Rows="18" CssClass="required" ID="txtPageIndexKeyword" runat="server"
            Width="300" onkeydown="return TabKeyTextArea(event, 'ctl00_MainContent_txtDocType', '    ');">
        </asp:TextBox>
    </li>
    <li>
        <asp:Label runat="server" ID="Label2" AssociatedControlID="drlPageIndexType" CssClass="jform_name-lbl"
            Text="Page Index Keyword Type*"></asp:Label>
        <asp:DropDownList runat="server" ID="drlPageIndexType">
            <asp:ListItem Selected="true" Value="1">Query String</asp:ListItem>
            <asp:ListItem Value="2">Seo Template</asp:ListItem>
        </asp:DropDownList>
    </li>
    <li>
        <asp:Label runat="server" ID="Label6" AssociatedControlID="drlContainerTag" CssClass="jform_name-lbl"
            Text="Container Tag*"></asp:Label>
        <asp:DropDownList runat="server" ID="drlContainerTag">
            <Items>
                <asp:ListItem Value="ul">UL</asp:ListItem>
                <asp:ListItem Value="div">DIV</asp:ListItem>
            </Items>
        </asp:DropDownList>
    </li>
    <li>
        <asp:Label runat="server" ID="Label7" AssociatedControlID="txtContainerCss" CssClass="jform_name-lbl"
            Text="Container Css"></asp:Label>
        <asp:TextBox ID="txtContainerCss" runat="server" Width="300">
        </asp:TextBox>
    </li>
    <li>
        <asp:Label runat="server" ID="Label8" AssociatedControlID="drlInnerContainerTag"
            CssClass="jform_name-lbl" Text="Inner Container Tag*"></asp:Label>
        <asp:DropDownList runat="server" ID="drlInnerContainerTag">
            <Items>
                <asp:ListItem Value="li">LI</asp:ListItem>
                <asp:ListItem Value="span">SPAN</asp:ListItem>
            </Items>
        </asp:DropDownList>
    </li>
    <li>
        <asp:Label runat="server" ID="Label9" AssociatedControlID="txtInnerContainerCss"
            CssClass="jform_name-lbl" Text="Inner Container Css"></asp:Label>
        <asp:TextBox ID="txtInnerContainerCss" runat="server" Width="300">
        </asp:TextBox>
    </li>
     <li>
        <asp:Label runat="server" ID="Label10" AssociatedControlID="txtActiveCss"
            CssClass="jform_name-lbl" Text="Active Item Css"></asp:Label>
        <asp:TextBox ID="txtActiveCss" runat="server" Width="300">
        </asp:TextBox>
    </li>
    <li>
        <asp:Label runat="server" ID="Label4" AssociatedControlID="txtPrev" CssClass="jform_name-lbl"
            Text="Prev Text*"></asp:Label>
        <asp:TextBox Rows="18"  ID="txtPrev" runat="server" Width="300"
            onkeydown="return TabKeyTextArea(event, 'ctl00_MainContent_txtDocType', '    ');">
        </asp:TextBox>
    </li>
    <li>
        <asp:Label runat="server" ID="Label5" AssociatedControlID="txtNext" CssClass="jform_name-lbl"
            Text="Next Text*"></asp:Label>
        <asp:TextBox Rows="18" ID="txtNext" runat="server" Width="300"
            onkeydown="return TabKeyTextArea(event, 'ctl00_MainContent_txtDocType', '    ');">
        </asp:TextBox>
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
<script type="text/javascript" src="/Content/js/dataviewerparameters.js"></script>
