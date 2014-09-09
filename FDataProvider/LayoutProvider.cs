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
    public class LayoutProvider : BaseProvider<LayoutInfo>
    {
        private static GoodDictionary<int, LayoutInfo> _collection;
        private readonly BlockProvider _blockProvider;

        public LayoutProvider()
            : this(null)
        {
        }

        public LayoutProvider(DataConnection dataConnection)
        {
            if (Connection != null)
                DataConnection = dataConnection;
            else
                DataConnection = new DataConnection();
            _blockProvider = new BlockProvider(DataConnection);
            EnsureCreated();
        }

        public override object Create(LayoutInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[10, 3];
                param[0, 0] = "@Name";
                param[0, 1] = info.Name;
                param[1, 0] = "@Description";
                param[1, 1] = info.Description;
                param[2, 0] = "@Layout";
                param[2, 1] = info.Layout;
                param[3, 0] = "@Css";
                param[3, 1] = info.Css;
                param[4, 0] = "@Screenshot";
                param[4, 1] = info.Screenshot;
                param[5, 0] = "@BodyOption";
                param[5, 1] = info.BodyOption;
                param[6, 0] = "@DocOption";
                param[6, 1] = info.DocOption;
                param[7, 0] = "@IsMaster";
                param[7, 1] = info.IsMaster;
                param[8, 0] = "@IsDeleted";
                param[8, 1] = info.IsDeleted;
                param[9, 0] = "@LayoutCategoryId";
                param[9, 1] = info.LayoutCategoryId;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_Layout_Insert", param, QueryType.StoredProcedure,
                                                             error);
                if (error.Ok)
                {
                    info.Id = ValidationHelper.GetInteger(result, 0);
                    RegisterObjectToCache(info);
                    lock (this)
                    {
                        ClearLayoutCache(info);
                    }
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

        public override bool Update(LayoutInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[11, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@Description";
                param[2, 1] = info.Description;
                param[3, 0] = "@Layout";
                param[3, 1] = info.Layout;
                param[4, 0] = "@Css";
                param[4, 1] = info.Css;
                param[5, 0] = "@Screenshot";
                param[5, 1] = info.Screenshot;
                param[6, 0] = "@BodyOption";
                param[6, 1] = info.BodyOption;
                param[7, 0] = "@DocOption";
                param[7, 1] = info.DocOption;
                param[8, 0] = "@IsMaster";
                param[8, 1] = info.IsMaster;
                param[9, 0] = "@IsDeleted";
                param[9, 1] = info.IsDeleted;
                param[10, 0] = "@LayoutCategoryId";
                param[10, 1] = info.LayoutCategoryId;
                var error = new ErrorInfo();
                DataConnection.ExecuteScalar("freb_Layout_Update", param, QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    RegisterObjectToCache(info);
                    lock (this)
                    {
                        ClearLayoutCache(info);
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
                error.Message = "LayoutInfo object is null";
                RegisterError(errors, error);
            }

            return false;
        }

        public override bool Delete(int id, ErrorInfoList errors)
        {
            LayoutInfo result = Select(id, errors);
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataConnection.ExecuteNonQuery("freb_Layout_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                DeleteObjectFromCache(result);
                lock (this)
                {
                    ClearLayoutCache(result);
                }
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public override LayoutInfo Select(int id, ErrorInfoList errors)
        {
            LayoutInfo layoutInfo = GetObjectFromCache(id);
            if (layoutInfo != null)
                return layoutInfo;
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "Id";
            param[0, 1] = id;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Layout_SelectById", param,
                                                                       QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                var layout = new LayoutInfo(dataTable.Rows[0]);
                RegisterObjectToCache(layout);
                return layout;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<LayoutInfo> SelectAll(ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public List<LayoutInfo> SelectAllByType(bool isMaster, ErrorInfoList errors)
        {
            var param = new object[1, 3];
            param[0, 0] = "@IsMaster";
            param[0, 1] = isMaster;

            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Layout_SelectAllByType", param,
                                                                       QueryType.StoredProcedure, error);
            var result = new List<LayoutInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new LayoutInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<LayoutInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
        {
            throw new NotImplementedException();
        }

        public List<LayoutInfo> SelectPagingSortingByCategoryId(int pageSize, int pageIndex, string sortBy, string sortOrder, int categoryId, ErrorInfoList errors)
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
            param[4, 0] = "@CategoryId";
            param[4, 1] = categoryId;
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Layout_SelectByPaging", param,
                                                                       QueryType.StoredProcedure, error);
            var result = new List<LayoutInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new LayoutInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override void RegisterObjectToCache(LayoutInfo info)
        {
            if (!_collection.ContainsKey(info.Id))
                _collection.Add(info.Id, info);
            else
                _collection[info.Id] = info;
        }

        public override void DeleteObjectFromCache(LayoutInfo info)
        {
            if (_collection.ContainsKey(info.Id))
                _collection.Remove(info.Id);
        }

        public override LayoutInfo GetObjectFromCache(int id)
        {
            if (_collection.ContainsKey(id))
                return _collection[id];
            return null;
        }

        public override LayoutInfo GetObjectFromCache(string name)
        {
            throw new NotImplementedException();
        }

        private void ClearLayoutCache(LayoutInfo info)
        {
            string realPath = HttpContext.Current.Server.MapPath(SiteConstants.LayoutCacheXmlPath);
            XDocument xDoc = XDocument.Load(realPath);
            XElement element = xDoc.Element("layouts");
            if (element != null)
            {
                if (element.Elements("layout").Count() > 0)
                {
                    XElement template =
                        element.Elements("layout").FirstOrDefault(
                            x => x.Attribute("id").Value == info.Id.ToString());
                    if (template != null && template.Attribute("updated") != null)
                    {
                        template.Attribute("updated").Value = DateTime.Now.ToString();
                    }
                    else
                    {
                        element.Add(new XElement("layout", new XAttribute("id", info.Id),
                                                 new XAttribute("updated", DateTime.Now)));
                    }
                }
                else
                {
                    element.Add(new XElement("layout", new XAttribute("id", info.Id),
                                             new XAttribute("updated", DateTime.Now)));
                }
                xDoc.Save(realPath);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                if (_blockProvider != null)
                    _blockProvider.Dispose();
                disposing = true;
            }
        }

        private void EnsureCreated()
        {
            if (_collection == null)
                _collection = new GoodDictionary<int, LayoutInfo>();
        }
    }
}
