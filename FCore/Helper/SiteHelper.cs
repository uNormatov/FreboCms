using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FCore.Helper
{
    public static class SiteHelper
    {
        public static string ToUrl(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char[] c = new char[s.Length];

            int k = -1;
            for (int i = 0; i < s.Length; i++)
            {
                if ((s[i] >= 'a' && s[i] <= 'z') || (s[i] >= 'A' && s[i] <= 'Z') || (s[i] >= '0' && s[i] <= '9'))
                {
                    k++;
                    c[k] = s[i];
                }
                else
                {
                    if (k > 0)
                    {
                        if (!c[k].Equals('-'))
                        {
                            k++;
                            c[k] = '-';
                        }
                    }
                }
            }

            if (k == -1)
                return string.Empty;

            if (c[k].Equals('-'))
            {
                k--;
                if (k == -1)
                    return string.Empty;
            }

            return new string(c, 0, k + 1).ToLower();
        }

        public static string ToContentUrl(string parenturl, string url)
        {

            if (parenturl.EndsWith("/"))
            {
                url = parenturl + url;
            }
            else if (parenturl.ToLower().EndsWith("default.aspx"))
            {
                string s = parenturl.Substring(0, parenturl.LastIndexOf("/") + 1);
                url = s + url;
            }
            else
                url = parenturl + "/" + url;
            return url;
        }

        public static string GetSiteUrl()
        {
            Uri uri = HttpContext.Current.Request.Url;
            return (uri.Scheme + "://" + uri.Host + (uri.Port == 80 ? "" : ":" + uri.Port)).ToLower();

        }

        public static string ToHtmlEncode(this string text)
        {
            return HttpUtility.HtmlEncode(text);
        }

        public static string ToHtmlDecode(this string text)
        {
            return HttpUtility.HtmlDecode(text);
        }

        public static bool IsImageFile(string value)
        {
            return value.ToLower().EndsWith(".jpg") || value.ToLower().EndsWith(".jpeg") ||
                   value.ToLower().EndsWith(".png") || value.ToLower().EndsWith(".gif") ||
                   value.ToLower().EndsWith(".tiff");
        }
    }
}
