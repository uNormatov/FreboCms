using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Collection;
using FCore.Helper;
using FUIControls.PortalControl;

namespace FWeb.WebParts.general.datalist
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
            string beforeContainer = GetControlValue(txtBeforeContainer.ID);
            if (!string.IsNullOrEmpty(beforeContainer))
                SetValue("BeforeContainer", beforeContainer);

            string transformation = GetControlValue(txtTransformation.ID);
            if (!string.IsNullOrEmpty(transformation))
                SetValue("Transformation", transformation);

            string altertransformation = GetControlValue(txtAlterTransformation.ID);
            if (!string.IsNullOrEmpty(altertransformation))
                SetValue("AlterTransformation", altertransformation);

            string afterContainer = GetControlValue(txtAfterContainer.ID);
            if (!string.IsNullOrEmpty(afterContainer))
                SetValue("AfterContainer", afterContainer);

            string queryName = GetControlValue(txtQueryName.ID);
            if (!string.IsNullOrEmpty(queryName))
                SetValue("QueryName", queryName);

            string repeatColumns = GetControlValue(drlRepeatColumns.ID);
            if (!string.IsNullOrEmpty(repeatColumns))
                SetValue("RepeatColumns", repeatColumns);

            string rowBefore = GetControlValue(txtRowBefore.ID);
            if (!string.IsNullOrEmpty(rowBefore))
                SetValue("RowBefore", rowBefore);
            string rowAfter = GetControlValue(txtRowAfter.ID);
            if (!string.IsNullOrEmpty(rowAfter))
                SetValue("RowAfter", rowAfter);

            Dictionary<string, string> queryParams = GetControlValues("parameter");
            string queryParamsBuilder = FormHelper.GetDataViewerParametersXml(queryParams);
            if (!string.IsNullOrEmpty(queryName))
                SetValue("QueryParameters", queryParamsBuilder.Length > 0 ? queryParamsBuilder : string.Empty);
        }

        protected override void EnsureControlsValue()
        {
            txtBeforeContainer.Text = ValidationHelper.GetString(GetValue("BeforeContainer"), string.Empty);
            txtRowBefore.Text = ValidationHelper.GetString(GetValue("RowBefore"), string.Empty);
            txtTransformation.Text = ValidationHelper.GetString(GetValue("Transformation"), string.Empty);
            txtAlterTransformation.Text = ValidationHelper.GetString(GetValue("AlterTransformation"), string.Empty);
            txtAfterContainer.Text = ValidationHelper.GetString(GetValue("AfterContainer"), string.Empty);
            txtRowAfter.Text = ValidationHelper.GetString(GetValue("RowAfter"), string.Empty);
            txtQueryName.Text = ValidationHelper.GetString(GetValue("QueryName"), string.Empty);
            drlRepeatColumns.SelectedValue = ValidationHelper.GetString(GetValue("RepeatColumns"), "0");

            ltlTransformationSelect.Text = string.Format("<a class=\"transselectors ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/transformationselector.aspx?name={0}&controlid={1}\"><span class=\"ui-button-text\">select</span></a>", txtTransformation.Text, txtTransformation.ClientID);
            if (!string.IsNullOrEmpty(txtTransformation.Text))
                ltlTransformationSelect.Text += string.Format("<a target=\"_blank\" class=\"transformationeditor ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/transformationeditor.aspx?id={0}\"><span class=\"ui-button-text\">edit</span></a>", txtTransformation.Text);

            ltlAlterTransformationSelect.Text = string.Format("<a class=\"transselectors ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/transformationselector.aspx?name={0}&controlid={1}\"><span class=\"ui-button-text\">select</span></a>", txtAlterTransformation.Text, txtAlterTransformation.ClientID);
            if (!string.IsNullOrEmpty(txtAlterTransformation.Text))
                ltlAlterTransformationSelect.Text += string.Format("<a target=\"_blank\" class=\"transformationeditor ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/transformationeditor.aspx?id={0}\"><span class=\"ui-button-text\">edit</span></a>", txtAlterTransformation.Text);

            ltlQuerySelector.Text = string.Format("<a class=\"queryselectors ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/queryselector.aspx?name={0}&controlid={1}\"><span class=\"ui-button-text\">select</span></a>", txtQueryName.Text, txtQueryName.ClientID);
            if (!string.IsNullOrEmpty(txtQueryName.Text))
                ltlQuerySelector.Text += string.Format("<a target=\"_blank\" class=\"queryeditor ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/queryeditor.aspx?id={0}\"><span class=\"ui-button-text\">edit</span></a>", txtQueryName.Text);


            string queryParamaters = ValidationHelper.GetString(GetValue("QueryParameters"), string.Empty);
            if (FormHelper.GetParamatersCount(queryParamaters) == 0)
                Page.ClientScript.RegisterStartupScript(typeof(Page), "addfields", "<script>addField();</script>");
            else
            {
                ltlQueryParams.Text = FormHelper.GetDataViewerParametersTable(queryParamaters);
            }
        }
    }
}