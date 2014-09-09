using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FCore.Helper;

namespace FCore.Class
{
    public class LayoutWebPartZoneInfo : ClassInfo
    {
        public int LayoutId { get; set; }
        public LayoutWebPartZoneInfo() { }
        public LayoutWebPartZoneInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("Name") > -1)
                Name = ValidationHelper.GetString(dataRow["Name"], "");
            if (dataRow.Table.Columns.IndexOf("LayoutId") > -1)
                LayoutId = ValidationHelper.GetInteger(dataRow["LayoutId"], 0);
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is LayoutWebPartZoneInfo)
            {

                return this.Name.Equals(((LayoutWebPartZoneInfo)obj).Name);
            }
            return false;

        }
    }
}
