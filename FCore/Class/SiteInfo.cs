using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FCore.Helper;

namespace FCore.Class
{
    [Serializable]
    public class SiteInfo : ClassInfo
    {
        public SiteInfo() { }

        public SiteInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("Name") > -1)
                Name = ValidationHelper.GetString(dataRow["Name"], "");
            if (dataRow.Table.Columns.IndexOf("RootFolder") > -1)
                RootFolder = ValidationHelper.GetString(dataRow["RootFolder"], "");
            if (dataRow.Table.Columns.IndexOf("DefaultPageId") > -1)
                DefaultPageId = ValidationHelper.GetInteger(dataRow["DefaultPageId"], 0);
            if (dataRow.Table.Columns.IndexOf("NotFoundPageId") > -1)
                NotFoundPageId = ValidationHelper.GetInteger(dataRow["NotFoundPageId"], 0);
            if (dataRow.Table.Columns.IndexOf("IsMultilanguage") > -1)
                IsMultilanguage = ValidationHelper.GetBoolean(dataRow["IsMultilanguage"], false);
            if (dataRow.Table.Columns.IndexOf("DefaultLanguage") > -1)
                DefaultLanguage = ValidationHelper.GetString(dataRow["DefaultLanguage"], "");
            if (dataRow.Table.Columns.IndexOf("ArticleWebpartId") > -1)
                ArticleWebpartId = ValidationHelper.GetInteger(dataRow["ArticleWebpartId"], 0);

            if (dataRow.Table.Columns.IndexOf("ModifiedDate") > -1)
                CreatedDate = ValidationHelper.GetDateTime(dataRow["ModifiedDate"], DateTime.Today);
            if (dataRow.Table.Columns.IndexOf("CreatedBy") > -1)
                CreatedBy = ValidationHelper.GetString(dataRow["CreatedBy"], "");
            if (dataRow.Table.Columns.IndexOf("CreatedDate") > -1)
                CreatedDate = ValidationHelper.GetDateTime(dataRow["CreatedBy"], DateTime.Today);
            if (dataRow.Table.Columns.IndexOf("ModifiedBy") > -1)
                ModifiedBy = ValidationHelper.GetString(dataRow["CreatedBy"], "");
            if (dataRow.Table.Columns.IndexOf("ModifiedDate") > -1)
                CreatedDate = ValidationHelper.GetDateTime(dataRow["ModifiedDate"], DateTime.Today);
        }

        public int DefaultPageId { get; set; }
        public int NotFoundPageId { get; set; }
        public string RootFolder { get; set; }
        public bool IsMultilanguage { get; set; }
        public string DefaultLanguage { get; set; }
        public int ArticleWebpartId { get; set; }
    }
}

