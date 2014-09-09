using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.Helper;
using FUIControls.PortalControl;

namespace FWeb.WebParts.custom.tagcloud
{
    public partial class webpart : FWebPart
    {

        private SortedDictionary<string, int> _tags;
        private Dictionary<string, string> _urls;
        private int _totalCount = 0;
        protected override void LoadWebPart()
        {
            GetSordeDictionary();
            StringBuilder tagsBuilder = new StringBuilder();
            foreach (string key in _tags.Keys)
            {
                tagsBuilder.AppendFormat("<li><a class='{0}' href='/tag/{1}'>{2}</a></li>", GetCssClass(_tags[key], _totalCount),
                    _urls[key], key);
            }
            tagCloud.Text = tagsBuilder.ToString();
        }

        private void GetSordeDictionary()
        {
            _tags = new SortedDictionary<string, int>();
            _urls = new Dictionary<string, string>();
            using (GeneralConnection generalConnection = new GeneralConnection())
            {
                ErrorInfoList errors = new ErrorInfoList();
                DataTable dataTable = generalConnection.ExecuteDataTableQuery("n_maqola.tag_cloud_uchun", null,
                    QueryType.SqlQuery, errors);

                if (errors.Count == 0 && dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        _tags.Add(ValidationHelper.GetString(row["Name"], string.Empty), ValidationHelper.GetInteger(row["Count"], 0));
                        _urls.Add(ValidationHelper.GetString(row["Name"], string.Empty), ValidationHelper.GetString(row["SeoTemplate"], string.Empty));
                    }

                    dataTable = generalConnection.ExecuteDataTableQuery("n_maqola.all_published_count", null, QueryType.SqlQuery, errors);
                    if (errors.Count == 0 && dataTable.Rows.Count > 0)
                    {
                        _totalCount = ValidationHelper.GetInteger(dataTable.Rows[0][0], 0);
                    }
                    else
                    {
                        _totalCount = 0;
                    }
                }
            }

        }


        private string GetCssClass(int tagCount, int postCount)
        {
            int result = (tagCount * 100) / postCount;
            if (result <= 20)
                return "tag-size1";
            if (result <= 40)
                return "tag-size2";
            if (result <= 60)
                return "tag-size3";
            if (result <= 80)
                return "tag-size4";
            if (result <= 100)
                return "tag-size5";


            return string.Empty;
        }
    }
}