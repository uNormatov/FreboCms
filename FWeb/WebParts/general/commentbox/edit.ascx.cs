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

namespace FWeb.WebParts.general.commentbox
{
    public partial class edit : FWebPartEdit
    {
        private ContentTypeProvider _contentTypeProvider;
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
            string contentTypeId = GetControlValue(drlContentType.ID);
            if (!string.IsNullOrEmpty(contentTypeId))
                SetValue("ContentTypeId", contentTypeId);

            string parameterType = GetControlValue(drlParameterType.ID);
            if (!string.IsNullOrEmpty(parameterType))
                SetValue("ParameterType", parameterType);

            string parameterName = GetControlValue(txtParameterName.ID);
            if (!string.IsNullOrEmpty(parameterName))
                SetValue("ParameterName", parameterName);

            string allowAnonym = GetControlValue(chbxAnonym.ID);
            if (!string.IsNullOrEmpty(allowAnonym))
                SetValue("AllowAnonym", allowAnonym);
        }

        protected override void EnsureControlsValue()
        {
            _contentTypeProvider = new ContentTypeProvider();
            List<ContentTypeInfo> contentTypeInfos = _contentTypeProvider.SelectAll(ErrorInfoList);
            drlContentType.DataSource = contentTypeInfos;
            drlContentType.DataValueField = "Id";
            drlContentType.DataTextField = "Name";
            drlContentType.DataBind();
            drlContentType.SelectedValue = ValidationHelper.GetString(GetValue("ContentTypeId"), string.Empty);
            drlParameterType.SelectedValue = ValidationHelper.GetString(GetValue("ParameterType"), string.Empty);
            txtParameterName.Text = ValidationHelper.GetString(GetValue("ParameterName"), string.Empty);
            chbxAnonym.Checked = ValidationHelper.GetBoolean(GetValue("AllowAnonym"), false);
        }
    }
}