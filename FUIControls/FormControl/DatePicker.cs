using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Enum;
using FCore.Helper;

namespace FUIControls.FormControl
{
    [ToolboxData("<fr:DatePicker runat=\"server\" ID=\"DatePicker1\" />")]
    public sealed class DatePicker : AbsractBasicControl
    {
        #region Variables

        private TextBox _txtBase;

        #endregion

        #region Methods

        public override string GetValue()
        {
            DateTime datetime = ValidationHelper.GetDateTime(_txtBase.Text, DateTime.Today);
            
            return datetime.Year + "-" + datetime.Month + "-" + datetime.Day;
        }

        public override void SetValue(string value)
        {
            EnsureControls();
            _txtBase.Text = DateTime.Parse(value).ToShortDateString();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RegisterScripts();
            EnsureControls();
            if (Page.IsPostBack)
                _txtBase.Text = GetControlValue(_txtBase.ID);
        }

        protected override void CreateChildControls()
        {
            if (ViewMode == FormControlViewMode.Editor)
            {
                Controls.Add(_txtBase);


                string script = "<script>$(document).ready(function(){ $('#" + _txtBase.ClientID + "').datepicker();});</script>";
                Page.ClientScript.RegisterStartupScript(typeof(DatePicker), _txtBase.ClientID, script);
            }
        }

        protected override void EnsureControls()
        {
            if (_txtBase == null)
            {
                _txtBase = new TextBox();
                _txtBase.ID = string.Format("txtDatePicker{0}", ID);
            }
        }

        public override bool Validate()
        {
            bool require = true;
            bool regular = true;
            if (IsRequired)
            {
                require = !string.IsNullOrEmpty(_txtBase.Text);
                if (!require)
                {
                    ErrorInfo error = new ErrorInfo();
                    error.Message = RequiredErrorMessage;
                    error.Source = FieldName;
                    RegisterError(error);
                }

            }
            if (!string.IsNullOrEmpty(RegularExpression) && require)
            {
                Regex regex = RegexHelper.GetRegex(RegularExpression);
                regular = regex.IsMatch(_txtBase.Text);
                if (!regular)
                {
                    ErrorInfo error = new ErrorInfo();
                    error.Message = RegularExpressionErrorMessage;
                    error.Source = FieldName;
                    RegisterError(error);
                }
            }

            return require && regular;
        }

        private void RegisterScripts()
        {
            string core = Page.ClientScript.GetWebResourceUrl(typeof(DatePicker), "FUIControls.Script.core.js");
            string datepicker = Page.ClientScript.GetWebResourceUrl(typeof(DatePicker), "FUIControls.Script.datepicker.js");

            //if (!Page.ClientScript.IsClientScriptIncludeRegistered("datepickercore"))
            //    Page.ClientScript.RegisterClientScriptInclude("datepickercore", core);
            //if (!Page.ClientScript.IsClientScriptIncludeRegistered("datepicker"))
            //    Page.ClientScript.RegisterClientScriptInclude("datepicker", datepicker);
        }

        #endregion
    }
}
