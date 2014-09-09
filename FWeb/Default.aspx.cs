using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.Helper;
using FUIControls.Settings;

namespace FWeb
{
    public partial class Default : FUIControls.Page.FPage
    {
        protected override void Init()
        {
            base.Init();

            GetMetaData();

            InsertMetaData();

            InsertLayoutData();
        }
        private void GetMetaData()
        {
            if (!string.IsNullOrEmpty(PageInfo.MetaQueryName))
            {
                using (GeneralConnection generalConnection = new GeneralConnection())
                {
                    ErrorInfoList errors = new ErrorInfoList();
                    bool ok;
                    DataTable dataTable = generalConnection.ExecuteDataTableQuery(PageInfo.MetaQueryName, GetSelectParamaters(PageInfo.MetaQueryParameters, out ok), QueryType.SqlQuery, errors);

                    if (ok && dataTable != null && dataTable.Rows.Count > 0)
                    {
                        if (dataTable.Columns.Contains("MetaTitle"))
                            PageInfo.MetaTitle = ValidationHelper.GetString(dataTable.Rows[0]["MetaTitle"], string.Empty);
                        if (dataTable.Columns.Contains("MetaDescription"))
                            PageInfo.MetadataDescription = ValidationHelper.GetString(dataTable.Rows[0]["MetaDescription"], string.Empty);
                        if (dataTable.Columns.Contains("MetaKeywords"))
                            PageInfo.MetadataKeywords = ValidationHelper.GetString(dataTable.Rows[0]["MetaKeywords"], string.Empty);
                        if (dataTable.Columns.Contains("CopyRights"))
                            PageInfo.ContentRights = ValidationHelper.GetString(dataTable.Rows[0]["CopyRights"], string.Empty);
                        if (dataTable.Columns.Contains("MetaImage"))
                            PageInfo.MetaImage = ValidationHelper.GetString(dataTable.Rows[0]["MetaImage"], string.Empty);
                    }
                }
            }
        }

        private void InsertMetaData()
        {
            string title = LanguageHelper.Instance.GetTranslateByPattern(GetCurrentLanguage(), PageInfo.Title);
            List<string> columns = LanguageHelper.Instance.GetMetaColumnNames(title);
            if (columns.Count == 0)
                Title = title;
            else
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("MetaTitle", PageInfo.MetaTitle);
                Title = LanguageHelper.Instance.EvaluateMetaData(title, values);
            }

            MetaDescription = PageInfo.MetadataDescription;
            MetaKeywords = PageInfo.MetadataKeywords;


            //facebook
            HtmlMeta fb = new HtmlMeta();
            fb.Attributes.Add("property", "og:site_name");
            fb.Content = CoreSettings.CurrentSite.Name;
            Header.Controls.Add(fb);


            fb = new HtmlMeta();
            fb.Attributes.Add("property", "og:title");
            fb.Content = PageInfo.MetaTitle;
            Header.Controls.Add(fb);

            fb = new HtmlMeta();
            fb.Attributes.Add("property", "og:description");
            fb.Content = PageInfo.MetadataDescription;
            Header.Controls.Add(fb);

            fb = new HtmlMeta();
            fb.Attributes.Add("property", "og:url");
            fb.Content = PageInfo.FullUrl;
            Header.Controls.Add(fb);
        }

        private void InsertLayoutData()
        {
            LayoutProvider layoutProvider = new LayoutProvider();

            HtmlHead htmlHead = Header;

            LayoutInfo layoutInfo = layoutProvider.Select(PageInfo.SiteLayoutId, new ErrorInfoList());
            if (layoutInfo != null)
            {
                htmlHead.Controls.Add(new LiteralControl() { Text = layoutInfo.Css });
            }
        }

    }
}