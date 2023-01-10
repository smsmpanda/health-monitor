using Dapper;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace HealthMonitor.Utility
{
    internal abstract class BaseDb
    {
        protected bool DbStatus { get; }
        protected string _connectionString;
        public BaseDb(string connectionString)
        {
            this._connectionString = connectionString;
        }
        public IDbConnection Connection => CreateDbConnection();

        protected abstract IDbConnection CreateDbConnection();
        public bool HealthCheck()
        {
            try
            {
                this.CreateDbConnection();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                this.Connection.Close();
                this.Connection.Dispose();
            }
        }
    }

    internal class OracleDb : BaseDb
    {
        public OracleDb(string connectionString) : base(connectionString)
        { }
        protected override IDbConnection CreateDbConnection()
        {
            try
            {
                var connection = new OracleConnection(this._connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    internal class SQLServerDb : BaseDb
    {
        public SQLServerDb(string connectionString) : base(connectionString)
        { }

        protected override IDbConnection CreateDbConnection()
        {
            try
            {
                var connection = new SqlConnection(this._connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    internal class MySqlDb : BaseDb
    {
        public MySqlDb(string connectionString) : base(connectionString)
        { }
        protected override IDbConnection CreateDbConnection()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection();
                conn.ConnectionString = this._connectionString;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
