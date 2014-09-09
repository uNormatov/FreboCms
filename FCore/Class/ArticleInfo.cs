using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FCore.Helper;

namespace FCore.Class
{
    public class ArticleInfo : ClassInfo
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string Text { get; set; }
        public string Language { get; set; }

        public int SiteLayoutId { get; set; }
        public string SiteLayoutZone { get; set; }
        public int SiteLayoutOrder { get; set; }
        public int SiteLayoutNBlockId { get; set; }
        public int PageLayoutId { get; set; }
        public string PageLayoutZone { get; set; }
        public int PageLayoutOrder { get; set; }
        public int PageLayoutNBlockId { get; set; }
        public int BlockId { get; set; }
        public int PageId { get; set; }
        public string PageZone { get; set; }
        public int PageOrder { get; set; }
        public int PageNBlockId { get; set; }

        public ArticleInfo()
        {
        }

        public ArticleInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("Title") > -1)
                Title = ValidationHelper.GetString(dataRow["Title"], "");
            if (dataRow.Table.Columns.IndexOf("Code") > -1)
                Code = ValidationHelper.GetString(dataRow["Code"], "");
            if (dataRow.Table.Columns.IndexOf("Text") > -1)
                Text = ValidationHelper.GetString(dataRow["Text"], "");
            if (dataRow.Table.Columns.IndexOf("Language") > -1)
                Language = ValidationHelper.GetString(dataRow["Language"], "");
            if (dataRow.Table.Columns.IndexOf("SiteLayoutId") > -1)
                SiteLayoutId = ValidationHelper.GetInteger(dataRow["SiteLayoutId"], 0);
            if (dataRow.Table.Columns.IndexOf("SiteLayoutZone") > -1)
                SiteLayoutZone = ValidationHelper.GetString(dataRow["SiteLayoutZone"], "");
            if (dataRow.Table.Columns.IndexOf("SiteLayoutOrder") > -1)
                SiteLayoutOrder = ValidationHelper.GetInteger(dataRow["SiteLayoutOrder"], 0);
            if (dataRow.Table.Columns.IndexOf("SiteLayoutNBlockId") > -1)
                SiteLayoutNBlockId = ValidationHelper.GetInteger(dataRow["SiteLayoutNBlockId"], 0);
            if (dataRow.Table.Columns.IndexOf("PageLayoutId") > -1)
                PageLayoutId = ValidationHelper.GetInteger(dataRow["PageLayoutId"], 0);
            if (dataRow.Table.Columns.IndexOf("PageLayoutZone") > -1)
                PageLayoutZone = ValidationHelper.GetString(dataRow["PageLayoutZone"], "");
            if (dataRow.Table.Columns.IndexOf("PageLayoutOrder") > -1)
                PageLayoutOrder = ValidationHelper.GetInteger(dataRow["PageLayoutOrder"], 0);
            if (dataRow.Table.Columns.IndexOf("PageLayoutNBlockId") > -1)
                PageLayoutNBlockId = ValidationHelper.GetInteger(dataRow["PageLayoutNBlockId"], 0);
            if (dataRow.Table.Columns.IndexOf("PageId") > -1)
                PageId = ValidationHelper.GetInteger(dataRow["PageId"], 0);
            if (dataRow.Table.Columns.IndexOf("BlockId") > -1)
                BlockId = ValidationHelper.GetInteger(dataRow["BlockId"], 0);
            if (dataRow.Table.Columns.IndexOf("PageZone") > -1)
                PageZone = ValidationHelper.GetString(dataRow["PageZone"], "");
            if (dataRow.Table.Columns.IndexOf("PageOrder") > -1)
                PageOrder = ValidationHelper.GetInteger(dataRow["PageOrder"], 0);
            if (dataRow.Table.Columns.IndexOf("PageNBlockId") > -1)
                PageNBlockId = ValidationHelper.GetInteger(dataRow["PageNBlockId"], 0);
            if (dataRow.Table.Columns.IndexOf("CreatedBy") > -1)
                CreatedBy = ValidationHelper.GetString(dataRow["CreatedBy"], "");
            if (dataRow.Table.Columns.IndexOf("CreatedDate") > -1)
                CreatedDate = ValidationHelper.GetDateTime(dataRow["CreatedDate"], DateTime.Now);
            if (dataRow.Table.Columns.IndexOf("ModifiedBy") > -1)
                ModifiedBy = ValidationHelper.GetString(dataRow["ModifiedBy"], "");
            if (dataRow.Table.Columns.IndexOf("ModifiedDate") > -1)
                ModifiedDate = ValidationHelper.GetDateTime(dataRow["ModifiedDate"], DateTime.Now);
        }
    }
}
