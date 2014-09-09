using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Enum;
using FUIControls.Context;

namespace FUIControls.UIControl
{
    public class FPager : Control
    {
        public FPager()
        {
            IsBackend = false;
        }

        public bool IsBackend { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalCount { get; set; }

        public string NextText { get; set; }

        public string PreviusText { get; set; }

        public string PageIndexKeyword { get; set; }

        public string ContainerTag { get; set; }

        public string ContainetCss { get; set; }

        public string InnerContainerTag { get; set; }

        public string InnerContainerCss { get; set; }

        public string ActiveCss { get; set; }

        public QueryParameterType PageIndexKeywordType { get; set; }

        protected override void CreateChildControls()
        {
            if (TotalCount > 0)
                Controls.Add(CreatePager());
        }

        private WebControl CreatePager()
        {
            if (PageIndex == 0)
                PageIndex = 1;
            if (string.IsNullOrEmpty(PreviusText))
                PreviusText = "&lt;";
            if (string.IsNullOrEmpty(NextText))
                NextText = "&gt;";

            string containerTag = "div";
            string containerCss = "pager";
            string innerContainerTag = "span";
            string innerContainerCss = "other";
            string activeCss = "current";
            if (!string.IsNullOrEmpty(ContainerTag))
                containerTag = ContainerTag;
            if (!string.IsNullOrEmpty(ContainetCss))
                containerCss = ContainetCss;
            if (!string.IsNullOrEmpty(InnerContainerTag))
                innerContainerTag = InnerContainerTag;
            if (!string.IsNullOrEmpty(InnerContainerCss))
                innerContainerCss = InnerContainerCss;
            if (!string.IsNullOrEmpty(ActiveCss))
                activeCss = ActiveCss;

            var div = new WebControl(containerTag.Equals("div") ? HtmlTextWriterTag.Div : HtmlTextWriterTag.Ul);
            div.Attributes.Add("itemscope", string.Empty);
            div.Attributes.Add("itemtype", "http://schema.org/SiteNavigationElement");
            div.CssClass = containerCss;
            var pageCount = (int)Math.Ceiling(TotalCount / (double)PageSize);
            int nrOfPagesToDisplay = 10;


            if (PageIndex > 1)
            {
                var span1 = new WebControl(innerContainerTag.Equals("span") ? HtmlTextWriterTag.Span : HtmlTextWriterTag.Li);
                span1.CssClass = innerContainerCss;
                span1.Controls.Add(GeneratePageLink(PreviusText, (PageIndex - 1).ToString()));
                span1.Attributes.Add("itemprop", "url");
                div.Controls.Add(span1);
            }

            int start = 1;
            int end = pageCount;

            if (pageCount > nrOfPagesToDisplay)
            {
                int middle = (int)Math.Ceiling(nrOfPagesToDisplay / 2d) - 1;
                int below = (PageIndex - middle);
                int above = (PageIndex + middle);

                if (below < 4)
                {
                    above = nrOfPagesToDisplay;
                    below = 1;
                }
                else if (above > (pageCount - 4))
                {
                    above = pageCount;
                    below = (pageCount - nrOfPagesToDisplay);
                }

                start = below;
                end = above;
            }

            if (start > 3)
            {
                var span2 = new WebControl(innerContainerTag.Equals("span") ? HtmlTextWriterTag.Span : HtmlTextWriterTag.Li);
                span2.CssClass = innerContainerCss;
                span2.Controls.Add(GeneratePageLink("1", "1"));
                div.Controls.Add(span2);

                var span3 = new WebControl(innerContainerTag.Equals("span") ? HtmlTextWriterTag.Span : HtmlTextWriterTag.Li);
                span3.CssClass = innerContainerCss;
                span3.Controls.Add(GeneratePageLink("2", "1"));
                div.Controls.Add(span3);

                var span4 = new WebControl(innerContainerTag.Equals("span") ? HtmlTextWriterTag.Span : HtmlTextWriterTag.Li);
                span4.CssClass = innerContainerCss;
                span4.Controls.Add(new LiteralControl("..."));
                div.Controls.Add(span4);
            }
            for (int i = start; i <= end; i++)
            {
                var span5 = new WebControl(innerContainerTag.Equals("span") ? HtmlTextWriterTag.Span : HtmlTextWriterTag.Li);
                if (i == PageIndex)
                {
                    span5.CssClass = activeCss;
                    span5.Controls.Add(new LiteralControl(string.Format("<a>{0}</a>", i)));
                }
                else
                {
                    span5.CssClass = innerContainerCss;
                    span5.Controls.Add(GeneratePageLink(i.ToString(), i.ToString()));
                }
                div.Controls.Add(span5);
            }

            if (end < (pageCount - 3))
            {
                var span6 = new WebControl(innerContainerTag.Equals("span") ? HtmlTextWriterTag.Span : HtmlTextWriterTag.Li);
                span6.CssClass = innerContainerCss;
                span6.Controls.Add(new LiteralControl("..."));
                div.Controls.Add(span6);
                var span7 = new WebControl(innerContainerTag.Equals("span") ? HtmlTextWriterTag.Span : HtmlTextWriterTag.Li);
                span7.CssClass = innerContainerCss;
                span7.Controls.Add(GeneratePageLink((pageCount - 1).ToString(), (pageCount - 1).ToString()));
                div.Controls.Add(span7);
                var span8 = new WebControl(innerContainerTag.Equals("span") ? HtmlTextWriterTag.Span : HtmlTextWriterTag.Li);
                span8.CssClass = innerContainerCss;
                span8.Controls.Add(GeneratePageLink(pageCount.ToString(), pageCount.ToString()));
                div.Controls.Add(span8);
            }

            // Next

            if (PageIndex < pageCount)
            {
                var span9 = new WebControl(innerContainerTag.Equals("span") ? HtmlTextWriterTag.Span : HtmlTextWriterTag.Li);
                span9.CssClass = innerContainerCss;
                span9.Controls.Add(GeneratePageLink(NextText, (PageIndex + 1).ToString()));
                div.Controls.Add(span9);
            }


            return div;
        }

        private HyperLink GeneratePageLink(string text, string argument)
        {
            if (PageIndexKeywordType == QueryParameterType.QueryString)
                return GenerateQueryStringLink(text, argument);
            return GenerateSeoLink(text, argument);
        }

        private HyperLink GenerateQueryStringLink(string text, string argument)
        {
            var link = new HyperLink();
            link.Text = text;
            link.Attributes.Add("itemprop", "name");
            string currentRequestString;
            string currentUrl;
            if (!IsBackend)
            {
                currentRequestString = FContext.CurrentQueryString;
                if (FContext.CurrentSeoUrl.IndexOf("?") > -1)
                    currentUrl = FContext.CurrentSeoUrl.Substring(0, FContext.CurrentSeoUrl.IndexOf("?"));
                else
                    currentUrl = FContext.CurrentSeoUrl;
            }
            else
            {
                currentRequestString = Context.Request.QueryString.ToString();
                if (!string.IsNullOrEmpty(currentRequestString))
                    currentRequestString = "?" + currentRequestString;
                currentUrl = Context.Request.Path;
            }


            if (!string.IsNullOrEmpty(currentRequestString))
            {
                string[] pars = currentRequestString.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                string qs = pars[0];

                bool ok = false;
                if (qs.Contains(string.Format("?{0}=", PageIndexKeyword)))
                {
                    qs = string.Format("?{0}={1}", PageIndexKeyword, argument);
                    ok = true;
                }


                for (int i = 1; i < pars.Length; i++)
                {
                    if (pars[i].Contains(string.Format("{0}=", PageIndexKeyword)))
                    {
                        qs += string.Format("&{0}={1}", PageIndexKeyword, argument);
                        ok = true;
                    }

                    else
                    {
                        qs += "&" + pars[i];
                    }
                }
                if (!ok)
                    qs += string.Format("&{0}={1}", PageIndexKeyword, argument);
                ;

                link.NavigateUrl = string.Format("{0}{1}", currentUrl, qs);
            }
            else
            {
                link.NavigateUrl = string.Format("{0}?{2}={1}", currentUrl, argument, PageIndexKeyword);
            }
            return link;
        }

        private HyperLink GenerateSeoLink(string text, string argument)
        {
            var link = new HyperLink();
            link.Text = text;

            return link;
        }

    }
}
