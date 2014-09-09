using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FCore.Class;
using FCore.Enum;
using FDataProvider;

namespace FImportExport
{
    public class ImportExportProvider
    {
        private DataConnection _connection;

        private string connectionString = @"data source=Ulugbek-PC\sql2008;Initial Catalog=master; User Id=sa;Password=1;";

        public ImportExportProvider()
        {
            _connection = new DataConnection(connectionString);
        }

        public void CreateDatabase(string databaseName, string dbFileSource, ErrorInfo error)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("CREATE DATABASE [{0}] ON  PRIMARY ", databaseName);
            queryBuilder.Append("(");
            queryBuilder.AppendFormat("NAME = N'{0}', ", databaseName);
            queryBuilder.AppendFormat("FILENAME = N'{0}{1}.mdf', ", dbFileSource, databaseName);
            queryBuilder.Append("SIZE = 38912KB , ");
            queryBuilder.Append("MAXSIZE = UNLIMITED, ");
            queryBuilder.Append("FILEGROWTH = 1024KB )");
            queryBuilder.Append("LOG ON ");
            queryBuilder.AppendFormat("( NAME = N'{0}_log', ", databaseName);
            queryBuilder.AppendFormat("FILENAME = N'{0}{1}.ldf' , ", dbFileSource, databaseName);
            queryBuilder.Append("SIZE = 112384KB , ");
            queryBuilder.Append("MAXSIZE = 2048GB , ");
            queryBuilder.Append("FILEGROWTH = 10%)");

            _connection.ExecuteNonQuery(queryBuilder.ToString(), null, QueryType.SqlQuery, error);

        }

        public void CreateDefaultTables()
        {

        }

        public void CreateDefaultStoredProcedures()
        {

        }

    }
}
