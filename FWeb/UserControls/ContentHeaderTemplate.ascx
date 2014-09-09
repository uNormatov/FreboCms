<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContentHeaderTemplate.ascx.cs"
    Inherits="FWeb.UserControls.ContentHeaderTemplate" %>
<table class="adminlist">
    <thead>
        <tr>
            <th width="1%">
                <input type="checkbox" title="Check All" value="" name="checkall-toggle" onclick="Frebo.selectAll(this)">
            </th>
        </tr>
        <asp:Literal runat="server" ID="ltlHeaderList"></asp:Literal>
    </thead>
