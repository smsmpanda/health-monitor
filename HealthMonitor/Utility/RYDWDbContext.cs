using Dapper;
using HealthMonitor.Model;
using HealthMonitor.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Utility
{
    public class RYDWDbContext
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["RYDWDb"].ConnectionString;
        public static async Task InsertAlarmAsync(AlarmRecord alarm)
        {
            try
            {
                using (IDbConnection conn = DbFactory.GetDbByType(_connectionString, Enums.DbType.ORACLE).Connection)
                {
                    await conn.ExecuteAsync("INSERT INTO TB_EMP_REALTIME_ALARM(ALARMTYPE,DESCRIPTION,STARTDATE) VALUES(:ALARMTYPE, :DESCRIPTION, :STARTDATE)", alarm);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
