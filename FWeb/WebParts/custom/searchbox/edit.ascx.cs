using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Collection;
using FCore.Helper;
using FUIControls.PortalControl;

namespace FWeb.WebParts.custom.searchbox
{
    public partial class edit : FWebPartEdit
    {
        public edit()
            : base("", null)
        {
        }

        public edit(string properties, ErrorInfoList errorInfoList)
            : base(properties, errorInfoList)
        {
        }

        protected override void GetValues()
        {

        }

        protected override void EnsureControlsValue()
        {

        }
    }
}