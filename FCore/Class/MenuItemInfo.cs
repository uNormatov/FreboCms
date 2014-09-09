using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FCore.Helper;

namespace FCore.Class
{
    public class MenuItemInfo : ClassInfo
    {
        public string Title { get; set; }
        public string ParentTitle { get; set; }
        public string Url { get; set; }
        public int MenuId { get; set; }
        public int ParentId { get; set; }
        public int OpenType { get; set; }
        public string OpenTypeName { get; set; }
        public bool IsMain { get; set; }

        public MenuItemInfo()
        {
            IsMain = false;
        }

        public MenuItemInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("Title") > -1)
                Title = ValidationHelper.GetString(dataRow["Title"], "");
            if (dataRow.Table.Columns.IndexOf("Url") > -1)
                Url = ValidationHelper.GetString(dataRow["Url"], "");
            if (dataRow.Table.Columns.IndexOf("ParentTitle") > -1)
                ParentTitle = ValidationHelper.GetString(dataRow["ParentTitle"], "");
            if (dataRow.Table.Columns.IndexOf("MenuId") > -1)
                MenuId = ValidationHelper.GetInteger(dataRow["MenuId"], 0);
            if (dataRow.Table.Columns.IndexOf("ParentId") > -1)
                ParentId = ValidationHelper.GetInteger(dataRow["ParentId"], 0);
            if (dataRow.Table.Columns.IndexOf("OpenType") > -1)
                OpenType = ValidationHelper.GetInteger(dataRow["OpenType"], 0);
            if (dataRow.Table.Columns.IndexOf("IsPublished") > -1)
                IsPublished = ValidationHelper.GetBoolean(dataRow["IsPublished"], false);
            if (dataRow.Table.Columns.IndexOf("IsDeleted") > -1)
                IsDeleted = ValidationHelper.GetBoolean(dataRow["IsDeleted"], false);
            if (OpenType == 0)
                OpenTypeName = "Same Tab";
            else if (OpenType == 1)
                OpenTypeName = "New Tab";
            else
            {
                OpenTypeName = "New Window";
            }
        }

    }
}

