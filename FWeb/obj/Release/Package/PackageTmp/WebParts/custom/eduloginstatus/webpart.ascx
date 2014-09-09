<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="webpart.ascx.cs" Inherits="FWeb.WebParts.custom.eduloginstatus.webpart" %>
<% if (!Page.User.Identity.IsAuthenticated)
   {%>
<span style="display: inline;">
    <%= GetResource("lblXushKelibsiz")%>
    | <a id="login" href="<%= BuildUrl("/login") %>">
        <%= GetResource("lblKirish")%></a></span>
<% }
   else
   {
       string url = BuildUrl("/profil");
       if (Roles.IsUserInRole(Page.User.Identity.Name, "superuser") || Roles.IsUserInRole(Page.User.Identity.Name, "editor"))
       {
           url = "/administrator";
       } %>
<span>
    <%= GetResource("lblXushKelibsiz")%>
    <b><a href="<%= url %>">
        <%= Page.User.Identity.Name %></a> </b><span>| <a href="<%= BuildUrl("/login?action=signout") %>">
            <%= GetResource("lblChiqish")%></a></span></span>
<% } %>