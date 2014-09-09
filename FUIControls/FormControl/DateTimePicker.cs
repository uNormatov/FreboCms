using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FCore.Enum;
using FCore.Helper;

namespace FUIControls.FormControl
{
    [ToolboxData("<fr:DateTimePicker runat=\"server\" ID=\"DateTimePicker1\" />")]
    public sealed class DateTimePicker : AbsractBasicControl
    {
        #region Variables

        private TextBox _txtDate;
        private DropDownList _drlHour;
        private DropDownList _drlMinute;
        private DropDownList _drlAMPM;
        private CheckBox _chbxAMPM;

        #endregion

        #region Properties

        public bool IsAMPM
        {
            get
            {
                object o = ViewState["__datetimepicker_isampm"];
                if (o == null)
                    return false;
                return (bool)o;
            }
            set { ViewState["__datetimepicker_isampm"] = value; }
        }

        #endregion

        #region Constructors

        public DateTimePicker()
            : this(null, null)
        {
        }

        public DateTimePicker(string controlId, string options)
        {
            if (!string.IsNullOrEmpty(controlId))
                ID = controlId;
            if (!string.IsNullOrEmpty(options))
                SetOptions(options);
        }

        #endregion

        #region Methods

        public override void SetValue(string value)
        {
            if (ViewMode == FormControlViewMode.Editor)
            {
                EnsureControls();
                if (!string.IsNullOrEmpty(value))
                {
                    DateTime datetime = DateTime.Parse(value);
                    _txtDate.Text = datetime.Date.ToShortDateString();
                    string hour = datetime.Hour.ToString();

                    int selectedindex = -1;
                    //this code the hours
                    foreach (ListItem item in _drlHour.Items)
                    {
                        selectedindex++;
                        if (item.Value == hour)
                            break;
                    }
                    _drlHour.SelectedIndex = selectedindex;

                    string minute = datetime.Minute.ToString();
                    selectedindex = -1;
                    //this code the minutes
                    foreach (ListItem item in _drlMinute.Items)
                    {
                        selectedindex++;
                        if (item.Value == minute)
                            break;
                    }
                    _drlMinute.SelectedIndex = selectedindex;

                    if (IsAMPM)
                    {
                        string pattern = value.Substring(value.LastIndexOf("A"));
                        if (string.IsNullOrEmpty(pattern))
                            pattern = value.Substring(value.LastIndexOf("P"));

                        selectedindex = -1;
                        foreach (ListItem item in _drlAMPM.Items)
                        {
                            selectedindex++;
                            if (item.Value == pattern)
                                break;
                        }
                    }
                }
            }
            else
            {
                Dictionary<string, string> options = GetOptionsFromXml(value);
                if (options != null && options.ContainsKey("IsAMPM"))
                    IsAMPM = ValidationHelper.GetBoolean(options["IsAMPM"], false);
            }
        }

        public override string GetValue()
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                var resultDictionary = new Dictionary<string, string>();
                resultDictionary.Add("IsAMPM", _chbxAMPM.Checked.ToString());
                return GetXmlFromOptions(resultDictionary);
            }
            string result = string.Format("{0} {1}:{2}:00", _txtDate.Text.Replace("/", "."), _drlHour.SelectedValue,
                                                _drlMinute.SelectedValue);
            if (IsAMPM)
                result += _drlAMPM.SelectedValue;
            return result;
        }

        public override void SetOptions(string xmloptions)
        {
            if (!string.IsNullOrEmpty(xmloptions))
            {
                if (ViewMode == FormControlViewMode.Editor)
                {
                    Dictionary<string, string> options = GetOptionsFromXml(xmloptions);
                    IsAMPM = bool.Parse(options["IsAMPM"]);
                }
            }
        }

        protected override void EnsureControls()
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                if (_chbxAMPM == null)
                {
                    _chbxAMPM = new CheckBox();
                    _chbxAMPM.ID = string.Format("_chbxAMPM{0}", ID);
                    _chbxAMPM.Text = "Show AM/PM";
                }
                _chbxAMPM.Checked = IsAMPM;
            }
            else
            {
                if (_txtDate == null)
                {
                    _txtDate = new TextBox();
                    _txtDate.ID = string.Format("_txtDate{0}", ID);
                }
                if (_drlHour == null)
                {
                    _drlHour = new DropDownList();
                    _drlHour.ID = string.Format("_drlHour{0}", ID);

                    for (int i = 0; i <= 24; i++)
                    {
                        if (IsAMPM && i > 12) break;
                        _drlHour.Items.Add(new ListItem { Text = i.ToString(), Value = i.ToString(), Selected = (i == 0) });
                    }
                }
                if (_drlMinute == null)
                {
                    _drlMinute = new DropDownList();
                    _drlMinute.ID = string.Format("_drlMinute{0}", ID);
                    for (int i = 0; i <= 60; i += 5)
                        _drlMinute.Items.Add(new ListItem { Text = i.ToString(), Value = i.ToString(), Selected = (i == 0) });
                }
                if (IsAMPM && _drlAMPM == null)
                {
                    _drlAMPM = new DropDownList();
                    _drlAMPM.ID = string.Format("_drlAMPM{0}", ID);
                    _drlAMPM.Items.Add(new ListItem { Value = "AM", Text = "AM", Selected = true });
                    _drlAMPM.Items.Add(new ListItem { Value = "PM", Text = "PM" });
                }
            }
        }

        protected override void CreateChildControls()
        {
            EnsureControls();
            if (ViewMode == FormControlViewMode.Development)
            {
                Controls.Add(_chbxAMPM);
            }
            else
            {
                var table = new HtmlTable { Width = "100%" };
                table.Rows.Add(new HtmlTableRow());
                table.Rows[0].Cells.Add(new HtmlTableCell());
                table.Rows[0].Cells.Add(new HtmlTableCell());
                table.Rows[0].Cells.Add(new HtmlTableCell());
                table.Rows[0].Cells[0].Controls.Add(_txtDate);
                table.Rows[0].Cells[1].Controls.Add(_drlHour);
                table.Rows[0].Cells[2].Controls.Add(_drlMinute);

                if (IsAMPM)
                {
                    table.Rows[0].Cells.Add(new HtmlTableCell());
                    table.Rows[0].Cells[3].Controls.Add(_drlAMPM);
                }
                Controls.Add(table);
                var script = new StringBuilder();
                script.AppendFormat("<script>$(\"#{0}\").datepicker();</script>", _txtDate.ClientID);
                Page.ClientScript.RegisterStartupScript(typeof(DateTimePicker), _txtDate.ClientID, script.ToString());
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnsureControls();

            if (ViewMode == FormControlViewMode.Development)
            {
                if (Page.IsPostBack)
                {
                    var resultDictionary = new Dictionary<string, string>();
                    resultDictionary.Add("IsAMPM", GetControlValue(_chbxAMPM.ID));
                    SetValue(GetXmlFromOptions(resultDictionary));
                }
            }
            else
            {
                RegisterScripts();
                if (Page.IsPostBack)
                {
                    string result = string.Format("{0} {1}:{2}", GetControlValue(_txtDate.ClientID),
                                                  GetControlValue(_drlHour.ID), GetControlValue(_drlMinute.ID));
                    if (IsAMPM)
                        result += _drlAMPM.SelectedValue;
                    SetValue(result);
                }
            }
        }

        private void RegisterScripts()
        {
            string core = Page.ClientScript.GetWebResourceUrl(typeof(DateTimePicker), "PCMS.Controls.Scripts.core.js");
            string datepicker = Page.ClientScript.GetWebResourceUrl(typeof(DateTimePicker),
                                                                    "PCMS.Controls.Scripts.datepicker.js");

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("datepickercore"))
                Page.ClientScript.RegisterClientScriptInclude("datepickercore", core);
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("datepicker"))
                Page.ClientScript.RegisterClientScriptInclude("datepicker", datepicker);
        }

        #endregion
    }
}
