using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Helper;
using FUIControls.PortalControl;

namespace FWeb.WebParts.general.datalist
{
    public partial class webpart : FWebPart
    {
        #region Properties
        protected string BeforeContainer
        {
            get { return GetProperty("BeforeContainer"); }
        }

        protected string AfterContainer
        {
            get { return GetProperty("AfterContainer"); }
        }

        protected string Transformation
        {
            get { return GetProperty("Transformation"); }
        }

        protected string RowBefore
        {
            get { return ValidationHelper.GetString(GetProperty("RowBefore"), string.Empty); }
        }

        protected string RowAfter
        {
            get { return ValidationHelper.GetString(GetProperty("RowAfter"), string.Empty); }
        }

        protected int RepeatColumns
        {
            get { return ValidationHelper.GetInteger(GetProperty("RepeatColumns"), 0); }
        }

        protected string QueryName
        {
            get { return GetProperty("QueryName"); }
        }

        protected string QueryParameters
        {
            get { return GetProperty("QueryParameters"); }
        }
        #endregion

        #region Methods
        protected override void LoadWebPart()
        {
            beforeContainer.Text = BeforeContainer;
            dataList.AlternativeTransformation = Transformation;
            dataList.Transformation = Transformation;
            if (RepeatColumns != 0)
                dataList.RepeatColumns = RepeatColumns;

            afterContainer.Text = AfterContainer;
            dataList.QueryName = QueryName;
            dataList.RowBefore = RowBefore;
            dataList.RowAfter = RowAfter;
            dataList.RepeatLayout = RepeatLayout.Table;
            bool ok;
            var paramaters = GetSelectParamaters(QueryParameters, out ok);
            if (ok)
                dataList.QueryParameters = paramaters;

            dataList.DataBind();
        }
        #endregion
    }
}