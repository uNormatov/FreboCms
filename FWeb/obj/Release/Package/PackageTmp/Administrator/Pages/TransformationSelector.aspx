<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransformationSelector.aspx.cs"
    Inherits="FWeb.Administrator.Pages.TransformationSelector" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="/Content/js/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
    <script src="/Content/js/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="/Content/css/template.css" rel="stylesheet" type="text/css" />
    <link href="/Content/css/system.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function select(id) {
            parent.$('#<%= ControlId %>').val(id);
            parent.$.fancybox.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="element-box">
        <div class="m">
            <div class="clr">
            </div>
            <fieldset id="filter-bar">
                <div class="filter-search fltlft">
                    <label for="filter_search" class="filter-search-lbl">
                        Filter:</label>
                    <asp:TextBox runat="server" ID="txtSearch"></asp:TextBox>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_OnClick" />
                    <asp:Button runat="server" ID="btnClear" Text="Clear" OnClick="btnClear_OnClick" />
                </div>
                <div class="filter-select fltrt">
                    <asp:DropDownList runat="server" ID="drlList" AutoPostBack="true" OnSelectedIndexChanged="drlList_OnSelectedIndexChanged"
                        DataTextField="Name" DataValueField="Id">
                    </asp:DropDownList>
                </div>
            </fieldset>
            <asp:Repeater runat="server" ID="rptList" OnItemDataBound="rptList_OnItemDataBound">
                <HeaderTemplate>
                    <div class="clr">
                    </div>
                    <table class="adminlist">
                        <thead>
                            <tr>
                                <th>
                                    <asp:LinkButton runat="server" ID="lnkSortByName" ToolTip="Click to sort by Name"
                                        Text="Name" OnClick="lnkSortByName_OnClick">
                                        <asp:Image runat="server" ID="imgSortByName" /></asp:LinkButton>
                                </th>
                                <th width="1%" class="nowrap">
                                    <asp:LinkButton runat="server" ID="lnkSortById" ToolTip="Click to sort by Id" Text="Id"
                                        OnClick="lnkSortById_OnClick">
                                        <asp:Image runat="server" ID="imgSortById" />
                                    </asp:LinkButton>
                                </th>
                                <th>
                                </th>
                            </tr>
                        </thead>
                </HeaderTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="3" class="left">
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
                        <td>
                            <a href="javascript:;" onclick="select('<%# DataBinder.Eval(Container.DataItem, "Name") %>')">
                                <%# DataBinder.Eval(Container.DataItem, "Name") %>
                            </a>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "Id") %>
                        </td>
                        <td>
                            <a target="_blank" href="/administrator/contenttype/transformationaction.aspx?type=entry&id=<%# DataBinder.Eval(Container.DataItem, "Id") %>">
                                edit </a>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="row1">
                        <td>
                            <a href="javascript:;" onclick="select('<%# DataBinder.Eval(Container.DataItem, "Name") %>')">
                                <%# DataBinder.Eval(Container.DataItem, "Name") %>
                            </a>
                        </td>
                        <td class="center">
                            <%# DataBinder.Eval(Container.DataItem, "Id") %>
                        </td>
                        <td>
                            <a target="_blank" href="/administrator/contenttype/transformationaction.aspx?type=entry&id=<%# DataBinder.Eval(Container.DataItem, "Id") %>">
                                edit </a>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    </form>
</body>
</html>
