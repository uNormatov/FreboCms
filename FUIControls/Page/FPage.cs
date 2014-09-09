using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml.Linq;
using FCore.Class;
using FCore.Collection;
using FCore.Constant;
using FCore.Enum;
using FCore.Helper;
using FUIControls.Helper;

namespace FUIControls.Page
{
    public class FPage : System.Web.UI.Page
    {
        protected ErrorInfoList ErrorList { get; set; }

        private PageInfo _pageInfo;
        public PageInfo PageInfo
        {
            get
            {
                if (_pageInfo == null && HttpContext.Current.Items.Contains(SiteConstants.PageData))
                    _pageInfo = HttpContext.Current.Items[SiteConstants.PageData] as PageInfo;
                return _pageInfo;
            }
        }

        private UserProfileInfo _currentUserProfileInfo;
        private UserProfileInfo CurrentUserProfileInfo
        {
            get
            {
                if (_currentUserProfileInfo == null && HttpContext.Current.Items.Contains(SiteConstants.UserData))
                    _currentUserProfileInfo = HttpContext.Current.Items[SiteConstants.UserData] as UserProfileInfo;
                return _currentUserProfileInfo;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            CheckUser();
            Init();
        }

        protected new virtual void Init()
        {
            if (ErrorList == null)
                ErrorList = new ErrorInfoList();
        }

        protected UserControl LoadControl(string path, params object[] constructorParameters)
        {
            List<Type> constParamTypes = new List<Type>();
            foreach (object constParam in constructorParameters)
            {
                constParamTypes.Add(constParam.GetType());
            }
            constParamTypes.Add(typeof(ErrorInfoList));
            UserControl control = Page.LoadControl(path) as UserControl;
            if (control != null)
            {
                var baseType = control.GetType().BaseType;
                if (baseType != null)
                {
                    ConstructorInfo constructor = baseType.GetConstructor(constParamTypes.ToArray());
                    if (constructor != null)
                    {
                        List<object> constructorParams = constructorParameters.ToList();
                        constructorParams.Add(ErrorList);

                        constructor.Invoke(control, constructorParams.ToArray());
                    }
                }
            }
            return control;
        }

        private void CheckUser()
        {
            if (PageInfo != null && PageInfo.AvailableRoles != null && PageInfo.AvailableRoles.Count > 0)
            {
                if (!PageInfo.AvailableRoles.Contains("All"))
                {
                    if (!Page.User.Identity.IsAuthenticated)
                    {
                        Response.Redirect(BuildUrl(PageInfo.RedirectNoAuthenticated));
                    }
                    else
                    {
                        bool permessionDenied = true;
                        List<string> roles = Roles.GetRolesForUser(Page.User.Identity.Name).ToList();
                        foreach (string role in PageInfo.AvailableRoles)
                        {
                            if (roles.Contains(role))
                            {
                                permessionDenied = false;
                                break;
                            }
                        }
                        if (permessionDenied)
                        {
                            Response.Redirect(BuildUrl(PageInfo.RedirectNoPermission));
                        }
                    }
                }
            }
        }

        protected virtual string GetControlValue(string controlid)
        {
            string[] values = Context.Request.Form.AllKeys;
            foreach (string item in values)
            {
                if (item.Contains(controlid))
                {
                    controlid = item;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(controlid) && Context.Request.Form.AllKeys.Contains(controlid))
                return Context.Request.Form[controlid];

            return string.Empty;
        }

        protected string GetCurrentLanguage()
        {
            if (HttpContext.Current.Items.Contains(SiteConstants.SiteLanguage))
                return HttpContext.Current.Items[SiteConstants.SiteLanguage].ToString();
            return string.Empty;

        }

        protected string GetResource(string keyword)
        {
            return LanguageHelper.Instance.GetTranslate(GetCurrentLanguage(), keyword);
        }

        protected string BuildUrl(string url)
        {
            string currentLanguage = GetCurrentLanguage();
            if (string.IsNullOrEmpty(currentLanguage))
                return url;

            if (url.Equals("/") || string.IsNullOrEmpty(url))
                return "/" + currentLanguage;
            return string.Format("/{0}{1}", currentLanguage, url);
        }

        protected virtual Dictionary<string, string> GetControlValues(string startWith)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string[] values = Context.Request.Form.AllKeys;
            foreach (string item in values)
            {
                if (item.StartsWith(startWith))
                {
                    dictionary.Add(item, Context.Request.Form[item]);
                }
            }
            return dictionary;
        }

        protected string GetSeoValue(string key, string defaultValue)
        {
            string[] templateTokens = PageInfo.SeoTemplate.Split('/');
            string tempToken = PageInfo.SeoUrl;
            if (tempToken.Contains("?"))
                tempToken = tempToken.Substring(0, tempToken.IndexOf("?"));
            string[] urlTokens = tempToken.Split('/');
            int index = 0;
            for (int i = 0; i < templateTokens.Length; i++)
            {
                if (templateTokens[i].Equals("{" + key + "}"))
                {
                    index = i;
                    break;
                }
            }
            if (index != 0)
                return urlTokens[index];

            return defaultValue;
        }

        protected string GetQueryValue(string key, string defaultValue)
        {
            return ValidationHelper.GetString(HttpUtility.UrlDecode(Request.QueryString[key]), defaultValue);
        }

        protected string GetCookieValue(string key, string defaultValue)
        {
            return ValidationHelper.GetString(Request.Cookies[key], defaultValue);
        }

        protected string GetUserProfile(string key)
        {
            if (Page.User.Identity.IsAuthenticated && CurrentUserProfileInfo != null)
            {
                return _currentUserProfileInfo.GetValue(key);
            }
            return string.Empty;
        }

        protected object[,] GetSelectParamaters(string queryParameters, out bool ok)
        {
            XElement parameterElements = XDocument.Parse(queryParameters).Element("parameters");
            if (parameterElements != null)
            {
                XElement[] elements = parameterElements.Elements("parameter").ToArray();

                object[,] paramaters = new object[elements.Length, 3];
                ok = false;
                int index = 0;
                foreach (XElement item in elements)
                {
                    if ((item.Attribute("type") != null && item.Attribute("type").Value == "0") && (item.Attribute("dbtype") != null && item.Attribute("dbtype").Value == "0"))
                    {
                        ok = false;
                    }
                    else
                    {
                        string type = item.Attribute("type").Value;
                        string name = item.Attribute("name").Value;
                        string value = item.Attribute("value").Value;

                        string dbtype = "-1";
                        if (item.Attribute("dbtype") != null)
                            dbtype = item.Attribute("dbtype").Value;
                        string defaultvalue = item.Attribute("defaultvalue").Value;

                        if (type == ((int)QueryParameterType.QueryString).ToString())
                        {
                            paramaters[index, 0] = name;
                            paramaters[index, 1] = GetQueryValue(value, defaultvalue);
                        }
                        if (type == ((int)QueryParameterType.SeoTemplate).ToString())
                        {
                            paramaters[index, 0] = name;
                            paramaters[index, 1] = GetSeoValue(value, defaultvalue);
                        }
                        if (type == ((int)QueryParameterType.Cookie).ToString())
                        {
                            paramaters[index, 0] = name;
                            paramaters[index, 1] = GetCookieValue(value, defaultvalue);
                        }
                        if (type == ((int)QueryParameterType.Language).ToString())
                        {
                            paramaters[index, 0] = name;
                            paramaters[index, 1] = GetCurrentLanguage();
                        }
                        if (type == ((int)QueryParameterType.UserProfileProperty).ToString())
                        {
                            paramaters[index, 0] = name;
                            paramaters[index, 1] = GetUserProfile(value);
                        }
                        if (dbtype.Equals("1"))
                            paramaters[index, 2] = SqlDbType.NVarChar;
                        else if (dbtype.Equals("2"))
                            paramaters[index, 2] = SqlDbType.Int;

                        if (!string.IsNullOrEmpty(name))
                            ok = true;
                        index++;
                    }
                }
                return paramaters;
            }
            ok = false;
            return null;
        }
    }
}
