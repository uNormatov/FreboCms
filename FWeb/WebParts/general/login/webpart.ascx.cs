using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Helper;
using FUIControls.PortalControl;

namespace FWeb.WebParts.general.login
{
    public partial class webpart : FWebPart
    {
        private const string Form = "Form";
        private const string RedrictUrl = "RedirictUrl";
        private const string ErrorMessage = "ErrorMessage";

        protected override void LoadWebPart()
        {
            string action = ValidationHelper.GetString(Request.QueryString["action"], string.Empty);
            if (!string.IsNullOrEmpty(action) && action.ToLower() == "signout")
            {
                FormsAuthentication.SignOut();
                Response.Redirect(BuildUrl("/"));
            }

            ltlMain.Text = GetResourceByPattern(GetProperty(Form));
            if (IsPostBack)
            {
                string login = ValidationHelper.GetString(Request.Form["login"], string.Empty);
                string password = ValidationHelper.GetString(Request.Form["password"], string.Empty);
                if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
                    LoginUser(login, password);

            }
        }

        private void LoginUser(string login, string password)
        {
            if (Membership.ValidateUser(login, password))
            {
                FormsAuthentication.SetAuthCookie(login, false);
                Response.Redirect(BuildUrl(GetProperty(RedrictUrl)));
            }
            else
            {
                ltlError.Text = GetResourceByPattern(GetProperty(ErrorMessage));
            }
        }
    }
}