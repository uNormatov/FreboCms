using System;
using System.Collections.Generic;
using System.Data;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;

namespace FDataProvider
{
    public class LayoutNBlockProvider : BaseProvider<LayoutNBlockInfo>
    {
        public LayoutNBlockProvider()
            : this(null)
        {
        }

        public LayoutNBlockProvider(DataConnection dataConnection)
        {
            if (Connection != null)
                DataConnection = dataConnection;
            else
                DataConnection = new DataConnection();
        }

        public override object Create(LayoutNBlockInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[5, 3];
                param[0, 0] = "@LayoutId";
                param[0, 1] = info.LayoutId;
                param[1, 0] = "@WebPartZoneName";
                param[1, 1] = info.WebPartZoneName;
                param[2, 0] = "@BlockId";
                param[2, 1] = info.BlockId;
                param[3, 0] = "@Order";
                param[3, 1] = info.Order;
                param[4, 0] = "@Language";
                param[4, 1] = info.Language;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_LayoutNBlock_Insert", param,
                                                             QueryType.StoredProcedure, error);
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
                error.Message = "LayoutInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(LayoutNBlockInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[6, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@LayoutId";
                param[1, 1] = info.LayoutId;
                param[2, 0] = "@WebPartZoneName";
                param[2, 1] = info.WebPartZoneName;
                param[3, 0] = "@BlockId";
                param[3, 1] = info.BlockId;
                param[4, 0] = "@Order";
                param[4, 1] = info.Order;
                param[5, 0] = "@Language";
                param[5, 1] = info.Language;
                var error = new ErrorInfo();
                DataConnection.ExecuteScalar("freb_LayoutNBlock_Update", param, QueryType.StoredProcedure, error);
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
                error.Message = "LayoutInfo object is null";
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
            DataConnection.ExecuteDataTableQuery("freb_LayoutNBlock_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                //   DeleteObjectFromCache(id);
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override LayoutNBlockInfo Select(int id, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_LayoutNBlock_SelectById", param,
                                                                       QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                return new LayoutNBlockInfo(dataTable.Rows[0]);
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<LayoutNBlockInfo> SelectAll(ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override List<LayoutNBlockInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override void RegisterObjectToCache(LayoutNBlockInfo info)
        {
            throw new NotImplementedException();
        }

        public override void DeleteObjectFromCache(LayoutNBlockInfo info)
        {
            throw new NotImplementedException();
        }

        public override LayoutNBlockInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override LayoutNBlockInfo GetObjectFromCache(string name)
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

