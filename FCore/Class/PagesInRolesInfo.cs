using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FCore.Helper;

namespace FCore.Class
{
    public class PagesInRolesInfo : ClassInfo
    {
        public int PageId { get; set; }
        public string RoleId { get; set; }

        public PagesInRolesInfo()
        {
        }

        public PagesInRolesInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("PageId") > -1)
                PageId = ValidationHelper.GetInteger(dataRow["PageId"], 0);
            if (dataRow.Table.Columns.IndexOf("RoleId") > -1)
                RoleId = ValidationHelper.GetString(dataRow["RoleId"], "");
        }
    }
}

