using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FUIControls.Context
{
    public class FContext
    {
        public static string CurrentSeoUrl
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    string seourl = "";
                    object o = RequestStockHelper.GetItem("CurrentSeoUrl");
                    if (o == null)
                    {
                        if (!string.IsNullOrEmpty(HttpContext.Current.Request.RawUrl))
                        {
                            seourl = HttpUtility.UrlDecode(HttpContext.Current.Request.RawUrl).ToLower();

                            if (seourl.ToLower() == "/default.aspx")
                            {
                                seourl = "/";
                            }
                        }
                        else
                        {
                            seourl = HttpContext.Current.Request.Path.ToLower();
                        }
                    }
                    else
                        seourl = o.ToString();
                    RequestStockHelper.Add("CurrentSeoUrl", seourl, true);
                    return seourl;
                }

                return "";
            }
            set
            {
                RequestStockHelper.Add("CurrentSeoUrl", value, true);
            }
        }

        public static string CurrentUrl
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    string seourl = CurrentSeoUrl;
                    object o = RequestStockHelper.GetItem("CurrentUrl");
                    if (o == null)
                    {
                        bool first = true;
                        string[] queries = HttpContext.Current.Request.Url.Query.Split('&');
                        foreach (string item in queries)
                        {
                            if (!item.ToLower().Contains(CurrentSeoUrl) && !string.IsNullOrEmpty(item))
                            {
                                if (first)
                                {
                                    if (!item.Contains("?"))
                                        seourl += "?" + item;
                                    else
                                        seourl += item;
                                    first = false;
                                }
                                else
                                    seourl += "&" + item;
                            }
                        }
                    }
                    else
                        seourl = o.ToString();
                    RequestStockHelper.Add("CurrentUrl", seourl, true);
                    return seourl;
                }

                return "";
            }
            set
            {
                RequestStockHelper.Add("CurrentUrl", value, true);
            }
        }

        public static string CurrentQueryString
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    string seourl = string.Empty;
                    object o = RequestStockHelper.GetItem("CurrentQueryString");
                    if (o == null)
                    {
                        bool first = true;
                        string[] queries = HttpContext.Current.Request.Url.Query.Split('&');
                        foreach (string item in queries)
                        {

                            if (!item.ToLower().Contains(CurrentSeoUrl) &&
                                !string.IsNullOrEmpty(item))
                            {
                                if (first)
                                {
                                    if (!item.Contains("?"))
                                        seourl += "?" + item;
                                    else
                                        seourl += item;
                                    first = false;
                                }
                                else
                                    seourl += "&" + item;
                            }
                        }
                    }
                    else
                        seourl = o.ToString();
                    RequestStockHelper.Add("CurrentQueryString", seourl, true);
                    return seourl;
                }

                return "";
            }
            set
            {
                RequestStockHelper.Add("CurrentQueryString", value, true);
            }
        }

        public static string QueryString(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            string[] pars = CurrentQueryString.Split('&');
            if (pars != null && pars.Length > 0)
            {
                foreach (string item in pars)
                {
                    string[] token = item.Split('=');
                    if (token[0].Contains("?"))
                        name = "?" + name;
                    if (token[0].ToLower() == name.ToLower())
                        return token[1];
                }
            }
            return string.Empty;
        }
    }
}
