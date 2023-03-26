using Dapper;
using HealthMonitor.Domain;
using HealthMonitor.SqlMaps;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HealthMonitor.Repository.imp
{
    public class DwRepository : IDwRepository
    {
        public IDbConnection DbConnection { get; }

        public DwRepository(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public async Task<IEnumerable<DwInOutwellModel>> GetInOutwellListByCompareDateAsync(DateTime compareDate)
        {
            using (IDbConnection conn = DbConnection)
            {
                try
                {
                    return await conn.QueryAsync<DwInOutwellModel>(CompareDwSql.QueryInOutwell, compareDate);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
