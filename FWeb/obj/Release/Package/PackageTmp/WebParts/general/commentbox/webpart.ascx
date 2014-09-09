<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="webpart.ascx.cs" Inherits="FWeb.WebParts.general.commentbox.webpart" %>
<%@ Import Namespace="FCore.Helper" %>
<div id="commentbox" class="add-comments">
    <%if (Page.User.Identity.IsAuthenticated)
      { %>
    <h2>
        Fikrlar</h2>
    <img alt="" src="/themes/kitobim/images/art_user_1.png" class="left" />
    <textarea id="txtBody" rows="1" cols="1"></textarea>
    <a href="javascript:void(0);" id="btnSave" class="btn-1 right"><span>Saqlash</span></a>
    <% }%>
</div>
<img src="/themes/kitobim/images/ajax-loader.gif" id="ajaxload" style="margin-top: 20px;
    margin-left: 50px;" />
<div class="view-comments" id="commentList">
</div>
<script type="text/javascript">
    $(document).ready(function () {
        var url = '<%= Request.RawUrl %>';
        var order = 0;
        $.ajax({
            type: "get",
            url: "/comment.ashx",
            data: "action=list&url=" + url,
            dataType: "json",
            success: function (data) {
                $("#ajaxload").hide();
                $.each(data, function (index, item) {
                    var html = '<div class="comment">';
                    html += '<img class="user-photo" src="/themes/kitobim/images/art_user_1.png" alt="" />';
                    html += '<span class="user-name">' + item.Name + '</span>';
                    html += '<p>' + item.Body + '</p>';
                    html += '<a href="#" class="right">' + item.Time + '</a></div>';
                    order = item.Order;
                    $("#commentList").append(html).slideDown();
                });
            }
        });

        $("#btnSave").click(function () {
            if ($("#txtBody").val().length == 0)
                return;
            var data = 'action=add&contenttypeid=<%= ContentTypeId %>&email=<%= Page.User.Identity.Name %>&body=' + $("#txtBody").val() + '&seotemplate=<%= SeoTemplate %>&order=' + order + '&url=' + url + '&name=<%= Page.User.Identity.Name %>';
            $("#ajaxload").show();
            $.getJSON('/comment.ashx', data, function (item) {
                if (item.IsOk) {
                    $("#ajaxload").hide();
                    var html = '<div class="comment">';
                    html += '<img class="user-photo" src="/themes/kitobim/images/art_user_1.png" alt="" />';
                    html += '<span class="user-name">' + item.Name + '</span>';
                    html += '<p>' + item.Body + '</p>';
                    html += '<a href="#" class="right">' + item.Time + '</a></div>';
                    order = item.Order;
                    $("#commentList").append(html).slideDown();
                }
            });
        });
    });
</script>
