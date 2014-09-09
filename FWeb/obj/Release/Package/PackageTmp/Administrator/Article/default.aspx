<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits="FWeb.Administrator.Article._default" %>

<%@ Import Namespace="FUIControls.Settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Articles |
        <%= CoreSettings.CurrentSite.Name %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <div id="toolbar-box">
        <div class="m">
            <div id="toolbar" class="toolbar-list">
                <ul>
                    
                    <li class="divider"></li>
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
            <div class="pagetitle icon-48-article">
                <h2>
                    Article Manager</h2>
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
            <asp:Repeater runat="server" ID="rptList" OnItemDataBound="rptList_OnItemDataBound">
                <HeaderTemplate>
                    <table class="adminlist">
                        <thead>
                            <tr>
                                <th width="1%">
                                    <input type="checkbox" title="Check All" value="" name="checkall-toggle" onclick="Frebo.selectAll(this)">
                                </th>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkSortByTitle" ToolTip="Click to sort by Title"
                                        Text="Title" OnClick="lnkSortByTitle_OnClick">
                                        <asp:Image runat="server" ID="imgSortByTitle" /></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkCode" ToolTip="Click to sort by Code" Text="Code"
                                        OnClick="lnkCode_OnClick">
                                        <asp:Image runat="server" ID="imgSortByCode" /></asp:LinkButton>
                                </th>
                                <th>
                                    Language
                                </th>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkCreatedBy" ToolTip="Click to sort by Created By"
                                        Text="Created By" OnClick="lnkCreatedBy_OnClick">
                                        <asp:Image runat="server" ID="imgSortByCreatedBy" /></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkCreatedDate" ToolTip="Click to sort by Created Date"
                                        Text="Created Date" OnClick="lnkCreatedDate_OnClick">
                                        <asp:Image runat="server" ID="imgSortByCreatedDate" /></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkModifiedBy" ToolTip="Click to sort by Modified By"
                                        Text="Modified By" OnClick="lnkModifiedBy_OnClick">
                                        <asp:Image runat="server" ID="imgSortByModifedBy" /></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkModifiedDate" ToolTip="Click to sort by Modified Date"
                                        Text="Created Date" OnClick="lnkModifiedDate_OnClick">
                                        <asp:Image runat="server" ID="imgSortByModifiedDate" /></asp:LinkButton>
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
                        <td colspan="9">
                            <div class="container">
                                <div class="pagination">
                                    <div class="limit">
                                        Display #
                                        <asp:DropDownList runat="server" ID="drlDisplay" CssClass="inputbox" AutoPostBack="true"
                                            OnSelectedIndexChanged="drlDisplay_OnSelectedIndexChanged">
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
                            <input type="checkbox" title="Checkbox for row <%# DataBinder.Eval(Container.DataItem, "Title") %>"
                                value="<%# DataBinder.Eval(Container.DataItem, "Id") %>" name="chbxRow" id="chbxRow"
                                onclick="Frebo.checked(this.checked);">
                        </td>
                        <td>
                            <a href="/administrator/article/action.aspx?type=entry&id=<%# DataBinder.Eval(Container.DataItem, "Id") %> ">
                                <%# DataBinder.Eval(Container.DataItem, "Title") %>
                            </a>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "Code") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "Language") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "CreatedBy") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "CreatedDate") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "ModifiedBy") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "ModifiedDate") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "Id") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="row1">
                        <td class="center">
                            <input type="checkbox" title="Checkbox for row <%# DataBinder.Eval(Container.DataItem, "Title") %>"
                                value="<%# DataBinder.Eval(Container.DataItem, "Id") %>" name="chbxRow" id="chbxRow"
                                onclick="Frebo.checked(this.checked);">
                        </td>
                        <td>
                            <a href="/administrator/article/action.aspx?type=entry&id=<%# DataBinder.Eval(Container.DataItem, "Id") %> ">
                                <%# DataBinder.Eval(Container.DataItem, "Title") %>
                            </a>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "Code") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "Language") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "CreatedBy") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "CreatedDate") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "ModifiedBy") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "ModifiedDate") %>
                            <td class="center">
                                <%# DataBinder.Eval(Container.DataItem, "Id") %>
                            </td>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <input type="hidden" name="form-action" id="form-action" />
    <input type="hidden" name="checkboxCount" id="checkboxCount" value="0" />
</asp:Content>
