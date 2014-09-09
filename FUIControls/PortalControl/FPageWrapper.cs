using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;

using FCore.Class;
using FCore.Constant;
using FUIControls.Helper;

namespace FUIControls.PortalControl
{
    [ToolboxData("<fr:FPageWrapper runat=\"server\" ID=\"FPageWrapper1\"></fr:FPageWrapper>")]
    public class FPageWrapper : Control, INamingContainer
    {
        private PageInfo pageInfo;
        public PageInfo PageInfo
        {
            get
            {
                if (pageInfo == null && HttpContext.Current.Items.Contains(SiteConstants.PageData))
                    pageInfo = HttpContext.Current.Items[SiteConstants.PageData] as PageInfo;

                return pageInfo;
            }
        }

        public FPageWrapper()
        {
            Init += FPageWrapperInit;
        }

        void FPageWrapperInit(object sender, EventArgs e)
        {
            LoadPage();
        }

        private void LoadPage()
        {
            List<FPlaceHolder> placeHolders = PortalHelper.GetPlaceHolders(this.Page);
            if (placeHolders != null)
            {
                foreach (FPlaceHolder t in placeHolders)
                {
                    t.LoadPlaceHolder(PageInfo, true);
                }
            }
        }

    }
}
