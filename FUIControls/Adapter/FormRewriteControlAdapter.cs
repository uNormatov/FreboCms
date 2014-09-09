using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace FUIControls.Adapter
{
   public class FormRewriteControlAdapter : System.Web.UI.Adapters.ControlAdapter
    {
        /// <summary>
        /// Override the render method
        /// </summary>
        /// <param name="writer">The text writer</param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(new RewriteFormHtmlTextWriter(writer));
        }
    }
}
