using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Collection;

namespace FWeb
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorInfoList errorInfoList = ((ErrorInfoList)HttpContext.Current.Items["page_data"]);

            for (int i = 0; i < errorInfoList.Count; i++)
            {
                ltlErrorMessage.Text += errorInfoList[i].Message + "</br>";
            }

        }
    }
}