using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FCore.Helper;

namespace FCore.Class
{
    [Serializable]
    public class ContentTypeInfo : ClassInfo
    {
        public string TableName { get; set; }
        public string FieldsXml { get; set; }
        public string Image { get; set; }
        public string XmlSchema { get; set; }
        public int DefaultFormId { get; set; }
        public int DefaultTransformationId { get; set; }
        public bool IsSystem { get; set; }

        public ContentTypeInfo() { }

        public ContentTypeInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("Name") > -1)
                Name = ValidationHelper.GetString(dataRow["Name"], "");
            if (dataRow.Table.Columns.IndexOf("Description") > -1)
                Description = ValidationHelper.GetString(dataRow["Description"], "");
            if (dataRow.Table.Columns.IndexOf("TableName") > -1)
                TableName = ValidationHelper.GetString(dataRow["TableName"], "");
            if (dataRow.Table.Columns.IndexOf("Fields") > -1)
                FieldsXml = ValidationHelper.GetString(dataRow["Fields"], "");
            if (dataRow.Table.Columns.IndexOf("Image") > -1)
                Image = ValidationHelper.GetString(dataRow["Image"], "");
            if (dataRow.Table.Columns.IndexOf("XmlSchema") > -1)
                XmlSchema = ValidationHelper.GetString(dataRow["XmlSchema"], "");
            if (dataRow.Table.Columns.IndexOf("DefaultFormId") > -1)
                DefaultFormId = ValidationHelper.GetInteger(dataRow["DefaultFormId"], 0);
            if (dataRow.Table.Columns.IndexOf("DefaultTransformationId") > -1)
                DefaultTransformationId = ValidationHelper.GetInteger(dataRow["DefaultTransformationId"], 0);
            if (dataRow.Table.Columns.IndexOf("IsSystem") > -1)
                IsSystem = ValidationHelper.GetBoolean(dataRow["IsSystem"], false);
            if (dataRow.Table.Columns.IndexOf("IsDeleted") > -1)
                IsDeleted = ValidationHelper.GetBoolean(dataRow["IsDeleted"], false);
        }
    }
}
