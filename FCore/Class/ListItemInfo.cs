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
    public class ListItemInfo : ClassInfo
    {
        public int ListId { get; set; }
        public string ListName { get; set; }
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public string SeoTemplate { get; set; }

        public ListItemInfo()
        {
        }

        public ListItemInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("Name") > -1)
                Name = ValidationHelper.GetString(dataRow["Name"], "");
            if (dataRow.Table.Columns.IndexOf("SeoTemplate") > -1)
                SeoTemplate = ValidationHelper.GetString(dataRow["SeoTemplate"], "");
            if (dataRow.Table.Columns.IndexOf("Description") > -1)
                Description = ValidationHelper.GetString(dataRow["Description"], "");
            if (dataRow.Table.Columns.IndexOf("ListId") > -1)
                ListId = ValidationHelper.GetInteger(dataRow["ListId"], 0);
            if (dataRow.Table.Columns.IndexOf("ListName") > -1)
                ListName = ValidationHelper.GetString(dataRow["ListName"], "");
            if (dataRow.Table.Columns.IndexOf("ParentId") > -1)
                ParentId = ValidationHelper.GetInteger(dataRow["ParentId"], 0);
            if (dataRow.Table.Columns.IndexOf("ParentName") > -1)
                ParentName = ValidationHelper.GetString(dataRow["ParentName"], "");

        }

        public static ListItemInfo[] GetArray(DataTable dataTable)
        {
            ListItemInfo[] result = new ListItemInfo[dataTable.Rows.Count];
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                result[i] = new ListItemInfo(dataTable.Rows[i]);
            }
            return result;
        }
    }
}
