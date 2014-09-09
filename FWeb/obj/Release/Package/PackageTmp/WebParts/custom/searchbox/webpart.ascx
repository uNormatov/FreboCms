<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="webpart.ascx.cs" Inherits="FWeb.WebParts.custom.searchbox.webpart" %>
<input id="txtSearch" class="txt" value="Sayt bo'yicha qidiruv" type="text" onblur="if (this.value == '') {this.value = 'Sayt bo\'yicha qidiruv';}"
    onfocus="if (this.value == 'Sayt bo\'yicha qidiruv') {this.value = '';}" />
<input class="btn-search" value="Qidirish" type="button" id="btnSearch" />
<div class="clear overhide">
    <ul>
        <asp:Literal runat="server" ID="ltlAlphabet"></asp:Literal>
      
    </ul>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $("#btnSearch").click(function () {
            if ($("#txtSearch").val() == '') {
                return false;
            }
            else {
                window.location = '/search/keyword/barcha/' + $("#txtSearch").val();
            }
        });
        $("#form1").submit(function () {
            if ($("#txtSearch").val() == '') {
                return false;
            }
            else {
                window.location = '/search/keyword/barcha/' + $("#txtSearch").val();
                return false;
            }
        });
    });
</script>
