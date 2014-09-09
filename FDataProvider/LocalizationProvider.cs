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
    public class LocalizationProvider : BaseProvider<LanguageInfo>
    {
        public LocalizationProvider()
            : this(null)
        {
        }

        public LocalizationProvider(DataConnection connection)
        {
            DataConnection = connection ?? new DataConnection();
        }

        public override object Create(LanguageInfo info, ErrorInfoList errors)
        {
            if (info != null && !CheckFieldExists("Translation", info.Code, errors))
            {
                var param = new object[2, 3];
                param[0, 0] = "@Name";
                param[0, 1] = info.Name;
                param[1, 0] = "@Code";
                param[1, 1] = info.Code;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_Language_Insert", param, QueryType.StoredProcedure, error);
                if (error.Ok)
                {
                    FieldInfo fieldInfo = new FieldInfo();
                    fieldInfo.Name = info.Code;
                    fieldInfo.DataType = DataFieldType.Text;
                    fieldInfo.IsAllowNull = true;
                    fieldInfo.DefaultValue = string.Empty;
                    if (!CreateColumn("Translation", fieldInfo, errors))
                        Delete(ValidationHelper.GetInteger(result, 0), errors);
                }
                RegisterError(errors, error);
            }
            else
            {
                var error = new ErrorInfo();
                error.Ok = false;
                error.Date = DateTime.Now;
                error.Message = "Language object is null";
                RegisterError(errors, error);
            }

            return null;
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
                DataConnection.ExecuteReader("SELECT  dbo.freb_asystem_FieldExists(@TableName,@ColumnName)", param,
                                             QueryType.SqlQuery, CommandBehavior.CloseConnection, error);
            if (error.Ok && reader.Read())
            {
                bool result = reader.GetBoolean(0);
                if (result)
                    RegisterError(errors, new ErrorInfo
                    {
                        Name = "LocalizationProvider",
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

        public override bool Update(LanguageInfo info, ErrorInfoList errors)
        {
            if (info != null)
            {
                var param = new object[3, 3];
                param[0, 0] = "@Id";
                param[0, 1] = info.Id;
                param[1, 0] = "@Name";
                param[1, 1] = info.Name;
                param[2, 0] = "@Code";
                param[2, 1] = info.Code;
                var error = new ErrorInfo();
                object result = DataConnection.ExecuteScalar("freb_Language_Update", param, QueryType.StoredProcedure, error);
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
                error.Message = "ListInfo object is null";
                RegisterError(errors, error);
            }

            return false;
        }

        public override bool Delete(int id, ErrorInfoList errors)
        {
            LanguageInfo languageInfo = Select(id, errors);
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "@Id";
            param[0, 1] = id;
            DataConnection.ExecuteScalar("freb_Language_Delete", param, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                if (CheckFieldExists("Translation", languageInfo.Code, errors))
                    DeleteColumn("Translation", languageInfo.Code, errors);

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

        public override LanguageInfo Select(int id, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "@Id";
            param[0, 1] = id;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Language_SelectById", param,
                                                                       QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                var listInfo = new LanguageInfo(dataTable.Rows[0]);
                return listInfo;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<LanguageInfo> SelectAll(ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Language_SelectAll", null,
                                                                       QueryType.StoredProcedure, error);
            var result = new List<LanguageInfo>();
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new LanguageInfo(dataTable.Rows[i]));
                }
                return result;
            }
            RegisterError(errors, error);
            return null;
        }

        public override List<LanguageInfo> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
        {
            return null;
        }

        public DataTable SelecAllTranslations(ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Translation_SelectAll", null, QueryType.StoredProcedure, error);
            if (!error.Ok)
            {
                RegisterError(errors, error);
            }
            return dataTable;
        }

        public DataTable SelectTranslationByKeyword(string keyword, ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            var param = new object[1, 3];
            param[0, 0] = "@Keyword";
            param[0, 1] = keyword;
            DataTable dataTable = DataConnection.ExecuteDataTableQuery("freb_Translation_SelectByKeyword", param, QueryType.StoredProcedure, error);
            if (!error.Ok)
                RegisterError(errors, error);
            return dataTable;
        }

        public bool CreateTranslation(List<LanguageInfo> languageInfos, object[,] param, ErrorInfoList errors)
        {
            string insertQuery = SqlHelper.GenerateTranslationInsertScript(languageInfos);
            var error = new ErrorInfo();
            DataConnection.ExecuteNonQuery(insertQuery, param, QueryType.SqlQuery, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public bool UpdateTranslation(List<LanguageInfo> languageInfos, object[,] param, ErrorInfoList errors)
        {
            string updateQuery = SqlHelper.GenerateTranslationUpdateScript(languageInfos);
            var error = new ErrorInfo();
            DataConnection.ExecuteNonQuery(updateQuery, param, QueryType.SqlQuery, error);
            if (error.Ok)
            {
                return true;
            }
            RegisterError(errors, error);
            return false;
        }

        public bool DeleteTranslation(string keyword, ErrorInfoList errors)
        {
            var param = new object[1, 3];
            param[0, 0] = "@Keyword";
            param[0, 1] = keyword;
            ErrorInfo error = new ErrorInfo();
            DataConnection.ExecuteNonQuery("freb_Translation_Delete", param, QueryType.StoredProcedure, error);
            if (!error.Ok)
            {
                RegisterError(errors, error);
                return false;
            }
            return true;
        }

        public override void RegisterObjectToCache(LanguageInfo info)
        {
            throw new NotImplementedException();
        }

        public override void DeleteObjectFromCache(LanguageInfo info)
        {
            throw new NotImplementedException();
        }

        public override LanguageInfo GetObjectFromCache(int id)
        {
            throw new NotImplementedException();
        }

        public override LanguageInfo GetObjectFromCache(string name)
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
