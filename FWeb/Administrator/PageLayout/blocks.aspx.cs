﻿using System;
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

namespace FWeb.Administrator.PageLayout
{
    public partial class blocks : FAdminPage
    {
        private BlockProvider _blockProvider;
        private LayoutProvider _layoutProvider;
        private LayoutNBlockProvider _layoutNBlockProvider;

        private int _layoutId = 0;
        protected int LayoutId
        {
            get
            {
                if (ViewState["_layoutId"] == null)
                {
                    _layoutId = ValidationHelper.GetInteger(Request.QueryString["layoutid"], 0);
                    if (_layoutId == 0)
                    {

                        List<LayoutInfo> layoutCategories = _layoutProvider.SelectAllByType(false, ErrorList);
                        if (layoutCategories != null && layoutCategories.Count > 0)
                            _layoutId = layoutCategories[0].Id;
                    }
                    return _layoutId;
                }
                return ValidationHelper.GetInteger(ViewState["_layoutId"], 0);
            }
            set { ViewState["_layoutId"] = value; }
        }

        private string SearchKey { get; set; }

        protected override void Init()
        {
            base.Init();
            if (_blockProvider == null)
                _blockProvider = new BlockProvider();
            if (_layoutProvider == null)
                _layoutProvider = new LayoutProvider();
            if (_layoutNBlockProvider == null)
                _layoutNBlockProvider = new LayoutNBlockProvider();
        }

        protected override void FillGrid()
        {
            List<LayoutInfo> layoutInfos = _layoutProvider.SelectAllByType(false, ErrorList);
            drlLayoutList.DataSource = layoutInfos;
            drlLayoutList.DataBind();
            drlLayoutList.SelectedValue = LayoutId.ToString();
            List<BlockInfo> blockInfos =
                _blockProvider.SelectPagingSortingByLayoutId(PageSize, PageIndex, SortBy, SortOrder, LayoutId, ErrorList);
            rptList.DataSource = blockInfos;
            rptList.DataBind();
        }

        protected override void ParsePost()
        {
            string action = Request.Form["form-action"];
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("add"))
                {
                    Response.Redirect("/administrator/pagelayout/blockaction.aspx?type=entry&layoutid=" + drlLayoutList.SelectedValue);
                }
                else if (action.Equals("edit"))
                {
                    string checkboxs = Request.Form["chbxRow"];
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        Response.Redirect("/administrator/pagelayout/blockaction.aspx?type=entry&id=" + temps[0]);
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
            Response.Redirect("/default.aspx?size=" + drl.SelectedValue);
            FillGrid();
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

        protected void lnkSortByNameWebPartZoneName_Click(object sender, EventArgs e)
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

        protected void lnkSortByOrder_Click(object sender, EventArgs e)
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

        protected void drlLayoutList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drlLayoutList = sender as DropDownList;
            LayoutId = ValidationHelper.GetInteger(drlLayoutList.SelectedValue, 1);
            FillGrid();
        }

        private void DeleteRows(string[] ids)
        {
            foreach (string item in ids)
            {
                if (!_layoutNBlockProvider.Delete(ValidationHelper.GetInteger(item, 0), ErrorList))
                {
                    PrintErrors();
                    break;
                }
            }
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
            builder.Append("<li>Page Layout Category successfully saved</li>");
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
        }
    }
}