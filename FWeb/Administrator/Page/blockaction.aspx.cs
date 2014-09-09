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
using FUIControls.Page;
using FUIControls.PortalControl;
using FUIControls.Settings;

namespace FWeb.Administrator.Page
{
    public partial class blockaction : FAdminEditPage
    {
        private LayoutProvider _layoutProvider;
        private PageProvider _pageProvider;
        private LayoutWebPartZoneProvider _layoutWebPartZoneProvider;
        private PageNBlockProvider _pageNBlockProvider;
        private WebPartProvider _webPartProvider;
        private BlockProvider _blockProvider;
        private LocalizationProvider _localizationProvider;

        private int PageId
        {
            get
            {
                return ValidationHelper.GetInteger(ViewState["_pageId"], 1);
            }
            set { ViewState["_pageId"] = value; }
        }

        protected override void Init()
        {
            base.Init();

            if (_layoutProvider == null)
                _layoutProvider = new LayoutProvider();
            if (_layoutWebPartZoneProvider == null)
                _layoutWebPartZoneProvider = new LayoutWebPartZoneProvider();
            if (_webPartProvider == null)
                _webPartProvider = new WebPartProvider();
            if (_blockProvider == null)
                _blockProvider = new BlockProvider();
            if (_pageNBlockProvider == null)
                _pageNBlockProvider = new PageNBlockProvider();
            if (_pageProvider == null)
                _pageProvider = new PageProvider();
            if (_localizationProvider == null)
                _localizationProvider = new LocalizationProvider();

            if (IsEdit)
            {
                Title = "Edit Block | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "Edit Block";
            }
            else
            {
                Title = "New Block | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "New Block";
            }
            FillLanguages();
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/page/blocks.aspx?pageid=" + PageId;
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

        protected override void FillFields()
        {
            List<PageInfo> pages = _pageProvider.SelectAll(ErrorList);
            drlPage.DataSource = pages;
            drlPage.DataBind();

            int pageId = ValidationHelper.GetInteger(Request.QueryString["pageid"], -1);
            if (pageId != -1)
            {
                drlPage.SelectedValue = pageId.ToString();
                PageId = pageId;
            }

            List<WebPartInfo> webPartInfos = _webPartProvider.SelectAll(ErrorList);
            drlWebPart.DataSource = webPartInfos;
            drlWebPart.DataBind();
            drlPage_OnSelectedIndexChanged(this, null);

            if (IsEdit)
            {
                PageNBlockInfo pageNBlockInfo = _pageNBlockProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
                if (pageNBlockInfo != null)
                {
                    drlPage.SelectedValue = pageNBlockInfo.PageId.ToString();

                    PageId = ValidationHelper.GetInteger(drlPage.SelectedValue, 1);
                    PageInfo pageInfo = _pageProvider.Select(PageId, ErrorList);

                    List<LayoutWebPartZoneInfo> layoutWebPartZoneInfos = _layoutWebPartZoneProvider.SelectAllByLayoutId(pageInfo.PageLayoutId, ErrorList);
                    drlWebPartZoneName.DataSource = layoutWebPartZoneInfos;
                    drlWebPartZoneName.DataBind();
                    drlWebPartZoneName.SelectedValue = pageNBlockInfo.WebPartZoneName;

                    drlOrder.SelectedValue = pageNBlockInfo.Order.ToString();
                    drlLanguages.SelectedValue = pageNBlockInfo.Language;
                    BlockInfo blockInfo = _blockProvider.Select(pageNBlockInfo.BlockId, ErrorList);
                    txtName.Text = blockInfo.Name;
                    drlWebPart.SelectedValue = blockInfo.WebPartId.ToString();
                    FillWebPartZones();
                    RenderWebPart(blockInfo.Properties);
                }
            }

        }

        protected override bool Update()
        {
            PageNBlockInfo pageNBlockInfo = _pageNBlockProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
            if (pageNBlockInfo != null)
            {
                BlockInfo blockInfo = _blockProvider.Select(pageNBlockInfo.BlockId, ErrorList);
                blockInfo.Name = txtName.Text;
                RenderWebPart();
                FWebPartEdit webPartControl = pnlWebPart.FindControl("webPartEditControl") as FWebPartEdit;
                if (webPartControl != null)
                {
                    blockInfo.Properties = webPartControl.GetAttributes();
                }
                blockInfo.WebPartId = ValidationHelper.GetInteger(drlWebPart.SelectedValue, 0);
                blockInfo.Order = ValidationHelper.GetInteger(drlOrder.SelectedValue, 0);
                _blockProvider.Update(blockInfo, ErrorList);

                pageNBlockInfo.Language = drlLanguages.SelectedValue;
                pageNBlockInfo.PageId = ValidationHelper.GetInteger(drlPage.SelectedValue, 0);
                pageNBlockInfo.WebPartZoneName = drlWebPartZoneName.SelectedValue;
                pageNBlockInfo.Order = ValidationHelper.GetInteger(drlOrder.SelectedValue, 0);
                _pageNBlockProvider.Update(pageNBlockInfo, ErrorList);
            }

            RenderWebPart();
            return CheckErrors();
        }

        protected override bool Insert()
        {
            BlockInfo blockInfo = new BlockInfo();
            blockInfo.Name = txtName.Text;
            RenderWebPart();
            FWebPartEdit webPartControl = pnlWebPart.FindControl("webPartEditControl") as FWebPartEdit;
            if (webPartControl != null)
            {
                blockInfo.Properties = webPartControl.GetAttributes();
            }
            blockInfo.WebPartId = ValidationHelper.GetInteger(drlWebPart.SelectedValue, 0);
            blockInfo.Order = ValidationHelper.GetInteger(drlOrder.SelectedValue, 0);
            _blockProvider.Create(blockInfo, ErrorList);

            if (CheckErrors())
            {
                PageNBlockInfo pageNBlockInfo = new PageNBlockInfo();
                pageNBlockInfo.BlockId = blockInfo.Id;
                pageNBlockInfo.PageId = ValidationHelper.GetInteger(drlPage.SelectedValue, 0);
                pageNBlockInfo.WebPartZoneName = drlWebPartZoneName.SelectedValue;
                pageNBlockInfo.Order = ValidationHelper.GetInteger(drlOrder.SelectedValue, 0);
                pageNBlockInfo.Language = drlLanguages.SelectedValue;
                _pageNBlockProvider.Create(pageNBlockInfo, ErrorList);

            }
            return CheckErrors();
        }

        protected void drlPage_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PageId = ValidationHelper.GetInteger(drlPage.SelectedValue, 1);

            FillWebPartZones();
            RenderWebPart();
        }

        protected void drlWebPart_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            RenderWebPart();
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

        private void FillWebPartZones()
        {
            PageInfo pageInfo = _pageProvider.Select(PageId, ErrorList);

            if (pageInfo != null)
            {
                List<LayoutWebPartZoneInfo> layoutWebPartZoneInfos = _layoutWebPartZoneProvider.SelectAllByLayoutId(ValidationHelper.GetInteger(pageInfo.PageLayoutId, 0), ErrorList);

                if (layoutWebPartZoneInfos.Count > 0)
                {
                    drlWebPartZoneName.DataSource = layoutWebPartZoneInfos;
                    drlWebPartZoneName.DataBind();
                }
            }
        }

        private void RenderWebPart()
        {
            if (drlWebPart.SelectedIndex > 0)
            {
                WebPartInfo webPartInfo = _webPartProvider.Select(ValidationHelper.GetInteger(drlWebPart.SelectedValue, 0), ErrorList);
                if (webPartInfo != null)
                {
                    string path = webPartInfo.FolderPath;
                    if (path.EndsWith("/"))
                        path += "edit.ascx";
                    else
                        path += "/edit.ascx";
                    FWebPartEdit webPartControl = LoadControl(path) as FWebPartEdit;

                    pnlWebPart.Controls.Clear();
                    if (webPartControl != null)
                    {
                        webPartControl.ID = "webPartEditControl";
                        pnlWebPart.Controls.Add(webPartControl);
                    }
                }
            }
        }

        private void RenderWebPart(string properties)
        {
            if (drlWebPart.SelectedIndex > 0)
            {
                WebPartInfo webPartInfo =
                    _webPartProvider.Select(ValidationHelper.GetInteger(drlWebPart.SelectedValue, 0), ErrorList);
                if (webPartInfo != null)
                {
                    string path = webPartInfo.FolderPath;
                    if (path.EndsWith("/"))
                        path += "edit.ascx";
                    else
                        path += "/edit.ascx";
                    FWebPartEdit webPartControl = LoadControl(path, properties) as FWebPartEdit;

                    pnlWebPart.Controls.Clear();
                    if (webPartControl != null)
                    {
                        webPartControl.ID = "webPartEditControl";
                        pnlWebPart.Controls.Add(webPartControl);
                    }
                }
            }
        }
    }
}