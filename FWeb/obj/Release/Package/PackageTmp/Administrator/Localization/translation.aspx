<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="translation.aspx.cs" Inherits="FWeb.Administrator.Localization.translation" %>

<%@ Import Namespace="FUIControls.Settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Translations |
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
                <li><a href="/administrator/localization/">Languages</a> </li>
                <li><a href="" class="active">Translations</a> </li>
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
            <fr:FRepeater runat="server" ID="rptList" OnItemDataBound="rptList_OnItemDataBound">
                <FooterTemplate>
                    <tr>
                        <td colspan="10" class="left">
                            <div class="container">
                                <div class="pagination">
                                    <div class="limit">
                                        Display #
                                        <asp:DropDownList runat="server" ID="drlDisplay" CssClass="inputbox" AutoPostBack="true"
                                            OnSelectedIndexChanged="DrlDisplaySelectedIndexChanged">
                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="20" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="limit">
                                        <fr:FPager runat="server" ID="pager" PageIndexKeywordType="QueryString" PageIndexKeyword="page"
                                            IsBackend="true">
                                        </fr:FPager>
                                    </div>
                                    <input type="hidden" value="0" name="limitstart">
                                </div>
                            </div>
                        </td>
                    </tr>
                    </table>
                </FooterTemplate>
            </fr:FRepeater>
        </div>
    </div>
    <input type="hidden" name="form-action" id="form-action" />
    <input type="hidden" name="checkboxCount" id="checkboxCount" value="0" />
</asp:Content>
