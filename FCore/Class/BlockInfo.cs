using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FCore.Helper;

namespace FCore.Class
{
    public class BlockInfo : ClassInfo
    {
        public string Domain { get; set; }
        public string SeoTemplate { get; set; }
        public string Seo { get; set; }
        public string QueryString { get; set; }
        public string RootUrl { get; set; }

        public string Language { get; set; }
        public int LayoutNBlockId { get; set; }
        public int PageNBlockId { get; set; }
        public int WebPartId { get; set; }
        public string Properties { get; set; }
        public string WebPartZoneName { get; set; }
        public int Order { get; set; }
        public string WebPartFolderPath { get; set; }
        public WebPartInfo WebPart { get; set; }

        public BlockInfo() { }

        public BlockInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("LayoutNBlockId") > -1)
                LayoutNBlockId = ValidationHelper.GetInteger(dataRow["LayoutNBlockId"], 0);
            if (dataRow.Table.Columns.IndexOf("PageNBlockId") > -1)
                PageNBlockId = ValidationHelper.GetInteger(dataRow["PageNBlockId"], 0);
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("Name") > -1)
                Name = ValidationHelper.GetString(dataRow["Name"], "");
            if (dataRow.Table.Columns.IndexOf("Description") > -1)
                Description = ValidationHelper.GetString(dataRow["Description"], "");
            if (dataRow.Table.Columns.IndexOf("Properties") > -1)
                Properties = ValidationHelper.GetString(dataRow["Properties"], "");
            if (dataRow.Table.Columns.IndexOf("WebPartZoneName") > -1)
                WebPartZoneName = ValidationHelper.GetString(dataRow["WebPartZoneName"], "");
            if (dataRow.Table.Columns.IndexOf("WebPartId") > -1)
                WebPartId = ValidationHelper.GetInteger(dataRow["WebPartId"], 0);
            if (dataRow.Table.Columns.IndexOf("Order") > -1)
                Order = ValidationHelper.GetInteger(dataRow["Order"], 0);
            if (dataRow.Table.Columns.IndexOf("FolderPath") > -1)
                WebPartFolderPath = ValidationHelper.GetString(dataRow["FolderPath"], "");
            if (dataRow.Table.Columns.IndexOf("Language") > -1)
                Language = ValidationHelper.GetString(dataRow["Language"], "");
        }
    }
}
