using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Enum;

namespace FUIControls.FormControl
{
    public sealed class YesNoSelector : AbsractBasicControl
    {
        #region Variables

        private DropDownList _drlControlTypes;
        private Panel _pnlListItems;
        private static ListItemInfo[] _listitems;

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

        public ListItemInfo[] ListItems
        {
            get
            {
                if (_listitems == null)
                {
                    var list = new List<ListItemInfo>
                                   {
                                       new ListItemInfo {Id = 1, ListName = "No", ParentId = 0},
                                       new ListItemInfo {Id = 2, ListName = "Yes", ParentId = 0}
                                   };
                    _listitems = list.ToArray();
                }
                return _listitems;
            }
        }

        #endregion

        #region Constructors

        public YesNoSelector()
            : this(null, null)
        {
        }

        public YesNoSelector(string controlId, string options)
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
                    var options = new Dictionary<string, string>();
                    options["ControlType"] = GetControlValue(_drlControlTypes.ID);
                    SetValue(GetXmlFromOptions(options));
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
                return GetXmlFromOptions(options);
            }

            bool result = false;
            if (PrivateValue.ToLower() == "true" || PrivateValue == "2" || PrivateValue.ToLower() == "on")
                result = true;
            return result.ToString();
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

                Controls.Add(table);
            }
            else
            {
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

            foreach (ListItemInfo item in ListItems)
            {
                string id = string.Format("rbtn{0}{1}", item.Id, ID);
                string name = string.Format("rbtn{0}", ID);


                var li = new WebControl(HtmlTextWriterTag.Li);
                var chbxItem = new WebControl(HtmlTextWriterTag.Input);
                var label = new WebControl(HtmlTextWriterTag.Label);

                label.Attributes.Add("for", id);
                label.Controls.Add(new LiteralControl(item.ListName));

                chbxItem.Attributes.Add("type", "radio");
                chbxItem.Attributes.Add("id", id);
                chbxItem.Attributes.Add("value", item.Id.ToString());
                chbxItem.Attributes.Add("name", name);

                if (value.ToLower() == "false" || value == "1")
                    value = "1";
                else value = "2";

                if (value == item.Id.ToString())
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
            _pnlListItems = new Panel { ID = string.Format("_pnlListItems{0}", ID) };

            List<string> selectedVals = new List<string>();

            if (!string.IsNullOrEmpty(values))
                selectedVals = values.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            WebControl ul = new WebControl(HtmlTextWriterTag.Ul) { ID = string.Format("_ulListItemsCheckbox{0}", ID) };

            foreach (ListItemInfo item in ListItems)
            {
                string id = string.Format("chbx{0}{1}", item.Id, ID);
                string name = string.Format("_chbxItem{0}", ID);


                var li = new WebControl(HtmlTextWriterTag.Li);
                var chbxItem = new WebControl(HtmlTextWriterTag.Input);
                var label = new WebControl(HtmlTextWriterTag.Label);

                label.Attributes.Add("for", id);
                label.Controls.Add(new LiteralControl(item.ListName));

                chbxItem.Attributes.Add("type", "checkbox");
                chbxItem.Attributes.Add("id", id);
                chbxItem.Attributes.Add("value", item.Id.ToString());
                chbxItem.Attributes.Add("name", name);

                if (selectedVals.Count() > 0 && selectedVals.Contains(item.Id.ToString()))
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
            var drlListItems = new DropDownList();
            drlListItems.Items.Add(new ListItem { Text = "Yes", Value = "True", Selected = true });
            drlListItems.Items.Add(new ListItem { Text = "No", Value = "False", Selected = true });
            drlListItems.ID = string.Format("_drlListItems{0}", ID);

            int selected = -1;
            if (string.IsNullOrEmpty(value))
                selected = 0;
            else
                foreach (ListItem item in drlListItems.Items)
                {
                    selected++;
                    if (item.Value == value)
                        break;
                }
            drlListItems.SelectedIndex = selected;
            _pnlListItems.Controls.Clear();
            _pnlListItems.Controls.Add(drlListItems);
        }

        #endregion
    }
}
