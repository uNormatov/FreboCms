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
    public class QueryProvider : BaseProvider<QueryInfo>
    {
        private static GoodDictionary<string, QueryInfo> _collection;

        public QueryProvider() : this(null) { }

        public QueryProvider(DataConnection connection)
        {
            if (connection != null)
                this.DataConnection = new DataConnection(connection.ConnectionString);
            else
                this.DataConnection = new DataConnection();
            EnsureCreated();
        }

        public override object Create(QueryInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[4, 3];
                param[0, 0] = "@Name";
                param[0, 1] = info.Name;
                param[1, 0] = "@Text";
                param[1, 1] = info.Text;
                param[2, 0] = "@ContentTypeId";
                param[2, 1] = info.ContentTypeId;
                param[3, 0] = "@IsDeleted";
                param[3, 1] = info.IsDeleted;
                ErrorInfo error = new ErrorInfo();
                object result = this.DataConnection.ExecuteScalar("freb_Query_Insert", param, QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    info.Id = ValidationHelper.GetInteger(result, 0);
                    _collection.Clear();
                    return result;
                }
                RegisterError(errors, error);
            }
            else
            {
                ErrorInfo error = new ErrorInfo();
                error.Ok = false;
                error.Date = DateTime.Now;
                error.Message = "QueryInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(QueryInfo info, ErrorInfoList errors)
        {

            if (info != null)
            {
                object[,] param = new object[5, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@Text";
                param[2, 1] = info.Text;
                param[3, 0] = "@ContentTypeId";
                param[3, 1] = info.ContentTypeId;
                param[4, 0] = "@IsDeleted";
                param[4, 1] = info.IsDeleted;
                ErrorInfo error = new ErrorInfo();
                this.DataConnection.ExecuteScalar("freb_Query_Update", param, QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    _collection.Clear();
                    return true;
                }
                RegisterError(errors, error);
            }
            else
            {
                ErrorInfo error = new ErrorInfo();
                error.Ok = false;
                error.Name = "Object is null";
                error.Date = DateTime.Now;
                error.Message = "QueryInfo object is null";
                RegisterError(errors, error);
            }

            return false;
        }

        public override bool Delete(int id, ErrorInfoList errors)
        {
            ErrorInfo error = new ErrorInfo();
            object[,] param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataConnection.ExecuteDataTableQuery("freb_Query_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                _collection.Clear();
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override QueryInfo Select(int id, ErrorInfoList errors)
        {
            ErrorInfo error = new ErrorInfo();
            object[,] param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("freb_Query_SelectById", param, QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                QueryInfo query = new QueryInfo(dataTable.Rows[0]);
                return query;
            }

            RegisterError(errors, error);
            return null;
        }

        public override List<QueryInfo> SelectAll(ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override List<QueryInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public List<QueryInfo> SelectPagingSortingByContentTypeId(int pageSize, int pageIndex, string sortBy, string sortOrder, int contentTypeId, ErrorInfoList errors)
        {
            object[,] param = new object[5, 3];
            param[0, 0] = "@PageSize";
            param[0, 1] = pageSize;
            param[1, 0] = "@PageIndex";
            param[1, 1] = pageIndex;
            param[2, 0] = "@OrderBy";
            param[2, 1] = sortBy;
            param[3, 0] = "@SortOrder";
            param[3, 1] = sortOrder;
            param[4, 0] = "@ContentTypeId";
            param[4, 1] = contentTypeId;
            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("freb_Query_SelectByPagingSortingByContentTypeId", param, QueryType.StoredProcedure, error);
            List<QueryInfo> result = new List<QueryInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new QueryInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public List<QueryInfo> SelectPagingSortingByContentTypeIdSearchKeyword(int pageSize, int pageIndex, string sortBy, string sortOrder, int contentTypeId, string keyword, ErrorInfoList errors)
        {
            object[,] param = new object[6, 3];
            param[0, 0] = "@PageSize";
            param[0, 1] = pageSize;
            param[1, 0] = "@PageIndex";
            param[1, 1] = pageIndex;
            param[2, 0] = "@OrderBy";
            param[2, 1] = sortBy;
            param[3, 0] = "@SortOrder";
            param[3, 1] = sortOrder;
            param[4, 0] = "@ContentTypeId";
            param[4, 1] = contentTypeId;
            param[5, 0] = "@Keyword";
            param[5, 1] = keyword;
            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("[dbo].[freb_Query_SelectByPagingSortingByKeyword]", param, QueryType.StoredProcedure, error);
            List<QueryInfo> result = new List<QueryInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new QueryInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public int SelectTotalCountByContentTypeId(int contentTypeId, ErrorInfoList errors)
        {
            ErrorInfo error = new ErrorInfo();
            object[,] param = new object[1, 3];
            param[0, 0] = "ContentTypeId";
            param[0, 1] = contentTypeId;

            object result = DataConnection.ExecuteScalar("[dbo].[freb_Query_TotalCount_ByContentType]", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return ValidationHelper.GetInteger(result, 0);
            }
            RegisterError(errors, error);
            return 0;
        }

        public int SelectTotalCountByContentTypeIdKeyword(int contentTypeId, string keyword, ErrorInfoList errors)
        {
            ErrorInfo error = new ErrorInfo();
            object[,] param = new object[2, 3];
            param[0, 0] = "@ContentTypeId";
            param[0, 1] = contentTypeId;
            param[1, 0] = "@Keyword";
            param[1, 1] = keyword;

            object result = DataConnection.ExecuteScalar("[dbo].[freb_Query_TotalCount_ByContentTypeKeyword]", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return ValidationHelper.GetInteger(result, 0);
            }
            RegisterError(errors, error);
            return 0;
        }

        public QueryInfo SelectByName(string queryName, ErrorInfoList errors)
        {
            QueryInfo queryInfo = GetObjectFromCache(queryName);
            if (queryInfo != null)
                return queryInfo;

            object[,] param = new object[1, 3];
            param[0, 0] = "@Name";
            param[0, 1] = queryName;

            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("freb_Query_SelectByName", param, QueryType.StoredProcedure, error);

            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                queryInfo = new QueryInfo(dataTable.Rows[0]);
                RegisterObjectToCache(queryInfo);
                return queryInfo;
            }
            RegisterError(errors, error);
            return null;
        }

        private void EnsureCreated()
        {
            if (_collection == null)
                _collection = new GoodDictionary<string, QueryInfo>();
        }



        public override void RegisterObjectToCache(QueryInfo info)
        {
            if (_collection.ContainsKey(info.Name))
                _collection[info.Name] = info;
            else
                _collection.Add(info.Name, info);
        }

        public override void DeleteObjectFromCache(QueryInfo info)
        {
            if (_collection.ContainsKey(info.Name))
                _collection.Remove(info);
        }

        public override QueryInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override QueryInfo GetObjectFromCache(string name)
        {
            if (_collection.ContainsKey(name))
                return _collection[name];
            return null;
        }
    }
}
