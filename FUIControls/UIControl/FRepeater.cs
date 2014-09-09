using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FVirtualPathProvider;

namespace FUIControls.UIControl
{
    [ToolboxData("<fr:FRepeater runat=\"server\" ID=\"FRepeater1\"></<fr:FRepeater>")]
    public class FRepeater : Repeater
    {
        private QueryType _queryType = QueryType.SqlQuery;

        public string Transformation { get; set; }

        public string AlternativeTransformation { get; set; }

        public QueryType QueryType
        {
            set { _queryType = value; }
        }
            
        public string QueryName { get; set; }

        public object[,] QueryParameters { get; set; }

        public void LoadTemplate()
        {
            this.EnableViewState = false;
            if (!string.IsNullOrEmpty(Transformation))
            {
                string path = FVirtualDirectories.Transformations + "/" + Transformation + ".ascx";
                ItemTemplate = Page.LoadTemplate(path);
            }

            if (!string.IsNullOrEmpty(AlternativeTransformation))
            {
                string path = FVirtualDirectories.Transformations + "/" + AlternativeTransformation + ".ascx";
                AlternatingItemTemplate = Page.LoadTemplate(path);
            }
        }

        public override void DataBind()
        {
            using (GeneralConnection generalConnection = new GeneralConnection())
            {
                ErrorInfoList errors = new ErrorInfoList();
                string key = GeneratCacheKey();
                DataTable dataTable = null;
                if (CacheHelper.Contains(key))
                    dataTable = (DataTable)CacheHelper.Get(key);

                if (dataTable == null)
                {
                    dataTable = generalConnection.ExecuteDataTableQuery(QueryName, QueryParameters, _queryType, errors);
                    if (dataTable != null)
                        CacheHelper.Add(key, dataTable);
                }

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    DataSource = dataTable;
                    LoadTemplate();
                    base.DataBind();
                }
            }
        }

        private string GeneratCacheKey()
        {
            StringBuilder keyBuilder = new StringBuilder();
            keyBuilder.Append(QueryName);

            if (QueryParameters != null)
            {
                for (int i = 0; i < QueryParameters.GetLongLength(0); i++)
                {
                    keyBuilder.AppendFormat("_{0}_{1}", QueryParameters[i, 0], QueryParameters[i, 1]);
                }
            }
            return keyBuilder.ToString();
        }
    }
}
