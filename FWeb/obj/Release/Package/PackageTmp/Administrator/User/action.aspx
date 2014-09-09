<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="action.aspx.cs" Inherits="FWeb.Administrator.User.action" %>

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
                    User Manager:
                    <asp:Literal runat="server" ID="ltlTitle"></asp:Literal></h2>
            </div>
        </div>
    </div>
    <div id="system-message-container">
        <asp:Literal runat="server" ID="ltlMessage"></asp:Literal>
    </div>
    <div id="element-box">
        <div class="m">
            <div class="width-40 fltlft">
                <fieldset class="adminform">
                    <legend>User Details</legend>
                    <ul class="adminformlist">
                        <%if (!IsEdit)
                          { %>
                        <li>
                            <asp:Label runat="server" ID="llbl1" AssociatedControlID="txtUserName" CssClass="jform_name-lbl"
                                Text="User Name *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtUserName" Width="200"></asp:TextBox>
                        </li>
                        <% } %>
                        <li>
                            <asp:Label runat="server" ID="Label4" AssociatedControlID="chbxIsActive" CssClass="jform_name-lbl"
                                Text="Role"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlRoles" Width="200" OnSelectedIndexChanged="drlRoles_OnSelectedIndexChanged"
                                AutoPostBack="true" />
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label1" AssociatedControlID="txtEmail" CssClass="jform_name-lbl"
                                Text="Email *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtEmail" Width="200"></asp:TextBox>
                        </li>
                        <%if (!IsEdit)
                          { %>
                        <li>
                            <asp:Label runat="server" ID="Label3" AssociatedControlID="txtPassword" CssClass="jform_name-lbl"
                                Text="Password *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label5" AssociatedControlID="txtComfirmPassword" CssClass="jform_name-lbl"
                                Text="Confirm Password*"></asp:Label>
                            <asp:TextBox runat="server" ID="txtComfirmPassword" TextMode="Password" Width="200"></asp:TextBox>
                        </li>
                        <% } %>
                        <li>
                            <asp:Label runat="server" ID="Label2" AssociatedControlID="chbxIsActive" CssClass="jform_name-lbl"
                                Text="Is Active"></asp:Label>
                            <asp:CheckBox runat="server" ID="chbxIsActive" />
                            <br />
                        </li>
                    </ul>
                </fieldset>
            </div>
            <asp:Panel runat="server" ID="pnlContentType" Visible="false">
                <div class="width-60 fltlft">
                    <fieldset class="adminform">
                        <legend>User Profile </legend>
                        <fr:MainForm runat="server" ID="mainForm"></fr:MainForm>
                    </fieldset>
                </div>
            </asp:Panel>
            <div class="clr">
            </div>
        </div>
    </div>
    <input type="hidden" name="form-action" id="form-action" />
    <input type="hidden" name="checkboxCount" id="checkboxCount" value="0" />
</asp:Content>
