using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Helper;
using FDataProvider;
using FUIControls.PortalControl;

namespace FWeb.WebParts.general.form
{
    public partial class edit : FWebPartEdit
    {
        private const string ContentTypeId = "ContentTypeId";
        private const string FormId = "FormId";

        private readonly ContentTypeProvider _contentTypeProvider;
        private readonly FormProvider _formProvider;

        public edit()
            : base("", null)
        {
            _contentTypeProvider = new ContentTypeProvider();
            _formProvider = new FormProvider();
        }

        public edit(string properties, ErrorInfoList errorInfoList)
            : base(properties, errorInfoList)
        {
            _contentTypeProvider = new ContentTypeProvider();
            _formProvider = new FormProvider();
        }

        protected override void GetValues()
        {
            string contenTypeId = GetControlValue(drlContentType.ID);
            SetValue(ContentTypeId, contenTypeId);

            string formId = GetControlValue(drlForms.ID);
            SetValue(FormId, formId);

            string returnUrl = GetControlValue(txtReturnUrl.ID);
            SetValue("ReturnUrl", returnUrl);

            string buttonText = GetControlValue(txtSaveButtonText.ID);
            SetValue("ButtonText", buttonText);

            string buttonCancelText = GetControlValue(txtCancelButtonText.ID);
            SetValue("CancelButtonText", buttonCancelText);

            string queryName = GetControlValue(txtQueryName.ID);
            if (!string.IsNullOrEmpty(queryName))
                SetValue("QueryName", queryName);

            Dictionary<string, string> queryParams = GetControlValues("parameter");
            string queryParamsBuilder = FormHelper.GetDataViewerParametersXml(queryParams);
            if (!string.IsNullOrEmpty(queryName))
                SetValue("QueryParameters", queryParamsBuilder.Length > 0 ? queryParamsBuilder : string.Empty);

        }

        protected override void EnsureControlsValue()
        {
            List<ContentTypeInfo> contentTypeInfos = _contentTypeProvider.SelectAll(ErrorInfoList);
            drlContentType.DataSource = contentTypeInfos;
            drlContentType.DataBind();


            string contentType = ValidationHelper.GetString(GetValue(ContentTypeId), string.Empty);
            if (!string.IsNullOrEmpty(contentType))
                drlContentType.SelectedValue = contentType;

            string formId = ValidationHelper.GetString(GetValue(FormId), string.Empty);
            if (!string.IsNullOrEmpty(formId))
                drlForms.SelectedValue = formId;

            txtQueryName.Text = ValidationHelper.GetString(GetValue("QueryName"), string.Empty);
            ltlQuerySelector.Text = string.Format("<a class=\"queryselectors ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/queryselector.aspx?name={0}&controlid={1}\"><span class=\"ui-button-text\">select</span></a>", txtQueryName.Text, txtQueryName.ClientID);
            if (!string.IsNullOrEmpty(txtQueryName.Text))
                ltlQuerySelector.Text += string.Format("<a target=\"_blank\" class=\" queryeditor ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/queryeditor.aspx?id={0}\"><span class=\"ui-button-text\">edit</span></a>", txtQueryName.Text);


            string queryParamaters = ValidationHelper.GetString(GetValue("QueryParameters"), string.Empty);
            if (string.IsNullOrEmpty(queryParamaters))
                Page.ClientScript.RegisterStartupScript(typeof(Page), "addfields", "<script>addField();</script>");
            else
            {
                ltlQueryParams.Text = FormHelper.GetDataViewerParametersTable(queryParamaters);
            }

            string buttonText = ValidationHelper.GetString(GetValue("ButtonText"), string.Empty);
            if (!string.IsNullOrEmpty(buttonText))
                txtSaveButtonText.Text = buttonText;

            string cancelButtonText = ValidationHelper.GetString(GetValue("CancelButtonText"), string.Empty);
            if (!string.IsNullOrEmpty(cancelButtonText))
                txtCancelButtonText.Text = cancelButtonText;

            string returnUrl = ValidationHelper.GetString(GetValue("ReturnUrl"), string.Empty);
            if (!string.IsNullOrEmpty(returnUrl))
                txtReturnUrl.Text = returnUrl;



            FillFormsComboBox();
        }

        protected void drlCustomEntity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            FillFormsComboBox();
        }

        private void FillFormsComboBox()
        {
            List<FormInfo> formInfos = _formProvider.SelectAllByContentTypeId(ValidationHelper.GetInteger(drlContentType.SelectedValue, 0), ErrorInfoList);
            drlForms.DataSource = formInfos;
            drlForms.DataBind();
        }
    }
}