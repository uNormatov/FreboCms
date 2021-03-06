﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Collection;
using FUIControls.Page;
using FDataProvider;
using FCore.Helper;
using FCore.Class;

namespace FWeb.Administrator.SiteLayout
{
    public partial class layouts : FAdminPage
    {
        private LayoutProvider _layoutProvider;
        private LayoutCategoryProvider _layoutCategoryProvider;

        private int _categoryId = 0;
        private int CategoryId
        {
            get
            {
                if (ViewState["_categoryId"] == null)
                {
                    _categoryId = ValidationHelper.GetInteger(Request.QueryString["categoryid"], 0);
                    if (_categoryId == 0)
                    {
                        List<LayoutCategoryInfo> layoutCategories = _layoutCategoryProvider.SelectAllByType(true, ErrorList);
                        if (layoutCategories != null && layoutCategories.Count > 0)
                            _categoryId = layoutCategories[0].Id;
                    }
                    return _categoryId;
                }
                return ValidationHelper.GetInteger(ViewState["_categoryId"], 0);
            }
            set { ViewState["_categoryId"] = value; }
        }

        private string SearchKey { get; set; }

        protected override void Init()
        {
            base.Init();
            if (_layoutProvider == null)
                _layoutProvider = new LayoutProvider();
            if (_layoutCategoryProvider == null)
                _layoutCategoryProvider = new LayoutCategoryProvider();
        }

        protected override void FillGrid()
        {
            List<LayoutCategoryInfo> layoutCategories = _layoutCategoryProvider.SelectAllByType(true, ErrorList);
            drlCategoryList.DataSource = layoutCategories;
            drlCategoryList.DataBind();
            drlCategoryList.SelectedValue = CategoryId.ToString();
            List<LayoutInfo> layoutInfos = _layoutProvider.SelectPagingSortingByCategoryId(PageSize, PageIndex, SortBy, SortOrder, CategoryId, ErrorList);
            rptList.DataSource = layoutInfos;
            rptList.DataBind();
        }

        protected override void ParsePost()
        {
            string action = Request.Form["form-action"];
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("add"))
                {
                    Response.Redirect("/administrator/sitelayout/action.aspx?type=entry&categoryid=" + drlCategoryList.SelectedValue);
                }
                else if (action.Equals("edit"))
                {
                    string checkboxs = Request.Form["chbxRow"];
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        Response.Redirect("/administrator/sitelayout/action.aspx?type=entry&id=" + temps[0]);
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
            Response.Redirect("layouts.aspx?size=" + drl.SelectedValue);
            FillGrid();
        }

        protected void RptListItemDataBound(object sender, RepeaterItemEventArgs e)
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

        protected void BtnSearchOnClick(object sender, EventArgs e)
        {
            TextBox txtSearch = sender as TextBox;
            SearchKey = txtSearch.Text;
            FillGrid();
        }

        protected void DrlCategoryListSelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drlCategoryList = sender as DropDownList;
            CategoryId = ValidationHelper.GetInteger(drlCategoryList.SelectedValue, 1);
            FillGrid();
        }

        private void DeleteRows(string[] ids)
        {
            foreach (string item in ids)
            {
                if (!_layoutProvider.Delete(ValidationHelper.GetInteger(item, 0), ErrorList))
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
            builder.Append("<li>Site Layout successfully saved</li>");
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
        }
    }
}