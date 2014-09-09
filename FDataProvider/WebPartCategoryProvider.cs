using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FCore.Class;
using System.Data;
using FCore.Collection;
using FCore.Helper;
using FCore.Enum;

namespace FDataProvider
{
    public class WebPartCategoryProvider : BaseProvider<WebPartCategoryInfo>
    {
        public WebPartCategoryProvider() : this(null) { }

        public WebPartCategoryProvider(DataConnection connection)
        {
            if (connection != null)
                this.DataConnection = new DataConnection(connection.ConnectionString);
            else
                this.DataConnection = new DataConnection();
            EnsureCreated();
        }

        public override object Create(WebPartCategoryInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[2, 3];
                param[0, 0] = "@Name";
                param[0, 1] = info.Name;
                param[1, 0] = "@Description";
                param[1, 1] = info.Description;
                ErrorInfo error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_WebPartCategory_Insert", param, QueryType.StoredProcedure, error);
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
                error.Message = "WebPartCategoryInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(WebPartCategoryInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[3, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@Description";
                param[2, 1] = info.Description;
                ErrorInfo error = new ErrorInfo();
                object result = this.DataConnection.ExecuteScalar("freb_WebPartCategory_Update", param, QueryType.StoredProcedure, error);
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
                error.Message = "WebPartCategoryInfo object is null";
                RegisterError(errors, error);
            }

            return false;
        }

        public override bool Delete(int id, ErrorInfoList errors)
        {
            ErrorInfo error = new ErrorInfo();
            object[,] param = new object[1, 3];
            param[0, 0] = "@Id";
            param[0, 1] = id;
            this.DataConnection.ExecuteScalar("freb_WebPartCategory_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override WebPartCategoryInfo Select(int id, ErrorInfoList errors)
        {
            ErrorInfo error = new ErrorInfo();
            object[,] param = new object[1, 3];
            param[0, 0] = "@Id";
            param[0, 1] = id;
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("freb_WebPartCategory_SelectById", param, QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                return new WebPartCategoryInfo(dataTable.Rows[0]);

            }
            RegisterError(errors, error);
            return null;
        }

        public override List<WebPartCategoryInfo> SelectAll(ErrorInfoList errors)
        {
            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("freb_WebPartCategory_SelectAll", null, QueryType.StoredProcedure, error);
            List<WebPartCategoryInfo> result = new List<WebPartCategoryInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new WebPartCategoryInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<WebPartCategoryInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
        {
            object[,] param = new object[4, 3];
            param[0, 0] = "@PageSize";
            param[0, 1] = pageSize;
            param[1, 0] = "@PageIndex";
            param[1, 1] = pageIndex;
            param[2, 0] = "@OrderBy";
            param[2, 1] = sortBy;
            param[3, 0] = "@SortOrder";
            param[3, 1] = sortOrder;
            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("freb_WebPartCategory_SelectByPagingSortingType", param, QueryType.StoredProcedure, error);
            List<WebPartCategoryInfo> result = new List<WebPartCategoryInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new WebPartCategoryInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override void RegisterObjectToCache(WebPartCategoryInfo info)
        {
            throw new NotImplementedException();
        }

        public override void DeleteObjectFromCache(WebPartCategoryInfo info)
        {
            throw new NotImplementedException();
        }

        public override WebPartCategoryInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override WebPartCategoryInfo GetObjectFromCache(string name)
        {
            throw new NotImplementedException();
        }

        private void EnsureCreated()
        {

        }

    }
}
