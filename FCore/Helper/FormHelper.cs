using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using FCore.Class;
using FCore.Enum;

namespace FCore.Helper
{
    public class FormHelper
    {
        #region Variables

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Returns dataset with all data types.
        /// </summary>
        public static SortedDictionary<string, int> GetDataTypes()
        {
            SortedDictionary<string, int> result = new SortedDictionary<string, int>();

            result.Add(DataFieldTypeCode.VARCHAR, (int)DataFieldType.Varchar);
            result.Add(DataFieldTypeCode.CHAR, (int)DataFieldType.Char);
            result.Add(DataFieldTypeCode.TEXT, (int)DataFieldType.Text);
            result.Add(DataFieldTypeCode.INTEGER, (int)DataFieldType.Integer);
            result.Add(DataFieldTypeCode.NUMERIC, (int)DataFieldType.Numeric);
            result.Add(DataFieldTypeCode.DATETIME, (int)DataFieldType.DateTime);
            result.Add(DataFieldTypeCode.BOOLEAN, (int)DataFieldType.Boolean);

            return result;
        }

        /// <summary>
        /// Get Field Control Types
        /// </summary>
        public static SortedDictionary<string, string> GetFieldTypes()
        {
            SortedDictionary<string, string> result = new SortedDictionary<string, string>();
            GetDefaultFieldTypes(ref result);
            GetCustomFormControls(ref result);
            return result;
        }

        /// <summary>
        /// Get Default Field Types
        /// </summary>
        /// <returns></returns>
        private static void GetDefaultFieldTypes(ref SortedDictionary<string, string> dictionary)
        {
            // add rows
            dictionary.Add("Label", FormFieldTypeCode.LABEL);
            dictionary.Add("Textbox", FormFieldTypeCode.TEXTBOX);
            dictionary.Add("Date picker", FormFieldTypeCode.DATEPICKER);
            dictionary.Add("Date and Time Picker", FormFieldTypeCode.DATETIMEPICKER);
            dictionary.Add("File Upload", FormFieldTypeCode.FILEUPLOAD);
            dictionary.Add("Multi File Upload", FormFieldTypeCode.MULTIFILEUPLOAD);
            dictionary.Add("Year Selector", FormFieldTypeCode.YEARSELECTOR);
            dictionary.Add("Captcha", FormFieldTypeCode.CAPTCHA);
            dictionary.Add("Guid Generator", FormFieldTypeCode.GUIDGENERATOR);
            dictionary.Add("Textbox - FckEditor", FormFieldTypeCode.FCKEDITOR);
            dictionary.Add("Yes/No Selector", FormFieldTypeCode.YESNOSELECTOR);
            dictionary.Add("Page Lookup List", FormFieldTypeCode.PAGELISTLOOKUP);
            dictionary.Add("Sql  Lookup List", FormFieldTypeCode.SQLLISTLOOKUP);
            dictionary.Add("Image Selector", FormFieldTypeCode.IMAGESELECTOR);
            dictionary.Add("Lookup List", FormFieldTypeCode.LISTLOOKUP);
            dictionary.Add("Sql Selector", FormFieldTypeCode.SQLSELECTOR);
            dictionary.Add("ContentType Lookup List", FormFieldTypeCode.CONTENTTYPELOOKUP);
            dictionary.Add("Parameter Getter", FormFieldTypeCode.PARAMETERGETTER);
            dictionary.Add("User Profile Getter", FormFieldTypeCode.USERPROFILEGETTER);

        }

        /// <summary>
        /// Get Custom Form controls
        /// </summary>
        /// <param name="dictionary"></param>
        private static void GetCustomFormControls(ref SortedDictionary<string, string> dictionary)
        {
            //FormControlInfo[] infos = DataService.FormControl.All();

            //foreach (FormControlInfo info in infos)
            //{
            //    dictionary.Add(info.Label, string.Format("uc_{0}", info.Name));
            //}
        }

        /// <summary>
        /// Get Data Type Name By ID
        /// </summary>
        /// <param name="dataFieldType"></param>
        /// <returns></returns>
        public static string GetDataTypeCodeByType(DataFieldType dataFieldType)
        {
            switch (dataFieldType)
            {
                case DataFieldType.Boolean:
                    return DataFieldTypeCode.BOOLEAN;
                case DataFieldType.Char:
                    return DataFieldTypeCode.CHAR;
                case DataFieldType.DateTime:
                    return DataFieldTypeCode.DATETIME;
                case DataFieldType.Decimal:
                    return DataFieldTypeCode.DECIMAL;
                case DataFieldType.GUID:
                    return DataFieldTypeCode.GUID;
                case DataFieldType.File:
                    return DataFieldTypeCode.FILE;
                case DataFieldType.Integer:
                    return DataFieldTypeCode.INTEGER;
                case DataFieldType.Text:
                    return DataFieldTypeCode.TEXT;
                case DataFieldType.Varchar:
                    return DataFieldTypeCode.VARCHAR;
                case DataFieldType.Numeric:
                    return DataFieldTypeCode.NUMERIC;
                default:
                    return DataFieldTypeCode.UNKNOWN;
            }

        }

        public static string GetFieldCodeByType(FormFieldType formFieldType)
        {
            switch (formFieldType)
            {
                case FormFieldType.TextBoxControl:
                    return FormFieldTypeCode.TEXTBOX;
                case FormFieldType.LabelControl:
                    return FormFieldTypeCode.LABEL;
                case FormFieldType.DatePickerControl:
                    return FormFieldTypeCode.DATEPICKER;
                case FormFieldType.DateTimePickerControl:
                    return FormFieldTypeCode.DATETIMEPICKER;
                case FormFieldType.SqlSelector:
                    return FormFieldTypeCode.SQLSELECTOR;
                case FormFieldType.ListLookUp:
                    return FormFieldTypeCode.LISTLOOKUP;
                case FormFieldType.PageListLookUp:
                    return FormFieldTypeCode.PAGELISTLOOKUP;
                case FormFieldType.YesNoSelector:
                    return FormFieldTypeCode.YESNOSELECTOR;
                case FormFieldType.FileUpload:
                    return FormFieldTypeCode.FILEUPLOAD;
                case FormFieldType.MultiFileUpload:
                    return FormFieldTypeCode.MULTIFILEUPLOAD;
                case FormFieldType.YearSelector:
                    return FormFieldTypeCode.YEARSELECTOR;
                case FormFieldType.CaptchaControl:
                    return FormFieldTypeCode.CAPTCHA;
                case FormFieldType.GuidGenerator:
                    return FormFieldTypeCode.GUIDGENERATOR;
                case FormFieldType.FckEditor:
                    return FormFieldTypeCode.FCKEDITOR;
                case FormFieldType.ImageSelector:
                    return FormFieldTypeCode.IMAGESELECTOR;
                case FormFieldType.SqlListLookUp:
                    return FormFieldTypeCode.SQLLISTLOOKUP;
                case FormFieldType.ContentTypeLookUp:
                    return FormFieldTypeCode.CONTENTTYPELOOKUP;
                case FormFieldType.ParameterGetter:
                    return FormFieldTypeCode.PARAMETERGETTER;
                case FormFieldType.UserProfileGetter:
                    return FormFieldTypeCode.USERPROFILEGETTER;
                case FormFieldType.CustomFormControl:
                    return "uc";
                default:
                    return FormFieldTypeCode.UNKNOWN;
            }
        }

        public static FormFieldType GetFieldTypeByCode(string formFieldTypeCode)
        {
            if (formFieldTypeCode.StartsWith("uc"))
                return FormFieldType.CustomFormControl;

            switch (formFieldTypeCode)
            {
                case FormFieldTypeCode.TEXTBOX:
                    return FormFieldType.TextBoxControl;
                case FormFieldTypeCode.LABEL:
                    return FormFieldType.LabelControl;
                case FormFieldTypeCode.DATEPICKER:
                    return FormFieldType.DatePickerControl;
                case FormFieldTypeCode.DATETIMEPICKER:
                    return FormFieldType.DateTimePickerControl;
                case FormFieldTypeCode.SQLSELECTOR:
                    return FormFieldType.SqlSelector;
                case FormFieldTypeCode.LISTLOOKUP:
                    return FormFieldType.ListLookUp;
                case FormFieldTypeCode.PAGELISTLOOKUP:
                    return FormFieldType.PageListLookUp;
                case FormFieldTypeCode.YESNOSELECTOR:
                    return FormFieldType.YesNoSelector;
                case FormFieldTypeCode.FILEUPLOAD:
                    return FormFieldType.FileUpload;
                case FormFieldTypeCode.MULTIFILEUPLOAD:
                    return FormFieldType.MultiFileUpload;
                case FormFieldTypeCode.YEARSELECTOR:
                    return FormFieldType.YearSelector;
                case FormFieldTypeCode.CAPTCHA:
                    return FormFieldType.CaptchaControl;
                case FormFieldTypeCode.GUIDGENERATOR:
                    return FormFieldType.GuidGenerator;
                case FormFieldTypeCode.FCKEDITOR:
                    return FormFieldType.FckEditor;
                case FormFieldTypeCode.IMAGESELECTOR:
                    return FormFieldType.ImageSelector;
                case FormFieldTypeCode.SQLLISTLOOKUP:
                    return FormFieldType.SqlListLookUp;
                case FormFieldTypeCode.CONTENTTYPELOOKUP:
                    return FormFieldType.ContentTypeLookUp;
                case FormFieldTypeCode.PARAMETERGETTER:
                    return FormFieldType.ParameterGetter;
                case FormFieldTypeCode.USERPROFILEGETTER:
                    return FormFieldType.UserProfileGetter;
                default:
                    return FormFieldType.Unknown;
            }
        }

        public static bool IsComponent(FieldInfo fieldInfo)
        {
            switch (fieldInfo.FieldType)
            {
                case FormFieldType.ListLookUp:
                case FormFieldType.PageListLookUp:
                case FormFieldType.SqlListLookUp:
                case FormFieldType.MultiFileUpload:
                case FormFieldType.ContentTypeLookUp:
                    return true;
                case FormFieldType.CustomFormControl:
                    string[] tokens = fieldInfo.CustomFormControlName.Split('_');
                    return true;
            }
            return false;
        }

        public static DataSet GetDataSet(string xmlschema)
        {
            DataSet ds = new DataSet();
            StringReader sr = new StringReader(xmlschema);
            XmlReader xml = XmlReader.Create(sr);
            ds.ReadXmlSchema(xml);

            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].AllowDBNull = true;
                dt.Columns[i].ReadOnly = false;
            }
            return ds;
        }

        public static object[,] ConvertDataRowToParams(DataRow sourceDataRow, List<string> columnNames, bool isInsert)
        {
            if (sourceDataRow == null)
                return null;

            if (isInsert)
                columnNames.Remove("Id");

            int totalColumns = columnNames.Count;
            object[,] parameters = new object[totalColumns, 3];
            for (int i = 0; i < totalColumns; i++)
            {
                parameters[i, 0] = "@" + columnNames[i];
                parameters[i, 1] = sourceDataRow[columnNames[i]];
            }

            return parameters;
        }

        public static int GetParamatersCount(string queryParamaters)
        {
            if (string.IsNullOrEmpty(queryParamaters))
                return 0;
            XElement queryParamsDocument = XDocument.Parse(queryParamaters).Element("parameters");
            return queryParamsDocument.Elements("parameter").Count();
        }

        public static string GetDataViewerParametersTable(string queryParamaters)
        {
            StringBuilder queryBuilder = new StringBuilder();
            XElement queryParamsDocument = XDocument.Parse(queryParamaters).Element("parameters");
            XElement[] elements = queryParamsDocument.Elements("parameter").ToArray();
            int index = 0;
            foreach (XElement item in elements)
            {
                index++;
                queryBuilder.AppendFormat("<li id=\"multipleli_{0}\">", index);
                queryBuilder.AppendFormat(
                    "<table style=\"float:left\"><tr><td>Type</td><td><select width=\"40\" id=\"parameterType_{0}\" name=\"parameterType_{0}\">",
                    index);
                queryBuilder.Append("<option value=\"0\" >Please Select</option>");
                queryBuilder.AppendFormat("<option value=\"1\" {0}>Query String</option>",
                                          item.Attribute("type").Value == ((int)QueryParameterType.QueryString).ToString() ? "selected" : "");
                queryBuilder.AppendFormat("<option value=\"2\" {0}>Seo Template</option>",
                item.Attribute("type").Value == ((int)QueryParameterType.SeoTemplate).ToString() ? "selected" : "");
                queryBuilder.AppendFormat("<option value=\"3\" {0}>Cookie</option>",
                item.Attribute("type").Value == ((int)QueryParameterType.Cookie).ToString() ? "selected" : "");
                queryBuilder.AppendFormat("<option value=\"4\" {0}>User Profile Property</option>",
                 item.Attribute("type").Value == ((int)QueryParameterType.UserProfileProperty).ToString() ? "selected" : "");
                queryBuilder.AppendFormat("<option value=\"5\" {0}>Language</option>",
                item.Attribute("type").Value == ((int)QueryParameterType.Language).ToString() ? "selected" : "");

                var dbType = item.Attribute("dbtype");
                if (dbType != null)
                {
                    queryBuilder.AppendFormat(
                        "</select></td><td>Db Type</td><td><select width=\"40\" id=\"parameterDbType_{0}\" name=\"parameterDbType_{0}\">",
                        index);
                    queryBuilder.Append("<option value=\"0\">Please Select</option>");
                    queryBuilder.AppendFormat("<option value=\"1\" {0}>String</option>", dbType.Value == "1" ? "selected" : string.Empty);
                    queryBuilder.AppendFormat("<option value=\"2\" {0}>Integer</option>", dbType.Value == "2" ? "selected" : string.Empty);
                }
                var name = item.Attribute("name");
                if (name != null)
                    queryBuilder.AppendFormat(
                        "</select></td></tr><tr><td>Name</td><td><input type=\"text\" name=\"parameterName_{0}\" id=\"parameterName_{0}\" value=\"{1}\"/></td>",
                        index, name.Value);
                var value = item.Attribute("value");
                if (value != null)
                    queryBuilder.AppendFormat(
                        "<td>Value</td><td><input type=\"text\" name=\"parameterValue_{0}\" id=\"parameterValue_{0}\" value=\"{1}\"/></td>",
                        index, value.Value);
                var defaultValue = item
                    .Attribute("defaultvalue");
                if (defaultValue != null)
                    queryBuilder.AppendFormat(
                        "<td>Default Value</td><td><input type=\"text\" name=\"parameterDefaultValue_{0}\" id=\"parameterDefaultValue_{0}\" value=\"{1}\"/></td>",
                        index, defaultValue.Value);
                if (index == 1)
                    queryBuilder.Append(
                        "<td><a class=\"addRow\" href=\"javascript:addField();\"><img src=\"/content/css/images/menu/addMore_add.png\"></a></td>");
                else
                    queryBuilder.AppendFormat(
                        "<td><a class=\"addRow\" href=\"javascript:addField();\"><img src=\"/content/css/images/menu/addMore_add.png\"></a><a class=\"removeRow\" href=\"javascript:removeField({0});\"><img src=\"/content/css/images/menu/addMore_remove.png\"></a></td>",
                        index);
                queryBuilder.Append("</table></li>");
            }
            return queryBuilder.ToString();
        }

        public static string GetDataViewerParametersXml(Dictionary<string, string> queryParams)
        {
            if (queryParams.Count(x => x.Key.StartsWith("parameterType")) == 0)
                return string.Empty;
            StringBuilder queryParamsBuilder = new StringBuilder();
            queryParamsBuilder.Append("<parameters>");
            int count = queryParams.Count / 5;
            for (int i = 1; i <= count; i++)
            {
                queryParamsBuilder.AppendFormat("<parameter type=\"{0}\" name=\"{1}\" value=\"{2}\" defaultvalue=\"{3}\" dbtype=\"{4}\" />",
                                                queryParams["parameterType_" + i], queryParams["parameterName_" + i],
                                                queryParams["parameterValue_" + i], queryParams["parameterDefaultValue_" + i], queryParams["parameterDbType_" + i]);
            }
            queryParamsBuilder.Append("</parameters>");
            return queryParamsBuilder.ToString();
        }

        #endregion
    }
}
