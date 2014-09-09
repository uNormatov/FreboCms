using System.Linq;
using System.Xml.Linq;
using FCore.Enum;
using FCore.Helper;
using FUIControls.PortalControl;

namespace FWeb.WebParts.general.details
{
    public partial class webpart : FWebPart
    {
        #region Properties
        protected string Transformation
        {
            get { return GetProperty("Transformation"); }
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
            rptRepeater.Transformation = Transformation;
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