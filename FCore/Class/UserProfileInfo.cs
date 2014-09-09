using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using FCore.Helper;

namespace FCore.Class
{
    [Serializable]
    public class UserProfileInfo : ClassInfo
    {
        public string UserId { get; set; }
        public int ContentTypeId { get; set; }
        public int ContentId { get; set; }

        private Dictionary<string, string> _attributes;

        public UserProfileInfo()
        {
            _attributes = new Dictionary<string, string>();

        }

        public UserProfileInfo(DataRow dataRow)
        {
            _attributes = new Dictionary<string, string>();

            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("UserId") > -1)
                UserId = ValidationHelper.GetString(dataRow["UserId"], "");
            if (dataRow.Table.Columns.IndexOf("ContentTypeId") > -1)
                ContentTypeId = ValidationHelper.GetInteger(dataRow["ContentTypeId"], 0);
            if (dataRow.Table.Columns.IndexOf("ContentId") > -1)
                ContentId = ValidationHelper.GetInteger(dataRow["ContentId"], 0);

            _attributes.Add("UserName", UserId);
            _attributes.Add("ContentTypeId", ContentTypeId.ToString());
            _attributes.Add("ContentId", ContentId.ToString());
        }

        public void FillAttributes(DataRow dataRow)
        {
            foreach (DataColumn column in dataRow.Table.Columns)
            {
                _attributes.Add(column.ColumnName, ValidationHelper.GetString(dataRow[column.ColumnName], string.Empty));
            }
        }

        public string GetValue(string name)
        {
            if (_attributes.ContainsKey(name))
                return _attributes[name];
            return string.Empty;
        }
    }
}
