using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;
using FUIControls.Settings;

namespace FWeb.Administrator.Content
{
    public partial class action : FAdminEditPage
    {
        private ContentTypeProvider _contentTypeProvider;
        private QueryProvider _queryProvider;
        private LocalizationProvider _localizationProvider;

        private int ContentTypeId
        {
            get
            {
                return ValidationHelper.GetInteger(ViewState["_contentTypeId"], 1);
            }
            set { ViewState["_contentTypeId"] = value; }
        }

        protected bool IsPublished { get; set; }

        protected override void Init()
        {
            base.Init();

            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();
            if (_queryProvider == null)
                _queryProvider = new QueryProvider();
            if (_localizationProvider == null)
                _localizationProvider = new LocalizationProvider();

            if (IsEdit)
            {
                Title = "Edit Content | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "Edit";
            }
            else
            {
                Title = "New Content  | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "New";
            }

            int contentTypeid = ValidationHelper.GetInteger(Request.QueryString["contenttypeid"], -1);
            if (contentTypeid != -1)
            {
                ContentTypeId = contentTypeid;
                ContentTypeInfo contentTypeInfo = _contentTypeProvider.Select(contentTypeid, ErrorList);
                ltlFieldSetTitle.Text = contentTypeInfo.Name;
            }
            if (IsEdit)
            {
                mainForm.FormMode = FormActionMode.Edit;
                mainForm.ContentId = ValidationHelper.GetInteger(Id, 0);
            }
            mainForm.ContentTypeId = ContentTypeId;
            mainForm.ErrorInfoList = ErrorList;
            mainForm.LoadData();
            IsPublished = mainForm.ContentTypeModel.IsPublished;
            FillLanguages();
            if (ErrorList == null)
                ErrorList = new ErrorInfoList();
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/content/default.aspx?contenttypeid=" + ContentTypeId;
            RedrictUrl = CancelUrl;
        }

        private void FillLanguages()
        {
            List<LanguageInfo> languages = _localizationProvider.SelectAll(ErrorList);
            if (CheckErrors())
            {
                drlLanguages.Items.Add(new ListItem("All", "all", true));
                if (languages != null && languages.Count > 0)
                {
                    foreach (LanguageInfo item in languages)
                    {
                        drlLanguages.Items.Add(new ListItem(item.Name, item.Code));
                    }
                }
            }
        }


        protected override void FillFields()
        {
            if (IsEdit)
            {

                txtSeo.Text = ValidationHelper.GetString(mainForm.ContentTypeModel.GetValue("SeoTemplate"), string.Empty);
                txtMetaTile.Text = ValidationHelper.GetString(mainForm.ContentTypeModel.GetValue("MetaTitle"), string.Empty);
                txtMetaDescription.Text = ValidationHelper.GetString(mainForm.ContentTypeModel.GetValue("MetaDescription"), string.Empty);
                txtMetaKeywords.Text = ValidationHelper.GetString(mainForm.ContentTypeModel.GetValue("MetaKeywords"), string.Empty);
                txtCopyRights.Text = ValidationHelper.GetString(mainForm.ContentTypeModel.GetValue("CopyRights"), string.Empty);
                if (mainForm.ContentTypeModel.GetValue("Language") != null)
                    drlLanguages.SelectedValue = mainForm.ContentTypeModel.GetValue("Language").ToString();
            }
        }

        protected override bool Update()
        {
            return Update(IsPublished);
        }

        protected override bool Publish()
        {
            if (IsEdit)
                return Update(true);

            return Insert(true);

        }

        protected override bool Unpublish()
        {
            if (IsEdit)
                return Update(false);

            return Insert(false);
        }

        protected override bool Insert()
        {
            return Insert(false);
        }

        private bool Insert(bool isPublished)
        {
            mainForm.FormMode = FormActionMode.Insert;
            mainForm.OnValidationFailure += mainForm_OnValidationFailure;
            mainForm.ContentTypeModel.SetValue("SeoTemplate", !string.IsNullOrEmpty(txtSeo.Text) ? txtSeo.Text : null);
            mainForm.ContentTypeModel.SetValue("MetaTitle", !string.IsNullOrEmpty(txtMetaTile.Text) ? txtMetaTile.Text : null);
            mainForm.ContentTypeModel.SetValue("MetaDescription", !string.IsNullOrEmpty(txtMetaDescription.Text) ? txtMetaDescription.Text : null);
            mainForm.ContentTypeModel.SetValue("MetaKeywords", !string.IsNullOrEmpty(txtMetaKeywords.Text) ? txtMetaKeywords.Text : null);
            mainForm.ContentTypeModel.SetValue("CopyRights", !string.IsNullOrEmpty(txtCopyRights.Text) ? txtCopyRights.Text : null);


            mainForm.ContentTypeModel.SetValue("Language", drlLanguages.SelectedValue);
            mainForm.ContentTypeModel.IsPublished = isPublished;
            mainForm.SaveData();
            CacheHelper.DeleteAll(mainForm.ContentTypeModel.TableName);
            return CheckErrors();
        }

        private bool Update(bool isPublished)
        {
            mainForm.FormMode = FormActionMode.Edit;
            mainForm.OnValidationFailure += mainForm_OnValidationFailure;
            mainForm.ContentTypeModel.SetValue("SeoTemplate", !string.IsNullOrEmpty(txtSeo.Text) ? txtSeo.Text : null);
            mainForm.ContentTypeModel.SetValue("MetaTitle", !string.IsNullOrEmpty(txtMetaTile.Text) ? txtMetaTile.Text : null);
            mainForm.ContentTypeModel.SetValue("MetaDescription", !string.IsNullOrEmpty(txtMetaDescription.Text) ? txtMetaDescription.Text : null);
            mainForm.ContentTypeModel.SetValue("MetaKeywords", !string.IsNullOrEmpty(txtMetaKeywords.Text) ? txtMetaKeywords.Text : null);
            mainForm.ContentTypeModel.SetValue("CopyRights", !string.IsNullOrEmpty(txtCopyRights.Text) ? txtCopyRights.Text : null);
            mainForm.ContentTypeModel.SetValue("Language", drlLanguages.SelectedValue);
            mainForm.ContentTypeModel.IsPublished = isPublished;
            mainForm.SaveData();
            CacheHelper.DeleteAll(mainForm.ContentTypeModel.TableName);
            return CheckErrors();
        }

        private void mainForm_OnValidationFailure(ErrorInfoList errors)
        {
            ErrorList = errors;
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
            throw new NotImplementedException();
        }

    }
}