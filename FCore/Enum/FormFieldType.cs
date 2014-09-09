using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCore.Enum
{
    public enum FormFieldType : int
    {
        /// <summary>
        /// Label control.
        /// </summary>
        LabelControl = 1,

        /// <summary>
        /// TextBox control.
        /// </summary>
        TextBoxControl = 2,

        /// <summary>
        /// Date Picker control.
        /// </summary>
        DatePickerControl = 3,

        /// <summary>
        /// DateTime Picker control.
        /// </summary>
        DateTimePickerControl = 4,

        /// <summary>
        /// Yes/No selector control.
        /// </summary>
        YesNoSelector = 6,

        /// <summary>
        /// Year selector control
        /// </summary>
        YearSelector = 7,

        /// <summary>
        /// List Look up control.
        /// </summary>
        ListLookUp = 8,

        /// <summary>
        /// Page List Look Up control
        /// </summary>
        PageListLookUp = 9,

        /// <summary>
        /// FileUpload control.
        /// </summary>
        FileUpload = 10,

        /// <summary>
        /// MultiFileUpload control.
        /// </summary>
        MultiFileUpload = 11,

        /// <summary>
        /// Custom form Controls.
        /// </summary>
        CustomFormControl = 12,

        /// <summary>
        /// Captcha Controls.
        /// </summary>
        CaptchaControl = 13,


        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 14,

        /// <summary>
        /// Guid Generator.
        /// </summary>
        GuidGenerator = 15,

        /// <summary>
        /// FckEditor.
        /// </summary>
        FckEditor = 16,


        /// <summary>
        /// ImageSelector.
        /// </summary>
        ImageSelector = 17,

        /// <summary>
        /// SqlListLookUp.
        /// </summary>
        SqlListLookUp = 18,

        /// <summary>
        /// SqlSelector.
        /// </summary>
        SqlSelector = 19,

        ContentTypeLookUp = 20,

        ParameterGetter = 21,

        UserProfileGetter = 22

    }

    public class FormFieldTypeCode
    {
        /// <summary>
        /// Label with text
        /// </summary>
        public const string LABEL = "label";

        /// <summary>
        /// TextBox with text.
        /// </summary>
        public const string TEXTBOX = "textbox";

        /// <summary>
        /// DatePicker with text;
        /// </summary>
        public const string DATEPICKER = "datepicker";

        /// <summary>
        /// DateTimePicker.
        /// </summary>
        public const string DATETIMEPICKER = "datetimepicker";

        /// <summary>
        /// YesNoSelector with text.
        /// </summary>
        public const string YESNOSELECTOR = "yesnoselector";


        /// <summary>
        /// ListLookUp with text.
        /// </summary>
        public const string LISTLOOKUP = "lookuplist";

        /// <summary>
        /// PageListLookUp with text.
        /// </summary>
        public const string PAGELISTLOOKUP = "pagelookuplist";

        /// <summary>
        /// FileUplolad with text.
        /// </summary>
        public const string FILEUPLOAD = "fileupload";

        /// <summary>
        /// MultiFileUplolad with text.
        /// </summary>
        public const string MULTIFILEUPLOAD = "multifileupload";

        /// <summary>
        /// Year selector
        /// </summary>
        public const string YEARSELECTOR = "yearselector";

        /// <summary>
        /// Captcha
        /// </summary>
        public const string CAPTCHA = "captcha";

        /// <summary>
        /// UNKNOWN.
        /// </summary>
        public const string UNKNOWN = "unknown";

        /// <summary>
        /// GUIDGENERATOR.
        /// </summary>
        public const string GUIDGENERATOR = "guidgenerator";

        /// <summary>
        /// FckEditor.
        /// </summary>
        public const string FCKEDITOR = "fckeditor";

        /// <summary>
        /// ImageSelector.
        /// </summary>
        public const string IMAGESELECTOR = "imageselector";

        /// <summary>
        /// SqlListLookup.
        /// </summary>
        public const string SQLLISTLOOKUP = "sqllookuplist";

        /// <summary>
        /// SqlSelector.
        /// </summary>
        public const string SQLSELECTOR = "sqlselector";

        public const string CONTENTTYPELOOKUP = "contenttypelookup";

        public const string PARAMETERGETTER = "parametergetter";

        public const string USERPROFILEGETTER = "userprofilegetter";
    }
}
