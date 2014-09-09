using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FUIControls.Page;
using FCore.Class;
using FCore.Collection;
using FDataProvider;
using FCore.Helper;
using System.Text;

namespace FWeb.Administrator.SiteLayout
{
    public partial class categoryaction : FAdminEditPage
    {
        private LayoutCategoryProvider _layoutCategoryProvider;

        protected override void Init()
        {
            base.Init();

            if (_layoutCategoryProvider == null)
                _layoutCategoryProvider = new LayoutCategoryProvider();

            RedrictUrl = "/administrator/sitelayout/default.aspx";
            CancelUrl = RedrictUrl;

            if (IsEdit)
            {
                this.Title = "Edit Site Layout Category | Frebo Cms";
                ltlTitle.Text = "Edit Site Layout Category";
            }
            else
            {
                this.Title = "New Site Layout Category | Frebo Cms";
                ltlTitle.Text = "New Site Layout Category";
            }
        }

        protected override void FillFields()
        {
            if (IsEdit)
            {
                LayoutCategoryInfo info = _layoutCategoryProvider.Select(ValidationHelper.GetInteger(Id, -1), ErrorList);
                if (info != null)
                {
                    txtName.Text = info.Name;
                }
                CheckErrors();
            }
        }

        protected override bool Update()
        {
            LayoutCategoryInfo info = _layoutCategoryProvider.Select(ValidationHelper.GetInteger(Id, -1), ErrorList);
            if (info != null)
            {
                info.Name = txtName.Text;
                _layoutCategoryProvider.Update(info, ErrorList);
            }
            return CheckErrors();
        }

        protected override bool Insert()
        {
            LayoutCategoryInfo info = new LayoutCategoryInfo();
            info.Name = txtName.Text;
            info.IsMaster = true;
            ErrorInfoList errorList = new ErrorInfoList();
            _layoutCategoryProvider.Create(info, errorList);
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