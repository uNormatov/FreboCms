<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainMenu.ascx.cs" Inherits="FWeb.UserControls.MainMenu" %>
<div id="module-menu">
    <%if (Request.QueryString["type"] == null || Request.QueryString["type"].ToLower() != "entry")
      {%>
    <ul id="menu">
        <li class="node"><a href="#">Main</a>
            <ul>
                <li><a href="/administrator/Default.aspx" class="icon-16-cpanel">Dashboard</a></li>
                <li class="separator"><span></span></li>
                <li><a href="#" class="icon-16-profile">My Profile</a></li>
                <li class="separator"><span></span></li>
                <li><a href="#" class="icon-16-config">Global Configuration</a></li>
                <li class="separator"><span></span></li>
                <li><a href="/administrator/pages/clearcaches.aspx?returnUrl=<%= Request.Url %>"
                    class="icon-16-clearcache">Clear Caches</a></li>
                <li><a href="/administrator/login.aspx?action=logout" class="icon-16-logout">Logout</a></li>
            </ul>
        </li>
        <li class="node"><a href="/administrator/user">Users</a><ul>
            <li class="node"><a href="/administrator/user" class="icon-16-user">User Manager</a><ul
                class="menu-component" id="menu-com-users-users">
                <li><a href="/administrator/user/action.aspx?type=entry" class="icon-16-newarticle">
                    Add New User</a></li>
            </ul>
            </li>
            <li class="node"><a href="/administrator/user/roles.aspx" class="icon-16-groups">Roles</a>
                <ul class="menu-component" id="menu-com-users-groups">
                    <li><a href="/administrator/user/roleaction.aspx?type=entry" class="icon-16-newarticle">
                        Add New Role</a></li>
                </ul>
            </li>
        </ul>
        </li>
        <li class="node"><a href="#">Menus</a><ul>
            <li class="node"><a href="/administrator/menus/" class="icon-16-menumgr">Menu Manager</a><ul
                class="menu-component" id="menu-com-menus-menus">
                <li><a href="/administrator/menus/action.aspx?type=entry" class="icon-16-newarticle">Add New Menu</a></li>
            </ul>
            </li>
        </ul>
        </li>
        <li class="node"><a href="#">Content</a>
            <ul>
                <li class="node"><a href="/administrator/article/" class="icon-16-article">Article Manager</a>
                    <ul class="menu-component">
                        <li><a href="/administrator/article/action.aspx?type=entry" class="icon-16-newarticle">
                            New Article</a></li>
                    </ul>
                </li>
                <li class="node"><a href="/administrator/list" class="icon-16-listitems">Lists</a>
                    <ul class="menu-component">
                        <li><a href="/administrator/list/action.aspx?type=entry" class="icon-16-newarticle">
                            Add New List</a></li>
                    </ul>
                </li>
                <li class="separator"><span></span></li>
                <asp:Literal runat="server" ID="ltlContents"></asp:Literal>
                <li><a href="/administrator/localization/" class="icon-16-localization">Localization
                    Manager</a></li>
            </ul>
        </li>
        <li class="node"><a href="#">Structure</a><ul>
            <li class="node"><a href="/administrator/page" class="icon-16-pages">Pages</a><ul
                class="menu-component" id="menu-com-banners">
                <li><a href="/administrator/page/action.aspx?type=entry" class="icon-16-newarticle">
                    Add New Page</a></li>
            </ul>
            </li>
            <li class="node"><a href="/administrator/pagelayout" class="icon-16-pagelayouts">Page
                Layouts</a>
                <ul class="menu-component">
                    <li><a href="/administrator/pagelayout/action.aspx?type=entry" class="icon-16-newarticle">
                        Add New Page Layout</a></li>
                </ul>
            </li>
            <li class="node"><a href="/administrator/sitelayout" class="icon-16-sitelayouts">Site
                Layouts</a>
                <ul class="menu-component">
                    <li><a href="/administrator/sitelayout/action.aspx?type=entry" class="icon-16-newarticle">
                        Add New Site Layout</a></li>
                </ul>
            </li>
        </ul>
        </li>
        <li class="node"><a href="#">Development</a><ul>
            <li class="node"><a href="/administrator/webpart" class="icon-16-webpart">Web Parts</a>
                <ul class="menu-component" id="Ul3">
                    <li><a href="/administrator/webpart/action.aspx?type=entry" class="icon-16-newarticle">
                        Add New Web Part</a></li>
                </ul>
            </li>
            <li class="separator"><span></span></li>
            <li class="node"><a href="/administrator/contenttype/" class="icon-16-contenttype">Content
                Types</a>
                <ul class="menu-component" id="Ul6">
                    <li><a href="" class="icon-16-newarticle">Add New Content Type</a></li>
                </ul>
            </li>
        </ul>
        </li>
    </ul>
    <%}
      else
      { %>
    <ul id="menu" class="disabled">
        <li class="disabled"><a>Main</a> </li>
        <li class="disabled"><a>Users</a></li>
        <li class="disabled"><a>Menus</a></li>
        <li class="disabled"><a>Content</a></li>
        <li class="disabled"><a>Structure</a></li>
        <li class="disabled"><a>Development</a> </li>
    </ul>
    <%} %>
</div>
