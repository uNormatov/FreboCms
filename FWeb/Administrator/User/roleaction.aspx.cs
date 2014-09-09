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

namespace FWeb.Administrator.User
{
    public partial class roleaction : FAdminEditPage
    {

        private RoleProfileProvider _roleProfileProvider;
        private ContentTypeProvider _contentTypeProvider;

        protected override void Init()
        {
            base.Init();
            if (_roleProfileProvider == null)
                _roleProfileProvider = new RoleProfileProvider();
            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();

            if (IsEdit)
            {
                Title = "Edit Role | Frebo Cms";
                ltlTitle.Text = "Edit";
            }
            else
            {
                Title = "New Role | Frebo Cms";
                ltlTitle.Text = "New";
            }
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/user/roles.aspx";
            RedrictUrl = CancelUrl;
        }

        protected override void FillFields()
        {
            List<ContentTypeInfo> contentTypeInfos = new List<ContentTypeInfo>();
            contentTypeInfos.Add(new ContentTypeInfo() { Name = "Select", Id = 0 });
            contentTypeInfos.AddRange(_contentTypeProvider.SelectAll(ErrorList));

            drlContentType.DataTextField = "Name";
            drlContentType.DataValueField = "Id";
            drlContentType.DataSource = contentTypeInfos;
            drlContentType.DataBind();
            if (IsEdit && CheckErrors())
            {

            }
        }

        protected override bool Update()
        {
            return CheckErrors();
        }

        protected override bool Insert()
        {
            Roles.CreateRole(txtName.Text);

            if (drlContentType.SelectedValue != "0")
            {
                RoleProfileInfo roleProfileInfo = new RoleProfileInfo();
                roleProfileInfo.RoleId = txtName.Text;
                roleProfileInfo.ContentTypeId = ValidationHelper.GetInteger(drlContentType.SelectedValue, 0);
                roleProfileInfo.UserProfileQuery = txtUserProfileQuery.Text;
                _roleProfileProvider.Create(roleProfileInfo, ErrorList);
            }
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