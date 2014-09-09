using System;
using System.Collections.Generic;
using System.Data;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;

namespace FDataProvider
{
    public class RoleProfileProvider : BaseProvider<RoleProfileInfo>
    {
        public RoleProfileProvider()
            : this(null)
        {
        }

        public RoleProfileProvider(DataConnection connection)
        {
            DataConnection = connection ?? new DataConnection();
        }

        public override object Create(RoleProfileInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[3, 3];
                param[0, 0] = "@RoleId";
                param[0, 1] = info.RoleId;
                param[1, 0] = "@ContentTypeId";
                param[1, 1] = info.ContentTypeId;
                param[2, 0] = "@UserProfileQuery";
                param[2, 1] = info.UserProfileQuery;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_RoleProfile_Insert", param, QueryType.StoredProcedure, error);
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
                error.Message = "RoleProofile object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(RoleProfileInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[4, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@RoleId";
                param[1, 1] = info.RoleId;
                param[2, 0] = "@ContentTypeId";
                param[2, 1] = info.ContentTypeId;
                param[3, 0] = "@UserProfileQuery";
                param[3, 1] = info.UserProfileQuery;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_RoleProfile_Update", param, QueryType.StoredProcedure, error);
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
                error.Message = "RoleProfileInfo object is null";
                RegisterError(errors, error);
            }

            return false;
        }

        public override bool Delete(int id, ErrorInfoList errors)
        {
            return false;
        }

        public override RoleProfileInfo Select(int id, ErrorInfoList errors)
        {
            return null;
        }

        public override List<RoleProfileInfo> SelectAll(ErrorInfoList errors)
        {
            return null;
        }

        public override List<RoleProfileInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder,
                                                           ErrorInfoList errors)
        {
            return null;
        }

        public RoleProfileInfo SelectByRoleId(string roleId, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "@RoleId";
            param[0, 1] = roleId;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_RoleProfile_SelectByRoleId", param, QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                return new RoleProfileInfo(dataTable.Rows[0]);
            }

            RegisterError(errors, error);
            return null;
        }

        public bool DeleteRoleProfile(string roleId, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "@RoleId";
            param[0, 1] = roleId;
            DataConnection.ExecuteDataTableQuery("freb_RoleProfile_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override void RegisterObjectToCache(RoleProfileInfo info)
        {
            throw new NotImplementedException();
        }

        public override void DeleteObjectFromCache(RoleProfileInfo info)
        {
            throw new NotImplementedException();
        }

        public override RoleProfileInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override RoleProfileInfo GetObjectFromCache(string name)
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
