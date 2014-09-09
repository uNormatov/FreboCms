using System;
using System.Collections.Generic;
using System.Data;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;

namespace FDataProvider
{
    public class ArticleProvider : BaseProvider<ArticleInfo>
    {
        private PageNBlockProvider _pageNBlockProvider;
        private LayoutNBlockProvider _layoutNBlockProvider;
        public ArticleProvider()
            : this(null)
        {
        }

        public ArticleProvider(DataConnection connection)
        {
            if (connection != null)
                DataConnection = connection;
            else
                DataConnection = new DataConnection();
            EnsureCreated();
        }

        public override object Create(ArticleInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[21, 3];
                param[0, 0] = "@Title";
                param[0, 1] = info.Title;
                param[1, 0] = "@Code";
                param[1, 1] = info.Code;
                param[2, 0] = "@Text";
                param[2, 1] = info.Text;
                param[3, 0] = "@Language";
                param[3, 1] = info.Language;
                param[4, 0] = "@SiteLayoutId";
                param[4, 1] = info.SiteLayoutId;
                param[5, 0] = "@SiteLayoutZone";
                param[5, 1] = info.SiteLayoutZone;
                param[6, 0] = "@SiteLayoutNBlockId";
                param[6, 1] = info.SiteLayoutNBlockId;
                param[7, 0] = "@SiteLayoutOrder";
                param[7, 1] = info.SiteLayoutOrder;
                param[8, 0] = "@PageLayoutId";
                param[8, 1] = info.PageLayoutId;
                param[9, 0] = "@PageLayoutZone";
                param[9, 1] = info.PageLayoutZone;
                param[10, 0] = "@PageLayoutOrder";
                param[10, 1] = info.PageLayoutOrder;
                param[11, 0] = "@PageLayoutNBlockId";
                param[11, 1] = info.PageLayoutNBlockId;
                param[12, 0] = "@PageId";
                param[12, 1] = info.PageId;
                param[13, 0] = "@PageZone";
                param[13, 1] = info.PageZone;
                param[14, 0] = "@PageOrder";
                param[14, 1] = info.PageOrder;
                param[15, 0] = "@PageNBlockId";
                param[15, 1] = info.PageNBlockId;
                param[16, 0] = "@BlockId";
                param[16, 1] = info.BlockId;
                param[17, 0] = "@CreatedBy";
                param[17, 1] = info.CreatedBy;
                param[18, 0] = "@CreatedDate";
                param[18, 1] = info.CreatedDate;
                param[19, 0] = "@ModifiedBy";
                param[19, 1] = info.ModifiedBy;
                param[20, 0] = "@ModifiedDate";
                param[20, 1] = info.ModifiedDate;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_Article_Insert", param, QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    info.Id = ValidationHelper.GetInteger(result, 0);
                    RegisterObjectToCache(info);
                    return result;
                }
                RegisterError(errors, error);
            }
            else
            {
                var error = new ErrorInfo();
                error.Ok = false;
                error.Date = DateTime.Now;
                error.Message = "ArticleInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(ArticleInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[22, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Title";
                param[1, 1] = info.Title;
                param[2, 0] = "@Code";
                param[2, 1] = info.Code;
                param[3, 0] = "@Text";
                param[3, 1] = info.Text;
                param[4, 0] = "@Language";
                param[4, 1] = info.Language;
                param[5, 0] = "@SiteLayoutId";
                param[5, 1] = info.SiteLayoutId;
                param[6, 0] = "@SiteLayoutZone";
                param[6, 1] = info.SiteLayoutZone;
                param[7, 0] = "@SiteLayoutOrder";
                param[7, 1] = info.SiteLayoutOrder;
                param[8, 0] = "@SiteLayoutNBlockId";
                param[8, 1] = info.SiteLayoutNBlockId;
                param[9, 0] = "@PageLayoutId";
                param[9, 1] = info.PageLayoutId;
                param[10, 0] = "@PageLayoutZone";
                param[10, 1] = info.PageLayoutZone;
                param[11, 0] = "@PageLayoutOrder";
                param[11, 1] = info.PageLayoutOrder;
                param[12, 0] = "@PageLayoutNBlockId";
                param[12, 1] = info.PageLayoutNBlockId;
                param[13, 0] = "@PageId";
                param[13, 1] = info.PageId;
                param[14, 0] = "@PageZone";
                param[14, 1] = info.PageZone;
                param[15, 0] = "@PageOrder";
                param[15, 1] = info.PageOrder;
                param[16, 0] = "@PageNBlockId";
                param[16, 1] = info.PageNBlockId;
                param[17, 0] = "@BlockId";
                param[17, 1] = info.BlockId;
                param[18, 0] = "@CreatedBy";
                param[18, 1] = info.CreatedBy;
                param[19, 0] = "@CreatedDate";
                param[19, 1] = info.CreatedDate;
                param[20, 0] = "@ModifiedBy";
                param[20, 1] = info.ModifiedBy;
                param[21, 0] = "@ModifiedDate";
                param[21, 1] = info.ModifiedDate;
                var error = new ErrorInfo();
                DataConnection.ExecuteScalar("freb_Article_Update", param, QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    RegisterObjectToCache(info);
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
                error.Message = "Article object is null";
                RegisterError(errors, error);
            }

            return false;
        }

        public override bool Delete(int id, ErrorInfoList errors)
        {
            ArticleInfo info = Select(id, errors);
            if (info == null)
                return false;
            _pageNBlockProvider.Delete(info.PageNBlockId, errors);
            _layoutNBlockProvider.Delete(info.PageLayoutNBlockId, errors);
            _layoutNBlockProvider.Delete(info.SiteLayoutNBlockId, errors);

            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataConnection.ExecuteDataTableQuery("freb_Article_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                DeleteObjectFromCache(info);
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override ArticleInfo Select(int id, ErrorInfoList errors)
        {
            ArticleInfo result = GetObjectFromCache(id);
            if (result != null)
                return result;

            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Article_SelectById", param, QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                result = new ArticleInfo(dataTable.Rows[0]);
                RegisterObjectToCache(result);
                return result;
            }

            RegisterError(errors, error);
            return null;
        }

        public int SelectTotalCount(ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            object result = DataConnection.ExecuteScalar("[dbo].[freb_Article_TotalCount]", null, QueryType.StoredProcedure,
                                                         error);
            if (error.Ok)
            {
                return ValidationHelper.GetInteger(result, 0);
            }
            RegisterError(errors, error);
            return 0;
        }

        public override List<ArticleInfo> SelectAll(ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Article_SelectAll", null,
                                                                       QueryType.StoredProcedure, error);
            var result = new List<ArticleInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new ArticleInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<ArticleInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
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
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Article_SelectByPagingSorting", param, QueryType.StoredProcedure, error);
            var result = new List<ArticleInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new ArticleInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override void RegisterObjectToCache(ArticleInfo info)
        {
            //CacheHelper.Add();
        }

        public override void DeleteObjectFromCache(ArticleInfo info)
        {
            //CacheHelper.DeletePageFromCache(info);
        }

        public override ArticleInfo GetObjectFromCache(int id)
        {
            return null;
        }

        public override ArticleInfo GetObjectFromCache(string name)
        {
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
            if (_pageNBlockProvider == null)
                _pageNBlockProvider = new PageNBlockProvider();

            if (_layoutNBlockProvider == null)
                _layoutNBlockProvider = new LayoutNBlockProvider();

        }
    }
}
