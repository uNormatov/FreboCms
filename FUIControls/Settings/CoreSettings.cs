using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using FCore.Class;
using FCore.Collection;
using FDataProvider;

namespace FUIControls.Settings
{
    public class CoreSettings
    {
        public static string CurrentUserName
        {
            get
            {
                return HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : null;
            }
        }

        public static bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        public static string[] CurrentUserRoles
        {
            get
            {

                return Roles.GetRolesForUser();
            }
        }

        private static readonly Object LockObject = new Object();

        private static SiteInfo _currentSite;
        public static SiteInfo CurrentSite
        {
            get
            {
                lock (LockObject)
                {
                    if (_currentSite == null)
                    {
                        SiteProvider siteProvider = new SiteProvider();
                        _currentSite = siteProvider.Select(0, new ErrorInfoList());
                        if (_currentSite == null)
                            return new SiteInfo() { Name = "Frebo Cms", ArticleWebpartId = 11 };
                        return _currentSite;
                    }
                    return _currentSite;
                }
            }
            set
            {
                lock (LockObject)
                {
                    _currentSite = value;
                }
            }
        }
    }
}
