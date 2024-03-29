﻿using Dapper;
using Dapper.Contrib.Extensions;
using DapperExtensions;
using HealthMonitor.Domain;
using HealthMonitor.Enums;
using HealthMonitor.Model.Entity;
using HealthMonitor.SqlMaps;
using HealthMonitor.Utility;
using ImTools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Repository
{
    public class RYDWDbContext
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["RYDWDb"].ConnectionString;

        #region 报警记录
        /// <summary>
        /// 报警记录存储处理
        /// </summary>
        /// <param name="alarm"></param>
        /// <param name="istransfer">是否转存至报警记录</param>
        /// <returns></returns>
        public static async Task InsertAlarmAsync(AlarmRecordEntity alarm)
        {
            try
            {
                using (IDbConnection conn = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
                {
                    var alarm_record = await conn.QueryFirstOrDefaultAsync<AlarmRecordEntity>(AlarmSql.QueryAlarmSql, alarm);

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

        /// <summary>
        /// 批量插入报警记录
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        public static async Task InsertAlarmBlukAsync(params AlarmRecordEntity[] alarms)
        {
            try
            {
                using (IDbConnection conn = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
                {
                    StringBuilder sqlAppender = new StringBuilder();

                    alarms.ForEach(a =>
                    {
                        sqlAppender.Append($" INTO TB_EMP_REALTIME_ALARM(ALARMTYPE,ALARMMANID,ALARMMANNAME, TAGMAC, DEPARTMENT,DESCRIPTION,STARTDATE) VALUES('{a.ALARMTYPE}',{a.ALARMMANID},'{a.ALARMMANNAME}','{a.TAGMAC}','{a.DEPARTMENT}','{a.DESCRIPTION}',to_date('{a.STARTDATE}', 'yyyy-MM-dd HH24:mi:ss'))");
                    });

                    string blukSql = string.Format(AlarmSql.InsertAlarmBlukSql, sqlAppender.ToString());
                    Logger.Error($"批量SQL校验：{blukSql}");

                    await conn.ExecuteAsync(blukSql);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task DeleteUniqueAlarmAsync(params long[] empIds) 
        {
            if (!empIds.Any())
                return;

            using (IDbConnection conn = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                await conn.ExecuteAsync(string.Format(UniqueComparsionSql.DeleteUniqueAlarm,AlarmType.ATP_UNIQUE_FAIL,string.Join(",", empIds.Select(x => $"'{x}'"))));
            }
        }

        /// <summary>
        /// 转存报警记录至历史报警记录
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        public static async Task TransferAlarmRecordAsync(AlarmRecordEntity alarm)
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

        /// <summary>
        /// 删除报警记录
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        public static async Task DeleteAlarmRecord(AlarmRecordEntity alarm)
        {
            using (IDbConnection conn = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                await conn.ExecuteAsync(AlarmSql.DeleteAlarmSql, alarm);
            }
        }
        #endregion

        #region 唯一性比对报警
        /// <summary>
        /// 获取唯一性比对出入井记录
        /// </summary>
        /// <param name="rollID"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<UniqueInexitWellEntity>> GetUniqueComparsionInexitWellData(ulong rollID)
        {
            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                return await connection.QueryAsync<UniqueInexitWellEntity>(string.Format(UniqueComparsionSql.GetInExitWellDataByStartID, rollID));
            }
        }

        /// <summary>
        /// 获取唯一性比对记录
        /// </summary>
        /// <param name="rollID"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<UniqueRecordEntity>> GetUniqueComparsionRecordData(ulong rollID)
        {
            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                return await connection.QueryAsync<UniqueRecordEntity>(string.Format(UniqueComparsionSql.GetUniqueData, rollID));
            }
        }

        /// <summary>
        /// 获取定位条件范围内的出入井数据Max ID
        /// </summary>
        /// <returns></returns>
        public static async Task<ulong> GetInexitWellMaxID()
        {
            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                var entity = await connection.QueryFirstAsync<MaxValueEntity>(UniqueComparsionSql.GetInExitWellStartID);
                return entity.MaxID;
            }
        }

        /// <summary>
        /// 获取唯一性比对失败报警记录
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<AlarmRecordEntity>> GetUniqueAlarmRecordAsync() 
        {
            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                return await connection.QueryAsync<AlarmRecordEntity>(UniqueComparsionSql.GetUniqueAlarmData,AlarmType.ATP_UNIQUE_FAIL);
            }
        }

        /// <summary>
        /// 获取唯一性通过记录
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<UniqueRecordEntity>> GetUniqueReocrdByEmpIDAsync(params long[] empIds) 
        {
            if (!empIds.Any()) 
                return Enumerable.Empty<UniqueRecordEntity>();

            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                return await connection.QueryAsync<UniqueRecordEntity>(UniqueComparsionSql.GetUniqueReocrdByEmpID, string.Join(",", empIds.Select(x => $"'{x}'")));
            }
        }
        
        /// <summary>
        /// 转存报警记录至历史报警记录
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        public static async Task TransferAlarmRecordAsync(params long[] alarmIds)
        {
            //转存至报警日志记录
            using (IDbConnection conn = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    //转存至报警历史记录中
                    var alarmIdArray = string.Join(",", alarmIds.Select(x => $"{x}"));
                    await conn.ExecuteAsync(string.Format(UniqueComparsionSql.TransferAlarmSql,AlarmType.ATP_UNIQUE_FAIL,alarmIdArray));
                    await conn.ExecuteAsync(string.Format(UniqueComparsionSql.DeleteUniqueAlarmById, AlarmType.ATP_UNIQUE_FAIL, alarmIdArray));
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        #endregion

        #region 限员屏 & 闸门
        /// <summary>
        /// 获取限员屏信息
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<PassengerScreenEntity>> GetPassengerLimitScreensAsync()
        {
            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync()) 
            {
                return await connection.QueryAsync<PassengerScreenEntity>(PassenagerLimitSql.GetList);
            }
        }

        /// <summary>
        /// 新增屏幕信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<int> InsertPassengerLimitScreenAsync(PassengerScreenEntity model) 
        {
            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                return await connection.ExecuteAsync(PassenagerLimitSql.Insert,model);
            }
        }

        /// <summary>
        /// 更新屏幕信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<int> UpdatePassengerLimitScreenAsync(PassengerScreenEntity model)
        {
            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                return await connection.ExecuteAsync(PassenagerLimitSql.Update,model);
            }
        }

        /// <summary>
        /// 删除屏幕信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<int> DeletePassengerLimitAsync(int id)
        {
            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                return await connection.ExecuteAsync(PassenagerLimitSql.Delete,new { ID=id });
            }
        }

        /// <summary>
        /// 获取闸门列表
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<GateEntity>> GetGatesAsync()
        {
            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                return await connection.QueryAsync<GateEntity>(GateSql.GetGateSql);
            }
        }

        /// <summary>
        /// 新增闸门信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<int> InsertGateAsync(GateEntity model)
        {
            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                return await connection.ExecuteAsync(GateSql.Insert, model);
            }
        }

        /// <summary>
        /// 更新闸门信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<int> UpdateGateAsync(GateEntity model)
        {
            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                return await connection.ExecuteAsync(GateSql.Update, model);
            }
        }

        /// <summary>
        /// 删除闸门信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<int> DeleteGateAsync(int ID)
        {
            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                return await connection.ExecuteAsync(GateSql.Delete, new { ID = ID });
            }
        }

        /// <summary>
        /// 获取区域信息
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<AreaModel>> GetAreasAsync()
        {
            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                return await connection.QueryAsync<AreaModel>(AreaSql.Get);
            }
        }

        public static async Task<AreaRealDataEntity> GetAreaRealDataAsync(int areaID) 
        {
            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                return await connection.QueryFirstAsync<AreaRealDataEntity>(AreaSql.GetAreaRealData,new { ID = areaID });
            }
        }

        public static async Task<IEnumerable<AreaRealEmployeeEntity>> GetAreaRealEmployeesAsync(int areaID) 
        {
            using (IDbConnection connection = await DbFactory.GetDbInstance(_connectionString, Enums.DbType.ORACLE).CreateConnectionAsync())
            {
                return await connection.QueryAsync<AreaRealEmployeeEntity>(AreaSql.GetRealAreaEmployee, new { AreaID = areaID });
            }
        }
        #endregion
    }
}
