<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="action.aspx.cs" Inherits="FWeb.Administrator.Article.action" %>

<%@ Import Namespace="FCore.Constant" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            <div class="pagetitle icon-48-article">
                <h2>
                    Article Manager:
                    <asp:Literal runat="server" ID="ltlTitle"></asp:Literal>
                </h2>
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
                    <legend>Article details</legend>
                    <ul class="adminformlist">
                        <li>
                            <asp:Label runat="server" ID="llbl1" AssociatedControlID="txtTitle" CssClass="jform_name-lbl"
                                Text="Title *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtTitle" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label3" AssociatedControlID="txtCode" CssClass="jform_name-lbl"
                                Text="Code *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtCode" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label1" AssociatedControlID="drlLanguages" CssClass="jform_name-lbl"
                                Text="Language "></asp:Label>
                            <asp:DropDownList runat="server" ID="drlLanguages" />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label5" AssociatedControlID="fckEditor" CssClass="jform_name-lbl"
                                Text="Text *"></asp:Label>
                             <fr:FCKeditor ID="fckEditor" runat="server" BasePath="~/Content/FCKeditor/" Height="585px" FillEmptyBlocks="false">
                            </fr:FCKeditor>    
                            
                        </li>
                    </ul>
                </fieldset>
            </div>
            <div class="width-50 fltlft">
                <fieldset class="adminform">
                    <legend>Positions</legend>
                    <div id="accordion">
                        <h3>
                            Page Position
                        </h3>
                        <div class="pane-slider content">
                            <ul class="adminformlist">
                                <li>
                                    <asp:Label runat="server" ID="Label4" AssociatedControlID="drlPages" CssClass="jform_name-lbl"
                                        Text="Page"></asp:Label>
                                    <asp:DropDownList runat="server" ID="drlPages" onchange="fillWebPartZone(0,'')" />
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="Label7" AssociatedControlID="drlPageZones" CssClass="jform_name-lbl"
                                        Text="Zone"></asp:Label>
                                    <asp:DropDownList runat="server" ID="drlPageZones">
                                        <Items>
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </Items>
                                    </asp:DropDownList>
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="Label10" AssociatedControlID="drlPageOrder" CssClass="jform_name-lbl"
                                        Text="Order *"></asp:Label>
                                    <asp:DropDownList runat="server" ID="drlPageOrder" CssClass="required" Width="200">
                                        <asp:ListItem Text="1" Value="1" Selected="true"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </li>
                            </ul>
                        </div>
                        <h3>
                            Page Layout Position</h3>
                        <div class="pane-slider content">
                            <ul class="adminformlist">
                                <li>
                                    <asp:Label runat="server" ID="Label8" AssociatedControlID="drlPageLayouts" CssClass="jform_name-lbl"
                                        Text="Page Layout"></asp:Label>
                                    <asp:DropDownList runat="server" ID="drlPageLayouts" onchange="fillWebPartZone(1,'')" />
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="Label9" AssociatedControlID="drlPageLayoutZones" CssClass="jform_name-lbl"
                                        Text="Zone"></asp:Label>
                                    <asp:DropDownList runat="server" ID="drlPageLayoutZones">
                                        <Items>
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </Items>
                                    </asp:DropDownList>
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="Label11" AssociatedControlID="drlPageLayoutOrder" CssClass="jform_name-lbl"
                                        Text="Order *"></asp:Label>
                                    <asp:DropDownList runat="server" ID="drlPageLayoutOrder" CssClass="required" Width="200">
                                        <asp:ListItem Text="1" Value="1" Selected="true"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </li>
                            </ul>
                        </div>
                        <h3>
                            Site Layout Position</h3>
                        <div class="pane-slider content">
                            <ul class="adminformlist">
                                <li>
                                    <asp:Label runat="server" ID="Label2" AssociatedControlID="drlSiteLayouts" CssClass="jform_name-lbl"
                                        Text="Site Layout"></asp:Label>
                                    <asp:DropDownList runat="server" ID="drlSiteLayouts" />
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="Label6" AssociatedControlID="drlSiteLayoutZones" CssClass="jform_name-lbl"
                                        Text="Zone"></asp:Label>
                                    <asp:DropDownList runat="server" ID="drlSiteLayoutZones" onchange="fillWebPartZone(2,'')">
                                        <Items>
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </Items>
                                    </asp:DropDownList>
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="Label12" AssociatedControlID="drlSiteLayoutOrder" CssClass="jform_name-lbl"
                                        Text="Order *"></asp:Label>
                                    <asp:DropDownList runat="server" ID="drlSiteLayoutOrder" CssClass="required" Width="200">
                                        <asp:ListItem Text="1" Value="1" Selected="true"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
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

        function fillWebPartZone(typeId, value) {
            var data = '';
            if (typeId == 0) {
                data = "type=<%= SiteConstants.JsonWebpartZoneByPageId %>&id=" + $('#<%= drlPages.ClientID %>').val();
            }
            else if (typeId == 1) {
                data = "type=<%= SiteConstants.JsonWebpartZoneByLayoutId%>&id=" + $('#<%= drlPageLayouts.ClientID %>').val();
            }
            else {
                data = "type=<%= SiteConstants.JsonWebpartZoneByLayoutId%>&id=" + $('#<%= drlSiteLayouts.ClientID %>').val();
            }
            $.ajax({
                type: "GET",
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                url: "/jsonhandler.ashx",
                data: data,
                success: function (resp) {
                    var selectControl;
                    if (typeId == 0) {
                        selectControl = $("#<%= drlPageZones.ClientID %>");
                    }
                    else if (typeId == 1) {
                        selectControl = $("#<%= drlPageLayoutZones.ClientID %>");
                    }
                    else {
                        selectControl = $("#<%= drlSiteLayoutZones.ClientID %>");
                    }
                    $('option', selectControl).remove();
                    selectControl.append("<option value=''>Select</option>");
                    if (resp != null) {
                        $.each(resp, function (i, item) {
                            if (item.Name == value)
                                selectControl.append("<option selected value='" + item.Name + "'>" + item.Name + "</option>");
                            else
                                selectControl.append("<option value='" + item.Name + "'>" + item.Name + "</option>");
                        });
                    }
                }
            });
        }
    </script>
</asp:Content>
