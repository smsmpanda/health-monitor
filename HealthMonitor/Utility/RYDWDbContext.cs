using Dapper;
using HealthMonitor.Model;
using HealthMonitor.Services;
using HealthMonitor.SqlMaps;
using System;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace HealthMonitor.Utility
{
    public class RYDWDbContext
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["RYDWDb"].ConnectionString;

        /// <summary>
        /// 报警记录存储处理
        /// </summary>
        /// <param name="alarm"></param>
        /// <param name="istransfer">是否转存至报警记录</param>
        /// <returns></returns>
        public static async Task InsertAlarmAsync(AlarmRecord alarm)
        {
            try
            {
                using (IDbConnection conn = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
                {
                    var alarm_record = await conn.QueryFirstOrDefaultAsync<AlarmRecord>(AlarmSql.QueryAlarmSql, alarm);

                    if (alarm_record is null)
                    {
                        await conn.ExecuteAsync(AlarmSql.InsertAlarmSql, alarm);
                    }
                    else
                    {
                        await conn.ExecuteAsync(AlarmSql.UpdateAlarmByTypeSql, alarm);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task TransferAlarmRecordAsync(AlarmRecord alarm)
        {
            //转存至报警日志记录
            using (IDbConnection conn = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    //转存至报警历史记录中
                    await conn.ExecuteAsync(AlarmSql.TransferAlarmSql, alarm);
                    await conn.ExecuteAsync(AlarmSql.DeleteAlarmSql, alarm);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        public static async Task DeleteAlarmRecord(AlarmRecord alarm)
        {
            using (IDbConnection conn = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                await conn.ExecuteAsync(AlarmSql.DeleteAlarmSql, alarm);
            }
        }
    }
}
