using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;
using FCore.Class;
using FCore.Collection;
using FCore.Constant;
using FCore.Enum;
using FCore.Helper;
using FUIControls.Helper;
using FUIControls.PortalControl;
using FUIControls.Settings;

namespace FUIControls.FormControl
{
    public abstract class AbsractBasicControl : AbstractControl, IValidatable
    {
        public string FieldName { get; set; }

        public virtual string Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        public virtual string CssClass { get; set; }

        public virtual FormControlViewMode ViewMode
        {
            get
            {
                object o = ViewState["__view_mode"];
                if (o == null)
                    return FormControlViewMode.Editor;
                return (FormControlViewMode)o;
            }
            set { ViewState["__view_mode"] = value; }
        }

        public virtual void SetValue(string value)
        {
        }

        public virtual void SetOptions(string xmloptions)
        {
        }

        public virtual string GetValue()
        {
            return "";
        }

        public virtual bool IsValid
        {
            get
            {
                object o = ViewState["__is_valid"];
                if (o == null)
                    return true;
                return (bool)o;
            }
            set { ViewState["__is_valid"] = value; }
        }

        public virtual bool IsRequired
        {
            get
            {
                object o = ViewState["__is_required"];
                if (o == null)
                    return false;
                return (bool)o;
            }
            set { ViewState["__is_required"] = value; }
        }

        public virtual string RequiredErrorMessage
        {
            get
            {
                object o = ViewState["__required_error_message"];
                if (o == null)
                    return "";
                if (CoreSettings.CurrentSite.IsMultilanguage)
                    return LanguageHelper.Instance.GetTranslateByPattern(GetCurrentLanguage(), o.ToString());
                return o.ToString();
            }
            set { ViewState["__required_error_message"] = value; }
        }

        public virtual string RegularExpression
        {
            get
            {
                object o = ViewState["__regular_expression"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["__regular_expression"] = value; }
        }

        public virtual string RegularExpressionErrorMessage
        {
            get
            {
                object o = ViewState["__regular_expression_error_message"];
                if (o == null)
                    return "";
                if (CoreSettings.CurrentSite.IsMultilanguage)
                    return LanguageHelper.Instance.GetTranslateByPattern(GetCurrentLanguage(), o.ToString());
                return o.ToString();
            }
            set { ViewState["__regular_expression_error_message"] = value; }
        }

        public virtual bool Validate()
        {
            return true;
        }

        protected virtual void EnsureControls()
        {
        }

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

        /// <summary>
        /// XML formatida kelgan optionlarni Dictionaryga o'girib beradi.
        /// </summary>
        protected Dictionary<string, string> GetOptionsFromXml(string xmloptions)
        {
            xmloptions = string.Format("<options>{0}</options>", xmloptions);
            var result = new Dictionary<string, string>();

            XElement element = XElement.Parse(xmloptions);
            foreach (XElement item in element.Elements())
            {
                result.Add(item.Name.ToString(), item.Value);
            }
            return result;
        }

        /// <summary>
        /// Dictionary formatida kelgan optionlarni XMLga o'girib beradi.
        /// </summary>
        protected string GetXmlFromOptions(Dictionary<string, string> options)
        {
            var xmlresult = new StringBuilder();
            foreach (var item in options)
            {
                xmlresult.AppendFormat("<{0}>{1}</{0}>", item.Key, item.Value);
            }
            return xmlresult.ToString();
        }

        public virtual string[] ErrorMessage
        {
            get
            {
                object o = ViewState["__error_messege"];
                if (o == null)
                    return new string[0];
                return o as string[];
            }
            set { ViewState["__error_messege"] = value; }
        }

        public ErrorInfoList ErrorInfoList { get; set; }

        protected void RegisterError(ErrorInfo error)
        {
            if (ErrorInfoList == null)
                ErrorInfoList = new ErrorInfoList();
            ErrorInfoList.Add(error);
        }
    }
}
