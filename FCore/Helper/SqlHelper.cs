using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FCore.Class;

namespace FCore.Helper
{
    public static class SqlHelper
    {
        /// <summary>
        /// Create Insert statements dynamically
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="fieldinfos"></param>
        /// <returns></returns>
        public static string GenerateInsertScript(string tablename, FieldInfo[] fieldinfos)
        {
            StringBuilder parametrs = new StringBuilder();
            StringBuilder columns = new StringBuilder();
            foreach (FieldInfo info in fieldinfos)
            {
                if (FormHelper.IsComponent(info))
                    continue;

                if (info.IsPrimaryKey)
                    continue;

                if (parametrs.Length > 0)
                    parametrs.Append(",");

                if (columns.Length > 0)
                    columns.Append(",");

                columns.AppendFormat("[{0}]", info.Name);
                parametrs.AppendFormat("@{0}", info.Name);

            }

            StringBuilder insertscript = new StringBuilder();
            insertscript.AppendFormat(" INSERT INTO {0}({1}) ", tablename, columns);
            insertscript.AppendFormat("VALUES ( {0} );", parametrs);
            insertscript.Append("SELECT  @@IDENTITY; ");
            return insertscript.ToString();
        }

        /// <summary>
        /// Create Update statements dynamically
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="fieldinfos"></param>
        /// <returns></returns>
        public static string GenerateUpdateScript(string tablename, FieldInfo[] fieldinfos)
        {
            StringBuilder parametrs = new StringBuilder();
            string primaryKey = string.Empty;

            foreach (FieldInfo info in fieldinfos)
            {
                if (FormHelper.IsComponent(info))
                    continue;

                if (info.IsPrimaryKey)
                {
                    primaryKey = string.Format("[{0}] = @{0}", info.Name);
                    continue;
                }

                if (parametrs.Length > 0)
                    parametrs.Append(",");

                parametrs.AppendFormat("[{0}] = @{0}", info.Name);
            }

            StringBuilder insertscript = new StringBuilder();
            insertscript.AppendFormat(" UPDATE {0} SET {1} WHERE {2}; ", tablename, parametrs, primaryKey);
            return insertscript.ToString();
        }


        /// <summary>
        /// Create Select ALL statements dynamically
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public static string GenerateSelectAllScript(string tablename)
        {
            StringBuilder insertscript = new StringBuilder();
            insertscript.AppendFormat(" SELECT * FROM {0} ORDER BY ModifiedDate DESC;", tablename);
            return insertscript.ToString();
        }

        /// <summary>
        /// Create Select statements dynamically
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public static string GenerateSelectScript(string tablename)
        {
            StringBuilder insertscript = new StringBuilder();
            insertscript.AppendFormat(" SELECT * FROM {0} WHERE Id=@id;", tablename);
            return insertscript.ToString();
        }

        /// <summary>
        /// Create Select statements dynamically
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public static string GenerateSelectBySeoTemplateScript(string tablename)
        {
            StringBuilder insertscript = new StringBuilder();
            insertscript.AppendFormat(" SELECT * FROM {0} WHERE SeoTemplate=@SeoTemplate;", tablename);
            return insertscript.ToString();
        }


        public static string GenerateSelectPagingScript(string tablename)
        {
            StringBuilder selectPaging = new StringBuilder();
            selectPaging.AppendFormat("WITH {0}RS AS( SELECT ROW_NUMBER() OVER(ORDER BY  ModifiedDate DESC) " +
                                "AS RowNum,* FROM {0} ) SELECT * FROM {0}RS WHERE RowNum " +
                                "BETWEEN (@PageIndex - 1) * @PageSize + 1 AND @PageIndex * @PageSize;", tablename);
            return selectPaging.ToString();
        }

        public static string GenerateSelectTotalCountScript(string tablename)
        {
            StringBuilder selectPaging = new StringBuilder();
            selectPaging.AppendFormat("SELECT COUNT(*) FROM {0}", tablename);
            return selectPaging.ToString();
        }

        /// <summary>
        /// Create Delete statements dynamically
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public static string GenerateDeleteScript(string tablename)
        {
            StringBuilder insertscript = new StringBuilder();
            insertscript.AppendFormat(" DELETE FROM {0} WHERE Id=@id;", tablename);
            return insertscript.ToString();
        }


        public static string GenerateTranslationInsertScript(List<LanguageInfo> languageInfos)
        {
            StringBuilder parametrs = new StringBuilder();
            StringBuilder columns = new StringBuilder();
            foreach (LanguageInfo info in languageInfos)
            {
                if (parametrs.Length > 0)
                    parametrs.Append(",");

                if (columns.Length > 0)
                    columns.Append(",");

                columns.AppendFormat("{0}", info.Code);
                parametrs.AppendFormat("@{0}", info.Code);

            }

            StringBuilder insertscript = new StringBuilder();
            insertscript.AppendFormat(" INSERT INTO Translation(Keyword,DefaultValue,{0})", columns);
            insertscript.AppendFormat(" VALUES (@Keyword,@DefaultValue,{0});", parametrs);
            return insertscript.ToString();
        }

        public static string GenerateTranslationUpdateScript(List<LanguageInfo> languageInfos)
        {
            StringBuilder parametrs = new StringBuilder();

            foreach (LanguageInfo info in languageInfos)
            {
                if (parametrs.Length > 0)
                    parametrs.Append(",");

                parametrs.AppendFormat("{0} = @{0}", info.Code);
            }

            StringBuilder insertscript = new StringBuilder();
            insertscript.AppendFormat(" UPDATE Translation SET Keyword=@Keyword,DefaultValue=@DefaultValue, {0} WHERE Id = @Id; ", parametrs);
            return insertscript.ToString();
        }

        public static string GenerateMetaTagsSelectScript(string tableName)
        {
            StringBuilder selectPaging = new StringBuilder();
            selectPaging.AppendFormat("SELECT [MetaTitle], [MetaDescription], [MetaKeywords], [CopyRights], [MetaImage],[ModifiedDate] FROM {0} WHERE SeoTemplate=@SeoTemplate;", tableName);
            return selectPaging.ToString();
        }
    }
}
