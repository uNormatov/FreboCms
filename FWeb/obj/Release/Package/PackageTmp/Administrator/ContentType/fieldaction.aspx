<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="fieldaction.aspx.cs" Inherits="FWeb.Administrator.ContentType.fieldaction" %>

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
                    Field:
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
                <%if (!IsEdit)
                  {%>
                <fieldset class="adminform">
                    <legend>Database Details</legend>
                    <ul class="adminformlist">
                        <li>
                            <asp:Label runat="server" ID="Label1" AssociatedControlID="txtColumnName" CssClass="jform_name-lbl"
                                Text="Column Name *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtColumnName" CssClass="required" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label3" AssociatedControlID="drlList" CssClass="jform_name-lbl"
                                Text="Content Type *"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlList" CssClass="required" Width="200" DataTextField="Name"
                                DataValueField="Id" />
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label4" AssociatedControlID="drlDataType" CssClass="jform_name-lbl"
                                Text="Data Type *"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlDataType" CssClass="required" Width="200"
                                DataTextField="Key" DataValueField="Value" />
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label5" AssociatedControlID="txtColumnSize" CssClass="jform_name-lbl"
                                Text="Size"></asp:Label>
                            <asp:TextBox runat="server" ID="txtColumnSize" Width="200"></asp:TextBox>
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label9" AssociatedControlID="txtDeafultValue" CssClass="jform_name-lbl"
                                Text="Default Value"></asp:Label>
                            <asp:TextBox runat="server" ID="txtDeafultValue" Width="200"></asp:TextBox>
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label8" AssociatedControlID="chbxIsAllowNull" CssClass="jform_name-lbl"
                                Text="Allow Null"></asp:Label>
                            <asp:CheckBox runat="server" Checked="true" ID="chbxIsAllowNull" />
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label20" AssociatedControlID="chbxUseAsSeoTemplate"
                                CssClass="jform_name-lbl" Text="Use As SeoTemplate"></asp:Label>
                            <asp:CheckBox runat="server" ID="chbxUseAsSeoTemplate" />
                            <br />
                        </li>
                    </ul>
                </fieldset>
                <% }%>
                <fieldset class="adminform">
                    <legend>UI Details</legend>
                    <ul class="adminformlist">
                        <li>
                            <asp:Label runat="server" ID="Label12" AssociatedControlID="txtDiplayName" CssClass="jform_name-lbl"
                                Text="Display Name *"></asp:Label>
                            <asp:TextBox runat="server" ID="txtDiplayName" CssClass="required" Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label15" AssociatedControlID="drlFieldType" CssClass="jform_name-lbl"
                                Text="Field Type *"></asp:Label>
                            <asp:DropDownList runat="server" ID="drlFieldType" CssClass="required" Width="200"
                                DataTextField="Key" DataValueField="Value" OnSelectedIndexChanged="drlFieldType_OnSelectedIndexChanged"
                                AutoPostBack="true" />
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label11" AssociatedControlID="chbxShowInListing" CssClass="jform_name-lbl"
                                Text="Show In Listing"></asp:Label>
                            <asp:CheckBox runat="server" ID="chbxShowInListing" />
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label13" AssociatedControlID="drlOrder" CssClass="jform_name-lbl"
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
                                <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                 <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                <asp:ListItem Text="46" Value="46"></asp:ListItem>
                                <asp:ListItem Text="47" Value="47"></asp:ListItem>
                                <asp:ListItem Text="48" Value="48"></asp:ListItem>
                                <asp:ListItem Text="49" Value="49"></asp:ListItem>
                                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                <asp:ListItem Text="51" Value="51"></asp:ListItem>
                                <asp:ListItem Text="52" Value="52"></asp:ListItem>
                                <asp:ListItem Text="53" Value="53"></asp:ListItem>
                                <asp:ListItem Text="54" Value="54"></asp:ListItem>
                                <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                <asp:ListItem Text="56" Value="56"></asp:ListItem>
                                <asp:ListItem Text="57" Value="57"></asp:ListItem>
                                <asp:ListItem Text="58" Value="58"></asp:ListItem>
                                <asp:ListItem Text="59" Value="59"></asp:ListItem>
                                <asp:ListItem Text="60" Value="60"></asp:ListItem>
                                <asp:ListItem Text="61" Value="61"></asp:ListItem>
                                <asp:ListItem Text="62" Value="62"></asp:ListItem>
                                <asp:ListItem Text="63" Value="63"></asp:ListItem>
                                <asp:ListItem Text="64" Value="64"></asp:ListItem>
                                <asp:ListItem Text="65" Value="65"></asp:ListItem>
                                <asp:ListItem Text="66" Value="66"></asp:ListItem>
                                <asp:ListItem Text="67" Value="67"></asp:ListItem>
                                <asp:ListItem Text="68" Value="68"></asp:ListItem>
                                <asp:ListItem Text="69" Value="69"></asp:ListItem>
                                <asp:ListItem Text="60" Value="60"></asp:ListItem>
                                <asp:ListItem Text="61" Value="61"></asp:ListItem>
                                <asp:ListItem Text="62" Value="62"></asp:ListItem>
                                <asp:ListItem Text="63" Value="63"></asp:ListItem>
                                <asp:ListItem Text="64" Value="64"></asp:ListItem>
                                <asp:ListItem Text="65" Value="65"></asp:ListItem>
                                <asp:ListItem Text="66" Value="66"></asp:ListItem>
                                <asp:ListItem Text="67" Value="67"></asp:ListItem>
                                <asp:ListItem Text="68" Value="68"></asp:ListItem>
                                <asp:ListItem Text="69" Value="69"></asp:ListItem>
                                <asp:ListItem Text="70" Value="70"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </li>
                    </ul>
                </fieldset>
            </div>
            <div class="width-50 fltlft">
                <fieldset class="adminform">
                    <legend>Field Details</legend>
                    <ul class="adminformlist">
                        <li>
                            <asp:Panel runat="server" ID="pnlCustomControlOptions">
                            </asp:Panel>
                        </li>
                    </ul>
                </fieldset>
            </div>
            <div class="width-50 fltlft">
                <fieldset class="adminform">
                    <legend>Validation Details</legend>
                    <ul class="adminformlist">
                        <li>
                            <asp:Label runat="server" ID="Label2" AssociatedControlID="chbxIsRequired" CssClass="jform_name-lbl"
                                Text="Is Required"></asp:Label>
                            <asp:CheckBox runat="server" ID="chbxIsRequired" />
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label6" AssociatedControlID="txtRequiredMessage" CssClass="jform_name-lbl"
                                Text="Required Error Message"></asp:Label>
                            <asp:TextBox runat="server" ID="txtRequiredMessage" TextMode="MultiLine" Height="70"
                                Width="200"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label7" AssociatedControlID="txtRegularExpression"
                                CssClass="jform_name-lbl" Text="Regular Expression"></asp:Label>
                            <asp:TextBox runat="server" ID="txtRegularExpression" Width="200"></asp:TextBox>
                            <br />
                        </li>
                        <li>
                            <asp:Label runat="server" ID="Label10" AssociatedControlID="txtRegularExpressionMessage"
                                CssClass="jform_name-lbl" Text="RE Error Message"></asp:Label>
                            <asp:TextBox runat="server" ID="txtRegularExpressionMessage" TextMode="MultiLine"
                                Height="70" Width="200"></asp:TextBox>
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
