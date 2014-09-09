using System;
using System.Collections.Generic;
using System.Data;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;

namespace FDataProvider
{
    public class PageProvider : BaseProvider<PageInfo>
    {
        private readonly BlockProvider _blockProvider;
        private readonly LayoutProvider _layoutProvider;

        public PageProvider()
            : this(null)
        {
        }

        public PageProvider(DataConnection connection)
        {
            if (connection != null)
                DataConnection = connection;
            else
                DataConnection = new DataConnection();
            _blockProvider = new BlockProvider(DataConnection);
            _layoutProvider = new LayoutProvider(DataConnection);
            EnsureCreated();
        }

        public override object Create(PageInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[19, 3];
                param[0, 0] = "@Name";
                param[0, 1] = info.Name;
                param[1, 0] = "@Title";
                param[1, 1] = info.Title;
                param[2, 0] = "@Description";
                param[2, 1] = info.Description;
                param[3, 0] = "@ParentId";
                param[3, 1] = info.ParentId;
                param[4, 0] = "@SeoTemplate";
                param[4, 1] = info.SeoTemplate;
                param[5, 0] = "@BreadCrumbTitle";
                param[5, 1] = info.BreadCrumbTitle;
                param[6, 0] = "@PageLayoutId";
                param[6, 1] = info.PageLayoutId;
                param[7, 0] = "@SiteLayoutId";
                param[7, 1] = info.SiteLayoutId;
                param[8, 0] = "@IsRequiresAuthentication";
                param[8, 1] = info.IsRequiresAuthentication;
                param[9, 0] = "@RedirectNoAuthenticated";
                param[9, 1] = info.RedirectNoAuthenticated;
                param[10, 0] = "@RedirectNoPermission";
                param[10, 1] = info.RedirectNoPermission;
                param[11, 0] = "@MetadataDescription";
                param[11, 1] = info.MetadataDescription;
                param[12, 0] = "@MetadataKeywords";
                param[12, 1] = info.MetadataKeywords;
                param[13, 0] = "@MetaQueryName";
                param[13, 1] = info.MetaQueryName;
                param[14, 0] = "@MetaQueryParameters";
                param[14, 1] = info.MetaQueryParameters;
                param[15, 0] = "@ContentRights";
                param[15, 1] = info.ContentRights;
                param[16, 0] = "@IsStatic";
                param[16, 1] = info.IsStatic;
                param[17, 0] = "@IsDeleted";
                param[17, 1] = false;
                param[18, 0] = "@IsPublished";
                param[18, 1] = true;

                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_Page_Insert", param, QueryType.StoredProcedure, error);
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
                error.Message = "PageInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(PageInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[20, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@Title";
                param[2, 1] = info.Title;
                param[3, 0] = "@Description";
                param[3, 1] = info.Description;
                param[4, 0] = "@ParentId";
                param[4, 1] = info.ParentId;
                param[5, 0] = "@SeoTemplate";
                param[5, 1] = info.SeoTemplate;
                param[6, 0] = "@BreadCrumbTitle";
                param[6, 1] = info.BreadCrumbTitle;
                param[7, 0] = "@PageLayoutId";
                param[7, 1] = info.PageLayoutId;
                param[8, 0] = "@SiteLayoutId";
                param[8, 1] = info.SiteLayoutId;
                param[9, 0] = "@IsRequiresAuthentication";
                param[9, 1] = info.IsRequiresAuthentication;
                param[10, 0] = "@RedirectNoAuthenticated";
                param[10, 1] = info.RedirectNoAuthenticated;
                param[11, 0] = "@RedirectNoPermission";
                param[11, 1] = info.RedirectNoPermission;
                param[12, 0] = "@MetadataDescription";
                param[12, 1] = info.MetadataDescription;
                param[13, 0] = "@MetadataKeywords";
                param[13, 1] = info.MetadataKeywords;
                param[14, 0] = "@MetaQueryName";
                param[14, 1] = info.MetaQueryName;
                param[15, 0] = "@MetaQueryParameters";
                param[15, 1] = info.MetaQueryParameters;
                param[16, 0] = "@ContentRights";
                param[16, 1] = info.ContentRights;
                param[17, 0] = "@IsStatic";
                param[17, 1] = info.IsStatic;
                param[18, 0] = "@IsDeleted";
                param[18, 1] = false;
                param[19, 0] = "@IsPublished";
                param[19, 1] = true;
                var error = new ErrorInfo();
                DataConnection.ExecuteScalar("freb_Page_Update", param, QueryType.StoredProcedure, error);
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
            PageInfo page = Select(id, errors);
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataConnection.ExecuteDataTableQuery("freb_Page_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override PageInfo Select(int id, ErrorInfoList errors)
        {
            PageInfo result = GetObjectFromCache(id);
            if (result != null)
                return result;

            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Page_SelectById", param, QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                result = new PageInfo(dataTable.Rows[0]);
                result.PageBlocks = _blockProvider.SelectByPage(result.Id, errors);
                result.PageLayoutBlocks = _blockProvider.SelectByLayout(result.PageLayoutId, errors);
                result.SiteBlocks = _blockProvider.SelectByLayout(result.SiteLayoutId, errors);
                result.AvailableRoles = SelectPagesInRolesByPageId(result.Id, errors);
                RegisterObjectToCache(result);
                return result;
            }

            RegisterError(errors, error);
            return null;
        }

        public PageInfo SelectBySeo(string seo, ErrorInfoList errors)
        {
            var log = new ErrorInfo();
            string tempSeo = seo;
            if (tempSeo.EndsWith("/"))
                tempSeo = tempSeo.Substring(0, tempSeo.Length - 2);

            string[] tokens = tempSeo.Split('/');

            //List<PageInfo> pages = GetPagesFromCache("/" + tokens[1]);

            //if (pages.Count > 0)
            //{
            //    int max = 0;
            //    int index = -1;
            //    for (int i = 0; i < pages.Count; i++)
            //    {
            //        string[] seoTempTokens = pages[i].SeoTemplate.Split('/');

            //        if (seoTempTokens.Length != tokens.Length)
            //            break;
            //        int tempMax = 0;
            //        for (int j = 1; j < seoTempTokens.Length; j++)
            //        {
            //            if (tokens[j] == seoTempTokens[j])
            //                tempMax++;
            //            else
            //                break;
            //        }
            //        if (tempMax >= max)
            //        {
            //            max = tempMax;
            //            index = i;
            //        }
            //    }
            //    if (index != -1 && index < pages.Count)
            //        return pages[index];
            //}            //List<PageInfo> pages = GetPagesFromCache("/" + tokens[1]);

            //if (pages.Count > 0)
            //{
            //    int max = 0;
            //    int index = -1;
            //    for (int i = 0; i < pages.Count; i++)
            //    {
            //        string[] seoTempTokens = pages[i].SeoTemplate.Split('/');

            //        if (seoTempTokens.Length != tokens.Length)
            //            break;
            //        int tempMax = 0;
            //        for (int j = 1; j < seoTempTokens.Length; j++)
            //        {
            //            if (tokens[j] == seoTempTokens[j])
            //                tempMax++;
            //            else
            //                break;
            //        }
            //        if (tempMax >= max)
            //        {
            //            max = tempMax;
            //            index = i;
            //        }
            //    }
            //    if (index != -1 && index < pages.Count)
            //        return pages[index];
            //}

            PageInfo page = null;
            var param = new object[1, 3];
            param[0, 0] = "SeoTemplate";
            param[0, 1] = "/" + tokens[1];
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Page_SelectBySeoTemplate", param, QueryType.StoredProcedure, log);

            if (log.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                int max = 0;
                int index = 0;
                if (dataTable.Rows.Count > 1)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        int tempMax = 0;
                        string[] seoTempTokens =
                            ValidationHelper.GetString(dataTable.Rows[i]["SeoTemplate"], "").Split('/');
                        for (int j = 1; j < seoTempTokens.Length; j++)
                        {
                            if (tokens.Length != seoTempTokens.Length)
                                continue;
                            if (tokens[j] == seoTempTokens[j])
                                tempMax++;
                            else
                                break;
                        }
                        if (tempMax > max)
                        {
                            max = tempMax;
                            index = i;
                        }
                    }
                }

                page = new PageInfo(dataTable.Rows[index]);
                page.PageBlocks = _blockProvider.SelectByPage(page.Id, errors);
                page.PageLayoutBlocks = _blockProvider.SelectByLayout(page.PageLayoutId, errors);
                page.SiteBlocks = _blockProvider.SelectByLayout(page.SiteLayoutId, errors);
                page.AvailableRoles = SelectPagesInRolesByPageId(page.Id, errors);
                RegisterObjectToCache(page);
            }
            RegisterError(errors, log);
            return page;
        }

        public int SelectTotalCount(ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            object result = DataConnection.ExecuteScalar("[dbo].[freb_Page_TotalCount]", null, QueryType.StoredProcedure,
                                                         error);
            if (error.Ok)
            {
                return ValidationHelper.GetInteger(result, 0);
            }
            RegisterError(errors, error);
            return 0;
        }

        public override List<PageInfo> SelectAll(ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Page_SelectAll", null,
                                                                       QueryType.StoredProcedure, error);
            var result = new List<PageInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new PageInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<PageInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
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
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Page_SelectByPagingSorting", param,
                                                                       QueryType.StoredProcedure, error);
            var result = new List<PageInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new PageInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public List<string> SelectPagesInRolesByPageId(int pageId, ErrorInfoList errors)
        {
            var param = new object[1, 3];
            param[0, 0] = "@PageId";
            param[0, 1] = pageId;
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_PagesInRoles_SelectByPageId", param, QueryType.StoredProcedure, error);
            var result = new List<string>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (dataTable.Rows[0].Table.Columns.Contains("RoleId"))
                        result.Add(ValidationHelper.GetString(dataTable.Rows[i]["RoleId"], string.Empty));

                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public object CreatePagesInRoles(PagesInRolesInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[2, 3];
                param[0, 0] = "@PageId";
                param[0, 1] = info.PageId;
                param[1, 0] = "@RoleId";
                param[1, 1] = info.RoleId;

                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_PagesInRoles_Insert", param, QueryType.StoredProcedure, error);
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
                error.Message = "PagesInRolesInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public bool DeletePagesInRoles(int pageId, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "PageId";
            param[0, 1] = pageId;
            DataConnection.ExecuteDataTableQuery("freb_PagesInRoles_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override void RegisterObjectToCache(PageInfo info)
        {
            CacheHelper.AddPageToCache(info);
        }

        public override void DeleteObjectFromCache(PageInfo info)
        {
            CacheHelper.DeletePageFromCache(info);
        }

        public override PageInfo GetObjectFromCache(int id)
        {
            return CacheHelper.GetPageFromCache(id);
        }

        public override PageInfo GetObjectFromCache(string name)
        {
            return CacheHelper.GetPageFromCache(name);
        }

        private List<PageInfo> GetPagesFromCache(string urlPattern)
        {
            return CacheHelper.GetPagesFromCache(urlPattern);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                if (_blockProvider != null)
                    _blockProvider.Dispose();

                if (_layoutProvider != null)
                    _layoutProvider.Dispose();
                disposing = true;
            }
        }

        private void EnsureCreated()
        {
        }
    }
}
