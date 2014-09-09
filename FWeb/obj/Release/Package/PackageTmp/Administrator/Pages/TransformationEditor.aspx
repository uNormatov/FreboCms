<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransformationEditor.aspx.cs"
    Inherits="FWeb.Administrator.Pages.TransformationEditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Content/js/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
    <script src="/Content/js/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="/Content/css/template.css" rel="stylesheet" type="text/css" />
    <link href="/Content/css/system.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function closePopup() {
            parent.$.fancybox.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="system-message-container">
        <asp:Literal runat="server" ID="ltlMessage"></asp:Literal>
    </div>
    <div id="element-box">
        <div class="m">
            <div class="width-100 fltlft">
                <div style="margin-left: 510px;">
                    <asp:Button runat="server" ID="btnSave" Text="Save" Width="70" Height="20" OnClick="btnSave_OnClick" />
                    <input type="button" value="Cancel" style="width: 70px; height: 20px; cursor: pointer;"
                        onclick="closePopup();" />
                </div>
                <fieldset class="adminform">
                    <legend>Transormation Details</legend>
                    <ul class="adminformlist">
                        <li>
                            <asp:Label runat="server" ID="llbl1" AssociatedControlID="txtName" CssClass="jform_name-lbl"
                                Text="Name *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtName" CssClass="required" Width="200" Enabled="false"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label5" AssociatedControlID="txtQuery" CssClass="jform_name-lbl"
                                Text="Transformation Text"></asp:Label>
                            <asp:TextBox runat="server" ID="txtQuery" CssClass="required" TextMode="MultiLine"
                                Height="150" Width="400"></asp:TextBox>
                            <br />
                        </li>
                    </ul>
                </fieldset>
            </div>
            <div class="clr">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
