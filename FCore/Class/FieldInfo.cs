using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FCore.Enum;
using FCore.Helper;

namespace FCore.Class
{
    [Serializable]
    public class FieldInfo : ClassInfo
    {
        public FormFieldType FieldType { get; set; }
        public string FieldTypeName { get; set; }
        public DataFieldType DataType { get; set; }
        public string DataTypeName { get; set; }
        public string DisplayName { get; set; }
        public bool Enabled { get; set; }
        public bool IsAllowNull { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool UseAsSeoTemplate { get; set; }
        public bool ShowInListing { get; set; }
        public int Size { get; set; }
        public string DefaultValue { get; set; }
        public int SortOrder { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public string Options { get; set; }


        public bool IsRequired { get; set; }
        public string RequiredErrorMessage { get; set; }
        public string RegularExpression { get; set; }
        public string RegExpErrorMessage { get; set; }
        public bool IsCompareAble { get; set; }
        public string CompareWith { get; set; }
        public string CompareErrorMessage { get; set; }
        public string CustomFormControlName { get; set; }


        public FieldInfo()
        {
        }

        public FieldInfo(XElement element)
        {
            if (element.Element("name") != null)
                Name = element.Element("name").Value;

            if (element.Element("fieldtype") != null)
            {
                FieldType =
                    (FormFieldType)System.Enum.Parse(typeof(FormFieldType), element.Element("fieldtype").Value, true);
                FieldTypeName = FormHelper.GetFieldCodeByType(FieldType);
            }
            if (element.Element("customformcontrolname") != null)
                CustomFormControlName = element.Element("customformcontrolname").Value;

            if (element.Element("datatype") != null)
            {
                DataType =
                    (DataFieldType)System.Enum.Parse(typeof(DataFieldType), element.Element("datatype").Value, true);
                DataTypeName = FormHelper.GetDataTypeCodeByType(DataType);

            }

            if (element.Element("displayname") != null)
                DisplayName = element.Element("displayname").Value;

            if (element.Element("enabaled") != null)
                Enabled = bool.Parse(element.Element("enabaled") == null ? "false" : element.Element("enabaled").Value);

            if (element.Element("isallownull") != null)
                IsAllowNull =
                    bool.Parse(element.Element("isallownull") == null ? "false" : element.Element("isallownull").Value);
            if (element.Element("isprimarykey") != null)
                IsPrimaryKey =
                    bool.Parse(element.Element("isprimarykey") == null ? "false" : element.Element("isprimarykey").Value);

            if (element.Element("useasseotemplate") != null)
                UseAsSeoTemplate =
                    bool.Parse(element.Element("useasseotemplate") == null
                                   ? "false"
                                   : element.Element("useasseotemplate").Value);
            if (element.Element("showinlisting") != null)
                ShowInListing =
                    bool.Parse(element.Element("showinlisting") == null
                                   ? "false"
                                   : element.Element("showinlisting").Value);

            if (element.Element("size") != null)
                Size = int.Parse(element.Element("size") == null ? "0" : element.Element("size").Value);

            if (element.Element("sortorder") != null)
                SortOrder = int.Parse(element.Element("sortorder") == null ? "0" : element.Element("sortorder").Value);

            if (element.Element("defaultvalue") != null)
                DefaultValue = element.Element("defaultvalue").Value;

            if (element.Element("description") != null)
                Description = element.Element("description").Value;

            if (element.Element("modifiedby") != null)
                ModifiedBy = element.Element("modifiedby").Value;

            if (element.Element("modifieddate") != null)
                ModifiedDate = DateTime.Parse(element.Element("modifieddate").Value);

            if (element.Element("createdby") != null)
                CreatedBy = element.Element("createdby").Value;

            if (element.Element("createddate") != null)
                CreatedDate = DateTime.Parse(element.Element("createddate").Value);

            if (element.Element("options") != null)
                Options = element.Element("options") != null ? element.Element("options").Value : null;

            XElement elem = element.Element("validation");
            if (elem == null) return;

            if (elem.Element("isrequired") != null)
                IsRequired = elem.Element("isrequired").Value != null
                                 ? bool.Parse(elem.Element("isrequired").Value)
                                 : false;
            if (elem.Element("requirederrormessage") != null)
                RequiredErrorMessage = elem.Element("requirederrormessage").Value;

            if (elem.Element("regularexpression") != null)
                RegularExpression = elem.Element("regularexpression").Value;

            if (elem.Element("regularexpressionerrormessage") != null)
                RegExpErrorMessage = elem.Element("regularexpressionerrormessage").Value;

            if (elem.Element("iscompareable") != null)
                IsCompareAble = elem.Element("iscompareable").Value != null
                                    ? bool.Parse(elem.Element("iscompareable").Value)
                                    : false;
            if (elem.Element("comparewith") != null)
                CompareWith = elem.Element("comparewith").Value;

            if (elem.Element("compareerrormessage") != null)
                CompareErrorMessage = elem.Element("compareerrormessage").Value;
        }

        public static string GetFieldXml(FieldInfo[] fieldinfos)
        {
            var xdoc = new XDocument();

            var fieldsxml = new XElement("fields");
            foreach (FieldInfo item in fieldinfos)
            {
                fieldsxml.Add(new XElement("field",
                                           new XElement("name", item.Name),
                                           new XElement("fieldtype", item.FieldType),
                                           new XElement("datatype", item.DataType),
                                           new XElement("displayname", item.DisplayName),
                                           new XElement("customformcontrolname", item.CustomFormControlName),
                                           new XElement("enabled", item.Enabled),
                                           new XElement("isallownull", item.IsAllowNull),
                                           new XElement("isprimarykey", item.IsPrimaryKey),
                                           new XElement("useasseotemplate", item.UseAsSeoTemplate),
                                           new XElement("showinlisting", item.ShowInListing),
                                           new XElement("size", item.Size),
                                           new XElement("sortorder", item.SortOrder),
                                           new XElement("defaultvalue", item.DefaultValue),
                                           new XElement("validation",
                                                        new XElement("isrequired", item.IsRequired),
                                                        new XElement("requirederrormessage", item.RequiredErrorMessage),
                                                        new XElement("regularexpression", item.RegularExpression),
                                                        new XElement("regularexpressionerrormessage",
                                                                     item.RegExpErrorMessage),
                                                        new XElement("iscompareable", item.IsCompareAble),
                                                        new XElement("comparewith", item.CompareWith),
                                                        new XElement("compareerrormessage", item.CompareErrorMessage)),
                                           new XElement("description", item.Description),
                                           new XElement("isdeleted", item.IsDeleted),
                                           new XElement("modifiedby", item.ModifiedBy),
                                           new XElement("modifieddate", item.ModifiedDate),
                                           new XElement("createdby", item.CreatedBy),
                                           new XElement("createddate", item.CreatedDate),
                                           new XElement("options", item.Options)));
            }
            xdoc.Add(fieldsxml);
            return xdoc.ToString();
        }

        public static FieldInfo[] GetFieldArray(string xmlinput)
        {
            // xdocumnetga yuklandi

            if (string.IsNullOrEmpty(xmlinput)) return null;
            XDocument xdoc = XDocument.Parse(xmlinput, LoadOptions.None);

            XElement fields = xdoc.Element("fields");
            if (fields == null) return null;

            List<FieldInfo> result = new List<FieldInfo>();
            foreach (XElement element in fields.Elements("field"))
            {
                result.Add(new FieldInfo(element));
            }
            return result.OrderBy(x => x.SortOrder).ToArray();
        }

        private static string GetOptionsString(IEnumerable<XElement> elements)
        {
            StringBuilder result = new StringBuilder();
            foreach (XElement element in elements)
            {
                result.AppendFormat("<{0}>{1}</{0}>", element.Name, element.Value);
            }
            return result.ToString();
        }

    }
}


