using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.PortalControl;

namespace FWeb.UserControls
{
    public partial class BlockEditor : UserControl
    {
        private BlockProvider _blockProvider;

        public int ObjectId { get; set; }
        public int StructureId { get; set; }
        public StructureType Type { get; set; }
        public ErrorInfoList ErrorList { get; set; }
        public bool IsEdit { get; set; }
        public int WebPartId
        {
            get
            {
                return ValidationHelper.GetInteger(ViewState["_webPartId"], 0);
            }
            set { ViewState["_webPartId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillFields();
        }

        private void FillFields()
        {
            _blockProvider = new BlockProvider();

            WebPartProvider webPartProvider = new WebPartProvider();
            List<WebPartInfo> webPartInfos = webPartProvider.SelectAll(ErrorList);
            drlWebPart.Items.Clear();

            drlWebPart.DataSource = webPartInfos;
            drlWebPart.DataBind();

            if (Type == StructureType.Page)
                FillPageFields();
            else
                FillLayoutFields();


            drlStructureItems_OnSelectedIndexChanged(this, null);
        }

        private void FillPageFields()
        {
            if (!IsPostBack)
            {
                lblStructureType.Text = "Page *";
                FillStructureItems();
                FillWebpartZones();
                if (IsEdit)
                {
                    PageNBlockProvider pageNBlockProvider = new PageNBlockProvider();
                    PageNBlockInfo pageNBlockInfo = pageNBlockProvider.Select(ObjectId, ErrorList);
                    if (drlWebPartZoneName.Items.FindByValue(pageNBlockInfo.WebPartZoneName) != null)
                        drlWebPartZoneName.SelectedValue = pageNBlockInfo.WebPartZoneName;

                    drlOrder.SelectedValue = pageNBlockInfo.Order.ToString();
                    BlockInfo blockInfo = _blockProvider.Select(pageNBlockInfo.BlockId, ErrorList);
                    txtName.Text = blockInfo.Name;
                    drlWebPart.SelectedValue = blockInfo.WebPartId.ToString();
                    RenderWebPart(blockInfo.Properties);
                }
            }
        }

        private void FillLayoutFields()
        {
            if (Type == StructureType.PageLayout)
                lblStructureType.Text = "Page Layout *";
            else
                lblStructureType.Text = "Site Layout *";

            FillStructureItems();
            FillWebpartZones();
            if (IsEdit)
            {
                LayoutNBlockProvider layoutNBlockProvider = new LayoutNBlockProvider();
                LayoutNBlockInfo layoutNBlockInfo = layoutNBlockProvider.Select(ObjectId, ErrorList);
                if (layoutNBlockInfo != null)
                {
                    if (drlWebPartZoneName.Items.FindByText(layoutNBlockInfo.WebPartZoneName) != null)
                        drlWebPartZoneName.SelectedValue = layoutNBlockInfo.WebPartZoneName;
                    drlOrder.SelectedValue = layoutNBlockInfo.Order.ToString();

                    BlockInfo blockInfo = _blockProvider.Select(layoutNBlockInfo.BlockId, ErrorList);
                    txtName.Text = blockInfo.Name;
                    drlWebPart.SelectedValue = blockInfo.WebPartId.ToString();
                    RenderWebPart(blockInfo.Properties);
                }
            }
        }

        private void FillStructureItems()
        {
            if (Type == StructureType.Page)
            {
                PageProvider pageProvider = new PageProvider();
                drlStructureItems.DataSource = pageProvider.SelectAll(ErrorList);
            }
            else if (Type == StructureType.PageLayout)
            {
                LayoutProvider layoutProvider = new LayoutProvider();
                drlStructureItems.DataSource = layoutProvider.SelectAllByType(false, ErrorList);
            }
            else
            {
                LayoutProvider layoutProvider = new LayoutProvider();
                drlStructureItems.DataSource = layoutProvider.SelectAllByType(true, ErrorList); ;
            }

            drlStructureItems.DataBind();
            drlStructureItems.SelectedValue = StructureId.ToString();
        }

        private void FillWebpartZones()
        {
            LayoutProvider layoutProvider = new LayoutProvider();
            LayoutInfo layoutInfo = null;
            if (Type == StructureType.Page)
            {
                PageProvider pageProvider = new PageProvider();
                PageInfo pageInfo = pageProvider.Select(StructureId, ErrorList);
                if (pageInfo != null)
                {
                    layoutInfo = layoutProvider.Select(pageInfo.PageLayoutId, ErrorList);
                }
            }
            else if (Type == StructureType.PageLayout || Type == StructureType.SiteLayout)
            {
                layoutInfo = layoutProvider.Select(StructureId, ErrorList);
            }

            if (layoutInfo != null)
            {
                LayoutWebPartZoneProvider layoutWebPartZoneProvider = new LayoutWebPartZoneProvider();
                List<LayoutWebPartZoneInfo> layoutWebPartZoneInfos = layoutWebPartZoneProvider.SelectAllByLayoutId(layoutInfo.Id, ErrorList);

                if (layoutWebPartZoneInfos.Count > 0)
                {
                    drlWebPartZoneName.DataSource = layoutWebPartZoneInfos;
                    drlWebPartZoneName.DataBind();
                }
            }
        }

        private void RenderWebPart()
        {
            WebPartProvider webPartProvider = new WebPartProvider();
            WebPartInfo webPartInfo = webPartProvider.Select(ValidationHelper.GetInteger(drlWebPart.SelectedValue, 1), ErrorList);
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
            WebPartProvider webPartProvider = new WebPartProvider();
            WebPartInfo webPartInfo = webPartProvider.Select(ValidationHelper.GetInteger(drlWebPart.SelectedValue, 1), ErrorList);
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

        protected void drlStructureItems_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            StructureId = ValidationHelper.GetInteger(drlStructureItems.SelectedValue, 0);
            FillWebpartZones();
        }

        protected void drlWebPart_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            WebPartId = ValidationHelper.GetInteger(drlWebPart.SelectedValue, 0);
            RenderWebPart();
        }

        protected UserControl LoadControl(string path, params object[] constructorParameters)
        {
            List<Type> constParamTypes = new List<Type>();
            foreach (object constParam in constructorParameters)
            {
                constParamTypes.Add(constParam.GetType());
            }
            constParamTypes.Add(typeof(ErrorInfoList));
            UserControl control = Page.LoadControl(path) as UserControl;
            if (control != null)
            {
                var baseType = control.GetType().BaseType;
                if (baseType != null)
                {
                    ConstructorInfo constructor = baseType.GetConstructor(constParamTypes.ToArray());
                    if (constructor != null)
                    {
                        List<object> constructorParams = constructorParameters.ToList();
                        constructorParams.Add(ErrorList);

                        constructor.Invoke(control, constructorParams.ToArray());
                    }
                }
            }
            return control;
        }
    }
}