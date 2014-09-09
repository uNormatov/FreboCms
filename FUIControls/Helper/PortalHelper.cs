using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Collections;
using FUIControls.PortalControl;

namespace FUIControls.Helper
{
    public class PortalHelper
    {
        public static List<FPlaceHolder> GetPlaceHolders(Control parent)
        {
            if (parent == null) return null;

            List<FPlaceHolder> result = null;

            foreach (Control item in parent.Controls)
            {
                if (item is FPlaceHolder)
                {
                    if (result == null)
                        result = new List<FPlaceHolder>();

                    result.Add((FPlaceHolder)item);
                }
                else
                {
                    List<FPlaceHolder> innerList = GetPlaceHolders(item);
                    if (innerList != null && innerList.Count > 0)
                    {
                        if (result == null)
                            result = new List<FPlaceHolder>();
                        result.AddRange(innerList);
                    }
                }
            }
            return result;
        }

        public static List<FWebPartZone> GetWebPartZones(Control parent)
        {
            if (parent == null) return null;

            List<FWebPartZone> result = null;

            foreach (Control item in parent.Controls)
            {
                if (item is FWebPartZone)
                {
                    if (result == null)
                        result = new List<FWebPartZone>();

                    result.Add((FWebPartZone)item);
                }
                else
                {
                    List<FWebPartZone> innerList = GetWebPartZones(item);
                    if (innerList != null && innerList.Count > 0)
                    {
                        if (result == null)
                            result = new List<FWebPartZone>();
                        result.AddRange(innerList);
                    }
                }
            }
            return result;
        }
    }
}
