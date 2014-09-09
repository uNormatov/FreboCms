using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using FCore.Class;
using FCore.Collection;
using FCore.Constant;
using FCore.Enum;
using FCore.Helper;

namespace FDataProvider
{
    public class TransformationProvider : BaseProvider<TransformationInfo>
    {
        private static GoodDictionary<string, TransformationInfo> _collection;

        public TransformationProvider() : this(null)
        {
        }

        public TransformationProvider(DataConnection connection)
        {
            if (connection != null)
                DataConnection = new DataConnection(connection.ConnectionString);
            else
                DataConnection = new DataConnection();
            EnsureCreated();
        }

        public override object Create(TransformationInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[3,3];
                param[0, 0] = "@Name";
                param[0, 1] = info.Name;
                param[1, 0] = "@Text";
                param[1, 1] = info.Text;
                param[2, 0] = "@ContentTypeId";
                param[2, 1] = info.ContentTypeId;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_Transformation_Insert", param,
                                                             QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    info.Id = ValidationHelper.GetInteger(result, 0);
                    RegisterObjectToCache(info);
                    lock (this)
                    {
                        FireLayoutCache(info);
                    }
                    return result;
                }
                RegisterError(errors, error);
            }
            else
            {
                var error = new ErrorInfo();
                error.Ok = false;
                error.Date = DateTime.Now;
                error.Message = "TransformationInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(TransformationInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[4,3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@Text";
                param[2, 1] = info.Text;
                param[3, 0] = "@ContentTypeId";
                param[3, 1] = info.ContentTypeId;
                var error = new ErrorInfo();
                DataConnection.ExecuteScalar("freb_Transformation_Update", param, QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    RegisterObjectToCache(info);
                    lock (this)
                    {
                        FireLayoutCache(info);
                    }
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
                error.Message = "TransformationInfo object is null";
                RegisterError(errors, error);
            }

            return false;
        }

        public override bool Delete(int id, ErrorInfoList errors)
        {
            TransformationInfo info = Select(id, errors);

            var error = new ErrorInfo();
            var param = new object[1,3];
            param[0, 0] = "@Id";
            param[0, 1] = id;
            DataConnection.ExecuteNonQuery("freb_Transformation_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                DeleteObjectFromCache(info);
                lock (this)
                {
                    FireLayoutCache(info);
                }
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override TransformationInfo Select(int id, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1,3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Transformation_SelectById", param,
                                                                       QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                return new TransformationInfo(dataTable.Rows[0]);
            }

            RegisterError(errors, error);
            return null;
        }

        public override List<TransformationInfo> SelectAll(ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override List<TransformationInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy,
                                                                     string sortOrder, ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public override void RegisterObjectToCache(TransformationInfo info)
        {
            if (_collection.ContainsKey(info.Name))
                _collection[info.Name] = info;
            else
            {
                _collection.Add(info.Name, info);
            }
        }

        public override void DeleteObjectFromCache(TransformationInfo info)
        {
            if (_collection.ContainsKey(info.Name))
                _collection.Remove(info.Name);
        }

        public override TransformationInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override TransformationInfo GetObjectFromCache(string name)
        {
            if (_collection.ContainsKey(name))
                return _collection[name];
            return null;
        }

        private void FireLayoutCache(TransformationInfo info)
        {
            string realPath = HttpContext.Current.Server.MapPath(SiteConstants.TransformationCacheXmlPath);
            XDocument xDoc = XDocument.Load(realPath);
            XElement element = xDoc.Element("transformations");
            if (element != null)
            {
                if (element.Elements("transformation").Count() > 0)
                {
                    XElement template =
                        element.Elements("transformation").FirstOrDefault(
                            x => x.Attribute("id").Value == info.Id.ToString());
                    if (template != null && template.Attribute("updated") != null)
                    {
                        template.Attribute("updated").Value = DateTime.Now.ToString();
                    }
                    else
                    {
                        element.Add(new XElement("transformation", new XAttribute("id", info.Id),
                                                 new XAttribute("updated", DateTime.Now)));
                    }
                }
                else
                {
                    element.Add(new XElement("transformation", new XAttribute("id", info.Id),
                                             new XAttribute("updated", DateTime.Now)));
                }
                xDoc.Save(realPath);
            }
        }

        public List<TransformationInfo> SelectPagingSortingByContentTypeId(int pageSize, int pageIndex, string sortBy,
                                                                           string sortOrder, int contentTypeId,
                                                                           ErrorInfoList errors)
        {
            var param = new object[5,3];
            param[0, 0] = "@PageSize";
            param[0, 1] = pageSize;
            param[1, 0] = "@PageIndex";
            param[1, 1] = pageIndex;
            param[2, 0] = "@OrderBy";
            param[2, 1] = sortBy;
            param[3, 0] = "@SortOrder";
            param[3, 1] = sortOrder;
            param[4, 0] = "@ContentTypeId";
            param[4, 1] = contentTypeId;
            var error = new ErrorInfo();
            DataTable dataTable =
                DataConnection.ExecuteDataTableQuery("freb_Transformation_SelectByPagingSortingByContentTypeId", param,
                                                     QueryType.StoredProcedure, error);
            var result = new List<TransformationInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new TransformationInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public List<TransformationInfo> SelectPagingSortingByContentTypeIdKeyword(int pageSize, int pageIndex,
                                                                                  string sortBy, string sortOrder,
                                                                                  int contentTypeId, string keyword,
                                                                                  ErrorInfoList errors)
        {
            var param = new object[6,3];
            param[0, 0] = "@PageSize";
            param[0, 1] = pageSize;
            param[1, 0] = "@PageIndex";
            param[1, 1] = pageIndex;
            param[2, 0] = "@OrderBy";
            param[2, 1] = sortBy;
            param[3, 0] = "@SortOrder";
            param[3, 1] = sortOrder;
            param[4, 0] = "@ContentTypeId";
            param[4, 1] = contentTypeId;
            param[5, 0] = "@Keyword";
            param[5, 1] = keyword;
            var error = new ErrorInfo();
            DataTable dataTable =
                DataConnection.ExecuteDataTableQuery(
                    "[dbo].[freb_Transformation_SelectByPagingSortingByContentTypeIdKeyword]", param,
                    QueryType.StoredProcedure, error);
            var result = new List<TransformationInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new TransformationInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public int SelectTotalCountByContentTypeId(int contentTypeId, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1,3];
            param[0, 0] = "ContentTypeId";
            param[0, 1] = contentTypeId;

            object result = DataConnection.ExecuteScalar("[dbo].[freb_Transformation_TotalCount_ByContentType]", param,
                                                         QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return ValidationHelper.GetInteger(result, 0);
            }
            RegisterError(errors, error);
            return 0;
        }

        public int SelectTotalCountByContentTypeIdKeyword(int contentTypeId, string keyword, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[2,3];
            param[0, 0] = "@ContentTypeId";
            param[0, 1] = contentTypeId;
            param[1, 0] = "@Keyword";
            param[1, 1] = keyword;

            object result = DataConnection.ExecuteScalar("[dbo].[freb_Transformation_TotalCount_ByContentTypeKeyword]",
                                                         param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return ValidationHelper.GetInteger(result, 0);
            }
            RegisterError(errors, error);
            return 0;
        }

        public TransformationInfo SelectByName(string transformationName, ErrorInfoList errors)
        {
            TransformationInfo result = GetObjectFromCache(transformationName);

            if (result != null)
                return result;

            var param = new object[1,3];
            param[0, 0] = "@Name";
            param[0, 1] = transformationName;

            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("[dbo].[freb_Transformation_SelectByName]", param,
                                                                       QueryType.StoredProcedure, error);

            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                result = new TransformationInfo(dataTable.Rows[0]);
                RegisterObjectToCache(result);
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        private void EnsureCreated()
        {
            if (_collection == null)
                _collection = new GoodDictionary<string, TransformationInfo>();
        }
    }
}
