using HealthMonitor.Utility;
using System.Threading.Tasks;
using System.Windows.Forms;
using HealthMonitorDbType = HealthMonitor.Enums.DbType;

namespace HealthMonitor.Services
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
                    return new MySqlDb(connectionString);
                case HealthMonitorDbType.MYSQL:
                    return new SQLServerDb(connectionString);
                default:
                    return null;
            }
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

        public static async Task<(bool health,string message)> DbConnectionTestAsync(DbConfig config) 
        {
            string connectionStr = CreateConnectionString(config);
            return await GetDbInstance(connectionStr, config.DbType).HealthCheckAsync();
        }
    }
}
