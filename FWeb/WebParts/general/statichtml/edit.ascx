<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="edit.ascx.cs" Inherits="FWeb.WebParts.statichtml.edit" %>
<ul class="adminformlist">
    <li>
        <asp:Label runat="server" ID="llbl1" AssociatedControlID="txtHtml" CssClass="jform_name-lbl"
            Text="Html *"></asp:Label>
        <asp:TextBox TextMode="MultiLine" Rows="18" CssClass="required" ID="txtHtml" runat="server"
            Width="435" onkeydown="return TabKeyTextArea(event, 'ctl00_MainContent_txtDocType', '    ');">
        </asp:TextBox>
    </li>
</ul>
<div class="clr"></div>
