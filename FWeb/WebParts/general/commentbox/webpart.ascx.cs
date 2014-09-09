using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Enum;
using FCore.Helper;
using FUIControls.PortalControl;

namespace FWeb.WebParts.general.commentbox
{
    public partial class webpart : FWebPart
    {
        #region Properties
        protected int ContentTypeId
        {
            get { return ValidationHelper.GetInteger(GetProperty("ContentTypeId"), 0); }
        }

        protected int ParameterType
        {
            get { return ValidationHelper.GetInteger(GetProperty("ParameterType"), 0); }
        }

        protected string ParameterName
        {
            get { return ValidationHelper.GetString(GetProperty("ParameterName"), string.Empty); }
        }

        protected bool AllowAnonym
        {
            get { return ValidationHelper.GetBoolean(GetProperty("AllowAnonym"), false); }
        }

        protected string SeoTemplate
        {
            get
            {
                if (ParameterType == ((int)QueryParameterType.QueryString))
                    return GetQueryValue(ParameterName, string.Empty);
                if (ParameterType == ((int)QueryParameterType.SeoTemplate))
                    return GetSeoValue(ParameterName, string.Empty);
                if (ParameterType == ((int)QueryParameterType.Cookie))
                    return GetCookieValue(ParameterName, string.Empty);
                if (ParameterType == ((int)QueryParameterType.UserProfileProperty))
                    return Page.User.Identity.Name;
                return string.Empty;
            }
        }

        #endregion

        #region Methods
        protected override void LoadWebPart()
        {

        }
        #endregion
    }
}