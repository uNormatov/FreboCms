<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="pollaction.aspx.cs" Inherits="FWeb.Administrator.Poll.pollaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <style>
        .radiobtn-list label
        {
            float: right;
        }
    </style>
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
                    Polls:
                    <asp:Literal runat="server" ID="ltlTitle"></asp:Literal></h2>
            </div>
        </div>
    </div>
    <div id="system-message-container">
        <asp:Literal runat="server" ID="ltlMessage"></asp:Literal>
    </div>
    <div id="element-box">
        <div class="m">
            <div class="width-45 fltlft">
                <fieldset class="adminform">
                    <legend>Poll details</legend>
                    <ul class="adminformlist">
                        <li>
                            <asp:Label runat="server" ID="llbl1" AssociatedControlID="txtQuestion" CssClass="jform_name-lbl"
                                Text="Question *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtQuestion" CssClass="required" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label3" CssClass="jform_name-lbl" AssociatedControlID="rbtBlockTypes"
                                Text="Block repeated voting by"></asp:Label>
                            <div class="radiobtn-list">
                                <asp:RadioButtonList runat="server" ID="rbtBlockTypes" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Selected="true">Cookie</asp:ListItem>
                                    <asp:ListItem Value="2">IP Address</asp:ListItem>
                                    <asp:ListItem Value="3">Don't block</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label1" AssociatedControlID="chbxIsActive" CssClass="jform_name-lbl"
                                Text="Is Active"></asp:Label>
                            <asp:CheckBox runat="server" ID="chbxIsActive" Checked="true" />
                        </li>
                    </ul>
                </fieldset>
            </div>
            <div class="width-55 fltlft">
                <fieldset class="adminform">
                    <legend>Poll Result</legend>
                </fieldset>
            </div>
            <div class="clr">
            </div>
        </div>
    </div>
    <input type="hidden" name="form-action" id="form-action" />
    <input type="hidden" name="checkboxCount" id="checkboxCount" value="0" />
</asp:Content>
