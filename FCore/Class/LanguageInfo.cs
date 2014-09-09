using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FCore.Helper;

namespace FCore.Class
{
    public class LanguageInfo : ClassInfo
    {
        public string Code { get; set; }
        public bool IsDefault { get; set; }

        public LanguageInfo()
        {
        }

        public LanguageInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("Name") > -1)
                Name = ValidationHelper.GetString(dataRow["Name"], "");
            if (dataRow.Table.Columns.IndexOf("Code") > -1)
                Code = ValidationHelper.GetString(dataRow["Code"], "");
        }
    }
}
