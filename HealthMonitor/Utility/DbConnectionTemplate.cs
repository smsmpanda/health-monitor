using HealthMonitor.Model.Common;

namespace HealthMonitor.Utility
{
    /// <summary>
    /// 数据库连接模板
    /// </summary>
    public class DbConnectionTemplate
    {
        public const string MYSQL = "Data Source={0};Port={1};Initial Catalog={2};User Id={3};Password={4};";

        public const string MSSQL = "Server={0},{1};Database={2};uid={3};pwd={4};";

        public const string ORACLE = "Data Source={0}:{1}/{2};Persist Security Info=True;User ID={3};Password={4};Connection Timeout=3;";


        public static string CreateOracleString(DbConfig config)
        {
            return string.Format(ORACLE, config.Host, config.Port, config.DbCatalog, config.User, config.Password);
        }

        public static string CreateMySQLString(DbConfig config)
        {
            return string.Format(MYSQL, config.Host, config.Port, config.DbCatalog, config.User, config.Password);
        }

        public static string CreateMSSQLString(DbConfig config)
        {
            return string.Format(MSSQL, config.Host, config.Port, config.DbCatalog, config.User, config.Password);
        }
    }
}
