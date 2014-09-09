using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FCore.Helper;

namespace FCore.Class
{
    [Serializable]
    public class CommentBoxInfo : ClassInfo
    {
        public string Email { get; set; }
        public string Website { get; set; }
        public string Body { get; set; }
        public int ContentTypeId;
        public string SeoTemplate { get; set; }
        public int Order { get; set; }
        public int ParentId { get; set; }
        public string Url { get; set; }
        public bool IsOk { get; set; }
        public string Time { get; set; }

        public CommentBoxInfo()
        {
        }

        public CommentBoxInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("Name") > -1)
                Name = ValidationHelper.GetString(dataRow["Name"], "");
            if (dataRow.Table.Columns.IndexOf("Email") > -1)
                Email = ValidationHelper.GetString(dataRow["Email"], "");
            if (dataRow.Table.Columns.IndexOf("Website") > -1)
                Website = ValidationHelper.GetString(dataRow["Website"], "");
            if (dataRow.Table.Columns.IndexOf("Body") > -1)
                Body = ValidationHelper.GetString(dataRow["Body"], "");
            if (dataRow.Table.Columns.IndexOf("ContentTypeId") > -1)
                ContentTypeId = ValidationHelper.GetInteger(dataRow["ContentTypeId"], 0);
            if (dataRow.Table.Columns.IndexOf("SeoTemplate") > -1)
                SeoTemplate = ValidationHelper.GetString(dataRow["SeoTemplate"], "");
            if (dataRow.Table.Columns.IndexOf("Order") > -1)
                Order = ValidationHelper.GetInteger(dataRow["Order"], 0);
            if (dataRow.Table.Columns.IndexOf("ParentId") > -1)
                ParentId = ValidationHelper.GetInteger(dataRow["ParentId"], 0);
            if (dataRow.Table.Columns.IndexOf("Url") > -1)
                Url = ValidationHelper.GetString(dataRow["Url"], "");
            if (dataRow.Table.Columns.IndexOf("CreatedDate") > -1)
                CreatedDate = ValidationHelper.GetDateTime(dataRow["CreatedDate"], DateTime.Today);
            if (CreatedDate.HasValue)
                Time = UzbKeywordHelper.GetDateString(CreatedDate.Value, DateTime.Now);
        }
    }
}

