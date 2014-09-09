using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Helper;
using FUIControls.PortalControl;
using FVirtualPathProvider;

namespace FWeb.WebParts.general.article
{
    public partial class webpart : FWebPart
    {
        protected int ArticleId
        {
            get { return ValidationHelper.GetInteger(GetProperty("ArticleId"), 0); }
        }

        protected override void LoadWebPart()
        {
            string path = FVirtualDirectories.Articles + "/" + ArticleId + ".ascx";
            var article = Page.LoadControl(path);
            Controls.Add(article);
        }
    }
}