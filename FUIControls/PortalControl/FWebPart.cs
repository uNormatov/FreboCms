using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;
using FCore.Class;
using FCore.Constant;
using FCore.Helper;

namespace FUIControls.PortalControl
{
    [ToolboxData("<fr:FWebPart runat=\"server\" ID=\"FWebPart1\" ></fr:FWebPart>")]
    public class FWebPart : AbstractControl
    {
        private Hashtable _properties;
        private string _blockSettings;

        public void SetProperty(string key, string value)
        {
            if (_properties.Contains(key))
                _properties[key] = value;
            else
                _properties.Add(key, value);
        }

        public string GetProperty(string key)
        {
            if (_properties.Contains(key))
                return Context.Server.HtmlDecode(_properties[key].ToString());
            return string.Empty;
        }

        private void FillProperties()
        {
            _properties = new Hashtable();
            if (!string.IsNullOrEmpty(_blockSettings))
            {
                XElement xdoc = XDocument.Parse(_blockSettings).Element("properties");
                if (xdoc != null)
                {
                    XElement[] props = xdoc.Elements("property").ToArray();
                    foreach (XElement element in props)
                    {
                        XAttribute xAttribute = element.Attribute("name");
                        if (xAttribute != null && !_properties.ContainsKey(xAttribute.Value))
                        {
                            XAttribute attribute = element.Attribute("name");
                            if (attribute != null)
                                _properties.Add(attribute.Value, element.Value);
                        }
                    }
                }
            }
        }

        protected virtual void LoadWebPart()
        {
        }

        public void LoadWebPart(PageInfo pageInfo, string blockSettings)
        {
            PageInfo = pageInfo;
            _blockSettings = blockSettings;

            FillProperties();
            LoadWebPart();
        }
    }
}
