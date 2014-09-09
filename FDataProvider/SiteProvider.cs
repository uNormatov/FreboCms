using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FCore.Class;
using FCore.Collection;
using System.Data;
using FCore.Enum;

namespace FDataProvider
{
    public class SiteProvider : BaseProvider<SiteInfo>
    {
        private static SiteInfo siteInfo;

        public SiteProvider() : this(null) { }

        public SiteProvider(DataConnection connection)
        {
            if (connection != null)
                this.DataConnection = connection;
            else
                this.DataConnection = new DataConnection();
        }

        public override object Create(SiteInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[2, 3];
                param[0, 0] = "@Name";
                param[0, 1] = info.Name;
                param[1, 0] = "@DefaultPageId";
                param[1, 1] = info.DefaultPageId;
                ErrorInfo error = new ErrorInfo();
                object result = this.DataConnection.ExecuteScalar("freb_Site_Insert", param, QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    return result;
                }
                RegisterError(errors, error);
            }
            else
            {
                ErrorInfo error = new ErrorInfo();
                error.Ok = false;
                error.Date = DateTime.Now;
                error.Message = "SiteInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(SiteInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[6, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@DefaultPageId";
                param[2, 1] = info.DefaultPageId;
                param[3, 0] = "@NotFoundPageId";
                param[3, 1] = info.NotFoundPageId;
                param[4, 0] = "@IsMultilanguage";
                param[4, 1] = info.IsMultilanguage;
                param[5, 0] = "@DefaultLanguage";
                param[5, 1] = info.DefaultLanguage;
                ErrorInfo error = new ErrorInfo();
                object result = this.DataConnection.ExecuteScalar("freb_Site_Update", param, QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    return true;
                }
                RegisterError(errors, error);
            }
            else
            {
                ErrorInfo error = new ErrorInfo();
                error.Ok = false;
                error.Date = DateTime.Now;
                error.Message = "Site object is null";
                RegisterError(errors, error);
            }

            return false;
        }

        public override bool Delete(int id, ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override SiteInfo Select(int id, ErrorInfoList errors)
        {
            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Site_Select", null, QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                SiteInfo listInfo = new SiteInfo(dataTable.Rows[0]);
                return listInfo;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<SiteInfo> SelectAll(ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override List<SiteInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override void RegisterObjectToCache(SiteInfo info)
        {
            throw new NotImplementedException();
        }

        public override void DeleteObjectFromCache(SiteInfo info)
        {
            throw new NotImplementedException();
        }

        public override SiteInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override SiteInfo GetObjectFromCache(string name)
        {
            throw new NotImplementedException();
        }
    }
}


