using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;

namespace FUIControls.Adapter
{
    /// <summary>
    /// The internal text writer to use in FormRewriteControlAdapter
    /// </summary>
    public class RewriteFormHtmlTextWriter : HtmlTextWriter
    {
        /// <summary>
        /// Initializes a new instance of the RewriteFormHtmlTextWriter class
        /// </summary>
        /// <param name="writer">Html text writer</param>
        public RewriteFormHtmlTextWriter(HtmlTextWriter writer)
            : base(writer)
        {
            this.InnerWriter = writer.InnerWriter;
        }

        /// <summary>
        /// Initializes a new instance of the RewriteFormHtmlTextWriter class
        /// </summary>
        /// <param name="writer">An IO text writer</param>
        public RewriteFormHtmlTextWriter(System.IO.TextWriter writer)
            : base(writer)
        {
            this.InnerWriter = writer;
        }

        /// <summary>
        /// Do the actual attribute writing of the action attribute
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="value">The value of the attribute</param>
        /// <param name="fEncode">Dont know</param>
        public override void WriteAttribute(string name, string value, bool fEncode)
        {
            if (name.Equals("action"))
            {
                var context = HttpContext.Current;

                if (context.Items["ActionAlreadyWritten"] == null)
                {
                    // Set the action to the raw url
                    value = context.Request.RawUrl;
                    context.Items["ActionAlreadyWritten"] = true;
                }
            }

            base.WriteAttribute(name, value, fEncode);
        }
    }
}
