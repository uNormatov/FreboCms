using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Helper;
using FDataProvider;
using FUIControls.PortalControl;

namespace FWeb.WebParts.custom.edulogin
{
    public partial class webpart : FWebPart
    {
        protected override void LoadWebPart()
        {
            base.LoadWebPart();
            string action = ValidationHelper.GetString(Request.QueryString["action"], string.Empty);
            if (action == "signout")
            {
                FormsAuthentication.SignOut();
                Response.Redirect("/");
            }

            if (IsPostBack)
            {
                string login = ValidationHelper.GetString(Request.Form["login"], string.Empty);
                string password = ValidationHelper.GetString(Request.Form["password"], string.Empty);
                string yoddaSaqlash = ValidationHelper.GetString(Request.Form["remember"], string.Empty);
                LoginUser(login, password, yoddaSaqlash == "on");
            }
        }

        private void LoginUser(string login, string password, bool isRemember)
        {
            if (Membership.ValidateUser(login, password))
            {
                FormsAuthentication.SetAuthCookie(login, isRemember);
                string url = BuildUrl("/profil");
                if (Roles.IsUserInRole(login, "superuser") || Roles.IsUserInRole(login, "editor"))
                {
                    url = "/administrator";
                }
                
                Page.ClientScript.RegisterStartupScript(typeof(FWebPart), "windowclose",
                                                       "<script>parent.location ='" + url + "';parent.$.fancybox.close();</script>");
            }
        }
    }
}