using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FCore.Helper;

namespace FCore.Class.Poll
{
    public class PollIpAddressInfo : ClassInfo
    {
        public int PollId { get; set; }
        public string IpAddress { get; set; }
        public int ChoiceId { get; set; }

        public PollIpAddressInfo()
        {
        }

        public PollIpAddressInfo(DataRow row)
        {
            if (row.Table.Columns.Contains("PollId"))
                PollId = ValidationHelper.GetInteger(row["PollId"], 0);
            if (row.Table.Columns.Contains("IpAddress"))
                IpAddress = ValidationHelper.GetString(row["IpAddress"], string.Empty);
            if (row.Table.Columns.Contains("ChoiceId"))
                ChoiceId = ValidationHelper.GetInteger(row["ChoiceId"], 0);

        }
    }
}
