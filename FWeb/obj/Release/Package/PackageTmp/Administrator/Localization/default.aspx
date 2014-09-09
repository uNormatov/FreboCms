<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits="FWeb.Administrator.Localization._default" %>

<%@ Import Namespace="FUIControls.Settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Localization |
        <%= CoreSettings.CurrentSite.Name%></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <div id="toolbar-box">
        <div class="m">
            <div id="toolbar" class="toolbar-list">
                <ul>
                    <li id="toolbar-new" class="button"><a class="toolbar" href="#" onclick="Frebo.formsubmit('add');">
                        <span class="icon-32-new"></span>New </a></li>
                    <li id="toolbar-edit" class="button"><a class="toolbar" onclick="if($('#checkboxCount').val()==0){alert('Please first make a selection from the list')}else{ Frebo.formsubmit('edit');}"
                        href="#"><span class="icon-32-edit"></span>Edit </a></li>
                    <li id="toolbar-delete" class="button"><a class="toolbar" onclick="if($('#checkboxCount').val()==0){alert('Please first make a selection from the list')}else{ Frebo.formsubmit('delete');}"
                        href="#"><span class="icon-32-delete"></span>Delete </a></li>
                </ul>
                <div class="clr">
                </div>
            </div>
            <div class="pagetitle language-48">
                <h2>
                    Localization Manager</h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
        <div class="m">
            <ul id="submenu">
                <li><a href="" class="active">Languages</a> </li>
                <li><a href="/administrator/localization/translation.aspx">Translations</a> </li>
            </ul>
            <div class="clr">
            </div>
        </div>
    </div>
    <div id="system-message-container">
        <asp:Literal runat="server" ID="ltlMessage"></asp:Literal>
    </div>
    <div id="element-box">
        <div class="m">
            <div class="clr">
            </div>
            <asp:Repeater runat="server" ID="rptList">
                <HeaderTemplate>
                    <table class="adminlist">
                        <thead>
                            <tr>
                                <th width="1%">
                                    <input type="checkbox" title="Check All" value="" name="checkall-toggle" onclick="Frebo.selectAll(this)">
                                </th>
                                <th>
                                    Name
                                </th>
                                <th>
                                    Code
                                </th>
                                <th width="1%" class="nowrap">
                                    Is Default
                                </th>
                                <th>
                                    Id
                                </th>
                            </tr>
                        </thead>
                </HeaderTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="5">
                        </td>
                    </tr>
                    </table>
                </FooterTemplate>
                <ItemTemplate>
                    <tr class="row0">
                        <td class="center">
                            <input type="checkbox" title="Checkbox for row <%# DataBinder.Eval(Container.DataItem, "Name") %>"
                                value="<%# DataBinder.Eval(Container.DataItem, "Id") %>" name="chbxRow" id="chbxRow"
                                onclick="Frebo.checked(this.checked);">
                        </td>
                        <td>
                            <a href="/administrator/localization/action.aspx?type=entry&id=<%# DataBinder.Eval(Container.DataItem, "Id") %> ">
                                <%# DataBinder.Eval(Container.DataItem, "Name") %>
                            </a>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "Code") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "IsDefault").ToString() =="True" ? "Yes" : "No" %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "Id") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="row1">
                        <td class="center">
                            <input type="checkbox" title="Checkbox for row <%# DataBinder.Eval(Container.DataItem, "Name") %>"
                                value="<%# DataBinder.Eval(Container.DataItem, "Id") %>" name="chbxRow" id="chbxRow"
                                onclick="Frebo.checked(this.checked);">
                        </td>
                        <td>
                            <a href="/administrator/localization/action.aspx?type=entry&id=<%# DataBinder.Eval(Container.DataItem, "Id") %> ">
                                <%# DataBinder.Eval(Container.DataItem, "Name") %>
                            </a>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "Code") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "IsDefault").ToString() =="True" ? "Yes" : "No" %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "Id") %>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <input type="hidden" name="form-action" id="form-action" />
    <input type="hidden" name="checkboxCount" id="checkboxCount" value="0" />
</asp:Content>
