using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using FCore.Enum;
using FCore.Helper;

namespace FCore.Class.Poll
{
    public class PollInfo : ClassInfo
    {
        [DataMember]
        public string Question { get; set; }
        public int BlockMode { get; set; }
        public string BlockModeName { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveName { get; set; }

        public PollInfo()
        {
        }

        public PollInfo(DataRow row)
        {
            if (row.Table.Columns.Contains("Id"))
                Id = ValidationHelper.GetInteger(row["Id"], 0);
            if (row.Table.Columns.Contains("Question"))
                Question = ValidationHelper.GetString(row["Question"], string.Empty);

            if (row.Table.Columns.Contains("BlockMode"))
                BlockMode = ValidationHelper.GetInteger(row["Question"], 1);

            if (row.Table.Columns.Contains("IsActive"))
                IsActive = ValidationHelper.GetBoolean(row["IsActive"], false);

            IsActiveName = IsActive ? "Yes" : "No";
            switch (BlockMode)
            {
                case (int)PollBlockMode.Cookie:
                    BlockModeName = "Cookie";
                    break;
                case (int)PollBlockMode.IpAddress:
                    BlockModeName = "IpAddress";
                    break;
                default:
                    BlockModeName = "Dont Block";
                    break;
            }
        }
    }
}
