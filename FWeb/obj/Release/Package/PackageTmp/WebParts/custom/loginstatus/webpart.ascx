<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="webpart.ascx.cs" Inherits="FWeb.WebParts.custom.loginstatus.webpart" %>
<% if (!Page.User.Identity.IsAuthenticated)
   {
%>
<a id="login" href="/login">Kirish/Ro'yxatdan o'tish</a>
<% }
   else
   { %>
Xush kelibsiz <a href="/profile">
    <%= Page.User.Identity.Name %></a> | <a href="/login?action=signout">Chiqish</a>
<% } %>