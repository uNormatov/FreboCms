<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="itemaction.aspx.cs" Inherits="FWeb.Administrator.Menus.itemaction" %>

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
            <div class="pagetitle icon-48-sitelayoutmgr">
                <h2>
                    Menu Items:
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
                    <legend>Menu Item Details</legend>
                    <ul class="adminformlist">
                        <li>
                            <asp:Label runat="server" ID="Label1" AssociatedControlID="drlMenus" CssClass="jform_name-lbl"
                                Text="Menu *"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlMenus" Width="200" DataTextField="Name" DataValueField="Id" />
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="llbl1" AssociatedControlID="txtName" CssClass="jform_name-lbl"
                                Text="Title *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtName" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label2" AssociatedControlID="txtUrl" CssClass="jform_name-lbl"
                                Text="Url *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtUrl" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label3" AssociatedControlID="drlList" CssClass="jform_name-lbl"
                                Text="Parent Item *"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlList" Width="200" DataTextField="Title" DataValueField="Id" />
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label6" AssociatedControlID="drlPageTarget" CssClass="jform_name-lbl"
                                Text="Page Target"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlPageTarget" Width="200">
                                <asp:ListItem Value="0" Selected="true">Same Tab</asp:ListItem>
                                <asp:ListItem Value="1">New Tab</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </li>
                    </ul>
                </fieldset>
            </div>
            <div class="width-50 fltlft">
                <fieldset class="adminform">
                    <legend>Visibile For</legend>
                    <ul class="adminformlist">
                        <li>
                            <asp:CheckBoxList runat="server" ID="chbxListRoles" RepeatColumns="1" RepeatDirection="Vertical" />
                        </li>
                    </ul>
                </fieldset>
            </div>
            <div class="clr">
            </div>
        </div>
    </div>
    <input type="hidden" name="form-action" id="form-action" />
    <input type="hidden" name="checkboxCount" id="checkboxCount" value="0" />
</asp:Content>
