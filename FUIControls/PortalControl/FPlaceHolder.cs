using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FVirtualPathProvider;

namespace FUIControls.PortalControl
{
    [ToolboxData("<fr:FPlaceHolder runat=\"server\" ID=\"FPlaceHolder1\" ></fr:FPlaceHolder>")]
    public class FPlaceHolder : PlaceHolder, INamingContainer
    {
        public void LoadPlaceHolder(PageInfo pageInfo, bool isSiteLayout)
        {
            //this.Controls.Clear();
            string path = FVirtualDirectories.MasterPages + "/" + pageInfo.SiteLayoutId + ".ascx";
            if (!isSiteLayout)
                path = FVirtualDirectories.Layouts + "/" + pageInfo.PageLayoutId + ".ascx";
            var layout = (FAbstractLayout)Page.LoadControl(path);
            layout.LoadLayout(pageInfo, isSiteLayout);
            Controls.Add(layout);
        }
    }
}
