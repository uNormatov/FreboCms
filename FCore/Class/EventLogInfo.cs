using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FCore.Helper;

namespace FCore.Class
{
    [Serializable]
    public class EventLogInfo : ClassInfo
    {

        public int PageId { get; set; }
        public string FullUrl { get; set; }
        public string PageUrl { get; set; }
        public string ReferalUrl { get; set; }
        public string Ip { get; set; }
        public string UserName { get; set; }
        public string UserAgent { get; set; }
        public DateTime Date { get; set; }

        public EventLogInfo()
        {
        }

        public EventLogInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("PageId") > -1)
                PageId = ValidationHelper.GetInteger(dataRow["PageId"], 0);
            if (dataRow.Table.Columns.IndexOf("FullUrl") > -1)
                FullUrl = ValidationHelper.GetString(dataRow["FullUrl"], string.Empty);
            if (dataRow.Table.Columns.IndexOf("PageUrl") > -1)
                PageUrl = ValidationHelper.GetString(dataRow["PageUrl"], string.Empty);
            if (dataRow.Table.Columns.IndexOf("ReferalUrl") > -1)
                ReferalUrl = ValidationHelper.GetString(dataRow[""], string.Empty);
            if (dataRow.Table.Columns.IndexOf("Ip") > -1)
                Ip = ValidationHelper.GetString(dataRow["Ip"], string.Empty);
            if (dataRow.Table.Columns.IndexOf("UserName") > -1)
                UserName = ValidationHelper.GetString(dataRow["UserName"], string.Empty);
            if (dataRow.Table.Columns.IndexOf("UserAgent") > -1)
                UserAgent = ValidationHelper.GetString(dataRow["UserAgent"], string.Empty);
            if (dataRow.Table.Columns.IndexOf("Date") > -1)
                Date = ValidationHelper.GetDateTime(dataRow["Date"], DateTime.Now);

        }

    }
}
