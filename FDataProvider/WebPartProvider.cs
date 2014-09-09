using System;
using System.Collections.Generic;
using System.Data;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;

namespace FDataProvider
{
    public class WebPartProvider : BaseProvider<WebPartInfo>
    {
        public WebPartProvider()
            : this(null)
        {
        }

        public WebPartProvider(DataConnection connection)
        {
            if (connection != null)
                DataConnection = new DataConnection(connection.ConnectionString);
            else
                DataConnection = new DataConnection();
            EnsureCreated();
        }

        public override object Create(WebPartInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[6, 3];
                param[0, 0] = "@Name";
                param[0, 1] = info.Name;
                param[1, 0] = "@Description";
                param[1, 1] = info.Description;
                param[2, 0] = "@WebPartCategoryId";
                param[2, 1] = info.WebPartCategoryId;
                param[3, 0] = "@Screenshot";
                param[3, 1] = info.Screenshot;
                param[4, 0] = "@FolderPath";
                param[4, 1] = info.FolderPath;
                param[5, 0] = "@IsDeleted";
                param[5, 1] = info.IsDeleted;

                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_WebPart_Insert", param, QueryType.StoredProcedure,
                                                             error);
                if (error.Ok)
                {
                    return result;
                }
                RegisterError(errors, error);
            }
            else
            {
                var error = new ErrorInfo();
                error.Ok = false;
                error.Date = DateTime.Now;
                error.Message = "WebPartInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(WebPartInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[7, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@Description";
                param[2, 1] = info.Description;
                param[3, 0] = "@WebPartCategoryId";
                param[3, 1] = info.WebPartCategoryId;
                param[4, 0] = "@Screenshot";
                param[4, 1] = info.Screenshot;
                param[5, 0] = "@FolderPath";
                param[5, 1] = info.FolderPath;
                param[6, 0] = "@IsDeleted";
                param[6, 1] = info.IsDeleted;

                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_WebPart_Update", param, QueryType.StoredProcedure,
                                                             error);
                if (error.Ok)
                {
                    return true;
                }
                RegisterError(errors, error);
            }
            else
            {
                var error = new ErrorInfo();
                error.Ok = false;
                error.Date = DateTime.Now;
                error.Message = "WebPartInfo object is null";
                RegisterError(errors, error);
            }

            return false;
        }

        public override bool Delete(int id, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "@Id";
            param[0, 1] = id;
            DataConnection.ExecuteScalar("freb_WebPart_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override WebPartInfo Select(int id, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "@Id";
            param[0, 1] = id;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_WebPart_SelectById", param,
                                                                       QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                return new WebPartInfo(dataTable.Rows[0]);
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<WebPartInfo> SelectAll(ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_WebPart_SelectAll", null,
                                                                       QueryType.StoredProcedure, error);
            var result = new List<WebPartInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new WebPartInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<WebPartInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy,
                                                              string sortOrder, ErrorInfoList errors)
        {
            var param = new object[4, 3];
            param[0, 0] = "@PageSize";
            param[0, 1] = pageSize;
            param[1, 0] = "@PageIndex";
            param[1, 1] = pageIndex;
            param[2, 0] = "@OrderBy";
            param[2, 1] = sortBy;
            param[3, 0] = "@SortOrder";
            param[3, 1] = sortOrder;
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_WebPart_SelectByPagingSorting", param,
                                                                       QueryType.StoredProcedure, error);
            var result = new List<WebPartInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new WebPartInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override void RegisterObjectToCache(WebPartInfo info)
        {
            throw new NotImplementedException();
        }

        public override void DeleteObjectFromCache(WebPartInfo info)
        {
            throw new NotImplementedException();
        }

        public override WebPartInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override WebPartInfo GetObjectFromCache(string name)
        {
            throw new NotImplementedException();
        }

        public List<WebPartInfo> SelectPagingSortingByCategory(int pageSize, int pageIndex, string sortBy,
                                                               string sortOrder, int categoryId, ErrorInfoList errors)
        {
            var param = new object[5, 3];
            param[0, 0] = "@PageSize";
            param[0, 1] = pageSize;
            param[1, 0] = "@PageIndex";
            param[1, 1] = pageIndex;
            param[2, 0] = "@OrderBy";
            param[2, 1] = sortBy;
            param[3, 0] = "@SortOrder";
            param[3, 1] = sortOrder;
            param[4, 0] = "@WebPartCategoryId";
            param[4, 1] = categoryId;
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_WebPart_SelectByPagingSorting", param,
                                                                       QueryType.StoredProcedure, error);
            var result = new List<WebPartInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new WebPartInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        private void EnsureCreated()
        {
        }
    }
}
