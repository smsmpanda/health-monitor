using HealthMonitor.Model.Common;
using System.Data;
using System.Threading.Tasks;
using HealthMonitorDbType = HealthMonitor.Enums.DbType;

namespace HealthMonitor.Utility
{
    internal class DbFactory
    {
        public static BaseDb GetDbInstance(string connectionString, HealthMonitorDbType dbType)
        {
            switch (dbType)
            {
                case HealthMonitorDbType.ORACLE:
                    return new OracleDb(connectionString);
                case HealthMonitorDbType.MSSQL:
                    return new SQLServerDb(connectionString);
                case HealthMonitorDbType.MYSQL:
                    return new MySqlDb(connectionString);
                default:
                    return null;
            }
        }

        public static async Task<IDbConnection> GetDbConnectionAsync(DbConfig config)
        {
            string connectionStr = CreateConnectionString(config);
            return await GetDbInstance(connectionStr, config.DbType).CreateConnectionAsync();
        }

        public static string CreateConnectionString(DbConfig config)
        {
            switch (config.DbType)
            {
                case HealthMonitorDbType.ORACLE:
                    return DbConnectionTemplate.CreateOracleString(config);
                case HealthMonitorDbType.MSSQL:
                    return DbConnectionTemplate.CreateMSSQLString(config);
                case HealthMonitorDbType.MYSQL:
                    return DbConnectionTemplate.CreateMySQLString(config);
                default:
                    return null;
            }
        }

        public static async Task<(bool health, string message)> DbConnectionTestAsync(DbConfig config)
        {
            string connectionStr = CreateConnectionString(config);
            return await GetDbInstance(connectionStr, config.DbType).HealthCheckAsync();
        }
    }
}
