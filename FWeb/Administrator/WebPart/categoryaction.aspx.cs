using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;

namespace FWeb.Administrator.WebPart
{
    public partial class categoryaction : FAdminEditPage
    {
        private WebPartCategoryProvider _webpartCategoryProvider;

        protected override void Init()
        {
            base.Init();

            if (_webpartCategoryProvider == null)
                _webpartCategoryProvider = new WebPartCategoryProvider();

            RedrictUrl = "/administrator/webpart/default.aspx";
            CancelUrl = RedrictUrl;

            if (IsEdit)
            {
                this.Title = "Edit WebPart Category | Frebo Cms";
                ltlTitle.Text = "Edit WebPart Category";
            }
            else
            {
                this.Title = "New WebPart Category | Frebo Cms";
                ltlTitle.Text = "New WebPart Category";
            }
        }

        protected override void FillFields()
        {
            if (IsEdit)
            {
                WebPartCategoryInfo info = _webpartCategoryProvider.Select(ValidationHelper.GetInteger(Id, -1), ErrorList);
                if (info != null)
                {
                    txtName.Text = info.Name;
                    txtDescription.Text = info.Description;
                }
                CheckErrors();
            }
        }

        protected override bool Update()
        {
            WebPartCategoryInfo info = _webpartCategoryProvider.Select(ValidationHelper.GetInteger(Id, -1), ErrorList);
            if (info != null)
            {
                info.Name = txtName.Text;
                info.Description = txtDescription.Text;
                _webpartCategoryProvider.Update(info, ErrorList);
            }
            return CheckErrors();
        }

        protected override bool Insert()
        {
            WebPartCategoryInfo info = new WebPartCategoryInfo();
            info.Name = txtName.Text;
            info.Description = txtDescription.Text;
            ErrorInfoList errorList = new ErrorInfoList();
            _webpartCategoryProvider.Create(info, errorList);
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