using System.Collections.Generic;
using System.Data;
using System.Text;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.PortalControl;

namespace FWeb.WebParts.custom.searchbox
{
    public partial class webpart : FWebPart
    {
        private static readonly string[] Alphabet = new[] { "A", "B", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "Z", "O’", "G’", "SH", "CH" };
        private const string Query = "kitobim_asar.bor_kitoblar";

        #region Methods
        protected override void LoadWebPart()
        {
            if (PageInfo.Id == 24 || PageInfo.Id == 25 || PageInfo.Id == 26 || PageInfo.Id == 27)
            {
                string value = GetSeoValue("letter", string.Empty);
                StringBuilder script = new StringBuilder();
                script.Append("<script>$(document).ready(function(){");
                script.AppendFormat("$(\"#txtSearch\").val('{0}');", value);
                script.Append("});</script>");
                Page.ClientScript.RegisterStartupScript(typeof(FWebPart), "restoresearchvalue", script.ToString());
            }
            LoadAlphabet();
        }


        private void LoadAlphabet()
        {
            using (GeneralConnection generalConnection = new GeneralConnection())
            {
                ErrorInfoList errors = new ErrorInfoList();

                DataTable dataTable = null;
                if (CacheHelper.Contains(Query))
                    dataTable = (DataTable)CacheHelper.Get(Query);

                if (dataTable == null)
                {
                    dataTable = generalConnection.ExecuteDataTableQuery(Query, null, QueryType.SqlQuery, errors);
                    if (dataTable != null)
                        CacheHelper.Add(Query, dataTable);
                }

                if (dataTable != null && dataTable.Rows.Count > 0)
                {

                    for (int i = 0; i < Alphabet.Length; i++)
                    {
                        if (dataTable.Columns.Contains(Alphabet[i].ToLower().Replace("’", "1")) && dataTable.Rows[0][Alphabet[i].ToLower().Replace("’", "1")].ToString() != "0")
                        {
                            ltlAlphabet.Text += string.Format("<li><a href=\"/search/byletter/barcha/{0}\">{1}</a></li>", Alphabet[i].ToLower().Replace("’", ""), Alphabet[i]);
                        }
                        else
                        {
                            ltlAlphabet.Text += string.Format("<li><span>{0}</span></li>", Alphabet[i]);
                        }
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        if (dataTable.Columns.Contains("n" + i) && dataTable.Rows[0]["n" + i].ToString() != "0")
                        {
                            ltlAlphabet.Text += string.Format("<li><a href=\"/search/byletter/barcha/{0}\">{0}</a></li>", i);
                        }
                        else
                        {
                            ltlAlphabet.Text += string.Format("<li><span>{0}</span></li>", i);
                        }
                    }
                }
            }
        }
        #endregion
    }
}