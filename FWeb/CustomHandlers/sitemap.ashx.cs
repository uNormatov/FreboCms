using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FWeb.CustomHandlers
{
    /// <summary>
    /// Summary description for sitemap
    /// </summary>
    public class Sitemap : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        }

        private void WriteSiteMap()
        {

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}