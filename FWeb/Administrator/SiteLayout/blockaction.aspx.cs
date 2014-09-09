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

namespace FWeb.Administrator.SiteLayout
{
    public partial class blockaction : FAdminEditPage
    {
        private LayoutProvider _layoutProvider;
        private LayoutWebPartZoneProvider _layoutWebPartZoneProvider;
        private LayoutNBlockProvider _layoutNBlockProvider;
        private WebPartProvider _webPartProvider;
        private BlockProvider _blockProvider;
        private LocalizationProvider _localizationProvider;

        private int LayoutId
        {
            get
            {
                return ValidationHelper.GetInteger(ViewState["_layoutId"], 1);
            }
            set { ViewState["_layoutId"] = value; }
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
            if (_layoutNBlockProvider == null)
                _layoutNBlockProvider = new LayoutNBlockProvider();
            if (_localizationProvider == null)
                _localizationProvider = new LocalizationProvider();

            if (IsEdit)
            {
                Title = "Edit Site Layout Block | Frebo Cms";
                ltlTitle.Text = "Edit";
            }
            else
            {
                Title = "New Site Layout Block  | Frebo Cms";
                ltlTitle.Text = "New";
            }
            FillLanguages();
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/sitelayout/blocks.aspx?layoutid=" + LayoutId;
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
            List<LayoutInfo> layouts = _layoutProvider.SelectAllByType(true, ErrorList);
            drlLayout.DataSource = layouts;
            drlLayout.DataBind();

            int layoutId = ValidationHelper.GetInteger(Request.QueryString["layoutid"], -1);
            if (layoutId != -1)
            {
                drlLayout.SelectedValue = layoutId.ToString();
                LayoutId = layoutId;
            }
            List<LayoutWebPartZoneInfo> layoutWebPartZoneInfos = _layoutWebPartZoneProvider.SelectAllByLayoutId(LayoutId, ErrorList);
            drlWebPartZoneName.DataSource = layoutWebPartZoneInfos;
            drlWebPartZoneName.DataBind();

            List<WebPartInfo> webPartInfos = _webPartProvider.SelectAll(ErrorList);
            drlWebPart.DataSource = webPartInfos;
            drlWebPart.DataBind();


            if (IsEdit)
            {
                LayoutNBlockInfo layoutNBlockInfo = _layoutNBlockProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
                if (layoutNBlockInfo != null)
                {
                    drlLanguages.SelectedValue = layoutNBlockInfo.Language;
                    drlLayout.SelectedValue = layoutNBlockInfo.LayoutId.ToString();
                    FillWebPartZones();
                    if (layoutWebPartZoneInfos.Any(x => x.Name == layoutNBlockInfo.WebPartZoneName))
                        drlWebPartZoneName.SelectedValue = layoutNBlockInfo.WebPartZoneName;
                    drlOrder.SelectedValue = layoutNBlockInfo.Order.ToString();
                    BlockInfo blockInfo = _blockProvider.Select(layoutNBlockInfo.BlockId, ErrorList);
                    txtName.Text = blockInfo.Name;
                    drlWebPart.SelectedValue = blockInfo.WebPartId.ToString();
                    RenderWebPart(blockInfo.Properties);
                    LayoutId = ValidationHelper.GetInteger(drlLayout.SelectedValue, 1);
                }
            }

        }

        protected override bool Update()
        {
            LayoutNBlockInfo layoutNBlockInfo = _layoutNBlockProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
            if (layoutNBlockInfo != null)
            {
                BlockInfo blockInfo = _blockProvider.Select(layoutNBlockInfo.BlockId, ErrorList);
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

                layoutNBlockInfo.Language = drlLanguages.SelectedValue;
                layoutNBlockInfo.LayoutId = ValidationHelper.GetInteger(drlLayout.SelectedValue, 0);
                layoutNBlockInfo.WebPartZoneName = drlWebPartZoneName.SelectedValue;
                layoutNBlockInfo.Order = ValidationHelper.GetInteger(drlOrder.SelectedValue, 0);
                _layoutNBlockProvider.Update(layoutNBlockInfo, ErrorList);
                LayoutInfo layoutInfo = _layoutProvider.Select(layoutNBlockInfo.LayoutId, ErrorList);
                _layoutProvider.DeleteObjectFromCache(layoutInfo);
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
                LayoutNBlockInfo layoutNBlockInfo = new LayoutNBlockInfo();
                layoutNBlockInfo.Language = drlLanguages.SelectedValue;
                layoutNBlockInfo.BlockId = blockInfo.Id;
                layoutNBlockInfo.LayoutId = ValidationHelper.GetInteger(drlLayout.SelectedValue, 0);
                layoutNBlockInfo.WebPartZoneName = drlWebPartZoneName.SelectedValue;
                layoutNBlockInfo.Order = ValidationHelper.GetInteger(drlOrder.SelectedValue, 0);
                _layoutNBlockProvider.Create(layoutNBlockInfo, ErrorList);
                LayoutInfo layoutInfo = _layoutProvider.Select(layoutNBlockInfo.LayoutId, ErrorList);
                _layoutProvider.DeleteObjectFromCache(layoutInfo);
            }
            return CheckErrors();
        }

        protected void drlLayout_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            LayoutId = ValidationHelper.GetInteger(drlLayout.SelectedValue, 1);
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
            List<LayoutWebPartZoneInfo> layoutWebPartZoneInfos = _layoutWebPartZoneProvider.SelectAllByLayoutId(ValidationHelper.GetInteger(drlLayout.SelectedValue, LayoutId), ErrorList);
            drlWebPartZoneName.DataSource = layoutWebPartZoneInfos;
            drlWebPartZoneName.DataBind();
        }

        private void RenderWebPart()
        {
            WebPartInfo webPartInfo = _webPartProvider.Select(ValidationHelper.GetInteger(drlWebPart.SelectedValue, 1), ErrorList);
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

        private void RenderWebPart(string properties)
        {
            WebPartInfo webPartInfo = _webPartProvider.Select(ValidationHelper.GetInteger(drlWebPart.SelectedValue, 1), ErrorList);
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