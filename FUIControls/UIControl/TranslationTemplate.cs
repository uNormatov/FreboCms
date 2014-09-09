using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Helper;

namespace FUIControls.UIControl
{
    public class TranslationTemplate : Control, ITemplate
    {
        private readonly LanguageInfo[] _languages;
        private readonly int _contentTypeId;
        private readonly ListItemType _listItemType;

        public TranslationTemplate(LanguageInfo[] languageInfos, ListItemType listItemType)
        {
            _languages = languageInfos;
            _listItemType = listItemType;
        }

        public void InstantiateIn(Control container)
        {
            var placeHolder = new PlaceHolder();

            switch (_listItemType)
            {
                case ListItemType.Header:
                    StringBuilder htmlBuilder = new StringBuilder();
                    htmlBuilder.Append("<div class=\"clr\"></div>");
                    htmlBuilder.Append("<table class=\"adminlist\"><thead><tr><th width=\"1%\"><input type=\"checkbox\" title=\"Check All\" value=\"\" name=\"checkall-toggle\" onclick=\"Frebo.selectAll(this)\"></th><th>Keyword</th><th>Default</th>");
                    placeHolder.Controls.Add(new LiteralControl(htmlBuilder.ToString()));
                    foreach (LanguageInfo item in _languages)
                    {
                        LiteralControl ltl = new LiteralControl();
                        ltl.ID = string.Format("ltlTitle{0}", item.Name);
                        placeHolder.Controls.Add(new LiteralControl("<th>"));
                        placeHolder.Controls.Add(ltl);
                        placeHolder.Controls.Add(new LiteralControl("</th>"));
                    }
                    placeHolder.Controls.Add(new LiteralControl("</tr></thead>"));
                    break;
                case ListItemType.Item:
                    placeHolder.Controls.Add(new LiteralControl("<tr class=\"row0\">"));

                    var literalChckbox = new LiteralControl();
                    literalChckbox.ID = "ltlCheckbox";
                    placeHolder.Controls.Add(new LiteralControl("<td class=\"center\">"));
                    placeHolder.Controls.Add(literalChckbox);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    var literalKeyword = new LiteralControl();
                    literalKeyword.ID = "ltlKeyword";
                    placeHolder.Controls.Add(new LiteralControl("<td class=\"center\">"));
                    placeHolder.Controls.Add(literalKeyword);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    var literalDefault = new LiteralControl();
                    literalDefault.ID = "ltlDefault";
                    placeHolder.Controls.Add(new LiteralControl("<td class=\"center\">"));
                    placeHolder.Controls.Add(literalDefault);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));
                    foreach (LanguageInfo item in _languages)
                    {
                        var literalControl = new LiteralControl();
                        literalControl.ID = string.Format("ltl{0}", item.Code);
                        placeHolder.Controls.Add(new LiteralControl("<td class=\"center\">"));
                        placeHolder.Controls.Add(literalControl);
                        placeHolder.Controls.Add(new LiteralControl("</td>"));
                    }
                    placeHolder.Controls.Add(new LiteralControl("</tr>"));
                    break;
                case ListItemType.AlternatingItem:
                    placeHolder.Controls.Add(new LiteralControl("<tr class=\"row1\">"));

                    var literalChckbox1 = new LiteralControl();
                    literalChckbox1.ID = "ltlCheckbox";
                    placeHolder.Controls.Add(new LiteralControl("<td class=\"center\">"));
                    placeHolder.Controls.Add(literalChckbox1);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    var literalKeyword1 = new LiteralControl();
                    literalKeyword1.ID = "ltlKeyword";
                    placeHolder.Controls.Add(new LiteralControl("<td class=\"center\">"));
                    placeHolder.Controls.Add(literalKeyword1);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));

                    var literalDefault1 = new LiteralControl();
                    literalDefault1.ID = "ltlDefault";
                    placeHolder.Controls.Add(new LiteralControl("<td class=\"center\">"));
                    placeHolder.Controls.Add(literalDefault1);
                    placeHolder.Controls.Add(new LiteralControl("</td>"));
                    foreach (LanguageInfo item in _languages)
                    {
                        var literalControl = new LiteralControl();
                        literalControl.ID = string.Format("ltl{0}", item.Code);
                        placeHolder.Controls.Add(new LiteralControl("<td class=\"center\">"));
                        placeHolder.Controls.Add(literalControl);
                        placeHolder.Controls.Add(new LiteralControl("</td>"));
                    }
                    placeHolder.Controls.Add(new LiteralControl("</tr>"));
                    break;
            }
            placeHolder.DataBinding += PlaceHolderDataBinding;
            container.Controls.Add(placeHolder);
        }

        private void PlaceHolderDataBinding(object sender, EventArgs e)
        {
            PlaceHolder placeHolder = sender as PlaceHolder;
            if (placeHolder != null)
            {
                if (_listItemType == ListItemType.Item || _listItemType == ListItemType.AlternatingItem)
                {

                    RepeaterItem repeaterItem = placeHolder.NamingContainer as RepeaterItem;
                    LiteralControl ltlCheckbox = placeHolder.FindControl("ltlCheckbox") as LiteralControl;
                    LiteralControl ltlKeyword = placeHolder.FindControl("ltlKeyword") as LiteralControl;
                    LiteralControl ltlDefault = placeHolder.FindControl("ltlDefault") as LiteralControl;
                    if (repeaterItem != null && ltlCheckbox != null && ltlKeyword != null && ltlDefault != null)
                    {
                        ltlCheckbox.Text =
                            string.Format("<input type=\"checkbox\" title=\"Select\" value=\"{0}\" name=\"chbxRow\" " +
                                          "id=\"chbxRow\" onclick=\"Frebo.checked(this.checked);\"/>",
                                          ValidationHelper.GetString(DataBinder.Eval(repeaterItem.DataItem, "Keyword"), string.Empty));
                        ltlKeyword.Text = string.Format("<a href=\"/administrator/localization/translationaction.aspx?type=entry&id={0}\">{0}</a>", ValidationHelper.GetString(DataBinder.Eval(repeaterItem.DataItem, "Keyword"), string.Empty));
                        ltlDefault.Text = ValidationHelper.GetString(DataBinder.Eval(repeaterItem.DataItem, "DefaultValue"), string.Empty);

                        foreach (LanguageInfo item in _languages)
                        {
                            LiteralControl literalControl =
                                placeHolder.FindControl(string.Format("ltl{0}", item.Code)) as LiteralControl;
                            if (literalControl != null)
                                literalControl.Text = ValidationHelper.GetString(DataBinder.Eval(repeaterItem.DataItem, item.Code), string.Empty);

                        }
                    }
                }
                else if (_listItemType == ListItemType.Header)
                {
                    foreach (LanguageInfo item in _languages)
                    {
                        LiteralControl ltlHeader = placeHolder.FindControl(string.Format("ltlTitle{0}", item.Name)) as LiteralControl;
                        if (ltlHeader != null)
                        {
                            ltlHeader.Text = item.Name;
                        }
                    }
                }
            }
        }
    }
}
