using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FVirtualPathProvider
{
    public enum VirtualDirectoryType : int
    {
        Unknown = 1,
        Transformation = 2,
        Layout = 3,
        WebPart = 4,
        MasterPage = 5,
        Article = 6,
        EvaluableTransformation
    }
}
