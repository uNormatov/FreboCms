using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.FormControl;
using FUIControls.Page;
using FUIControls.Settings;

namespace FWeb.Administrator.ContentType
{
    public partial class fieldaction : FAdminEditPage
    {
        private ContentTypeProvider _contentTypeProvider;

        private int ContentTypeId
        {
            get
            {
                if (ViewState["_contentTypeId"] == null)
                {
                    int contentTypeId = ValidationHelper.GetInteger(Request.QueryString["contenttypeid"], -1);
                    if (contentTypeId == -1)
                    {
                        List<ContentTypeInfo> contentTypeInfos = _contentTypeProvider.SelectPagingSorting(1, 1, "Id", "DESC", ErrorList);
                        if (contentTypeInfos != null && contentTypeInfos.Count > 0)
                            contentTypeId = contentTypeInfos[0].Id;
                    }
                    return contentTypeId;
                }
                return ValidationHelper.GetInteger(ViewState["_contentTypeId"], 20);
            }
            set { ViewState["_contentTypeId"] = value; }
        }

        protected override void Init()
        {
            base.Init();
            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();

            if (IsEdit)
            {
                Title = "Edit Field | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "Edit Field";
            }
            else
            {
                Title = "New Field | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "New Field";
            }

        }

        protected override void Load()
        {
            CancelUrl = "/administrator/contenttype/field.aspx?contenttypeid=" + ContentTypeId;
            RedrictUrl = CancelUrl;
            base.Load();
        }

        protected override void FillFields()
        {
            List<ContentTypeInfo> contentTypeInfos = _contentTypeProvider.SelectAll(ErrorList);
            drlList.DataSource = contentTypeInfos;
            drlList.DataBind();
            drlList.SelectedValue = ContentTypeId.ToString();

            drlDataType.DataSource = FormHelper.GetDataTypes();
            drlDataType.DataBind();
            drlFieldType.DataSource = FormHelper.GetFieldTypes();
            drlFieldType.DataBind();
            if (IsEdit)
            {
                ContentTypeInfo contentTypeInfo = _contentTypeProvider.Select(ContentTypeId, ErrorList);
                if (contentTypeInfo != null)
                {
                    FieldInfo fieldInfo = FieldInfo.GetFieldArray(contentTypeInfo.FieldsXml).FirstOrDefault(x => x.Name == Id);

                    if (fieldInfo != null)
                    {
                        txtColumnName.Text = fieldInfo.Name;
                        drlDataType.SelectedValue = fieldInfo.DataType.ToString();
                        txtColumnSize.Text = fieldInfo.Size.ToString();
                        txtDeafultValue.Text = fieldInfo.DefaultValue;
                        chbxIsAllowNull.Checked = fieldInfo.IsAllowNull;
                        chbxUseAsSeoTemplate.Checked = fieldInfo.IsAllowNull;
                        chbxShowInListing.Checked = fieldInfo.ShowInListing;
                        drlOrder.SelectedValue = fieldInfo.SortOrder.ToString();
                        txtDiplayName.Text = fieldInfo.DisplayName;
                        drlFieldType.SelectedValue = FormHelper.GetFieldCodeByType(fieldInfo.FieldType);
                        chbxIsRequired.Checked = fieldInfo.IsRequired;
                        txtRequiredMessage.Text = fieldInfo.RequiredErrorMessage;
                        txtRegularExpression.Text = fieldInfo.RegularExpression;
                        txtRegularExpressionMessage.Text = fieldInfo.RegExpErrorMessage;
                        FillFieldTypeOptions();
                    }
                }
            }

        }

        protected override bool Update()
        {
            ContentTypeInfo contentTypeInfo = _contentTypeProvider.Select(ContentTypeId, ErrorList);
            if (contentTypeInfo != null)
            {
                bool isComponent = false;
                FillFieldTypeOptions();
                List<FieldInfo> allFields = _contentTypeProvider.CreateApplicationDefaultFields().ToList();
                FieldInfo[] fields = FieldInfo.GetFieldArray(contentTypeInfo.FieldsXml).Where(x => x.CreatedBy != "system").ToArray();
                FieldInfo fieldInfo = fields.FirstOrDefault(x => x.Name == Id);
                if (fieldInfo != null)
                {
                    fieldInfo.DisplayName = txtDiplayName.Text;
                    fieldInfo.ShowInListing = chbxShowInListing.Checked;
                    fieldInfo.SortOrder = ValidationHelper.GetInteger(drlOrder.SelectedValue, 0);
                    fieldInfo.IsRequired = chbxIsRequired.Checked;
                    fieldInfo.RequiredErrorMessage = txtRequiredMessage.Text;
                    fieldInfo.RegularExpression = txtRegularExpression.Text;
                    fieldInfo.RegExpErrorMessage = txtRegularExpressionMessage.Text;
                    fieldInfo.FieldType = FormHelper.GetFieldTypeByCode(drlFieldType.SelectedValue);
                    fieldInfo.Options = GetFieldOptions(ref isComponent);
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (fields[i].Name == Id)
                            fields[i] = fieldInfo;
                    }
                    allFields.AddRange(fields);
                    contentTypeInfo.FieldsXml = FieldInfo.GetFieldXml(allFields.ToArray());
                    _contentTypeProvider.UpdateField(contentTypeInfo, fieldInfo, FieldActionMode.Edit, isComponent, ErrorList);
                }
            }
            return CheckErrors();
        }

        protected override bool Insert()
        {
            bool isComponent = false;
            FillFieldTypeOptions();
            FieldInfo fieldInfo = new FieldInfo();
            fieldInfo.Name = txtColumnName.Text;
            fieldInfo.DataType = ((DataFieldType)ValidationHelper.GetInteger(drlDataType.SelectedValue, 1));
            fieldInfo.IsAllowNull = chbxIsAllowNull.Checked;
            fieldInfo.UseAsSeoTemplate = chbxUseAsSeoTemplate.Checked;
            fieldInfo.ShowInListing = chbxShowInListing.Checked;
            fieldInfo.Size = ValidationHelper.GetInteger(txtColumnSize.Text, 255);
            fieldInfo.DefaultValue = txtDeafultValue.Text;
            fieldInfo.DisplayName = txtDiplayName.Text;
            fieldInfo.FieldType = FormHelper.GetFieldTypeByCode(drlFieldType.SelectedValue);
            fieldInfo.IsRequired = chbxIsRequired.Checked;
            fieldInfo.RequiredErrorMessage = txtRequiredMessage.Text;
            fieldInfo.RegularExpression = txtRegularExpression.Text;
            fieldInfo.RegExpErrorMessage = txtRegularExpressionMessage.Text;
            fieldInfo.Options = GetFieldOptions(ref isComponent);
            fieldInfo.CreatedBy = Page.User.Identity.Name;
            fieldInfo.SortOrder = ValidationHelper.GetInteger(drlOrder.SelectedValue, 0);
            fieldInfo.CreatedDate = DateTime.Today;
            fieldInfo.ModifiedBy = Page.User.Identity.Name;
            fieldInfo.ModifiedDate = DateTime.Today;
            ContentTypeInfo contentTypeInfo = _contentTypeProvider.Select(ContentTypeId, ErrorList);
            if (contentTypeInfo != null)
            {
                List<FieldInfo> allFields = _contentTypeProvider.CreateApplicationDefaultFields().ToList();

                List<FieldInfo> fieldInfos = FieldInfo.GetFieldArray(contentTypeInfo.FieldsXml).Where(x => x.CreatedBy != "system").ToList();
                fieldInfos.Add(fieldInfo);
                allFields.AddRange(fieldInfos);

                contentTypeInfo.FieldsXml = FieldInfo.GetFieldXml(allFields.ToArray());
                if (!_contentTypeProvider.UpdateField(contentTypeInfo, fieldInfo, FieldActionMode.Create, isComponent, ErrorList))
                    _contentTypeProvider.DeleteObjectFromCache(contentTypeInfo);
                else
                {
                    _contentTypeProvider.RegisterObjectToCache(contentTypeInfo);
                }
            }


            return CheckErrors();
        }

        protected void drlFieldType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            FillFieldTypeOptions();
        }

        private void FillFieldTypeOptions()
        {
            ContentTypeInfo contentTypeInfo = _contentTypeProvider.Select(ContentTypeId, ErrorList);
            if (contentTypeInfo != null)
            {
                FieldInfo info = FieldInfo.GetFieldArray(contentTypeInfo.FieldsXml).FirstOrDefault(x => x.Name == Id);
                pnlCustomControlOptions.Controls.Clear();
                switch (FormHelper.GetFieldTypeByCode(drlFieldType.SelectedValue))
                {
                    case FormFieldType.TextBoxControl:
                        TextBoxControl control = new TextBoxControl();
                        control.ViewMode = FormControlViewMode.Development;
                        control.ID = FormFieldTypeCode.TEXTBOX;
                        if (info != null)
                            control.Value = info.Options;
                        pnlCustomControlOptions.Controls.Add(control);
                        break;
                    case FormFieldType.DatePickerControl:
                        DatePicker datepicker = new DatePicker();
                        datepicker.ViewMode = FormControlViewMode.Development;
                        datepicker.ID = FormFieldTypeCode.DATEPICKER;
                        if (info != null)
                            datepicker.Value = info.Options;
                        pnlCustomControlOptions.Controls.Add(datepicker);
                        break;
                    case FormFieldType.DateTimePickerControl:
                        DateTimePicker datetimepicker = new DateTimePicker();
                        datetimepicker.ViewMode = FormControlViewMode.Development;
                        datetimepicker.ID = FormFieldTypeCode.DATETIMEPICKER;
                        if (info != null)
                            datetimepicker.Value = info.Options;
                        pnlCustomControlOptions.Controls.Add(datetimepicker);
                        break;
                    case FormFieldType.YesNoSelector:
                        YesNoSelector yesnoselector = new YesNoSelector();
                        yesnoselector.ViewMode = FormControlViewMode.Development;
                        yesnoselector.ID = FormFieldTypeCode.YESNOSELECTOR;
                        if (info != null)
                            yesnoselector.Value = info.Options;
                        pnlCustomControlOptions.Controls.Add(yesnoselector);
                        break;
                    case FormFieldType.FileUpload:
                        FileUploader fuploader = new FileUploader();
                        fuploader.ViewMode = FormControlViewMode.Development;
                        fuploader.ID = FormFieldTypeCode.FILEUPLOAD;
                        if (info != null)
                            fuploader.Value = info.Options;
                        pnlCustomControlOptions.Controls.Add(fuploader);
                        break;
                    case FormFieldType.YearSelector:
                        YearSelector yearselector = new YearSelector();
                        yearselector.ViewMode = FormControlViewMode.Development;
                        yearselector.ID = FormFieldTypeCode.YEARSELECTOR;
                        if (info != null)
                            yearselector.Value = info.Options;
                        pnlCustomControlOptions.Controls.Add(yearselector);
                        break;
                    case FormFieldType.CaptchaControl:
                        AbstractBasicUserControl captcha =
                            (AbstractBasicUserControl)
                            this.Page.LoadControl("~/");
                        captcha.ViewMode = FormControlViewMode.Development;
                        captcha.ID = FormFieldTypeCode.CAPTCHA;
                        if (info != null)
                            captcha.Value = info.Options;
                        pnlCustomControlOptions.Controls.Add(captcha);
                        break;
                    case FormFieldType.GuidGenerator:
                        GuidGeneratorControl guidcontrol = new GuidGeneratorControl();
                        guidcontrol.ViewMode = FormControlViewMode.Development;
                        pnlCustomControlOptions.Controls.Add(guidcontrol);
                        break;
                    case FormFieldType.FckEditor:
                        FckEditorControl fckeditor = new FckEditorControl();
                        fckeditor.ID = FormFieldTypeCode.FCKEDITOR;
                        fckeditor.ViewMode = FormControlViewMode.Development;
                        if (info != null)
                            fckeditor.Value = info.Options;
                        pnlCustomControlOptions.Controls.Add(fckeditor);
                        break;
                    case FormFieldType.ListLookUp:
                        ListLookUp listLookUp = new ListLookUp();
                        listLookUp.ViewMode = FormControlViewMode.Development;
                        listLookUp.ID = FormFieldTypeCode.LISTLOOKUP;
                        if (info != null)
                            listLookUp.Value = info.Options;
                        pnlCustomControlOptions.Controls.Add(listLookUp);
                        break;
                    case FormFieldType.ContentTypeLookUp:
                        ContentTypeLookUp contentTypeLookUp = new ContentTypeLookUp();
                        contentTypeLookUp.ViewMode = FormControlViewMode.Development;
                        contentTypeLookUp.ID = FormFieldTypeCode.CONTENTTYPELOOKUP;
                        if (info != null)
                            contentTypeLookUp.Value = info.Options;
                        pnlCustomControlOptions.Controls.Add(contentTypeLookUp);
                        break;
                    case FormFieldType.ParameterGetter:
                        ParameterGetterControl parameterGetterControl = new ParameterGetterControl();
                        parameterGetterControl.ID = FormFieldTypeCode.PARAMETERGETTER;
                        parameterGetterControl.ViewMode = FormControlViewMode.Development;
                        if (info != null)
                            parameterGetterControl.Value = info.Options;
                        pnlCustomControlOptions.Controls.Add(parameterGetterControl);
                        break;
                    case FormFieldType.UserProfileGetter:
                        FromUserProfileControl userProfileControl = new FromUserProfileControl();
                        userProfileControl.ID = FormFieldTypeCode.USERPROFILEGETTER;
                        userProfileControl.ViewMode = FormControlViewMode.Development;
                        if (info != null)
                            userProfileControl.Value = info.Options;
                        pnlCustomControlOptions.Controls.Add(userProfileControl);
                        break;
                    //Load Custom Form control
                    default:
                        //AbstractBasicUserControl abstarctcontrol =
                        //    this.CreateCustomFormControl(ddlFieldType.SelectedItem.Value);
                        //abstarctcontrol.Value = info.Options;
                        //pnlCustomControlOptions.Controls.Add(abstarctcontrol);
                        break;
                }
            }
        }

        private string GetFieldOptions(ref bool isComponent)
        {
            string result = string.Empty;
            switch (drlFieldType.SelectedItem.Value)
            {
                case FormFieldTypeCode.LABEL:
                    break;
                case FormFieldTypeCode.TEXTBOX:
                    TextBoxControl tect = pnlCustomControlOptions.FindControl(FormFieldTypeCode.TEXTBOX) as TextBoxControl;
                    result = tect.Value;
                    break;
                case FormFieldTypeCode.DATETIMEPICKER:
                    DateTimePicker datetimepicker = pnlCustomControlOptions.FindControl(FormFieldTypeCode.DATETIMEPICKER) as DateTimePicker;
                    result = datetimepicker.Value;
                    break;
                case FormFieldTypeCode.DATEPICKER:
                    DatePicker datepicker = pnlCustomControlOptions.FindControl(FormFieldTypeCode.DATEPICKER) as DatePicker;
                    result = datepicker.Value;
                    break;
                case FormFieldTypeCode.LISTLOOKUP:
                    ListLookUp listlookup = pnlCustomControlOptions.FindControl(FormFieldTypeCode.LISTLOOKUP) as ListLookUp;
                    result = listlookup.Value;
                    isComponent = true;
                    break;
                case FormFieldTypeCode.CONTENTTYPELOOKUP:
                    ContentTypeLookUp contenTypeLookUp = pnlCustomControlOptions.FindControl(FormFieldTypeCode.CONTENTTYPELOOKUP) as ContentTypeLookUp;
                    result = contenTypeLookUp.Value;
                    isComponent = true;
                    break;

                case FormFieldTypeCode.YESNOSELECTOR:
                    YesNoSelector yesnoselector = pnlCustomControlOptions.FindControl(FormFieldTypeCode.YESNOSELECTOR) as YesNoSelector;
                    result = yesnoselector.Value;
                    break;
                case FormFieldTypeCode.FILEUPLOAD:
                    FileUploader fileuplaoder = pnlCustomControlOptions.FindControl(FormFieldTypeCode.FILEUPLOAD) as FileUploader;
                    result = fileuplaoder.Value;
                    break;
                case FormFieldTypeCode.YEARSELECTOR:
                    YearSelector yearselector = pnlCustomControlOptions.FindControl(FormFieldTypeCode.YEARSELECTOR) as YearSelector;
                    result = yearselector.Value;
                    break;
                case FormFieldTypeCode.CAPTCHA:
                    AbstractBasicUserControl captcha = pnlCustomControlOptions.FindControl(FormFieldTypeCode.CAPTCHA) as AbstractBasicUserControl;
                    result = captcha.Value;
                    isComponent = true;
                    break;
                case FormFieldTypeCode.GUIDGENERATOR:
                    break;
                case FormFieldTypeCode.FCKEDITOR:
                    FckEditorControl fckeditor = pnlCustomControlOptions.FindControl(FormFieldTypeCode.FCKEDITOR) as FckEditorControl;
                    result = fckeditor.Value;
                    break;
                case FormFieldTypeCode.PARAMETERGETTER:
                    ParameterGetterControl parameterGetterControl = pnlCustomControlOptions.FindControl(FormFieldTypeCode.PARAMETERGETTER) as ParameterGetterControl;
                    result = parameterGetterControl.Value;
                    break;
                case FormFieldTypeCode.USERPROFILEGETTER:
                    FromUserProfileControl userProfileControl = pnlCustomControlOptions.FindControl(FormFieldTypeCode.USERPROFILEGETTER) as FromUserProfileControl;
                    result = userProfileControl.Value;
                    break;
                default:
                    /* AbstractBasicUserControl customFormcontrol = pnlCustomControlOptions.FindControl(ddlFieldType.SelectedValue) as AbstractBasicUserControl;
                     info.CustomFormControlName = ddlFieldType.SelectedValue;
                     info.Options = customFormcontrol.Value;
                     string[] tokens = ddlFieldType.SelectedValue.Split('_');
                     FormControlInfo finfo = DataService.FormControl.ByName(tokens[1]);
                     isWidget = finfo.IsWidget;*/
                    break;

            }
            return result;
        }

        protected override void ValidateForm()
        {

        }

        protected override void PrintErrors()
        {

            StringBuilder builder = new StringBuilder();
            builder.Append("<dl id=\"system-message\">");
            builder.Append("<dt class=\"message\">Message</dt><dd class=\"error\">");
            builder.Append("<ul>");
            foreach (ErrorInfo error in ErrorList)
            {
                builder.AppendFormat("<li>{0} - {1}</li>", error.Name, error.Message);
            }
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
            ErrorList.Clear();
        }

        protected override void PrintSuccess()
        {
            ErrorList.Clear();
        }
    }
}