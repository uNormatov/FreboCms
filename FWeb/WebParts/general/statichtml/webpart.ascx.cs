using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Helper;
using FUIControls.PortalControl;

namespace FWeb.WebParts.statichtml
{
    public partial class webpart : FWebPart
    {

        protected string Html
        {
            get { return GetProperty("html").ToHtmlDecode(); }
        }

        protected override void LoadWebPart()
        {
            lblHtml.Text = Html;
        }
    }
}