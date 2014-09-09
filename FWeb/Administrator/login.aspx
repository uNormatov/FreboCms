<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="FWeb.Administrator.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="/Content/css/template.css" rel="stylesheet" type="text/css" />
    <link href="/Content/css/system.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="h_blue" id="border-top">
        <span class="title"><a href="index.php">Adminstrator</a></span>
    </div>
    <div id="content-box">
        <div id="element-box" class="login">
            <asp:Panel runat="server" ID="errorDiv" Visible="false">
                <div id="system-message-container">
                    <dl id="system-message">
                        <dt class="error">Error</dt>
                        <dd class="error message">
                            <ul>
                                <li>Username and password do not match or you do not have an account yet.</li>
                            </ul>
                        </dd>
                    </dl>
                </div>
            </asp:Panel>
            <div id="section-box">
                <div class="m">
                    <h1>
                        Login</h1>
                    <form id="form1" runat="server">
                    <%--  <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false">
                        <LayoutTemplate>--%>
                    <fieldset class="loginform">
                        <div>
                            <label class="mod-login-username-lbl">
                                User Name</label>
                            <asp:TextBox runat="server" ID="UserName" TextMode="SingleLine" CssClass="inputbox"></asp:TextBox>
                        </div>
                        <div>
                            <label class="mod-login-username-lbl">
                                Password</label>
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="inputbox"></asp:TextBox>
                        </div>
                        <div class="button-holder">
                            <div class="button1">
                                <asp:Button runat="server" ID="LoginButton" Text="Log In" OnClick="Login_OnClick" />
                            </div>
                        </div>
                    </fieldset>
                    <%--   </LayoutTemplate>
                    </asp:Login>--%>
                    </form>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
