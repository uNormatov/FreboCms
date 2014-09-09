using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.PortalControl;
using FVirtualPathProvider;

namespace FUIControls.UIControl
{
    [ToolboxData("<fr:FEvaluableRepeater runat=\"server\" ID=\"FEvaluableRepeater1\"></<fr:FEvaluableRepeater>")]
    public class FEvaluableRepeater : Control
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

        public DataTable DataSourse { get; set; }

        public void LoadTemplates()
        {
            EnableViewState = false;
            if (DataSourse != null)
            {
                for (int i = 0; i < DataSourse.Rows.Count; i++)
                {
                    FAbstractEvaluableTransformation transformation =
                          (FAbstractEvaluableTransformation)Page.LoadControl(FVirtualDirectories.EvaluableTransformations + "/" + (i % 2 == 0 ? Transformation : AlternativeTransformation) +
                                              ".ascx");

                    transformation.DataItem = DataSourse.Rows[i];
                    transformation.DataIndex = i;
                    Controls.Add(transformation);
                }
            }
        }

        public void DataBind()
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
                    DataSourse = dataTable;
                    LoadTemplates();

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
