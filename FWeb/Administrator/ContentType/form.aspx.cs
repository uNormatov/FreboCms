using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;
using FUIControls.UIControl;

namespace FWeb.Administrator.ContentType
{
    public partial class form : FAdminPage
    {
        private ContentTypeProvider _contentTypeProvider;
        private FormProvider _formProvider;

        protected int ContentTypeId
        {
            get
            {
                int contentTypeId = ValidationHelper.GetInteger(Request.QueryString["contenttypeid"], 0);
                if (contentTypeId == 0)
                {
                    List<ContentTypeInfo> list = _contentTypeProvider.SelectAll(ErrorList);
                    if (list != null && list.Count > 0)
                        contentTypeId = list[0].Id;
                }
                return contentTypeId;
            }
        }

        private string SearchKey { get; set; }

        protected override void Init()
        {
            base.Init();
            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();
            if (_formProvider == null)
                _formProvider = new FormProvider();
        }

        protected override void FillGrid()
        {
            List<ContentTypeInfo> listInfos = _contentTypeProvider.SelectAll(ErrorList);
            drlList.DataSource = listInfos;
            drlList.DataBind();
            drlList.SelectedValue = ContentTypeId.ToString();
            List<FormInfo> formInfos =
                _formProvider.SelectPagingSortingByContentTypeId(PageSize, PageIndex, SortBy, SortOrder, ContentTypeId,
                                                                 ErrorList);
            rptList.DataSource = formInfos;
            rptList.DataBind();
        }

        protected override void ParsePost()
        {
            string action = Request.Form["form-action"];
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("add"))
                {
                    Response.Redirect("/administrator/contenttype/formaction.aspx?type=entry&contenttypeid=" +
                                      drlList.SelectedValue);
                }
                else if (action.Equals("edit"))
                {
                    string checkboxs = Request.Form["chbxRow"];
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        Response.Redirect("/administrator/contenttype/formaction.aspx?type=entry&id=" + temps[0]);
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
            foreach (string item in ids)
            {
                if (!_formProvider.Delete(ValidationHelper.GetInteger(item, 0), ErrorList))
                {
                    PrintErrors();
                    break;
                }
            }

            FillGrid();
        }

        protected override void PrintErrors()
        {
            var builder = new StringBuilder();
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
            var builder = new StringBuilder();
            builder.Append("<dl id=\"system-message\">");
            builder.Append("<dt class=\"message\">Message</dt><dd class=\"message message\">");
            builder.Append("<ul>");
            builder.Append("<li>Form successfully saved</li>");
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
        }

        protected void drlDisplay_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var drl = sender as DropDownList;
            Response.Redirect("form.aspx?contenttypeid=" + ContentTypeId + "&size=" + drl.SelectedValue);
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void drlList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var drlList = sender as DropDownList;
            if (drlList != null)
            {
                Response.Redirect("form.aspx?contenttypeid=" + drlList.SelectedValue);
            }
        }

        protected void rptList_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                var nameImage = e.Item.FindControl("imgSortByName") as Image;
                var idImage = e.Item.FindControl("imgSortById") as Image;

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
                var drlDisplay = e.Item.FindControl("drlDisplay") as DropDownList;
                for (int i = 0; i < drlDisplay.Items.Count; i++)
                {
                    if (drlDisplay.Items[i].Value.Equals(PageSize.ToString()))
                    {
                        drlDisplay.SelectedIndex = i;
                        break;
                    }
                }

                var pager = e.Item.FindControl("pager") as FPager;
                if (pager != null)
                {
                    pager.PageIndex = PageIndex;
                    pager.PageSize = PageSize;
                    pager.TotalCount = _formProvider.SelectTotalCountByContentTypeId(ContentTypeId, new ErrorInfoList());
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
    }
}