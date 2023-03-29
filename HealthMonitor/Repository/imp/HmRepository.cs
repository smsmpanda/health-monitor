﻿using Dapper;
using HealthMonitor.Domain;
using HealthMonitor.SqlMaps;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HealthMonitor.Repository.imp
{
    public class HmRepository : IHmRepository
    {
        public IDbConnection DbConnection { get; }

        public HmRepository(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public async Task<IEnumerable<HongmoKaoqinModel>> GetHongMoKaoqinListByCompareDateAsync(DateTime compareDate)
        {
            try
            {
                using (IDbConnection conn = DbConnection)
                {
                    return await conn.QueryAsync<HongmoKaoqinModel>(string.Format(CompareHmSql.QueryKaoqin, compareDate, compareDate.AddDays(1)));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
