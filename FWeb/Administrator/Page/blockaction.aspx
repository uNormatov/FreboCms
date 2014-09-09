<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="blockaction.aspx.cs" Inherits="FWeb.Administrator.Page.blockaction" %>

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
                    Page Manager:
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
                    <legend>Block Details</legend>
                    <ul class="adminformlist">
                        <li>
                            <asp:Label runat="server" ID="llbl1" AssociatedControlID="txtName" CssClass="jform_name-lbl"
                                Text="Name *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtName" CssClass="required" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label3" AssociatedControlID="drlPage" CssClass="jform_name-lbl"
                                Text="Page *"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlPage" CssClass="required" Width="200" DataTextField="Name"
                                DataValueField="Id" OnSelectedIndexChanged="drlPage_OnSelectedIndexChanged" AutoPostBack="true" />
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label5" AssociatedControlID="drlWebPart" CssClass="jform_name-lbl"
                                Text="Web Part *"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlWebPart" CssClass="required" Width="200"
                                DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="drlWebPart_OnSelectedIndexChanged"
                                AutoPostBack="true" AppendDataBoundItems="true">
                                <asp:ListItem Value="" Text="Please Select"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label2" AssociatedControlID="drlOrder" CssClass="jform_name-lbl"
                                Text="Order *"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlOrder" CssClass="required" Width="200" DataTextField="Name"
                                DataValueField="Id">
                                <asp:ListItem Text="1" Value="1" Selected="true"></asp:ListItem>
                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label4" AssociatedControlID="drlLanguages" CssClass="jform_name-lbl"
                                Text="Language "></asp:Label>
                            <asp:DropDownList runat="server" ID="drlLanguages" />
                        </li>
                    </ul>
                </fieldset>
            </div>
            <div class="width-60 fltlft">
                <fieldset class="adminform">
                    <legend>Webpart Details</legend>
                    <%--  <asp:UpdatePanel runat="server" ID="updWebPart">
                        <Triggers>
                            <asp:AsyncPostBackTrigger runat="server" ControlID="drlLayout" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger runat="server" ControlID="drlWebPart" EventName="SelectedIndexChanged" />
                        </Triggers>
                        <ContentTemplate>--%>
                    <ul class="adminformlist">
                        <li>
                            <asp:Label runat="server" ID="Label1" AssociatedControlID="drlWebPartZoneName" CssClass="jform_name-lbl"
                                Text="WebPart Zone *"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlWebPartZoneName" CssClass="required" Width="200"
                                DataTextField="Name" DataValueField="Name" />
                        </li>
                        <li>
                            <asp:Panel runat="server" ID="pnlWebPart">
                            </asp:Panel>
                        </li>
                    </ul>
                    <%--    </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </fieldset>
            </div>
            <div class="clr">
            </div>
        </div>
    </div>
    <input type="hidden" name="form-action" id="form-action" />
    <input type="hidden" name="checkboxCount" id="checkboxCount" value="0" />
</asp:Content>
