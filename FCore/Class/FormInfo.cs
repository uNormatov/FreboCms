using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Data;
using FCore.Helper;

namespace FCore.Class
{
    [Serializable]
    [DataContract]
    public class FormInfo : ClassInfo
    {
        [DataMember]
        public string DisplayName { get; set; }
        public string Layout { get; set; }
        public int ContentTypeId { get; set; }

        public FormInfo()
        {
        }

        public FormInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("Name") > -1)
                Name = ValidationHelper.GetString(dataRow["Name"], "");
            if (dataRow.Table.Columns.IndexOf("DisplayName") > -1)
                DisplayName = ValidationHelper.GetString(dataRow["DisplayName"], "");
            if (dataRow.Table.Columns.IndexOf("ContentTypeId") > -1)
                ContentTypeId = ValidationHelper.GetInteger(dataRow["ContentTypeId"], 0);
            if (dataRow.Table.Columns.IndexOf("Layout") > -1)
                Layout = ValidationHelper.GetString(dataRow["Layout"], "");

        }

    }
}
