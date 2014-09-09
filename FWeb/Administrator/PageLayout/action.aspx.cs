using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;
using FUIControls.Settings;

namespace FWeb.Administrator.PageLayout
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
                            1, 1, "Id", "DESC", false, ErrorList);
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

            if (IsEdit)
            {
                Title = "Edit Layout | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "Edit Layout";
            }
            else
            {
                Title = "New Page Layout | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "New  Layout";
            }
            CancelUrl = "/administrator/pagelayout/layouts.aspx?categoryid=" + drlCategory.SelectedValue;
        }

        protected override void FillFields()
        {
            List<LayoutCategoryInfo> layoutCategories = _layoutCategoryProvider.SelectAllByType(false, ErrorList);
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
                    txtDescription.Text = layoutInfo.Description;
                    txtBodyContent.Text = layoutInfo.Layout;
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
                layoutInfo.Description = txtDescription.Text;
                layoutInfo.Layout = txtBodyContent.Text;
                layoutInfo.IsMaster = false;
                layoutInfo.IsDeleted = false;

                if (flpScreenshot.HasFile)
                {
                    string guid = Guid.NewGuid().ToString();
                    string fileName = guid + "_" + flpScreenshot.FileName;
                    flpScreenshot.SaveAs(Server.MapPath("~/userfiles/pagelayouts/") + fileName);
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
            RedrictUrl = "/administrator/pagelayout/layouts.aspx?categoryid=" + drlCategory.SelectedValue;
            CancelUrl = RedrictUrl;

            return CheckErrors();
        }

        protected override bool Insert()
        {
            LayoutInfo layoutInfo = new LayoutInfo();
            layoutInfo.Name = txtName.Text;
            layoutInfo.LayoutCategoryId = ValidationHelper.GetInteger(drlCategory.SelectedValue, 0);
            layoutInfo.Description = txtDescription.Text;
            layoutInfo.Layout = txtBodyContent.Text;
            layoutInfo.IsMaster = false;
            layoutInfo.IsDeleted = false;

            layoutInfo.BodyOption = string.Empty;
            layoutInfo.DocOption = string.Empty;
            layoutInfo.Css = string.Empty;
            if (flpScreenshot.HasFile)
            {
                string guid = Guid.NewGuid().ToString();
                string fileName = guid + "_" + flpScreenshot.FileName;
                flpScreenshot.SaveAs(Server.MapPath("~/userfiles/pagelayouts/") + fileName);
                layoutInfo.Screenshot = fileName;
            }
            else layoutInfo.Screenshot = string.Empty;

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
            RedrictUrl = "/administrator/pagelayout/layouts.aspx?categoryid=" + drlCategory.SelectedValue;
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