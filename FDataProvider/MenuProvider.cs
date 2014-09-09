using System;
using System.Collections.Generic;
using System.Data;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;

namespace FDataProvider
{
    public class MenuProvider : BaseProvider<MenuInfo>
    {
        public MenuProvider()
            : this(null)
        {
        }

        public MenuProvider(DataConnection connection)
        {
            if (connection != null)
                DataConnection = new DataConnection(connection.ConnectionString);
            else
                DataConnection = new DataConnection();
            EnsureCreated();
        }

        public override object Create(MenuInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[4, 3];
                param[0, 0] = "@Name";
                param[0, 1] = info.Name;
                param[1, 0] = "@Description";
                param[1, 1] = info.Description;
                param[2, 0] = "@IsMain";
                param[2, 1] = info.IsMain;
                param[3, 0] = "@IsDeleted";
                param[3, 1] = info.IsDeleted;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_Menu_Insert", param, QueryType.StoredProcedure, error);
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
                error.Message = "MenuInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(MenuInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[5, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@Description";
                param[2, 1] = info.Description;
                param[3, 0] = "@IsMain";
                param[3, 1] = info.IsMain;
                param[4, 0] = "@IsDeleted";
                param[4, 1] = info.IsDeleted;

                var error = new ErrorInfo();
                DataConnection.ExecuteScalar("freb_Menu_Update", param, QueryType.StoredProcedure, error);
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
                error.Message = "MenuInfo object is null";
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
            DataConnection.ExecuteDataTableQuery("freb_Menu_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override MenuInfo Select(int id, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Menu_SelectById", param,
                                                                       QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                return new MenuInfo(dataTable.Rows[0]);
            }

            RegisterError(errors, error);
            return null;
        }

        public override List<MenuInfo> SelectAll(ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Menu_SelectAll", null, QueryType.StoredProcedure, error);
            var result = new List<MenuInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new MenuInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<MenuInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder,
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
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Menu_SelectByPagingSorting", param, QueryType.StoredProcedure, error);
            var result = new List<MenuInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new MenuInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public int SelectTotalCount(ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            object result = DataConnection.ExecuteScalar("[dbo].[freb_Menu_TotalCount]", null, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return ValidationHelper.GetInteger(result, 0);
            }
            RegisterError(errors, error);
            return 0;
        }

        public object CreateMenuItem(MenuItemInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[7, 3];
                param[0, 0] = "@Title";
                param[0, 1] = info.Title;
                param[1, 0] = "@Url";
                param[1, 1] = info.Url;
                param[2, 0] = "@ParentId";
                param[2, 1] = info.ParentId;
                param[3, 0] = "@OpenType";
                param[3, 1] = info.OpenType;
                param[4, 0] = "@IsPublished";
                param[4, 1] = info.IsPublished;
                param[5, 0] = "@IsDeleted";
                param[5, 1] = info.IsDeleted;
                param[6, 0] = "@MenuId";
                param[6, 1] = info.MenuId;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_MenuItem_Insert", param, QueryType.StoredProcedure, error);
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
                error.Message = "MenuInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public bool UpdateMenuItem(MenuItemInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[8, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Title";
                param[1, 1] = info.Title;
                param[2, 0] = "@Url";
                param[2, 1] = info.Url;
                param[3, 0] = "@ParentId";
                param[3, 1] = info.ParentId;
                param[4, 0] = "@OpenType";
                param[4, 1] = info.OpenType;
                param[5, 0] = "@IsPublished";
                param[5, 1] = info.IsPublished;
                param[6, 0] = "@IsDeleted";
                param[6, 1] = info.IsDeleted;
                param[7, 0] = "@MenuId";
                param[7, 1] = info.MenuId;

                var error = new ErrorInfo();
                DataConnection.ExecuteScalar("freb_MenuItem_Update", param, QueryType.StoredProcedure, error);
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
                error.Message = "MenuItemInfo object is null";
                RegisterError(errors, error);
            }

            return false;
        }

        public bool DeleteMenuItem(int id, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataConnection.ExecuteDataTableQuery("freb_MenuItem_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public MenuItemInfo SelectMenuItem(int id, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_MenuItem_SelectById", param,
                                                                       QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                return new MenuItemInfo(dataTable.Rows[0]);
            }

            RegisterError(errors, error);
            return null;
        }

        public List<MenuItemInfo> SelectMenuItemsPagingSortingByMenuId(int pageSize, int pageIndex, string sortBy, string sortOrder, int menuId, ErrorInfoList errors)
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
            param[4, 0] = "@MenuId";
            param[4, 1] = menuId;
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_MenuItem_SelectByPagingSortingMenuId", param, QueryType.StoredProcedure, error);
            var result = new List<MenuItemInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new MenuItemInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public List<MenuItemInfo> SelectMenuItemsByMenuId(int menuId, ErrorInfoList errors)
        {
            var param = new object[1, 3];
            param[0, 0] = "@MenuId";
            param[0, 1] = menuId;
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_MenuItems_SelectByMenuId", param, QueryType.StoredProcedure, error);
            var result = new List<MenuItemInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new MenuItemInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public List<MenuItemInfo> SelectMenuItemPagingSortingByMenuIdByRoleId(int menuId, string roleId, ErrorInfoList errors)
        {
            var param = new object[2, 3];
            param[0, 0] = "@MenuId";
            param[0, 1] = menuId;
            param[1, 0] = "@RoleId";
            param[1, 1] = roleId;
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_MenuItems_SelectByMenuIdRoleId", param, QueryType.StoredProcedure, error);
            var result = new List<MenuItemInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new MenuItemInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public List<MenuItemInfo> SelectMenuItemsByMenuIdRoles(int menuId, string roles, ErrorInfoList errors)
        {
            string queryText = @" SELECT mi.* FROM MenuItem mi LEFT JOIN MenuItemsInRoles mr on mi.Id = mr.MenuItemId
                                 WHERE mi.MenuId= " + menuId + " and (mr.RoleId in (" + roles + ")) order by mi.ParentId;";
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery(queryText, null, QueryType.SqlQuery, error);
            var result = new List<MenuItemInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new MenuItemInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public int SelectMenuItemTotalCount(int menuId, ErrorInfoList errors)
        {
            var param = new object[1, 3];
            param[0, 0] = "@MenuId";
            param[0, 1] = menuId;
            var error = new ErrorInfo();
            object result = DataConnection.ExecuteScalar("[dbo].[freb_MenuItem_TotalCount]", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return ValidationHelper.GetInteger(result, 0);
            }
            RegisterError(errors, error);
            return 0;
        }

        public object CreateMenuItemsInRoles(MenuItemsInRolesInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[2, 3];
                param[0, 0] = "@MenuItemId";
                param[0, 1] = info.MenuItemId;
                param[1, 0] = "@RoleId";
                param[1, 1] = info.RoleId;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_MenuItemsInRoles_Insert", param, QueryType.StoredProcedure, error);
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
                error.Message = "MenuItemsInRoles object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public bool DeleteMenuItemsInRoles(int menuItemId, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "@MenuItemId";
            param[0, 1] = menuItemId;
            DataConnection.ExecuteDataTableQuery("freb_MenusItemsInRoles_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public List<MenuItemsInRolesInfo> SelectMenuItemsInRolesByMenuItemId(int menuItemId, ErrorInfoList errors)
        {
            var param = new object[1, 3];
            param[0, 0] = "@MenuItemId";
            param[0, 1] = menuItemId;
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_MenuItemsInRoles_SelectByMenuItemId", param, QueryType.StoredProcedure, error);
            var result = new List<MenuItemsInRolesInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new MenuItemsInRolesInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override void RegisterObjectToCache(MenuInfo info)
        {

        }

        public override void DeleteObjectFromCache(MenuInfo info)
        {

        }

        public override MenuInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override MenuInfo GetObjectFromCache(string name)
        {

            return null;
        }

        private void EnsureCreated()
        {

        }
    }
}
