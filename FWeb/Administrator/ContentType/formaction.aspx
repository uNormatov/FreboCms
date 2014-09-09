<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="formaction.aspx.cs" Inherits="FWeb.Administrator.ContentType.formaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var fkcEditorID = '<%= fckEditor.ClientID %>';
        
    </script>
    <script src="/Content/js/formbuilder.js" type="text/javascript"></script>
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
                    Form:
                    <asp:Literal runat="server" ID="ltlTitle"></asp:Literal></h2>
            </div>
        </div>
    </div>
    <div id="system-message-container">
        <asp:Literal runat="server" ID="ltlMessage"></asp:Literal>
    </div>
    <div id="element-box">
        <div class="m">
            <div class="width-35 fltlft">
                <fieldset class="adminform">
                    <legend>Form Details</legend>
                    <ul class="adminformlist">
                        <li>
                            <asp:Label runat="server" ID="llbl1" AssociatedControlID="txtName" CssClass="jform_name-lbl"
                                Text="Name *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtName" CssClass="required" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label1" AssociatedControlID="txtDisplayName" CssClass="jform_name-lbl"
                                Text="Display Name *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtDisplayName" CssClass="required" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label3" AssociatedControlID="drlList" CssClass="jform_name-lbl"
                                Text="Content Type *"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlList" CssClass="required" Width="200" DataTextField="Name"
                                DataValueField="Id" />
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label5" AssociatedControlID="chbxIsDefault" CssClass="jform_name-lbl"
                                Text="Is Default"></asp:Label>
                            <asp:CheckBox runat="server" ID="chbxIsDefault" />
                        </li>
                    </ul>
                </fieldset>
                <fieldset class="adminform">
                    <legend>Fields</legend>
                    <ul class="adminformlist">
                        <li>
                            <input type="button" value="Default" onclick="SetContent(document.getElementById('lstFieldSelect').options); return false;" />
                            <input type="button" value="SaveButton" onclick="InsertField('$$submitbutton$$'); return false;" />
                            <input type="button" value="CancelButton" onclick="InsertField('$$cancelbutton$$'); return false;" />
                            <input type="button" value="Label" onclick="InsertField('$$label:'+$('#lstFieldSelect :selected').val()+'$$'); return false;" />
                            <input type="button" value="Control" onclick="InsertField('$$input:'+$('#lstFieldSelect :selected').val()+'$$'); return false;" />
                            <input type="button" value="Validation" onclick="InsertField('$$validation:'+$('#lstFieldSelect :selected').val()+'$$'); return false;" />
                        </li>
                        <li>
                            <select id="lstFieldSelect" size="10" style="width: 400px;">
                                <asp:Literal runat="server" ID="ltlFields"></asp:Literal>
                            </select>
                        </li>
                    </ul>
                </fieldset>
            </div>
            <div class="width-65 fltlft">
                <fieldset class="adminform">
                    <legend>Form Layout</legend>
                    <ul class="adminformlist">
                        <li>
                            <fr:FCKeditor ID="fckEditor" runat="server" BasePath="~/Content/FCKeditor/" Height="500px" FillEmptyBlocks="false">
                            </fr:FCKeditor>
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
