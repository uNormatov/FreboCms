using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Enum;
using FCore.Helper;

namespace FUIControls.FormControl
{
    public sealed class YearSelector : AbsractBasicControl
    {
        #region Variables

        private CheckBox _chkToCurrentYear;
        private DropDownList _drlControlTypes;
        private Panel _pnlListItems;
        private TextBox _txtDefaultText;
        private TextBox _txtDefaultTextValue;
        private TextBox _txtStartFrom;
        private TextBox _txtTo;

        #endregion

        #region Properties

        private string PrivateValue
        {
            get
            {
                object o = ViewState["__yes_no_selector_up_private_value"];
                if (o == null)
                    return string.Empty;
                return o.ToString();
            }
            set { ViewState["__yes_no_selector_up_private_value"] = value; }
        }

        public ListControlType ListControlTypeValue
        {
            get
            {
                object o = ViewState["__yes_no_selector_list_control_type"];
                if (o == null)
                    return ListControlType.DropDownList;
                return (ListControlType)o;
            }
            set { ViewState["__yes_no_selector_list_control_type"] = value; }
        }

        private int StartFrom
        {
            get
            {
                object o = ViewState["__textboctronol_start_from"];
                if (o == null)
                    return 0;
                return (int)o;
            }
            set { ViewState["__textboctronol_start_from"] = value; }
        }

        private int To
        {
            get
            {
                object o = ViewState["__textboctronol_to"];
                if (o == null)
                    return 0;
                return (int)o;
            }
            set { ViewState["__textboctronol_to"] = value; }
        }

        private string DefaultText
        {
            get
            {
                object o = ViewState["__textboctronol_default_text"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["__textboctronol_default_text"] = value; }
        }

        private string DefaultTextValue
        {
            get
            {
                object o = ViewState["__textboctronol_default_text_value"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["__textboctronol_default_text_value"] = value; }
        }

        private bool ToCurrentYear
        {
            get
            {
                object o = ViewState["__textboctronol_to_current_year"];
                if (o == null)
                    return false;
                return (bool)o;
            }
            set { ViewState["__textboctronol_to_current_year"] = value; }
        }

        #endregion

        #region Constructors

        public YearSelector()
            : this(null, null)
        {
        }

        public YearSelector(string controlId, string options)
        {
            if (!string.IsNullOrEmpty(controlId))
                ID = controlId;
            if (!string.IsNullOrEmpty(options))
                SetOptions(options);
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
                                                   {"ControlType", GetControlValue(_drlControlTypes.ID)},
                                                   {"StartFrom", GetControlValue(_txtStartFrom.ID)},
                                                   {"To", GetControlValue(_txtTo.ID)},
                                                   {"DefaultText", GetControlValue(_txtDefaultText.ID)},
                                                   {"DefaultTextValue", GetControlValue(_txtDefaultTextValue.ID)},
                                                   {
                                                       "ToCurrentYear",
                                                       (GetControlValue(_chkToCurrentYear.ID)) == "on" ? "True" : "False"
                                                       }
                                               };
                    SetValue(GetXmlFromOptions(resultDictionary));
                }
                else
                {
                    switch (ListControlTypeValue)
                    {
                        case ListControlType.DropDownList:
                            SetValue(GetControlValue(string.Format("_drlListItems{0}", ID)));
                            break;
                        case ListControlType.CheckboxList:
                            SetValue(GetControlValue(string.Format("_chbxItem{0}", ID)));
                            break;
                        case ListControlType.RadioButtonList:
                            SetValue(GetControlValue(string.Format("rbtn{0}", ID)));
                            break;
                    }
                }
            }
        }

        public override void SetOptions(string xmloptions)
        {
            if (!string.IsNullOrEmpty(xmloptions))
            {
                if (ViewMode == FormControlViewMode.Editor)
                {
                    Dictionary<string, string> options = GetOptionsFromXml(xmloptions);
                    ListControlTypeValue =
                        (ListControlType)Enum.Parse(typeof(ListControlType), options["ControlType"], true);
                    if (!string.IsNullOrEmpty(options["StartFrom"]))
                        StartFrom = int.Parse(options["StartFrom"]);
                    if (!string.IsNullOrEmpty(options["To"]))
                        To = int.Parse(options["To"]);
                    if (!string.IsNullOrEmpty(options["DefaultText"]))
                        DefaultText = options["DefaultText"];
                    if (!string.IsNullOrEmpty(options["DefaultTextValue"]))
                        DefaultTextValue = options["DefaultTextValue"];
                    if (!string.IsNullOrEmpty(options["ToCurrentYear"]))
                        ToCurrentYear = bool.Parse(options["ToCurrentYear"]);
                }
            }
        }

        public override void SetValue(string value)
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                Dictionary<string, string> options = GetOptionsFromXml(value);
                if (!string.IsNullOrEmpty(options["ControlType"]))
                    ListControlTypeValue = (ListControlType)Enum.Parse(typeof(ListControlType), options["ControlType"]);
                if (!string.IsNullOrEmpty(options["StartFrom"]))
                    StartFrom = int.Parse(options["StartFrom"]);
                if (!string.IsNullOrEmpty(options["To"]))
                    To = int.Parse(options["To"]);
                if (!string.IsNullOrEmpty(options["DefaultText"]))
                    DefaultText = options["DefaultText"];
                if (!string.IsNullOrEmpty(options["DefaultTextValue"]))
                    DefaultTextValue = options["DefaultTextValue"];
                if (!string.IsNullOrEmpty(options["ToCurrentYear"]))
                    ToCurrentYear = bool.Parse(options["ToCurrentYear"]);
            }
            else
            {
                PrivateValue = value;
            }
        }

        public override string GetValue()
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                var options = new Dictionary<string, string>();
                options["ControlType"] = ((int)ListControlTypeValue).ToString();
                options["StartFrom"] = (StartFrom).ToString();
                options["To"] = (To).ToString();
                options["DefaultText"] = DefaultText;
                options["DefaultTextValue"] = DefaultTextValue;
                options["ToCurrentYear"] = (ToCurrentYear).ToString();
                return GetXmlFromOptions(options);
            }

            string result = PrivateValue;
            PrivateValue = string.Empty;
            return result;
        }

        protected override void EnsureControls()
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                if (_drlControlTypes == null)
                {
                    _drlControlTypes = new DropDownList { ID = string.Format("_drlControlTypes{0}", ID) };
                }

                string[] names = Enum.GetNames(typeof(ListControlType));
                var values = Enum.GetValues(typeof(ListControlType)) as int[];

                _drlControlTypes.Items.Clear();
                for (int i = 0; i < names.Length; i++)
                {
                    if (values != null)
                        _drlControlTypes.Items.Add(new ListItem
                                                       {
                                                           Text = names[i],
                                                           Value = values[i].ToString(),
                                                           Selected = (i == (int)ListControlTypeValue - 1)
                                                       });
                }

                if (_txtStartFrom == null)
                {
                    _txtStartFrom = new TextBox { ID = string.Format("txtStartFrom{0}", ID) };
                }
                _txtStartFrom.Text = StartFrom.ToString();

                if (_txtTo == null)
                {
                    _txtTo = new TextBox { ID = string.Format("txtTo{0}", ID) };
                }

                _txtTo.Text = To.ToString();

                if (_txtDefaultText == null)
                {
                    _txtDefaultText = new TextBox { ID = string.Format("txtDefaultText{0}", ID) };
                }

                _txtDefaultText.Text = DefaultText;

                if (_txtDefaultTextValue == null)
                {
                    _txtDefaultTextValue = new TextBox { ID = string.Format("txtDefaultTextValue{0}", ID) };
                }

                _txtDefaultTextValue.Text = DefaultTextValue;

                if (_chkToCurrentYear == null)
                {
                    _chkToCurrentYear = new CheckBox { ID = string.Format("txtToCurrentYear{0}", ID), Text = "Current year" };
                }

                _chkToCurrentYear.Checked = ToCurrentYear;
            }
            else
            {
                switch (ListControlTypeValue)
                {
                    case ListControlType.DropDownList:
                        CreateDropDownList(PrivateValue);
                        break;
                    case ListControlType.CheckboxList:
                        CreatCheckBoxList(PrivateValue);
                        break;
                    case ListControlType.RadioButtonList:
                        CreateRadioButtonList(PrivateValue);
                        break;
                }
            }
        }

        protected override void CreateChildControls()
        {
            EnsureControls();
            if (ViewMode == FormControlViewMode.Development)
            {
                var table = new HtmlTable { Width = "100%" };
                table.Rows.Add(new HtmlTableRow());
                table.Rows[0].Cells.Add(new HtmlTableCell());
                table.Rows[0].Cells[0].Attributes.Add("class", "label");
                table.Rows[0].Cells[0].Controls.Add(new LiteralControl("List control type :"));

                table.Rows[0].Cells.Add(new HtmlTableCell());
                table.Rows[0].Cells[1].Controls.Add(_drlControlTypes);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[1].Cells.Add(new HtmlTableCell());
                table.Rows[1].Cells[0].Attributes.Add("class", "label");
                table.Rows[1].Cells[0].Controls.Add(new LiteralControl("Start from:"));

                table.Rows[1].Cells.Add(new HtmlTableCell());
                table.Rows[1].Cells[1].Controls.Add(_txtStartFrom);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[2].Cells.Add(new HtmlTableCell());
                table.Rows[2].Cells[0].Attributes.Add("class", "label");
                table.Rows[2].Cells[0].Controls.Add(new LiteralControl("To:"));

                table.Rows[2].Cells.Add(new HtmlTableCell());
                table.Rows[2].Cells[1].Controls.Add(_txtTo);
                table.Rows[2].Cells[1].Controls.Add(_chkToCurrentYear);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[3].Cells.Add(new HtmlTableCell());
                table.Rows[3].Cells[0].Attributes.Add("class", "label");
                table.Rows[3].Cells[0].Controls.Add(new LiteralControl("Default text:"));

                table.Rows[3].Cells.Add(new HtmlTableCell());
                table.Rows[3].Cells[1].Controls.Add(_txtDefaultText);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[4].Cells.Add(new HtmlTableCell());
                table.Rows[4].Cells[0].Attributes.Add("class", "label");
                table.Rows[4].Cells[0].Controls.Add(new LiteralControl("Default text value:"));

                table.Rows[4].Cells.Add(new HtmlTableCell());
                table.Rows[4].Cells[1].Controls.Add(_txtDefaultTextValue);

                Controls.Add(table);
            }
            else
            {
                if (ListControlTypeValue != ListControlType.DropDownList)
                    Controls.Add(_pnlListItems);
            }
        }

        private void CreateRadioButtonList(string value)
        {
            if (_pnlListItems == null)
            {
                _pnlListItems = new Panel { ID = string.Format("_pnlListItems{0}", ID) };
            }
            var ul = new WebControl(HtmlTextWriterTag.Ul) { ID = string.Format("_ulListItemsRario{0}", ID) };

            int to = (ToCurrentYear ? DateTime.Now.Year : To);

            for (int i = to; i <= StartFrom; i--)
            {
                string id = string.Format("rbtn{0}{1}", i, ID);
                string name = string.Format("rbtn{0}", ID);


                var li = new WebControl(HtmlTextWriterTag.Li);
                var chbxItem = new WebControl(HtmlTextWriterTag.Input);
                var label = new WebControl(HtmlTextWriterTag.Label);

                label.Attributes.Add("for", id);
                label.Controls.Add(new LiteralControl(i.ToString()));

                chbxItem.Attributes.Add("type", "radio");
                chbxItem.Attributes.Add("id", id);
                chbxItem.Attributes.Add("value", i.ToString());
                chbxItem.Attributes.Add("name", name);

                if (value.ToLower() == "false" || value == "1")
                    value = "1";
                else value = "2";

                if (value == i.ToString())
                    chbxItem.Attributes.Add("checked", "true");
                li.Controls.Add(chbxItem);
                li.Controls.Add(label);
                ul.Controls.Add(li);
            }
            _pnlListItems.Controls.Clear();
            _pnlListItems.Controls.Add(ul);
        }

        private void CreatCheckBoxList(string values)
        {
            _pnlListItems = new Panel();
            _pnlListItems = new Panel { ID = string.Format("_pnlListItems{0}", ID) };

            List<string> selectedVals = new List<string>();

            if (!string.IsNullOrEmpty(values))
                selectedVals = values.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var ul = new WebControl(HtmlTextWriterTag.Ul) { ID = string.Format("_ulListItemsCheckbox{0}", ID) };

            int to = (ToCurrentYear ? DateTime.Now.Year : To);

            for (int i = to; i <= StartFrom; i--)
            {
                string id = string.Format("chbx{0}{1}", i, ID);
                string name = string.Format("_chbxItem{0}", ID);


                var li = new WebControl(HtmlTextWriterTag.Li);
                var chbxItem = new WebControl(HtmlTextWriterTag.Input);
                var label = new WebControl(HtmlTextWriterTag.Label);

                label.Attributes.Add("for", id);
                label.Controls.Add(new LiteralControl(i.ToString()));

                chbxItem.Attributes.Add("type", "checkbox");
                chbxItem.Attributes.Add("id", id);
                chbxItem.Attributes.Add("value", i.ToString());
                chbxItem.Attributes.Add("name", name);

                if (selectedVals.Count() > 0 && selectedVals.Contains(i.ToString()))
                    chbxItem.Attributes.Add("checked", "true");
                li.Controls.Add(chbxItem);
                li.Controls.Add(label);
                ul.Controls.Add(li);
            }
            _pnlListItems.Controls.Clear();
            _pnlListItems.Controls.Add(ul);
        }

        private void CreateDropDownList(string value)
        {
            if (_pnlListItems == null)
            {
                _pnlListItems = new Panel { ID = string.Format("_pnlListItems{0}", ID) };
            }

            DropDownList drlListItems = new DropDownList { ID = string.Format("_drlListItems{0}", ID) };
            drlListItems.Items.Clear();
            drlListItems.Items.Add(new ListItem { Text = DefaultText, Value = DefaultTextValue, Selected = true });
            int to = (ToCurrentYear ? DateTime.Now.Year : To);

            for (int i = to; i >= StartFrom; i--)
            {
                drlListItems.Items.Add(new ListItem { Text = i.ToString(), Value = i.ToString() });
            }

            int selected = -1;
            if (string.IsNullOrEmpty(value))
                selected = 0;
            else
                drlListItems.SelectedValue = value;

            Controls.Clear();
            Controls.Add(drlListItems);
        }

        public override bool Validate()
        {
            bool require = true;
            if (IsRequired)
            {
                require = !string.IsNullOrEmpty(PrivateValue) && !PrivateValue.Equals("0");
                if (!require)
                {
                    ErrorInfo error = new ErrorInfo();
                    error.Message = RequiredErrorMessage;
                    error.Source = FieldName;
                    RegisterError(error);
                }
            }
            return require;
        }

        #endregion
    }
}