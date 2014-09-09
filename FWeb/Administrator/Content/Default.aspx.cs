using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.FormControl;
using FUIControls.Page;
using FUIControls.UIControl;

namespace FWeb.Administrator.Content
{
    public partial class Default : FAdminPage
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

        private ContentTypeInfo _contentTypeInfo;

        protected override void Init()
        {
            base.Init();
            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();

            _contentTypeInfo = _contentTypeProvider.Select(ContentTypeId, ErrorList);

            RenderContentsList();

            if (_contentTypeInfo != null)
            {
                FieldInfo[] showInListingFields =
                    FieldInfo.GetFieldArray(_contentTypeInfo.FieldsXml).Where(x => x.CreatedBy != "system" && x.ShowInListing).OrderBy(x => x.FieldType == FormFieldType.LabelControl).ThenBy(x => x.FieldType == FormFieldType.Unknown).ToArray();

                object[,] param = new object[2, 3];
                param[0, 0] = "@PageIndex";
                param[0, 1] = PageIndex;
                param[1, 0] = "@PageSize";
                param[1, 1] = PageSize;
                rptList.HeaderTemplate = new ContentItemTemplate(showInListingFields, ListItemType.Header, ContentTypeId);
                rptList.ItemTemplate = new ContentItemTemplate(showInListingFields, ListItemType.Item, ContentTypeId);
                rptList.AlternatingItemTemplate = new ContentItemTemplate(showInListingFields, ListItemType.AlternatingItem, ContentTypeId);
                rptList.QueryParameters = param;
                rptList.QueryName = _contentTypeInfo.TableName + ".select_paging";
                rptList.DataBind();
            }
        }

        protected override void ParsePost()
        {
            string action = Request.Form["form-action"];
            string checkboxs = Request.Form["chbxRow"];
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("add"))
                {
                    Response.Redirect("/administrator/content/action.aspx?type=entry&contenttypeid=" + ContentTypeId);
                }
                else if (action.Equals("edit"))
                {

                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        Response.Redirect("/administrator/content/action.aspx?type=entry&contenttypeid=" + ContentTypeId +
                                          "&id=" + temps[0]);
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

        protected void DrlDisplaySelectedIndexChanged(object sender, EventArgs e)
        {
            var drl = sender as DropDownList;
            Response.Redirect("default.aspx?contenttypeid=" + ContentTypeId + "&size=" + drl.SelectedValue);
        }

        protected void rptList_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
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

                FPager pager = e.Item.FindControl("pager") as FPager;
                if (pager != null)
                {
                    GeneralConnection generalConnection = new GeneralConnection();
                    DataTable dataTable = generalConnection.ExecuteDataTableQuery(_contentTypeInfo.TableName + ".select_total_count", null, new ErrorInfoList());
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        pager.PageIndex = PageIndex;
                        pager.PageSize = PageSize;
                        pager.TotalCount = ValidationHelper.GetInteger(dataTable.Rows[0][0], 0);
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

        protected void lnkTableName_OnClick(object sender, EventArgs e)
        {
            if (SortBy == "TableName")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "TableName";
                SortOrder = "ASC";
            }

            FillGrid();
        }

        private void PublishRows(IEnumerable<string> ids, bool publish)
        {
            var generalConnection = new GeneralConnection();
            foreach (string item in ids)
            {
                var model = new ContentTypeModel(ContentTypeId, ValidationHelper.GetInteger(item, 0), generalConnection, _contentTypeProvider, ErrorList);
                model.IsPublished = publish;
                model.Update();
            }
            if (CheckErrors())
                Response.Redirect("/administrator/content/default.aspx?contenttypeid=" + ContentTypeId);
        }

        private void DeleteRows(string[] ids)
        {
            var generalConnection = new GeneralConnection();
            foreach (string item in ids)
            {
                var model = new ContentTypeModel(ContentTypeId, ValidationHelper.GetInteger(item, 0),
                                                 generalConnection, _contentTypeProvider, ErrorList);
                model.Delete();
            }
            if (CheckErrors())
                Response.Redirect("/administrator/content/default.aspx?contenttypeid=" + ContentTypeId);
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
            builder.Append("<li>Content Type successfully saved</li>");
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
        }

        private void RenderContentsList()
        {
            List<ContentTypeInfo> contentTypeInfos = _contentTypeProvider.SelectAll(ErrorList);
            var htmlBuilder = new StringBuilder();

            foreach (ContentTypeInfo item in contentTypeInfos)
            {
                if (item.Id.Equals(ContentTypeId))
                    htmlBuilder.AppendFormat(
                        "<li><a href=\"default.aspx?contenttypeid={0}\" class=\"active\">{1}</a> </li>",
                        item.Id, item.Name);
                else
                {
                    htmlBuilder.AppendFormat(
                        "<li><a href=\"default.aspx?contenttypeid={0}\" >{1}</a> </li>", item.Id,
                        item.Name);
                }
            }
            ltlContents.Text = htmlBuilder.ToString();
        }
    }
}