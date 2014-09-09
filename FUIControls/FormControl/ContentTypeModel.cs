using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FCore.Class;
using FCore.Collection;
using FCore.Helper;
using FDataProvider;
using FUIControls.Settings;

namespace FUIControls.FormControl
{
    public class ContentTypeModel
    {
        private ContentTypeProvider _contentTypeProvider;
        private GeneralConnection _generalConnection;

        private ContentTypeInfo _contentTypeInfo;
        private DataSet _dataSet;
        private DataTable _dataTable;
        private DataRow _dataRow;
        private int _columnCount;
        private int _contentTypeId;
        private int _contentId;

        public string Name { get; set; }

        public string TableName { get; set; }

        public List<string> ColumnNames { get; set; }

        public ErrorInfoList ErrorInfoList { get; set; }

        private bool _isPublished = false;
        public bool IsPublished
        {
            get { return _isPublished; }
            set { _isPublished = value; }
        }

        public bool IsEdit { get; set; }

        public int ContentTypeId
        {
            get { return _contentTypeId; }
            set { _contentTypeId = value; }
        }

        public string ContentTypeName { get; set; }

        public int ContentId
        {
            get { return _contentId; }
            set { _contentId = value; }
        }

        public ContentTypeInfo ContentTypeInfo
        {
            get { return _contentTypeInfo; }
        }

        public string SeoTemplateColumn { get; set; }

        public ContentTypeModel(int contentTypeId, GeneralConnection generalConnection, ContentTypeProvider contentTypeProvider, ErrorInfoList errors)
        {
            _contentTypeId = contentTypeId;
            if (generalConnection != null)
                _generalConnection = generalConnection;
            if (contentTypeProvider != null)
                _contentTypeProvider = contentTypeProvider;
            if (errors != null)
                ErrorInfoList = errors;
            ContentId = 0;
            _dataRow = null;
            IsEdit = false;
            Init(null);
        }

        public ContentTypeModel(int contentTypeId, int recordId, GeneralConnection generalConnection, ContentTypeProvider contentTypeProvider, ErrorInfoList errors)
        {
            _contentTypeId = contentTypeId;
            _contentId = recordId;
            if (generalConnection != null)
                _generalConnection = generalConnection;
            if (contentTypeProvider != null)
                _contentTypeProvider = contentTypeProvider;
            if (errors != null)
                ErrorInfoList = errors;
            IsEdit = true;
            Init(null);
        }

        public ContentTypeModel(int contentTypeId, DataRow row, GeneralConnection generalConnection, ContentTypeProvider contentTypeProvider, ErrorInfoList errors)
        {
            _contentTypeId = contentTypeId;
            if (generalConnection != null)
                _generalConnection = generalConnection;
            if (contentTypeProvider != null)
                _contentTypeProvider = contentTypeProvider;
            if (errors != null)
                ErrorInfoList = errors;
            IsEdit = true;
            Init(row);
        }


        private void Init(DataRow row)
        {
            if (_contentTypeInfo == null)
            {
                if (_contentTypeProvider == null)
                    _contentTypeProvider = new ContentTypeProvider();
                if (_generalConnection == null)
                    _generalConnection = new GeneralConnection();
            }

            if (_contentTypeInfo == null)
                _contentTypeInfo = _contentTypeProvider.Select(_contentTypeId, ErrorInfoList);

            _dataSet = FormHelper.GetDataSet(_contentTypeInfo.XmlSchema);
            _dataTable = _dataSet.Tables[0];
            _dataRow = _dataTable.NewRow();
            _dataTable.Rows.Add(_dataRow);
            Name = _contentTypeInfo.Name;
            TableName = _contentTypeInfo.TableName;


            FillColumns();
            if (IsEdit)
            {
                if (ContentId != 0)
                {
                    var parameters = new object[1, 3];
                    parameters[0, 0] = "@Id";
                    parameters[0, 1] = _contentId;
                    DataTable dt = _generalConnection.ExecuteDataTableQuery(TableName + ".select", parameters,
                                                                            ErrorInfoList);
                    if (dt != null && dt.Rows.Count > 0)
                        FillDefaultRowValues(dt.Rows[0]);
                }
                else
                {
                    FillDefaultRowValues(row);
                }
            }
            IsPublished = ValidationHelper.GetBoolean(GetValue("IsPublished"), false);
        }

        private void FillColumns()
        {
            _columnCount = _dataTable.Columns.Count;
            ColumnNames = new List<string>(_columnCount);
            foreach (DataColumn column in _dataTable.Columns)
            {
                string columnName = column.ColumnName;
                ColumnNames.Add(columnName);
            }
        }

        private void FillDefaultRowValues(DataRow row)
        {
            if (row != null)
            {
                foreach (DataColumn column in _dataTable.Columns)
                {
                    if (row.Table.Columns[column.ColumnName] != null)
                    {
                        _dataRow[column.ColumnName] = row[column.ColumnName];
                    }
                }
            }
        }

        public virtual bool ContainsColumn(string columnName)
        {
            return ColumnNames.Contains(columnName);
        }

        public virtual object GetValue(string columnName)
        {
            if (_dataRow == null)
            {
                return null;
            }

            if (_dataRow[columnName] == DBNull.Value)
            {
                return null;
            }
            return _dataRow[columnName];
        }

        public virtual void SetValue(string columnName, object value)
        {
            if (_dataRow != null)
            {
                if (value == null)
                {
                    value = DBNull.Value;
                }

                _dataRow[columnName] = value;
            }
        }

        public virtual int Insert()
        {
            CreateDefaultValues();
            object[,] pars = FormHelper.ConvertDataRowToParams(_dataRow, ColumnNames, true);
            object result = _generalConnection.ExecuteScalar(TableName + ".insert", pars, ErrorInfoList);
            _contentId = ValidationHelper.GetInteger(result, -1);
            if (_contentId != -1)
            {
                SetValue("Id", _contentId);
                CacheHelper.DeleteAll(TableName);
            }

            return _contentId;
        }

        public virtual void Update()
        {
            CreateDefaultValues();
            object[,] pars = FormHelper.ConvertDataRowToParams(_dataRow, ColumnNames, false);
            _generalConnection.ExecuteNonQuery(TableName + ".update", pars, ErrorInfoList);
            CacheHelper.DeleteAll(TableName);
        }

        public virtual void Delete()
        {
            object[,] pars = new object[1, 3];
            pars[0, 0] = "@Id";
            pars[0, 1] = ContentId;
            _generalConnection.ExecuteNonQuery(TableName + ".delete", pars, ErrorInfoList);
            CacheHelper.DeleteAll(TableName);
        }

        private void CreateDefaultValues()
        {
            if (!IsEdit)
            {
                _dataRow["CreatedBy"] = CoreSettings.CurrentUserName;
                _dataRow["CreatedDate"] = DateTime.Now;
                _dataRow["IsDeleted"] = false;

            }
            _dataRow["IsPublished"] = IsPublished;
            _dataRow["ModifiedBy"] = CoreSettings.CurrentUserName;
            _dataRow["ModifiedDate"] = DateTime.Now;

            if (string.IsNullOrEmpty(ValidationHelper.GetString(_dataRow["SeoTemplate"], string.Empty)))
            {
                FieldInfo seoTemplateField =
                    FieldInfo.GetFieldArray(_contentTypeInfo.FieldsXml).FirstOrDefault(x => x.UseAsSeoTemplate);

                if (seoTemplateField != null)
                    _dataRow["SeoTemplate"] =
                        SiteHelper.ToUrl(ValidationHelper.GetString(GetValue(seoTemplateField.Name), ""));
                else _dataRow["SeoTemplate"] = string.Empty;
            }
        }
    }
}

