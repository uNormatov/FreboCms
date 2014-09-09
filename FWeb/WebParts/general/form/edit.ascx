<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="edit.ascx.cs" Inherits="FWeb.WebParts.general.form.edit" %>
<%@ Import Namespace="FCore.Constant" %>
<script src="/Content/js/frebo.js" type="text/javascript"></script>
<script type="text/javascript">
    function fillForms() {
        $.ajax({
            type: "GET",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            url: "/jsonhandler.ashx",
            data: "type=<%= SiteConstants.JsonFormsByContentTypeId %>&id=" + $('#<%= drlContentType.ClientID %>').val(),
            success: function (resp) {
                var formsSelect = $("#<%= drlForms.ClientID %>");
                $('option', formsSelect).remove();
                if (resp != null) {
                    $.each(resp, function (i, item) {
                        if (i == 0)
                            formsSelect.append("<option selected value='" + item.Id + "'>" + item.DisplayName + "</option>");
                        else
                            formsSelect.append("<option value='" + item.Id + "'>" + item.DisplayName + "</option>");
                    });
                }
            }
        });
    }
</script>
<ul class="adminformlist">
    <li>
        <asp:Label runat="server" ID="Label2" AssociatedControlID="drlContentType" CssClass="jform_name-lbl"
            Text="Content Type*"></asp:Label>
        <asp:DropDownList runat="server" ID="drlContentType" CssClass="required" Width="200"
            DataTextField="Name" DataValueField="Id" onchange="fillForms()" />
        <br />
    </li>
    <li>
        <asp:Label runat="server" ID="Label1" AssociatedControlID="drlForms" CssClass="jform_name-lbl"
            Text="Forms *"></asp:Label>
        <asp:DropDownList runat="server" ID="drlForms" CssClass="required" Width="200" DataTextField="DisplayName"
            DataValueField="Id" />
        <br />
    </li>
    <li>
        <asp:Label runat="server" ID="llbl1" AssociatedControlID="txtQueryName" CssClass="jform_name-lbl"
            Text="Select Query Name*"></asp:Label>
        <asp:TextBox Rows="18" CssClass="required" ID="txtQueryName" runat="server" Width="435"
            onkeydown="return TabKeyTextArea(event, 'ctl00_MainContent_txtDocType', '    ');">
        </asp:TextBox>
        <asp:Literal runat="server" ID="ltlQuerySelector"></asp:Literal>
    </li>
    <li>
        <fieldset>
            <legend>Query Paramaters</legend>
            <ul class="adminformlist" id="ulmultiple">
                <asp:Literal runat="server" ID="ltlQueryParams"></asp:Literal>
            </ul>
        </fieldset>
    </li>
    <li>
        <asp:Label runat="server" ID="Label4" AssociatedControlID="txtReturnUrl" CssClass="jform_name-lbl"
            Text="Return Url"></asp:Label>
        <asp:TextBox runat="server" ID="txtReturnUrl"></asp:TextBox>
    </li>
    <li>
        <asp:Label runat="server" ID="Label3" AssociatedControlID="txtSaveButtonText" CssClass="jform_name-lbl"
            Text="Save Button Text *"></asp:Label>
        <asp:TextBox runat="server" ID="txtSaveButtonText"></asp:TextBox>
    </li>
     <li>
        <asp:Label runat="server" ID="Label5" AssociatedControlID="txtCancelButtonText" CssClass="jform_name-lbl"
            Text="Cancel Button Text *"></asp:Label>
        <asp:TextBox runat="server" ID="txtCancelButtonText"></asp:TextBox>
    </li>
</ul>
<script type="text/javascript" src="/Content/js/dataviewerparameters.js"></script>