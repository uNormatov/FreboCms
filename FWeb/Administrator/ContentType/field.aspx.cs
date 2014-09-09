using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;

namespace FWeb.Administrator.ContentType
{
    public partial class field : FAdminPage
    {
        private ContentTypeProvider _contentTypeProvider;
        
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

        protected override string SortOrder
        {
            get
            {
                if (ViewState["_sortOrder"] == null)
                    return "ASC";
                return ViewState["_sortOrder"].ToString();
            }
            set
            {
                ViewState["_sortOrder"] = value;
            }
        }

        protected override void Init()
        {
            base.Init();
            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();
        }

        protected override void FillGrid()
        {
            List<ContentTypeInfo> listInfos = _contentTypeProvider.SelectAll(ErrorList);
            drlList.DataSource = listInfos;
            drlList.DataBind();
            drlList.SelectedValue = ContentTypeId.ToString();

            ContentTypeInfo contentTypeInfo = _contentTypeProvider.Select(ContentTypeId, ErrorList);
            if (contentTypeInfo != null)
            {
                FieldInfo[] fieldInfos = FieldInfo.GetFieldArray(contentTypeInfo.FieldsXml).Where(x => x.CreatedBy != "system").ToArray();
                List<FieldInfo> list;
                if (SortOrder == "ASC")
                {
                    if (SortBy == "Name")
                    {
                        list = fieldInfos.OrderBy(x => x.Name).ToList();
                    }
                    else if (SortBy == "DiplayName")
                    {
                        list = fieldInfos.OrderBy(x => x.DisplayName).ToList();
                    }
                    else if (SortBy == "UIType")
                    {
                        list = fieldInfos.OrderBy(x => x.FieldTypeName).ToList();
                    }
                    else if (SortBy == "DataType")
                    {
                        list = fieldInfos.OrderBy(x => x.DataTypeName).ToList();
                    }
                    else
                    {
                        list = fieldInfos.OrderBy(x => x.SortOrder).ToList();
                    }
                }
                else
                {
                    if (SortBy == "Name")
                    {
                        list = fieldInfos.OrderByDescending(x => x.Name).ToList();
                    }
                    else if (SortBy == "DiplayName")
                    {
                        list = fieldInfos.OrderByDescending(x => x.DisplayName).ToList();
                    }
                    else if (SortBy == "UIType")
                    {
                        list = fieldInfos.OrderByDescending(x => x.FieldTypeName).ToList();
                    }
                    else if (SortBy == "DataType")
                    {
                        list = fieldInfos.OrderByDescending(x => x.DataTypeName).ToList();
                    }
                    else
                    {
                        list = fieldInfos.OrderByDescending(x => x.SortOrder).ToList();
                    }
                }
                rptList.DataSource = list;
                rptList.DataBind();
            }
        }

        protected override void ParsePost()
        {
            string action = Request.Form["form-action"];
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("add"))
                {
                    Response.Redirect("/administrator/contenttype/fieldaction.aspx?type=entry&contenttypeid=" +
                                      drlList.SelectedValue);
                }
                else if (action.Equals("edit"))
                {
                    string checkboxs = Request.Form["chbxRow"];
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        Response.Redirect("/administrator/contenttype/fieldaction.aspx?type=entry&id=" + temps[0] + "&contenttypeid=" + drlList.SelectedValue);
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
            var drl = sender as DropDownList;
            Response.Redirect("/default.aspx?size=" + drl.SelectedValue);
            FillGrid();
        }

        protected void RptListItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                var nameImage = e.Item.FindControl("imgSortByName") as Image;
                var displayImage = e.Item.FindControl("imgDisplayName") as Image;
                Image uiTypeImage = e.Item.FindControl("imgUIType") as Image;
                Image dataTypeImage = e.Item.FindControl("imgDataType") as Image;
                Image orderImage = e.Item.FindControl("imgOrder") as Image;

                string image = "/content/css/images/menu/sort-asc.png";
                if (SortOrder == "DESC")
                    image = "/content/css/images/menu/sort-desc.png";

                if (SortBy == "Name")
                {
                    nameImage.ImageUrl = image;
                    displayImage.ImageUrl = "";
                    uiTypeImage.ImageUrl = "";
                    dataTypeImage.ImageUrl = "";
                    orderImage.ImageUrl = "";
                }
                else if (SortBy == "DisplayName")
                {
                    displayImage.ImageUrl = image;
                    nameImage.ImageUrl = "";
                    uiTypeImage.ImageUrl = "";
                    dataTypeImage.ImageUrl = "";
                    orderImage.ImageUrl = "";
                }
                else if (SortBy == "UIType")
                {
                    displayImage.ImageUrl = "";
                    nameImage.ImageUrl = "";
                    uiTypeImage.ImageUrl = image;
                    dataTypeImage.ImageUrl = "";
                    orderImage.ImageUrl = "";
                }
                else if (SortBy == "DataType")
                {
                    displayImage.ImageUrl = "";
                    nameImage.ImageUrl = "";
                    uiTypeImage.ImageUrl = "";
                    dataTypeImage.ImageUrl = image;
                    orderImage.ImageUrl = "";
                }
                else if (SortBy == "SortOrder")
                {
                    displayImage.ImageUrl = "";
                    nameImage.ImageUrl = "";
                    uiTypeImage.ImageUrl = "";
                    dataTypeImage.ImageUrl = "";
                    orderImage.ImageUrl = image;
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
            }
        }

        protected void btnSearch_onClick(object sender, EventArgs e)
        {
            var txtSearch = sender as TextBox;
            SearchKey = txtSearch.Text;
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

        protected void lnkDisplayName_OnClick(object sender, EventArgs e)
        {
            if (SortBy == "DisplayName")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "DisplayName";
                SortOrder = "ASC";
            }
            FillGrid();

        }

        protected void lnkUiType_OnClick(object sender, EventArgs e)
        {

            if (SortBy == "UIType")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "UIType";
                SortOrder = "ASC";
            }
            FillGrid();

        }

        protected void lnlDataType_OnClick(object sender, EventArgs e)
        {
            if (SortBy == "DataType")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "DataType";
                SortOrder = "ASC";
            }
            FillGrid();
        }

        protected void lnkOrder_OnClick(object sender, EventArgs e)
        {

            if (SortBy == "SortOrder")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "SortOrder";
                SortOrder = "ASC";
            }
            FillGrid();
        }

        protected void drlList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var drlList = sender as DropDownList;
            Response.Redirect("field.aspx?contenttypeid=" + drlList.SelectedValue);
        }

        private void DeleteRows(IEnumerable<string> ids)
        {
            foreach (string item in ids)
            {
                ContentTypeInfo contentTypeInfo = _contentTypeProvider.Select(ContentTypeId, ErrorList);
                if (contentTypeInfo != null)
                {
                    List<FieldInfo> fieldInfos = FieldInfo.GetFieldArray(contentTypeInfo.FieldsXml).ToList();
                    FieldInfo fieldInfo = fieldInfos.FirstOrDefault(x => x.Name == item);
                    fieldInfos.Remove(fieldInfo);
                    contentTypeInfo.FieldsXml = FieldInfo.GetFieldXml(fieldInfos.ToArray());
                    _contentTypeProvider.UpdateField(contentTypeInfo, fieldInfo, FieldActionMode.Delete, FormHelper.IsComponent(fieldInfo), ErrorList);
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
            builder.Append("<li>Field successfully saved</li>");
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
        }
    }
}