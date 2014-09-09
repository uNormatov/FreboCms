using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Enum;
using FCore.Helper;

namespace FUIControls.UIControl
{
    public class ContentItemTemplate : Control, ITemplate
    {
        private readonly FieldInfo[] _fields;
        private readonly int _contentTypeId;
        private readonly ListItemType _listItemType;

        public ContentItemTemplate(FieldInfo[] fields, ListItemType listItemType, int contentTypeid)
        {
            _fields = fields;
            _listItemType = listItemType;
            _contentTypeId = contentTypeid;
        }

        public void InstantiateIn(Control container)
        {
            var placeHolder = new PlaceHolder();

            switch (_listItemType)
            {
                case ListItemType.Header:
                    StringBuilder htmlBuilder = new StringBuilder();
                    htmlBuilder.Append("<div class=\"clr\"></div>");
                    htmlBuilder.Append("<table class=\"adminlist\"><thead><tr><th width=\"1%\"><input type=\"checkbox\" title=\"Check All\" value=\"\" name=\"checkall-toggle\" onclick=\"Frebo.selectAll(this)\"></th>");
                    placeHolder.Controls.Add(new LiteralControl(htmlBuilder.ToString()));
                    foreach (FieldInfo item in _fields)
                    {
                        LiteralControl ltl = new LiteralControl();
                        ltl.ID = string.Format("ltlTitle{0}", item.Name);
                        placeHolder.Controls.Add(new LiteralControl("<th>"));
                        placeHolder.Controls.Add(ltl);
                        placeHolder.Controls.Add(new LiteralControl("</th>"));
                    }
                    placeHolder.Controls.Add(new LiteralControl("<th style=\"width:10px;\">Published</th>"));
                    placeHolder.Controls.Add(new LiteralControl("<th style=\"width:10px;\">Language</th>"));
                    placeHolder.Controls.Add(new LiteralControl("<th >Modified By</th>"));
                    placeHolder.Controls.Add(new LiteralControl("<th >Modified Date</th>"));
                    placeHolder.Controls.Add(new LiteralControl("<th style=\"width:10px;\">Id</th>"));

                    placeHolder.Controls.Add(new LiteralControl("</tr></thead>"));
                    break;
                case ListItemType.Item:
                    placeHolder.Controls.Add(new LiteralControl("<tr class=\"row0\">"));
                    var literalChckbox1 = new LiteralControl();
                    literalChckbox1.ID = "ltlCheckbox";
                    placeHolder.Controls.Add(new LiteralControl("<td class=\"center\">"));
                    placeHolder.Controls.Add(literalChckbox1);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    foreach (FieldInfo item in _fields)
                    {
                        var literalControl = new LiteralControl();
                        literalControl.ID = string.Format("ltl{0}", item.Name);
                        placeHolder.Controls.Add(new LiteralControl("<td  class=\"center\">"));
                        placeHolder.Controls.Add(literalControl);
                        placeHolder.Controls.Add(new LiteralControl("</td>"));
                    }
                    var literalControlItem = new LiteralControl();
                    literalControlItem.ID = "ltlIsPublished";
                    placeHolder.Controls.Add(new LiteralControl("<td  class=\"center\">"));
                    placeHolder.Controls.Add(literalControlItem);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    literalControlItem = new LiteralControl();
                    literalControlItem.ID = "ltlLanguage";
                    placeHolder.Controls.Add(new LiteralControl("<td  class=\"center\">"));
                    placeHolder.Controls.Add(literalControlItem);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    literalControlItem = new LiteralControl();
                    literalControlItem.ID = "ltlModifiedBy";
                    placeHolder.Controls.Add(new LiteralControl("<td  class=\"center\">"));
                    placeHolder.Controls.Add(literalControlItem);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    literalControlItem = new LiteralControl();
                    literalControlItem.ID = "ltlModifiedDate";
                    placeHolder.Controls.Add(new LiteralControl("<td  class=\"center\">"));
                    placeHolder.Controls.Add(literalControlItem);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    literalControlItem = new LiteralControl();
                    literalControlItem.ID = "ltlId";
                    placeHolder.Controls.Add(new LiteralControl("<td  class=\"center\">"));
                    placeHolder.Controls.Add(literalControlItem);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    break;

                case ListItemType.AlternatingItem:
                    placeHolder.Controls.Add(new LiteralControl("<tr class=\"row1\">"));
                    var literalChckbox0 = new LiteralControl();
                    literalChckbox0.ID = "ltlCheckbox";
                    placeHolder.Controls.Add(new LiteralControl("<td class=\"center\">"));
                    placeHolder.Controls.Add(literalChckbox0);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    foreach (FieldInfo item in _fields)
                    {
                        var literalControl = new LiteralControl();
                        literalControl.ID = string.Format("ltl{0}", item.Name);
                        placeHolder.Controls.Add(new LiteralControl("<td  class=\"center\">"));
                        placeHolder.Controls.Add(literalControl);
                        placeHolder.Controls.Add(new LiteralControl("</td>"));
                    }

                    var literalControlAlterItem = new LiteralControl();
                    literalControlAlterItem.ID = "ltlIsPublished";
                    placeHolder.Controls.Add(new LiteralControl("<td  class=\"center\">"));
                    placeHolder.Controls.Add(literalControlAlterItem);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    literalControlAlterItem = new LiteralControl();
                    literalControlAlterItem.ID = "ltlLanguage";
                    placeHolder.Controls.Add(new LiteralControl("<td  class=\"center\">"));
                    placeHolder.Controls.Add(literalControlAlterItem);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    literalControlAlterItem = new LiteralControl();
                    literalControlAlterItem.ID = "ltlModifiedBy";
                    placeHolder.Controls.Add(new LiteralControl("<td  class=\"center\">"));
                    placeHolder.Controls.Add(literalControlAlterItem);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    literalControlAlterItem = new LiteralControl();
                    literalControlAlterItem.ID = "ltlModifiedDate";
                    placeHolder.Controls.Add(new LiteralControl("<td  class=\"center\">"));
                    placeHolder.Controls.Add(literalControlAlterItem);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    literalControlAlterItem = new LiteralControl();
                    literalControlAlterItem.ID = "ltlId";
                    placeHolder.Controls.Add(new LiteralControl("<td  class=\"center\">"));
                    placeHolder.Controls.Add(literalControlAlterItem);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    placeHolder.Controls.Add(new LiteralControl("</tr>"));
                    break;
            }
            placeHolder.DataBinding += placeHolder_DataBinding;
            container.Controls.Add(placeHolder);
        }

        private void placeHolder_DataBinding(object sender, EventArgs e)
        {
            PlaceHolder placeHolder = sender as PlaceHolder;
            if (placeHolder != null)
            {
                if (_listItemType == ListItemType.Item || _listItemType == ListItemType.AlternatingItem)
                {

                    RepeaterItem repeaterItem = placeHolder.NamingContainer as RepeaterItem;
                    LiteralControl ltlCheckbox = placeHolder.FindControl("ltlCheckbox") as LiteralControl;
                    if (repeaterItem != null && ltlCheckbox != null)
                    {
                        ltlCheckbox.Text =
                            string.Format("<input type=\"checkbox\" title=\"Select\" value=\"{0}\" name=\"chbxRow\" " +
                                          "id=\"chbxRow\" onclick=\"Frebo.checked(this.checked);\">",
                                          ValidationHelper.GetInteger(DataBinder.Eval(repeaterItem.DataItem, "Id"), 0));
                        foreach (FieldInfo item in _fields)
                        {
                            LiteralControl literalControl =
                                placeHolder.FindControl(string.Format("ltl{0}", item.Name)) as LiteralControl;
                            if (literalControl != null)
                            {
                                if (item.DataType != DataFieldType.DateTime)
                                {
                                    literalControl.Text =
                                        string.Format(
                                            "<a href=\"/administrator/content/action.aspx?action=entry&contenttypeid={0}&id={1}\">{2}</a>",
                                            _contentTypeId,
                                            ValidationHelper.GetInteger(DataBinder.Eval(repeaterItem.DataItem, "Id"), 0),
                                            ValidationHelper.GetString(DataBinder.Eval(repeaterItem.DataItem, item.Name), ""));
                                }
                                else
                                {
                                    literalControl.Text =
                                      string.Format(
                                          "<a href=\"/administrator/content/action.aspx?action=entry&contenttypeid={0}&id={1}\">{2}</a>",
                                          _contentTypeId,
                                          ValidationHelper.GetInteger(DataBinder.Eval(repeaterItem.DataItem, "Id"), 0),
                                          ValidationHelper.GetString(DataBinder.Eval(repeaterItem.DataItem, item.Name, "{0:d/M/yyyy}"), ""));
                                }
                            }
                        }
                        LiteralControl literalControlItem =
                            placeHolder.FindControl("ltlIspublished") as LiteralControl;
                        if (literalControlItem != null)
                        {
                            bool published = ValidationHelper.GetBoolean(DataBinder.Eval(repeaterItem.DataItem, "IsPublished"), false);
                            int id = ValidationHelper.GetInteger(DataBinder.Eval(repeaterItem.DataItem, "Id"), 0);
                            string html = string.Empty;
                            if (published)
                            {
                                html = string.Format("<a class=\"jgrid\" href=\"javascript:void(0);\" onclick=\"Frebo.listTask('unpublish','{0}')\" title=\"Unpublish Item\"><span class=\"state publish\"><span class=\"text\">Published</span></span></a>", id);
                            }
                            else
                            {
                                html = string.Format("<a class=\"jgrid\" href=\"javascript:void(0);\" onclick=\"Frebo.listTask('publish','{0}')\" title=\"Publish Item\"><span class=\"state unpublish\"><span class=\"text\">Unpublished</span></span></a>", id);
                            }
                            literalControlItem.Text = html;

                        }
                        literalControlItem = placeHolder.FindControl("ltlLanguage") as LiteralControl;
                        if (literalControlItem != null)
                            literalControlItem.Text = ValidationHelper.GetString(DataBinder.Eval(repeaterItem.DataItem, "Language"), string.Empty);

                        literalControlItem = placeHolder.FindControl("ltlModifiedBy") as LiteralControl;
                        if (literalControlItem != null)
                            literalControlItem.Text = ValidationHelper.GetString(DataBinder.Eval(repeaterItem.DataItem, "ModifiedBy"), string.Empty);

                        literalControlItem = placeHolder.FindControl("ltlModifiedDate") as LiteralControl;
                        if (literalControlItem != null)
                            literalControlItem.Text = ValidationHelper.GetString(DataBinder.Eval(repeaterItem.DataItem, "ModifiedDate", "{0:d/M/yyyy}"), string.Empty);
                        literalControlItem = placeHolder.FindControl("ltlId") as LiteralControl;
                        if (literalControlItem != null)
                            literalControlItem.Text = ValidationHelper.GetString(DataBinder.Eval(repeaterItem.DataItem, "Id"), string.Empty);


                    }
                }
                else if (_listItemType == ListItemType.Header)
                {
                    foreach (FieldInfo item in _fields)
                    {
                        LiteralControl ltl = placeHolder.FindControl(string.Format("ltlTitle{0}", item.Name)) as LiteralControl;
                        ltl.Text = item.DisplayName;
                    }
                }
            }
        }
    }
}
