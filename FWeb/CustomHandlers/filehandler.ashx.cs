using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using FCore.Class;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;

namespace FWeb.CustomHandlers
{
    /// <summary>
    /// Summary description for filehandler
    /// </summary>
    public class filehandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int asarId = ValidationHelper.GetInteger(context.Request.QueryString["id"], 0);

            string fileName = GetFileName(asarId);

            string filePath = context.Server.MapPath("/website/" + fileName); //you know where your files are
            FileInfo file = new System.IO.FileInfo(filePath);
            if (file.Exists)
            {
                try
                {
                    Update(asarId);
                }
                catch (Exception)
                {

                }
                //return the file
                context.Response.Clear();
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                context.Response.AddHeader("Content-Length", file.Length.ToString());
                context.Response.ContentType = "application/octet-stream";
                context.Response.WriteFile(file.FullName);
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();
            }
        }

        private string GetFileName(int id)
        {
            using (DataConnection connection = new DataConnection())
            {
                string selectQuery = "SELECT fayl FROM kitobim_asar WHERE Id = @Id";
                var param = new object[1, 3];
                param[0, 0] = "Id";
                param[0, 1] = id;
                ErrorInfo error = new ErrorInfo();
                DataTable dataTable = connection.ExecuteDataTableQuery(selectQuery, param, QueryType.SqlQuery, error);
                if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0][0].ToString();
                }
                return string.Empty;
            }
        }

        private void Update(int id)
        {
            using (DataConnection connection = new DataConnection())
            {
                const string selectQuery = "UPDATE kitobim_asar SET DownloadCount = DownloadCount + 1 WHERE Id = @Id";
                var param = new object[1, 3];
                param[0, 0] = "Id";
                param[0, 1] = id;
                ErrorInfo error = new ErrorInfo();
                connection.ExecuteScalar(selectQuery, param, QueryType.SqlQuery, error);
                CacheHelper.DeleteAll("kitobim_asar.select_by_seo");
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}