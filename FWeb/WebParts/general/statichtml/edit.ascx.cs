using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Collection;
using FUIControls.PortalControl;
using FCore.Helper;

namespace FWeb.WebParts.statichtml
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
            string html = GetControlValue(txtHtml.ID);
            if (!string.IsNullOrEmpty(html))
                SetValue("html", html.ToHtmlEncode());
        }

        protected override void EnsureControlsValue()
        {
            txtHtml.Text = ValidationHelper.GetString(GetValue("html"), string.Empty);
        }
    }
}