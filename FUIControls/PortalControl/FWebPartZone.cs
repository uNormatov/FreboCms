using System.Collections.Generic;
using System.Web.UI;
using FCore.Class;
using FCore.Constant;

namespace FUIControls.PortalControl
{
    [ToolboxData("<fr:FWebPartZone runat=\"server\" ID=\"FWebPartZone1\" ></fr:FWebPartZone>")]
    public class FWebPartZone : AbstractControl, INamingContainer
    {
        public string Name { get; set; }

        public void LoadWebPartZone(PageInfo pageInfo, List<BlockInfo> blocks)
        {
            if (blocks != null)
            {
                for (int i = 0; i < blocks.Count; i++)
                {
                    BlockInfo block = blocks[i];
                    if (string.Compare(block.WebPartZoneName, ID, true) == 0 &&
                     (string.IsNullOrWhiteSpace(block.Language) || block.Language.ToLower().Equals(GetCurrentLanguage()) || block.Language.Equals(SiteConstants.All)))
                    {
                        var webpart =
                            (FWebPart)Page.LoadControl(string.Format("{0}/webpart.ascx", block.WebPartFolderPath));
                        webpart.PageInfo = pageInfo;
                        webpart.LoadWebPart(pageInfo, block.Properties);
                        Controls.Add(webpart);
                    }
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}
