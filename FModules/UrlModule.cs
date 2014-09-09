using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using FCore.Class;
using FCore.Collection;
using FCore.Constant;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.Helper;
using FUIControls.Settings;

namespace FModules
{
    public class UrlModule : IHttpModule
    {
        #region IHttpModule Members

        private DataConnection _connection;
        private SiteProvider _siteProvider;
        private PageProvider _pageProvider;
        private EventLogProvider _eventLogProvider;
        private ErrorInfoList errors;
        private UserProfileProvider _userProfileProfileProvider;

        public void Dispose()
        {
            errors = null;
            _pageProvider.Dispose();
            _siteProvider.Dispose();
            _connection.Dispose();
            _userProfileProfileProvider.Dispose();
        }

        public void Init(HttpApplication context)
        {
            _connection = new DataConnection();
            _pageProvider = new PageProvider(_connection);
            _siteProvider = new SiteProvider(_connection);
            _eventLogProvider = new EventLogProvider();
            errors = new ErrorInfoList();
            _userProfileProfileProvider = new UserProfileProvider();
            context.AuthenticateRequest += ContextAuthenticateRequest;
        }

        private void ContextAuthenticateRequest(object sender, EventArgs e)
        {
            var appliction = (HttpApplication)sender;
            HttpContext context = appliction.Context;
            const string renderPage = "~/default.aspx";
            const string errorPage = "~/error.aspx";
            const string robotstxt = "~/pages/robots.txt";

            if (context.Request.AppRelativeCurrentExecutionFilePath != null)
            {
                string path = context.Request.AppRelativeCurrentExecutionFilePath.ToLower();
                string queryString = context.Request.QueryString.ToString();
                if (path.StartsWith("~"))
                    path = path.Substring(1);
                if (path.EndsWith("/robots.txt"))
                {
                    context.RewritePath(robotstxt);
                }else if (IsSitePage(path))
                {
                    path = DetectSiteLanguage(context, path);
                    errors.Clear();
                    PageInfo page = GetPage(path);
                    page.FullUrl = context.Request.Url.ToString();
                    page.QueryString = queryString;
                    if (!errors.HasError())
                    {
                        context.Items.Add(SiteConstants.PageData, page);
                        context.RewritePath(renderPage);
                        LogEvent(page, context.Request);
                        RegisterUserProfile(context);
                    }
                    else
                    {
                        if (errors.Any(x => x.ErrorType == ErrorType.NotFound))
                        {
                            page = _pageProvider.Select(CoreSettings.CurrentSite.NotFoundPageId, errors);
                            string currentLanguage = string.Empty;
                            if (context.Items.Contains(SiteConstants.SiteLanguage))
                                currentLanguage = context.Items[SiteConstants.SiteLanguage].ToString();
                            string notFoundPage = page.SeoTemplate;
                            if (!string.IsNullOrEmpty(notFoundPage))
                                notFoundPage = string.Format("/{0}{1}", currentLanguage, notFoundPage);
                            context.Response.Redirect(notFoundPage);
                        }
                        else
                        {
                            context.Items.Add(SiteConstants.PageData, errors);
                            context.RewritePath(errorPage);
                        }

                    }
                }
            }
        }

        private PageInfo GetPage(string path)
        {
            var info = new PageInfo();
            if (CoreSettings.CurrentSite != null)
            {
                if (path == "/default.aspx" || path == "/" || path.Equals(""))
                {
                    path = "/";
                    info = _pageProvider.Select(CoreSettings.CurrentSite.DefaultPageId, errors);
                    if (info == null)
                        errors.Add(new ErrorInfo { Ok = false, Message = "Default page have not created yet!" });
                }
                else
                    info = _pageProvider.SelectBySeo(path, errors);

                if (info != null)
                    info.SeoUrl = path;
                else
                    errors.Add(new ErrorInfo { Ok = false, Message = "Page Not Found!", ErrorType = ErrorType.NotFound });

            }
            return info;
        }

        private bool IsSitePage(string path)
        {


            return !(path.StartsWith("/content") ||
                     path.StartsWith("/pages") ||
                     path.StartsWith("/userfiles") ||
                     path.StartsWith("/files") ||
                     path.StartsWith("/administrator") ||
                     path.EndsWith(".js") ||
                     path.EndsWith(".epub") ||
                     path.EndsWith(".css") ||
                     path.EndsWith(".png") ||
                     path.EndsWith(".gif") ||
                     path.EndsWith(".jpeg") ||
                     path.EndsWith(".jpg") ||
                     path.EndsWith(".axd") ||
                     path.EndsWith(".ashx") ||
                     path.EndsWith(".swf") ||
                     path.EndsWith(".eot") ||
                     path.EndsWith(".svg") ||
                     path.EndsWith(".ttf") ||
                     path.EndsWith(".woff") ||
                     path.EndsWith(".ico"));
        }

        private string DetectSiteLanguage(HttpContext context, string path)
        {
            if (CoreSettings.CurrentSite.IsMultilanguage)
            {
                if (path == "/default.aspx" || path == "/")
                {
                    context.Items.Add(SiteConstants.SiteLanguage, CoreSettings.CurrentSite.DefaultLanguage);
                    return path;
                }
                string language = path.Substring(1);
                if (language.IndexOf("/") > 0)
                    language = language.Substring(0, language.IndexOf("/"));
                if (LanguageHelper.Instance.IsAvailableLanguage(language))
                {
                    context.Items.Add(SiteConstants.SiteLanguage, language);
                    return path.Substring(language.Length + 1);
                }
                context.Items.Add(SiteConstants.SiteLanguage, CoreSettings.CurrentSite.DefaultLanguage);
                return path;
            }

            if (context.Items.Contains(SiteConstants.SiteLanguage))
                context.Items.Remove(SiteConstants.SiteLanguage);

            return path;
        }

        private void LogEvent(PageInfo pageInfo, HttpRequest request)
        {
            if (_eventLogProvider == null)
                _eventLogProvider = new EventLogProvider();

            EventLogInfo eventLogInfo = new EventLogInfo();
            eventLogInfo.PageId = pageInfo.Id;
            eventLogInfo.FullUrl = SiteHelper.GetSiteUrl() + pageInfo.SeoUrl;
            eventLogInfo.PageUrl = pageInfo.SeoUrl;
            eventLogInfo.ReferalUrl = request.UrlReferrer != null ? request.UrlReferrer.OriginalString : string.Empty;
            if (string.IsNullOrEmpty(request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            {
                eventLogInfo.Ip = request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                eventLogInfo.Ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault();
            }
            eventLogInfo.UserName = request.IsAuthenticated ? CoreSettings.CurrentUserName : string.Empty;
            eventLogInfo.UserAgent = request.UserAgent;
            eventLogInfo.Date = DateTime.Now;
            _eventLogProvider.Create(eventLogInfo, errors);
        }

        private void RegisterUserProfile(HttpContext context)
        {
            if (CoreSettings.IsAuthenticated)
            {
                UserProfileInfo userProfile = _userProfileProfileProvider.SelectByUserId(CoreSettings.CurrentUserName, new ErrorInfoList());
                context.Items.Add(SiteConstants.UserData, userProfile);
            }
        }

      
        #endregion
    }
}
