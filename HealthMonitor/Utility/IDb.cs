using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HealthMonitor.Utility
{
    public abstract class BaseDb
    {
        protected bool DbStatus { get; }
        protected string _connectionString;
        public BaseDb(string connectionString)
        {
            this._connectionString = connectionString;
        }


        public abstract IDbConnection CreateConnection();
        public bool HealthCheck()
        {
            IDbConnection conn = null;
            try
            {
                conn = CreateConnection();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                conn?.Close();
                conn?.Dispose();
            }
        }
    }

    public class OracleDb : BaseDb
    {
        public OracleDb(string connectionString) : base(connectionString)
        { }
        public override IDbConnection CreateConnection()
        {
            try
            {
                var conn = new OracleConnection(this._connectionString);
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
    public class SQLServerDb : BaseDb
    {
        public SQLServerDb(string connectionString) : base(connectionString)
        { }

        public override IDbConnection CreateConnection()
        {
            try
            {
                var conn = new SqlConnection(this._connectionString);
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
    public class MySqlDb : BaseDb
    {
        public MySqlDb(string connectionString) : base(connectionString)
        { }
        public override IDbConnection CreateConnection()
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
