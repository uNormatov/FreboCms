using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FCore.Collection;
using FCore.Enum;
using FCore.Class;
using FCore.Helper;
using System.Configuration;

namespace FDataProvider
{
    public sealed class GeneralConnection : IDisposable
    {
        private readonly DataConnection _connection;
        private readonly QueryProvider _queryProvider;

        public GeneralConnection() : this(null) { }

        public GeneralConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                _connection = new DataConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            else
                _connection = new DataConnection();

            _queryProvider = new QueryProvider(_connection);
        }

        public void ExecuteNonQuery(string queryName, object[,] parameters, ErrorInfoList errors)
        {
            QueryInfo query = _queryProvider.SelectByName(queryName, errors);
            if (!errors.HasError() && query != null)
            {
                ErrorInfo error = new ErrorInfo();
                _connection.ExecuteNonQuery(query.Text, parameters, QueryType.SqlQuery, error);
                if (!error.Ok)
                {
                    errors.Add(error);
                }
            }
        }

        public object ExecuteScalar(string queryName, object[,] parameters, ErrorInfoList errors)
        {
            QueryInfo query = _queryProvider.SelectByName(queryName, errors);
            if (!errors.HasError() && query != null)
            {
                ErrorInfo error = new ErrorInfo();
                object result = _connection.ExecuteScalar(query.Text, parameters, QueryType.SqlQuery, error);
                if (!error.Ok)
                {
                    errors.Add(error);
                    return null;
                }
                return result;
            }
            return null;
        }

        public DataTable ExecuteDataTableQuery(string queryName, object[,] parameters, ErrorInfoList errors)
        {
            QueryInfo query = _queryProvider.SelectByName(queryName, errors);
            if (!errors.HasError() && query != null)
            {
                ErrorInfo error = new ErrorInfo();
                DataTable dataTable = _connection.ExecuteDataTableQuery(query.Text, parameters, QueryType.SqlQuery, error);
                if (!error.Ok)
                {
                    errors.Add(error);
                    return null;
                }
                return dataTable;

            }
            return null;
        }

        public void ExecuteNonQuery(string queryName, object[,] parameters, QueryType queryType, ErrorInfoList errors)
        {
            if (queryType == QueryType.SqlQuery)
                ExecuteNonQuery(queryName, parameters, errors);
            ErrorInfo error = new ErrorInfo();
            _connection.ExecuteNonQuery(queryName, parameters, queryType, error);
            if (!error.Ok)
                errors.Add(error);
        }

        public object ExecuteScalar(string queryName, object[,] parameters, QueryType queryType, ErrorInfoList errors)
        {
            if (queryType == QueryType.SqlQuery)
                return ExecuteScalar(queryName, parameters, errors);
            ErrorInfo error = new ErrorInfo();
            object result = _connection.ExecuteScalar(queryName, parameters, queryType, error);
            if (!error.Ok)
                errors.Add(error);
            return result;
        }

        public DataTable ExecuteDataTableQuery(string queryName, object[,] parameters, QueryType queryType, ErrorInfoList errors)
        {
            if (queryType == QueryType.SqlQuery)
                return ExecuteDataTableQuery(queryName, parameters, errors);
            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = _connection.ExecuteDataTableQuery(queryName, parameters, queryType, error);
            if (!error.Ok)
            {
                errors.Add(error);
            }
            return dataTable;
        }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
            if (_queryProvider != null)
                _queryProvider.Dispose();
            GC.Collect();
        }
    }
}
