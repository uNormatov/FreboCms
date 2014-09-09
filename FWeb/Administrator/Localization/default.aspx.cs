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
using FUIControls.Helper;
using FUIControls.Page;
using FUIControls.Settings;

namespace FWeb.Administrator.Localization
{
    public partial class _default : FAdminPage
    {
        private LocalizationProvider _localizationProvider;
        private SiteProvider _siteProvider;

        protected override void Init()
        {
            base.Init();
            if (_localizationProvider == null)
                _localizationProvider = new LocalizationProvider();
        }

        protected override void FillGrid()
        {
            List<LanguageInfo> languageInfos =
                            _localizationProvider.SelectAll(ErrorList);

            if (languageInfos != null && languageInfos.Count > 0)
            {
                foreach (LanguageInfo info in languageInfos)
                {
                    info.IsDefault = info.Code.Equals(CoreSettings.CurrentSite.DefaultLanguage);
                }
                rptList.DataSource = languageInfos;
                rptList.DataBind();
            }
        }

        protected override void ParsePost()
        {
            string action = Request.Form["form-action"];
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("add"))
                {
                    Response.Redirect("/administrator/localization/action.aspx?type=entry");
                }
                else if (action.Equals("edit"))
                {
                    string checkboxs = Request.Form["chbxRow"];
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        Response.Redirect("/administrator/localization/action.aspx?type=entry&id=" + temps[0]);
                    }
                }
                else
                {
                    string checkboxs = Request.Form["chbxRow"];
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        DeleteRows(temps);
                    }
                }
            }
            base.ParsePost();
        }

        private void DeleteRows(string[] ids)
        {
            foreach (string item in ids)
            {
                LanguageInfo languageInfo = _localizationProvider.Select(ValidationHelper.GetInteger(item, 0), ErrorList);
                if (languageInfo != null && languageInfo.Code.Equals(CoreSettings.CurrentSite.DefaultLanguage))
                {
                    if (_siteProvider == null)
                        _siteProvider = new SiteProvider();
                    CoreSettings.CurrentSite.DefaultLanguage = string.Empty;
                    CoreSettings.CurrentSite.IsMultilanguage = false;
                    _siteProvider.Update(CoreSettings.CurrentSite, ErrorList);
                    LanguageHelper.Instance.Clear();
                }
                if (!_localizationProvider.Delete(ValidationHelper.GetInteger(item, 0), ErrorList))
                {
                    PrintErrors();
                    break;
                }
            }
            LanguageHelper.Instance.Clear();
            FillGrid();
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
            ErrorList.Clear();
        }

        protected override void PrintSuccess()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<dl id=\"system-message\">");
            builder.Append("<dt class=\"message\">Message</dt><dd class=\"message message\">");
            builder.Append("<ul>");
            builder.Append("<li>Language successfully saved</li>");
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
        }
    }
}