using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Collection;
using FDataProvider;
using FVirtualPathProvider;

namespace FUIControls.UIControl
{
    [ToolboxData("<fr:FDataList runat=\"server\" ID=\"FDataList1\"></<fr:FDataList>")]
    public class FDataList : DataList
    {
        public string Transformation { get; set; }

        public string AlternativeTransformation { get; set; }

        public string RowBefore { get; set; }

        public string RowAfter { get; set; }

        public string QueryName { get; set; }

        public object[,] QueryParameters { get; set; }

        public void LoadTemplate()
        {
            if (!string.IsNullOrEmpty(Transformation))
            {
                string path = FVirtualDirectories.Transformations + "/" + Transformation + ".ascx";
                ItemTemplate = Page.LoadTemplate(path);
                //  ItemTemplate = Page.LoadTemplate("~/UserControls/test.ascx");
            }

            if (!string.IsNullOrEmpty(AlternativeTransformation))
            {
                string path = FVirtualDirectories.Transformations + "/" + AlternativeTransformation + ".ascx";
                AlternatingItemTemplate = Page.LoadTemplate(path);
            }
        }

        public override void DataBind()
        {
            GeneralConnection generalConnection = new GeneralConnection();
            DataTable dataTable = generalConnection.ExecuteDataTableQuery(QueryName, QueryParameters, new ErrorInfoList());
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                LoadTemplate();
                DataSource = dataTable;
                base.DataBind();
            }
        }


        protected override void RenderContents(HtmlTextWriter writer)
        {
            int count = Items.Count;
            for (int i = 0; i < count; i++)
            {
                writer.Write(RowBefore);
                for (int j = 0; j < RepeatColumns; j++)
                {
                    Items[i].RenderControl(writer);
                    i++;
                    if (i >= count)
                        break;
                }
                i--;
                writer.Write(RowAfter);
            }
        }
    }
}
