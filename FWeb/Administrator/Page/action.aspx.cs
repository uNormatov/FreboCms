using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;
using FUIControls.PortalControl;
using FUIControls.Settings;

namespace FWeb.Administrator.Page
{
    public partial class action : FAdminEditPage
    {
        private PageProvider _pageProvider;
        private LayoutProvider _layoutProvider;
        private SiteProvider _siteProvider;

        protected override void Init()
        {
            base.Init();

            if (_pageProvider == null)
                _pageProvider = new PageProvider();
            if (_layoutProvider == null)
                _layoutProvider = new LayoutProvider();
            if (_siteProvider == null)
                _siteProvider = new SiteProvider();

            if (IsEdit)
            {
                Title = "Edit Page | Frebo Cms";
                ltlTitle.Text = "Edit";
            }
            else
            {
                Title = "New Page | Frebo Cms";
                ltlTitle.Text = "New";
            }
            FillPages();
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/page/default.aspx";
            RedrictUrl = CancelUrl;
        }

        protected override void FillFields()
        {
            List<LayoutInfo> siteLayouts = _layoutProvider.SelectAllByType(true, ErrorList);
            drlSiteLayout.DataSource = siteLayouts;
            drlSiteLayout.DataBind();

            List<LayoutInfo> pageLayouts = _layoutProvider.SelectAllByType(false, ErrorList);
            drlPageLayout.DataSource = pageLayouts;
            drlPageLayout.DataBind();

            List<string> roles = new List<string>();
            roles.Add("All");
            roles.AddRange(Roles.GetAllRoles().ToList());
            chbxListRoles.DataSource = roles;
            chbxListRoles.DataBind();


            if (IsEdit && CheckErrors())
            {
                PageInfo pageInfo = _pageProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
                txtName.Text = pageInfo.Name;
                txtTitle.Text = pageInfo.Title;
                txtDescription.Text = pageInfo.Description;
                txtSeoTemplate.Text = pageInfo.SeoTemplate;
                txtBreadCrumb.Text = pageInfo.BreadCrumbTitle;
                txtMetaDescription.Text = pageInfo.MetadataDescription;
                txtMetaKeywords.Text = pageInfo.MetadataKeywords;
                txtContentRights.Text = pageInfo.ContentRights;
                drlSiteLayout.SelectedValue = pageInfo.SiteLayoutId.ToString();
                drlPageLayout.SelectedValue = pageInfo.PageLayoutId.ToString();

                List<String> selectedRoles = _pageProvider.SelectPagesInRolesByPageId(pageInfo.Id, ErrorList);

                foreach (ListItem item in chbxListRoles.Items)
                {
                    if (selectedRoles != null && selectedRoles.Any(x => x.Equals(item.Value)))
                        item.Selected = true;
                }

                for (int i = 0; i < drlPages.Items.Count; i++)
                {
                    if (drlPages.Items[i].Value == pageInfo.ParentId.ToString())
                        drlPages.SelectedIndex = i;

                    if (drlNoAuthenticatedPage.Items[i].Value == pageInfo.RedirectNoAuthenticated)
                        drlNoAuthenticatedPage.SelectedIndex = i;

                    if (drlNoPermission.Items[i].Value == pageInfo.RedirectNoPermission)
                        drlNoPermission.SelectedIndex = i;
                }
                SiteInfo siteInfo = _siteProvider.Select(0, ErrorList);
                chbxIsDefault.Checked = siteInfo != null && siteInfo.DefaultPageId == pageInfo.Id;
                txtQueryName.Text = pageInfo.MetaQueryName;
                ltlQuerySelector.Text = string.Format("<a class=\"queryselectors ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/queryselector.aspx?name={0}&controlid={1}\"><span class=\"ui-button-text\">select</span></a>", txtQueryName.Text, txtQueryName.ClientID);
                if (!string.IsNullOrEmpty(txtQueryName.Text))
                    ltlQuerySelector.Text += string.Format("<a target=\"_blank\" class=\"queryeditor ui-button ui-state-default ui-button-text-only\" href=\"/administrator/pages/queryeditor.aspx?id={0}\"><span class=\"ui-button-text\">edit</span></a>", txtQueryName.Text);


                string queryParamaters = pageInfo.MetaQueryParameters;
                if (FormHelper.GetParamatersCount(queryParamaters) == 0)
                    Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "addfields", "<script>addField();</script>");
                else
                {
                    ltlQueryParams.Text = FormHelper.GetDataViewerParametersTable(queryParamaters);
                }

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "addfields", "<script>addField();</script>");
            }

        }

        private void FillPages()
        {
            List<PageInfo> pages = _pageProvider.SelectAll(ErrorList);
            if (CheckErrors())
            {
                int depth = 0;
                drlPages.Items.Add(new ListItem("Select", "0", true));
                drlNoAuthenticatedPage.Items.Add(new ListItem("Select", "0", true));
                drlNoPermission.Items.Add(new ListItem("Select", "0", true));
                if (pages != null && pages.Count > 0)
                    FillChildPages(pages, 0, depth + 1);
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
                if (pageInfo.Id != ValidationHelper.GetInteger(Id, 0))
                    drlPages.Items.Add(new ListItem(string.Format(" - {0}{1}", childRow, pageInfo.Name), pageInfo.Id.ToString()));
                drlNoAuthenticatedPage.Items.Add(new ListItem(string.Format(" - {0}{1}", childRow, pageInfo.Name), pageInfo.SeoTemplate));
                drlNoPermission.Items.Add(new ListItem(string.Format(" - {0}{1}", childRow, pageInfo.Name), pageInfo.SeoTemplate));
                FillChildPages(pages, pageInfo.Id, depth + 1);
            }
        }

        protected override bool Update()
        {
            PageInfo info = _pageProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
            if (info != null)
            {
                info.Name = txtName.Text;
                info.Title = txtTitle.Text;
                info.SeoTemplate = txtSeoTemplate.Text;
                info.BreadCrumbTitle = txtBreadCrumb.Text;
                info.Description = txtDescription.Text;
                info.ParentId = ValidationHelper.GetInteger(drlPages.SelectedValue, 0);
                info.PageLayoutId = ValidationHelper.GetInteger(drlPageLayout.SelectedValue, 0);
                info.SiteLayoutId = ValidationHelper.GetInteger(drlSiteLayout.SelectedValue, 0);
                info.RedirectNoAuthenticated = drlNoAuthenticatedPage.SelectedValue;
                info.RedirectNoPermission = drlNoPermission.SelectedValue;
                info.MetadataDescription = txtMetaDescription.Text;
                info.MetadataKeywords = txtMetaKeywords.Text;
                info.MetaQueryName = txtQueryName.Text;
                Dictionary<string, string> queryParams = GetControlValues("parameter");
                string queryParamsBuilder = FormHelper.GetDataViewerParametersXml(queryParams);
                info.MetaQueryParameters = queryParamsBuilder;
                info.ContentRights = txtContentRights.Text;
                UpdateSiteDefaultPage(info.Id);
                bool result = _pageProvider.Update(info, ErrorList);
                if (result)
                {
                    _pageProvider.DeletePagesInRoles(info.Id, ErrorList);
                    foreach (ListItem item in chbxListRoles.Items)
                    {
                        if (item.Selected)
                        {
                            PagesInRolesInfo pagesInRolesInfo = new PagesInRolesInfo();
                            pagesInRolesInfo.PageId = info.Id;
                            pagesInRolesInfo.RoleId = item.Value;
                            _pageProvider.CreatePagesInRoles(pagesInRolesInfo, ErrorList);
                        }
                    }
                }
                CacheHelper.ClearCaches();
            }
            return CheckErrors();
        }

        protected override bool Insert()
        {
            PageInfo info = new PageInfo();
            info.Name = txtName.Text;
            info.Title = txtTitle.Text;
            info.SeoTemplate = txtSeoTemplate.Text;
            info.BreadCrumbTitle = txtBreadCrumb.Text;
            info.Description = txtDescription.Text;
            info.ParentId = ValidationHelper.GetInteger(drlPages.SelectedValue, 0);
            info.PageLayoutId = ValidationHelper.GetInteger(drlPageLayout.SelectedValue, 0);
            info.SiteLayoutId = ValidationHelper.GetInteger(drlSiteLayout.SelectedValue, 0);
            info.RedirectNoAuthenticated = drlNoAuthenticatedPage.SelectedValue;
            info.RedirectNoPermission = drlNoPermission.SelectedValue;
            info.MetadataDescription = txtMetaDescription.Text;
            info.MetadataKeywords = txtMetaKeywords.Text;
            info.MetaQueryName = txtQueryName.Text;
            info.ContentRights = txtContentRights.Text;
            Dictionary<string, string> queryParams = GetControlValues("parameter");
            string queryParamsBuilder = FormHelper.GetDataViewerParametersXml(queryParams);
            if (!string.IsNullOrEmpty(queryParamsBuilder))
                info.MetaQueryParameters = queryParamsBuilder;
            else info.MetaQueryParameters = string.Empty;
            object id = _pageProvider.Create(info, ErrorList);
            if (id != null)
                UpdateSiteDefaultPage(ValidationHelper.GetInteger(id, 0));
            if (id != null)
            {
                foreach (ListItem item in chbxListRoles.Items)
                {
                    if (item.Selected)
                    {
                        PagesInRolesInfo pagesInRolesInfo = new PagesInRolesInfo();
                        pagesInRolesInfo.PageId = ValidationHelper.GetInteger(id, 0);
                        pagesInRolesInfo.RoleId = item.Value;
                        _pageProvider.CreatePagesInRoles(pagesInRolesInfo, ErrorList);
                    }
                }
            }
            CacheHelper.ClearCaches();
            return CheckErrors();
        }

        private void UpdateSiteDefaultPage(int pageId)
        {
            if (chbxIsDefault.Checked)
            {
                SiteInfo siteInfo = _siteProvider.Select(0, ErrorList);
                if (siteInfo == null)
                {
                    siteInfo = new SiteInfo();
                    siteInfo.DefaultPageId = pageId;
                    siteInfo.Name = "";
                    _siteProvider.Create(siteInfo, ErrorList);
                }
                else
                {
                    siteInfo.DefaultPageId = pageId;
                    _siteProvider.Update(siteInfo, ErrorList);
                }
                CoreSettings.CurrentSite = siteInfo;
            }
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
    }
}
