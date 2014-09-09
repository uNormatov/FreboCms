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
    public partial class MainMenu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ContentTypeProvider contentTypeProvider = new ContentTypeProvider();
            List<ContentTypeInfo> contentTypeInfos = contentTypeProvider.SelectAll(new ErrorInfoList());
            var htmlBuilder = new StringBuilder();

            if (contentTypeInfos != null)
            {
                int index = 0;
                foreach (ContentTypeInfo item in contentTypeInfos)
                {
                    htmlBuilder.AppendFormat(
                        @"<li class='node'><a href='/administrator/content/default.aspx?contenttypeid={0}' class='icon-16-content'>{1}</a><ul
                class='menu-component'><li><a href='/administrator/content/action.aspx?contenttypeid={0}&type=entry' class='icon-16-newarticle'>
                    Add New {1}</a></li></ul></li>", item.Id, item.Name);

                    if (index == contentTypeInfos.Count - 1)
                        htmlBuilder.Append("<li class=\"separator\"><span></span></li>");
                    index++;
                }
                ltlContents.Text = htmlBuilder.ToString();
            }
        }
    }
}