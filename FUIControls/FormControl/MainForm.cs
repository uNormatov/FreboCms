using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.Helper;
using FUIControls.PortalControl;
using FUIControls.Settings;

namespace FUIControls.FormControl
{
    [ToolboxData("<fr:MainForm runat=\"server\" ID=\"MainForm1\" />")]
    public class MainForm : AbstractControl, INamingContainer
    {
        private readonly ContentTypeProvider _contentTypeProvider;
        private readonly GeneralConnection _generalConnection;
        private readonly FormProvider _formProvider;

        #region Delegate & Event

        public delegate void OnBeforeSaveEventHandler();

        public event OnBeforeSaveEventHandler OnBeforeSave;

        public delegate void OnAfterSaveEventHandler();

        public event OnAfterSaveEventHandler OnAfterSave;

        public delegate void OnBeforeUpdateEventHandler();

        public event OnBeforeUpdateEventHandler OnBeforeUpdate;

        public delegate void OnAfterUpdateEventHandler();

        public event OnAfterUpdateEventHandler OnAfterUpdate;

        public delegate void OnBeforeSelectEventHandler();

        public event OnBeforeSelectEventHandler OnBeforeSelect;

        public delegate void OnAfterSelectEventHandler();

        public event OnAfterSelectEventHandler OnAfterSelect;

        public delegate void OnOnFailureEventHandler();

        public event OnOnFailureEventHandler OnFailure;

        public delegate void OnValidationFailed(ErrorInfoList errors);

        public event OnValidationFailed OnValidationFailure;

        #endregion

        #region Variables

        private Dictionary<string, string> WidgetControls
        {
            get
            {
                object o = ViewState["__main_form_widget_controls"];
                if (o == null) return new Dictionary<string, string>();
                return (Dictionary<string, string>)o;
            }
            set { ViewState["__main_form_widget_controls"] = value; }
        }

        private Button _submitbutton;
        private static Regex _regularcustomformsplitter;
        private bool _isvalid = true;
        private string _buttonCssClass;
        private bool _isContent = true;

        #endregion

        #region Properties

        public ErrorInfoList ErrorInfoList { get; set; }

        public bool IsValid
        {
            get { return _isvalid; }
            set { _isvalid = value; }
        }

        public ContentTypeInfo ContentTypeInfo { get; set; }

        public int ContentTypeId
        {
            get
            {
                object o = ViewState["__content_tpe_info"];
                return ValidationHelper.GetInteger(o, 0);
            }
            set { ViewState["__content_tpe_info"] = value; }
        }

        public ContentTypeModel ContentTypeModel { get; set; }

        public int ContentId { get; set; }

        public DataRow DataRow { get; set; }

        public bool IsContent
        {
            get { return _isContent; }
            set { _isContent = value; }
        }

        public bool IsUseCaptcha { get; set; }

        public FormActionMode FormMode { get; set; }

        public string ContentTypeName { get; set; }

        public string FormName
        {
            get
            {
                object o = ViewState["__main_form_name"];
                if (o == null)
                    return string.Empty;
                return o.ToString();
            }
            set { ViewState["__main_form_name"] = value; }
        }

        public FormInfo CustomForm
        {
            get
            {
                object o = ViewState["__main_form_custom_form"];
                if (o == null)
                {
                    if (string.IsNullOrEmpty(FormName))
                        o = _formProvider.Select(ContentTypeInfo.DefaultFormId, ErrorInfoList);
                    else
                        o = _formProvider.SelectByName(FormName, ErrorInfoList);

                    ViewState["__main_form_custom_form"] = o;
                }
                return (FormInfo)o;
            }
            set { ViewState["__main_form_custom_form"] = value; }
        }

        public Regex RegularFormSplitter
        {
            get
            {
                if (_regularcustomformsplitter == null)
                {
                    _regularcustomformsplitter = RegexHelper.GetRegex("\\$\\$\\w+(?::(?:[A-Za-z]|_[A-Za-z])\\w*|)\\$\\$");
                }
                return _regularcustomformsplitter;
            }
            set { _regularcustomformsplitter = value; }
        }

        public string ButtonCssClass
        {
            get
            {
                if (_buttonCssClass == null)
                {
                    _buttonCssClass = String.Empty;
                }

                return _buttonCssClass;
            }
            set { _buttonCssClass = value; }
        }

        public string SaveButtonText { get; set; }

        public string CancelButtonText { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button SubmitButton
        {
            get
            {
                if (_submitbutton == null)
                {
                    _submitbutton = new Button();
                    if (string.IsNullOrEmpty(SaveButtonText))
                        _submitbutton.Text = "Save";
                    else
                        _submitbutton.Text = LanguageHelper.Instance.GetTranslateByPattern(GetCurrentLanguage(), SaveButtonText);
                    _submitbutton.CssClass = ButtonCssClass;
                }
                return _submitbutton;
            }
            set { _submitbutton = value; }
        }


        [Category("Behavior")]
        public FieldInfoCollection FormControls { get; set; }

        #endregion

        #region Constructor
        public MainForm()
        {
            _generalConnection = new GeneralConnection();
            _contentTypeProvider = new ContentTypeProvider();
            _formProvider = new FormProvider();
            ErrorInfoList = new ErrorInfoList();
            OnValidationFailure += MainForm_OnValidationFailure;
        }

        #endregion

        #region Methods

        protected override void CreateChildControls()
        {
            LoadData();
            if (CustomForm == null)
                CreateDefaultFormLayout();
            else
                CreateCustomFormLayout();
        }

        protected void CreateDefaultFormLayout()
        {
            if (FormControls == null)
                return;
            var widgets = new Dictionary<string, string>();
            Controls.Add(new LiteralControl("  <ul class=\"adminformlist\">"));
            foreach (FieldInfo finfo in FormControls)
            {
                AbsractBasicControl control = null;
                if (CreateFieldControl(finfo, ref control, ref widgets))
                {
                    Controls.Add(new LiteralControl("<li>"));
                    Controls.Add(new LiteralControl(string.Format("<label class=\"jform_name-lbl\">{0} {1}</label>", finfo.DisplayName, finfo.IsRequired ? "*" : "")));
                    Controls.Add((Control)control);
                    Controls.Add(new LiteralControl("</li>"));
                }
            }
            Controls.Add(new LiteralControl("</ul>"));
            WidgetControls = widgets;
        }

        protected void CreateCustomFormLayout()
        {
            if (FormControls == null)
                return;
            if (CustomForm != null && !string.IsNullOrEmpty(CustomForm.Layout))
            {
                string layout = LanguageHelper.Instance.GetTranslateByPattern(GetCurrentLanguage(), CustomForm.Layout.ToHtmlDecode());
                MatchCollection matchCollection = RegularFormSplitter.Matches(layout);
                var widgets = new Dictionary<string, string>();
                int curPost = 0;
                int newPos = 0;
                string pattern = string.Empty;
                string controlType = string.Empty;
                string controlName = string.Empty;

                foreach (Match item in matchCollection)
                {
                    newPos = item.Index;
                    if (curPost < newPos)
                        Controls.Add(new LiteralControl(layout.Substring(curPost, newPos - curPost)));

                    curPost = newPos + item.Length;
                    pattern = item.Value.Replace("$$", "");
                    int colonPos = pattern.IndexOf(":");
                    if (colonPos < 0)
                    {
                        controlType = pattern;
                        controlName = string.Empty;
                    }
                    else
                    {
                        controlType = pattern.Substring(0, colonPos);
                        controlName = pattern.Substring(colonPos + 1, pattern.Length - colonPos - 1);
                    }
                    FieldInfo finfo = FormControls[controlName];
                    AbsractBasicControl control = null;
                    bool allowAdd = CreateFieldControl(finfo, ref control, ref widgets);
                    switch (controlType)
                    {
                        case "label":
                            if (allowAdd && finfo != null)
                                Controls.Add(CreateFieldLabel(finfo));
                            break;
                        case "input":
                            if (allowAdd && control != null)
                                Controls.Add(control);
                            break;
                        case "submitbutton":
                            SubmitButton.Click += SubmitButtonClick;
                            Controls.Add(SubmitButton);
                            break;
                        case "validation":
                            Controls.Add(CreateValidationControl(controlName));
                            break;
                        case "cancelbutton":
                            Controls.Add(new LiteralControl("<input type=\"button\" value=\"" + (string.IsNullOrEmpty(CancelButtonText) ? "Cancel" : CancelButtonText) + "\" onclick=\"history.go(-1);\"/>"));
                            break;
                        case "captcha":
                            IsUseCaptcha = true;
                            CaptchaControl captchaControl = new CaptchaControl(FormFieldTypeCode.CAPTCHA, null);
                            captchaControl.FieldName = "captcha";
                            Controls.Add(captchaControl);
                            break;

                    }
                }
                if (curPost < layout.Length)
                {
                    Controls.Add(new LiteralControl(layout.Substring(curPost, layout.Length - curPost)));
                }
                WidgetControls = widgets;
            }
        }

        protected LiteralControl CreateFieldLabel(FieldInfo fieldInfo)
        {
            var ltlResult = new LiteralControl();
            if (FormControls[fieldInfo.Name] != null)
            {
                ltlResult.Text = "";

                string labelText = FormControls[fieldInfo.Name].DisplayName;
                if (CoreSettings.CurrentSite.IsMultilanguage)
                    labelText = GetResource(fieldInfo.Name);
                if (labelText.Equals(fieldInfo.Name))
                    labelText = FormControls[fieldInfo.Name].DisplayName;

                ltlResult.Text = labelText;
                if (fieldInfo.IsRequired)
                    ltlResult.Text += "<span class=\"red\">*</span>";
            }
            return ltlResult;
        }

        protected LiteralControl CreateValidationControl(string fieldName)
        {
            LiteralControl validationControl = new LiteralControl();
            validationControl.ID = "ltlValidation" + fieldName;
            return validationControl;
        }

        protected bool CreateFieldControl(FieldInfo finfo, ref AbsractBasicControl control, ref Dictionary<string, string> widgets)
        {
            bool allowToAdd = false;
            switch (finfo.FieldType)
            {
                case FormFieldType.TextBoxControl:
                    control = new TextBoxControl(FormFieldTypeCode.TEXTBOX + finfo.Name, finfo.Options);

                    if (FormMode == FormActionMode.Edit)
                    {
                        object value = ContentTypeModel.GetValue(finfo.Name);
                        if (value != null)
                            ((TextBoxControl)control).Value = value.ToString();
                    }
                    allowToAdd = true;
                    break;
                case FormFieldType.FckEditor:
                    control = new FckEditorControl(FormFieldTypeCode.FCKEDITOR + finfo.Name, finfo.Options);
                    if (FormMode == FormActionMode.Edit)
                    {
                        object value = ContentTypeModel.GetValue(finfo.Name);
                        if (value != null)
                            ((FckEditorControl)control).Value = value.ToString();
                    }
                    allowToAdd = true;
                    break;
                case FormFieldType.GuidGenerator:
                    control = new GuidGeneratorControl(FormFieldTypeCode.GUIDGENERATOR + finfo.Name);
                    if (FormMode == FormActionMode.Edit)
                    {
                        object value = ContentTypeModel.GetValue(finfo.Name);
                        if (value != null)
                            ((GuidGeneratorControl)control).Value = value.ToString();
                    }
                    allowToAdd = true;
                    break;
                case FormFieldType.DatePickerControl:
                    control = new DatePicker();
                    ((Control)control).ID = FormFieldTypeCode.DATEPICKER + finfo.Name;
                    if (FormMode == FormActionMode.Edit)
                    {
                        object value = ContentTypeModel.GetValue(finfo.Name);
                        if (value != null)
                            ((DatePicker)control).Value = value.ToString();
                    }
                    allowToAdd = true;
                    break;
                case FormFieldType.DateTimePickerControl:
                    control = new DateTimePicker(FormFieldTypeCode.DATETIMEPICKER + finfo.Name, finfo.Options);
                    if (FormMode == FormActionMode.Edit)
                    {
                        object value = ContentTypeModel.GetValue(finfo.Name);
                        if (value != null)
                            ((DateTimePicker)control).Value = value.ToString();
                    }
                    allowToAdd = true;
                    break;
                case FormFieldType.YesNoSelector:
                    control = new YesNoSelector(FormFieldTypeCode.YESNOSELECTOR + finfo.Name, finfo.Options);
                    if (FormMode == FormActionMode.Edit)
                    {
                        object value = ContentTypeModel.GetValue(finfo.Name);
                        if (value != null)
                            ((YesNoSelector)control).Value = value.ToString();
                    }
                    allowToAdd = true;
                    break;
                case FormFieldType.YearSelector:
                    control = new YearSelector(FormFieldTypeCode.YEARSELECTOR + finfo.Name, finfo.Options);
                    if (FormMode == FormActionMode.Edit)
                    {
                        object value = ContentTypeModel.GetValue(finfo.Name);
                        if (value != null)
                            ((YearSelector)control).Value = value.ToString();
                    }
                    allowToAdd = true;
                    break;

                case FormFieldType.FileUpload:
                    control = new FileUploader(FormFieldTypeCode.FILEUPLOAD + finfo.Name, finfo.Options);
                    if (FormMode == FormActionMode.Edit)
                    {
                        object value = ContentTypeModel.GetValue(finfo.Name);
                        if (value != null)
                            ((FileUploader)control).Value = value.ToString();
                    }
                    allowToAdd = true;
                    break;
                case FormFieldType.ParameterGetter:
                    control = new ParameterGetterControl(FormFieldTypeCode.PARAMETERGETTER + finfo.Name, finfo.Options);
                    if (FormMode == FormActionMode.Edit)
                    {
                        object value = ContentTypeModel.GetValue(finfo.Name);
                        if (value != null)
                            ((ParameterGetterControl)control).Value = value.ToString();
                    }
                    allowToAdd = true;
                    break;
                case FormFieldType.UserProfileGetter:
                    control = new FromUserProfileControl(FormFieldTypeCode.USERPROFILEGETTER + finfo.Name, finfo.Options);
                    if (FormMode == FormActionMode.Edit)
                    {
                        object value = ContentTypeModel.GetValue(finfo.Name);
                        if (value != null)
                            ((FromUserProfileControl)control).Value = value.ToString();
                    }
                    allowToAdd = true;
                    break;
                case FormFieldType.ListLookUp:
                    control = new ListLookUp(FormFieldTypeCode.LISTLOOKUP + finfo.Name, finfo.Options, _generalConnection, ErrorInfoList);
                    if (!widgets.ContainsKey(FormFieldTypeCode.LISTLOOKUP + finfo.Name))
                        widgets.Add(FormFieldTypeCode.LISTLOOKUP + finfo.Name, finfo.Name);
                    if (FormMode == FormActionMode.Edit)
                        ((ListLookUp)control).SelectData(ContentTypeId, ContentId, finfo.Name);
                    allowToAdd = true;
                    break;
                case FormFieldType.ContentTypeLookUp:
                    control = new ContentTypeLookUp(FormFieldTypeCode.CONTENTTYPELOOKUP + finfo.Name, finfo.Options, _generalConnection, ErrorInfoList);
                    if (!widgets.ContainsKey(FormFieldTypeCode.CONTENTTYPELOOKUP + finfo.Name))
                        widgets.Add(FormFieldTypeCode.CONTENTTYPELOOKUP + finfo.Name, finfo.Name);
                    if (FormMode == FormActionMode.Edit)
                        ((ContentTypeLookUp)control).SelectData(ContentTypeId, ContentId, finfo.Name);
                    allowToAdd = true;
                    break;

            }

            if (allowToAdd)
            {
                control.IsRequired = finfo.IsRequired;
                control.RegularExpression = finfo.RegularExpression;
                control.RequiredErrorMessage = finfo.RequiredErrorMessage;
                control.RegularExpressionErrorMessage = finfo.RegExpErrorMessage;
            }
            if (control != null)
                control.FieldName = finfo.Name;
            return allowToAdd;
        }

        protected void SubmitButtonClick(object sender, EventArgs e)
        {
            SaveData();
        }

        public virtual void LoadData()
        {
            if (OnBeforeSelect != null)
                OnBeforeSelect();


            if (ContentTypeModel == null)
            {
                if (ContentId != 0)
                    ContentTypeModel = new ContentTypeModel(ContentTypeId, ContentId, _generalConnection, _contentTypeProvider, ErrorInfoList);
                else if (DataRow != null)
                    ContentTypeModel = new ContentTypeModel(ContentTypeId, DataRow, _generalConnection, _contentTypeProvider, ErrorInfoList);
                else
                    ContentTypeModel = new ContentTypeModel(ContentTypeId, _generalConnection, _contentTypeProvider, ErrorInfoList);
            }
            if (ContentTypeModel != null)
            {
                ContentTypeInfo = ContentTypeModel.ContentTypeInfo;
                if (ContentTypeInfo != null)
                {
                    ContentTypeId = ContentTypeInfo.Id;
                    ContentId = ContentTypeModel.ContentId;
                    FormControls = new FieldInfoCollection(FieldInfo.GetFieldArray(ContentTypeInfo.FieldsXml));
                }
            }
            if (OnAfterSelect != null)
                OnAfterSelect();
        }

        public virtual void SaveData()
        {
            if (!Validate())
            {
                if (OnValidationFailure != null)
                    OnValidationFailure(ErrorInfoList);
                if (OnFailure != null)
                    OnFailure();
                return;
            }
            var widgets = new List<IComponentControl>();
            foreach (string columnName in ContentTypeModel.ColumnNames)
            {
                switch (FormControls[columnName].FieldType)
                {
                    case FormFieldType.GuidGenerator:
                        var guidControl = FindControl(FormFieldTypeCode.GUIDGENERATOR + columnName) as GuidGeneratorControl;
                        if (guidControl != null)
                        {
                            string guid = ValidationHelper.GetString(guidControl.Value, "0000-0000-0000-0000");
                            ContentTypeModel.SetValue(columnName, guid);
                        }
                        break;
                    case FormFieldType.TextBoxControl:
                        var txt = FindControl(FormFieldTypeCode.TEXTBOX + columnName) as TextBoxControl;
                        if (txt != null)
                        {
                            ContentTypeModel.SetValue(columnName, txt.Value);
                        }
                        break;
                    case FormFieldType.FckEditor:
                        var fckeditor = FindControl(FormFieldTypeCode.FCKEDITOR + columnName) as FckEditorControl;
                        if (fckeditor != null)
                        {
                            ContentTypeModel.SetValue(columnName, fckeditor.Value);
                        }
                        break;
                    case FormFieldType.DatePickerControl:
                        var datepicker = FindControl(FormFieldTypeCode.DATEPICKER + columnName) as DatePicker;
                        if (datepicker != null)
                        {
                            ContentTypeModel.SetValue(columnName, datepicker.Value);
                        }

                        break;
                    case FormFieldType.DateTimePickerControl:
                        var datetimepicker = FindControl(FormFieldTypeCode.DATETIMEPICKER + columnName) as DateTimePicker;
                        if (datetimepicker != null)
                        {
                            ContentTypeModel.SetValue(columnName, datetimepicker.Value);
                        }
                        break;
                    case FormFieldType.YesNoSelector:
                        var yesno = FindControl(FormFieldTypeCode.YESNOSELECTOR + columnName) as YesNoSelector;
                        if (yesno != null)
                        {
                            ContentTypeModel.SetValue(columnName, yesno.Value);
                        }
                        break;
                    case FormFieldType.FileUpload:
                        var fileUpload = FindControl(FormFieldTypeCode.FILEUPLOAD + columnName) as FileUploader;
                        if (fileUpload != null)
                        {
                            ContentTypeModel.SetValue(columnName, fileUpload.Value);
                        }
                        break;
                    case FormFieldType.YearSelector:
                        var yearSelector = FindControl(FormFieldTypeCode.YEARSELECTOR + columnName) as YearSelector;
                        if (yearSelector != null)
                        {
                            ContentTypeModel.SetValue(columnName, yearSelector.Value);
                        }
                        break;
                    case FormFieldType.ParameterGetter:
                        var parameterGetter = FindControl(FormFieldTypeCode.PARAMETERGETTER + columnName) as ParameterGetterControl;
                        if (parameterGetter != null)
                        {
                            ContentTypeModel.SetValue(columnName, parameterGetter.Value);
                        }
                        break;
                    case FormFieldType.UserProfileGetter:
                        var userProfileGetter = FindControl(FormFieldTypeCode.USERPROFILEGETTER + columnName) as FromUserProfileControl;
                        if (userProfileGetter != null)
                        {
                            ContentTypeModel.SetValue(columnName, userProfileGetter.Value);
                        }
                        break;
                }
            }
            if (FormMode == FormActionMode.Insert)
            {
                if (OnBeforeSave != null)
                    OnBeforeSave();

                ContentId = ContentTypeModel.Insert();
                if (ContentId != -1)
                {
                    foreach (var item in WidgetControls)
                    {
                        IComponentControl control = null;
                        switch (FormControls[item.Value].FieldType)
                        {
                            case FormFieldType.ListLookUp:
                                control = FindControl(item.Key) as ListLookUp;
                                break;
                            case FormFieldType.ContentTypeLookUp:
                                control = FindControl(item.Key) as ContentTypeLookUp;
                                break;
                        }
                        if (control != null)
                            control.InsertData(ContentTypeId, ContentId, item.Value);

                    }
                }
                else
                {
                    if (OnValidationFailure != null)
                        OnValidationFailure(ContentTypeModel.ErrorInfoList);

                    if (OnFailure != null)
                        OnFailure();
                }

                if (OnAfterSave != null)
                    OnAfterSave();
            }
            else if (FormMode == FormActionMode.Edit)
            {
                if (OnBeforeUpdate != null)
                    OnBeforeUpdate();

                ContentTypeModel.ContentId = ContentId;
                ContentTypeModel.Update();

                foreach (var item in WidgetControls)
                {
                    IComponentControl control = null;
                    switch (FormControls[item.Value].FieldType)
                    {
                        case FormFieldType.ListLookUp:
                            control = FindControl(item.Key) as ListLookUp;
                            break;
                        case FormFieldType.ContentTypeLookUp:
                            control = FindControl(item.Key) as ContentTypeLookUp;
                            break;
                    }
                    if (control != null)
                        control.UpdateData(ContentTypeId, this.ContentId, item.Value);
                }
                if (OnAfterUpdate != null)
                    OnAfterUpdate();
            }

        }

        public virtual void Delete()
        {
            if (ContentTypeModel == null)
                ContentTypeModel = new ContentTypeModel(ContentTypeId, ContentId, _generalConnection, _contentTypeProvider, ErrorInfoList);

            if (ContentTypeModel != null)
                ContentTypeModel.Delete();
        }

        public virtual bool Validate()
        {
            ErrorInfoList.Clear();
            ValidateControls();
            ValidateCaptcha();
            ValidateComponentControl();
            IsValid = ErrorInfoList.Count == 0;
            return IsValid;
        }

        private void ValidateControls()
        {
            foreach (string columnName in ContentTypeModel.ColumnNames)
            {
                switch (FormControls[columnName].FieldType)
                {
                    case FormFieldType.TextBoxControl:
                        var txt = FindControl(FormFieldTypeCode.TEXTBOX + columnName) as TextBoxControl;
                        if (txt != null)
                        {
                            if (!txt.Validate())
                            {
                                ErrorInfoList.AddRange(txt.ErrorInfoList);
                            }
                        }
                        break;
                    case FormFieldType.FckEditor:
                        var fckeditor = FindControl(FormFieldTypeCode.FCKEDITOR + columnName) as FckEditorControl;
                        if (fckeditor != null)
                        {
                            if (!fckeditor.Validate())
                            {
                                ErrorInfoList.AddRange(fckeditor.ErrorInfoList);
                            }
                        }
                        break;
                    case FormFieldType.DatePickerControl:
                        var datepicker = FindControl(FormFieldTypeCode.DATEPICKER + columnName) as DatePicker;
                        if (datepicker != null)
                        {
                            if (!datepicker.Validate())
                            {
                                ErrorInfoList.AddRange(datepicker.ErrorInfoList);
                            }
                        }

                        break;
                    case FormFieldType.DateTimePickerControl:
                        var datetimepicker = FindControl(FormFieldTypeCode.DATETIMEPICKER + columnName) as DateTimePicker;
                        if (datetimepicker != null)
                        {
                            if (!datetimepicker.Validate())
                            {
                                ErrorInfoList.AddRange(datetimepicker.ErrorInfoList);
                            }
                        }
                        break;
                    case FormFieldType.FileUpload:
                        var fileUpload = FindControl(FormFieldTypeCode.FILEUPLOAD + columnName) as FileUploader;
                        if (fileUpload != null)
                        {
                            if (!fileUpload.Validate())
                            {
                                ErrorInfoList.AddRange(fileUpload.ErrorInfoList);
                            }
                        }
                        break;
                    case FormFieldType.YearSelector:
                        var yearSelector = FindControl(FormFieldTypeCode.YEARSELECTOR + columnName) as YearSelector;
                        if (yearSelector != null)
                        {
                            if (!yearSelector.Validate())
                            {
                                ErrorInfoList.AddRange(yearSelector.ErrorInfoList);
                            }
                        }
                        break;
                    case FormFieldType.ParameterGetter:
                        var parameterGetter = FindControl(FormFieldTypeCode.PARAMETERGETTER + columnName) as ParameterGetterControl;
                        if (parameterGetter != null)
                        {
                            if (!parameterGetter.Validate())
                            {
                                ErrorInfoList.AddRange(parameterGetter.ErrorInfoList);
                            }
                        }
                        break;
                    case FormFieldType.UserProfileGetter:
                        var userProfileGetter = FindControl(FormFieldTypeCode.USERPROFILEGETTER + columnName) as FromUserProfileControl;
                        if (userProfileGetter != null)
                        {
                            if (!userProfileGetter.Validate())
                            {
                                ErrorInfoList.AddRange(userProfileGetter.ErrorInfoList);
                            }
                        }
                        break;
                }
            }

        }

        private void ValidateComponentControl()
        {
            foreach (var item in WidgetControls)
            {
                IComponentControl control = null;
                switch (FormControls[item.Value].FieldType)
                {
                    case FormFieldType.ListLookUp:
                        control = FindControl(item.Key) as ListLookUp;
                        break;
                    case FormFieldType.ContentTypeLookUp:
                        control = FindControl(item.Key) as ContentTypeLookUp;
                        break;
                }
                if (control != null)
                {
                    if (!((AbsractBasicControl)control).Validate())
                    {
                        IsValid = false;
                        ErrorInfoList errors = ((AbsractBasicControl)control).ErrorInfoList;
                        foreach (ErrorInfo error in errors)
                        {
                            if (!ErrorInfoList.Contains(error))
                                ErrorInfoList.Add(error);
                        }
                    }

                }
            }
        }

        private void ValidateCaptcha()
        {
            if (IsUseCaptcha)
            {
                CaptchaControl captchaControl = FindControl(FormFieldTypeCode.CAPTCHA) as CaptchaControl;
                if (captchaControl != null && !captchaControl.Validate())
                {
                    ErrorInfoList.AddRange(captchaControl.ErrorInfoList);
                }
            }
        }

        private void MainForm_OnValidationFailure(ErrorInfoList errors)
        {
            foreach (ErrorInfo error in errors)
            {
                if (string.IsNullOrEmpty(error.Source))
                    continue;
                LiteralControl literalControl = FindControl("ltlValidation" + error.Source) as LiteralControl;

                if (literalControl != null)
                {
                    if (!string.IsNullOrEmpty(literalControl.Text))
                        literalControl.Text += "<br>" + error.Message;
                    else
                        literalControl.Text = error.Message;
                }
            }
        }

        #endregion
    }
}
