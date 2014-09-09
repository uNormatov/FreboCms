using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;
using FUIControls.Settings;

namespace FWeb.Administrator.Menus
{
    public partial class action : FAdminEditPage
    {
        private MenuProvider _menuProvider;

        protected override void Init()
        {
            base.Init();

            if (_menuProvider == null)
                _menuProvider = new MenuProvider();


            if (IsEdit)
            {
                Title = "Edit Menu | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "Edit";
            }
            else
            {
                Title = "New Menu | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "New";
            }
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/menus/default.aspx";
            RedrictUrl = CancelUrl;
        }

        protected override void FillFields()
        {
            if (IsEdit && CheckErrors())
            {
                MenuInfo listInfo = _menuProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
                txtName.Text = listInfo.Name;
                txtDescription.Text = listInfo.Description;
            }
        }

        protected override bool Update()
        {
            MenuInfo info = _menuProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
            if (info != null)
            {
                info.Name = txtName.Text;
                info.Description = txtDescription.Text;
                _menuProvider.Update(info, ErrorList);
            }
            return CheckErrors();
        }

        protected override bool Insert()
        {
            MenuInfo info = new MenuInfo();
            info.Name = txtName.Text;
            info.Description = txtDescription.Text;
            _menuProvider.Create(info, ErrorList);
            return CheckErrors();
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