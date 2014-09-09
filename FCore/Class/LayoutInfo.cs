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
    public class LayoutInfo : ClassInfo
    {
        public string Layout { get; set; }
        public string Css { get; set; }
        public string Screenshot { get; set; }
        public string BodyOption { get; set; }
        public string DocOption { get; set; }
        public List<BlockInfo> Blocks { get; set; }
        public int LayoutCategoryId { get; set; }
        public bool IsMaster { get; set; }

        public LayoutInfo()
        {
        }

        public LayoutInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("Name") > -1)
                Name = ValidationHelper.GetString(dataRow["Name"], "");
            if (dataRow.Table.Columns.IndexOf("Description") > -1)
                Description = ValidationHelper.GetString(dataRow["Description"], "");
            if (dataRow.Table.Columns.IndexOf("Layout") > -1)
                Layout = ValidationHelper.GetString(dataRow["Layout"], "");
            if (dataRow.Table.Columns.IndexOf("Css") > -1)
                Css = ValidationHelper.GetString(dataRow["Css"], "");
            if (dataRow.Table.Columns.IndexOf("Screenshot") > -1)
                Screenshot = ValidationHelper.GetString(dataRow["Screenshot"], "");
            if (dataRow.Table.Columns.IndexOf("BodyOption") > -1)
                BodyOption = ValidationHelper.GetString(dataRow["BodyOption"], "");
            if (dataRow.Table.Columns.IndexOf("DocOption") > -1)
                DocOption = ValidationHelper.GetString(dataRow["DocOption"], "");
            if (dataRow.Table.Columns.IndexOf("LayoutCategoryId") > -1)
                LayoutCategoryId = ValidationHelper.GetInteger(dataRow["LayoutCategoryId"], 0);
            if (dataRow.Table.Columns.IndexOf("IsDeleted") > -1)
                IsDeleted = ValidationHelper.GetBoolean(dataRow["IsDeleted"], false);
            if (dataRow.Table.Columns.IndexOf("IsMaster") > -1)
                IsMaster = ValidationHelper.GetBoolean(dataRow["IsMaster"], false);
        }
    }
}
