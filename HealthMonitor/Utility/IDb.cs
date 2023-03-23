using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

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


        public abstract Task<IDbConnection> CreateConnectionAsync();
        public async Task<(bool health,string message)> HealthCheckAsync()
        {
            IDbConnection conn = null;
            try
            {
                conn = await CreateConnectionAsync();
                return (true,"连接成功");
            }
            catch (Exception ex)
            {
                return (false,ex.Message);
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
        public override async Task<IDbConnection> CreateConnectionAsync()
        {
            try
            {
                var conn = new OracleConnection(this._connectionString);
                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync();
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

        public override async Task<IDbConnection> CreateConnectionAsync()
        {
            try
            {
                var conn = new SqlConnection(this._connectionString);
                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync();
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
        public override async Task<IDbConnection> CreateConnectionAsync()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection();
                conn.ConnectionString = this._connectionString;
                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync();
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
