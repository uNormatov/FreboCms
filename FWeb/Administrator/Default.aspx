<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="FWeb.Administrator.Default" %>

<%@ Import Namespace="FUIControls.Settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Dashboard |
        <%= CoreSettings.CurrentSite.Name %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <div id="element-box">
        <div id="system-message-container">
        </div>
        <div class="m">
            <div class="adminform">
                <%
                    if (Page.User.IsInRole("superuser"))
                    {%>
                <%@ register tagprefix="fr" tagname="MainDashboard" src="~/UserControls/MainDashboard.ascx" %>
                <fr:MainDashboard runat="server" ID="MainDashboard" />
                <%}
                    else if (Page.User.IsInRole("editor"))
                    {%>
                <%@ register tagprefix="fr" tagname="EditorDashbaord" src="~/UserControls/EditorDashboard.ascx" %>
                <fr:EditorDashbaord runat="server" ID="EditorMenu" />
                <% }%>
            </div>
            <div class="clr">
            </div>
        </div>
    </div>
</asp:Content>
