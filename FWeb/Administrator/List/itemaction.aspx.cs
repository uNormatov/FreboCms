using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Helper;
using FUIControls.Page;
using FDataProvider;
using FUIControls.Settings;

namespace FWeb.Administrator.List
{
    public partial class itemaction : FAdminEditPage
    {
        private ListProvider _listProvider;
        private ListItemProvider _listItemProvider;

        private int ListId
        {
            get
            {
                return ValidationHelper.GetInteger(ViewState["_listId"], 1);
            }
            set { ViewState["_listId"] = value; }
        }

        protected override void Init()
        {
            base.Init();

            if (_listItemProvider == null)
                _listItemProvider = new ListItemProvider();
            if (_listProvider == null)
                _listProvider = new ListProvider();

            if (IsEdit)
            {
                Title = "Edit List Item | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "Edit";
            }
            else
            {
                Title = "New List Item  | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "New";
            }
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/list/items.aspx?listid=" + ListId;
            RedrictUrl = CancelUrl;
        }

        protected override void FillFields()
        {
            List<ListInfo> listInfos = _listProvider.SelectAll(ErrorList);
            drlList.DataSource = listInfos;
            drlList.DataBind();

            int listid = ValidationHelper.GetInteger(Request.QueryString["listid"], -1);
            if (listid != -1)
            {
                drlList.SelectedValue = listid.ToString();
                ListId = listid;
            }

            drlList_OnSelectedIndexChanged(null, null);
            if (IsEdit)
            {
                ListItemInfo listItemInfo = _listItemProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
                if (listItemInfo != null)
                {
                    drlList.SelectedValue = listItemInfo.ListId.ToString();
                    ListId = listItemInfo.ListId;
                    txtName.Text = listItemInfo.Name;
                    txtDescription.Text = listItemInfo.Description;

                    drlParentList.SelectedValue = listItemInfo.ParentId.ToString();
                }
            }
        }

        protected override bool Update()
        {
            ListItemInfo listItemInfo = _listItemProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
            listItemInfo.Name = txtName.Text;
            listItemInfo.Description = txtDescription.Text;
            listItemInfo.ListId = ValidationHelper.GetInteger(drlList.SelectedValue, 0);
            listItemInfo.ParentId = ValidationHelper.GetInteger(drlParentList.SelectedValue, 0);
            listItemInfo.SeoTemplate = SiteHelper.ToUrl(txtName.Text);
            _listItemProvider.Update(listItemInfo, ErrorList);
            return CheckErrors();
        }

        protected override bool Insert()
        {
            ListItemInfo listItemInfo = new ListItemInfo();
            listItemInfo.Name = txtName.Text;
            listItemInfo.Description = txtDescription.Text;
            listItemInfo.ListId = ValidationHelper.GetInteger(drlList.SelectedValue, 0);
            listItemInfo.ParentId = ValidationHelper.GetInteger(drlParentList.SelectedValue, 0);
            listItemInfo.SeoTemplate = SiteHelper.ToUrl(txtName.Text);
            _listItemProvider.Create(listItemInfo, ErrorList);
            return CheckErrors();
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
        }

        protected override void PrintSuccess()
        {
            throw new NotImplementedException();
        }

        protected void drlList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ListId = ValidationHelper.GetInteger(drlList.SelectedValue, 1);
            List<ListItemInfo> listItemInfos = _listItemProvider.SelectAll(ListId, ErrorList);
            drlParentList.DataSource = listItemInfos;
            drlParentList.DataBind();
        }
    }
}