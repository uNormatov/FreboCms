using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FVirtualPathProvider
{
    public class VirtualPathHelper
    {
        public static string GetFileName(string virtualpath)
        {
            int sindex = virtualpath.LastIndexOf("/") + 1;
            int findex = virtualpath.LastIndexOf(".");
            return virtualpath.Substring(sindex, findex - sindex);
        }
    }
}
