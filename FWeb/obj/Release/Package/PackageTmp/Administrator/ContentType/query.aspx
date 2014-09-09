﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="query.aspx.cs" Inherits="FWeb.Administrator.ContentType.query" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Queries | Frebo Cms</title>
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
                    Development: Queries
                </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
        <div class="m">
            <ul id="submenu">
                <li><a href="/administrator/contentType/">Content Types</a> </li>
                <li><a href="/administrator/contenttype/field.aspx?contenttypeid=<%= ContentTypeId %>">
                    Fields</a> </li>
                <li><a class="active" href="/administrator/contenttype/query.aspx?contenttypeid=<%= ContentTypeId %>">
                    Queries</a> </li>
                <li><a href="/administrator/contenttype/transformation.aspx?contenttypeid=<%= ContentTypeId %>">
                    Transformation</a> </li>
                <li><a href="/administrator/contenttype/form.aspx?contenttypeid=<%= ContentTypeId %>">
                    Forms</a> </li>
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
            <fieldset id="filter-bar">
                <div class="filter-search fltlft">
                    <label for="filter_search" class="filter-search-lbl">
                        Filter:</label>
                    <asp:TextBox runat="server" ID="txtSearch"></asp:TextBox>
                    <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_onClick" Text="Search" />
                    <button onclick="document.id('filter_search').value='';this.form.submit();" type="button">
                        Clear</button>
                </div>
                <div class="filter-select fltrt">
                    <asp:DropDownList runat="server" ID="drlList" AutoPostBack="true" OnSelectedIndexChanged="drlList_OnSelectedIndexChanged"
                        DataTextField="Name" DataValueField="Id">
                    </asp:DropDownList>
                </div>
            </fieldset>
            <asp:Repeater runat="server" ID="rptList" OnItemDataBound="rptList_ItemDataBound">
                <HeaderTemplate>
                    <div class="clr">
                    </div>
                    <table class="adminlist">
                        <thead>
                            <tr>
                                <th width="1%">
                                    <input type="checkbox" title="Check All" value="" name="checkall-toggle" onclick="Frebo.selectAll(this)">
                                </th>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkSortByName" ToolTip="Click to sort by Name"
                                        Text="Name" OnClick="lnkSortByName_Click">
                                        <asp:Image runat="server" ID="imgSortByName" /></asp:LinkButton>
                                </th>
                                <th>
                                    Query Text
                                </th>
                                <th width="1%" class="nowrap">
                                    <asp:LinkButton runat="server" ID="lnkSortById" ToolTip="Click to sort by Id" Text="Id"
                                        OnClick="lnkSortById_Click">
                                        <asp:Image runat="server" ID="imgSortById" />
                                    </asp:LinkButton>
                                </th>
                            </tr>
                        </thead>
                </HeaderTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td colspan="5" class="left">
                            <div class="container">
                                <div class="pagination">
                                    <div class="limit">
                                        Display #
                                        <asp:DropDownList runat="server" ID="drlDisplay" CssClass="inputbox" AutoPostBack="true"
                                            OnSelectedIndexChanged="drlDisplay_SelectedIndexChanged">
                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="20" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="limit">
                                    </div>
                                    <input type="hidden" value="0" name="limitstart">
                                </div>
                                <fr:FPager runat="server" ID="pager" PageIndexKeywordType="QueryString" PageIndexKeyword="page"
                                    IsBackend="true">
                                </fr:FPager>
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
                            <a href="/administrator/contenttype/queryaction.aspx?type=entry&id=<%# DataBinder.Eval(Container.DataItem, "Id") %>">
                                <%# DataBinder.Eval(Container.DataItem, "Name") %>
                            </a>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "Text") %>
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
                            <a href="/administrator/contenttype/queryaction.aspx?type=entry&id=<%# DataBinder.Eval(Container.DataItem, "Id") %>">
                                <%# DataBinder.Eval(Container.DataItem, "Name") %>
                            </a>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "Text") %>
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
