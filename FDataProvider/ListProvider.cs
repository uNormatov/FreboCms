using System;
using System.Collections.Generic;
using System.Data;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;

namespace FDataProvider
{
    public class ListProvider : BaseProvider<ListInfo>
    {
        public ListProvider()
            : this(null)
        {
        }

        public ListProvider(DataConnection connection)
        {
            DataConnection = connection ?? new DataConnection();
        }

        public override object Create(ListInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[2, 3];
                param[0, 0] = "@Name";
                param[0, 1] = info.Name;
                param[1, 0] = "@Description";
                param[1, 1] = info.Description;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_List_Insert", param, QueryType.StoredProcedure, error);
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
                error.Message = "ListInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(ListInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[3, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@Description";
                param[2, 1] = info.Description;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_List_Update", param, QueryType.StoredProcedure, error);
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
                error.Message = "ListInfo object is null";
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
            DataConnection.ExecuteScalar("freb_List_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override ListInfo Select(int id, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "@Id";
            param[0, 1] = id;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_List_SelectById", param,
                                                                       QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                var listInfo = new ListInfo(dataTable.Rows[0]);
                return listInfo;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<ListInfo> SelectAll(ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_List_SelectAll", null,
                                                                       QueryType.StoredProcedure, error);
            var result = new List<ListInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new ListInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<ListInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder,
                                                           ErrorInfoList errors)
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
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_List_SelectByPagingSorting", param,
                                                                       QueryType.StoredProcedure, error);
            var result = new List<ListInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new ListInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override void RegisterObjectToCache(ListInfo info)
        {
            throw new NotImplementedException();
        }

        public override void DeleteObjectFromCache(ListInfo info)
        {
            throw new NotImplementedException();
        }

        public override ListInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override ListInfo GetObjectFromCache(string name)
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
