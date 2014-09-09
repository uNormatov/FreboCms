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
    [ToolboxData("<fr:ListLookUp runat=\"server\" ID=\"ListLookUp1\" />")]
    public sealed class ListLookUp : AbsractBasicControl, IComponentControl
    {
        #region Variables

        private DropDownList _drlControlTypes;
        private DropDownList _drlLists;
        private DropDownList _drlHtmlTag;

        private TextBox _txtContainerCssClass;
        private TextBox _txtItemCssClass;
        private TextBox _txtRepeatColumn;
        private TextBox _txtWidth;

        private Panel _pnlListItems;
        private readonly GeneralConnection _generalConnection;
        private readonly ListItemProvider _listItemProvider;
        private readonly ListProvider _listProvider;
        #endregion

        #region Properties

        private string PrivateValue
        {
            get
            {
                object o = ViewState["__list_look_up_private_value"];
                if (o == null)
                    return string.Empty;
                return o.ToString();
            }
            set { ViewState["__list_look_up_private_value"] = value; }
        }

        public string ContainerCssClass
        {
            get
            {
                object o = ViewState["__list_lookup_container_css"];
                if (o == null)
                    return string.Empty;
                return o.ToString();
            }
            set { ViewState["__list_lookup_container_css"] = value; }
        }

        public string ItemCssClass
        {
            get
            {
                object o = ViewState["__list_lookup_item_css"];
                if (o == null)
                    return string.Empty;
                return o.ToString();
            }
            set { ViewState["__list_lookup_item_css"] = value; }
        }

        public int RepeatColumn
        {
            get
            {
                object o = ViewState["__list_lookup_repeat_column"];
                if (o == null)
                    return 1;
                return (int)o;
            }
            set { ViewState["__list_lookup_repeat_column"] = value; }
        }

        public int HtmlTag
        {
            get
            {
                object o = ViewState["__list_lookup_Html_tab"];
                if (o == null)
                    return 1;
                return (int)o;
            }
            set { ViewState["__list_lookup_Html_tab"] = value; }
        }

        public string Width
        {
            get
            {
                object o = ViewState["__list_lookup_width"];
                if (o == null)
                    return "200";
                return o.ToString();
            }
            set { ViewState["__list_lookup_width"] = value; }
        }

        public int ListId
        {
            get
            {
                object o = ViewState["__list_lookup_list_id"];
                if (o == null)
                    return 0;
                return (int)o;
            }
            set { ViewState["__list_lookup_list_id"] = value; }
        }

        public bool IsComponent()
        {
            return true;
        }

        public ListControlType ListControlType
        {
            get
            {
                object o = ViewState["__list_lookup_list_control_type"];
                if (o == null)
                    return ListControlType.DropDownList;
                return (ListControlType)o;
            }
            set { ViewState["__list_lookup_list_control_type"] = value; }
        }

        public List<ListItemInfo> ListItems
        {
            get
            {
                object o = ViewState["__list_lookup_list_items"];
                if (o == null)
                {
                    o = _listItemProvider.SelectAllByListId(ListId, ErrorInfoList);
                    if (o == null)
                        return null;
                    ViewState["__list_lookup_list_items"] = o;
                }
                return (List<ListItemInfo>)o;
            }
        }

        #endregion

        #region Constructors

        public ListLookUp()
            : this(null, null, null, null)
        {
        }

        public ListLookUp(string controlId, string options, GeneralConnection generalConnection, ErrorInfoList errors)
        {
            if (!string.IsNullOrEmpty(controlId))
                ID = controlId;
            if (!string.IsNullOrEmpty(options))
                SetOptions(options);

            _generalConnection = generalConnection ?? new GeneralConnection();

            if (errors != null)
                ErrorInfoList = errors;

            if (_listItemProvider == null)
                _listItemProvider = new ListItemProvider();

            if (_listProvider == null)
                _listProvider = new ListProvider();
        }

        #endregion

        #region Methods

        public override void SetOptions(string xmloptions)
        {
            if (!string.IsNullOrEmpty(xmloptions))
            {

                Dictionary<string, string> options = GetOptionsFromXml(xmloptions);
                if (options.ContainsKey("ControlType"))
                    ListControlType = (ListControlType)Enum.Parse(typeof(TextBoxMode), options["ControlType"], true);
                if (options.ContainsKey("ListId"))
                    ListId = int.Parse(options["ListId"]);
                if (options.ContainsKey("ContainerCSS"))
                    ContainerCssClass = options["ContainerCSS"];
                if (options.ContainsKey("ItemCss"))
                    ItemCssClass = options["ItemCSS"];
                if (options.ContainsKey("RepeatColumn"))
                    RepeatColumn = int.Parse(options["RepeatColumn"]);
                if (options.ContainsKey("HtmlTag"))
                    HtmlTag = int.Parse(options["HtmlTag"]);
                if (options.ContainsKey("Width"))
                    Width = options["Width"];

            }
        }

        public override void SetValue(string value)
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                Dictionary<string, string> options = GetOptionsFromXml(value);

                if (!string.IsNullOrEmpty(options["ControlType"]))
                    ListControlType = (ListControlType)Enum.Parse(typeof(ListControlType), options["ControlType"]);

                if (!string.IsNullOrEmpty(options["ListId"]))
                    ListId = int.Parse(options["ListId"]);

                if (!string.IsNullOrEmpty(options["ContainerCSS"]))
                    ContainerCssClass = options["ContainerCSS"];

                if (!string.IsNullOrEmpty(options["ItemCSS"]))
                    ItemCssClass = options["ItemCSS"];

                if (options.ContainsKey("RepeatColumn") && !string.IsNullOrEmpty(options["RepeatColumn"]))
                    RepeatColumn = int.Parse(options["RepeatColumn"]);

                if (options.ContainsKey("RepeatColumn") && !string.IsNullOrEmpty(options["HtmlTag"]))
                    HtmlTag = int.Parse(options["HtmlTag"]);

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
                options["ListId"] = ListId.ToString();
                options["ContainerCSS"] = ContainerCssClass;
                options["ItemCSS"] = ItemCssClass;
                options["RepeatColumn"] = RepeatColumn.ToString();
                options["HtmlTag"] = HtmlTag.ToString();
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
            pars[0, 0] = "@ListId";
            pars[0, 1] = ListId;
            pars[1, 0] = "@ContentTypeId";
            pars[1, 1] = contentTypeId;
            pars[2, 0] = "@ContentId";
            pars[2, 1] = contentId;
            pars[3, 0] = "@FieldName";
            pars[3, 1] = fieldName;
            DataTable dataTable = _generalConnection.ExecuteDataTableQuery("freb_ListToContentLookUpList_Select", pars,
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
                table.Rows[1].Cells[0].Controls.Add(new LiteralControl("List type :"));
                table.Rows[1].Cells[0].Attributes.Add("class", "label");
                table.Rows[1].Cells.Add(new HtmlTableCell());
                table.Rows[1].Cells[1].Controls.Add(_drlLists);


                table.Rows.Add(new HtmlTableRow());
                table.Rows[2].Cells.Add(new HtmlTableCell());
                table.Rows[2].Cells[0].Controls.Add(new LiteralControl("Container CSS:"));
                table.Rows[2].Cells[0].Attributes.Add("class", "label");
                table.Rows[2].Cells.Add(new HtmlTableCell());
                table.Rows[2].Cells[1].Controls.Add(_txtContainerCssClass);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[3].Cells.Add(new HtmlTableCell());
                table.Rows[3].Cells[0].Controls.Add(new LiteralControl("Item CSS:"));
                table.Rows[3].Cells[0].Attributes.Add("class", "label");
                table.Rows[3].Cells.Add(new HtmlTableCell());
                table.Rows[3].Cells[1].Controls.Add(_txtItemCssClass);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[4].Cells.Add(new HtmlTableCell());
                table.Rows[4].Cells[0].Controls.Add(new LiteralControl("Repeat column:"));
                table.Rows[4].Cells[0].Attributes.Add("class", "label");
                table.Rows[4].Cells.Add(new HtmlTableCell());
                table.Rows[4].Cells[1].Controls.Add(_txtRepeatColumn);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[5].Cells.Add(new HtmlTableCell());
                table.Rows[5].Cells[0].Controls.Add(new LiteralControl("Html tag:"));
                table.Rows[5].Cells[0].Attributes.Add("class", "label");
                table.Rows[5].Cells.Add(new HtmlTableCell());
                table.Rows[5].Cells[1].Controls.Add(_drlHtmlTag);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[6].Cells.Add(new HtmlTableCell());
                table.Rows[6].Cells[0].Controls.Add(new LiteralControl("Width:"));
                table.Rows[6].Cells[0].Attributes.Add("class", "label");
                table.Rows[6].Cells.Add(new HtmlTableCell());
                table.Rows[6].Cells[1].Controls.Add(_txtWidth);

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

                if (_drlLists == null)
                {
                    _drlLists = new DropDownList();
                    _drlLists.ID = string.Format("_drlLists{0}", ID);
                    _drlLists.DataSource = _listProvider.SelectAll(ErrorInfoList);
                    _drlLists.DataTextField = "Name";
                    _drlLists.DataValueField = "Id";
                    _drlLists.DataBind();
                }

                int selected = -1;
                foreach (ListItem item in _drlLists.Items)
                {
                    selected++;
                    if (item.Value == ListId.ToString())
                        break;
                }
                _drlLists.SelectedIndex = selected;


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
            foreach (ListItemInfo item in ListItems)
            {
                string id = string.Format("rbtn{0}{1}", item.Id, ID);
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
                    label.Controls.Add(new LiteralControl(item.Name));

                    chbxItem.Attributes.Add("type", "radio");
                    chbxItem.Attributes.Add("id", id);
                    chbxItem.Attributes.Add("value", item.Id.ToString());
                    chbxItem.Attributes.Add("name", name);

                    if (value == item.Id.ToString())
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
                    label.Controls.Add(new LiteralControl(item.Name));

                    chbxItem.Attributes.Add("type", "radio");
                    chbxItem.Attributes.Add("id", id);
                    chbxItem.Attributes.Add("value", item.Id.ToString());
                    chbxItem.Attributes.Add("name", name);

                    if (value == item.Id.ToString())
                        chbxItem.Attributes.Add("checked", "true");
                    li.Controls.Add(chbxItem);
                    li.Controls.Add(label);
                    ul.Controls.Add(li);
                }

                repeater++;
            }
            Controls.Clear();
            _pnlListItems.Controls.Clear();

            if (HtmlTag == 1)
            {
                //_pnlListItems.Controls.Add(table);
                Controls.Add(table);
            }
            else
            {
                //_pnlListItems.Controls.Add(ul);
                Controls.Add(ul);
            }
        }

        private void CreatCheckBoxList(string values)
        {
            if (_pnlListItems == null)
            {
                _pnlListItems = new Panel();
                _pnlListItems.ID = string.Format("_pnlListItems{0}", ID);
                _pnlListItems.CssClass = ContainerCssClass;
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

            foreach (ListItemInfo item in ListItems)
            {
                string id = string.Format("chbx{0}{1}", item.Id, ID);
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
                    label.Controls.Add(new LiteralControl(item.Name));

                    chbxItem.Attributes.Add("type", "checkbox");
                    chbxItem.Attributes.Add("id", id);
                    chbxItem.Attributes.Add("value", item.Id.ToString());
                    chbxItem.Attributes.Add("name", name);

                    if (selectedVals.Count() > 0)
                        if (selectedVals.Contains(item.Id.ToString()))
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
                    label.Controls.Add(new LiteralControl(item.Name));

                    chbxItem.Attributes.Add("type", "checkbox");
                    chbxItem.Attributes.Add("id", id);
                    chbxItem.Attributes.Add("value", item.Id.ToString());
                    chbxItem.Attributes.Add("name", name);

                    if (selectedVals.Count() > 0)
                        if (selectedVals.Contains(item.Id.ToString()))
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
            if (_pnlListItems == null)
            {
                _pnlListItems = new Panel();
                _pnlListItems.ID = string.Format("_pnlListItems{0}", ID);
                _pnlListItems.Attributes.Add("style", "overflow:auto; width:300; height:100; ");
            }
            var drlListItems = new DropDownList();
            drlListItems.AppendDataBoundItems = true;
            drlListItems.Items.Add(new ListItem { Text = "Please select", Value = "0", Selected = true });
            drlListItems.ID = string.Format("_drlListItems{0}", ID);
            drlListItems.DataSource = ListItems;
            drlListItems.DataTextField = "Name";
            drlListItems.DataValueField = "Id";
            drlListItems.Width = ValidationHelper.GetInteger(Width, 200);
            drlListItems.DataBind();

            int selected = -1;
            if (string.IsNullOrEmpty(value))
                selected = 0;
            else
                drlListItems.SelectedValue = value;
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


            foreach (ListItemInfo item in ListItems)
            {
                string id = string.Format("txt{0}{1}", item.Id, ID);
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
                    label.Controls.Add(new LiteralControl(item.Name));

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
                    label.Controls.Add(new LiteralControl(item.Name));

                    txtItem.Attributes.Add("type", "text");
                    txtItem.Attributes.Add("id", id);
                    txtItem.Attributes.Add("value", "");
                    txtItem.Attributes.Add("name", name);

                    if (selectedVals.Count() > 0)
                        if (selectedVals.Contains(item.Id.ToString()))
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
                    options["ListId"] = GetControlValue(_drlLists.ID);
                    options["ContainerCSS"] = GetControlValue(_txtContainerCssClass.ID);
                    options["ItemCSS"] = GetControlValue(_txtItemCssClass.ID);
                    options["RepeatColumn"] = GetControlValue(_txtRepeatColumn.ID);
                    options["HtmlTag"] = GetControlValue(_drlHtmlTag.ID);
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
                pars[0, 0] = "@ListId";
                pars[0, 1] = ListId;
                pars[1, 0] = "@ContentId";
                pars[1, 1] = contentId;

                _generalConnection.ExecuteNonQuery("freb_ListToContentLookUpList_Delete",
                                                                                  pars, QueryType.StoredProcedure, ErrorInfoList);
            }

            foreach (string value in values)
            {
                var pars = new object[5, 3];
                pars[0, 0] = "@ListId";
                pars[0, 1] = ListId;
                pars[1, 0] = "@ListItemId";
                pars[1, 1] = value;
                pars[2, 0] = "@ContentTypeId";
                pars[2, 1] = contentTypeid;
                pars[3, 0] = "@ContentId";
                pars[3, 1] = contentId;
                pars[4, 0] = "@FieldName";
                pars[4, 1] = fieldName;
                _generalConnection.ExecuteNonQuery("freb_ListToContentLookUpList_Insert", pars,
                                                                  QueryType.StoredProcedure, ErrorInfoList);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_listItemProvider != null)
                _listItemProvider.Dispose();
            if (_listProvider != null)
                _listProvider.Dispose();
            if (_generalConnection != null)
                _generalConnection.Dispose();
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
