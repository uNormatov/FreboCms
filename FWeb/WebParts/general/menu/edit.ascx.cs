using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Helper;
using FDataProvider;
using FUIControls.PortalControl;

namespace FWeb.WebParts.general.menu
{
    public partial class edit : FWebPartEdit
    {
        private const string MenuId = "MenuId";
        private const string ContainerCss = "ContainerCss";
        private const string ItemCss = "ItemCss";
        private const string ActiveItemCss = "ActiveItemCss";
        private const string ChildContainerCss = "ChildContainerCss";
        private const string ChildItemCss = "ChildItemCss";

        private readonly MenuProvider _menuProvider;

        public edit()
            : base("", null)
        {
            _menuProvider = new MenuProvider();
        }

        public edit(string properties, ErrorInfoList errorInfoList)
            : base(properties, errorInfoList)
        {
            _menuProvider = new MenuProvider();
        }


        protected override void GetValues()
        {
            string menuId = GetControlValue(drlMenu.ID);
            SetValue(MenuId, menuId);

            string containerCss = GetControlValue(txtContainerCss.ID);
            SetValue(ContainerCss, containerCss);

            string itemCss = GetControlValue(txtItemCss.ID);
            SetValue(ItemCss, itemCss);

            string activeCss = GetControlValue(txtSelectedItemCss.ID);
            SetValue(ActiveItemCss, activeCss);

            string childContainerCss = GetControlValue(txtChildContainerCss.ID);
            SetValue(ChildContainerCss, childContainerCss);

            string childItemCss = GetControlValue(txtChildItemCss.ID);
            SetValue(ChildItemCss, childItemCss);
        }

        protected override void EnsureControlsValue()
        {
            List<MenuInfo> menus = _menuProvider.SelectAll(ErrorInfoList);
            drlMenu.DataSource = menus;
            drlMenu.DataBind();

            string menuId = ValidationHelper.GetString(GetValue(MenuId), string.Empty);
            if (!string.IsNullOrEmpty(menuId))
                drlMenu.SelectedValue = menuId;

            string containerCss = ValidationHelper.GetString(GetValue(ContainerCss), string.Empty);
            if (!string.IsNullOrEmpty(containerCss))
                txtContainerCss.Text = containerCss;

            string itemCss = ValidationHelper.GetString(GetValue(ItemCss), string.Empty);
            if (!string.IsNullOrEmpty(itemCss))
                txtItemCss.Text = itemCss;

            string activeCss = ValidationHelper.GetString(GetValue(ActiveItemCss), string.Empty);
            if (!string.IsNullOrEmpty(activeCss))
                txtSelectedItemCss.Text = activeCss;

            string childContainerCss = ValidationHelper.GetString(GetValue(ChildContainerCss), string.Empty);
            if (!string.IsNullOrEmpty(childContainerCss))
                txtChildContainerCss.Text = childContainerCss;

            string childItemCss = ValidationHelper.GetString(GetValue(ChildItemCss), string.Empty);
            if (!string.IsNullOrEmpty(childItemCss))
                txtChildItemCss.Text = childItemCss;
        }
    }
}