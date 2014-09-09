using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using FCore.Class;
using FCore.Collection;
using FCore.Helper;
using FDataProvider;


namespace FModules
{
    public class CommentBoxHandler : IHttpHandler
    {
        private CommentBoxProvider _commentBoxProvider;

        public void ProcessRequest(HttpContext context)
        {
            if (_commentBoxProvider == null)
                _commentBoxProvider = new CommentBoxProvider();

            string action = ValidationHelper.GetString(context.Request.QueryString["action"], string.Empty);
            string url = ValidationHelper.GetString(context.Request.QueryString["url"], string.Empty);
            string data = ValidationHelper.GetString(context.Request.QueryString["data"], string.Empty);
            string result = string.Empty;
            if (!string.IsNullOrEmpty(action) && !string.IsNullOrEmpty(url))
            {
                if (action == "list")
                    result = GetComments(url);
                else if (action == "add")
                    result = AddComment(context.Request);
            }
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get { return false; }
        }

        private string GetComments(string url)
        {
            List<CommentBoxInfo> comments = _commentBoxProvider.SelectByUrl(url, new ErrorInfoList());
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(comments);
        }

        private string AddComment(HttpRequest request)
        {
            int contentTypeId = ValidationHelper.GetInteger(request.QueryString["contenttypeid"], 0);
            string email = ValidationHelper.GetString(request.QueryString["email"], string.Empty);
            string name = ValidationHelper.GetString(request.QueryString["name"], string.Empty);
            string body = ValidationHelper.GetString(request.QueryString["body"], string.Empty);
            string seotemplate = ValidationHelper.GetString(request.QueryString["seotemplate"], string.Empty);
            int order = ValidationHelper.GetInteger(request.QueryString["order"], 0);
            string url = ValidationHelper.GetString(request.QueryString["url"], string.Empty);
            string website = ValidationHelper.GetString(request.QueryString["website"], string.Empty);
            int parentId = ValidationHelper.GetInteger(request.QueryString["parentid"], 0);
            JavaScriptSerializer js = new JavaScriptSerializer();

            CommentBoxInfo comment = new CommentBoxInfo();
            comment.Name = name;
            comment.Email = email;
            comment.Body = body;
            comment.SeoTemplate = seotemplate;
            comment.Order = order + 1;
            comment.Url = url;
            comment.Website = website;
            comment.CreatedDate = DateTime.Now;
            comment.ContentTypeId = contentTypeId;
            ErrorInfoList errorInfoList = new ErrorInfoList();
            _commentBoxProvider.Create(comment, errorInfoList);
            if (errorInfoList.HasError())
                comment.IsOk = false;
            else
                comment.IsOk = true;
            comment.Time = UzbKeywordHelper.GetDateString(comment.CreatedDate.Value, DateTime.Now);
            return js.Serialize(comment);
        }
    }
}
