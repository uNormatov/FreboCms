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
using FUIControls.Settings;

namespace FWeb.Administrator.Localization
{
    public partial class action : FAdminEditPage
    {
        private LocalizationProvider _localizationProvider;
        private SiteProvider _siteProvider;

        protected override void Init()
        {
            base.Init();

            if (_localizationProvider == null)
                _localizationProvider = new LocalizationProvider();

            if (_siteProvider == null)
                _siteProvider = new SiteProvider();


            if (IsEdit)
            {
                Title = "Edit Language | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "Edit";
            }
            else
            {
                Title = "New Language | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "New";
            }
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/localization";
            RedrictUrl = CancelUrl;
        }

        protected override void FillFields()
        {
            if (IsEdit && CheckErrors())
            {
                LanguageInfo languageInfo = _localizationProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
                txtName.Text = languageInfo.Name;
                txtCode.Text = languageInfo.Code;
                txtCode.Enabled = false;
                chbxIsDefault.Checked = CoreSettings.CurrentSite.DefaultLanguage.Equals(languageInfo.Code);
            }
        }

        protected override bool Update()
        {
            LanguageInfo info = _localizationProvider.Select(ValidationHelper.GetInteger(Id, 0), ErrorList);
            if (info != null)
            {
                info.Name = txtName.Text;
                info.Code = txtCode.Text;
                _localizationProvider.Update(info, ErrorList);
                if (chbxIsDefault.Checked)
                {
                    CoreSettings.CurrentSite.DefaultLanguage = info.Code;
                    CoreSettings.CurrentSite.IsMultilanguage = true;
                    _siteProvider.Update(CoreSettings.CurrentSite, ErrorList);
                }
            }
            return CheckErrors();
        }

        protected override bool Insert()
        {
            LanguageInfo info = new LanguageInfo();
            info.Name = txtName.Text;
            info.Code = txtCode.Text;
            _localizationProvider.Create(info, ErrorList);
            if (chbxIsDefault.Checked)
            {
                CoreSettings.CurrentSite.DefaultLanguage = info.Code;
                CoreSettings.CurrentSite.IsMultilanguage = true;
                _siteProvider.Update(CoreSettings.CurrentSite, ErrorList);
            }
            return CheckErrors();
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
    }
}