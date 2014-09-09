using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Enum;
using FCore.Helper;
using FredCK.FCKeditorV2;

namespace FUIControls.FormControl
{
    [ToolboxData("<fr:FckEditorControl runat=\"server\" ID=\"FckEditorControl1\" />")]
    public sealed class FckEditorControl : AbsractBasicControl
    {
        #region Variables

        private FCKeditor _fckeditor;
        private TextBox _txtWidth;
        private TextBox _txtHeight;
        private TextBox _txtBasePath;

        #endregion

        #region Constructors

        public FckEditorControl()
            : this(null, null)
        {
        }

        public FckEditorControl(string controlId, string options)
        {
            if (!string.IsNullOrEmpty(controlId))
                ID = controlId;
            if (!string.IsNullOrEmpty(options))
                SetOptions(options);
        }

        #endregion

        #region Properties

        private string DefaultValue
        {
            get
            {
                object o = ViewState["__fckeditor_default_value"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["__fckeditor_default_value"] = value; }
        }

        private string Width
        {
            get
            {
                object o = ViewState["__fckeditor_width"];
                if (o == null)
                    return "700";
                return o.ToString();
            }
            set { ViewState["__fckeditor_width"] = value; }
        }

        private string Height
        {
            get
            {
                object o = ViewState["__fckeditor_height"];
                if (o == null)
                    return "300";
                return o.ToString();
            }
            set { ViewState["__fckeditor_height"] = value; }
        }

        private string BasePath
        {
            get
            {
                object o = ViewState["__fckeditor_base_bath"];
                if (o == null)
                    return "~/Content/FCKeditor/";
                return o.ToString();
            }
            set { ViewState["__fckeditor_base_bath"] = value; }
        }

        #endregion

        #region Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnsureControls();

            if (Page.IsPostBack)
            {
                if (ViewMode == FormControlViewMode.Development)
                {
                    var resultDictionary = new Dictionary<string, string>
                                               {
                                                   {"BasePath", GetControlValue(_txtBasePath.ID)},
                                                   {"Width", GetControlValue(_txtWidth.ID)},
                                                   {"Height", GetControlValue(_txtHeight.ID)}
                                               };
                    SetValue(GetXmlFromOptions(resultDictionary));
                }
                else
                {
                    _fckeditor.Value = GetControlValue(_fckeditor.ID);
                }
            }
        }

        public override void SetValue(string value)
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                Dictionary<string, string> options = GetOptionsFromXml(value);
                if (!string.IsNullOrEmpty(options["BasePath"]))
                    BasePath = options["BasePath"];
                if (!string.IsNullOrEmpty(options["Width"]))
                    Width = options["Width"];
                if (!string.IsNullOrEmpty(options["Height"]))
                    Height = options["Height"];
            }
            else
            {
                if (!string.IsNullOrEmpty(value))
                    DefaultValue = value;
            }
        }

        public override void SetOptions(string xmloptions)
        {
            if (!string.IsNullOrEmpty(xmloptions))
            {
                Dictionary<string, string> options = GetOptionsFromXml(xmloptions);
                if (!string.IsNullOrEmpty(options["BasePath"]))
                    BasePath = options["BasePath"];
                if (!string.IsNullOrEmpty(options["Width"]))
                    Width = options["Width"];
                if (!string.IsNullOrEmpty(options["Height"]))
                    Height = options["Height"];
            }
        }

        public override string GetValue()
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                var resultDictionary = new Dictionary<string, string> { { "BasePath", BasePath }, { "Width", Width }, { "Height", Height } };
                return GetXmlFromOptions(resultDictionary);
            }
            return _fckeditor.Value;
        }

        protected override void CreateChildControls()
        {
            EnsureControls();
            if (ViewMode == FormControlViewMode.Development)
            {
                var table = new HtmlTable { ID = "tblOptions" };
                table.Attributes["class"] = CssClass;
                table.Width = "100%";

                table.Rows.Add(new HtmlTableRow());
                table.Rows[0].Cells.Add(new HtmlTableCell());
                table.Rows[0].Cells[0].Controls.Add(new LiteralControl("Base path"));
                table.Rows[0].Cells[0].Attributes.Add("class", "label");
                table.Rows[0].Cells.Add(new HtmlTableCell());
                table.Rows[0].Cells[1].Controls.Add(_txtBasePath);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[1].Cells.Add(new HtmlTableCell());
                table.Rows[1].Cells[0].Controls.Add(new LiteralControl("Width"));
                table.Rows[1].Cells[0].Attributes.Add("class", "label");

                table.Rows[1].Cells.Add(new HtmlTableCell());
                table.Rows[1].Cells[1].Controls.Add(_txtWidth);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[2].Cells.Add(new HtmlTableCell());
                table.Rows[2].Cells[0].Controls.Add(new LiteralControl("Height"));
                table.Rows[2].Cells[0].Attributes.Add("class", "label");

                table.Rows[2].Cells.Add(new HtmlTableCell());
                table.Rows[2].Cells[1].Controls.Add(_txtHeight);

                Controls.Add(table);
            }
            else
            {
                Controls.Add(_fckeditor);
            }
        }

        protected override void EnsureControls()
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                if (_txtWidth == null)
                {
                    _txtWidth = new TextBox { ID = string.Format("txtWidth{0}", ID) };
                }
                _txtWidth.Text = Width;

                if (_txtHeight == null)
                {
                    _txtHeight = new TextBox { ID = string.Format("txtHeight{0}", ID) };
                }
                _txtHeight.Text = Height;

                if (_txtBasePath == null)
                {
                    _txtBasePath = new TextBox { ID = string.Format("txtBaseFolder{0}", ID) };
                }
                _txtBasePath.Text = BasePath;
            }
            else
            {
                if (_fckeditor == null)
                {
                    _fckeditor = new FCKeditor { ID = string.Format("fckeditor{0}", ID) };
                }
                _fckeditor.EnableTheming = false;
                _fckeditor.FillEmptyBlocks = false;
                _fckeditor.Width = int.Parse(Width);
                _fckeditor.Height = int.Parse(Height);
                _fckeditor.BasePath = BasePath;
                _fckeditor.Value = DefaultValue;
            }
        }

        public override bool Validate()
        {
            bool require = true;
            bool regular = true;
            if (IsRequired)
            {
                require = !string.IsNullOrEmpty(_fckeditor.Value);
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
                regular = regex.IsMatch(_fckeditor.Value);
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

        #endregion
    }
}
