using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;
using FCore.Collection;

namespace FUIControls.PortalControl
{
    public abstract class FWebPartEdit : UserControl
    {
        protected ErrorInfoList ErrorInfoList { get; set; }

        protected FWebPartEdit()
        {
        }

        protected FWebPartEdit(string properties, ErrorInfoList errorInfoList)
        {
            if (_properties == null)
                _properties = new Hashtable();

            ErrorInfoList = errorInfoList ?? new ErrorInfoList();

            if (!string.IsNullOrEmpty(properties))
            {
                SetValues(properties);
            }
        }

        private Hashtable _properties;

        public Hashtable Properties
        {
            get
            {
                if (_properties == null)
                {
                    _properties = new Hashtable();
                }
                return _properties;
            }
            set { _properties = value; }
        }

        public virtual object GetValue(string key)
        {
            if (Properties.Contains(key))
                return HttpContext.Current.Server.HtmlDecode(Properties[key].ToString());
            return null;
        }

        public virtual void SetValue(string key, object value)
        {
            if (Properties.Contains(key))
                Properties[key] = value;
            else
                Properties.Add(key, value);
        }

        public void SetValues(string properties)
        {
            XElement xdoc = XDocument.Parse(properties).Element("properties");
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

        public string GetAttributes()
        {
            try
            {
                var webpart = new XElement("properties");
                if (Properties != null && Properties.Count > 0)
                {
                    foreach (object item in Properties.Keys)
                    {
                        webpart.Add(new XElement("property", new XAttribute("name", item), Properties[item]));
                    }
                    return webpart.ToString();
                }
            }
            catch
            {
                return "";
            }
            return "";
        }

        protected override void OnInit(EventArgs e)
        {
            EnableViewState = true;
            if (_properties != null && _properties.Count > 0)
                EnsureControlsValue();

            base.OnInit(e);

            GetValues();
        }

        protected abstract void GetValues();

        protected override void CreateChildControls()
        {
            EnsureControlsValue();
            base.CreateChildControls();
        }

        protected abstract void EnsureControlsValue();

        protected virtual string GetControlValue(string controlid)
        {
            string[] values = Context.Request.Form.AllKeys;
            foreach (string item in values)
            {
                if (item.Contains(controlid))
                {
                    controlid = item;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(controlid) && Context.Request.Form.AllKeys.Contains(controlid))
                return Context.Request.Form[controlid];

            return string.Empty;
        }

        protected virtual Dictionary<string, string> GetControlValues(string startWith)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string[] values = Context.Request.Form.AllKeys;
            foreach (string item in values)
            {
                if (item.StartsWith(startWith))
                {
                    dictionary.Add(item, Context.Request.Form[item]);
                }
            }
            return dictionary;
        }
    }
}
