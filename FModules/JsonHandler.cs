using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using FCore.Class;
using FCore.Collection;
using FCore.Constant;
using FCore.Helper;
using FDataProvider;

namespace FModules
{
    public class JsonHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            response.AddHeader("Content-Type", "application/json; charset=UTF-8");
            HttpRequest request = context.Request;
            int jsonType = ValidationHelper.GetInteger(request.QueryString["type"], -1);
            if (jsonType != -1)
            {
                switch (jsonType)
                {
                    case SiteConstants.JsonFormsByContentTypeId:
                        GetFormsByContentTypeId(request, response);
                        break;
                    case SiteConstants.JsonWebpartZoneByPageId:
                        GetWebpartZoneByPageId(request, response);
                        break;
                    case SiteConstants.JsonWebpartZoneByLayoutId:
                        GetWebpartZoneByLayoutId(request, response);
                        break;
                    case SiteConstants.JsonPollVote:
                        GetPollVote(request, response);
                        break;
                }
            }
        }

        private void GetFormsByContentTypeId(HttpRequest request, HttpResponse response)
        {
            int contentTypeId = ValidationHelper.GetInteger(request.QueryString["id"], 0);
            using (FormProvider formProvider = new FormProvider())
            {
                List<FormInfo> formInfos = formProvider.SelectAllByContentTypeId(contentTypeId, new ErrorInfoList());
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<FormInfo>));

                serializer.WriteObject(response.OutputStream, formInfos);
                response.OutputStream.Close();
            }
        }

        private void GetWebpartZoneByPageId(HttpRequest request, HttpResponse response)
        {
            int pageId = ValidationHelper.GetInteger(request.QueryString["id"], 0);
            using (PageProvider pageProvider = new PageProvider())
            {
                using (LayoutWebPartZoneProvider layoutWebPartZoneProvider = new LayoutWebPartZoneProvider())
                {
                    PageInfo pageInfo = pageProvider.Select(pageId, new ErrorInfoList());
                    if (pageInfo != null)
                    {
                        List<LayoutWebPartZoneInfo> webPartZones =
                            layoutWebPartZoneProvider.SelectAllByLayoutId(pageInfo.PageLayoutId, new ErrorInfoList());
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<LayoutWebPartZoneInfo>));

                        serializer.WriteObject(response.OutputStream, webPartZones);
                        response.OutputStream.Close();
                    }
                }
            }
        }

        private void GetWebpartZoneByLayoutId(HttpRequest request, HttpResponse response)
        {
            int layoutId = ValidationHelper.GetInteger(request.QueryString["id"], 0);
            using (LayoutWebPartZoneProvider layoutWebPartZoneProvider = new LayoutWebPartZoneProvider())
            {
                List<LayoutWebPartZoneInfo> webPartZones = layoutWebPartZoneProvider.SelectAllByLayoutId(layoutId, new ErrorInfoList());
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<LayoutWebPartZoneInfo>));

                serializer.WriteObject(response.OutputStream, webPartZones);
                response.OutputStream.Close(); 
            }

        }

        private void GetPollVote(HttpRequest request, HttpResponse response)
        {
            int pollId = ValidationHelper.GetInteger(request.QueryString["poll"], 0);
            if (pollId == 0)
                return;
            int voteId = ValidationHelper.GetInteger(request.QueryString["vote"], 0);
        }

    }
}