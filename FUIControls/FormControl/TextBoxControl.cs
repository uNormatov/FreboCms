using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Enum;
using FCore.Helper;

namespace FUIControls.FormControl
{
    [ToolboxData("<fr:TextBoxControl runat=\"server\" ID=\"TextBoxControl1\" />")]
    public sealed class TextBoxControl : AbsractBasicControl
    {
        #region Variables

        private TextBox _txtBase;
        private DropDownList _drlTextMode;
        private TextBox _txtDefaultValue;
        private TextBox _txtCssClass;
        private TextBox _txtRows;
        private TextBox _txtCols;

        #endregion

        #region Constructors

        public TextBoxControl()
            : this(null, null)
        {
        }

        public TextBoxControl(string controlId, string options)
        {
            if (!string.IsNullOrEmpty(controlId))
                ID = controlId;
            if (!string.IsNullOrEmpty(options))
                SetOptions(options);
        }

        #endregion

        #region Properties

        private TextBoxMode TextMode { get; set; }

        private string DefaultValue { get; set; }

        private string Rows { get; set; }

        private string Cols { get; set; }

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
                    _drlTextMode.SelectedValue = GetControlValue(_drlTextMode.ID);
                    _txtDefaultValue.Text = GetControlValue(_txtDefaultValue.ID);
                    _txtCssClass.Text = GetControlValue(_txtCssClass.ID);
                    _txtRows.Text = GetControlValue(_txtRows.ID);
                    _txtCols.Text = GetControlValue(_txtCols.ID);
                }
                else
                {
                    _txtBase.Text = GetControlValue(_txtBase.ID);
                }
            }
        }

        public override void SetValue(string value)
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                Dictionary<string, string> options = GetOptionsFromXml(value);
                if (!string.IsNullOrEmpty(options["DefaultValue"]))
                    DefaultValue = options["DefaultValue"];
                if (!string.IsNullOrEmpty(options["CssClass"]))
                    CssClass = options["CssClass"];
                if (!string.IsNullOrEmpty(options["TextBoxMode"]))
                    TextMode = (TextBoxMode)Enum.Parse(typeof(TextBoxMode), options["TextBoxMode"]);
                if (!string.IsNullOrEmpty(options["Rows"]))
                    Rows = options["Rows"];
                if (!string.IsNullOrEmpty(options["Cols"]))
                    Cols = options["Cols"];
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
                if (!string.IsNullOrEmpty(options["DefaultValue"]))
                    DefaultValue = options["DefaultValue"];
                if (!string.IsNullOrEmpty(options["CssClass"]))
                    CssClass = options["CssClass"];
                if (!string.IsNullOrEmpty(options["TextBoxMode"]))
                    TextMode = (TextBoxMode)Enum.Parse(typeof(TextBoxMode), options["TextBoxMode"]);
                if (!string.IsNullOrEmpty(options["Rows"]))
                    Rows = options["Rows"];
                if (!string.IsNullOrEmpty(options["Cols"]))
                    Cols = options["Cols"];
            }
        }

        public override string GetValue()
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                var resultDictionary = new Dictionary<string, string>();
                resultDictionary.Add("TextBoxMode", _drlTextMode.SelectedValue);
                resultDictionary.Add("DefaultValue", _txtDefaultValue.Text);
                resultDictionary.Add("CssClass", _txtCssClass.Text);
                resultDictionary.Add("Rows", _txtRows.Text);
                resultDictionary.Add("Cols", _txtCols.Text);
                return GetXmlFromOptions(resultDictionary);
            }
            else
            {
                return _txtBase.Text;
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
                table.Rows[0].Cells[0].Controls.Add(new LiteralControl("Text mode"));
                table.Rows[0].Cells[0].Attributes.Add("class", "label");
                table.Rows[0].Cells.Add(new HtmlTableCell());
                table.Rows[0].Cells[1].Controls.Add(_drlTextMode);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[1].Cells.Add(new HtmlTableCell());
                table.Rows[1].Cells[0].Controls.Add(new LiteralControl("Default value"));
                table.Rows[1].Cells[0].Attributes.Add("class", "label");

                table.Rows[1].Cells.Add(new HtmlTableCell());
                table.Rows[1].Cells[1].Controls.Add(_txtDefaultValue);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[2].Cells.Add(new HtmlTableCell());
                table.Rows[2].Cells[0].Controls.Add(new LiteralControl("Css class"));
                table.Rows[2].Cells[0].Attributes.Add("class", "label");

                table.Rows[2].Cells.Add(new HtmlTableCell());
                table.Rows[2].Cells[1].Controls.Add(_txtCssClass);


                if (TextMode == TextBoxMode.MultiLine)
                {
                    table.Rows.Add(new HtmlTableRow());
                    table.Rows[3].Cells.Add(new HtmlTableCell());
                    table.Rows[3].Cells[0].Controls.Add(new LiteralControl("Rows"));
                    table.Rows[3].Cells[0].Attributes.Add("class", "label");

                    table.Rows[3].Cells.Add(new HtmlTableCell());
                    table.Rows[3].Cells[1].Controls.Add(_txtRows);

                    table.Rows.Add(new HtmlTableRow());
                    table.Rows[4].Cells.Add(new HtmlTableCell());
                    table.Rows[4].Cells[0].Controls.Add(new LiteralControl("Cols"));
                    table.Rows[4].Cells[0].Attributes.Add("class", "label");

                    table.Rows[4].Cells.Add(new HtmlTableCell());
                    table.Rows[4].Cells[1].Controls.Add(_txtCols);
                }
                else
                {
                    table.Rows.Add(new HtmlTableRow());
                    table.Rows[3].Cells.Add(new HtmlTableCell());
                    table.Rows[3].Cells[0].Controls.Add(new LiteralControl("Height"));
                    table.Rows[3].Cells[0].Attributes.Add("class", "label");

                    table.Rows[3].Cells.Add(new HtmlTableCell());
                    table.Rows[3].Cells[1].Controls.Add(_txtRows);

                    table.Rows.Add(new HtmlTableRow());
                    table.Rows[4].Cells.Add(new HtmlTableCell());
                    table.Rows[4].Cells[0].Controls.Add(new LiteralControl("Width"));
                    table.Rows[4].Cells[0].Attributes.Add("class", "label");

                    table.Rows[4].Cells.Add(new HtmlTableCell());
                    table.Rows[4].Cells[1].Controls.Add(_txtCols);
                }

                Controls.Add(table);
            }
            else
            {
                Controls.Add(_txtBase);
            }
        }

        protected override void EnsureControls()
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                if (_drlTextMode == null)
                {
                    _drlTextMode = new DropDownList();
                    _drlTextMode.ID = string.Format("_drlTextBoxMode{0}", ID);
                }

                string[] names = Enum.GetNames(typeof(TextBoxMode));
                var values = (int[])Enum.GetValues(typeof(TextBoxMode));

                _drlTextMode.Items.Clear();
                for (int i = 0; i < names.Length; i++)
                {
                    _drlTextMode.Items.Add(new ListItem
                                               {
                                                   Value = values[i].ToString(),
                                                   Text = names[i],
                                                   Selected = (i == (int)TextMode)
                                               });
                }

                if (_txtDefaultValue == null)
                {
                    _txtDefaultValue = new TextBox();
                    _txtDefaultValue.ID = string.Format("txtDefaultValue{0}", ID);
                }
                if (_txtCssClass == null)
                {
                    _txtCssClass = new TextBox();
                    _txtCssClass.ID = string.Format("txtCssClass{0}", ID);
                }
                _txtCssClass.Text = CssClass;

                if (_txtCols == null)
                {
                    _txtCols = new TextBox();
                    _txtCols.ID = string.Format("_txtCols{0}", ID);
                }
                _txtCols.Text = Cols;

                if (_txtRows == null)
                {
                    _txtRows = new TextBox();
                    _txtRows.ID = string.Format("_txtRows{0}", ID);
                }
                _txtRows.Text = Rows;
            }
            else
            {
                if (_txtBase == null)
                {
                    _txtBase = new TextBox();
                    _txtBase.ID = string.Format("txtBase{0}", ID);
                }
                _txtBase.Text = DefaultValue;
                _txtBase.CssClass = CssClass;
                if (TextMode == TextBoxMode.MultiLine)
                {
                    _txtBase.TextMode = TextBoxMode.MultiLine;
                    //if (string.IsNullOrEmpty(Rows))
                    //    Rows = "15";
                    //_txtBase.Rows = int.Parse(Rows);
                    //if (string.IsNullOrEmpty(Cols))
                    //    Cols = "150";
                    //_txtBase.Columns = int.Parse(Cols);
                }
                else
                {
                    //if (string.IsNullOrEmpty(Rows))
                    //    Rows = "20";
                    //_txtBase.Height = int.Parse(Rows);
                    //if (string.IsNullOrEmpty(Cols))
                    //    Cols = "40";
                    //_txtBase.Width = int.Parse(Cols);
                }
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
                    error.Source = FieldName;
                    error.Message = RequiredErrorMessage;
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
