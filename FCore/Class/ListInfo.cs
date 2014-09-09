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
    public class ListInfo : ClassInfo
    {
        public ListInfo()
        {
        }

        public ListInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("Name") > -1)
                Name = ValidationHelper.GetString(dataRow["Name"], "");
            if (dataRow.Table.Columns.IndexOf("Description") > -1)
                Description = ValidationHelper.GetString(dataRow["Description"], "");
        }
    }
}
