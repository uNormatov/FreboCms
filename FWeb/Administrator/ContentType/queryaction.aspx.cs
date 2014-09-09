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

namespace FWeb.Administrator.ContentType
{
    public partial class queryaction : FAdminEditPage
    {
        private ContentTypeProvider _contentTypeProvider;
        private QueryProvider _queryProvider;

        private int ContentTypeId
        {
            get
            {
                return ValidationHelper.GetInteger(ViewState["_contentTypeId"], 1);
            }
            set { ViewState["_contentTypeId"] = value; }
        }

        protected override void Init()
        {
            base.Init();

            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();
            if (_queryProvider == null)
                _queryProvider = new QueryProvider();

            if (IsEdit)
            {
                Title = "Edit Query | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "Edit Query";
            }
            else
            {
                Title = "New Query | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "New Query";
            }
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/contenttype/query.aspx?contenttypeid=" + ContentTypeId;
            RedrictUrl = CancelUrl;
        }

        protected override void FillFields()
        {
            List<ContentTypeInfo> contentTypeInfos = _contentTypeProvider.SelectAll(ErrorList);
            drlList.DataSource = contentTypeInfos;
            drlList.DataBind();

            int listid = ValidationHelper.GetInteger(Request.QueryString["contenttypeid"], -1);
            if (listid != -1)
            {
                drlList.SelectedValue = listid.ToString();
                ContentTypeId = listid;
            }

            if (IsEdit)
            {
                QueryInfo queryInfo;
                if (IsByName)
                    queryInfo = _queryProvider.SelectByName(Id, ErrorList);
                else
                    queryInfo = _queryProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
                if (queryInfo != null)
                {
                    drlList.SelectedValue = queryInfo.ContentTypeId.ToString();
                    ContentTypeId = queryInfo.ContentTypeId;
                    txtName.Text = queryInfo.Name.Substring(queryInfo.Name.IndexOf(".") + 1);
                    txtQuery.Text = queryInfo.Text;
                }
            }
        }

        protected override bool Update()
        {
            ContentTypeInfo contentTypeInfo =
                _contentTypeProvider.Select(ValidationHelper.GetInteger(drlList.SelectedValue, 0), ErrorList);
            if (contentTypeInfo != null)
            {
                QueryInfo queryInfo;
                if (IsByName)
                    queryInfo = _queryProvider.SelectByName(Id, ErrorList);
                else
                    queryInfo = _queryProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
                queryInfo.Name = string.Format("{0}.{1}", contentTypeInfo.TableName, txtName.Text);
                queryInfo.Text = txtQuery.Text;
                queryInfo.ContentTypeId = ValidationHelper.GetInteger(drlList.SelectedValue, 0);
                _queryProvider.Update(queryInfo, ErrorList);
                CacheHelper.DeleteAll(contentTypeInfo.TableName);
            }
            return CheckErrors();
        }

        protected override bool Insert()
        {
            ContentTypeInfo contentTypeInfo =
                _contentTypeProvider.Select(ValidationHelper.GetInteger(drlList.SelectedValue, 0), ErrorList);
            if (contentTypeInfo != null)
            {

                QueryInfo queryInfo = new QueryInfo();
                queryInfo.Name = string.Format("{0}.{1}", contentTypeInfo.TableName, txtName.Text);
                queryInfo.Text = txtQuery.Text;
                queryInfo.ContentTypeId = ValidationHelper.GetInteger(drlList.SelectedValue, 0);
                _queryProvider.Create(queryInfo, ErrorList);
                CacheHelper.DeleteAll(contentTypeInfo.TableName);
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