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

        public async Task<IEnumerable<DwInOutwellModel>> GetInOutwellListByCompareDateAsync(DateTime inwellDatetime, DateTime outwellDatetime)
        {
            try
            {
                using (IDbConnection conn = DbConnection)
                {
                    return await conn.QueryAsync<DwInOutwellModel>(string.Format(CompareDwSql.QueryInOutwell,inwellDatetime,outwellDatetime));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
