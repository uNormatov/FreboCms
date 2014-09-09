using System;
using System.Collections.Generic;
using System.Data;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;

namespace FDataProvider
{
    public class FormProvider : BaseProvider<FormInfo>
    {
        private static GoodDictionary<string, FormInfo> _collection;

        public FormProvider()
            : this(null)
        {
        }

        public FormProvider(DataConnection connection)
        {
            if (connection != null)
                DataConnection = new DataConnection(connection.ConnectionString);
            else
                DataConnection = new DataConnection();
            EnsureCreated();
        }

        public override object Create(FormInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[4, 3];
                param[0, 0] = "@Name";
                param[0, 1] = info.Name;
                param[1, 0] = "@DisplayName";
                param[1, 1] = info.DisplayName;
                param[2, 0] = "@Layout";
                param[2, 1] = info.Layout;
                param[3, 0] = "@ContentTypeId";
                param[3, 1] = info.ContentTypeId;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_Form_Insert", param, QueryType.StoredProcedure, error);
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
                error.Date = DateTime.Now;
                error.Message = "FormInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(FormInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[5, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@DisplayName";
                param[2, 1] = info.DisplayName;
                param[3, 0] = "@Layout";
                param[3, 1] = info.Layout;
                param[4, 0] = "@ContentTypeId";
                param[4, 1] = info.ContentTypeId;

                var error = new ErrorInfo();
                DataConnection.ExecuteScalar("freb_Form_Update", param, QueryType.StoredProcedure, error);
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
                error.Name = "Object is null";
                error.Date = DateTime.Now;
                error.Message = "FormInfo object is null";
                RegisterError(errors, error);
            }

            return false;
        }

        public override bool Delete(int id, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataConnection.ExecuteDataTableQuery("freb_Form_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override FormInfo Select(int id, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Form_SelectById", param,
                                                                       QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                return new FormInfo(dataTable.Rows[0]);
            }

            RegisterError(errors, error);
            return null;
        }

        public FormInfo SelectByName(string queryName, ErrorInfoList errors)
        {
            var param = new object[1, 3];
            param[0, 0] = "@Name";
            param[0, 1] = queryName;

            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Form_SelectByName", param,
                                                                       QueryType.StoredProcedure, error);

            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                var query = new FormInfo(dataTable.Rows[0]);
                return query;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<FormInfo> SelectAll(ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override List<FormInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder,
                                                           ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public List<FormInfo> SelectPagingSortingByContentTypeId(int pageSize, int pageIndex, string sortBy,
                                                                 string sortOrder, int contentTypeId,
                                                                 ErrorInfoList errors)
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
            param[4, 0] = "@ContentTypeId";
            param[4, 1] = contentTypeId;
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery(
                "freb_Form_SelectByPagingSortingByContentTypeId", param, QueryType.StoredProcedure, error);
            var result = new List<FormInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new FormInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public List<FormInfo> SelectAllByContentTypeId(int contentTypeId, ErrorInfoList errors)
        {
            var param = new object[1, 3];
            param[0, 0] = "@ContentTypeId";
            param[0, 1] = contentTypeId;
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Form_SelectAllByContentTypeId", param,
                                                                       QueryType.StoredProcedure, error);
            var result = new List<FormInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new FormInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public int SelectTotalCountByContentTypeId(int contentTypeId, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "ContentTypeId";
            param[0, 1] = contentTypeId;

            object result = DataConnection.ExecuteScalar("[dbo].[freb_Form_TotalCount_ByContentType]", param,
                                                         QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return ValidationHelper.GetInteger(result, 0);
            }
            RegisterError(errors, error);
            return 0;
        }

        public override void RegisterObjectToCache(FormInfo info)
        {
            if (_collection.ContainsKey(info.Name))
                _collection[info.Name] = info;
            else
                _collection.Add(info.Name, info);
        }

        public override void DeleteObjectFromCache(FormInfo info)
        {
            if (_collection.ContainsKey(info.Name))
                _collection.Remove(info.Name);
        }

        public override FormInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override FormInfo GetObjectFromCache(string name)
        {
            if (_collection.ContainsKey(name))
                return _collection[name];
            return null;
        }

        private void EnsureCreated()
        {
            if (_collection == null)
                _collection = new GoodDictionary<string, FormInfo>();
        }
    }
}
