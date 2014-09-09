using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using FCore.Helper;

namespace FCore.Class.Poll
{
    public class PollChoiceInfo : ClassInfo
    {
        public int PollId { get; set; }
        [DataMember]
        public string Choice { get; set; }
        [DataMember]
        public int VoteCount { get; set; }

        public PollChoiceInfo()
        {
        }

        public PollChoiceInfo(DataRow row)
        {
            if (row.Table.Columns.Contains("PollId"))
                PollId = ValidationHelper.GetInteger(row["PollId"], 0);
            if (row.Table.Columns.Contains("Choice"))
                Choice = ValidationHelper.GetString(row["Choice"], string.Empty);
            if (row.Table.Columns.Contains("VoteCount"))
                VoteCount = ValidationHelper.GetInteger(row["VoteCount"], 0);
        }
    }
}
