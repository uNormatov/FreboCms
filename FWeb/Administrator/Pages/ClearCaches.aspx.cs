using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Helper;
using FDataProvider;
using FUIControls.Helper;
using FUIControls.Settings;

namespace FWeb.Administrator.Pages
{
    public partial class ClearCaches : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string returnUrl = ValidationHelper.GetString(Request.QueryString["returnUrl"], string.Empty);
            CacheHelper.ClearCaches();
            LanguageHelper.Instance.Clear();
            CoreSettings.CurrentSite = null;
            Response.Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
        }
    }
}