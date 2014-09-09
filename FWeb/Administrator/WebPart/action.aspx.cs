using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;

namespace FWeb.Administrator.WebPart
{
    public partial class action : FAdminEditPage
    {
        private WebPartProvider _webPartProvider;
        private WebPartCategoryProvider _webpartCategoryProvider;


        private int CategoryId
        {
            get
            {
                if (ViewState["_categroyId"] == null)
                {
                    int categoryId = ValidationHelper.GetInteger(Request.QueryString["categoryid"], -1);
                    if (categoryId == -1)
                    {
                        List<WebPartCategoryInfo> layoutCategories = _webpartCategoryProvider.SelectPagingSorting(1, 1, "Id", "DESC", ErrorList);
                        if (layoutCategories != null && layoutCategories.Count > 0)
                            categoryId = layoutCategories[0].Id;
                    }
                    return categoryId;
                }
                return ValidationHelper.GetInteger(ViewState["_categroyId"], 20);
            }
            set { ViewState["_categroyId"] = value; }
        }

        protected override void Init()
        {
            base.Init();
            if (_webPartProvider == null)
                _webPartProvider = new WebPartProvider();
            if (_webpartCategoryProvider == null)
                _webpartCategoryProvider = new WebPartCategoryProvider();

            if (IsEdit)
            {
                Title = "Edit WebPart | Frebo Cms";
                ltlTitle.Text = "Edit WebPart";
            }
            else
            {
                this.Title = "New WebPart| Frebo Cms";
                ltlTitle.Text = "New WebPart";
            }

        }

        protected override void Load()
        {
            CancelUrl = "/administrator/webpart/webparts.aspx?categoryid=" + CategoryId;
            base.Load();
        }

        protected override void FillFields()
        {
            List<WebPartCategoryInfo> categories = _webpartCategoryProvider.SelectAll(ErrorList);
            if (ErrorList.Count > 0)
            {
                PrintErrors();
            }
            else
            {
                drlCategory.DataSource = categories;
                drlCategory.DataBind();
                drlCategory.SelectedValue = CategoryId.ToString();
            }

            if (IsEdit)
            {
                WebPartInfo info = _webPartProvider.Select(ValidationHelper.GetInteger(Id, -1), ErrorList);
                if (info != null)
                {
                    txtName.Text = info.Name;
                    drlCategory.SelectedValue = info.WebPartCategoryId.ToString();
                    txtFolderPath.Text = info.FolderPath;
                    txtDescription.Text = info.Description;
                }
            }

        }

        protected override bool Update()
        {
            WebPartInfo info = _webPartProvider.Select(ValidationHelper.GetInteger(Id, -1), ErrorList);
            if (info != null)
            {
                info.WebPartCategoryId = ValidationHelper.GetInteger(drlCategory.SelectedValue, 0);
                info.Name = txtName.Text;
                info.FolderPath = txtFolderPath.Text;
                info.Description = txtDescription.Text;
                info.IsDeleted = false;

                string guid = Guid.NewGuid().ToString();
                string fileName = guid + "_" + flpScreenshot.FileName;
                flpScreenshot.SaveAs(Server.MapPath("~/userfiles/") + fileName);
                info.Screenshot = fileName;

                _webPartProvider.Update(info, ErrorList);


            }
            RedrictUrl = "/administrator/webpart/webparts.aspx?categoryid=" + drlCategory.SelectedValue;
            CancelUrl = RedrictUrl;

            return CheckErrors();
        }

        protected override bool Insert()
        {
            WebPartInfo info = new WebPartInfo();
            info.WebPartCategoryId = ValidationHelper.GetInteger(drlCategory.SelectedValue, 0);
            info.Name = txtName.Text;
            info.FolderPath = txtFolderPath.Text;
            info.Description = txtDescription.Text;
            info.IsDeleted = false;
            string guid = Guid.NewGuid().ToString();
            string fileName = guid + "_" + flpScreenshot.FileName;
            flpScreenshot.SaveAs(Server.MapPath("~/userfiles/webparts/") + fileName);
            info.Screenshot = fileName;
            Object idObject = _webPartProvider.Create(info, ErrorList);
            RedrictUrl = "/administrator/webpart/webparts.aspx?categoryid=" + drlCategory.SelectedValue;
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
            ErrorList.Clear();
        }

        protected override void PrintSuccess()
        {
            ErrorList.Clear();
        }
    }
}