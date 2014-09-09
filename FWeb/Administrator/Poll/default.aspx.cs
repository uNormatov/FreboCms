using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Class.Poll;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;
using FUIControls.UIControl;

namespace FWeb.Administrator.Poll
{
    public partial class _default : FAdminPage
    {
        private PollProvider _pollProvider;

        protected override void Init()
        {
            base.Init();
            if (_pollProvider == null)
                _pollProvider = new PollProvider();
        }

        protected override void FillGrid()
        {
            List<PollInfo> pollInfos = _pollProvider.SelectPollsPagingSorting(ValidationHelper.GetInteger(PageSize, 20), PageIndex, SortBy, SortOrder, ErrorList);
            rptList.DataSource = pollInfos;
            rptList.DataBind();
        }

        protected override void ParsePost()
        {
            string action = Request.Form["form-action"];
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("add"))
                {
                    Response.Redirect("/administrator/poll/pollaction.aspx?type=entry");
                }
                else if (action.Equals("edit"))
                {
                    string checkboxs = Request.Form["chbxRow"];
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        Response.Redirect("/administrator/poll/pollaction.aspx?type=entry&id=" + temps[0]);
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

        private void DeleteRows(string[] ids)
        {
            foreach (string id in ids)
            {
                _pollProvider.DeletePoll(ValidationHelper.GetInteger(id, 0), ErrorList);
            }
        }

        protected void DrlDisplaySelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drl = sender as DropDownList;
            Response.Redirect("default.aspx?size=" + drl.SelectedValue);
        }

        protected override void PrintErrors()
        {
            throw new NotImplementedException();
        }

        protected override void PrintSuccess()
        {
            throw new NotImplementedException();
        }

        protected void RptListItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                Image imgSortByQueation = e.Item.FindControl("imgSortByQueation") as Image;
                Image imgSortByBlockMode = e.Item.FindControl("imgSortByBlockMode") as Image;
                Image imgSortByIsActive = e.Item.FindControl("imgSortByIsActive") as Image;
                Image imgId = e.Item.FindControl("imgSortById") as Image;

                string image = "/content/css/images/menu/sort-asc.png";
                if (SortOrder == "DESC")
                    image = "/content/css/images/menu/sort-desc.png";

                switch (SortBy)
                {
                    case "Question":
                        imgSortByQueation.ImageUrl = image;
                        imgSortByBlockMode.ImageUrl = string.Empty;
                        imgSortByIsActive.ImageUrl = string.Empty;
                        imgId.ImageUrl = string.Empty;
                        break;
                    case "BlockMode":
                        imgSortByQueation.ImageUrl = string.Empty;
                        imgSortByBlockMode.ImageUrl = image;
                        imgSortByIsActive.ImageUrl = string.Empty;
                        imgId.ImageUrl = string.Empty;
                        break;
                    case "IsActive":
                        imgSortByQueation.ImageUrl = string.Empty;
                        imgSortByBlockMode.ImageUrl = string.Empty;
                        imgSortByIsActive.ImageUrl = image;
                        imgId.ImageUrl = string.Empty;
                        break;
                    case "Id":
                        imgSortByQueation.ImageUrl = string.Empty;
                        imgSortByBlockMode.ImageUrl = string.Empty;
                        imgSortByIsActive.ImageUrl = string.Empty;
                        imgId.ImageUrl = image;
                        break;
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
                    pager.TotalCount = _pollProvider.SelectTotalCount(new ErrorInfoList());
                    pager.PageIndex = PageIndex;
                    pager.PageSize = PageSize;
                    pager.PageIndexKeywordType = QueryParameterType.QueryString;
                    pager.PageIndexKeyword = "page";
                }
            }

        }

        protected void LnkSortByQuestionClick(object sender, EventArgs e)
        {
            if (SortBy == "Question")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "Question";
                SortOrder = "ASC";
            }
            FillGrid();
        }

        protected void LnkSortByBlockModeClick(object sender, EventArgs e)
        {
            if (SortBy == "BlockMode")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "BlockMode";
                SortOrder = "ASC";
            }
            FillGrid();
        }

        protected void LnkSortByIsActive(object sender, EventArgs e)
        {
            if (SortBy == "IsActive")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "IsActive";
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
    }
}