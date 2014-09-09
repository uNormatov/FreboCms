<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="action.aspx.cs" Inherits="FWeb.Administrator.Page.action" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Content/js/jquery-ui-1.9.1.custom.min.js" type="text/javascript"></script>
    <link href="/Content/css/jquery-ui-1.9.1.custom.min.css" rel="stylesheet" type="text/css" />
    <script src="/Content/js/frebo.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <div id="toolbar-box">
        <div class="m">
            <div id="toolbar" class="toolbar-list">
                <ul>
                    <li id="toolbar-apply" class="button"><a class="toolbar" onclick="Frebo.formsubmit('save',true)"
                        href="#"><span class="icon-32-save"></span>Save </a></li>
                    <li id="toolbar-cancel" class="button"><a class="toolbar" onclick="Frebo.formsubmit('cancel')"
                        href="#"><span class="icon-32-cancel"></span>Cancel </a></li>
                </ul>
                <div class="clr">
                </div>
            </div>
            <div class="pagetitle icon-48-sitelayoutmgr">
                <h2>Page Manager:
                    <asp:Literal runat="server" ID="ltlTitle"></asp:Literal></h2>
            </div>
        </div>
    </div>
    <div id="system-message-container">
        <asp:Literal runat="server" ID="ltlMessage"></asp:Literal>
    </div>
    <div id="element-box">
        <div class="m">
            <div class="width-50 fltlft">
                <fieldset class="adminform">
                    <legend>Page Details</legend>
                    <ul class="adminformlist">
                        <li>
                            <asp:Label runat="server" ID="llbl1" AssociatedControlID="txtName" CssClass="jform_name-lbl"
                                Text="Name *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtName" CssClass="required" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label3" AssociatedControlID="txtTitle" CssClass="jform_name-lbl"
                                Text="Title *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtTitle" CssClass="required" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label5" AssociatedControlID="txtSeoTemplate" CssClass="jform_name-lbl"
                                Text="Seo Template *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtSeoTemplate" CssClass="required" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label2" AssociatedControlID="drlSiteLayout" CssClass="jform_name-lbl"
                                Text="Site Layout *"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlSiteLayout" CssClass="required" Width="200"
                                DataTextField="Name" DataValueField="Id">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label4" AssociatedControlID="drlPageLayout" CssClass="jform_name-lbl"
                                Text="Page Layout *"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlPageLayout" CssClass="required" Width="200"
                                DataTextField="Name" DataValueField="Id">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label13" AssociatedControlID="drlPages" CssClass="jform_name-lbl"
                                Text="Parent Page "></asp:Label>
                            <asp:DropDownList runat="server" ID="drlPages" Width="200">
                            </asp:DropDownList>
                        </li>
                    </ul>
                </fieldset>
            </div>
            <div class="width-50 fltlft">
                <fieldset class="adminform">
                    <legend>Page Details</legend>
                    <div id="accordion">
                        <h3>Page Options</h3>
                        <div class="pane-slider content">
                            <ul class="adminformlist">
                                <li>
                                    <asp:Label runat="server" ID="Label14" AssociatedControlID="txtBreadCrumb" CssClass="jform_name-lbl"
                                        Text="Breadcrumb Title"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtBreadCrumb" Width="200"></asp:TextBox>
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="Label1" AssociatedControlID="txtName" CssClass="jform_name-lbl"
                                        Text="Description"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Height="50"
                                        Width="250"></asp:TextBox>
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="Label6" AssociatedControlID="chbxIsDefault" CssClass="jform_name-lbl"
                                        Text="Default"></asp:Label>
                                    <asp:CheckBox runat="server" ID="chbxIsDefault" />
                                </li>
                            </ul>
                        </div>
                        <h3>Security Options</h3>
                        <div class="pane-slider content">
                            <ul class="adminformlist">
                                <li>
                                    <asp:CheckBoxList runat="server" ID="chbxListRoles" RepeatColumns="1" RepeatDirection="Vertical" />
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="Label8" AssociatedControlID="drlNoAuthenticatedPage"
                                        CssClass="jform_name-lbl" Text="No Authenticated Page"></asp:Label>
                                    <asp:DropDownList runat="server" ID="drlNoAuthenticatedPage" Width="200">
                                    </asp:DropDownList>
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="Label9" AssociatedControlID="drlNoPermission" CssClass="jform_name-lbl"
                                        Text="No Permission Page"></asp:Label>
                                    <asp:DropDownList runat="server" ID="drlNoPermission" Width="200">
                                    </asp:DropDownList>
                                </li>
                            </ul>
                        </div>
                        <h3>Metadata Options</h3>
                        <div class="pane-slider content">
                            <ul class="adminformlist">
                                <li>
                                    <asp:Label runat="server" ID="Label7" AssociatedControlID="txtQueryName" CssClass="jform_name-lbl"
                                        Text="Query Name*"></asp:Label>
                                    <asp:TextBox Rows="18" ID="txtQueryName" runat="server" Width="200"
                                        onkeydown="return TabKeyTextArea(event, 'ctl00_MainContent_txtDocType', '    ');">
                                    </asp:TextBox>
                                    <asp:Literal runat="server" ID="ltlQuerySelector"></asp:Literal>
                                </li>
                                <li>
                                    <fieldset style="float: left;">
                                        <legend>Query Paramaters</legend>
                                        <ul class="adminformlist" id="ulmultiple">
                                            <asp:Literal runat="server" ID="ltlQueryParams"></asp:Literal>
                                        </ul>
                                    </fieldset>
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="Label10" AssociatedControlID="txtMetaDescription" CssClass="jform_name-lbl"
                                        Text="Meta Description"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtMetaDescription" TextMode="MultiLine" Height="50"
                                        Width="250"></asp:TextBox>
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="Label11" AssociatedControlID="txtMetaKeywords" CssClass="jform_name-lbl"
                                        Text="Meta Keywords"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtMetaKeywords" TextMode="MultiLine" Height="50"
                                        Width="250"></asp:TextBox>
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="Label12" AssociatedControlID="txtContentRights" CssClass="jform_name-lbl"
                                        Text="Content Rights"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtContentRights" TextMode="MultiLine" Height="50"
                                        Width="250"></asp:TextBox>
                                </li>
                            </ul>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div class="clr">
            </div>
        </div>
    </div>
    <input type="hidden" name="form-action" id="form-action" />
    <input type="hidden" name="checkboxCount" id="checkboxCount" value="0" />
    <script type="text/javascript">
        $(function () {
            $("#accordion").accordion({
                heightStyle: "content"
            });
        });
    </script>
    <script type="text/javascript" src="/Content/js/dataviewerparameters.js"></script>
</asp:Content>
