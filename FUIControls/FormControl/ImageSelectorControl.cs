using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Enum;
using FCore.Helper;

namespace FUIControls.FormControl
{
    [ToolboxData("<fr:ImageSelectorControl runat=\"server\" ID=\"ImageSelectorControl1\" />")]
    public class ImageSelectorControl : AbsractBasicControl
    {
        #region Variables

        private TextBox _txtImage;

        #endregion

        #region Constructors

        public ImageSelectorControl()
            : this(null, null)
        {
        }

        public ImageSelectorControl(string controlId, string options)
        {
            if (!string.IsNullOrEmpty(controlId))
                ID = controlId;
            if (!string.IsNullOrEmpty(options))
                SetOptions(options);
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnsureControls();

            if (Page.IsPostBack)
            {
                if (ViewMode == FormControlViewMode.Editor)
                {
                    _txtImage.Text = GetControlValue(_txtImage.ID);
                }
            }
        }

        public override void SetValue(string value)
        {
            if (ViewMode == FormControlViewMode.Editor)
            {
                EnsureControls();
                if (!string.IsNullOrEmpty(value) && !_txtImage.Text.Equals(value))
                    _txtImage.Text = value;
            }
        }

        public override void SetOptions(string xmloptions)
        {
        }

        public override string GetValue()
        {
            if (ViewMode == FormControlViewMode.Editor)
            {
                var resultDictionary = new Dictionary<string, string>();

                return _txtImage.Text;
            }
            return string.Empty;
        }

        protected override void CreateChildControls()
        {
            EnsureControls();
            if (ViewMode == FormControlViewMode.Editor)
            {
                var btn = new HtmlInputButton();
                btn.Value = "...";
                Controls.Add(_txtImage);
                Controls.Add(btn);

                var script = new StringBuilder();
                script.Append("<script type=\"text/javascript\">");
                script.Append("function OpenImageSelector(){");
                script.AppendFormat("var url = '/PCMSEditor/PropertyEditors/ImageSelector.aspx?controlid={0}'; ",
                                    _txtImage.ClientID);
                script.Append(" window.open(url, 'ImageSelector', 'width=550, height=700, menubar=no, resizable=no'); ");
                script.Append("}</script>");
                Page.ClientScript.RegisterClientScriptBlock(typeof(ImageSelectorControl), "imageselectorblock",
                                                            script.ToString());
                btn.Attributes.Add("onclick", "OpenImageSelector();");
            }
        }

        protected override void EnsureControls()
        {
            if (ViewMode == FormControlViewMode.Editor)
            {
                if (_txtImage == null)
                {
                    _txtImage = new TextBox { ID = string.Format("txtImage{0}", ID) };
                }
            }
        }

        public override bool Validate()
        {
            bool require = true;
            bool regular = true;
            if (IsRequired)
            {
                require = !string.IsNullOrEmpty(_txtImage.Text);
                if (!require)
                {
                    ErrorInfo error = new ErrorInfo();
                    error.Source = FieldName;
                    error.Message = RequiredErrorMessage;
                    RegisterError(error);
                }
            }
            if (!string.IsNullOrEmpty(RegularExpression) && require)
            {
                Regex regex = RegexHelper.GetRegex(RegularExpression);
                regular = regex.IsMatch(_txtImage.Text);
                if (!regular)
                {
                    ErrorInfo error = new ErrorInfo();
                    error.Source = FieldName;
                    error.Message = RegularExpressionErrorMessage;
                    RegisterError(error);
                }
            }

            return require && regular;
        }

        #endregion
    }
}
