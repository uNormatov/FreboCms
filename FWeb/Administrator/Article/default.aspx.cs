using System;
using System.Collections.Generic;
using System.Linq;
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

namespace FWeb.Administrator.Article
{
    public partial class _default : FAdminPage
    {
        private ArticleProvider _articleProvider;

        protected override void Init()
        {
            base.Init();
            if (_articleProvider == null)
                _articleProvider = new ArticleProvider();
        }

        protected override void PrintErrors()
        {

        }

        protected override void PrintSuccess()
        {

        }

        protected override void FillGrid()
        {
            List<ArticleInfo> articleInfos = _articleProvider.SelectPagingSorting(ValidationHelper.GetInteger(PageSize, 20), PageIndex, SortBy, SortOrder, ErrorList);
            rptList.DataSource = articleInfos;
            rptList.DataBind();
        }

        protected override void ParsePost()
        {
            string action = Request.Form["form-action"];
            if (!string.IsNullOrEmpty(action))
            {
                string checkboxs = Request.Form["chbxRow"];
                if (action.Equals("add"))
                {
                    Response.Redirect("/administrator/article/action.aspx?type=entry");
                }
                else if (action.Equals("edit"))
                {
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        Response.Redirect("/administrator/article/action.aspx?type=entry&id=" + temps[0]);
                    }
                }
                else if (action.Equals("publish"))
                {
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        PublishRows(temps, true);
                    }
                }
                else if (action.Equals("unpublish"))
                {
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        PublishRows(temps, false);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        DeleteRows(temps);
                    }
                }
            }
            base.ParsePost();
        }

        private void DeleteRows(IEnumerable<string> ids)
        {
            if (ids.Any(item => !_articleProvider.Delete(ValidationHelper.GetInteger(item, 0), ErrorList)))
                PrintErrors();
            CacheHelper.ClearCaches();
            FillGrid();
        }

        private void PublishRows(IEnumerable<string> ids, bool publish)
        {

        }

        protected void drlDisplay_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drl = sender as DropDownList;
            Response.Redirect("default.aspx?size=" + drl.SelectedValue);
        }

        protected void rptList_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                Image imgSortByTitle = e.Item.FindControl("imgSortByTitle") as Image;
                Image imgSortByCode = e.Item.FindControl("imgSortByCode") as Image;
                Image imgSortByCreatedBy = e.Item.FindControl("imgSortByCreatedBy") as Image;
                Image imgSortByCreatedDate = e.Item.FindControl("imgSortByCreatedDate") as Image;
                Image imgSortByModifedBy = e.Item.FindControl("imgSortByModifedBy") as Image;
                Image imgSortByModifiedDate = e.Item.FindControl("imgSortByModifiedDate") as Image;
                Image imgSortById = e.Item.FindControl("imgSortById") as Image;

                string image = "/content/css/images/menu/sort-asc.png";
                if (SortOrder == "DESC")
                    image = "/content/css/images/menu/sort-desc.png";

                if (SortBy == "Title")
                {
                    imgSortByTitle.ImageUrl = image;
                    imgSortByCode.ImageUrl = "";
                    imgSortByCreatedBy.ImageUrl = "";
                    imgSortByCreatedDate.ImageUrl = "";
                    imgSortByModifedBy.ImageUrl = "";
                    imgSortByModifiedDate.ImageUrl = "";
                    imgSortById.ImageUrl = "";
                }
                else if (SortBy == "Id")
                {
                    imgSortByTitle.ImageUrl = "";
                    imgSortByCode.ImageUrl = "";
                    imgSortByCreatedBy.ImageUrl = "";
                    imgSortByCreatedDate.ImageUrl = "";
                    imgSortByModifedBy.ImageUrl = "";
                    imgSortByModifiedDate.ImageUrl = "";
                    imgSortById.ImageUrl = image;
                }
                else if (SortBy == "Code")
                {
                    imgSortByTitle.ImageUrl = "";
                    imgSortByCode.ImageUrl = image;
                    imgSortByCreatedBy.ImageUrl = "";
                    imgSortByCreatedDate.ImageUrl = "";
                    imgSortByModifedBy.ImageUrl = "";
                    imgSortByModifiedDate.ImageUrl = "";
                    imgSortById.ImageUrl = "";
                }
                else if (SortBy == "CreatedBy")
                {
                    imgSortByTitle.ImageUrl = "";
                    imgSortByCode.ImageUrl = "";
                    imgSortByCreatedBy.ImageUrl = image;
                    imgSortByCreatedDate.ImageUrl = "";
                    imgSortByModifedBy.ImageUrl = "";
                    imgSortByModifiedDate.ImageUrl = "";
                    imgSortById.ImageUrl = "";
                }
                else if (SortBy == "CreatedDate")
                {
                    imgSortByTitle.ImageUrl = "";
                    imgSortByCode.ImageUrl = "";
                    imgSortByCreatedBy.ImageUrl = "";
                    imgSortByCreatedDate.ImageUrl = image;
                    imgSortByModifedBy.ImageUrl = "";
                    imgSortByModifiedDate.ImageUrl = "";
                    imgSortById.ImageUrl = "";
                }
                else if (SortBy == "ModifiedBy")
                {
                    imgSortByTitle.ImageUrl = "";
                    imgSortByCode.ImageUrl = "";
                    imgSortByCreatedBy.ImageUrl = "";
                    imgSortByCreatedDate.ImageUrl = "";
                    imgSortByModifedBy.ImageUrl = image;
                    imgSortByModifiedDate.ImageUrl = "";
                    imgSortById.ImageUrl = "";
                }
                else if (SortBy == "ModifiedDate")
                {
                    imgSortByTitle.ImageUrl = "";
                    imgSortByCode.ImageUrl = "";
                    imgSortByCreatedBy.ImageUrl = "";
                    imgSortByCreatedDate.ImageUrl = "";
                    imgSortByModifedBy.ImageUrl = "";
                    imgSortByModifiedDate.ImageUrl = image;
                    imgSortById.ImageUrl = "";
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
                    pager.TotalCount = _articleProvider.SelectTotalCount(new ErrorInfoList());
                    pager.PageIndex = PageIndex;
                    pager.PageSize = PageSize;
                    pager.PageIndexKeywordType = QueryParameterType.QueryString;
                    pager.PageIndexKeyword = "page";
                }
            }
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

        protected void lnkCode_OnClick(object sender, EventArgs e)
        {
            if (SortBy == "Code")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "Code";
                SortOrder = "ASC";
            }
            FillGrid();
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

        protected void lnkCreatedBy_OnClick(object sender, EventArgs e)
        {
            if (SortBy == "CreatedBy")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "CreatedBy";
                SortOrder = "ASC";
            }
            FillGrid();
        }

        protected void lnkCreatedDate_OnClick(object sender, EventArgs e)
        {
            if (SortBy == "CreatedDate")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "CreatedDate";
                SortOrder = "ASC";
            }
            FillGrid();
        }

        protected void lnkModifiedBy_OnClick(object sender, EventArgs e)
        {
            if (SortBy == "ModifiedBy")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "ModifiedBy";
                SortOrder = "ASC";
            }
            FillGrid();
        }

        protected void lnkModifiedDate_OnClick(object sender, EventArgs e)
        {
            if (SortBy == "ModifiedDate")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "ModifiedDate";
                SortOrder = "ASC";
            }
            FillGrid();
        }
    }
}