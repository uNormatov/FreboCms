<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainDashboard.ascx.cs"
    Inherits="FWeb.UserControls.MainDashboard" %>
<script type="text/javascript">
    $(function () {
        $("#accordion").accordion();
    });
</script>
<div class="cpanel-left">
    <div id="cpanel">
        <div>
            <div class="icon-wrapper">
                <div class="icon">
                    <a href="/administrator/article">
                        <img alt="Article Manager" src="/content/css/images/menu/article-48.png" />
                        <span>Article Manager</span></a>
                </div>
            </div>
        </div>
        <div class="icon-wrapper">
            <div class="icon">
                <a href="/administrator/list/">
                    <img alt="List Manager" src="/content/css/images/menu/lists-48.png" />
                    <span>List Manager</span></a>
            </div>
        </div>
        <div class="icon-wrapper">
            <div class="icon">
                <a href="/administrator/page/">
                    <img alt="Page Manager" src="/content/css/images/menu/page-48.png" />
                    <span>Page Manager</span></a>
            </div>
        </div>
        <div class="icon-wrapper">
            <div class="icon">
                <a href="/administrator/pagelayout/">
                    <img alt="Page Layout Manager" src="/content/css/images/menu/pagelayout-48.png" />
                    <span>Page Layout Manager</span></a>
            </div>
        </div>
        <div class="icon-wrapper">
            <div class="icon">
                <a href="/administrator/sitelayout/">
                    <img alt="Site Layout Manager" src="/content/css/images/menu/sitelayout-48.png" />
                    <span>Site Layout Manager</span></a>
            </div>
        </div>
        <div class="icon-wrapper">
            <div class="icon">
                <a href="/administrator/webpart/">
                    <img alt="Web Part Manager" src="/content/css/images/menu/webpart-48.png" />
                    <span>Web Part Manager</span></a>
            </div>
        </div>
        <div class="icon-wrapper">
            <div class="icon">
                <a href="/administrator/contenttype/">
                    <img alt="Content Type Manager" src="/content/css/images/menu/contenttype-48.png" />
                    <span>Content Type Manager</span></a>
            </div>
        </div>
        <div class="icon-wrapper">
            <div class="icon">
                <a href="/administrator/user/">
                    <img alt="User Manager" src="/content/css/images/menu/users-48.png" />
                    <span>User Manager</span></a>
            </div>
        </div>
        <div class="icon-wrapper">
            <div class="icon">
                <a href="/administrator/user/roles.aspx">
                    <img alt="Role Manager" src="/content/css/images/menu/roles-48.png" />
                    <span>Role Manager</span></a>
            </div>
        </div>
        <div class="icon-wrapper">
            <div class="icon">
                <a href="/administrator/localization/">
                    <img alt="Localization Manager" src="/content/css/images/menu/language-48.png" />
                    <span>Localization Manager</span></a>
            </div>
        </div>
        <div class="icon-wrapper">
            <div class="icon">
                <a href="/administrator/site/">
                    <img alt="" src="/content/css/images/menu/settings-48.png" />
                    <span>Global Configuration</span></a>
            </div>
        </div>
    </div>
</div>
<div class="cpanel-right">
    <div class="pane-sliders" id="panel-sliders">
        <div id="accordion">
            <h3>
                Last 5 Logged-in Users</h3>
            <div class="pane-slider content">
            </div>
            <h3>
                Top 5 Popular Articles</h3>
            <div class="pane-slider content">
            </div>
            <h3>
                Last 5 Added Articles</h3>
            <div class="pane-slider content">
            </div>
        </div>
    </div>
</div>
