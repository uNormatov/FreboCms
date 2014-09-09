using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCore.Constant
{
    public class SiteConstants
    {
        public const string PageData = "page_data";
        public const string UserData = "user_data";
        public const string SiteLanguage = "site_lang";
        public const string LayoutCacheXmlPath = "~/app_data/LayoutCache.xml";
        public const string TransformationCacheXmlPath = "~/app_data/TransformationCache.xml";
        public const string ArticleCacheXmlPath = "~/app_data/ArticleCache.xml";

        public const int JsonFormsByContentTypeId = 1;
        public const int JsonWebpartZoneByPageId = 2;
        public const int JsonWebpartZoneByLayoutId = 3;
        public const int JsonPollVote = 4;


        public const string CaptchaErrorMessage = "incorrectCaptchaMessage";
        public const string CaptchaLabel = "captchaLabel";
        public const string CaptchaValueLabel = "captchaValueLabel";
        public const string All = "all";

    }
}
