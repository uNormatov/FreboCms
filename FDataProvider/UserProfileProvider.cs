using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Security;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;

namespace FDataProvider
{
    public class UserProfileProvider : BaseProvider<UserProfileInfo>
    {
        private RoleProfileProvider _roleProfileProvider;
        public UserProfileProvider()
            : this(null)
        {
        }

        public UserProfileProvider(DataConnection connection)
        {
            DataConnection = connection ?? new DataConnection();
            _roleProfileProvider = new RoleProfileProvider();
        }

        public override object Create(UserProfileInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[3, 3];
                param[0, 0] = "@UserId";
                param[0, 1] = info.UserId;
                param[1, 0] = "@ContentTypeId";
                param[1, 1] = info.ContentTypeId;
                param[2, 0] = "@ContentId";
                param[2, 1] = info.ContentId;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_UserProfile_Insert", param, QueryType.StoredProcedure, error);
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
                error.Message = "UserProofile object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(UserProfileInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[4, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@UserId";
                param[1, 1] = info.UserId;
                param[2, 0] = "@ContentTypeId";
                param[2, 1] = info.ContentTypeId;
                param[3, 0] = "@ContentId";
                param[3, 1] = info.ContentId;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_UserProfile_Update", param, QueryType.StoredProcedure, error);
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
                error.Message = "UserProfileInfo object is null";
                RegisterError(errors, error);
            }

            return false;
        }

        public override bool Delete(int id, ErrorInfoList errors)
        {
            return false;
        }

        public override UserProfileInfo Select(int id, ErrorInfoList errors)
        {
            return null;
        }

        public override List<UserProfileInfo> SelectAll(ErrorInfoList errors)
        {
            return null;
        }

        public override List<UserProfileInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder,
                                                           ErrorInfoList errors)
        {
            return null;
        }

        public UserProfileInfo SelectByUserId(string userId, ErrorInfoList errors)
        {
            UserProfileInfo userProfileInfo = GetObjectFromCache(userId);
            if (userProfileInfo == null)
            {
                var error = new ErrorInfo();
                var param = new object[1, 3];
                param[0, 0] = "@UserId";
                param[0, 1] = userId;
                DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_UserProfile_SelectByUserId", param, QueryType.StoredProcedure, error);
                if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
                {
                    userProfileInfo = new UserProfileInfo(dataTable.Rows[0]);
                    string[] roleNames = Roles.GetRolesForUser(userProfileInfo.UserId);
                    if (roleNames.Length > 0)
                    {
                        RoleProfileInfo roleProfileInfo = _roleProfileProvider.SelectByRoleId(roleNames[0], errors);
                        if (roleProfileInfo != null)
                        {
                            dataTable = DataConnection.ExecuteDataTableQuery(roleProfileInfo.UserProfileQuery, param, QueryType.SqlQuery, error);
                            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
                            {
                                userProfileInfo.FillAttributes(dataTable.Rows[0]);
                                RegisterObjectToCache(userProfileInfo);
                                return userProfileInfo;
                            }
                            RegisterError(errors, error);
                        }
                    }
                }
                RegisterError(errors, error);
                return null;
            }

            return userProfileInfo;
        }

        public bool DeleteUserProfile(string userId, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "@UserId";
            param[0, 1] = userId;
            DataConnection.ExecuteDataTableQuery("freb_UserProfile_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override void RegisterObjectToCache(UserProfileInfo info)
        {
            CacheHelper.AddUserProfileToCache(info);
        }

        public override void DeleteObjectFromCache(UserProfileInfo info)
        {
            CacheHelper.DeleteUserProfileFromCache(info);
        }

        public override UserProfileInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override UserProfileInfo GetObjectFromCache(string name)
        {
            return CacheHelper.GetUserProfileFromCache(name);
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
