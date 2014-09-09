using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.FormControl;
using FUIControls.Helper;
using FUIControls.Page;
using FUIControls.Settings;
using FUIControls.UIControl;

namespace FWeb.Administrator.Localization
{
    public partial class translation : FAdminPage
    {
        private LocalizationProvider _localizationProvider;

        protected override void Init()
        {
            base.Init();
            if (_localizationProvider == null)
                _localizationProvider = new LocalizationProvider();

            List<LanguageInfo> languageInfos = _localizationProvider.SelectAll(ErrorList);

            if (languageInfos != null && languageInfos.Count > 0)
            {
                object[,] param = new object[2, 3];
                param[0, 0] = "@PageIndex";
                param[0, 1] = PageIndex;
                param[1, 0] = "@PageSize";
                param[1, 1] = PageSize;

                rptList.HeaderTemplate = new TranslationTemplate(languageInfos.ToArray(), ListItemType.Header);
                rptList.ItemTemplate = new TranslationTemplate(languageInfos.ToArray(), ListItemType.Item);
                rptList.AlternatingItemTemplate = new TranslationTemplate(languageInfos.ToArray(), ListItemType.AlternatingItem);
                rptList.QueryType = QueryType.StoredProcedure;
                rptList.QueryParameters = param;
                rptList.QueryName = "[dbo].[freb_Translation_SelectByPaging]";
                rptList.DataBind();
            }
        }

        protected override void FillGrid()
        {

        }

        protected override void ParsePost()
        {
            string action = Request.Form["form-action"];
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("add"))
                {
                    Response.Redirect("/administrator/localization/translationaction.aspx");
                }
                else if (action.Equals("edit"))
                {
                    string checkboxs = Request.Form["chbxRow"];
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        Response.Redirect("/administrator/localization/translationaction.aspx?type=entry&id=" + temps[0]);
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
                if (!_localizationProvider.DeleteTranslation(item, ErrorList))
                {
                    PrintErrors();
                    break;
                }
            }
            if (CheckErrors())
            {
                LanguageHelper.Instance.Clear();
                CacheHelper.DeleteAll();
                Response.Redirect("/administrator/localization/translation.aspx");
            }
        }

        protected void rptList_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                var drlDisplay = e.Item.FindControl("drlDisplay") as DropDownList;
                for (int i = 0; i < drlDisplay.Items.Count; i++)
                {
                    if (drlDisplay.Items[i].Value.Equals(PageSize.ToString()))
                    {
                        drlDisplay.SelectedIndex = i;
                        break;
                    }
                }

                FPager pager = e.Item.FindControl("pager") as FPager;
                if (pager != null)
                {
                    GeneralConnection generalConnection = new GeneralConnection();
                    DataTable dataTable = generalConnection.ExecuteDataTableQuery("[dbo].[freb_Translation_TotalCount]", null, QueryType.StoredProcedure, new ErrorInfoList());
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        pager.PageIndex = PageIndex;
                        pager.PageSize = PageSize;
                        pager.TotalCount = ValidationHelper.GetInteger(dataTable.Rows[0][0], 0);
                    }
                }
            }
        }

        protected void DrlDisplaySelectedIndexChanged(object sender, EventArgs e)
        {
            var drl = sender as DropDownList;
            Response.Redirect("translation.aspx?size=" + drl.SelectedValue);
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
            builder.Append("<li>Translation successfully saved</li>");
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
        }


    }
}