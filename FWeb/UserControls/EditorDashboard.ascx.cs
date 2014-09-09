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
    public partial class EditorDashboard : System.Web.UI.UserControl
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
                        @"<div class='icon-wrapper'>
            <div class='icon'>
                <a href='/administrator/content/default.aspx?contenttypeid={0}'>
                    <img alt='{1}' src='/content/images/menu/content-48.png'>
                    <span>{1}</span></a>
            </div>
        </div>", item.Id, item.Name);
                }
                ltlMain.Text = htmlBuilder.ToString();
            }

        }
    }
}