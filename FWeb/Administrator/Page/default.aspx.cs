using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;
using FUIControls.UIControl;

namespace FWeb.Administrator.Page
{
    public partial class _default : FAdminPage
    {
        private PageProvider _pageProvider;

        protected override void Init()
        {
            base.Init();
            if (_pageProvider == null)
                _pageProvider = new PageProvider();
        }

        protected override void FillGrid()
        {
            List<PageInfo> categoryList = _pageProvider.SelectPagingSorting(ValidationHelper.GetInteger(PageSize, 20), PageIndex, SortBy, SortOrder, ErrorList);
            rptList.DataSource = categoryList;
            rptList.DataBind();
        }

        protected override void ParsePost()
        {
            string action = Request.Form["form-action"];
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("add"))
                {
                    Response.Redirect("/administrator/page/action.aspx?type=entry");
                }
                else if (action.Equals("edit"))
                {
                    string checkboxs = Request.Form["chbxRow"];
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        Response.Redirect("/administrator/page/action.aspx?type=entry&id=" + temps[0]);
                    }
                }
                else
                {
                    string checkboxs = Request.Form["chbxRow"];
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        DeleteRows(temps);
                    }
                }
            }
            base.ParsePost();
        }

        protected void DrlDisplaySelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drl = sender as DropDownList;
            Response.Redirect("default.aspx?size=" + drl.SelectedValue);
        }

        protected void RptListItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                Image nameImage = e.Item.FindControl("imgSortByName") as Image;
                Image idImage = e.Item.FindControl("imgSortById") as Image;
                Image titleImage = e.Item.FindControl("imgSortByTitle") as Image;
                Image seoTemplate = e.Item.FindControl("imgSortBySeoTemplate") as Image;

                string image = "/content/css/images/menu/sort-asc.png";
                if (SortOrder == "DESC")
                    image = "/content/css/images/menu/sort-desc.png";

                if (SortBy == "Name")
                {
                    nameImage.ImageUrl = image;
                    idImage.ImageUrl = "";
                    titleImage.ImageUrl = "";
                    seoTemplate.ImageUrl = "";
                }
                else if (SortBy == "Id")
                {
                    idImage.ImageUrl = image;
                    nameImage.ImageUrl = "";
                    titleImage.ImageUrl = "";
                    seoTemplate.ImageUrl = "";
                }
                else if (SortBy == "Title")
                {
                    idImage.ImageUrl = "";
                    nameImage.ImageUrl = "";
                    titleImage.ImageUrl = image;
                    seoTemplate.ImageUrl = "";
                }
                else if (SortBy == "SeoTemplate")
                {
                    idImage.ImageUrl = "";
                    nameImage.ImageUrl = "";
                    titleImage.ImageUrl = "";
                    seoTemplate.ImageUrl = image;
                }

            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList drlDisplay = e.Item.FindControl("drlDisplay") as DropDownList;
                if (drlDisplay != null)
                {
                    for (int i = 0; i < drlDisplay.Items.Count; i++)
                    {
                        if (drlDisplay.Items[i].Value.Equals(PageSize.ToString()))
                        {
                            drlDisplay.SelectedIndex = i;
                            break;
                        }
                    }
                }

                FPager pager = e.Item.FindControl("pager") as FPager;
                if (pager != null)
                {
                    pager.IsBackend = true;
                    pager.TotalCount = _pageProvider.SelectTotalCount(new ErrorInfoList());
                    pager.PageIndex = PageIndex;
                    pager.PageSize = PageSize;
                    pager.PageIndexKeywordType = QueryParameterType.QueryString;
                    pager.PageIndexKeyword = "page";
                }
            }
        }

        protected void LnkSortByIdClick(object sender, EventArgs e)
        {
            if (SortBy == "Id")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "Id";
                SortOrder = "ASC";
            }
            FillGrid();
        }

        protected void lnkSortByTitle_OnClick(object sender, EventArgs e)
        {

            if (SortBy == "Title")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "Title";
                SortOrder = "ASC";
            }
            FillGrid();
        }

        protected void lnkSortBySeoTemplate_OnClick(object sender, EventArgs e)
        {

            if (SortBy == "SeoTemplate")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "SeoTemplate";
                SortOrder = "ASC";
            }
            FillGrid();
        }

        protected void LnkSortByNameClick(object sender, EventArgs e)
        {
            if (SortBy == "Name")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "Name";
                SortOrder = "ASC";
            }

            FillGrid();
        }

        private void DeleteRows(IEnumerable<string> ids)
        {
            if (ids.Any(item => !_pageProvider.Delete(ValidationHelper.GetInteger(item, 0), ErrorList)))
                PrintErrors();
            CacheHelper.ClearCaches();
            FillGrid();
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
            ErrorList.Clear();
        }

        protected override void PrintSuccess()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<dl id=\"system-message\">");
            builder.Append("<dt class=\"message\">Message</dt><dd class=\"message message\">");
            builder.Append("<ul>");
            builder.Append("<li>Page successfully saved</li>");
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
        }
    }
}