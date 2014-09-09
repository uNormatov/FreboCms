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
    public class LayoutWebPartZoneProvider : BaseProvider<LayoutWebPartZoneInfo>
    {

        public LayoutWebPartZoneProvider() : this(null) { }

        public LayoutWebPartZoneProvider(DataConnection dataConnection)
        {
            if (Connection != null)
                DataConnection = dataConnection;
            else
                DataConnection = new DataConnection();
            EnsureCreated();
        }

        public override object Create(LayoutWebPartZoneInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[2, 3];
                param[0, 0] = "@Name";
                param[0, 1] = info.Name;
                param[1, 0] = "@LayoutId";
                param[1, 1] = info.LayoutId;
                ErrorInfo error = new ErrorInfo();
                object result = this.DataConnection.ExecuteScalar("freb_LayoutWebPartZone_Insert", param, QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    info.Id = ValidationHelper.GetInteger(result, 1);
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
                error.Message = "LayoutWebPartZone object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(LayoutWebPartZoneInfo info, ErrorInfoList errors)
        {
            return false;
        }

        public override bool Delete(int id, ErrorInfoList errors)
        {
            ErrorInfo error = new ErrorInfo();
            object[,] param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataConnection.ExecuteDataTableQuery("freb_LayoutWebPartZone_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public bool DeleteByLayoutId(int layoutId, ErrorInfoList errors)
        {
            ErrorInfo error = new ErrorInfo();
            object[,] param = new object[1, 3];
            param[0, 0] = "LayoutId";
            param[0, 1] = layoutId;
            DataConnection.ExecuteDataTableQuery("freb_LayoutWebPartZone_DeleteByLayoutId", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override LayoutWebPartZoneInfo Select(int id, ErrorInfoList errors)
        {
            return null;
        }

        public override List<LayoutWebPartZoneInfo> SelectAll(ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override List<LayoutWebPartZoneInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override void RegisterObjectToCache(LayoutWebPartZoneInfo info)
        {
            throw new NotImplementedException();
        }

        public override void DeleteObjectFromCache(LayoutWebPartZoneInfo info)
        {
            throw new NotImplementedException();
        }

        public override LayoutWebPartZoneInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override LayoutWebPartZoneInfo GetObjectFromCache(string name)
        {
            throw new NotImplementedException();
        }

        public List<LayoutWebPartZoneInfo> SelectAllByLayoutId(int layoutId, ErrorInfoList errors)
        {
            object[,] param = new object[1, 3];
            param[0, 0] = "@LayoutId";
            param[0, 1] = layoutId;
            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_LayoutWebPartZone_SelectAllByLayoutId", param, QueryType.StoredProcedure, error);
            List<LayoutWebPartZoneInfo> result = new List<LayoutWebPartZoneInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new LayoutWebPartZoneInfo(dataTable.Rows[i]));
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
