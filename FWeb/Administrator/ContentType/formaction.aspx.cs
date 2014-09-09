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
    public partial class formaction : FAdminEditPage
    {
        private ContentTypeProvider _contentTypeProvider;
        private FormProvider _formProvider;

        private int ContentTypeId
        {
            get
            {
                if (ViewState["_contentTypeId"] == null)
                {
                    int contentTypeId = ValidationHelper.GetInteger(Request.QueryString["contenttypeid"], -1);
                    if (contentTypeId == -1)
                    {
                        List<ContentTypeInfo> contentTypeInfos = _contentTypeProvider.SelectPagingSorting(1, 1, "Id", "DESC", ErrorList);
                        if (contentTypeInfos != null && contentTypeInfos.Count > 0)
                            contentTypeId = contentTypeInfos[0].Id;
                    }
                    return contentTypeId;
                }
                return ValidationHelper.GetInteger(ViewState["_contentTypeId"], 20);
            }
            set { ViewState["_contentTypeId"] = value; }

        }

        protected override void Load()
        {
            CancelUrl = "/administrator/contenttype/form.aspx?contenttypeid=" + ContentTypeId;
            RedrictUrl = CancelUrl;
        }

        protected override void Init()
        {
            base.Init();

            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();
            if (_formProvider == null)
                _formProvider = new FormProvider();

            if (IsEdit)
            {
                Title = "Edit Form | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "Edit Form";
            }
            else
            {
                Title = "New Form  | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "New Form";
            }
        }

        protected override void FillFields()
        {
            List<ContentTypeInfo> contentTypeInfos = _contentTypeProvider.SelectAll(ErrorList);
            drlList.DataSource = contentTypeInfos;
            drlList.DataBind();
            drlList.SelectedIndex = 0;

            if (ContentTypeId != -1)
            {
                drlList.SelectedValue = ContentTypeId.ToString();
            }
            ContentTypeInfo contentTypeInfo = null;
            if (IsEdit)
            {
                FormInfo formInfo;
                if (IsByName)
                    formInfo = _formProvider.SelectByName(Id, ErrorList);
                else
                    formInfo = _formProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
                if (formInfo != null)
                {
                    drlList.SelectedValue = formInfo.ContentTypeId.ToString();
                    txtName.Text = formInfo.Name.Substring(formInfo.Name.IndexOf(".") + 1);
                    txtDisplayName.Text = formInfo.DisplayName;
                    fckEditor.Value = formInfo.Layout.ToHtmlDecode();

                    contentTypeInfo = _contentTypeProvider.Select(ValidationHelper.GetInteger(drlList.SelectedValue, 0), ErrorList);
                    chbxIsDefault.Checked = formInfo.Id == contentTypeInfo.DefaultFormId;
                }
            }
            else
                contentTypeInfo = _contentTypeProvider.Select(ValidationHelper.GetInteger(drlList.SelectedValue, 0), ErrorList);

            if (contentTypeInfo != null)
            {
                FieldInfo[] fieldInfos =
                    FieldInfo.GetFieldArray(contentTypeInfo.FieldsXml).Where(x => x.CreatedBy != "system").ToArray();

                int i = 0;
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    if (i == 0)
                    {
                        ltlFields.Text += string.Format("<option selected value=\"{0}\">{1}</option>",
                                                        fieldInfo.Name,
                                                        fieldInfo.DisplayName);
                        i++;
                    }
                    else
                        ltlFields.Text += string.Format("<option value=\"{0}\">{1}</option>", fieldInfo.Name,
                                                        fieldInfo.DisplayName);
                }


            }

        }

        protected override bool Update()
        {
            ContentTypeInfo contentTypeInfo =
                 _contentTypeProvider.Select(ValidationHelper.GetInteger(drlList.SelectedValue, 0), ErrorList);
            if (contentTypeInfo != null)
            {
                FormInfo formInfo = _formProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
                formInfo.Name = string.Format("{0}.{1}", contentTypeInfo.TableName, txtName.Text);
                formInfo.DisplayName = txtDisplayName.Text;
                formInfo.Layout = fckEditor.Value.ToHtmlEncode();
                formInfo.ContentTypeId = ValidationHelper.GetInteger(drlList.SelectedValue, 0);
                _formProvider.Update(formInfo, ErrorList);
            }
            return CheckErrors();
        }

        protected override bool Insert()
        {
            ContentTypeInfo contentTypeInfo =
              _contentTypeProvider.Select(ValidationHelper.GetInteger(drlList.SelectedValue, 0), ErrorList);
            if (contentTypeInfo != null)
            {
                FormInfo formInfo = new FormInfo();
                formInfo.Name = string.Format("{0}.{1}", contentTypeInfo.TableName, txtName.Text);
                formInfo.DisplayName = txtDisplayName.Text;
                formInfo.Layout = fckEditor.Value.ToHtmlEncode();
                formInfo.ContentTypeId = ValidationHelper.GetInteger(drlList.SelectedValue, 0);
                int formId = ValidationHelper.GetInteger(_formProvider.Create(formInfo, ErrorList), 0);
                if (formId != 0 && chbxIsDefault.Checked)
                {
                    contentTypeInfo.DefaultFormId = formId;
                }
                _contentTypeProvider.Update(contentTypeInfo, ErrorList);
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

        }
    }
}