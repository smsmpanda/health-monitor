using HealthMonitor.Enums;
using HealthMonitor.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Services
{
    internal class DbFactory
    {
        public static BaseDb GetDbByType(string connectionString, Enums.DbType dbType)
        {
            switch (dbType)
            {
                case Enums.DbType.ORACLE:
                    return new OracleDb(connectionString);
                case Enums.DbType.MSSQL:
                    return new MySqlDb(connectionString);
                case Enums.DbType.MYSQL:
                    return new SQLServerDb(connectionString);
                default:
                    return null;
            }
        }
    }
}
