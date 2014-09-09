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
    public class BlockProvider : BaseProvider<BlockInfo>
    {
        public BlockProvider() : this(null) { }

        public BlockProvider(DataConnection dataConnection)
        {
            if (dataConnection != null)
                DataConnection = dataConnection;
            else
                DataConnection = new DataConnection();

            EnsureCreated();
        }

        public override object Create(BlockInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[4, 3];
                param[0, 0] = "@Name";
                param[0, 1] = info.Name;
                param[1, 0] = "@WebPartId";
                param[1, 1] = info.WebPartId;
                param[2, 0] = "@Properties";
                param[2, 1] = info.Properties;
                param[3, 0] = "@IsDeleted";
                param[3, 1] = false;
                ErrorInfo error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_Block_Insert", param, QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    info.Id = ValidationHelper.GetInteger(result, 0);
                    CacheHelper.ClearCaches();
                    return result;
                }
                RegisterError(errors, error);
            }
            else
            {
                ErrorInfo error = new ErrorInfo();
                error.Name = "Object refereance is null";
                error.Ok = false;
                error.Date = DateTime.Now;
                error.Message = "BlockInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(BlockInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[5, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@WebPartId";
                param[2, 1] = info.WebPartId;
                param[3, 0] = "@Properties";
                param[3, 1] = info.Properties;
                param[4, 0] = "@IsDeleted";
                param[4, 1] = info.IsDeleted;
                ErrorInfo error = new ErrorInfo();
                DataConnection.ExecuteScalar("freb_Block_Update", param, QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    CacheHelper.ClearCaches();
                    return true;
                }
                RegisterError(errors, error);
            }
            else
            {
                ErrorInfo error = new ErrorInfo();
                error.Name = "Object refereance is null";
                error.Ok = false;
                error.Date = DateTime.Now;
                error.Message = "BlockInfo object is null";
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
            DataConnection.ExecuteDataTableQuery("freb_Block_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                CacheHelper.ClearCaches();
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public List<BlockInfo> SelectByPage(int pageId, ErrorInfoList errors)
        {
            object[,] param = new object[1, 3];
            param[0, 0] = "PageId";
            param[0, 1] = pageId;

            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("freb_Block_SelectByPageId", param, QueryType.StoredProcedure, error);

            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                List<BlockInfo> blocks = new List<BlockInfo>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    BlockInfo block = new BlockInfo(dataTable.Rows[i]);
                    blocks.Add(block);
                }
                return blocks;
            }
            RegisterError(errors, error);
            return null;
        }

        public List<BlockInfo> SelectByLayout(int layoutId, ErrorInfoList errors)
        {
            object[,] param = new object[1, 3];
            param[0, 0] = "LayoutId";
            param[0, 1] = layoutId;

            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("freb_Block_SelectByLayoutId", param, QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                List<BlockInfo> blocks = new List<BlockInfo>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    BlockInfo block = new BlockInfo(dataTable.Rows[i]);
                    blocks.Add(block);
                }
                return blocks;
            }
            RegisterError(errors, error);
            return null;

        }

        public override BlockInfo Select(int id, ErrorInfoList errors)
        {
            ErrorInfo error = new ErrorInfo();
            object[,] param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataTable dataTable = this.DataConnection.ExecuteDataTableQuery("freb_Block_SelectById", param, QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                BlockInfo blockInfo = new BlockInfo(dataTable.Rows[0]);
                return blockInfo;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<BlockInfo> SelectAll(ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override List<BlockInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public List<BlockInfo> SelectPagingSortingByLayoutId(int pageSize, int pageIndex, string sortBy, string sortOrder, int layoutId, ErrorInfoList errors)
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
            param[4, 0] = "@LayoutId";
            param[4, 1] = layoutId;
            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Block_SelectByPagingortingLayout", param, QueryType.StoredProcedure, error);
            List<BlockInfo> result = new List<BlockInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new BlockInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public List<BlockInfo> SelectPagingSortingByPageId(int pageSize, int pageIndex, string sortBy, string sortOrder, int pageId, ErrorInfoList errors)
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
            param[4, 0] = "@PageId";
            param[4, 1] = pageId;
            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Block_SelectByPagingortingPage", param, QueryType.StoredProcedure, error);
            List<BlockInfo> result = new List<BlockInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new BlockInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override void RegisterObjectToCache(BlockInfo info)
        {

        }

        public override void DeleteObjectFromCache(BlockInfo info)
        {

        }

        public override BlockInfo GetObjectFromCache(int id)
        {
            return null;
        }

        public override BlockInfo GetObjectFromCache(string name)
        {
            throw new NotImplementedException();
        }

        private void EnsureCreated()
        {

        }
    }
}

