using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;
using System.Text.RegularExpressions;

namespace FWeb.Administrator.SiteLayout
{
    public partial class action : FAdminEditPage
    {
        private LayoutProvider _layoutProvider;
        private LayoutCategoryProvider _layoutCategoryProvider;
        private LayoutWebPartZoneProvider _layoutWebPartZoneProvider;

        private int CategoryId
        {
            get
            {
                if (ViewState["_categroyId"] == null)
                {
                    int categoryId = ValidationHelper.GetInteger(Request.QueryString["categoryid"], -1);
                    if (categoryId == -1)
                    {
                        List<LayoutCategoryInfo> layoutCategories = _layoutCategoryProvider.SelectPagingSortingByType(
                            1, 1, "Id", "DESC", true, ErrorList);
                        if (layoutCategories != null && layoutCategories.Count > 0)
                            categoryId = layoutCategories[0].Id;
                    }
                    return categoryId;
                }
                return ValidationHelper.GetInteger(ViewState["_categroyId"], 20);
            }
            set { ViewState["_categroyId"] = value; }
        }

        protected override void Init()
        {
            base.Init();
            if (_layoutProvider == null)
                _layoutProvider = new LayoutProvider();
            if (_layoutCategoryProvider == null)
                _layoutCategoryProvider = new LayoutCategoryProvider();
            if (_layoutWebPartZoneProvider == null)
                _layoutWebPartZoneProvider = new LayoutWebPartZoneProvider();
            RenderText();
            if (IsEdit)
            {
                Title = "Edit Site Layout | Frebo Cms";
                ltlTitle.Text = "Edit Site Layout ";
            }
            else
            {
                this.Title = "New Site Layout | Frebo Cms";
                ltlTitle.Text = "New Site Layout ";
            }
            CancelUrl = "/administrator/sitelayout/layouts.aspx?categoryid=" + drlCategory.SelectedValue;
        }

        protected override void Load()
        {
            base.Load();
            RedrictUrl = "/administrator/sitelayout/layouts.aspx?categoryid=" + CategoryId;
            CancelUrl = RedrictUrl;
        }

        protected override void FillFields()
        {
            List<LayoutCategoryInfo> layoutCategories = _layoutCategoryProvider.SelectAllByType(true, ErrorList);
            drlCategory.DataSource = layoutCategories;
            drlCategory.DataBind();
            drlCategory.SelectedValue = CategoryId.ToString();

            if (IsEdit)
            {
                LayoutInfo layoutInfo = _layoutProvider.Select(ValidationHelper.GetInteger(Id, -1), ErrorList);
                if (layoutInfo != null)
                {
                    txtName.Text = layoutInfo.Name;
                    drlCategory.SelectedValue = layoutInfo.LayoutCategoryId.ToString();
                    txtBodyInit.Text = layoutInfo.BodyOption;
                    txtDocType.Text = layoutInfo.DocOption;
                    txtDescription.Text = layoutInfo.Description;
                    txtBodyContent.Text = layoutInfo.Layout;
                    txtCSSSheet.Text = layoutInfo.Css;
                }
            }

        }

        protected override bool Update()
        {
            LayoutInfo layoutInfo = _layoutProvider.Select(ValidationHelper.GetInteger(Id, -1), ErrorList);
            if (layoutInfo != null)
            {
                layoutInfo.LayoutCategoryId = ValidationHelper.GetInteger(drlCategory.SelectedValue, 0);
                layoutInfo.Name = txtName.Text;
                layoutInfo.BodyOption = txtBodyInit.Text;
                layoutInfo.DocOption = txtDocType.Text;
                layoutInfo.Description = txtDescription.Text;
                layoutInfo.Layout = txtBodyContent.Text;
                layoutInfo.Css = txtCSSSheet.Text;
                layoutInfo.IsMaster = true;
                layoutInfo.IsDeleted = false;

                if (flpScreenshot.HasFile)
                {
                    string guid = Guid.NewGuid().ToString();
                    string fileName = guid + "_" + flpScreenshot.FileName;
                    flpScreenshot.SaveAs(Server.MapPath("~/userfiles/sitelayouts/") + fileName);
                    layoutInfo.Screenshot = fileName;
                }

                if (_layoutProvider.Update(layoutInfo, ErrorList))
                {
                    List<string> webPartZones = GetWebPartZones(layoutInfo.Layout);
                    if (_layoutWebPartZoneProvider.DeleteByLayoutId(layoutInfo.Id, ErrorList))
                    {
                        foreach (string item in webPartZones)
                        {
                            LayoutWebPartZoneInfo info = new LayoutWebPartZoneInfo();
                            info.Name = item;
                            info.LayoutId = layoutInfo.Id;
                            _layoutWebPartZoneProvider.Create(info, ErrorList);
                        }
                    }
                }

            }
            return CheckErrors();
        }

        protected override bool Insert()
        {
            LayoutInfo layoutInfo = new LayoutInfo();
            layoutInfo.Name = txtName.Text;
            layoutInfo.LayoutCategoryId = ValidationHelper.GetInteger(drlCategory.SelectedValue, 0);
            layoutInfo.BodyOption = txtBodyInit.Text;
            layoutInfo.DocOption = txtDocType.Text;
            layoutInfo.Description = txtDescription.Text;
            layoutInfo.Layout = txtBodyContent.Text;
            layoutInfo.Css = txtCSSSheet.Text;
            layoutInfo.IsMaster = true;
            layoutInfo.IsDeleted = false;
            if (flpScreenshot.HasFile)
            {
                string guid = Guid.NewGuid().ToString();
                string fileName = guid + "_" + flpScreenshot.FileName;
                flpScreenshot.SaveAs(Server.MapPath("~/userfiles/sitelayouts/") + fileName);
                layoutInfo.Screenshot = fileName;
            }
            else
                layoutInfo.Screenshot = string.Empty;
            Object idObject = _layoutProvider.Create(layoutInfo, ErrorList);
            if (idObject != null)
            {
                List<string> webPartZones = GetWebPartZones(layoutInfo.Layout);
                foreach (string item in webPartZones)
                {
                    LayoutWebPartZoneInfo info = new LayoutWebPartZoneInfo();
                    info.Name = item;
                    info.LayoutId = ValidationHelper.GetInteger(idObject, 0);
                    _layoutWebPartZoneProvider.Create(info, ErrorList);
                }
            }
            return CheckErrors();
        }

        protected override void PrintErrors()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<dl id=\"system-message\">");
            builder.Append("<dt class=\"message\">Message</dt><dd class=\"error\">");
            builder.Append("<ul>");
            foreach (ErrorInfo error in ErrorList)
            {
                builder.AppendFormat("<li>{0} - {1}</li>", error.Name, error.Message);
            }
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
        }

        protected override void PrintSuccess()
        {
            throw new NotImplementedException();
        }

        private void RenderText()
        {
            ltrHead.Text = "<br/>" + HighlightHTML("<html>") + "<br />";
            ltrHead.Text += AddSpaces(1) + HighlightHTML("<head>") + "<br />";
            ltrHead.Text += AddSpaces(2) + HighlightHTML("<title>") + HighlightHTML("</title>") + "<br />";
            ltrHead.Text += AddSpaces(2) + HighlightHTML("<meta http-equiv=\"pragma\" content=\"no-cache\" />") +
                            "<br />";
            ;
            ltrHeadClose.Text = AddSpaces(1) + HighlightHTML("</head>") + "<br />";
            ltrBody.Text += HighlightHTML("<body ") + "<br/>";
            ltrBodyTagClose.Text = HighlightHTML(">") + "<br />";
            ltrBodyClose.Text += AddSpaces(3) + HighlightHTML("</body>") + "<br />";
            ltrBodyClose.Text += HighlightHTML("</html>") + "<br />";
        }

        public string HighlightHTML(string inputHtml)
        {
            string tagspan = "<span style=\"color: #0000ff;\">";
            string tagcontentspan = "<span style=\"color: #a31515;\">";
            string propertyspan = "<span style=\"color: #0000ff;\">";
            string propertynamespan = "<span style=\"color: #ff0000;\">";
            string endspan = "</span>";


            inputHtml = inputHtml.Replace("<", "%%tagsspan%%" + Server.HtmlEncode("<") + "%%endspan%%%%tagcontentspan%%");
            inputHtml = inputHtml.Replace("/>", "%%endspan%%%%tagsspan%%/" + Server.HtmlEncode(">") + "%%endspan%%");
            inputHtml = inputHtml.Replace(">", "%%endspan%%%%tagsspan%%" + Server.HtmlEncode(">") + "%%endspan%%");


            inputHtml = Regex.Replace(inputHtml, "(\\s(\\w*|\\w*[\\S]*\\w*)\\s*(=))",
                                      " %%propertynamespan%%$2%%endspan%%$3", RegexOptions.IgnoreCase);
            inputHtml = Regex.Replace(inputHtml, "(=\"(\\w*|[^\"]*|\\s*)\")", "%%propertyspan%%$1%%endspan%%",
                                      RegexOptions.IgnoreCase);


            inputHtml = inputHtml.Replace("%%tagcontentspan%%", tagcontentspan);
            inputHtml = inputHtml.Replace("%%propertynamespan%%", propertynamespan);
            inputHtml = inputHtml.Replace("%%propertyspan%%", propertyspan);
            inputHtml = inputHtml.Replace("%%tagsspan%%", tagspan);
            inputHtml = inputHtml.Replace("%%endspan%%", endspan);

            return inputHtml;
        }

        public string AddSpaces(int level)
        {
            string toReturn = "";
            for (int i = 0; i < level * 5; i++)
            {
                toReturn += "&nbsp;";
            }

            return toReturn;
        }

        public List<string> GetWebPartZones(string layout)
        {
            List<string> result = new List<string>();
            Regex zoneRegEx =
                RegexHelper.GetRegex("<fr:FWebPartZone [^>]*ID=\"([a-zA-Z1-9]+)\"[^>]*>[^>]*</fr:FWebPartZone>",
                                     RegexOptions.None);
            if (zoneRegEx.IsMatch(layout))
            {
                MatchCollection values = zoneRegEx.Matches(layout);

                foreach (Match val in values)
                {
                    result.Add(val.Groups[1].Value);
                }
            }
            return result;
        }
    }

}