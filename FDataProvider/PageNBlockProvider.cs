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
    public class PageNBlockProvider : BaseProvider<PageNBlockInfo>
    {
        public PageNBlockProvider() : this(null) { }

        public PageNBlockProvider(DataConnection dataConnection)
        {
            if (Connection != null)
                DataConnection = dataConnection;
            else
                DataConnection = new DataConnection();
        }

        public override object Create(PageNBlockInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[5, 3];
                param[0, 0] = "@PageId";
                param[0, 1] = info.PageId;
                param[1, 0] = "@WebPartZoneName";
                param[1, 1] = info.WebPartZoneName;
                param[2, 0] = "@BlockId";
                param[2, 1] = info.BlockId;
                param[3, 0] = "@Order";
                param[3, 1] = info.Order;
                param[4, 0] = "@Language";
                param[4, 1] = info.Language;
                ErrorInfo error = new ErrorInfo();
                object result = this.DataConnection.ExecuteScalar("freb_PageNBlock_Insert", param, QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    info.Id = ValidationHelper.GetInteger(result, 0);
                    return result;
                }
                RegisterError(errors, error);
            }
            else
            {
                ErrorInfo error = new ErrorInfo();
                error.Ok = false;
                error.Name = "Object is null";
                error.Date = DateTime.Now;
                error.Message = "LayoutInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(PageNBlockInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[6, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@PageId";
                param[1, 1] = info.PageId;
                param[2, 0] = "@WebPartZoneName";
                param[2, 1] = info.WebPartZoneName;
                param[3, 0] = "@BlockId";
                param[3, 1] = info.BlockId;
                param[4, 0] = "@Order";
                param[4, 1] = info.Order;
                param[5, 0] = "@Language";
                param[5, 1] = info.Language;
                ErrorInfo error = new ErrorInfo();
                this.DataConnection.ExecuteScalar("freb_PageNBlock_Update", param, QueryType.StoredProcedure, error);
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
                error.Name = "Object is null";
                error.Date = DateTime.Now;
                error.Message = "LayoutInfo object is null";
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
            DataConnection.ExecuteDataTableQuery("freb_PageNBlock_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                // DeleteObjectFromCache(id);
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override PageNBlockInfo Select(int id, ErrorInfoList errors)
        {
            ErrorInfo error = new ErrorInfo();
            object[,] param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("freb_PageNBlock_SelectById", param, QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                return new PageNBlockInfo(dataTable.Rows[0]);
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<PageNBlockInfo> SelectAll(ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override List<PageNBlockInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override void RegisterObjectToCache(PageNBlockInfo info)
        {
            throw new NotImplementedException();
        }

        public override void DeleteObjectFromCache(PageNBlockInfo info)
        {
            throw new NotImplementedException();
        }

        public override PageNBlockInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override PageNBlockInfo GetObjectFromCache(string name)
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
