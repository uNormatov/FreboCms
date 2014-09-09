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

namespace FWeb.Administrator.ContentType
{
    public partial class action : FAdminEditPage
    {
        private ContentTypeProvider _contentTypeProvider;

        protected override void Init()
        {
            base.Init();
            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();


            if (IsEdit)
            {
                Title = "Edit Content Type | Frebo Cms";
                ltlTitle.Text = "Edit";
            }
            else
            {
                Title = "New Content Type  | Frebo Cms";
                ltlTitle.Text = "New";
            }
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/contenttype/default.aspx";
            RedrictUrl = CancelUrl;
        }

        protected override void FillFields()
        {
            if (IsEdit)
            {
                ContentTypeInfo info = _contentTypeProvider.Select(ValidationHelper.GetInteger(Id, -1), ErrorList);
                if (info != null)
                {
                    txtName.Text = info.Name;
                    txtDescription.Text = info.Description;
                }
            }

        }

        protected override bool Update()
        {
            ContentTypeInfo info = _contentTypeProvider.Select(ValidationHelper.GetInteger(Id, -1), ErrorList);
            if (info != null)
            {

                info.Name = txtName.Text;
                info.Description = txtDescription.Text;
                if (flpScreenshot.HasFile)
                {
                    string guid = Guid.NewGuid().ToString();
                    string fileName = guid + "_" + flpScreenshot.FileName;
                    flpScreenshot.SaveAs(Server.MapPath("~/userfiles/") + fileName);
                    info.Image = fileName;
                }

                _contentTypeProvider.Update(info, ErrorList);
            }
            return CheckErrors();
        }

        protected override bool Insert()
        {

            ContentTypeInfo info = new ContentTypeInfo();
            info.Name = txtName.Text;
            info.TableName = txtTableName.Text;
            info.Description = txtDescription.Text;
            if (flpScreenshot.HasFile)
            {
                string guid = Guid.NewGuid().ToString();
                string fileName = guid + "_" + flpScreenshot.FileName;
                flpScreenshot.SaveAs(Server.MapPath("~/userfiles/") + fileName);
                info.Image = fileName;
            }
            else
                info.Image = "";

            _contentTypeProvider.Create(info, ErrorList);
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