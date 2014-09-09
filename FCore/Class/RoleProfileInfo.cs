using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using FCore.Helper;

namespace FCore.Class
{
    [Serializable]
    public class RoleProfileInfo : ClassInfo
    {
        public string RoleId { get; set; }
        public int ContentTypeId { get; set; }
        public string UserProfileQuery { get; set; }

        public RoleProfileInfo()
        {
        }

        public RoleProfileInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("RoleId") > -1)
                RoleId = ValidationHelper.GetString(dataRow["RoleId"], "");
            if (dataRow.Table.Columns.IndexOf("ContentTypeId") > -1)
                ContentTypeId = ValidationHelper.GetInteger(dataRow["ContentTypeId"], 0);
            if (dataRow.Table.Columns.IndexOf("UserProfileQuery") > -1)
                UserProfileQuery = ValidationHelper.GetString(dataRow["UserProfileQuery"], string.Empty);
        }
    }
}
