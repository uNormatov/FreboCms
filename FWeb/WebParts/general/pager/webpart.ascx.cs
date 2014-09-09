using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.PortalControl;

namespace FWeb.WebParts.general.pager
{
    public partial class webpart : FWebPart
    {
        private GeneralConnection _generalConnection;

        protected int PageIndexKeywordType
        {
            get { return ValidationHelper.GetInteger(GetProperty("PageIndexKeywordType"), 1); }
        }

        protected string PageIndexKeyword
        {
            get { return GetProperty("PageIndexKeyword"); }
        }

        protected string ContainerTag
        {
            get { return GetProperty("ContainerTag"); }
        }

        protected string ContainerCss
        {
            get { return GetProperty("ContainerCss"); }
        }

        protected string InnerContainerTag
        {
            get { return GetProperty("InnerContainerTag"); }
        }

        protected string InnerContainerCss
        {
            get { return GetProperty("InnerContainerCss"); }
        }

        protected string ActiveCss
        {
            get { return GetProperty("ActiveCss"); }
        }

        protected int PageSize
        {
            get { return ValidationHelper.GetInteger(GetProperty("PageSize"), 5); }
        }

        protected string QueryName
        {
            get { return GetProperty("QueryName"); }
        }

        protected string PrevText
        {
            get { return GetProperty("PrevText"); }
        }

        protected string NextText
        {
            get { return GetProperty("NextText"); }
        }

        protected string QueryParameters
        {
            get { return GetProperty("QueryParameters"); }
        }



        protected override void LoadWebPart()
        {
            if (_generalConnection == null)
                _generalConnection = new GeneralConnection();

            bool ok;
            var paramaters = GetSelectParamaters(QueryParameters, out ok);

            pager.TotalCount =
                ValidationHelper.GetInteger(
                    ok
                        ? _generalConnection.ExecuteScalar(QueryName, paramaters, new ErrorInfoList())
                        : _generalConnection.ExecuteScalar(QueryName, null, new ErrorInfoList()), 0);

            pager.PageSize = PageSize;
            pager.PageIndexKeyword = PageIndexKeyword;
            pager.PreviusText = string.IsNullOrEmpty(PrevText) ? "&lt;" : PrevText;
            pager.NextText = string.IsNullOrEmpty(NextText) ? "&gt;" : NextText;
            
            pager.ContainerTag = ContainerTag;
            pager.ContainetCss = ContainerCss;
            pager.InnerContainerTag = InnerContainerTag;
            pager.InnerContainerCss = InnerContainerCss;
            pager.ActiveCss = ActiveCss;
            const string defaultvalue = "1";
            if (PageIndexKeywordType == ((int)QueryParameterType.QueryString))
            {
                pager.PageIndexKeywordType = QueryParameterType.QueryString;
                pager.PageIndex = ValidationHelper.GetInteger(GetQueryValue(PageIndexKeyword, defaultvalue), 1);
            }
            if (PageIndexKeywordType == ((int)QueryParameterType.SeoTemplate))
            {
                pager.PageIndexKeywordType = QueryParameterType.SeoTemplate;
                pager.PageIndex = ValidationHelper.GetInteger(GetSeoValue(PageIndexKeyword, defaultvalue), 1);
            }
        }
    }
}