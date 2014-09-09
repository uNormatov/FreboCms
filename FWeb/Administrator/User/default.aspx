<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits="FWeb.Administrator.User._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Users | Frebo Cms</title>
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
                    User Manager: Users</h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
        <div class="m">
            <ul id="submenu">
                <li><a href="" class="active">Users</a> </li>
                <li><a href="/administrator/user/roles.aspx">Roles</a> </li>
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
            <asp:Repeater runat="server" ID="rptList" OnItemDataBound="rptList_ItemDataBound">
                <HeaderTemplate>
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
                                    <asp:LinkButton runat="server" ID="lnkEmail" ToolTip="Click to sort by Email" Text="Email"
                                        OnClick="lnkEmail_OnClick">
                                        <asp:Image runat="server" ID="imgEmail" /></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkLastLoginDate" ToolTip="Click to sort by Last Login Date"
                                        Text="Last Login Date" OnClick="lnkLastLoginDate_OnClick">
                                        <asp:Image runat="server" ID="imgLastLogin" /></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkIsOnline" ToolTip="Click to sort by IsOnline"
                                        Text="Is Online" OnClick="lnkIsOnline_OnClick">
                                        <asp:Image runat="server" ID="imgIsOnline" /></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkIsApproved" ToolTip="Click to sort by IsApproved"
                                        Text="Is Approved" OnClick="lnkIsApproved_OnClick">
                                        <asp:Image runat="server" ID="imgIsApproved" /></asp:LinkButton>
                                </th>
                            </tr>
                        </thead>
                </HeaderTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="7">
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
                            </div>
                        </td>
                    </tr>
                    </table>
                </FooterTemplate>
                <ItemTemplate>
                    <tr class="row0">
                        <td class="center">
                            <input type="checkbox" title="Checkbox for row <%# DataBinder.Eval(Container.DataItem, "UserName") %>"
                                value="<%# DataBinder.Eval(Container.DataItem, "UserName") %>" name="chbxRow"
                                id="chbxRow" onclick="Frebo.checked(this.checked);">
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "UserName") %>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "Email") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "LastLoginDate")!=null ? DataBinder.Eval(Container.DataItem, "LastLoginDate","{0:dd-MMMM-yyyy}"):""%>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "IsOnline").ToString()=="True" ? "Yes" : "No"%>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "IsApproved").ToString() == "True" ? "Yes" : "No"%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="row1">
                        <td class="center">
                            <input type="checkbox" title="Checkbox for row <%# DataBinder.Eval(Container.DataItem, "UserName") %>"
                                value="<%# DataBinder.Eval(Container.DataItem, "UserName") %>" name="chbxRow"
                                id="chbxRow" onclick="Frebo.checked(this.checked);">
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "UserName") %>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "Email") %>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "LastLoginDate")!=null ? DataBinder.Eval(Container.DataItem, "LastLoginDate","{0:dd-MMMM-yyyy}"):""%>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "IsOnline").ToString()=="True" ? "Yes" : "No"%>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "IsApproved").ToString() == "True" ? "Yes" : "No"%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <input type="hidden" name="form-action" id="form-action" />
    <input type="hidden" name="checkboxCount" id="checkboxCount" value="0" />
</asp:Content>
