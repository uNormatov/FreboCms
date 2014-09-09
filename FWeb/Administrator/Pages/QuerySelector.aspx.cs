using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;
using FUIControls.UIControl;

namespace FWeb.Administrator.Pages
{
    public partial class QuerySelector : FAdminPage
    {
        private ContentTypeProvider _contentTypeProvider;
        private QueryProvider _queryProvider;

        protected int ContentTypeId
        {
            get
            {
                int contentTypeId = ValidationHelper.GetInteger(Request.QueryString["contenttypeid"], 0);
                if (contentTypeId == 0)
                {
                    if (!string.IsNullOrEmpty(QueryName))
                    {
                        QueryInfo queryInfo = _queryProvider.SelectByName(QueryName, ErrorList);
                        contentTypeId = queryInfo.ContentTypeId;
                    }
                    else
                    {
                        List<ContentTypeInfo> list = _contentTypeProvider.SelectAll(ErrorList);
                        if (list != null && list.Count > 0)
                            contentTypeId = list[0].Id;
                    }
                }
                return contentTypeId;
            }
        }

        private string QueryName
        {
            get { return ValidationHelper.GetString(Request.QueryString["name"], string.Empty); }
        }

        protected string ControlId
        {
            get
            {
                return ValidationHelper.GetString(Request.QueryString["controlid"], string.Empty);
            }
        }

        protected override void Init()
        {
            base.Init();
            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();
            if (_queryProvider == null)
                _queryProvider = new QueryProvider();
        }

        protected override void FillGrid()
        {
            txtSearch.Text = SearchKeyword;

            List<ContentTypeInfo> listInfos = _contentTypeProvider.SelectAll(ErrorList);
            drlList.DataSource = listInfos;
            drlList.DataBind();
            drlList.SelectedValue = ContentTypeId.ToString();

            List<QueryInfo> queryInfos;
            if (string.IsNullOrEmpty(SearchKeyword))
                queryInfos = _queryProvider.SelectPagingSortingByContentTypeId(PageSize, PageIndex, SortBy, SortOrder, ContentTypeId, ErrorList);
            else
                queryInfos = _queryProvider.SelectPagingSortingByContentTypeIdSearchKeyword(PageSize, PageIndex, SortBy, SortOrder, ContentTypeId, SearchKeyword, ErrorList);
            rptList.DataSource = queryInfos;
            rptList.DataBind();
        }

        protected override void PrintErrors()
        {
            throw new NotImplementedException();
        }

        protected override void PrintSuccess()
        {
            throw new NotImplementedException();
        }

        protected void rptList_OnItemDataBound(object sender, RepeaterItemEventArgs e)
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
                drlDisplay.SelectedValue = PageSize.ToString();

                FPager pager = e.Item.FindControl("pager") as FPager;
                if (pager != null)
                {
                    pager.PageIndex = PageIndex;
                    pager.PageSize = PageSize;
                    if (string.IsNullOrEmpty(SearchKeyword))
                        pager.TotalCount = _queryProvider.SelectTotalCountByContentTypeId(ContentTypeId, new ErrorInfoList());
                    else
                        pager.TotalCount = _queryProvider.SelectTotalCountByContentTypeIdKeyword(ContentTypeId, SearchKeyword, new ErrorInfoList());
                }

            }
        }

        protected void lnkSortByName_OnClick(object sender, EventArgs e)
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

        protected void lnkSortById_OnClick(object sender, EventArgs e)
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

        protected void drlDisplay_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drl = sender as DropDownList;
            string url = string.Format("queryselector.aspx?controlid={0}&contenttypeid={1}&size={2}&name={3}", ControlId, ContentTypeId, drl.SelectedValue, QueryName);
            if (!string.IsNullOrEmpty(SearchKeyword))
                url = string.Format("{0}&keyword={1}", url, Server.UrlEncode(SearchKeyword));
            Response.Redirect(url);
        }

        protected void drlList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string url = string.Format("queryselector.aspx?controlid={0}&contenttypeid={1}&size={2}&name={3}", ControlId, drlList.SelectedValue, PageSize, QueryName);
            if (!string.IsNullOrEmpty(SearchKeyword))
                url = string.Format("{0}&keyword={1}", url, Server.UrlEncode(SearchKeyword));
            Response.Redirect(url);
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            string url = string.Format("queryselector.aspx?controlid={0}&contenttypeid={1}&size={2}&name={3}", ControlId, ContentTypeId, PageSize, QueryName);
            if (!string.IsNullOrEmpty(txtSearch.Text))
                url = string.Format("{0}&keyword={1}", url, Server.UrlEncode(txtSearch.Text));
            Response.Redirect(url);
        }

        protected void btnClear_OnClick(object sender, EventArgs e)
        {
            string url = string.Format("queryselector.aspx?controlid={0}&contenttypeid={1}&size={2}&name={3}", ControlId, ContentTypeId, PageSize, QueryName);
            Response.Redirect(url);
        }
    }
}