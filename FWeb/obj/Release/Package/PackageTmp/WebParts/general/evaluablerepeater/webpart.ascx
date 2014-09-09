<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="webpart.ascx.cs" Inherits="FWeb.WebParts.general.evaluablerepeater.webpart" %>
<asp:Literal runat="server" ID="beforeContainer"></asp:Literal>
<fr:FEvaluableRepeater runat="server" ID="rptRepeater" />
<asp:Literal runat="server" ID="afterContainer"></asp:Literal>