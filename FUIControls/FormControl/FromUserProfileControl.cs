using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Enum;

namespace FUIControls.FormControl
{
    [ToolboxData("<fr:FromUserProfileControl runat=\"server\" ID=\"FromUserProfileControl1\" />")]
    public class FromUserProfileControl : AbsractBasicControl
    {
        private TextBox _txtProfilePropertyName;
        private Label _lblValue;

        public FromUserProfileControl()
            : this(null, null)
        {
        }

        public FromUserProfileControl(string controlId, string options)
        {
            if (!string.IsNullOrEmpty(controlId))
                ID = controlId;
            if (!string.IsNullOrEmpty(options))
                SetOptions(options);
        }

        private string ProfilePropertyName
        {
            get
            {
                object value = ViewState["__profile_property_name"];
                if (value == null)
                    return string.Empty;
                return value.ToString();
            }
            set { ViewState["__profile_property_name"] = value; }
        }

        private string ProfilePropertyValue
        {
            get
            {
                object value = ViewState["__profile_property_value"];
                if (value == null)
                    return string.Empty;
                return value.ToString();
            }
            set { ViewState["__profile_property_value"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnsureControls();

            if (Page.IsPostBack)
            {
                if (ViewMode == FormControlViewMode.Development)
                {
                    _txtProfilePropertyName.Text = GetControlValue(_txtProfilePropertyName.ID);
                }
                else
                {
                    _lblValue.Text = GetUserProfile(ProfilePropertyName);
                }
            }
        }

        public override void SetValue(string value)
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                Dictionary<string, string> options = GetOptionsFromXml(value);
                if (options.ContainsKey("ProfilePropertyName") && !string.IsNullOrEmpty(options["ProfilePropertyName"]))
                    ProfilePropertyName = options["ProfilePropertyName"];
            }
            else
            {
                ProfilePropertyValue = value;
            }
        }

        public override string GetValue()
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                var resultDictionary = new Dictionary<string, string>();
                resultDictionary.Add("ProfilePropertyName", _txtProfilePropertyName.Text);
                return GetXmlFromOptions(resultDictionary);
            }
            string value = GetUserProfile(ProfilePropertyName);
            if (!string.IsNullOrEmpty(value))
                return value;
            return ProfilePropertyValue;
        }

        public override void SetOptions(string xmloptions)
        {
            if (ViewMode == FormControlViewMode.Editor)
            {
                Dictionary<string, string> options = GetOptionsFromXml(xmloptions);
                if (!string.IsNullOrEmpty(options["ProfilePropertyName"]))
                    ProfilePropertyName = options["ProfilePropertyName"];
            }
        }

        protected override void CreateChildControls()
        {
            EnsureControls();
            if (ViewMode == FormControlViewMode.Development)
            {
                var table = new HtmlTable();
                table.ID = "tblOptions";
                table.Attributes["class"] = CssClass;
                table.Width = "100%";

                table.Rows.Add(new HtmlTableRow());
                table.Rows[0].Cells.Add(new HtmlTableCell());
                table.Rows[0].Cells[0].Controls.Add(new LiteralControl("User Profile Property Name"));
                table.Rows[0].Cells[0].Attributes.Add("class", "label");
                table.Rows[0].Cells.Add(new HtmlTableCell());
                table.Rows[0].Cells[1].Controls.Add(_txtProfilePropertyName);
                Controls.Add(table);
            }
            else
            {
                Controls.Add(_lblValue);
            }
        }

        protected override void EnsureControls()
        {
            if (ViewMode == FormControlViewMode.Development)
            {

                if (_txtProfilePropertyName == null)
                {
                    _txtProfilePropertyName = new TextBox();
                    _txtProfilePropertyName.ID = string.Format("_txtProfilePropertyName{0}", ID);
                }
                _txtProfilePropertyName.Text = ProfilePropertyName;
            }
            else
            {
                if (_lblValue == null)
                {
                    _lblValue = new Label();
                }
                // _lblValue.Text = GetUserProfile(ProfilePropertyName);
            }
        }

        public override bool Validate()
        {
            if (IsRequired)
            {
                if (string.IsNullOrEmpty(GetUserProfile(ProfilePropertyName)) && string.IsNullOrEmpty(ProfilePropertyValue))
                {
                    ErrorInfo error = new ErrorInfo();
                    error.Source = FieldName;
                    error.Message = RequiredErrorMessage;
                    RegisterError(error);
                    return false;
                }
            }
            return true;
        }
    }
}
