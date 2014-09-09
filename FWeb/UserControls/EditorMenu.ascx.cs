using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FDataProvider;

namespace FWeb.UserControls
{
    public partial class EditorMenu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ContentTypeProvider contentTypeProvider = new ContentTypeProvider();
            List<ContentTypeInfo> contentTypeInfos = contentTypeProvider.SelectAll(new ErrorInfoList());

            if (contentTypeInfos != null)
            {
                var htmlBuilder = new StringBuilder();

                foreach (ContentTypeInfo item in contentTypeInfos)
                {
                    htmlBuilder.AppendFormat(
                        @"<li class='node'><a href='/administrator/content/default.aspx?contenttypeid={0}'>{1}</a></li>", item.Id, item.Name);
                }
                ltlContentMenu.Text = htmlBuilder.ToString();
                htmlBuilder.Clear();
                foreach (ContentTypeInfo item in contentTypeInfos)
                {
                    htmlBuilder.AppendFormat(
                        @"<li class='disabled'><a>{0}</a> </li>", item.Name);
                }
                ltlHiddenMenu.Text = htmlBuilder.ToString();
            }
        }
    }
}