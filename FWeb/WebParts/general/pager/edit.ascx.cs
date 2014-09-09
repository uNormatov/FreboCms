using System.Collections.Generic;
using System.Web.UI;
using FCore.Collection;
using FCore.Helper;
using FUIControls.PortalControl;

namespace FWeb.WebParts.general.pager
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
            string queryName = GetControlValue(txtQueryName.ID);
            if (!string.IsNullOrEmpty(queryName))
                SetValue("QueryName", queryName);

            string pageSize = GetControlValue(txtPageSize.ID);
            if (!string.IsNullOrEmpty(pageSize))
                SetValue("PageSize", pageSize);

            string pageIndexKeyword = GetControlValue(txtPageIndexKeyword.ID);
            if (!string.IsNullOrEmpty(pageIndexKeyword))
                SetValue("PageIndexKeyword", pageIndexKeyword);

            string pageIndexKeywordType = GetControlValue(drlPageIndexType.ID);
            if (!string.IsNullOrEmpty(pageIndexKeywordType))
                SetValue("PageIndexKeywordType", pageIndexKeywordType);

            string containerTag = GetControlValue(drlContainerTag.ID);
            if (!string.IsNullOrEmpty(containerTag))
                SetValue("ContainerTag", containerTag);

            string containerCss = GetControlValue(txtContainerCss.ID);
            if (!string.IsNullOrEmpty(containerCss))
                SetValue("ContainerCss", containerCss);

            string innerContainerTag = GetControlValue(drlInnerContainerTag.ID);
            if (!string.IsNullOrEmpty(innerContainerTag))
                SetValue("InnerContainerTag", innerContainerTag);

            string innerContainerCss = GetControlValue(txtInnerContainerCss.ID);
            if (!string.IsNullOrEmpty(innerContainerCss))
                SetValue("InnerContainerCss", innerContainerCss);

            string activeCss = GetControlValue(txtActiveCss.ID);
            if (!string.IsNullOrEmpty(activeCss))
                SetValue("ActiveCss", activeCss);

            string prevText = GetControlValue(txtPrev.ID);
            if (!string.IsNullOrEmpty(prevText))
                SetValue("PrevText", prevText);

            string nextText = GetControlValue(txtNext.ID);
            if (!string.IsNullOrEmpty(nextText))
                SetValue("NextText", nextText);

            Dictionary<string, string> queryParams = GetControlValues("parameter");
            string queryParamsBuilder = FormHelper.GetDataViewerParametersXml(queryParams);
            if (!string.IsNullOrEmpty(queryName))
                SetValue("QueryParameters", queryParamsBuilder.Length > 0 ? queryParamsBuilder : string.Empty);
        }

        protected override void EnsureControlsValue()
        {
            txtQueryName.Text = ValidationHelper.GetString(GetValue("QueryName"), string.Empty);
            txtPageSize.Text = ValidationHelper.GetString(GetValue("PageSize"), string.Empty);
            txtPageIndexKeyword.Text = ValidationHelper.GetString(GetValue("PageIndexKeyword"), string.Empty);

            string contentType = ValidationHelper.GetString(GetValue("PageIndexKeywordType"), string.Empty);
            if (!string.IsNullOrEmpty(contentType))
                drlPageIndexType.SelectedValue = contentType;
            txtPrev.Text = ValidationHelper.GetString(GetValue("PrevText"), string.Empty);
            txtNext.Text = ValidationHelper.GetString(GetValue("NextText"), string.Empty);
            drlContainerTag.SelectedValue = ValidationHelper.GetString(GetValue("ContainerTag"), "ul");
            txtContainerCss.Text = ValidationHelper.GetString(GetValue("ContainerCss"), string.Empty);
            drlInnerContainerTag.SelectedValue = ValidationHelper.GetString(GetValue("InnerContainerTag"), "li");
            txtInnerContainerCss.Text = ValidationHelper.GetString(GetValue("InnerContainerCss"), string.Empty);
            txtActiveCss.Text = ValidationHelper.GetString(GetValue("ActiveCss"), string.Empty);

            string queryParamaters = ValidationHelper.GetString(GetValue("QueryParameters"), string.Empty);

            ltlQuerySelector.Text = string.Format("<a class=\"queryselectors ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/queryselector.aspx?name={0}&controlid={1}\"><span class=\"ui-button-text\">select</span></a>", txtQueryName.Text, txtQueryName.ClientID);
            if (!string.IsNullOrEmpty(txtQueryName.Text))
                ltlQuerySelector.Text += string.Format("<a target=\"_blank\" class=\"queryeditor ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/queryeditor.aspx?id={0}\"><span class=\"ui-button-text\">edit</span></a>", txtQueryName.Text);

            if (FormHelper.GetParamatersCount(queryParamaters) == 0)
                Page.ClientScript.RegisterStartupScript(typeof(Page), "addfields", "<script>addField();</script>");
            else
            {
                ltlQueryParams.Text = FormHelper.GetDataViewerParametersTable(queryParamaters);
            }
        }
    }
}