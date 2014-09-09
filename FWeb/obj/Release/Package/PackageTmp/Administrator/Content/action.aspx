<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="action.aspx.cs" Inherits="FWeb.Administrator.Content.action" ViewStateMode="Enabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <div id="toolbar-box">
        <div class="m">
            <div id="toolbar" class="toolbar-list">
                <ul>
                    <%if (!IsPublished)
                      { %>
                    <li id="toolbar-apply" class="button"><a class="toolbar" onclick="Frebo.formsubmit('publish',true)"
                        href="#"><span class="icon-32-publish"></span>Publish </a></li>
                    <% }
                      else
                      { %>
                    <li id="toolbar-apply" class="button"><a class="toolbar" onclick="Frebo.formsubmit('unpublish',true)"
                        href="#"><span class="icon-32-unpublish"></span>Unpublish </a></li>
                    <% } %>
                    <li class="divider"></li>
                    <li id="toolbar-apply" class="button"><a class="toolbar" onclick="Frebo.formsubmit('save',true)"
                        href="#"><span class="icon-32-save"></span>Save </a></li>
                    <li id="toolbar-cancel" class="button"><a class="toolbar" onclick="Frebo.formsubmit('cancel')"
                        href="#"><span class="icon-32-cancel"></span>Cancel </a></li>
                </ul>
                <div class="clr">
                </div>
            </div>
            <div class="pagetitle icon-48-sitelayoutmgr">
                <h2>Content Manager:
                    <asp:Literal runat="server" ID="ltlTitle"></asp:Literal></h2>
            </div>
        </div>
    </div>
    <div id="system-message-container">
        <asp:Literal runat="server" ID="ltlMessage"></asp:Literal>
    </div>
    <div id="element-box">
        <div class="m">
            <div class="width-60 fltlft">
                <fieldset class="adminform">
                    <legend>
                        <asp:Literal runat="server" ID="ltlFieldSetTitle"></asp:Literal>
                    </legend>
                    <fr:MainForm runat="server" ID="mainForm"></fr:MainForm>
                </fieldset>
            </div>
            <div class="width-40 fltlft">
                <fieldset class="adminform">
                    <legend>Additional Options </legend>
                    <ul class="adminformlist">
                        <li>
                            <asp:Label runat="server" ID="Label1" AssociatedControlID="txtSeo" CssClass="jform_name-lbl"
                                Text="Seo Alias"></asp:Label>
                            <asp:TextBox runat="server" ID="txtSeo" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label2" AssociatedControlID="drlLanguages" CssClass="jform_name-lbl"
                                Text="Language "></asp:Label>
                            <asp:DropDownList runat="server" ID="drlLanguages" />
                        </li>
                    </ul>
                </fieldset>
                <fieldset class="adminform">
                    <legend>Metadata Options</legend>
                    <ul class="adminformlist">
                        <li>
                            <asp:Label runat="server" ID="Label3" AssociatedControlID="txtMetaTile" CssClass="jform_name-lbl"
                                Text="Meta Title"></asp:Label>
                            <asp:TextBox runat="server" ID="txtMetaTile" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label10" AssociatedControlID="txtMetaDescription" CssClass="jform_name-lbl"
                                Text="Meta Description"></asp:Label>
                            <asp:TextBox runat="server" ID="txtMetaDescription" TextMode="MultiLine" Height="50"
                                Width="250"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label11" AssociatedControlID="txtMetaKeywords" CssClass="jform_name-lbl"
                                Text="Meta Keywords"></asp:Label>
                            <asp:TextBox runat="server" ID="txtMetaKeywords" TextMode="MultiLine" Height="50"
                                Width="250"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label12" AssociatedControlID="txtCopyRights" CssClass="jform_name-lbl"
                                Text="Copy Rights"></asp:Label>
                            <asp:TextBox runat="server" ID="txtCopyRights" TextMode="MultiLine" Height="50"
                                Width="250"></asp:TextBox>
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
