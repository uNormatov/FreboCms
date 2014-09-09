<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditorMenu.ascx.cs"
    Inherits="FWeb.UserControls.EditorMenu" %>
<div id="module-menu">
    <%if (Request.QueryString["type"] == null || Request.QueryString["type"].ToLower() != "entry")
      {%>
    <ul id="menu">
        <li class="node"><a href="#">Main</a>
            <ul>
                <li><a href="/administrator/Default.aspx" class="icon-16-cpanel">Dashboard</a></li>
                <li class="separator"><span></span></li>
                <li><a href="/administrator/login.aspx?action=logout" class="icon-16-logout">Logout</a></li>
            </ul>
        </li>
        <li class="node"><a href="/administrator/list">Lists</a></li>
        <asp:Literal runat="server" ID="ltlContentMenu"></asp:Literal>
    </ul>
    <%}
      else
      { %>
    <ul id="menu" class="disabled">
        <li class="disabled"><a>List</a> </li>
        <asp:Literal runat="server" ID="ltlHiddenMenu"></asp:Literal>
    </ul>
    <%} %>
</div>
