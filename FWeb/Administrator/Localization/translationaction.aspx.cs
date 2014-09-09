using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class translationaction : FAdminEditPage
    {
        private LocalizationProvider _localizationProvider;
        private SiteProvider _siteProvider;

        private const int DefaultWidth = 200;

        protected override void Init()
        {
            base.Init();

            if (_localizationProvider == null)
                _localizationProvider = new LocalizationProvider();

            if (_siteProvider == null)
                _siteProvider = new SiteProvider();


            if (IsEdit)
            {
                Title = "Edit Translation | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "Edit Translation";
            }
            else
            {
                Title = "New Translation | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "New Translation";
            }

            BuilForm();
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/localization/translation.aspx";
            RedrictUrl = CancelUrl;
        }

        private void BuilForm()
        {
            Label lblKeyword = new Label();
            lblKeyword.Text = "Keyword";
            lblKeyword.AssociatedControlID = "txtKeyword";
            lblKeyword.CssClass = "jform_name-lbl";

            TextBox txtKeyword = new TextBox();
            txtKeyword.ID = "txtKeyword";
            txtKeyword.Width = DefaultWidth;
            txtKeyword.CssClass = "required";

            pnlMain.Controls.Add(new LiteralControl("<li>"));
            pnlMain.Controls.Add(lblKeyword);
            pnlMain.Controls.Add(txtKeyword);
            pnlMain.Controls.Add(new LiteralControl("</li>"));

            Label lblDefault = new Label();
            lblDefault.Text = "Default";
            lblDefault.AssociatedControlID = "txtDefault";
            lblDefault.CssClass = "jform_name-lbl";

            TextBox txtDefault = new TextBox();
            txtDefault.ID = "txtDefault";
            txtDefault.Width = DefaultWidth;
            txtDefault.CssClass = "required";

            pnlMain.Controls.Add(new LiteralControl("<li>"));
            pnlMain.Controls.Add(lblDefault);
            pnlMain.Controls.Add(txtDefault);
            pnlMain.Controls.Add(new LiteralControl("</li>"));


            List<LanguageInfo> languageInfos = _localizationProvider.SelectAll(ErrorList);

            if (languageInfos != null && languageInfos.Count > 0)
            {
                foreach (LanguageInfo item in languageInfos)
                {
                    Label lblTranslation = new Label();
                    lblTranslation.Text = item.Name;
                    lblTranslation.AssociatedControlID = string.Format("txtTranslation{0}", item.Code);
                    lblTranslation.CssClass = "jform_name-lbl";

                    TextBox txtLocalization = new TextBox();
                    txtLocalization.ID = string.Format("txtTranslation{0}", item.Code);
                    txtLocalization.Width = 400;
                    txtLocalization.Height = 70;
                    txtLocalization.TextMode = TextBoxMode.MultiLine;

                    pnlMain.Controls.Add(new LiteralControl("<li>"));
                    pnlMain.Controls.Add(lblTranslation);
                    pnlMain.Controls.Add(txtLocalization);
                    pnlMain.Controls.Add(new LiteralControl("</li>"));
                }
            }

        }

        protected override void FillFields()
        {
            if (IsEdit)
            {
                List<LanguageInfo> languageInfos = _localizationProvider.SelectAll(ErrorList);
                DataTable dataTable = _localizationProvider.SelectTranslationByKeyword(Id, ErrorList);
                if (CheckErrors() && dataTable != null && dataTable.Rows.Count > 0)
                {
                    TextBox txtKeyword = pnlMain.FindControl("txtKeyword") as TextBox;
                    if (dataTable.Columns.Contains("Keyword") && txtKeyword != null)
                        txtKeyword.Text = ValidationHelper.GetString(dataTable.Rows[0]["Keyword"], string.Empty);

                    TextBox txtDefault = pnlMain.FindControl("txtDefault") as TextBox;
                    if (dataTable.Columns.Contains("DefaultValue") && txtDefault != null)
                        txtDefault.Text = ValidationHelper.GetString(dataTable.Rows[0]["DefaultValue"], string.Empty);

                    foreach (LanguageInfo item in languageInfos)
                    {
                        if (dataTable.Columns.Contains(item.Code))
                        {
                            TextBox txtTranslation = pnlMain.FindControl(string.Format("txtTranslation{0}", item.Code)) as TextBox;
                            if (txtTranslation != null)
                                txtTranslation.Text = ValidationHelper.GetString(dataTable.Rows[0][item.Code], string.Empty);
                        }
                    }
                }
            }

        }

        protected override bool Update()
        {
            List<LanguageInfo> languageInfos = _localizationProvider.SelectAll(ErrorList);
            DataTable dataTable = _localizationProvider.SelectTranslationByKeyword(Id, ErrorList);

            if (CheckErrors() && languageInfos != null && dataTable != null && dataTable.Rows.Count > 0 && dataTable.Columns.Contains("Id"))
            {
                int index = 0;
                object[,] param = new object[languageInfos.Count + 3, 3];
                param[index, 0] = "@Id";
                param[index, 1] = ValidationHelper.GetInteger(dataTable.Rows[0]["Id"], 0);

                TextBox txtKeyword = pnlMain.FindControl("txtKeyword") as TextBox;
                if (txtKeyword != null)
                {
                    index++;
                    param[index, 0] = "@Keyword";
                    param[index, 1] = txtKeyword.Text;
                }

                TextBox txtDefault = pnlMain.FindControl("txtDefault") as TextBox;
                if (txtDefault != null)
                {
                    index++;
                    param[index, 0] = "@DefaultValue";
                    param[index, 1] = txtDefault.Text;
                }

                foreach (LanguageInfo item in languageInfos)
                {
                    TextBox txtTranslation = pnlMain.FindControl(string.Format("txtTranslation{0}", item.Code)) as TextBox;
                    if (txtTranslation != null)
                    {
                        index++;
                        param[index, 0] = string.Format("@{0}", item.Code);
                        param[index, 1] = txtTranslation.Text;
                    }
                }
                _localizationProvider.UpdateTranslation(languageInfos, param, ErrorList);

            }
            LanguageHelper.Instance.Clear();
            CacheHelper.DeleteAll();
            return CheckErrors();
        }

        protected override bool Insert()
        {
            List<LanguageInfo> languageInfos = _localizationProvider.SelectAll(ErrorList);
            if (CheckErrors() && languageInfos != null)
            {
                int index = -1;
                object[,] param = new object[languageInfos.Count + 2, 3];
                TextBox txtKeyword = pnlMain.FindControl("txtKeyword") as TextBox;
                if (txtKeyword != null)
                {
                    index++;
                    param[index, 0] = "@Keyword";
                    param[index, 1] = txtKeyword.Text;
                }

                TextBox txtDefault = pnlMain.FindControl("txtDefault") as TextBox;
                if (txtDefault != null)
                {
                    index++;
                    param[index, 0] = "@DefaultValue";
                    param[index, 1] = txtDefault.Text;
                }

                foreach (LanguageInfo item in languageInfos)
                {
                    TextBox txtTranslation = pnlMain.FindControl(string.Format("txtTranslation{0}", item.Code)) as TextBox;
                    if (txtTranslation != null)
                    {
                        index++;
                        param[index, 0] = string.Format("@{0}", item.Code);
                        param[index, 1] = txtTranslation.Text;
                    }
                }
                _localizationProvider.CreateTranslation(languageInfos, param, ErrorList);

            }
            LanguageHelper.Instance.Clear();
            CacheHelper.DeleteAll();
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
            ErrorList.Clear();
        }

        protected override void PrintSuccess()
        {

        }
    }
}