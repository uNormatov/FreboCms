using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FCore.Helper;

namespace FCore.Class
{
    public class MenuItemsInRolesInfo : ClassInfo
    {
        public int MenuItemId { get; set; }
        public string RoleId { get; set; }

        public MenuItemsInRolesInfo()
        {
        }

        public MenuItemsInRolesInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("MenuItemId") > -1)
                MenuItemId = ValidationHelper.GetInteger(dataRow["MenuItemId"], 0);
            if (dataRow.Table.Columns.IndexOf("RoleId") > -1)
                RoleId = ValidationHelper.GetString(dataRow["RoleId"], "");
        }
    }
}

