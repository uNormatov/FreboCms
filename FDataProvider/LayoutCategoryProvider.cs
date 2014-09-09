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
    public class LayoutCategoryProvider : BaseProvider<LayoutCategoryInfo>
    {
        public LayoutCategoryProvider() : this(null) { }

        public LayoutCategoryProvider(DataConnection connection)
        {
            this.DataConnection = connection ?? new DataConnection();
        }

        public override object Create(LayoutCategoryInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[2, 3];
                param[0, 0] = "@Name";
                param[0, 1] = info.Name;
                param[1, 0] = "@IsMaster";
                param[1, 1] = info.IsMaster;
                ErrorInfo error = new ErrorInfo();
                object result = this.DataConnection.ExecuteScalar("freb_LayoutCategory_Insert", param, QueryType.StoredProcedure, error);
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
                error.Message = "LayoutCategoryInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(LayoutCategoryInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[3, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@IsMaster";
                param[2, 1] = info.IsMaster;
                ErrorInfo error = new ErrorInfo();
                object result = this.DataConnection.ExecuteScalar("freb_LayoutCategory_Update", param, QueryType.StoredProcedure, error);
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
                error.Message = "LayoutCategoryInfo object is null";
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
            this.DataConnection.ExecuteScalar("freb_LayoutCategory_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override LayoutCategoryInfo Select(int id, ErrorInfoList errors)
        {
            ErrorInfo error = new ErrorInfo();
            object[,] param = new object[1, 3];
            param[0, 0] = "@Id";
            param[0, 1] = id;
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("freb_LayoutCategory_SelectById", param, QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                LayoutCategoryInfo layoutCategory = new LayoutCategoryInfo(dataTable.Rows[0]);
                return layoutCategory;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<LayoutCategoryInfo> SelectAll(ErrorInfoList errors)
        {
            return null;
        }

        public List<LayoutCategoryInfo> SelectAllByType(bool isMaster, ErrorInfoList errors)
        {
            object[,] param = new object[1, 3];
            param[0, 0] = "@IsMaster";
            param[0, 1] = isMaster;

            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("freb_LayoutCategory_SelectAllByType", param, QueryType.StoredProcedure, error);
            List<LayoutCategoryInfo> result = new List<LayoutCategoryInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new LayoutCategoryInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<LayoutCategoryInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
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
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("freb_LayoutCategory_SelectByPaging", param, QueryType.StoredProcedure, error);
            List<LayoutCategoryInfo> result = new List<LayoutCategoryInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new LayoutCategoryInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override void RegisterObjectToCache(LayoutCategoryInfo info)
        {
            throw new NotImplementedException();
        }

        public override void DeleteObjectFromCache(LayoutCategoryInfo info)
        {
            throw new NotImplementedException();
        }

        public override LayoutCategoryInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override LayoutCategoryInfo GetObjectFromCache(string name)
        {
            throw new NotImplementedException();
        }

        public List<LayoutCategoryInfo> SelectPagingSortingByType(int pageSize, int pageIndex, string sortBy, string sortOrder, bool isMaster, ErrorInfoList errors)
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
            param[4, 0] = "@IsMaster";
            param[4, 1] = isMaster;
            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("freb_LayoutCategory_SelectByPagingSortingType", param, QueryType.StoredProcedure, error);
            List<LayoutCategoryInfo> result = new List<LayoutCategoryInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new LayoutCategoryInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
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
