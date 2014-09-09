using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Helper;
using FUIControls.PortalControl;

namespace FWeb.WebParts.custom.register
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

            action = ValidationHelper.GetString(Request.Form["action"], string.Empty);



            string login = ValidationHelper.GetString(Request.Form["login"], string.Empty);
            string password = ValidationHelper.GetString(Request.Form["password"], string.Empty);
            if (!string.IsNullOrEmpty(action))
            {
                if (action == "1")
                {
                    RegisterUser(login, password);
                }
                else
                {
                    LoginUser(login, password);
                }
            }

        }

        private void LoginUser(string login, string password)
        {
            if (Membership.ValidateUser(login, password))
            {
                FormsAuthentication.SetAuthCookie(login, false);
                Page.ClientScript.RegisterStartupScript(typeof(FWebPart), "windowclose",
                                                        "<script>parent.location ='/profile';parent.$.fancybox.close();</script>");
            }
        }

        private void RegisterUser(string login, string password)
        {
            if (Membership.GetUser(login) == null)
            {
                MembershipCreateStatus status;
                Membership.CreateUser(login, password, login, null, null, true, out status);
                if (status == MembershipCreateStatus.Success)
                {
                    Roles.AddUserToRole(login, "basic");
                    Page.ClientScript.RegisterStartupScript(typeof(FWebPart), "windowclose",
                                                            "<script>parent.location ='/profile';parent.$.fancybox.close();</script>");
                    //  Response.Redirect("/profile");
                }
            }
            else
            {
                LoginUser(login, password);
            }
        }
    }
}