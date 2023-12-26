using Dapper;
using HealthMonitor.Model.Entity;
using HealthMonitor.SqlMaps;
using HealthMonitor.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Repository
{
    public class SCDbContext
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["SCMySQLDb"].ConnectionString;

        public static async Task<IEnumerable<SuncunUniqueRecordEntity>> GetUniqueRecordByTimeAsync(DateTime time) 
        {
            try
            {
                using (IDbConnection conn = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.MYSQL).CreateConnectionAsync())
                {
                    return await conn.QueryAsync<SuncunUniqueRecordEntity>(string.Format(UniqueComparsionSql.SuncunUniqueRecord, $"{time:yyyy-MM-dd HH:mm:ss}"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static async Task<IEnumerable<SuncunUniqueRecordEntity>> GetUniqueRecordByEmployeeNameAsync(params string[]employeeNames)
        {
            if(!employeeNames.Any())
            {
                return Enumerable.Empty<SuncunUniqueRecordEntity>();
            }

            try
            {
                using (IDbConnection conn = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.MYSQL).CreateConnectionAsync())
                {
                    return await conn.QueryAsync<SuncunUniqueRecordEntity>(string.Format(UniqueComparsionSql.SuncunUniqueRecordByName, string.Join(",",employeeNames.Select(x => $"'{x}'"))));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
