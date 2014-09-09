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
    public partial class transformationaction : FAdminEditPage
    {
        private ContentTypeProvider _contentTypeProvider;
        private TransformationProvider _transformationProvider;

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
            if (_transformationProvider == null)
                _transformationProvider = new TransformationProvider();

            if (IsEdit)
            {
                Title = "Edit Transformation | Frebo Cms";
                ltlTitle.Text = "Edit";
            }
            else
            {
                Title = "New Transformation  | Frebo Cms";
                ltlTitle.Text = "New";
            }
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/contenttype/transformation.aspx?contenttypeid=" + ContentTypeId;
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
                TransformationInfo transformationInfo;
                if (IsByName)
                    transformationInfo = _transformationProvider.SelectByName(Id, ErrorList);
                else
                    transformationInfo = _transformationProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
                if (transformationInfo != null)
                {
                    drlList.SelectedValue = transformationInfo.ContentTypeId.ToString();
                    ContentTypeId = transformationInfo.ContentTypeId;
                    txtName.Text = transformationInfo.Name.Substring(transformationInfo.Name.IndexOf(".") + 1); ;
                    txtQuery.Text = transformationInfo.Text.ToHtmlDecode();
                }
            }
        }

        protected override bool Update()
        {
            ContentTypeInfo contentTypeInfo =
                _contentTypeProvider.Select(ValidationHelper.GetInteger(drlList.SelectedValue, 0), ErrorList);
            if (contentTypeInfo != null)
            {
                TransformationInfo transformationInfo;
                if (IsByName)
                    transformationInfo = _transformationProvider.SelectByName(Id, ErrorList);
                else
                    transformationInfo = _transformationProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
                transformationInfo.Name = string.Format("{0}.{1}", contentTypeInfo.TableName, txtName.Text);
                transformationInfo.Text = txtQuery.Text.ToHtmlEncode();
                transformationInfo.ContentTypeId = ValidationHelper.GetInteger(drlList.SelectedValue, 0);
                _transformationProvider.Update(transformationInfo, ErrorList);
            }
            return CheckErrors();
        }

        protected override bool Insert()
        {
            ContentTypeInfo contentTypeInfo =
                _contentTypeProvider.Select(ValidationHelper.GetInteger(drlList.SelectedValue, 0), ErrorList);
            if (contentTypeInfo != null)
            {

                TransformationInfo transformationInfo = new TransformationInfo();
                transformationInfo.Name = string.Format("{0}.{1}", contentTypeInfo.TableName, txtName.Text);
                transformationInfo.Text = txtQuery.Text.ToHtmlEncode();
                transformationInfo.ContentTypeId = ValidationHelper.GetInteger(drlList.SelectedValue, 0);
                _transformationProvider.Create(transformationInfo, ErrorList);
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