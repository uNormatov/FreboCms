using System;
using System.Text;
using FCore.Class;

namespace FWeb.UserControls
{
    public partial class ContentHeaderTemplate : System.Web.UI.UserControl
    {
        private FieldInfo[] _fields;

        public ContentHeaderTemplate(FieldInfo[] fields)
        {
            _fields = fields;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_fields != null)
            {
                StringBuilder htmlBuilder = new StringBuilder();
                foreach (FieldInfo item in _fields)
                {
                    htmlBuilder.AppendFormat("<th>{0}</th>", item.DisplayName);
                }
                ltlHeaderList.Text = htmlBuilder.ToString();
            }
        }
    }
}