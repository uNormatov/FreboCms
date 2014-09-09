using System;
using System.Collections.Generic;
using System.Linq;
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
    [ToolboxData("<fr:ParameterGetterControl runat=\"server\" ID=\"ParameterGetterControl1\" />")]
    public sealed class ParameterGetterControl : AbsractBasicControl
    {
        private DropDownList _drlQueryParameterType;
        private TextBox _txtQueryParameterName;
        private Label _lblValue;

        public ParameterGetterControl()
            : this(null, null)
        {
        }

        public ParameterGetterControl(string controlId, string options)
        {
            if (!string.IsNullOrEmpty(controlId))
                ID = controlId;
            if (!string.IsNullOrEmpty(options))
                SetOptions(options);
        }

        private QueryParameterType QueryParameterType
        {
            get
            {
                object value = ViewState["_query_parameter_type"];
                if (value == null)
                    return QueryParameterType.QueryString;
                return (QueryParameterType)Enum.Parse(typeof(QueryParameterType), value.ToString()); ;
            }
            set { ViewState["_query_parameter_type"] = value; }
        }

        private string QueryParameterName
        {
            get
            {
                object value = ViewState["__query_parameter_name"];
                if (value == null)
                    return string.Empty;
                return value.ToString();
            }
            set { ViewState["__query_parameter_name"] = value; }
        }

        private string QueryParameterValue
        {
            get
            {
                object value = ViewState["__query_parameter_value"];
                if (value == null)
                    return string.Empty;
                return value.ToString();
            }
            set { ViewState["__query_parameter_value"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnsureControls();

            if (Page.IsPostBack)
            {
                if (ViewMode == FormControlViewMode.Development)
                {
                    _drlQueryParameterType.SelectedValue = GetControlValue(_drlQueryParameterType.ID);
                    _txtQueryParameterName.Text = GetControlValue(_txtQueryParameterName.ID);
                }
                else
                {
                    _lblValue.Text = GetParameterValue((int)QueryParameterType, QueryParameterName);
                }
            }
        }

        public override void SetValue(string value)
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                Dictionary<string, string> options = GetOptionsFromXml(value);
                if (options.ContainsKey("QueryParameterType") && !string.IsNullOrEmpty(options["QueryParameterType"]))
                    QueryParameterType = (QueryParameterType)Enum.Parse(typeof(QueryParameterType), options["QueryParameterType"]);
                if (options.ContainsKey("QueryParameterType") && !string.IsNullOrEmpty(options["QueryParameterName"]))
                    QueryParameterName = options["QueryParameterName"];
            }
            else
            {
                QueryParameterValue = value;
            }
        }

        public override string GetValue()
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                var resultDictionary = new Dictionary<string, string>();
                resultDictionary.Add("QueryParameterType", _drlQueryParameterType.SelectedValue);
                resultDictionary.Add("QueryParameterName", _txtQueryParameterName.Text);
                return GetXmlFromOptions(resultDictionary);
            }
            else
            {
                string value = GetParameterValue((int)QueryParameterType, QueryParameterName);
                if (!string.IsNullOrEmpty(value))
                    return value;
                return QueryParameterValue;
            }
        }

        public override void SetOptions(string xmloptions)
        {
            if (ViewMode == FormControlViewMode.Editor)
            {
                Dictionary<string, string> options = GetOptionsFromXml(xmloptions);
                if (!string.IsNullOrEmpty(options["QueryParameterType"]))
                    QueryParameterType = (QueryParameterType)Enum.Parse(typeof(QueryParameterType), options["QueryParameterType"]);
                if (!string.IsNullOrEmpty(options["QueryParameterName"]))
                    QueryParameterName = options["QueryParameterName"];
            }
        }

        private string GetParameterValue(int type, string value)
        {
            if (type == (int)QueryParameterType.QueryString)
            {
                return GetQueryValue(value, string.Empty);
            }
            if (type == (int)QueryParameterType.SeoTemplate)
            {
                return GetSeoValue(value, string.Empty);
            }
            if (type == ((int)QueryParameterType.Cookie))
            {
                return GetCookieValue(value, string.Empty);
            }
            if (type == ((int)QueryParameterType.Language))
            {
                return GetCurrentLanguage();
            }
            return string.Empty;
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
                table.Rows[0].Cells[0].Controls.Add(new LiteralControl("Query Parameter Type"));
                table.Rows[0].Cells[0].Attributes.Add("class", "label");
                table.Rows[0].Cells.Add(new HtmlTableCell());
                table.Rows[0].Cells[1].Controls.Add(_drlQueryParameterType);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[1].Cells.Add(new HtmlTableCell());
                table.Rows[1].Cells[0].Controls.Add(new LiteralControl("Query Parameter Name"));
                table.Rows[1].Cells[0].Attributes.Add("class", "label");

                table.Rows[1].Cells.Add(new HtmlTableCell());
                table.Rows[1].Cells[1].Controls.Add(_txtQueryParameterName);
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
                if (_drlQueryParameterType == null)
                {
                    _drlQueryParameterType = new DropDownList();
                    _drlQueryParameterType.ID = string.Format("_drlQueryParameterType{0}", ID);
                }

                string[] names = Enum.GetNames(typeof(QueryParameterType));
                var values = (int[])Enum.GetValues(typeof(QueryParameterType));

                _drlQueryParameterType.Items.Clear();
                for (int i = 0; i < names.Length; i++)
                {
                    _drlQueryParameterType.Items.Add(new ListItem
                    {
                        Value = values[i].ToString(),
                        Text = names[i],
                        Selected = (values[i] == (int)QueryParameterType)
                    });
                }

                if (_txtQueryParameterName == null)
                {
                    _txtQueryParameterName = new TextBox();
                    _txtQueryParameterName.ID = string.Format("txtQueryParameterName{0}", ID);
                }
                _txtQueryParameterName.Text = QueryParameterName;
            }
            else
            {
                if (_lblValue == null)
                {
                    _lblValue = new Label();
                }
                _lblValue.Text = GetParameterValue((int)QueryParameterType, QueryParameterName);
            }
        }

        public override bool Validate()
        {
            if (IsRequired)
            {
                if (string.IsNullOrEmpty(GetParameterValue((int)QueryParameterType, QueryParameterName)) && string.IsNullOrEmpty(QueryParameterValue))
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

