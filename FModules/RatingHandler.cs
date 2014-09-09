using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FModules
{
    public class RatingHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            const float rating1 = 10;
            const float count = 2;
            context.Response.ContentType = "text/plain";

            try
            {
                float rating = rating1 + float.Parse(context.Request.Form["rating"]);
                float result = rating / (count + 1);
                context.Response.Write(result.ToString("0.0"));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 202;
                context.Response.Write(ex.Message);
                context.Response.Flush();
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
