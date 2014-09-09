<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits="FWeb.Administrator.Poll._default" %>

<%@ Import Namespace="FUIControls.Settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Polls |
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
                    Poll Manager</h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
        <div class="m">
            <ul id="submenu">
                <li><a href="" class="active">Polls</a> </li>
                <li><a href="/administrator/poll/questions.aspx">Questions</a> </li>
                <li><a href="/administrator/poll/results.aspx">Results</a> </li>
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
                                    <asp:LinkButton runat="server" ID="lnkQuestion" ToolTip="Click to sort by Question"
                                        Text="Question" OnClick="LnkSortByQuestionClick">
                                        <asp:Image runat="server" ID="imgSortByQueation" /></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkBlockMode" ToolTip="Click to sort by BlockMode"
                                        Text="Block Mode" OnClick="LnkSortByBlockModeClick">
                                        <asp:Image runat="server" ID="imgSortByBlockMode" /></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkIsActive" ToolTip="Click to sort by IsActive"
                                        Text="Is Active" OnClick="LnkSortByIsActive">
                                        <asp:Image runat="server" ID="imgSortByIsActive" /></asp:LinkButton>
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
                            <a href="/administrator/poll/pollaction.aspx?id=<%# DataBinder.Eval(Container.DataItem, "Id") %> ">
                                <%# DataBinder.Eval(Container.DataItem, "Question") %>
                            </a>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "BlockModeName") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "IsActiveName") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "Id") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="row0">
                        <td class="center">
                            <input type="checkbox" title="Checkbox for row <%# DataBinder.Eval(Container.DataItem, "Name") %>"
                                value="<%# DataBinder.Eval(Container.DataItem, "Id") %>" name="chbxRow" id="chbxRow"
                                onclick="Frebo.checked(this.checked);">
                        </td>
                        <td>
                            <a href="/administrator/poll/pollaction.aspx?id=<%# DataBinder.Eval(Container.DataItem, "Id") %> ">
                                <%# DataBinder.Eval(Container.DataItem, "Question") %>
                            </a>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "BlockModeName") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "IsActiveName") %>
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
