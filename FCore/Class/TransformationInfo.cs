using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FCore.Helper;

namespace FCore.Class
{
    [Serializable]
    public class TransformationInfo : ClassInfo
    {
        public String Text { get; set; }
        public int ContentTypeId { get; set; }


        public TransformationInfo()
        {
        }

        public TransformationInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("Name") > -1)
                Name = ValidationHelper.GetString(dataRow["Name"], "");
            if (dataRow.Table.Columns.IndexOf("Text") > -1)
                Text = ValidationHelper.GetString(dataRow["Text"], "");
            if (dataRow.Table.Columns.IndexOf("ContentTypeId") > -1)
                ContentTypeId = ValidationHelper.GetInteger(dataRow["ContentTypeId"], 0);
            if (dataRow.Table.Columns.IndexOf("IsDeleted") > -1)
                IsDeleted = ValidationHelper.GetBoolean(dataRow["IsDeleted"], false);

        }

    }
}
