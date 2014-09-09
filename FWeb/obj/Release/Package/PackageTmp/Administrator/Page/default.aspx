<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits="FWeb.Administrator.Page._default" %>

<%@ Import Namespace="FUIControls.Settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Pages |
        <%=CoreSettings.CurrentSite.Name %></title>
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
            <div class="pagetitle icon-48-sitelayoutmgr">
                <h2>
                    Page Manager</h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
        <div class="m">
            <ul id="submenu">
                <li><a href="" class="active">Pages</a> </li>
                <li><a href="/administrator/page/blocks.aspx">Blocks</a> </li>
                <%--<li><a href="/administrator/poll/pollipaddress.aspx">Ip Addresses</a> </li>--%>
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
            <asp:Repeater runat="server" ID="rptList" OnItemDataBound="RptListItemDataBound">
                <HeaderTemplate>
                    <table class="adminlist">
                        <thead>
                            <tr>
                                <th width="1%">
                                    <input type="checkbox" title="Check All" value="" name="checkall-toggle" onclick="Frebo.selectAll(this)">
                                </th>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkSortByName" ToolTip="Click to sort by Name"
                                        Text="Name" OnClick="LnkSortByNameClick">
                                        <asp:Image runat="server" ID="imgSortByName" /></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkSortByTitle" ToolTip="Click to sort by Title"
                                        Text="Title" OnClick="lnkSortByTitle_OnClick">
                                        <asp:Image runat="server" ID="imgSortByTitle" /></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkSortBySeoTemplate" ToolTip="Click to sort by SeoTemplate"
                                        Text="Seo Url" OnClick="lnkSortBySeoTemplate_OnClick">
                                        <asp:Image runat="server" ID="imgSortBySeoTemplate" /></asp:LinkButton>
                                </th>
                                <th>
                                    Page Layout
                                </th>
                                <th>
                                    Site Layout
                                </th>
                                <th width="1%" class="nowrap">
                                    <asp:LinkButton runat="server" ID="lnkSortById" ToolTip="Click to sort by Id" Text="Id"
                                        OnClick="LnkSortByIdClick">
                                        <asp:Image runat="server" ID="imgSortById" />
                                    </asp:LinkButton>
                                </th>
                            </tr>
                        </thead>
                </HeaderTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="8" class="left">
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
                                        <fr:FPager runat="server" ID="pager">
                                        </fr:FPager>
                                    </div>
                                    <input type="hidden" value="0" name="limitstart">
                                </div>
                            </div>
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
                            <a href="/administrator/page/blocks.aspx?pageid=<%# DataBinder.Eval(Container.DataItem, "Id") %> ">
                                <%# DataBinder.Eval(Container.DataItem, "Name") %>
                            </a>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "Title") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "SeoTemplate") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "PageLayoutName") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "SiteLayoutName") %>
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
                            <a href="/administrator/page/blocks.aspx?pageid=<%# DataBinder.Eval(Container.DataItem, "Id") %> ">
                                <%# DataBinder.Eval(Container.DataItem, "Name") %>
                            </a>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "Title") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "SeoTemplate") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "PageLayoutName") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "SiteLayoutName") %>
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
