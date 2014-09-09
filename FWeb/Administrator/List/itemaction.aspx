<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="itemaction.aspx.cs" Inherits="FWeb.Administrator.List.itemaction" %>

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
                    List Item Items:
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
                    <legend>List Details</legend>
                    <ul class="adminformlist">
                        <li>
                            <asp:Label runat="server" ID="llbl1" AssociatedControlID="txtName" CssClass="jform_name-lbl"
                                Text="Name *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtName" CssClass="required" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label3" AssociatedControlID="drlList" CssClass="jform_name-lbl"
                                Text="List *"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlList" CssClass="required" Width="200" DataTextField="Name"
                                DataValueField="Id" OnSelectedIndexChanged="drlList_OnSelectedIndexChanged" />
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label1" AssociatedControlID="drlParentList" CssClass="jform_name-lbl"
                                Text="Parent"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlParentList" CssClass="required" Width="200"
                                DataTextField="Name" DataValueField="Id" AppendDataBoundItems="true">
                                <asp:ListItem runat="server" Value="0">Please Select</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label5" AssociatedControlID="txtDescription" CssClass="jform_name-lbl"
                                Text="Description"></asp:Label>
                            <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Height="50"
                                Width="200"></asp:TextBox>
                            <br />
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
