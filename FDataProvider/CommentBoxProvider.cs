using System;
using System.Collections.Generic;
using System.Data;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;

namespace FDataProvider
{
    public class CommentBoxProvider : BaseProvider<CommentBoxInfo>
    {
        public CommentBoxProvider() : this(null) { }

        public CommentBoxProvider(DataConnection dataConnection)
        {
            if (dataConnection != null)
                DataConnection = dataConnection;
            else
                DataConnection = new DataConnection();


        }

        public override object Create(CommentBoxInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[10, 3];
                param[0, 0] = "@Email";
                param[0, 1] = info.Email;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@Website";
                param[2, 1] = info.Website;
                param[3, 0] = "@Body";
                param[3, 1] = info.Body;
                param[4, 0] = "@ContentTypeId";
                param[4, 1] = info.ContentTypeId;
                param[5, 0] = "@SeoTemplate";
                param[5, 1] = info.SeoTemplate;
                param[6, 0] = "@Url";
                param[6, 1] = info.Url;
                param[7, 0] = "@Order";
                param[7, 1] = info.Order;
                param[8, 0] = "@ParentId";
                param[8, 1] = info.ParentId;
                param[9, 0] = "@CreatedDate";
                param[9, 1] = info.CreatedDate;
                ErrorInfo error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_CommentBox_Insert", param, QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    info.Id = ValidationHelper.GetInteger(result, 0);
                    return result;
                }
            }
            else
            {
                ErrorInfo error = new ErrorInfo();
                error.Ok = false;
                error.Date = DateTime.Now;
                error.Message = "CommentBoxInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(CommentBoxInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                object[,] param = new object[11, 3];
                param[0, 0] = "@Email";
                param[0, 1] = info.Email;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@Website";
                param[2, 1] = info.Website;
                param[3, 0] = "@Body";
                param[3, 1] = info.Body;
                param[4, 0] = "@ContentTypeId";
                param[4, 1] = info.ContentTypeId;
                param[5, 0] = "@SeoTemplate";
                param[5, 1] = info.SeoTemplate;
                param[6, 0] = "@Url";
                param[6, 1] = info.Url;
                param[7, 0] = "@Order";
                param[7, 1] = info.Order;
                param[8, 0] = "@ParentId";
                param[8, 1] = info.ParentId;
                param[9, 0] = "@CreatedDate";
                param[9, 1] = info.CreatedDate;
                ErrorInfo error = new ErrorInfo();
                this.DataConnection.ExecuteScalar("freb_CommentBox_Update", param, QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    RegisterObjectToCache(info);
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
            DataConnection.ExecuteDataTableQuery("freb_CommentBox_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            return false;
        }

        public override CommentBoxInfo Select(int id, ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override List<CommentBoxInfo> SelectAll(ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override List<CommentBoxInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public List<CommentBoxInfo> SelectByUrl(string url, ErrorInfoList errors)
        {
            ErrorInfo error = new ErrorInfo();
            object[,] param = new object[1, 3];
            param[0, 0] = "Url";
            param[0, 1] = url;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_CommentBox_SelectByUrl", param, QueryType.StoredProcedure, error);
            List<CommentBoxInfo> result = new List<CommentBoxInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new CommentBoxInfo(dataTable.Rows[i]));
                }
                return result;
            }
            return null;
        }

        public override void RegisterObjectToCache(CommentBoxInfo info)
        {
            throw new NotImplementedException();
        }

        public override void DeleteObjectFromCache(CommentBoxInfo info)
        {
            throw new NotImplementedException();
        }

        public override CommentBoxInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override CommentBoxInfo GetObjectFromCache(string name)
        {
            throw new NotImplementedException();
        }
    }
}
