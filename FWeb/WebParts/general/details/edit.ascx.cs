using System.Collections.Generic;
using System.Web.UI;
using FCore.Collection;
using FCore.Helper;
using FUIControls.PortalControl;

namespace FWeb.WebParts.details
{
    public partial class edit : FWebPartEdit
    {
        public edit()
            : base("", null)
        {
        }

        public edit(string properties, ErrorInfoList errorInfoList)
            : base(properties, errorInfoList)
        {
        }

        protected override void GetValues()
        {
            string transformation = GetControlValue(txtTransformation.ID);
            if (!string.IsNullOrEmpty(transformation))
                SetValue("Transformation", transformation);

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
            txtTransformation.Text = ValidationHelper.GetString(GetValue("Transformation"), string.Empty);
            txtQueryName.Text = ValidationHelper.GetString(GetValue("QueryName"), string.Empty);
            string queryParamaters = ValidationHelper.GetString(GetValue("QueryParameters"), string.Empty);
            ltlTransformationSelect.Text = string.Format("<a class=\"transselectors ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/transformationselector.aspx?name={0}&controlid={1}\"><span class=\"ui-button-text\">select</span></a>", txtTransformation.Text, txtTransformation.ClientID);
            if (!string.IsNullOrEmpty(txtTransformation.Text))
                ltlTransformationSelect.Text += string.Format("<a target=\"_blank\" class=\"transformationeditor ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/transformationeditor.aspx?id={0}\"><span class=\"ui-button-text\">edit</span></a>", txtTransformation.Text);

            ltlQuerySelector.Text = string.Format("<a class=\"queryselectors ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/queryselector.aspx?name={0}&controlid={1}\"><span class=\"ui-button-text\">select</span></a>", txtQueryName.Text, txtQueryName.ClientID);
            if (!string.IsNullOrEmpty(txtQueryName.Text))
                ltlQuerySelector.Text += string.Format("<a target=\"_blank\" class=\"queryeditor ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/queryeditor.aspx?id={0}\"><span class=\"ui-button-text\">edit</span></a>", txtQueryName.Text);


            if (string.IsNullOrEmpty(queryParamaters))
                Page.ClientScript.RegisterStartupScript(typeof(Page), "addfields", "<script>addField();</script>");
            else
            {
                ltlQueryParams.Text = FormHelper.GetDataViewerParametersTable(queryParamaters);
            }
        }
    }
}