using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.PortalControl;

namespace FWeb.WebParts.general.menu
{
    public partial class webpart : FWebPart
    {
        private const string MenuId = "MenuId";
        private const string ContainerCss = "ContainerCss";
        private const string ItemCss = "ItemCss";
        private const string ActiveItemCss = "ActiveItemCss";
        private const string ChildContainerCss = "ChildContainerCss";
        private const string ChildItemCss = "ChildItemCss";

        private MenuProvider _menuProvider;
        private StringBuilder _menuBuilder;

        private String _containerCss;
        private string _itemCss;
        private string _selectedItemCss;
        private string _childContainerCss;
        private string _childItemCss;

        protected override void LoadWebPart()
        {
            if (_menuProvider == null)
                _menuProvider = new MenuProvider();

            int menuId = ValidationHelper.GetInteger(GetProperty(MenuId), 0);

            _containerCss = GetProperty(ContainerCss);
            _itemCss = GetProperty(ItemCss);
            _selectedItemCss = GetProperty(ActiveItemCss);
            _childContainerCss = GetProperty(ChildContainerCss);
            _childItemCss = GetProperty(ChildItemCss);

            string roles = GetRolesString();
            List<MenuItemInfo> menuItems = _menuProvider.SelectMenuItemsByMenuIdRoles(menuId, roles, new ErrorInfoList());
            _menuBuilder = new StringBuilder();

            if (menuItems != null)
                BuildChildMenu(menuItems, 0, 0);
            ltlMain.Text = _menuBuilder.ToString();
        }

        private string GetRolesString()
        {
            string roles = "'All'";
            if (Page.User.Identity.IsAuthenticated)
            {
                string userName = Page.User.Identity.Name;
                string[] userRoles = Roles.GetRolesForUser(userName);
                if (userRoles != null && userRoles.Length > 0)
                {
                    foreach (string item in userRoles)
                    {
                        if (roles.Length > 0)
                            roles += ",";
                        roles += string.Format("'{0}'", item);
                    }
                }
            }
            return roles;
        }

        private void BuildChildMenu(List<MenuItemInfo> menuItems, int parentId, int depth)
        {

            List<MenuItemInfo> menus = menuItems.Where(x => x.ParentId == parentId).ToList();
            if (menus != null && menus.Count > 0)
            {
                string css = depth == 0 ? _containerCss : _childContainerCss;
                if (string.IsNullOrEmpty(css))
                    _menuBuilder.Append("<ul itemscope itemtype=\"http://schema.org/SiteNavigationElement\">");
                else
                    _menuBuilder.Append("<ul itemscope itemtype=\"http://schema.org/SiteNavigationElement\" class=\"" + css + "\">");
            }

            string innerCss = depth == 0 ? _itemCss : _childItemCss;

            foreach (MenuItemInfo item in menuItems)
            {
                string css = string.Empty;
                if (PageInfo.SeoUrl.Equals(item.Url.ToLower()))
                {
                    if (!string.IsNullOrEmpty(_selectedItemCss))
                        css = _selectedItemCss;
                }
                else css = innerCss;
                css = string.IsNullOrEmpty(css) ? string.Empty : "class=\"" + css + "\"";
                string target = string.Empty;
                switch (item.OpenType)
                {
                    case (int)MenuPageOpenType.OpenSameTab:
                        target = string.Empty;
                        break;
                    case (int)MenuPageOpenType.OpenNewTab:
                        target = "target=\"_blank\"";
                        break;
                }
                _menuBuilder.AppendFormat("<li itemprop=\"url\" {0}><a itemprop=\"name\" {1} title=\"{3}\" href=\"{2}\">{3}</a></li>", css, target, BuildUrl(item.Url),
                                         GetResourceByPattern(item.Title));
            }
            _menuBuilder.Append("</ul>");
        }
    }
}