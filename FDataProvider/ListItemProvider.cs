using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FCore.Class;
using FCore.Collection;
using System.Data;
using FCore.Enum;
using FCore.Helper;

namespace FDataProvider
{
    public class ListItemProvider : BaseProvider<ListItemInfo>
    {
        public ListItemProvider() : this(null) { }

        public ListItemProvider(DataConnection connection)
        {
            this.DataConnection = connection ?? new DataConnection();
        }

        public override object Create(ListItemInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[5, 3];
                param[0, 0] = "@Name";
                param[0, 1] = info.Name;
                param[1, 0] = "@SeoTemplate";
                param[1, 1] = info.SeoTemplate;
                param[2, 0] = "@Description";
                param[2, 1] = info.Description;
                param[3, 0] = "@ListId";
                param[3, 1] = info.ListId;
                param[4, 0] = "@ParentId";
                param[4, 1] = info.ParentId;
                ErrorInfo error = new ErrorInfo();
                object result = this.DataConnection.ExecuteScalar("freb_ListItem_Insert", param, QueryType.StoredProcedure, error);
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
                error.Message = "ListItemInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(ListItemInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[6, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@SeoTemplate";
                param[2, 1] = info.SeoTemplate;
                param[3, 0] = "@Description";
                param[3, 1] = info.Description;
                param[4, 0] = "@ListId";
                param[4, 1] = info.ListId;
                param[5, 0] = "@ParentId";
                param[5, 1] = info.ParentId;
                ErrorInfo error = new ErrorInfo();
                object result = this.DataConnection.ExecuteScalar("freb_ListItem_Update", param, QueryType.StoredProcedure, error);
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
                error.Message = "ListInfo object is null";
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
            this.DataConnection.ExecuteScalar("freb_ListItem_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override ListItemInfo Select(int id, ErrorInfoList errors)
        {
            ErrorInfo error = new ErrorInfo();
            object[,] param = new object[1, 3];
            param[0, 0] = "@Id";
            param[0, 1] = id;
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("freb_ListItem_SelectById", param, QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                ListItemInfo listInfo = new ListItemInfo(dataTable.Rows[0]);
                return listInfo;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<ListItemInfo> SelectAll(ErrorInfoList errors)
        {
            return null;
        }

        public List<ListItemInfo> SelectAll(int listId, ErrorInfoList errors)
        {
            object[,] param = new object[1, 3];
            param[0, 0] = "@ListId";
            param[0, 1] = listId;
            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_ListItem_SelectAllListId", param, QueryType.StoredProcedure, error);
            List<ListItemInfo> result = new List<ListItemInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new ListItemInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<ListItemInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override void RegisterObjectToCache(ListItemInfo info)
        {
            throw new NotImplementedException();
        }

        public override void DeleteObjectFromCache(ListItemInfo info)
        {
            throw new NotImplementedException();
        }

        public override ListItemInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override ListItemInfo GetObjectFromCache(string name)
        {
            throw new NotImplementedException();
        }

        public List<ListItemInfo> SelectPagingSortingByListId(int pageSize, int pageIndex, string sortBy, string sortOrder, int listId, ErrorInfoList errors)
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
            param[4, 0] = "@ListId";
            param[4, 1] = listId;
            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_ListItem_SelectByPagingSortingList", param, QueryType.StoredProcedure, error);
            List<ListItemInfo> result = new List<ListItemInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new ListItemInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public List<ListItemInfo> SelectAllByListId(int listId, ErrorInfoList errors)
        {
            object[,] param = new object[1, 3];
            param[0, 0] = "@ListId";
            param[0, 1] = listId;
            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_ListItem_SelectAllListId", param, QueryType.StoredProcedure, error);
            List<ListItemInfo> result = new List<ListItemInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new ListItemInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public int SelectTotalCountByListId(int listid, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "@ListId";
            param[0, 1] = listid;

            object result = DataConnection.ExecuteScalar("[dbo].[freb_ListItem_TotalCountByListId]", param,
                                                         QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return ValidationHelper.GetInteger(result, 0);
            }
            RegisterError(errors, error);
            return 0;
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
