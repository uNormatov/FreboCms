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

namespace FWeb.Administrator.List
{
    public partial class action : FAdminEditPage
    {
        private ListProvider _listProvider;

        protected override void Init()
        {
            base.Init();

            if (_listProvider == null)
                _listProvider = new ListProvider();


            if (IsEdit)
            {
                Title = "Edit List | Frebo Cms";
                ltlTitle.Text = "Edit";
            }
            else
            {
                Title = "New List | Frebo Cms";
                ltlTitle.Text = "New";
            }
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/list/default.aspx";
            RedrictUrl = CancelUrl;
        }

        protected override void FillFields()
        {
            if (IsEdit && CheckErrors())
            {
                ListInfo listInfo = _listProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
                txtName.Text = listInfo.Name;
                txtDescription.Text = listInfo.Description;
            }
        }

        protected override bool Update()
        {
            ListInfo info = _listProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
            if (info != null)
            {
                info.Name = txtName.Text;
                info.Description = txtDescription.Text;
                _listProvider.Update(info, ErrorList);
            }
            return CheckErrors();
        }

        protected override bool Insert()
        {
            ListInfo info = new ListInfo();
            info.Name = txtName.Text;
            info.Description = txtDescription.Text;
            _listProvider.Create(info, ErrorList);
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