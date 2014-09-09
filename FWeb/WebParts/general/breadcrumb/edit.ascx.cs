using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Collection;
using FCore.Helper;
using FUIControls.PortalControl;

namespace FWeb.WebParts.general.breadcrumb
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
            string separator = GetControlValue(txtSeparator.ID);
            if (!string.IsNullOrEmpty(separator))
                SetValue("Separator", separator.ToHtmlEncode());
        }

        protected override void EnsureControlsValue()
        {
            txtSeparator.Text = ValidationHelper.GetString(GetValue("Separator"), string.Empty).ToHtmlDecode();
        }
    }
}