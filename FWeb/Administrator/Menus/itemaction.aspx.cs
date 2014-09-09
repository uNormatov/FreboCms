using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;
using FUIControls.Settings;

namespace FWeb.Administrator.Menus
{
    public partial class itemaction : FAdminEditPage
    {
        private MenuProvider _menuProvider;

        private int MenuId
        {
            get
            {
                return ValidationHelper.GetInteger(ViewState["_menuId"], 1);
            }
            set { ViewState["_menuId"] = value; }
        }

        protected override void Init()
        {
            base.Init();

            if (_menuProvider == null)
                _menuProvider = new MenuProvider();


            if (IsEdit)
            {
                Title = "Edit Menu Item | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "Edit Menu Item";
            }
            else
            {
                Title = "New Menu Item | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "New Menu Item";
            }
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/menus/items.aspx?menuid=" + MenuId;
            RedrictUrl = CancelUrl;
        }

        protected override void FillFields()
        {
            List<MenuInfo> menuInfos = _menuProvider.SelectAll(ErrorList);
            drlMenus.DataSource = menuInfos;
            drlMenus.DataBind();


            List<string> roles = new List<string>();
            roles.Add("All");
            roles.AddRange(Roles.GetAllRoles().ToList());
            chbxListRoles.DataSource = roles;
            chbxListRoles.DataBind();

            FillMenuItems();

            int listid = ValidationHelper.GetInteger(Request.QueryString["menuid"], -1);
            if (listid != -1)
            {
                drlList.SelectedValue = listid.ToString();
                MenuId = listid;
            }

            if (IsEdit)
            {
                MenuItemInfo menuItemInfo = _menuProvider.SelectMenuItem(ValidationHelper.GetInteger(Id, 0), ErrorList);
                if (menuItemInfo != null)
                {
                    drlList.SelectedValue = menuItemInfo.MenuId.ToString();
                    MenuId = menuItemInfo.MenuId;
                    txtName.Text = menuItemInfo.Title;
                    txtUrl.Text = menuItemInfo.Url;
                    for (int i = 0; i < drlList.Items.Count; i++)
                    {
                        if (drlList.Items[i].Value == menuItemInfo.ParentId.ToString())
                        {
                            drlList.SelectedIndex = i;
                            break;
                        }
                    }

                    List<MenuItemsInRolesInfo> selectedRoles = _menuProvider.SelectMenuItemsInRolesByMenuItemId(
                        menuItemInfo.Id, ErrorList);

                    foreach (ListItem item in chbxListRoles.Items)
                    {
                        if (selectedRoles.Any(x => x.RoleId.Equals(item.Value)))
                            item.Selected = true;
                    }

                }
            }
        }

        private void FillMenuItems()
        {
            List<MenuItemInfo> menuItemInfos = _menuProvider.SelectMenuItemsByMenuId(MenuId, ErrorList);
            if (CheckErrors())
            {
                int depth = 0;
                drlList.Items.Add(new ListItem("Select", "0", true));
                if (menuItemInfos != null && menuItemInfos.Count > 0)
                    FillChildMenuItems(menuItemInfos, 0, depth);
            }
        }

        private void FillChildMenuItems(List<MenuItemInfo> menuItems, int parentId, int depth)
        {
            List<MenuItemInfo> childPages = menuItems.Where(x => x.ParentId == parentId).ToList();
            if (childPages.Count <= 0) return;

            string childRow = string.Empty;
            for (int i = 0; i < depth; i++)
                childRow += "- ";
            foreach (MenuItemInfo pageInfo in childPages)
            {
                if (pageInfo.Id != ValidationHelper.GetInteger(Id, 0))
                    drlList.Items.Add(new ListItem(string.Format(" - {0}{1}", childRow, pageInfo.Name), pageInfo.Id.ToString()));
                FillChildMenuItems(menuItems, pageInfo.Id, ++depth);
            }
        }

        protected override bool Update()
        {
            MenuItemInfo menuItemInfo = _menuProvider.SelectMenuItem(ValidationHelper.GetInteger(Id, 0), ErrorList);
            if (menuItemInfo != null)
            {
                menuItemInfo.MenuId = MenuId;
                menuItemInfo.Title = txtName.Text;
                menuItemInfo.Url = txtUrl.Text;
                menuItemInfo.OpenType = ValidationHelper.GetInteger(drlPageTarget.SelectedValue, 0);
                menuItemInfo.ParentId = ValidationHelper.GetInteger(drlList.SelectedValue, 0);
                bool result = _menuProvider.UpdateMenuItem(menuItemInfo, ErrorList);
                if (result)
                {
                    _menuProvider.DeleteMenuItemsInRoles(menuItemInfo.Id, ErrorList);
                    foreach (ListItem item in chbxListRoles.Items)
                    {
                        if (item.Selected)
                        {
                            MenuItemsInRolesInfo menuItemsInRolesInfo = new MenuItemsInRolesInfo();
                            menuItemsInRolesInfo.MenuItemId = menuItemInfo.Id;
                            menuItemsInRolesInfo.RoleId = item.Value;
                            _menuProvider.CreateMenuItemsInRoles(menuItemsInRolesInfo, ErrorList);
                        }
                    }
                }
            }
            return CheckErrors();
        }

        protected override bool Insert()
        {
            MenuItemInfo menuItemInfo = new MenuItemInfo();
            menuItemInfo.MenuId = MenuId;
            menuItemInfo.IsMain = false;
            menuItemInfo.IsDeleted = false;
            menuItemInfo.Title = txtName.Text;
            menuItemInfo.Url = txtUrl.Text;
            menuItemInfo.OpenType = ValidationHelper.GetInteger(drlPageTarget.SelectedValue, 0);
            menuItemInfo.ParentId = ValidationHelper.GetInteger(drlList.SelectedValue, 0);
            object menuItemId = _menuProvider.CreateMenuItem(menuItemInfo, ErrorList);
            if (menuItemId != null)
            {
                foreach (ListItem item in chbxListRoles.Items)
                {
                    if (item.Selected)
                    {
                        MenuItemsInRolesInfo menuItemsInRolesInfo = new MenuItemsInRolesInfo();
                        menuItemsInRolesInfo.MenuItemId = ValidationHelper.GetInteger(menuItemId, 0);
                        menuItemsInRolesInfo.RoleId = item.Value;
                        _menuProvider.CreateMenuItemsInRoles(menuItemsInRolesInfo, ErrorList);
                    }
                }
            }
            return CheckErrors();
        }

        protected override void ValidateForm()
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                ErrorList.Add(new ErrorInfo()
                                  {
                                      Message = "Title is required"
                                  });
            }

            if (string.IsNullOrEmpty(txtUrl.Text))
            {
                ErrorList.Add(new ErrorInfo()
                {
                    Message = "Url is required"
                });
            }

            bool roleChecked = false;
            foreach (ListItem item in chbxListRoles.Items)
            {
                if (item.Selected)
                {
                    roleChecked = true;
                    break;
                }
            }
            if (!roleChecked)
            {
                ErrorList.Add(new ErrorInfo()
                {
                    Message = "You have to choose at least one role"
                });
            }
        }

        protected override void PrintErrors()
        {

            StringBuilder builder = new StringBuilder();
            builder.Append("<dl id=\"system-message\">");
            builder.Append("<dt class=\"message\">Message</dt><dd class=\"error\">");
            builder.Append("<ul>");
            foreach (ErrorInfo error in ErrorList)
            {
                builder.AppendFormat("<li>{0} - {1}</li>", error.Name, error.Message);
            }
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
        }

        protected override void PrintSuccess()
        {
            throw new NotImplementedException();
        }
    }
}