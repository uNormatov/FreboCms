<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="FWeb.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <span>
        <h4>
            Error!</h4>
    </span>
    <div style="margin: 30px; background-color: Yellow; height: 400px;">
        <asp:Literal runat="server" ID="ltlErrorMessage"></asp:Literal>
    </div>
    </form>
</body>
</html>
