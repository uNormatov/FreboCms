using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FCore.Helper;

namespace FCore.Class
{
    [Serializable]
    public class WebPartInfo : ClassInfo
    {
        public string Properties { get; set; }
        public string Screenshot { get; set; }
        public string FolderPath { get; set; }
        public int WebPartCategoryId { get; set; }

        public WebPartInfo() { }

        public WebPartInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("Name") > -1)
                Name = ValidationHelper.GetString(dataRow["Name"], "");
            if (dataRow.Table.Columns.IndexOf("Screenshot") > -1)
                Screenshot = ValidationHelper.GetString(dataRow["Screenshot"], "");
            if (dataRow.Table.Columns.IndexOf("FolderPath") > -1)
                FolderPath = ValidationHelper.GetString(dataRow["FolderPath"], "");
            if (dataRow.Table.Columns.IndexOf("WebPartCategoryId") > -1)
                WebPartCategoryId = ValidationHelper.GetInteger(dataRow["WebPartCategoryId"], 0);
            if (dataRow.Table.Columns.IndexOf("IsDeleted") > -1)
                IsDeleted = ValidationHelper.GetBoolean(dataRow["IsDeleted"], false);
        }
    }
}
