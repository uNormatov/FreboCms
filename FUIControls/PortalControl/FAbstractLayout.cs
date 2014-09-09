using System;
using System.Collections.Generic;
using FCore.Class;
using FUIControls.Helper;

namespace FUIControls.PortalControl
{
    public class FAbstractLayout : AbstractControl
    {
        public void LoadLayout(PageInfo pageInfo, bool isSiteLayout)
        {
            PageInfo = pageInfo;
            List<FWebPartZone> webPartZones = PortalHelper.GetWebPartZones(this);
            if (webPartZones != null && webPartZones.Count > 0)
            {
                for (int i = 0; i < webPartZones.Count; i++)
                {
                    if (isSiteLayout)
                    {
                        webPartZones[i].LoadWebPartZone(pageInfo, pageInfo.SiteBlocks);
                    }
                    else
                    {
                        webPartZones[i].LoadWebPartZone(pageInfo, pageInfo.PageLayoutBlocks);
                        webPartZones[i].LoadWebPartZone(pageInfo, pageInfo.PageBlocks);
                    }
                }
            }

            List<FPlaceHolder> placeHolders = PortalHelper.GetPlaceHolders(this);
            if (placeHolders != null && placeHolders.Count > 0)
            {
                for (int i = 0; i < placeHolders.Count; i++)
                {
                    placeHolders[i].LoadPlaceHolder(pageInfo, false);
                }
            }
        }
    }
}
