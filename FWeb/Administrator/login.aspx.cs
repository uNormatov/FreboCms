using System;
using System.Web.Security;
using FCore.Class;
using FCore.Collection;
using FCore.Helper;
using FDataProvider;
using FUIControls.Settings;

namespace FWeb.Administrator
{
    public partial class login : System.Web.UI.Page
    {
        private SiteProvider _siteProvider;
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = ValidationHelper.GetString(Request.QueryString["action"], "");
            if (!string.IsNullOrEmpty(action) && action.ToLower() == "signout")
            {
                FormsAuthentication.SignOut();
                Response.Redirect("/administrator/login.aspx");
            }

            if (_siteProvider == null)
                _siteProvider = new SiteProvider();

            SiteInfo siteInfo = _siteProvider.Select(0, new ErrorInfoList());
            if (siteInfo != null)
            {
                CoreSettings.CurrentSite = siteInfo;
            }
        }

        protected void Login_OnClick(object sender, EventArgs e)
        {
            if (!Membership.ValidateUser(UserName.Text, Password.Text))
                errorDiv.Visible = true;
            else
                FormsAuthentication.RedirectFromLoginPage(UserName.Text, true);

        }
    }
}