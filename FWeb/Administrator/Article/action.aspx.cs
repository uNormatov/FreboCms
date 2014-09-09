using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Helper;
using FDataProvider;
using FUIControls.Context;
using FUIControls.Page;
using FUIControls.Settings;

namespace FWeb.Administrator.Article
{
    public partial class action : FAdminEditPage
    {
        private ArticleProvider _articleProvider;
        private LocalizationProvider _localizationProvider;
        private LayoutProvider _layoutProvider;
        private PageProvider _pageProvider;
        private BlockProvider _blockProvider;

        protected override void Init()
        {
            base.Init();

            if (_articleProvider == null)
                _articleProvider = new ArticleProvider();
            if (_localizationProvider == null)
                _localizationProvider = new LocalizationProvider();
            if (_layoutProvider == null)
                _layoutProvider = new LayoutProvider();
            if (_pageProvider == null)
                _pageProvider = new PageProvider();
            if (_blockProvider == null)
                _blockProvider = new BlockProvider();

            if (IsEdit)
            {
                Title = "Edit Article | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "Edit Article";
            }
            else
            {
                Title = "New Article | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "New Article";
            }
            FillLanguages();
            FillPages();
            FillLayouts();
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/article/default.aspx";
            RedrictUrl = CancelUrl;
        }

        private void FillLanguages()
        {
            List<LanguageInfo> languages = _localizationProvider.SelectAll(ErrorList);
            if (CheckErrors())
            {
                drlLanguages.Items.Add(new ListItem("All", "all", true));
                if (languages != null && languages.Count > 0)
                {
                    foreach (LanguageInfo item in languages)
                    {
                        drlLanguages.Items.Add(new ListItem(item.Name, item.Code));
                    }
                }
            }
        }

        private void FillPages()
        {
            List<PageInfo> pages = _pageProvider.SelectAll(ErrorList);
            if (CheckErrors())
            {
                int depth = 0;
                drlPages.Items.Add(new ListItem("Select", "0", true));
                if (pages != null && pages.Count > 0)
                    FillChildPages(pages, 0, depth);
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
                drlPages.Items.Add(new ListItem(string.Format(" - {0}{1}", childRow, pageInfo.Name), pageInfo.Id.ToString()));
                FillChildPages(pages, pageInfo.Id, depth + 1);
            }
        }

        private void FillLayouts()
        {
            List<LayoutInfo> siteLayouts = _layoutProvider.SelectAllByType(true, ErrorList);
            List<LayoutInfo> pageLayouts = _layoutProvider.SelectAllByType(false, ErrorList);
            if (CheckErrors())
            {
                drlPageLayouts.Items.Add(new ListItem("Select", "0", true));
                drlSiteLayouts.Items.Add(new ListItem("Select", "0", true));
                if (pageLayouts.Count > 0)
                {
                    foreach (LayoutInfo item in pageLayouts)
                    {
                        drlPageLayouts.Items.Add(new ListItem(item.Name, item.Id.ToString()));
                    }
                }
                if (siteLayouts.Count > 0)
                {
                    foreach (LayoutInfo item in siteLayouts)
                    {
                        drlSiteLayouts.Items.Add(new ListItem(item.Name, item.Id.ToString()));
                    }
                }
            }
        }

        protected override void FillFields()
        {
            if (IsEdit)
            {
                ArticleInfo articleInfo = _articleProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
                if (articleInfo != null && CheckErrors())
                {
                    txtTitle.Text = articleInfo.Title;
                    txtCode.Text = articleInfo.Code;
                    fckEditor.Value = articleInfo.Text.ToHtmlDecode();
                    for (int i = 0; i < drlLanguages.Items.Count; i++)
                    {
                        if (drlLanguages.Items[i].Value == articleInfo.Language)
                        {
                            drlLanguages.SelectedIndex = i;
                            break;
                        }
                    }
                    for (int i = 0; i < drlPages.Items.Count; i++)
                    {
                        if (drlPages.Items[i].Value == articleInfo.PageId.ToString())
                        {
                            drlPages.SelectedIndex = i;
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(articleInfo.PageZone))
                        ClientScript.RegisterStartupScript(typeof(action), "fillPages", "<script>fillWebPartZone(0,'" + articleInfo.PageZone + "')</script>");
                    drlPageOrder.SelectedValue = articleInfo.PageOrder.ToString();

                    for (int i = 0; i < drlPageLayouts.Items.Count; i++)
                    {
                        if (drlPageLayouts.Items[i].Value == articleInfo.PageLayoutId.ToString())
                        {
                            drlPageLayouts.SelectedIndex = i;
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(articleInfo.PageLayoutZone))
                        ClientScript.RegisterStartupScript(typeof(action), "fillPages", "<script>fillWebPartZone(1,'" + articleInfo.PageLayoutZone + "')</script>");
                    drlPageLayoutOrder.SelectedValue = articleInfo.PageLayoutOrder.ToString();
                    for (int i = 0; i < drlSiteLayouts.Items.Count; i++)
                    {
                        if (drlSiteLayouts.Items[i].Value == articleInfo.SiteLayoutId.ToString())
                        {
                            drlSiteLayouts.SelectedIndex = i;
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(articleInfo.SiteLayoutZone))
                        ClientScript.RegisterStartupScript(typeof(action), "fillPages", "<script>fillWebPartZone(2,'" + articleInfo.SiteLayoutZone + "')</script>");
                    drlSiteLayoutOrder.SelectedValue = articleInfo.SiteLayoutOrder.ToString();
                }
            }
        }

        protected override bool Update()
        {
            ArticleInfo articleInfo = _articleProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
            if (articleInfo != null)
            {
                articleInfo.Title = txtTitle.Text;
                articleInfo.Code = txtCode.Text;
                articleInfo.Text = fckEditor.Value.ToHtmlEncode();
                articleInfo.Language = drlLanguages.SelectedValue;
                articleInfo.PageId = ValidationHelper.GetInteger(drlPages.SelectedValue, 0);
                articleInfo.PageZone = GetControlValue(drlPageZones.ID);
                articleInfo.PageLayoutId = ValidationHelper.GetInteger(drlPageLayouts.SelectedValue, 0);
                articleInfo.PageLayoutZone = drlPageLayoutZones.SelectedValue;
                articleInfo.SiteLayoutId = ValidationHelper.GetInteger(drlSiteLayouts.SelectedValue, 0);
                articleInfo.SiteLayoutZone = drlSiteLayoutZones.SelectedValue;
                articleInfo.ModifiedBy = CoreSettings.CurrentUserName;
                articleInfo.ModifiedDate = DateTime.Now;
                int blockId = 0;
                if (CheckErrors())
                {
                    PageNBlockProvider pageNBlockProvider = new PageNBlockProvider();
                    if (articleInfo.PageId == 0 && articleInfo.PageNBlockId != 0)
                    {
                        pageNBlockProvider.Delete(articleInfo.PageNBlockId, ErrorList);
                    }
                    else
                    {
                        PageNBlockInfo pageNBlockInfo = pageNBlockProvider.Select(articleInfo.PageNBlockId, ErrorList);
                        if (pageNBlockInfo != null)
                        {
                            pageNBlockInfo.Language = articleInfo.Language;
                            pageNBlockInfo.PageId = ValidationHelper.GetInteger(drlPages.SelectedValue, 0);
                            pageNBlockInfo.WebPartZoneName = GetControlValue(drlPageZones.ID);
                            pageNBlockInfo.Order = ValidationHelper.GetInteger(drlPageOrder.SelectedValue, 0);
                            pageNBlockProvider.Update(pageNBlockInfo, ErrorList);
                        }
                        else
                        {
                            if (drlPages.SelectedValue != "0" && !string.IsNullOrEmpty(GetControlValue(drlPageZones.ID)))
                            {
                                pageNBlockInfo = new PageNBlockInfo();
                                pageNBlockInfo.BlockId = articleInfo.BlockId;
                                pageNBlockInfo.Language = articleInfo.Language;
                                pageNBlockInfo.PageId = ValidationHelper.GetInteger(drlPages.SelectedValue, 0);
                                pageNBlockInfo.WebPartZoneName = GetControlValue(drlPageZones.ID);
                                pageNBlockInfo.Order = ValidationHelper.GetInteger(drlPageOrder.SelectedValue, 0);
                                object pagenBlockid = pageNBlockProvider.Create(pageNBlockInfo, ErrorList);
                                articleInfo.PageNBlockId = ValidationHelper.GetInteger(pagenBlockid, 0);
                                articleInfo.PageZone = pageNBlockInfo.WebPartZoneName;
                            }
                        }
                    }

                    LayoutNBlockProvider layoutNBlockProvider = new LayoutNBlockProvider();
                    if (articleInfo.PageLayoutId == 0 && articleInfo.PageLayoutNBlockId != 0)
                    {
                        layoutNBlockProvider.Delete(articleInfo.PageLayoutNBlockId, ErrorList);
                    }
                    else
                    {
                        LayoutNBlockInfo layoutNBlockInfo = layoutNBlockProvider.Select(articleInfo.PageLayoutNBlockId, ErrorList);
                        if (layoutNBlockInfo != null)
                        {
                            layoutNBlockInfo.Language = articleInfo.Language;
                            layoutNBlockInfo.LayoutId = ValidationHelper.GetInteger(drlPageLayouts.SelectedValue, 0);
                            layoutNBlockInfo.WebPartZoneName = GetControlValue(drlPageLayoutZones.ID);
                            layoutNBlockInfo.Order = ValidationHelper.GetInteger(drlPageLayoutOrder.SelectedValue, 0);
                            articleInfo.PageLayoutZone = layoutNBlockInfo.WebPartZoneName;
                            layoutNBlockProvider.Update(layoutNBlockInfo, ErrorList);
                        }
                        else
                        {
                            if (drlPageLayouts.SelectedValue != "0" && !string.IsNullOrEmpty(GetControlValue(drlPageLayoutZones.ID)))
                            {
                                layoutNBlockInfo = new LayoutNBlockInfo();
                                layoutNBlockInfo.BlockId = articleInfo.BlockId;
                                layoutNBlockInfo.Language = articleInfo.Language;
                                layoutNBlockInfo.LayoutId = ValidationHelper.GetInteger(drlPageLayouts.SelectedValue, 0);
                                layoutNBlockInfo.WebPartZoneName = GetControlValue(drlPageLayoutZones.ID);
                                layoutNBlockInfo.Order = ValidationHelper.GetInteger(drlPageLayoutOrder.SelectedValue, 0);
                                object result = layoutNBlockProvider.Create(layoutNBlockInfo, ErrorList);
                                articleInfo.PageLayoutZone = layoutNBlockInfo.WebPartZoneName;
                                articleInfo.PageLayoutNBlockId = ValidationHelper.GetInteger(result, 0);
                            }
                        }
                    }

                    if (articleInfo.SiteLayoutId == 0 && articleInfo.SiteLayoutNBlockId != 0)
                    {
                        layoutNBlockProvider.Delete(articleInfo.SiteLayoutNBlockId, ErrorList);
                    }
                    else
                    {
                        LayoutNBlockInfo layoutNBlockInfo = layoutNBlockProvider.Select(articleInfo.SiteLayoutNBlockId, ErrorList);
                        if (layoutNBlockInfo != null)
                        {
                            layoutNBlockInfo.Language = articleInfo.Language;
                            layoutNBlockInfo.LayoutId = ValidationHelper.GetInteger(drlSiteLayouts.SelectedValue, 0);
                            layoutNBlockInfo.WebPartZoneName = GetControlValue(drlSiteLayoutZones.ID);
                            layoutNBlockInfo.Order = ValidationHelper.GetInteger(drlSiteLayoutOrder.SelectedValue, 0);
                            layoutNBlockProvider.Update(layoutNBlockInfo, ErrorList);
                        }
                        else
                        {
                            if (drlSiteLayouts.SelectedValue != "0" && !string.IsNullOrEmpty(GetControlValue(drlSiteLayoutZones.ID)))
                            {
                                layoutNBlockInfo = new LayoutNBlockInfo();
                                layoutNBlockInfo.BlockId = articleInfo.BlockId;
                                layoutNBlockInfo.Language = articleInfo.Language;
                                layoutNBlockInfo.LayoutId = ValidationHelper.GetInteger(drlSiteLayouts.SelectedValue, 0);
                                layoutNBlockInfo.WebPartZoneName = GetControlValue(drlSiteLayoutZones.ID);
                                layoutNBlockInfo.Order = ValidationHelper.GetInteger(drlSiteLayoutOrder.SelectedValue, 0);
                                object result = layoutNBlockProvider.Create(layoutNBlockInfo, ErrorList);
                                articleInfo.SiteLayoutNBlockId = ValidationHelper.GetInteger(result, 0);
                                articleInfo.SiteLayoutZone = layoutNBlockInfo.WebPartZoneName;
                            }
                        }
                    }

                }
                _articleProvider.Update(articleInfo, ErrorList);
            }
            CacheHelper.ClearCaches();
            return CheckErrors();
        }

        protected override bool Insert()
        {
            ArticleInfo articleInfo = new ArticleInfo();
            articleInfo.Title = txtTitle.Text;
            articleInfo.Code = txtCode.Text;
            articleInfo.Text = fckEditor.Value.ToHtmlEncode();
            articleInfo.Language = drlLanguages.SelectedValue;
            articleInfo.PageId = ValidationHelper.GetInteger(drlPages.SelectedValue, 0);
            articleInfo.PageZone = GetControlValue(drlPageZones.ID); ;
            articleInfo.PageLayoutId = ValidationHelper.GetInteger(drlPageLayouts.SelectedValue, 0);
            articleInfo.PageLayoutZone = drlPageLayoutZones.SelectedValue;
            articleInfo.SiteLayoutId = ValidationHelper.GetInteger(drlSiteLayouts.SelectedValue, 0);
            articleInfo.SiteLayoutZone = drlSiteLayoutZones.SelectedValue;
            articleInfo.CreatedBy = CoreSettings.CurrentUserName;
            articleInfo.CreatedDate = DateTime.Now;
            articleInfo.ModifiedBy = CoreSettings.CurrentUserName;
            articleInfo.ModifiedDate = DateTime.Now;
            object id = _articleProvider.Create(articleInfo, ErrorList);
            if (CheckErrors())
            {
                BlockInfo blockInfo = new BlockInfo();
                blockInfo.WebPartId = CoreSettings.CurrentSite.ArticleWebpartId;
                blockInfo.Name = string.Format("ARTICLE:{0}", articleInfo.Code);
                blockInfo.Properties = string.Format("<properties><property name=\"ArticleId\">{0}</property></properties>", id);
                object blockId = _blockProvider.Create(blockInfo, ErrorList);
                if (CheckErrors())
                {
                    int pageBlockId = 0;
                    int pageLayoutNBlock = 0;
                    int siteLayoutNBlock = 0;
                    if (drlPages.SelectedValue != "0" && !string.IsNullOrEmpty(GetControlValue(drlPageZones.ID)))
                    {
                        PageNBlockProvider pageNBlockProvider = new PageNBlockProvider();
                        PageNBlockInfo pageNBlockInfo = new PageNBlockInfo();
                        pageNBlockInfo.Language = articleInfo.Language;
                        pageNBlockInfo.BlockId = ValidationHelper.GetInteger(blockId, 0);
                        pageNBlockInfo.PageId = ValidationHelper.GetInteger(drlPages.SelectedValue, 0);
                        pageNBlockInfo.WebPartZoneName = GetControlValue(drlPageZones.ID);
                        pageNBlockInfo.Order = ValidationHelper.GetInteger(drlPageOrder.SelectedValue, 0);
                        pageBlockId = ValidationHelper.GetInteger(pageNBlockProvider.Create(pageNBlockInfo, ErrorList), 0);
                    }

                    if (drlPageLayouts.SelectedValue != "0" && !string.IsNullOrEmpty(GetControlValue(drlPageLayoutZones.ID)))
                    {
                        LayoutNBlockProvider layoutNBlockProvider = new LayoutNBlockProvider();
                        LayoutNBlockInfo layoutNBlockInfo = new LayoutNBlockInfo();
                        layoutNBlockInfo.Language = articleInfo.Language;
                        layoutNBlockInfo.BlockId = ValidationHelper.GetInteger(blockId, 0);
                        layoutNBlockInfo.LayoutId = ValidationHelper.GetInteger(drlPageLayouts.SelectedValue, 0);
                        layoutNBlockInfo.WebPartZoneName = GetControlValue(drlPageLayoutZones.ID);
                        layoutNBlockInfo.Order = ValidationHelper.GetInteger(drlPageLayoutOrder.SelectedValue, 0);
                        pageLayoutNBlock = ValidationHelper.GetInteger(layoutNBlockProvider.Create(layoutNBlockInfo, ErrorList), 0);
                    }

                    if (drlSiteLayouts.SelectedValue != "0" && !string.IsNullOrEmpty(GetControlValue(drlSiteLayoutZones.ID)))
                    {
                        LayoutNBlockProvider layoutNBlockProvider = new LayoutNBlockProvider();
                        LayoutNBlockInfo layoutNBlockInfo = new LayoutNBlockInfo();
                        layoutNBlockInfo.Language = articleInfo.Language;
                        layoutNBlockInfo.BlockId = ValidationHelper.GetInteger(blockId, 0);
                        layoutNBlockInfo.LayoutId = ValidationHelper.GetInteger(drlSiteLayouts.SelectedValue, 0);
                        layoutNBlockInfo.WebPartZoneName = GetControlValue(drlSiteLayoutZones.ID);
                        layoutNBlockInfo.Order = ValidationHelper.GetInteger(drlSiteLayoutOrder.SelectedValue, 0);
                        siteLayoutNBlock = ValidationHelper.GetInteger(layoutNBlockProvider.Create(layoutNBlockInfo, ErrorList), 0);
                    }

                    articleInfo.Id = ValidationHelper.GetInteger(id, 0);
                    articleInfo.PageNBlockId = pageBlockId;
                    articleInfo.PageLayoutNBlockId = pageLayoutNBlock;
                    articleInfo.SiteLayoutNBlockId = siteLayoutNBlock;
                    articleInfo.BlockId = ValidationHelper.GetInteger(blockId, 0);
                    _articleProvider.Update(articleInfo, ErrorList);
                }
            }
            CacheHelper.ClearCaches();
            return CheckErrors();
        }

        protected override void ValidateForm()
        {
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                ErrorList.Add(new ErrorInfo()
                {

                    Name = "Article details",
                    Message = "Name is required"
                });
            }

            if (string.IsNullOrEmpty(txtCode.Text))
            {
                ErrorList.Add(new ErrorInfo()
                {

                    Name = "Article details",
                    Message = "Code is required"
                });
            }

            if (string.IsNullOrEmpty(fckEditor.Value))
            {
                ErrorList.Add(new ErrorInfo()
                {

                    Name = "Article details",
                    Message = "Text is required"
                });
            }

            if ((drlPages.SelectedValue == "0" || string.IsNullOrEmpty(GetControlValue(drlPageZones.ID)))
              && (drlPageLayouts.SelectedValue == "0" || string.IsNullOrEmpty(GetControlValue(drlPageLayoutZones.ID))) &&
               (drlSiteLayouts.SelectedValue == "0" || string.IsNullOrEmpty(GetControlValue(drlSiteLayouts.ID))))
            {
                ErrorList.Add(new ErrorInfo()
                                  {

                                      Name = "Article Positions",
                                      Message = "Select at least one position"
                                  });
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
            ErrorList.Clear();
        }

        protected override void PrintSuccess()
        {
        }
    }
}