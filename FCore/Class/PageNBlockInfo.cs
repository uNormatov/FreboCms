using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FCore.Helper;

namespace FCore.Class
{
    public class PageNBlockInfo : ClassInfo
    {
        public int PageId { get; set; }
        public string WebPartZoneName { get; set; }
        public int BlockId { get; set; }
        public int Order { get; set; }
        public string Language { get; set; }

        public PageNBlockInfo()
        {
        }

        public PageNBlockInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("PageId") > -1)
                PageId = ValidationHelper.GetInteger(dataRow["PageId"], 0);
            if (dataRow.Table.Columns.IndexOf("WebPartZoneName") > -1)
                WebPartZoneName = ValidationHelper.GetString(dataRow["WebPartZoneName"], "");
            if (dataRow.Table.Columns.IndexOf("Language") > -1)
                Language = ValidationHelper.GetString(dataRow["Language"], "");
            if (dataRow.Table.Columns.IndexOf("BlockId") > -1)
                BlockId = ValidationHelper.GetInteger(dataRow["BlockId"], 0);
            if (dataRow.Table.Columns.IndexOf("Order") > -1)
                Order = ValidationHelper.GetInteger(dataRow["Order"], 0);
        }
    }
}