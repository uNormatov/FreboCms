using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using FCore.Enum;
using System.Configuration;
using FCore.Class;

namespace FDataProvider
{
    public class DataConnection : IDisposable
    {
        #region Variables
        private SqlTransaction _transaction = null;
        private string _connectionstring;
        #endregion

        #region Properties

        public bool AllowTransaction
        {
            get;
            set;
        }

        public IsolationLevel IsolationLevel
        {
            get;
            set;
        }

        public virtual string ConnectionString
        {
            get
            {
                if (String.IsNullOrEmpty(_connectionstring))
                {
                    return ConfigurationManager.ConnectionStrings["connectionStrings"].ConnectionString;
                }
                else
                {
                    return _connectionstring;
                }
            }
            set
            {
                _connectionstring = value;
            }
        }

        #endregion

        #region Constructors
        public DataConnection() : this(null) { }
        public DataConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                this.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            else
                this.ConnectionString = connectionString;

        }
        #endregion

        #region Methods

        /// <summary>
        /// Creating a DataTable from a Query Text
        /// </summary>
        /// <param name="queryText">Query text</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="querytype">Type of query</param>
        public virtual DataTable ExecuteDataTableQuery(string queryText, object[,] parameters, QueryType querytype, ErrorInfo log)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {

                    // SqlCommand yaratildi

                    SqlCommand command = new SqlCommand();

                    //SqlCommand tipi kursatildi
                    if (querytype == QueryType.SqlQuery)
                        command.CommandType = CommandType.Text;
                    else if (querytype == QueryType.StoredProcedure)
                        command.CommandType = CommandType.StoredProcedure;

                    // kelgan parametrlar null mas
                    if (parameters != null)
                    {
                        int i = 0;
                        for (i = parameters.GetLowerBound(0); i <= parameters.GetUpperBound(0); i++)
                        {
                            string paramname = parameters.GetValue(i, 0).ToString();
                            object paramvalue = parameters.GetValue(i, 1);
                            if (!string.IsNullOrEmpty(paramname))
                            {
                                //parametrlar qo'shildi
                                if (parameters.GetValue(i, 2) != null)
                                {
                                    SqlParameter parametr = new SqlParameter();
                                    parametr.ParameterName = paramname;
                                    parametr.Value = paramvalue;
                                    parametr.SqlDbType = (SqlDbType)parameters.GetValue(i, 2);
                                    command.Parameters.Add(parametr);
                                }
                                else
                                {
                                    command.Parameters.Add(paramname, paramvalue);
                                }
                            }
                        }
                    }
                    //connection e'lon qilindi
                    command.Connection = connection;
                    command.CommandText = queryText;


                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    // Data adapter tayyor
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = command;
                    //dataset tuldirildi
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);
                    command.Dispose();
                    log.Ok = true;
                    return dt;
                }
            }

            catch (Exception ex)
            {
                log.Ok = false;
                log.Message = ex.Message;
                log.Source = ex.Source;
                if (ex.InnerException != null)
                    log.InnerMessage = ex.InnerException.Message;

                return new DataTable();
            }
            finally
            {
            }


        }

        /// <summary>
        /// Creating a DataSet from a Query Text
        /// </summary>
        /// <param name="queryText">Query text</param>
        /// <param name="parameters">Queryda ishlatiladigan parametrlar</param>
        /// <param name="querytype">Queryning tipi</param>
        public virtual System.Data.DataSet ExecuteDataSetQuery(string queryText, object[,] parameters, QueryType querytype, ErrorInfo log)
        {
            bool commitTransaction = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {

                    // SqlCommand yaratildi

                    SqlCommand command = new SqlCommand();
                    //SqlCommand tipr kursatildi
                    if (querytype == QueryType.SqlQuery)
                        command.CommandType = CommandType.Text;
                    else if (querytype == QueryType.StoredProcedure)
                        command.CommandType = CommandType.StoredProcedure;

                    // kelgan parametrlar null mas
                    if (parameters != null)
                    {
                        int i = 0;
                        for (i = parameters.GetLowerBound(0); i <= parameters.GetUpperBound(0); i++)
                        {
                            string paramname = parameters.GetValue(i, 0).ToString();
                            object paramvalue = parameters.GetValue(i, 1);
                            if (!string.IsNullOrEmpty(paramname))
                            {
                                //parametrlar qo'shildi
                                if (parameters.GetValue(i, 2) != null)
                                {
                                    SqlParameter parametr = new SqlParameter();
                                    parametr.ParameterName = paramname;
                                    parametr.Value = paramvalue;
                                    parametr.SqlDbType = (SqlDbType)parameters.GetValue(i, 2);
                                    command.Parameters.Add(parametr);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue(paramname, paramvalue);
                                }
                            }
                        }
                    }
                    //connection e'lon qilindi
                    command.Connection = connection;
                    command.CommandText = queryText;


                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    if (this.AllowTransaction)
                    {
                        command.Transaction = this._transaction;
                        _transaction = connection.BeginTransaction();
                        commitTransaction = true;
                    }

                    // Data adapter tayyor
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = command;

                    //dataset tuldirildi
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    if (commitTransaction)
                    {
                        CommitTransaction();
                        commitTransaction = false;
                    }
                    log.Ok = true;
                    return ds;
                }
            }
            catch (Exception ex)
            {
                log.Ok = false;
                log.Message = ex.Message;
                log.Source = ex.Source;
                if (ex.InnerException != null)
                    log.InnerMessage = ex.InnerException.Message;
                return new DataSet();
            }
            finally
            {
                if (commitTransaction)
                    RollBackTransaction();

                if (this.AllowTransaction)
                    this.AllowTransaction = false;
            }
        }

        /// <summary>
        /// Creating a SqlDataReader from a Query Text
        /// </summary>
        /// <param name="queryText">Query text</param>
        /// <param name="parameters">Queryda ishlatiladigan parametrlar</param>
        /// <param name="querytype">Queryning tipi</param>
        /// <param name="commandbehavior"></param>
        public virtual SqlDataReader ExecuteReader(string queryText, object[,] parameters, QueryType querytype, System.Data.CommandBehavior commandbehavior, ErrorInfo log)
        {
            SqlDataReader reader = null;
            try
            {
                SqlConnection connection = new SqlConnection(this.ConnectionString);
                using (SqlCommand command = new SqlCommand())
                {
                    //SqlCommand tipr kursatildi
                    if (querytype == QueryType.SqlQuery)
                        command.CommandType = CommandType.Text;
                    else if (querytype == QueryType.StoredProcedure)
                        command.CommandType = CommandType.StoredProcedure;

                    // kelgan parametrlar null mas
                    if (parameters != null)
                    {
                        int i = 0;
                        for (i = parameters.GetLowerBound(0); i <= parameters.GetUpperBound(0); i++)
                        {
                            string paramname = parameters.GetValue(i, 0).ToString();
                            object paramvalue = parameters.GetValue(i, 1);
                            if (!string.IsNullOrEmpty(paramname))
                            {
                                //parametrlar qo'shildi
                                if (parameters.GetValue(i, 2) != null)
                                {
                                    SqlParameter parametr = new SqlParameter();
                                    parametr.ParameterName = paramname;
                                    parametr.Value = paramvalue;
                                    parametr.SqlDbType = (SqlDbType)parameters.GetValue(i, 2);
                                    command.Parameters.Add(parametr);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue(paramname, paramvalue);
                                }
                            }
                        }
                    }
                    //connection e'lon qilindi
                    command.Connection = connection;
                    command.CommandText = queryText;
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    reader = command.ExecuteReader(commandbehavior);
                    log.Ok = true;

                }
            }

            catch (Exception ex)
            {
                log.Ok = false;
                log.Message = ex.Message;
                log.Source = ex.Source;
                if (ex.InnerException != null)
                    log.InnerMessage = ex.InnerException.Message;
                return null;
            }
            finally
            {
                //  if (reader != null) reader.Close();
            }
            return reader;
        }

        /// <summary>
        /// Returning Scalar variable from a Query Text
        /// </summary>
        /// <param name="queryText">Query text</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="querytype">Type of Query</param>
        public virtual object ExecuteScalar(string queryText, object[,] parameters, QueryType querytype, ErrorInfo log)
        {

            bool commitTransaction = false;
            try
            {
                // SqlCommand yaratildi
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    SqlCommand command = new SqlCommand();

                    //SqlCommand tipr kursatildi
                    if (querytype == QueryType.SqlQuery)
                        command.CommandType = CommandType.Text;
                    else if (querytype == QueryType.StoredProcedure)
                        command.CommandType = CommandType.StoredProcedure;

                    // kelgan parametrlar null mas
                    if (parameters != null)
                    {
                        int i = 0;
                        for (i = parameters.GetLowerBound(0); i <= parameters.GetUpperBound(0); i++)
                        {
                            string paramname = parameters.GetValue(i, 0).ToString();
                            object paramvalue = parameters.GetValue(i, 1);
                            if (!string.IsNullOrEmpty(paramname))
                            {
                                //parametrlar qo'shildi
                                if (parameters.GetValue(i, 2) != null)
                                {
                                    SqlParameter parametr = new SqlParameter();
                                    parametr.ParameterName = paramname;
                                    parametr.Value = paramvalue;
                                    parametr.SqlDbType = (SqlDbType)parameters.GetValue(i, 2);
                                    command.Parameters.Add(parametr);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue(paramname, paramvalue);
                                }
                            }
                        }
                    }
                    //connection e'lon qilindi
                    command.Connection = connection;
                    command.CommandText = queryText;

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    if (this.AllowTransaction)
                    {
                        command.Transaction = this._transaction;
                        _transaction = connection.BeginTransaction();
                        commitTransaction = true;
                    }

                    // Command tayyor
                    object result = command.ExecuteScalar();

                    if (commitTransaction)
                    {
                        CommitTransaction();
                        commitTransaction = false;
                    }
                    command.Dispose();
                    log.Ok = true;
                    return result;
                }

            }
            catch (Exception ex)
            {
                log.Ok = false;
                log.Message = ex.Message;
                log.Source = ex.Source;
                if (ex.InnerException != null)
                    log.InnerMessage = ex.InnerException.Message;
                return -1;
            }
            finally
            {
                if (commitTransaction)
                    RollBackTransaction();

                if (this.AllowTransaction)
                    this.AllowTransaction = false;

            }
        }

        /// <summary>
        /// NonQuery from a Query Text
        /// </summary>
        /// <param name="queryText">Query text</param>
        /// <param name="parameters">Queryda ishlatiladigan parametrlar</param>
        /// <param name="querytype">Queryning tipi</param>
        public virtual object ExecuteNonQuery(string queryText, object[,] parameters, QueryType querytype, ErrorInfo log)
        {
            bool commitTransaction = false;
            try
            {
                // SqlCommand yaratildi
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    SqlCommand command = new SqlCommand();

                    //SqlCommand tipr kursatildi
                    if (querytype == QueryType.SqlQuery)
                        command.CommandType = CommandType.Text;
                    else if (querytype == QueryType.StoredProcedure)
                        command.CommandType = CommandType.StoredProcedure;

                    // kelgan parametrlar null mas
                    if (parameters != null)
                    {
                        int i = 0;
                        for (i = parameters.GetLowerBound(0); i <= parameters.GetUpperBound(0); i++)
                        {
                            string paramname = parameters.GetValue(i, 0).ToString();
                            object paramvalue = parameters.GetValue(i, 1);
                            if (!string.IsNullOrEmpty(paramname))
                            {
                                //parametrlar qo'shildi
                                if (parameters.GetValue(i, 2) != null)
                                {
                                    SqlParameter parametr = new SqlParameter();
                                    parametr.ParameterName = paramname;
                                    parametr.Value = paramvalue;
                                    parametr.SqlDbType = (SqlDbType)parameters.GetValue(i, 2);
                                    command.Parameters.Add(parametr);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue(paramname, paramvalue);
                                }
                            }
                        }
                    }
                    //connection e'lon qilindi
                    command.Connection = connection;
                    command.CommandText = queryText;
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    if (this.AllowTransaction)
                    {
                        command.Transaction = this._transaction;
                        _transaction = connection.BeginTransaction();
                        commitTransaction = true;
                    }

                    object result = command.ExecuteNonQuery();

                    if (commitTransaction)
                    {
                        CommitTransaction();
                        commitTransaction = false;
                    }
                    command.Dispose();
                    log.Ok = true;
                    return result;
                }
            }

            catch (SqlException ex)
            {
                log.Ok = false;
                log.Message = ex.Message;
                log.Source = ex.Source;
                if (ex.InnerException != null)
                    log.InnerMessage = ex.InnerException.Message;
                return 0;
            }
            finally
            {
                if (commitTransaction)
                    RollBackTransaction();

                if (this.AllowTransaction)
                    this.AllowTransaction = false;
            }
        }

        /// <summary>
        /// Commit transaction
        /// </summary>
        public void CommitTransaction()
        {
            _transaction.Commit();
        }

        /// <summary>
        /// RollBack
        /// </summary>
        public void RollBackTransaction()
        {
            _transaction.Rollback();
        }

        /// <summary>
        /// Get XML Scheme of a table
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public string GetXMLSchema(string tablename)
        {

            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(string.Format("SELECT TOP 1 * FROM {0};", tablename), conn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                // Get xml schema
                da.FillSchema(ds, SchemaType.Mapped, tablename);
                string xmlSchema = ds.GetXmlSchema();

                if ((xmlSchema != null) && (xmlSchema != ""))
                {
                    xmlSchema = xmlSchema.Replace("utf-16", "utf-8");
                }
                else
                {
                    xmlSchema = "";
                }

                return xmlSchema;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
