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

namespace FWeb.Administrator.Page
{
    public partial class blocks : FAdminPage
    {
        private BlockProvider _blockProvider;
        private PageProvider _pageProvider;
        private PageNBlockProvider _pageNBlockProvider;

        private int _pageId = 0;
        protected int PageId
        {
            get
            {
                _pageId = ValidationHelper.GetInteger(Request.QueryString["pageid"], 0);
                if (_pageId == 0)
                {

                    List<PageInfo> pages = _pageProvider.SelectAll(ErrorList);
                    if (pages != null && pages.Count > 0)
                        _pageId = pages[0].Id;
                }
                return _pageId;
            }
        }

        private string SearchKey { get; set; }

        protected override void Init()
        {
            base.Init();
            if (_blockProvider == null)
                _blockProvider = new BlockProvider();
            if (_pageProvider == null)
                _pageProvider = new PageProvider();
            if (_pageNBlockProvider == null)
                _pageNBlockProvider = new PageNBlockProvider();
        }

        protected override void FillGrid()
        {
            FillPages();
            List<BlockInfo> blockInfos =
                  _blockProvider.SelectPagingSortingByPageId(PageSize, PageIndex, SortBy, SortOrder, PageId, ErrorList);
            rptList.DataSource = blockInfos;
            rptList.DataBind();
        }

        private void FillPages()
        {
            List<PageInfo> pages = _pageProvider.SelectAll(ErrorList);
            if (CheckErrors())
            {
                int depth = 0;
                drlPageList.Items.Add(new ListItem("Select", "0", true));
                if (pages != null && pages.Count > 0)
                    FillChildPages(pages, 0, depth);

                for (int i = 0; i < drlPageList.Items.Count; i++)
                {
                    if (drlPageList.Items[i].Value != PageId.ToString()) continue;
                    drlPageList.SelectedIndex = i; break;
                }
            }
        }

        private void FillChildPages(List<PageInfo> pages, int parentId, int depth)
        {
            List<PageInfo> childPages = pages.Where(x => x.ParentId == parentId).ToList();
            if (childPages.Count <= 0) return;

            string childRow = string.Empty;
            for (int i = 0; i < depth; i++)
                childRow += "- ";
            foreach (PageInfo pageInfo in childPages)
            {
                drlPageList.Items.Add(new ListItem(string.Format(" - {0}{1}", childRow, pageInfo.Name), pageInfo.Id.ToString()));
                FillChildPages(pages, pageInfo.Id, depth + 1);
            }
        }

        protected override void ParsePost()
        {
            string action = Request.Form["form-action"];
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("add"))
                {
                    Response.Redirect("/administrator/page/blockaction.aspx?type=entry&pageid=" + drlPageList.SelectedValue);
                }
                else if (action.Equals("edit"))
                {
                    string checkboxs = Request.Form["chbxRow"];
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        Response.Redirect("/administrator/page/blockaction.aspx?type=entry&id=" + temps[0]);
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
            Response.Redirect("blocks.aspx?size=" + drl.SelectedValue);
            FillGrid();
        }

        protected void RptListItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                Image nameImage = e.Item.FindControl("imgSortByName") as Image;
                Image idImage = e.Item.FindControl("imgSortById") as Image;
                Image webPartZoneNameImage = e.Item.FindControl("imgSortByWebPartZoneName") as Image;
                Image orderImage = e.Item.FindControl("imgSortByOrder") as Image;

                string image = "/content/css/images/menu/sort-asc.png";
                if (SortOrder == "DESC")
                    image = "/content/css/images/menu/sort-desc.png";

                if (SortBy == "Name")
                {
                    nameImage.ImageUrl = image;
                    idImage.ImageUrl = "";
                    webPartZoneNameImage.ImageUrl = "";
                    orderImage.ImageUrl = "";

                }
                else if (SortBy == "Id")
                {
                    idImage.ImageUrl = image;
                    nameImage.ImageUrl = "";
                    webPartZoneNameImage.ImageUrl = "";
                    orderImage.ImageUrl = "";
                }
                else if (SortBy == "WebPartZoneName")
                {
                    idImage.ImageUrl = "";
                    nameImage.ImageUrl = "";
                    webPartZoneNameImage.ImageUrl = image;
                    orderImage.ImageUrl = "";
                }
                else
                {
                    idImage.ImageUrl = "";
                    nameImage.ImageUrl = "";
                    webPartZoneNameImage.ImageUrl = "";
                    orderImage.ImageUrl = image;
                }

            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList drlDisplay = e.Item.FindControl("drlDisplay") as DropDownList;
                for (int i = 0; i < drlDisplay.Items.Count; i++)
                {
                    if (drlDisplay.Items[i].Value.Equals(PageSize.ToString()))
                    {
                        drlDisplay.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        protected void BtnSearchOnClick(object sender, EventArgs e)
        {
            TextBox txtSearch = sender as TextBox;
            SearchKey = txtSearch.Text;
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

        protected void LnkSortByNameWebPartZoneNameClick(object sender, EventArgs e)
        {
            if (SortBy == "WebPartZoneName")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "WebPartZoneName";
                SortOrder = "ASC";
            }
            FillGrid();
        }

        protected void LnkSortByOrderClick(object sender, EventArgs e)
        {
            if (SortBy == "Order")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "Order";
                SortOrder = "ASC";
            }
            FillGrid();
        }

        protected void DrlPageListSelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drlPageList = sender as DropDownList;
            Response.Redirect("blocks.aspx?pageid=" + drlPageList.SelectedValue);

        }

        private void DeleteRows(string[] ids)
        {
            foreach (string item in ids)
            {
                if (!_pageNBlockProvider.Delete(ValidationHelper.GetInteger(item, 0), ErrorList))
                {
                    PrintErrors();
                    break;
                }
            }
            PageInfo pageInfo = _pageProvider.Select(PageId, ErrorList);
            _pageProvider.DeleteObjectFromCache(pageInfo);
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
            builder.Append("<li>Page Block successfully saved</li>");
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
        }
    }
}