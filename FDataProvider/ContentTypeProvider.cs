using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;

namespace FDataProvider
{
    public class ContentTypeProvider : BaseProvider<ContentTypeInfo>
    {
        private readonly QueryProvider _queryProvider;

        #region Constructor

        public ContentTypeProvider()
            : this(null)
        {
        }

        public ContentTypeProvider(DataConnection connection)
        {
            DataConnection = connection ?? new DataConnection();
            _queryProvider = new QueryProvider(DataConnection);

            EnsureCreated();
        }

        #endregion

        #region Default Methods

        public override object Create(ContentTypeInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                if (CheckContentTypeExists(info.Name, 0, errors))
                {
                    return 0;
                }
                if (CheckTableExists(info.TableName, errors))
                {
                    return 0;
                }
                if (CreateTable(info.TableName, errors))
                {
                    FieldInfo[] fieldInfoArray = CreateApplicationDefaultFields();
                    info.FieldsXml = FieldInfo.GetFieldXml(fieldInfoArray);
                    info.XmlSchema = DataConnection.GetXMLSchema(info.TableName);

                    var param = new object[10, 3];
                    param[0, 0] = "@Name";
                    param[0, 1] = info.Name;
                    param[1, 0] = "@Description";
                    param[1, 1] = info.Description;
                    param[2, 0] = "@TableName";
                    param[2, 1] = info.TableName;
                    param[3, 0] = "@Fields";
                    param[3, 1] = info.FieldsXml;
                    param[4, 0] = "@XmlSchema";
                    param[4, 1] = info.XmlSchema;
                    param[5, 0] = "@Image";
                    param[5, 1] = info.Image;
                    param[6, 0] = "@DefaultFormId";
                    param[6, 1] = info.DefaultFormId;
                    param[7, 0] = "@DefaultTransformationId";
                    param[7, 1] = info.DefaultTransformationId;
                    param[8, 0] = "@IsSystem";
                    param[8, 1] = info.IsSystem;
                    param[9, 0] = "@IsDeleted";
                    param[9, 1] = info.IsDeleted;
                    var error = new ErrorInfo();
                    object result = DataConnection.ExecuteScalar("freb_ContentType_Insert", param, QueryType.StoredProcedure, error);
                    if (error.Ok)
                    {
                        info.Id = ValidationHelper.GetInteger(result, 0);
                        CreateQuery(info, errors);
                        RegisterObjectToCache(info);
                        return result;
                    }
                    RegisterError(errors, error);
                }
            }
            else
            {
                var error = new ErrorInfo();
                error.Ok = false;
                error.Date = DateTime.Now;
                error.Message = "ContentTypeInfo object is null";
                RegisterError(errors, error);
            }

            return null;
        }

        public override bool Update(ContentTypeInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var error = new ErrorInfo();
                info.XmlSchema = DataConnection.GetXMLSchema(info.TableName);

                var param = new object[11, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@Description";
                param[2, 1] = info.Description;
                param[3, 0] = "@TableName";
                param[3, 1] = info.TableName;
                param[4, 0] = "@Fields";
                param[4, 1] = info.FieldsXml;
                param[5, 0] = "@XmlSchema";
                param[5, 1] = info.XmlSchema;
                param[6, 0] = "@Image";
                param[6, 1] = info.Image;
                param[7, 0] = "@DefaultFormId";
                param[7, 1] = info.DefaultFormId;
                param[8, 0] = "@DefaultTransformationId";
                param[8, 1] = info.DefaultTransformationId;
                param[9, 0] = "@IsSystem";
                param[9, 1] = info.IsSystem;
                param[10, 0] = "@IsDeleted";
                param[10, 1] = info.IsDeleted;
                DataConnection.ExecuteNonQuery("freb_ContentType_Update", param, QueryType.StoredProcedure, error);
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
                error.Date = DateTime.Now;
                error.Message = "ContentTypeInfo object is null";
                RegisterError(errors, error);
            }

            return false;
        }

        public override bool Delete(int id, ErrorInfoList errors)
        {
            ContentTypeInfo info = Select(id, errors);
            if (info != null)
            {
                if (DeleteTable(info.TableName, errors))
                {
                    var param = new object[1, 3];
                    param[0, 0] = "@Id";
                    param[0, 1] = id;

                    var error = new ErrorInfo();
                    DataConnection.ExecuteNonQuery("freb_ContentType_Delete", param, QueryType.StoredProcedure, error);
                    if (error.Ok)
                    {
                        DeleteObjectFromCache(info);
                        return true;
                    }
                    RegisterError(errors, error);
                }
            }
            return false;
        }

        public bool UpdateField(ContentTypeInfo contentTypeInfo, FieldInfo fieldInfo, FieldActionMode fieldActionMode, bool isComponent, ErrorInfoList errors)
        {
            if (fieldActionMode == FieldActionMode.Create && !isComponent)
            {
                if (CheckFieldExists(contentTypeInfo.TableName, fieldInfo.Name, errors))
                    return false;

                if (!CreateColumn(contentTypeInfo.TableName, fieldInfo, errors))
                    return false;
            }

            if (fieldActionMode == FieldActionMode.Delete && !isComponent)
            {
                if (CheckFieldExists(contentTypeInfo.TableName, fieldInfo.Name, errors))
                {
                    errors.Clear();
                    if (!DeleteColumn(contentTypeInfo.TableName, fieldInfo.Name, errors))
                        return false;
                }
                else
                {
                    RegisterError(errors, new ErrorInfo
                                              {
                                                  Name = "ContentTypeProvider",
                                                  Message = "Column name <" + fieldInfo.Name + "> is not exists",
                                                  Ok = false
                                              });
                    return false;
                }
            }

            UpdateQuery(contentTypeInfo, errors);
            RegisterObjectToCache(contentTypeInfo);
            if (Update(contentTypeInfo, errors))
                return true;
            return false;
        }

        public override ContentTypeInfo Select(int id, ErrorInfoList errors)
        {
            ContentTypeInfo result = GetObjectFromCache(id);
            if (result != null)
                return result;

            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "@Id";
            param[0, 1] = id;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_ContentType_SelectById", param,
                                                                       QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                result = new ContentTypeInfo(dataTable.Rows[0]);
                RegisterObjectToCache(result);
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public ContentTypeInfo SelectByName(string name, ErrorInfoList errors)
        {
            ContentTypeInfo result = GetObjectFromCache(name);
            if (result != null)
                return result;
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "@Name";
            param[0, 1] = name;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_ContentType_SelectByName", param,
                                                                       QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                result = new ContentTypeInfo(dataTable.Rows[0]);
                RegisterObjectToCache(result);
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<ContentTypeInfo> SelectAll(ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_ContentType_SelectAll", null,
                                                                       QueryType.StoredProcedure, error);
            var result = new List<ContentTypeInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new ContentTypeInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<ContentTypeInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
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
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_ContentType_SelectByPagingSorting", param, QueryType.StoredProcedure, error);
            var result = new List<ContentTypeInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new ContentTypeInfo(dataTable.Rows[i]));
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



        public override ContentTypeInfo GetObjectFromCache(int id)
        {
            return CacheHelper.GetContenTypeFromCache(id);
        }

        public override ContentTypeInfo GetObjectFromCache(string name)
        {
            return CacheHelper.GetContenTypeFromCache(name);
        }

        public override void RegisterObjectToCache(ContentTypeInfo contentTypeInfo)
        {
            CacheHelper.AddContentTypeToCache(contentTypeInfo);
        }

        public override void DeleteObjectFromCache(ContentTypeInfo contentTypeInfo)
        {
            CacheHelper.DeleteContentTypeFromCache(contentTypeInfo);
        }



        private void EnsureCreated()
        {

        }

        #endregion

        #region Methods

        public FieldInfo[] CreateApplicationDefaultFields()
        {
            var fieldInfoList = new List<FieldInfo>();

            var pars = new object[14, 5];
            pars[0, 0] = "Id";
            pars[0, 1] = "Id";
            pars[0, 2] = DataFieldType.Integer;
            pars[0, 3] = FormFieldType.LabelControl;
            pars[0, 4] = "true";

            pars[1, 0] = "SeoTemplate";
            pars[1, 1] = "SeoTemplate";
            pars[1, 2] = DataFieldType.Varchar;
            pars[1, 3] = FormFieldType.LabelControl;
            pars[1, 4] = "false";

            pars[2, 0] = "MetaTitle";
            pars[2, 1] = "true";
            pars[2, 2] = DataFieldType.Varchar;
            pars[2, 3] = FormFieldType.LabelControl;
            pars[2, 4] = "false";

            pars[3, 0] = "MetaDescription";
            pars[3, 1] = "true";
            pars[3, 2] = DataFieldType.Text;
            pars[3, 3] = FormFieldType.LabelControl;
            pars[3, 4] = "false";

            pars[4, 0] = "MetaKeywords";
            pars[4, 1] = "true";
            pars[4, 2] = DataFieldType.Text;
            pars[4, 3] = FormFieldType.LabelControl;
            pars[4, 4] = "false";

            pars[5, 0] = "CopyRights";
            pars[5, 1] = "true";
            pars[5, 2] = DataFieldType.Text;
            pars[5, 3] = FormFieldType.LabelControl;
            pars[5, 4] = "false";

            pars[6, 0] = "MetaImage";
            pars[6, 1] = "true";
            pars[6, 2] = DataFieldType.Text;
            pars[6, 3] = FormFieldType.LabelControl;
            pars[6, 4] = "false";

            pars[7, 0] = "Language";
            pars[7, 1] = "Language";
            pars[7, 2] = DataFieldType.Varchar;
            pars[7, 3] = FormFieldType.Unknown;
            pars[7, 4] = "true";

            pars[8, 0] = "CreatedBy";
            pars[8, 1] = "Created By";
            pars[8, 2] = DataFieldType.Varchar;
            pars[8, 3] = FormFieldType.Unknown;
            pars[8, 4] = "true";

            pars[9, 0] = "CreatedDate";
            pars[9, 1] = "Created Date";
            pars[9, 2] = DataFieldType.DateTime;
            pars[9, 3] = FormFieldType.Unknown;
            pars[9, 4] = "false";

            pars[10, 0] = "ModifiedBy";
            pars[10, 1] = "Modified By";
            pars[10, 2] = DataFieldType.Varchar;
            pars[10, 3] = FormFieldType.LabelControl;
            pars[10, 4] = "false";

            pars[11, 0] = "ModifiedDate";
            pars[11, 1] = "Modified Date";
            pars[11, 2] = DataFieldType.DateTime;
            pars[11, 3] = FormFieldType.LabelControl;
            pars[11, 4] = "false";

            pars[12, 0] = "IsDeleted";
            pars[12, 1] = "false";
            pars[12, 2] = DataFieldType.Boolean;
            pars[12, 3] = FormFieldType.LabelControl;
            pars[12, 4] = "false";

            pars[13, 0] = "IsPublished";
            pars[13, 1] = "true";
            pars[13, 2] = DataFieldType.Boolean;
            pars[13, 3] = FormFieldType.LabelControl;
            pars[13, 4] = "false";


            for (int i = pars.GetLowerBound(0); i <= pars.GetUpperBound(0); i++)
            {
                var fieldInfo = new FieldInfo();
                fieldInfo.Name = pars[i, 0].ToString();
                fieldInfo.DisplayName = pars[i, 1].ToString();
                fieldInfo.CreatedBy = "system";
                fieldInfo.DataType = (DataFieldType)pars[i, 2];
                fieldInfo.CreatedDate = DateTime.Today;
                fieldInfo.ModifiedBy = "system";
                fieldInfo.ModifiedDate = DateTime.Today;
                fieldInfo.SortOrder = 0;
                fieldInfo.FieldType = (FormFieldType)pars[i, 3];
                fieldInfo.ShowInListing = ValidationHelper.GetBoolean(pars[i, 4], false);
                if (pars[i, 0].ToString() == "Id")
                {
                    fieldInfo.IsPrimaryKey = true;
                }

                fieldInfoList.Add(fieldInfo);
            }

            return fieldInfoList.ToArray();
        }

        public bool CheckContentTypeExists(string contentTypeName, int id, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[2, 3];
            param[0, 0] = "@Name";
            param[0, 1] = contentTypeName;
            param[1, 0] = "@Id";
            param[1, 1] = id;
            SqlDataReader reader = DataConnection.ExecuteReader("SELECT dbo.freb_asystem_ContentTypeExists(@Name,@Id)",
                                                                param, QueryType.SqlQuery,
                                                                CommandBehavior.CloseConnection, error);
            if (error.Ok && reader.Read())
            {
                bool result = reader.GetBoolean(0);
                if (result)
                    RegisterError(errors, new ErrorInfo
                                              {
                                                  Ok = false,
                                                  Name = "ContentType Provider",
                                                  Message = ": Content type " + contentTypeName + "  already exists!"
                                              });
                reader.Close();
                return result;
            }
            RegisterError(errors, error);
            return false;
        }

        public bool CheckTableExists(string tableName, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "@TableName";
            param[0, 1] = tableName;
            SqlDataReader reader = DataConnection.ExecuteReader(
                "SELECT  dbo.freb_asystem_CheckExistsTable(@TableName)", param, QueryType.SqlQuery,
                CommandBehavior.CloseConnection, error);
            if (error.Ok && reader.Read())
            {
                bool result = reader.GetBoolean(0);
                if (result)
                    RegisterError(errors, new ErrorInfo
                                              {
                                                  Ok = false,
                                                  Name = "ContentType Provider",
                                                  Message = ": DB Table " + tableName + "  already exists!"
                                              });
                reader.Close();
                return result;
            }
            RegisterError(errors, error);

            return false;
        }

        public bool CreateTable(string tableName, ErrorInfoList errors)
        {
            var param = new object[1, 3];
            param[0, 0] = "@TableName";
            param[0, 1] = tableName;
            var error = new ErrorInfo();
            DataConnection.ExecuteNonQuery("freb_asystem_CreateTable", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public bool DeleteTable(string tableName, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "@TableName";
            param[0, 1] = tableName;
            DataConnection.ExecuteNonQuery("freb_asystem_DropTable", param, QueryType.StoredProcedure, error);
            if (error.Ok)
                return true;
            RegisterError(errors, error);
            return false;
        }

        public bool CheckFieldExists(string tableName, string columnName, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[2, 3];
            param[0, 0] = "@TableName";
            param[0, 1] = tableName;
            param[1, 0] = "@ColumnName";
            param[1, 1] = columnName;
            SqlDataReader reader =
                DataConnection.ExecuteReader("SELECT  dbo.freb_asystem_FieldExists(@TableName,@ColumnName)", param, QueryType.SqlQuery, CommandBehavior.CloseConnection, error);
            if (error.Ok && reader.Read())
            {
                bool result = reader.GetBoolean(0);
                if (result)
                    RegisterError(errors, new ErrorInfo
                                              {
                                                  Name = "ContentTypeProvider",
                                                  Message = "Column name " + columnName + " already exists",
                                                  Ok = false
                                              });
                reader.Close();
                return result;
            }
            RegisterError(errors, error);
            return false;
        }

        public bool CreateColumn(string tableName, FieldInfo fieldInfo, ErrorInfoList errors)
        {
            var param = new object[6, 3];
            param[0, 0] = "@TableName";
            param[0, 1] = tableName;
            param[1, 0] = "@ColumnName";
            param[1, 1] = fieldInfo.Name;
            param[2, 0] = "@ColumnType";
            param[2, 1] = FormHelper.GetDataTypeCodeByType(fieldInfo.DataType);
            param[3, 0] = "@ColumnSize";
            param[3, 1] = fieldInfo.Size;
            param[4, 0] = "@DefaultValue";
            param[4, 1] = fieldInfo.DefaultValue;
            param[5, 0] = "@IsNull";
            param[5, 1] = fieldInfo.IsAllowNull;
            var error = new ErrorInfo();
            DataConnection.ExecuteNonQuery("freb_asystem_CreateColumn", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public bool DeleteColumn(string tableName, string columnName, ErrorInfoList errors)
        {
            var param = new object[2, 3];
            param[0, 0] = "@TableName";
            param[0, 1] = tableName;
            param[1, 0] = "@ColumnName";
            param[1, 1] = columnName;
            var error = new ErrorInfo();
            DataConnection.ExecuteNonQuery("freb_asystem_DropColumn", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        private void CreateQuery(ContentTypeInfo contentTypeInfo, ErrorInfoList errors)
        {
            var info = new QueryInfo();
            info.ContentTypeId = contentTypeInfo.Id;
            info.IsDeleted = false;
            info.Name = string.Format("{0}.select", contentTypeInfo.TableName);
            info.Text = SqlHelper.GenerateSelectScript(contentTypeInfo.TableName);
            _queryProvider.Create(info, errors);

            info.Name = string.Format("{0}.select_by_seo", contentTypeInfo.TableName);
            info.Text = SqlHelper.GenerateSelectBySeoTemplateScript(contentTypeInfo.TableName);
            _queryProvider.Create(info, errors);

            info.Name = string.Format("{0}.select_all", contentTypeInfo.TableName);
            info.Text = SqlHelper.GenerateSelectAllScript(contentTypeInfo.TableName);
            _queryProvider.Create(info, errors);

            info.Name = string.Format("{0}.select_paging", contentTypeInfo.TableName);
            info.Text = SqlHelper.GenerateSelectPagingScript(contentTypeInfo.TableName);
            _queryProvider.Create(info, errors);

            info.Name = string.Format("{0}.select_total_count", contentTypeInfo.TableName);
            info.Text = SqlHelper.GenerateSelectTotalCountScript(contentTypeInfo.TableName);
            _queryProvider.Create(info, errors);

            info.Name = string.Format("{0}.delete", contentTypeInfo.TableName);
            info.Text = SqlHelper.GenerateDeleteScript(contentTypeInfo.TableName);
            _queryProvider.Create(info, errors);

            info.Name = string.Format("{0}.insert", contentTypeInfo.TableName);
            info.Text = SqlHelper.GenerateInsertScript(contentTypeInfo.TableName,
                                                       FieldInfo.GetFieldArray(contentTypeInfo.FieldsXml));
            _queryProvider.Create(info, errors);

            info.Name = string.Format("{0}.update", contentTypeInfo.TableName);
            info.Text = SqlHelper.GenerateUpdateScript(contentTypeInfo.TableName,
                                                       FieldInfo.GetFieldArray(contentTypeInfo.FieldsXml));
            _queryProvider.Create(info, errors);

            info.Name = string.Format("{0}.select_meta_tags", contentTypeInfo.TableName);
            info.Text = SqlHelper.GenerateMetaTagsSelectScript(contentTypeInfo.TableName);
            _queryProvider.Create(info, errors);
        }

        private void UpdateQuery(ContentTypeInfo contentTypeInfo, ErrorInfoList errors)
        {
            QueryInfo info = _queryProvider.SelectByName(string.Format("{0}.insert", contentTypeInfo.TableName), errors);
            info.Name = string.Format("{0}.insert", contentTypeInfo.TableName);
            info.Text = SqlHelper.GenerateInsertScript(contentTypeInfo.TableName,
                                                       FieldInfo.GetFieldArray(contentTypeInfo.FieldsXml));
            _queryProvider.Update(info, errors);

            info = _queryProvider.SelectByName(string.Format("{0}.update", contentTypeInfo.TableName), errors);
            info.Name = string.Format("{0}.update", contentTypeInfo.TableName);
            info.Text = SqlHelper.GenerateUpdateScript(contentTypeInfo.TableName,
                                                       FieldInfo.GetFieldArray(contentTypeInfo.FieldsXml));
            _queryProvider.Update(info, errors);
        }

        #endregion
    }
}
