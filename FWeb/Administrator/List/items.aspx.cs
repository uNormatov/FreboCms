using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;
using FUIControls.UIControl;

namespace FWeb.Administrator.List
{
    public partial class items : FAdminPage
    {
        private ListProvider _listProvider;
        private ListItemProvider _listItemProvider;

        private int _listId = 0;
        private int ListId
        {
            get
            {
                if (ViewState["_listId"] == null)
                {
                    _listId = ValidationHelper.GetInteger(Request.QueryString["listid"], 0);
                    if (_listId == 0)
                    {

                        List<ListInfo> list = _listProvider.SelectAll(ErrorList);
                        if (list != null && list.Count > 0)
                            _listId = list[0].Id;
                    }
                    return _listId;
                }
                return ValidationHelper.GetInteger(ViewState["_listId"], 0);
            }
            set { ViewState["_listId"] = value; }
        }

        private string SearchKey { get; set; }

        protected override void Init()
        {
            base.Init();
            if (_listProvider == null)
                _listProvider = new ListProvider();
            if (_listItemProvider == null)
                _listItemProvider = new ListItemProvider();

        }

        protected override void FillGrid()
        {
            List<ListInfo> listInfos = _listProvider.SelectAll(ErrorList);
            drlList.DataSource = listInfos;
            drlList.DataBind();
            drlList.SelectedValue = ListId.ToString();
            List<ListItemInfo> items = _listItemProvider.SelectPagingSortingByListId(PageSize, PageIndex, SortBy, SortOrder, ListId, ErrorList);
            rptList.DataSource = items;
            rptList.DataBind();
        }

        protected override void ParsePost()
        {
            string action = Request.Form["form-action"];
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("add"))
                {
                    Response.Redirect("/administrator/list/itemaction.aspx?type=entry&listid=" + drlList.SelectedValue);
                }
                else if (action.Equals("edit"))
                {
                    string checkboxs = Request.Form["chbxRow"];
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        Response.Redirect("/administrator/list/itemaction.aspx?type=entry&id=" + temps[0]);
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

        protected void drlDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drl = sender as DropDownList;
            Response.Redirect("items.aspx?size=" + drl.SelectedValue + "&listid=" + ListId);
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                Image nameImage = e.Item.FindControl("imgSortByName") as Image;
                Image idImage = e.Item.FindControl("imgSortById") as Image;

                string image = "/content/css/images/menu/sort-asc.png";
                if (SortOrder == "DESC")
                    image = "/content/css/images/menu/sort-desc.png";

                if (SortBy == "Name")
                {
                    nameImage.ImageUrl = image;
                    idImage.ImageUrl = "";

                }
                else if (SortBy == "Id")
                {
                    idImage.ImageUrl = image;
                    nameImage.ImageUrl = "";
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

                FPager pager = e.Item.FindControl("pager") as FPager;
                if (pager != null)
                {
                    pager.PageIndex = PageIndex;
                    pager.PageSize = PageSize;
                    pager.TotalCount = _listItemProvider.SelectTotalCountByListId(ListId, new ErrorInfoList());
                }
            }
        }

        protected void btnSearch_onClick(object sender, EventArgs e)
        {
            TextBox txtSearch = sender as TextBox;
            SearchKey = txtSearch.Text;
            FillGrid();
        }

        protected void lnkSortById_Click(object sender, EventArgs e)
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

        protected void lnkSortByName_Click(object sender, EventArgs e)
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

        protected void drlList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drlList = sender as DropDownList;
            ListId = ValidationHelper.GetInteger(drlList.SelectedValue, 1);
            FillGrid();
        }

        private void DeleteRows(string[] ids)
        {
            foreach (string item in ids)
            {
                if (!_listItemProvider.Delete(ValidationHelper.GetInteger(item, 0), ErrorList))
                {
                    PrintErrors();
                    break;
                }
            }

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
            builder.Append("<li>List Item successfully saved</li>");
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
        }
    }
}