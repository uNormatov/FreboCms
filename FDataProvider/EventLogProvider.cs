using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using FCore.Class;
using FCore.Collection;
using FCore.Constant;
using FCore.Enum;
using FCore.Helper;

namespace FDataProvider
{
    public class EventLogProvider : BaseProvider<EventLogInfo>
    {
        public EventLogProvider()
            : this(null)
        {
        }

        public EventLogProvider(DataConnection dataConnection)
        {
            if (Connection != null)
                DataConnection = dataConnection;
            else
                DataConnection = new DataConnection();
            EnsureCreated();
        }

        public override object Create(EventLogInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[8, 3];
                param[0, 0] = "@PageId";
                param[0, 1] = info.PageId;
                param[1, 0] = "@FullUrl";
                param[1, 1] = info.FullUrl;
                param[2, 0] = "@PageUrl";
                param[2, 1] = info.PageUrl;
                param[3, 0] = "@ReferalUrl";
                param[3, 1] = info.ReferalUrl;
                param[4, 0] = "@Ip";
                param[4, 1] = info.Ip;
                param[5, 0] = "@UserName";
                param[5, 1] = info.UserName;
                param[6, 0] = "@UserAgent";
                param[6, 1] = info.UserAgent;
                param[7, 0] = "@Date";
                param[7, 1] = info.Date;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_EventLog_Insert", param, QueryType.StoredProcedure,
                                                             error);
                if (error.Ok)
                {
                    info.Id = ValidationHelper.GetInteger(result, 0);
                    return result;
                }
                RegisterError(errors, error);
            }
            else
            {
                var error = new ErrorInfo();
                error.Ok = false;
                error.Name = "Object is null";
                error.Date = DateTime.Now;
                error.Message = "EventLogInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(EventLogInfo info, ErrorInfoList errors)
        {
            return false;
        }

        public override bool Delete(int id, ErrorInfoList errors)
        {
            return false;
        }

        public override EventLogInfo Select(int id, ErrorInfoList errors)
        {
            return null;
        }

        public override List<EventLogInfo> SelectAll(ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override List<EventLogInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override void RegisterObjectToCache(EventLogInfo info)
        {

        }

        public override void DeleteObjectFromCache(EventLogInfo info)
        {
        }

        public override EventLogInfo GetObjectFromCache(int id)
        {
            return null;
        }

        public override EventLogInfo GetObjectFromCache(string name)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                disposing = true;
            }
        }

        private void EnsureCreated()
        {

        }
    }
}
