using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;

namespace FUIControls.FormControl
{
    [ToolboxData("<fr:ContentTypeLookUp runat=\"server\" ID=\"ContentTypeLookUp1\" />")]
    public sealed class ContentTypeLookUp : AbsractBasicControl, IComponentControl
    {
        private readonly GeneralConnection _generalConnection;
        private readonly ContentTypeProvider _contentTypeProvider;

        private DropDownList _drlControlTypes;
        private DropDownList _drlContentTypes;
        private DropDownList _drlHtmlTag;
        private TextBox _txtDisplayMember;
        private TextBox _txtValueMember;
        private TextBox _txtWidth;

        private TextBox _txtContainerCssClass;
        private TextBox _txtItemCssClass;
        private TextBox _txtRepeatColumn;

        private Panel _pnlListItems;

        #region Constructors

        public ContentTypeLookUp()
            : this(null, null, null, null)
        {
        }

        public ContentTypeLookUp(string controlId, string options, GeneralConnection generalConnection,
                                 ErrorInfoList errors)
        {
            if (!string.IsNullOrEmpty(controlId))
                ID = controlId;
            if (!string.IsNullOrEmpty(options))
                SetOptions(options);

            _generalConnection = generalConnection ?? new GeneralConnection();

            if (errors != null)
                ErrorInfoList = errors;

            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();
        }

        #endregion

        private string PrivateValue
        {
            get
            {
                object o = ViewState["__contenttype_look_up_private_value"];
                if (o == null)
                    return string.Empty;
                return o.ToString();
            }
            set { ViewState["__contenttype_look_up_private_value"] = value; }
        }

        public string ContainerCssClass
        {
            get
            {
                object o = ViewState["__contenttype_lookup_container_css"];
                if (o == null)
                    return string.Empty;
                return o.ToString();
            }
            set { ViewState["__contenttype_lookup_container_css"] = value; }
        }

        public string ItemCssClass
        {
            get
            {
                object o = ViewState["__contenttype_lookup_item_css"];
                if (o == null)
                    return string.Empty;
                return o.ToString();
            }
            set { ViewState["__contenttype_lookup_item_css"] = value; }
        }

        public int RepeatColumn
        {
            get
            {
                object o = ViewState["__contenttype_lookup_repeat_column"];
                if (o == null)
                    return 1;
                return (int)o;
            }
            set { ViewState["__contenttype_lookup_repeat_column"] = value; }
        }

        public int HtmlTag
        {
            get
            {
                object o = ViewState["__contenttype_lookup_Html_tab"];
                if (o == null)
                    return 1;
                return (int)o;
            }
            set { ViewState["__contenttype_lookup_Html_tab"] = value; }
        }

        public string DisplayMember
        {
            get
            {
                object o = ViewState["__contenttype_display_member"];
                if (o == null)
                    return string.Empty;
                return o.ToString();
            }
            set { ViewState["__contenttype_display_member"] = value; }
        }

        public string ValueMember
        {
            get
            {
                object o = ViewState["__contenttype_value_member"];
                if (o == null)
                    return string.Empty;
                return o.ToString();
            }
            set { ViewState["__contenttype_value_member"] = value; }
        }

        public string Width
        {
            get
            {
                object o = ViewState["__contenttype_width"];
                if (o == null)
                    return "200";
                return o.ToString();
            }
            set { ViewState["__contenttype_width"] = value; }
        }

        public int ContentTypeId
        {
            get
            {
                object o = ViewState["__contenttype_lookup_contenttype_id"];
                if (o == null)
                    return 0;
                return (int)o;
            }
            set { ViewState["__contenttype_lookup_contenttype_id"] = value; }
        }

        public ListControlType ListControlType
        {
            get
            {
                object o = ViewState["__contenttype_lookup_list_control_type"];
                if (o == null)
                    return ListControlType.DropDownList;
                return (ListControlType)o;
            }
            set { ViewState["__contenttype_lookup_list_control_type"] = value; }
        }

        public DataRowCollection ContentItems
        {
            get
            {
                object o = ViewState["__contenttype_lookup_content_items"];
                if (o == null)
                {
                    o = GetRows();
                    ViewState["__contenttype_lookup_content_items"] = o;
                }
                return (DataRowCollection)o;
            }
        }

        public bool IsComponent()
        {
            return true;
        }

        public override void SetOptions(string xmloptions)
        {
            if (!string.IsNullOrEmpty(xmloptions))
            {
                Dictionary<string, string> options = GetOptionsFromXml(xmloptions);
                if (options.ContainsKey("ControlType"))
                    ListControlType = (ListControlType)Enum.Parse(typeof(TextBoxMode), options["ControlType"], true);
                if (options.ContainsKey("ContentTypeId"))
                    ContentTypeId = int.Parse(options["ContentTypeId"]);
                if (options.ContainsKey("ContainerCss"))
                    ContainerCssClass = options["ContainerCSS"];
                if (options.ContainsKey("ItemCss"))
                    ItemCssClass = options["ItemCSS"];
                if (options.ContainsKey("RepeatColumn"))
                    RepeatColumn = int.Parse(options["RepeatColumn"]);
                if (options.ContainsKey("HtmlTag"))
                    HtmlTag = int.Parse(options["HtmlTag"]);
                if (options.ContainsKey("DisplayMember"))
                    DisplayMember = options["DisplayMember"];
                if (options.ContainsKey("ValueMember"))
                    ValueMember = options["ValueMember"];
                if (options.ContainsKey("Width"))
                    Width = options["Width"];
            }
        }

        public override void SetValue(string value)
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                Dictionary<string, string> options = GetOptionsFromXml(value);

                if (options.ContainsKey("ControlType") && !string.IsNullOrEmpty(options["ControlType"]))
                    ListControlType = (ListControlType)Enum.Parse(typeof(ListControlType), options["ControlType"]);

                if (options.ContainsKey("ContentTypeId") && !string.IsNullOrEmpty(options["ContentTypeId"]))
                    ContentTypeId = int.Parse(options["ContentTypeId"]);

                if (options.ContainsKey("ContainerCSS") && !string.IsNullOrEmpty(options["ContainerCSS"]))
                    ContainerCssClass = options["ContainerCSS"];

                if (options.ContainsKey("ItemCSS") && !string.IsNullOrEmpty(options["ItemCSS"]))
                    ItemCssClass = options["ItemCSS"];

                if (options.ContainsKey("RepeatColumn") && !string.IsNullOrEmpty(options["RepeatColumn"]))
                    RepeatColumn = int.Parse(options["RepeatColumn"]);

                if (options.ContainsKey("HtmlTag") && !string.IsNullOrEmpty(options["HtmlTag"]))
                    HtmlTag = int.Parse(options["HtmlTag"]);

                if (options.ContainsKey("DisplayMember") && !string.IsNullOrEmpty(options["DisplayMember"]))
                    DisplayMember = options["DisplayMember"];

                if (options.ContainsKey("ValueMember") && !string.IsNullOrEmpty(options["ValueMember"]))
                    ValueMember = options["ValueMember"];

                if (options.ContainsKey("Width") && !string.IsNullOrEmpty(options["Width"]))
                    Width = options["Width"];
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
                options["ControlType"] = ((int)ListControlType).ToString();
                options["ContentTypeId"] = ContentTypeId.ToString();
                options["ContainerCSS"] = ContainerCssClass;
                options["ItemCSS"] = ItemCssClass;
                options["RepeatColumn"] = RepeatColumn.ToString();
                options["HtmlTag"] = HtmlTag.ToString();
                options["DisplayMember"] = DisplayMember;
                options["ValueMember"] = ValueMember;
                options["Width"] = Width;
                return GetXmlFromOptions(options);
            }
            string result = PrivateValue;
            PrivateValue = string.Empty;
            return result;
        }

        public void SelectData(int contentTypeId, int contentId, string fieldName)
        {
            var pars = new object[4, 3];
            pars[0, 0] = "@FromContentTypeId";
            pars[0, 1] = ContentTypeId;
            pars[1, 0] = "@ToContentTypeId";
            pars[1, 1] = contentTypeId;
            pars[2, 0] = "@ToContentId";
            pars[2, 1] = contentId;
            pars[3, 0] = "@FieldName";
            pars[3, 1] = fieldName;
            DataTable dataTable = _generalConnection.ExecuteDataTableQuery("freb_ContentToContentLookUpList_Select",
                                                                           pars,
                                                                           QueryType.StoredProcedure, ErrorInfoList);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                ListItemInfo[] listItems = ListItemInfo.GetArray(dataTable);
                string value = string.Empty;
                foreach (ListItemInfo item in listItems)
                {
                    if (!string.IsNullOrEmpty(value))
                        value += ",";
                    value += item.Id;
                }
                PrivateValue = value;
            }
        }

        public void InsertData(int contentTypeId, int contentId, string fieldName)
        {
            if (ViewMode == FormControlViewMode.Editor)
            {
                switch (ListControlType)
                {
                    case ListControlType.DropDownList:
                        Execute(new[] { Value }, contentTypeId, contentId, fieldName, false);
                        break;
                    case ListControlType.CheckboxList:
                        Execute(Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries), contentTypeId,
                                contentId, fieldName, false);
                        break;
                    case ListControlType.RadioButtonList:
                        Execute(new[] { Value }, contentTypeId, contentId, fieldName, false);
                        break;
                }
            }
        }

        public void UpdateData(int contentTypeId, int contentId, string fieldName)
        {
            if (ViewMode == FormControlViewMode.Editor)
            {
                switch (ListControlType)
                {
                    case ListControlType.DropDownList:
                        Execute(new[] { Value }, contentTypeId, contentId, fieldName, true);
                        break;
                    case ListControlType.CheckboxList:
                        Execute(Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries), contentTypeId,
                                contentId, fieldName, true);
                        break;
                    case ListControlType.RadioButtonList:
                        Execute(new[] { Value }, contentTypeId, contentId, fieldName, true);
                        break;
                }
            }
        }

        protected override void CreateChildControls()
        {
            EnsureControls();
            if (ViewMode == FormControlViewMode.Development)
            {
                var table = new HtmlTable();

                table.Rows.Add(new HtmlTableRow());
                table.Width = "100%";
                table.Rows[0].Cells.Add(new HtmlTableCell());
                table.Rows[0].Cells[0].Attributes.Add("class", "label");
                table.Rows[0].Cells[0].Controls.Add(new LiteralControl("List control type :"));
                table.Rows[0].Cells.Add(new HtmlTableCell());
                table.Rows[0].Cells[1].Controls.Add(_drlControlTypes);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[1].Cells.Add(new HtmlTableCell());
                table.Rows[1].Cells[0].Controls.Add(new LiteralControl("Content type :"));
                table.Rows[1].Cells[0].Attributes.Add("class", "label");
                table.Rows[1].Cells.Add(new HtmlTableCell());
                table.Rows[1].Cells[1].Controls.Add(_drlContentTypes);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[2].Cells.Add(new HtmlTableCell());
                table.Rows[2].Cells[0].Controls.Add(new LiteralControl("Display Member:"));
                table.Rows[2].Cells[0].Attributes.Add("class", "label");
                table.Rows[2].Cells.Add(new HtmlTableCell());
                table.Rows[2].Cells[1].Controls.Add(_txtDisplayMember);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[3].Cells.Add(new HtmlTableCell());
                table.Rows[3].Cells[0].Controls.Add(new LiteralControl("Value member:"));
                table.Rows[3].Cells[0].Attributes.Add("class", "label");
                table.Rows[3].Cells.Add(new HtmlTableCell());
                table.Rows[3].Cells[1].Controls.Add(_txtValueMember);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[4].Cells.Add(new HtmlTableCell());
                table.Rows[4].Cells[0].Controls.Add(new LiteralControl("Container CSS:"));
                table.Rows[4].Cells[0].Attributes.Add("class", "label");
                table.Rows[4].Cells.Add(new HtmlTableCell());
                table.Rows[4].Cells[1].Controls.Add(_txtContainerCssClass);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[5].Cells.Add(new HtmlTableCell());
                table.Rows[5].Cells[0].Controls.Add(new LiteralControl("Item CSS:"));
                table.Rows[5].Cells[0].Attributes.Add("class", "label");
                table.Rows[5].Cells.Add(new HtmlTableCell());
                table.Rows[5].Cells[1].Controls.Add(_txtItemCssClass);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[6].Cells.Add(new HtmlTableCell());
                table.Rows[6].Cells[0].Controls.Add(new LiteralControl("Repeat column:"));
                table.Rows[6].Cells[0].Attributes.Add("class", "label");
                table.Rows[6].Cells.Add(new HtmlTableCell());
                table.Rows[6].Cells[1].Controls.Add(_txtRepeatColumn);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[7].Cells.Add(new HtmlTableCell());
                table.Rows[7].Cells[0].Controls.Add(new LiteralControl("Html tag:"));
                table.Rows[7].Cells[0].Attributes.Add("class", "label");
                table.Rows[7].Cells.Add(new HtmlTableCell());
                table.Rows[7].Cells[1].Controls.Add(_drlHtmlTag);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[7].Cells.Add(new HtmlTableCell());
                table.Rows[7].Cells[0].Controls.Add(new LiteralControl("Html tag:"));
                table.Rows[7].Cells[0].Attributes.Add("class", "label");
                table.Rows[7].Cells.Add(new HtmlTableCell());
                table.Rows[7].Cells[1].Controls.Add(_drlHtmlTag);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[8].Cells.Add(new HtmlTableCell());
                table.Rows[8].Cells[0].Controls.Add(new LiteralControl("Width:"));
                table.Rows[8].Cells[0].Attributes.Add("class", "label");
                table.Rows[8].Cells.Add(new HtmlTableCell());
                table.Rows[8].Cells[1].Controls.Add(_txtWidth);

                Controls.Add(table);
            }
            else
            {
                if (ListControlType != ListControlType.DropDownList)
                    Controls.Add(_pnlListItems);
            }
        }

        protected override void EnsureControls()
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                if (_drlControlTypes == null)
                {
                    _drlControlTypes = new DropDownList();
                    _drlControlTypes.ID = string.Format("_drlControlTypes{0}", ID);
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
                                                           Selected = (i == (int)ListControlType - 1)
                                                       });
                }

                if (_drlContentTypes == null)
                {
                    _drlContentTypes = new DropDownList();
                    _drlContentTypes.ID = string.Format("_drlContentTypes{0}", ID);
                    _drlContentTypes.DataSource = _contentTypeProvider.SelectAll(ErrorInfoList);
                    _drlContentTypes.DataTextField = "Name";
                    _drlContentTypes.DataValueField = "Id";
                    _drlContentTypes.DataBind();
                }

                _drlContentTypes.SelectedValue = ContentTypeId.ToString();

                if (_txtDisplayMember == null)
                {
                    _txtDisplayMember = new TextBox();
                    _txtDisplayMember.ID = string.Format("_txtDisplayMember{0}", ID);
                }
                if (_txtValueMember == null)
                {
                    _txtValueMember = new TextBox();
                    _txtValueMember.ID = string.Format("_txtValueMember{0}", ID);
                }

                if (_txtContainerCssClass == null)
                {
                    _txtContainerCssClass = new TextBox();
                    _txtContainerCssClass.ID = string.Format("_txtContainerCssClass{0}", ID);
                }
                _txtContainerCssClass.Text = ContainerCssClass;


                if (_txtItemCssClass == null)
                {
                    _txtItemCssClass = new TextBox();
                    _txtItemCssClass.ID = string.Format("_txtItemCssClass{0}", ID);
                }
                _txtItemCssClass.Text = ItemCssClass;


                if (_txtRepeatColumn == null)
                {
                    _txtRepeatColumn = new TextBox();
                    _txtRepeatColumn.ID = string.Format("_txtRepeatColumn{0}", ID);
                }
                _txtRepeatColumn.Text = RepeatColumn.ToString();

                if (_drlHtmlTag == null)
                {
                    _drlHtmlTag = new DropDownList();
                    _drlHtmlTag.ID = string.Format("_drlHtmlTag{0}", ID);
                }

                string[] tagNames = Enum.GetNames(typeof(HtmlTagType));
                var tagValues = Enum.GetValues(typeof(HtmlTagType)) as int[];

                _drlHtmlTag.Items.Clear();

                for (int i = 0; i < tagNames.Length; i++)
                {
                    if (tagValues != null)
                        _drlHtmlTag.Items.Add(new ListItem
                                                  {
                                                      Text = tagNames[i],
                                                      Value = tagValues[i].ToString(),
                                                      Selected = (i == HtmlTag - 1)
                                                  });
                }
                if (_txtWidth == null)
                {
                    _txtWidth = new TextBox();
                    _txtWidth.ID = string.Format("_txtWidth{0}", ID);
                }
                _txtWidth.Text = Width;
            }
            else
            {
                switch (ListControlType)
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
                    case ListControlType.TextBoxList:
                        CreateTextBoxList(PrivateValue);
                        break;
                }
            }
        }

        private void CreateRadioButtonList(string value)
        {
            if (_pnlListItems == null)
            {
                _pnlListItems = new Panel();
                _pnlListItems.ID = string.Format("_pnlListItems{0}", ID);
                _pnlListItems.Attributes.Add("style", "overflow:auto; with:300; height:100;");
            }

            var table = new WebControl(HtmlTextWriterTag.Table);
            table.Attributes.Add("width", "100%");
            var ul = new WebControl(HtmlTextWriterTag.Ul);
            var tr = new WebControl(HtmlTextWriterTag.Tr);

            if (HtmlTag == 1)
            {
                table.ID = string.Format("_ulListItemsRario{0}", ID);
            }
            else
            {
                ul.ID = string.Format("_ulListItemsRario{0}", ID);
            }

            int repeater = 0;
            foreach (DataRow item in ContentItems)
            {
                string displayValue = GetRowValueByDisplayMember(item);
                string valueValue = ValidationHelper.GetString(item[ValueMember], string.Empty);

                string id = string.Format("rbtn{0}{1}", valueValue, ID);
                string name = string.Format("rbtn{0}", ID);

                //Table
                if (HtmlTag == 1)
                {
                    if (repeater == RepeatColumn)
                    {
                        tr = new WebControl(HtmlTextWriterTag.Tr);
                        repeater = 0;
                    }
                    var td = new WebControl(HtmlTextWriterTag.Td);
                    var chbxItem = new WebControl(HtmlTextWriterTag.Input);
                    var label = new WebControl(HtmlTextWriterTag.Label);

                    label.Attributes.Add("for", id);
                    label.Controls.Add(new LiteralControl(displayValue));

                    chbxItem.Attributes.Add("type", "radio");
                    chbxItem.Attributes.Add("id", id);
                    chbxItem.Attributes.Add("value", valueValue);
                    chbxItem.Attributes.Add("name", name);

                    if (value == valueValue)
                        chbxItem.Attributes.Add("checked", "true");
                    td.Controls.Add(chbxItem);
                    td.Controls.Add(label);
                    tr.Controls.Add(td);
                    table.Controls.Add(tr);
                }
                else //Ul
                {
                    var li = new WebControl(HtmlTextWriterTag.Li);
                    var chbxItem = new WebControl(HtmlTextWriterTag.Input);
                    var label = new WebControl(HtmlTextWriterTag.Label);

                    label.Attributes.Add("for", id);
                    label.Controls.Add(new LiteralControl(displayValue));

                    chbxItem.Attributes.Add("type", "radio");
                    chbxItem.Attributes.Add("id", id);
                    chbxItem.Attributes.Add("value", valueValue);
                    chbxItem.Attributes.Add("name", name);

                    if (value == valueValue)
                        chbxItem.Attributes.Add("checked", "true");
                    li.Controls.Add(chbxItem);
                    li.Controls.Add(label);
                    ul.Controls.Add(li);
                }

                repeater++;
            }
            _pnlListItems.Controls.Clear();

            if (HtmlTag == 1)
            {
                _pnlListItems.Controls.Add(table);
            }
            else
            {
                _pnlListItems.Controls.Add(ul);
            }
        }

        private void CreatCheckBoxList(string values)
        {
            if (_pnlListItems == null)
            {
                _pnlListItems = new Panel();
                _pnlListItems.ID = string.Format("_pnlListItems{0}", ID);
                _pnlListItems.Attributes.Add("style", "overflow:auto; with:300; height:100; ");
            }
            var selectedVals = new List<string>();

            if (!string.IsNullOrEmpty(values))
                selectedVals = values.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var table = new WebControl(HtmlTextWriterTag.Table);
            table.Attributes.Add("width", "100%");
            var ul = new WebControl(HtmlTextWriterTag.Ul);
            var tr = new WebControl(HtmlTextWriterTag.Tr);

            if (HtmlTag == 1)
            {
                table.ID = string.Format("_ulListItemsRario{0}", ID);
            }
            else
            {
                ul.ID = string.Format("_ulListItemsRario{0}", ID);
            }
            int repeater = 0;

            foreach (DataRow item in ContentItems)
            {
                string displayValue = GetRowValueByDisplayMember(item);
                string valueValue = ValidationHelper.GetString(item[ValueMember], string.Empty);

                string id = string.Format("chbx{0}{1}", valueValue, ID);
                string name = string.Format("_chbxItem{0}", ID);

                //Table
                if (HtmlTag == 1)
                {
                    if (repeater == RepeatColumn)
                    {
                        tr = new WebControl(HtmlTextWriterTag.Tr);
                        repeater = 0;
                    }
                    var td = new WebControl(HtmlTextWriterTag.Td);
                    var chbxItem = new WebControl(HtmlTextWriterTag.Input);
                    var label = new WebControl(HtmlTextWriterTag.Label);

                    label.Attributes.Add("for", id);
                    label.Controls.Add(new LiteralControl(displayValue));

                    chbxItem.Attributes.Add("type", "checkbox");
                    chbxItem.Attributes.Add("id", id);
                    chbxItem.Attributes.Add("value", valueValue);
                    chbxItem.Attributes.Add("name", name);

                    if (selectedVals.Count() > 0)
                        if (selectedVals.Contains(valueValue))
                            chbxItem.Attributes.Add("checked", "true");

                    td.Controls.Add(chbxItem);
                    td.Controls.Add(label);
                    tr.Controls.Add(td);
                    table.Controls.Add(tr);
                }
                else //Ul
                {
                    var li = new WebControl(HtmlTextWriterTag.Li);
                    var chbxItem = new WebControl(HtmlTextWriterTag.Input);
                    var label = new WebControl(HtmlTextWriterTag.Label);

                    label.Attributes.Add("for", id);
                    label.Controls.Add(new LiteralControl(displayValue));

                    chbxItem.Attributes.Add("type", "checkbox");
                    chbxItem.Attributes.Add("id", id);
                    chbxItem.Attributes.Add("value", valueValue);
                    chbxItem.Attributes.Add("name", name);

                    if (selectedVals.Count() > 0)
                        if (selectedVals.Contains(valueValue))
                            chbxItem.Attributes.Add("checked", "true");
                    li.Controls.Add(chbxItem);
                    li.Controls.Add(label);
                    ul.Controls.Add(li);
                }

                repeater++;
            }
            _pnlListItems.Controls.Clear();

            if (HtmlTag == 1)
            {
                _pnlListItems.Controls.Add(table);
            }
            else
            {
                _pnlListItems.Controls.Add(ul);
            }
        }

        private void CreateDropDownList(string value)
        {
            var drlListItems = new DropDownList();
            drlListItems.AppendDataBoundItems = true;
            drlListItems.Items.Add(new ListItem { Text = "Please select", Value = "0", Selected = true });
            drlListItems.ID = string.Format("_drlListItems{0}", ID);
            drlListItems.Width = ValidationHelper.GetInteger(Width, 200);
            if (ContentItems != null)
            {
                foreach (DataRow item in ContentItems)
                {
                    string displayValue = GetRowValueByDisplayMember(item);
                    string valueValue = ValidationHelper.GetString(item[ValueMember], string.Empty);

                    drlListItems.Items.Add(new ListItem
                                               {
                                                   Value = valueValue,
                                                   Text = displayValue
                                               });
                }
                if (!string.IsNullOrEmpty(value))
                    drlListItems.SelectedValue = value;
            }
            Controls.Clear();
            Controls.Add(drlListItems);
        }

        private void CreateTextBoxList(string values)
        {
            if (_pnlListItems == null)
            {
                _pnlListItems = new Panel();
                _pnlListItems.ID = string.Format("_pnlListItems{0}", ID);
                _pnlListItems.Attributes.Add("style", "overflow:auto; with:300; height:100; ");
            }
            var selectedVals = new List<string>();

            if (!string.IsNullOrEmpty(values))
                selectedVals = values.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var table = new WebControl(HtmlTextWriterTag.Table);
            table.Attributes.Add("width", "100%");
            table.Attributes.Add("class", "add-form");
            table.Attributes.Add("style", "text-align:right");
            var ul = new WebControl(HtmlTextWriterTag.Ul);
            var tr = new WebControl(HtmlTextWriterTag.Tr);

            if (HtmlTag == 1)
            {
                table.ID = string.Format("_ulListItemsTextBox{0}", ID);
            }
            else
            {
                ul.ID = string.Format("_ulListItemsTextBox{0}", ID);
            }

            int repeater = 0;


            foreach (DataRow item in ContentItems)
            {
                string displayValue = GetRowValueByDisplayMember(item);
                string valueValue = ValidationHelper.GetString(item[ValueMember], string.Empty);

                string id = string.Format("txt{0}{1}", valueValue, ID);
                string name = string.Format("_txtItem{0}", ID);

                //Table
                if (HtmlTag == 1)
                {
                    if (repeater == RepeatColumn)
                    {
                        tr = new WebControl(HtmlTextWriterTag.Tr);
                        repeater = 0;
                    }
                    var td = new WebControl(HtmlTextWriterTag.Td);
                    var td2 = new WebControl(HtmlTextWriterTag.Td);
                    var txtItem = new WebControl(HtmlTextWriterTag.Input);
                    var label = new WebControl(HtmlTextWriterTag.Label);

                    label.Attributes.Add("for", id);
                    label.Controls.Add(new LiteralControl(displayValue));

                    txtItem.Attributes.Add("type", "text");
                    txtItem.Attributes.Add("class", "txt-2");
                    txtItem.Attributes.Add("id", id);
                    txtItem.Attributes.Add("value", "");
                    txtItem.Attributes.Add("name", name);


                    td.Controls.Add(label);
                    td2.Controls.Add(txtItem);
                    tr.Controls.Add(td);
                    tr.Controls.Add(td2);
                    table.Controls.Add(tr);
                }
                else //Ul
                {
                    var li = new WebControl(HtmlTextWriterTag.Li);
                    var txtItem = new WebControl(HtmlTextWriterTag.Input);
                    var label = new WebControl(HtmlTextWriterTag.Label);

                    label.Attributes.Add("for", id);
                    label.Controls.Add(new LiteralControl(valueValue));

                    txtItem.Attributes.Add("type", "text");
                    txtItem.Attributes.Add("id", id);
                    txtItem.Attributes.Add("value", "");
                    txtItem.Attributes.Add("name", name);

                    if (selectedVals.Count() > 0)
                        if (selectedVals.Contains(valueValue))
                            txtItem.Attributes.Add("checked", "true");
                    li.Controls.Add(label);
                    li.Controls.Add(txtItem);
                    ul.Controls.Add(li);
                }

                repeater++;
            }
            _pnlListItems.Controls.Clear();
            if (HtmlTag == 1)
            {
                _pnlListItems.Controls.Add(table);
            }
            else
            {
                _pnlListItems.Controls.Add(ul);
            }
        }

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
                    options["ContentTypeId"] = GetControlValue(_drlContentTypes.ID);
                    options["ContainerCSS"] = GetControlValue(_txtContainerCssClass.ID);
                    options["ItemCSS"] = GetControlValue(_txtItemCssClass.ID);
                    options["RepeatColumn"] = GetControlValue(_txtRepeatColumn.ID);
                    options["HtmlTag"] = GetControlValue(_drlHtmlTag.ID);
                    options["DisplayMember"] = GetControlValue(_txtDisplayMember.ID);
                    options["ValueMember"] = GetControlValue(_txtValueMember.ID);
                    options["Width"] = GetControlValue(_txtWidth.ID);

                    SetValue(GetXmlFromOptions(options));
                }
                else
                {
                    switch (ListControlType)
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
                        case ListControlType.TextBoxList:
                            SetValue(GetControlValue(string.Format("_txtItem{0}", ID)));
                            break;
                    }
                }
            }
        }

        private void Execute(IEnumerable<string> values, int contentTypeid, int contentId, string fieldName, bool isUpdate)
        {
            if (isUpdate)
            {
                var pars = new object[2, 3];
                pars[0, 0] = "@FromContentTypeId";
                pars[0, 1] = ContentTypeId;
                pars[1, 0] = "@ToContentId";
                pars[1, 1] = contentId;

                _generalConnection.ExecuteNonQuery("freb_ContentToContentLookUpList_Delete", pars, QueryType.StoredProcedure, ErrorInfoList);
            }

            foreach (string value in values)
            {
                var pars = new object[5, 3];
                pars[0, 0] = "@FromContentTypeId";
                pars[0, 1] = ContentTypeId;
                pars[1, 0] = "@FromContentId";
                pars[1, 1] = value;
                pars[2, 0] = "@ToContentTypeId";
                pars[2, 1] = contentTypeid;
                pars[3, 0] = "@ToContentId";
                pars[3, 1] = contentId;
                pars[4, 0] = "@FieldName";
                pars[4, 1] = fieldName;
                _generalConnection.ExecuteNonQuery("freb_ContentToContentLookUpList_Insert", pars, QueryType.StoredProcedure, ErrorInfoList);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_generalConnection != null)
                _generalConnection.Dispose();
        }

        private DataRowCollection GetRows()
        {
            ContentTypeInfo contentTypeInfo = _contentTypeProvider.Select(ContentTypeId, ErrorInfoList);
            if (contentTypeInfo != null)
            {
                DataTable dataTable = _generalConnection.ExecuteDataTableQuery(
                    contentTypeInfo.TableName + ".select_all", null, ErrorInfoList);
                if (dataTable != null)
                    return dataTable.Rows;
            }
            return null;
        }

        private string GetRowValueByDisplayMember(DataRow dataRow)
        {
            string[] displayMember = DisplayMember.Split(',');
            string result = string.Empty;
            for (int i = 0; i < displayMember.Length; i++)
            {
                if (string.IsNullOrEmpty(result))
                    result += " ";
                result = ValidationHelper.GetString(dataRow[displayMember[i]], string.Empty);
            }
            return result;
        }

        public override bool Validate()
        {
            bool require = true;
            if (IsRequired)
            {
                require = !string.IsNullOrEmpty(PrivateValue) && !PrivateValue.Equals("0");
                if (!require)
                {
                    var error = new ErrorInfo();
                    error.Message = RequiredErrorMessage;
                    error.Source = FieldName;
                    RegisterError(error);
                }
            }
            return require;
        }
    }
}
