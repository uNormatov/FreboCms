<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="webpart.ascx.cs" Inherits="FWeb.WebParts.custom.edusearch.webpart" %>


    <table>
        <tbody>
            <tr>
                <td class="maydon">
                    <div>
                        <input name="key" value="" type="text" id="txtSearch">
                    </div>
                </td>
                <td class="tugma">
                    <input type="button" id="btnSearch" alt="" title="" value="Qidiruv">
                </td>
            </tr>
        </tbody>
    </table>

<script type="text/javascript">
    $(document).ready(function () {
        $("#btnSearch").click(function () {
            if ($("#txtSearch").val() == '') {
                return false;
            }
            else {
                window.location = '/qidiruv/barchasi?soz=' + encodeURI($("#txtSearch").val());
            }
        });
        
    });
</script>
