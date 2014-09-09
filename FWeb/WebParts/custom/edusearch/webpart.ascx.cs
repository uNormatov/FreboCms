using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FUIControls.PortalControl;

namespace FWeb.WebParts.custom.edusearch
{
    public partial class webpart : FWebPart
    {
        #region Methods
        protected override void LoadWebPart()
        {

            string value = GetQueryValue("soz", string.Empty);

            if (!string.IsNullOrEmpty(value))
            {
                StringBuilder script = new StringBuilder();
                script.Append("<script>$(document).ready(function(){");
                script.AppendFormat("$(\"#txtSearch\").val(decodeURI(\"{0}\"));", value);
                script.Append("});</script>");
                Page.ClientScript.RegisterStartupScript(typeof(FWebPart), "restoresearchvalue", script.ToString());

            }
        }

        #endregion
    }
}