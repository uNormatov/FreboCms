using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FUIControls.PortalControl;

namespace FWeb.WebParts.general.evaluablerepeater
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

        protected string AlterTransformation
        {
            get { return GetProperty("AlterTransformation"); }
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
            beforeContainer.Text = GetResourceByPattern(BeforeContainer);
            rptRepeater.AlternativeTransformation = Transformation;
            rptRepeater.Transformation = AlterTransformation;
            afterContainer.Text = GetResourceByPattern(AfterContainer);
            rptRepeater.QueryName = QueryName;
            bool ok;
            var paramaters = GetSelectParamaters(QueryParameters, out ok);
            if (ok)
                rptRepeater.QueryParameters = paramaters;
            rptRepeater.DataBind();
        }
        #endregion
    }
}