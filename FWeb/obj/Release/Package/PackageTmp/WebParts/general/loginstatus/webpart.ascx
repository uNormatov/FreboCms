<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="webpart.ascx.cs" Inherits="FWeb.WebParts.general.loginstatus.webpart" %>
<% if (!Page.User.Identity.IsAuthenticated)
   {
%>
<a id="login" href="<%= BuildUrl("/login") %>"><%= GetResource("lblLogin")%></a>
<% }
   else
   { %>
<%= GetResource("lblXushKelibsiz")%> <a href="<%= BuildUrl("/profil") %>">
    <%= Page.User.Identity.Name %></a> | <a href="/login?action=signout"><%= GetResource("lblChiqish")%></a>
<% } %>