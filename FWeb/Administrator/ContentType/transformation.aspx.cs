using System;
using System.Collections.Generic;
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

namespace FWeb.Administrator.ContentType
{
    public partial class transformation : FAdminPage
    {
        private ContentTypeProvider _contentTypeProvider;
        private TransformationProvider _transformationProvider;

        private int _contentTypeId = 0;
        protected int ContentTypeId
        {
            get
            {
                if (ViewState["_contentTypeId"] == null)
                {
                    _contentTypeId = ValidationHelper.GetInteger(Request.QueryString["contenttypeid"], 0);
                    if (_contentTypeId == 0)
                    {

                        List<ContentTypeInfo> list = _contentTypeProvider.SelectAll(ErrorList);
                        if (list != null && list.Count > 0)
                            _contentTypeId = list[0].Id;
                    }
                    return _contentTypeId;
                }
                return ValidationHelper.GetInteger(ViewState["_contentTypeId"], 0);
            }
            set { ViewState["_contentTypeId"] = value; }
        }

        private string SearchKey { get; set; }

        protected override void Init()
        {
            base.Init();
            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();
            if (_transformationProvider == null)
                _transformationProvider = new TransformationProvider();

        }

        protected override void FillGrid()
        {
            List<ContentTypeInfo> listInfos = _contentTypeProvider.SelectAll(ErrorList);
            drlList.DataSource = listInfos;
            drlList.DataBind();
            drlList.SelectedValue = ContentTypeId.ToString();
            List<TransformationInfo> transformationInfos =
                _transformationProvider.SelectPagingSortingByContentTypeId(PageSize, PageIndex, SortBy, SortOrder, ContentTypeId, ErrorList);
            rptList.DataSource = transformationInfos;
            rptList.DataBind();
        }

        protected override void ParsePost()
        {
            string action = Request.Form["form-action"];
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("add"))
                {
                    Response.Redirect("/administrator/contenttype/transformationaction.aspx?type=entry&contenttypeid=" + drlList.SelectedValue);
                }
                else if (action.Equals("edit"))
                {
                    string checkboxs = Request.Form["chbxRow"];
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        Response.Redirect("/administrator/contenttype/transformationaction.aspx?type=entry&id=" + temps[0]);
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
            Response.Redirect("transformation.aspx?contenttypeid=" + ContentTypeId.ToString() + "&size=" + drl.SelectedValue);
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
                    pager.TotalCount = _transformationProvider.SelectTotalCountByContentTypeId(ContentTypeId, new ErrorInfoList());
                }

            }
        }

        protected void btnSearch_onClick(object sender, EventArgs e)
        {
            SearchKey = txtSearch.Text;
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
            ContentTypeId = ValidationHelper.GetInteger(drlList.SelectedValue, 1);
            Response.Redirect("transformation.aspx?contenttypeid=" + ContentTypeId.ToString() + "&size=" + PageSize);
        }

        private void DeleteRows(string[] ids)
        {
            foreach (string item in ids)
            {
                if (!_transformationProvider.Delete(ValidationHelper.GetInteger(item, 0), ErrorList))
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
            builder.Append("<li>Transformation successfully saved</li>");
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
        }
    }
}