using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Helper;
using FDataProvider;
using FUIControls.Helper;
using FUIControls.PortalControl;
using FUIControls.Settings;

namespace FWeb.WebParts.general.breadcrumb
{
    public partial class webpart : FWebPart
    {
        private List<PageInfo> _breadCrumbList;
        protected string Separator
        {
            get { return GetProperty("Separator"); }

        }

        protected override void LoadWebPart()
        {
            if (CoreSettings.CurrentSite.DefaultPageId == PageInfo.Id)
                return;
            _breadCrumbList = new List<PageInfo>();
            PageProvider pageProvider = new PageProvider();
            List<PageInfo> list = pageProvider.SelectAll(new ErrorInfoList());
            if (PageInfo.ParentId != 0)
                FillBreadCrumbList(list, PageInfo.ParentId);

            if (_breadCrumbList.Count > 0)
            {
                _breadCrumbList.Reverse();
                StringBuilder breadCrumb = new StringBuilder();
                foreach (PageInfo pageInfo in _breadCrumbList)
                {
                    if (breadCrumb.Length > 0)
                        breadCrumb.AppendFormat("&nbsp;{0}&nbsp;", Separator);

                    if (breadCrumb.Length == 0)
                        breadCrumb.AppendFormat("<a class=\"birinchi\" href=\"/{0}{1}\">{2}</a>", GetCurrentLanguage(), pageInfo.SeoTemplate,
                                            LanguageHelper.Instance.GetTranslateByPattern(GetCurrentLanguage(), pageInfo.BreadCrumbTitle));
                    else
                    {
                        breadCrumb.AppendFormat("<a href=\"/{0}{1}\">{2}</a>", GetCurrentLanguage(), pageInfo.SeoTemplate,
                                                LanguageHelper.Instance.GetTranslateByPattern(GetCurrentLanguage(), pageInfo.BreadCrumbTitle));
                    }

                }
                string lastNode = LanguageHelper.Instance.GetTranslateByPattern(GetCurrentLanguage(), PageInfo.BreadCrumbTitle);
                string nodes = string.Format("<div id=\"navigator\" class=\"navigator\">{0}", breadCrumb);

                if (!string.IsNullOrEmpty(lastNode))
                    nodes += string.Format("&nbsp;{0}&nbsp;{1}", Separator, lastNode);

                ltlBreadcrumb.Text = nodes + "</div>";
            }
        }

        private void FillBreadCrumbList(List<PageInfo> list, int parent)
        {
            PageInfo pageInfo = list.FirstOrDefault(x => x.Id == parent);
            if (pageInfo != null)
            {
                _breadCrumbList.Add(pageInfo);
                if (pageInfo.ParentId != 0)
                    FillBreadCrumbList(list, pageInfo.ParentId);
            }
        }
    }
}