using HealthMonitor.Model.Entity;
using HealthMonitor.Repository;
using HealthMonitor.SqlMaps;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Services
{
    public interface IAlarmMonitService
    {
        Task<IEnumerable<AlarmRecordEntity>> GetUnqiueAlarmRecordAsync();

        Task CompareAsync();

        Task TransferUniqueAlarmToHistoryAsync(params long[] alarmId);
    }

    public abstract class DefaultAlarmMonitorService : IAlarmMonitService
    {
        public double ComparsionInterval
        {
            get
            {
                string intervalSetting = ConfigurationManager.AppSettings["UniqueComparsionInterval"].ToString();
                if (string.IsNullOrWhiteSpace(intervalSetting))
                {
                    return 30;
                }
                else
                {
                    return Convert.ToDouble(intervalSetting);
                }
            }
        }
        public abstract Task CompareAsync();

        /// <summary>
        /// 获取唯一性比对失败报警记录
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<AlarmRecordEntity>> GetUnqiueAlarmRecordAsync()
        {
            return await RYDWDbContext.GetUniqueAlarmRecordAsync();
        }

        /// <summary>
        /// 将唯一性比对失败实时报警记录转存至历史报警记录中
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task TransferUniqueAlarmToHistoryAsync(params long[] alarmId)
        {
            await RYDWDbContext.TransferAlarmRecordAsync(alarmId);
        }
    }

    public class ZhaoGuanAlarmMonitorService : DefaultAlarmMonitorService
    {
        public override async Task CompareAsync()
        {
            var uniqueAlarmList = await this.GetUnqiueAlarmRecordAsync();
            if (uniqueAlarmList is null || uniqueAlarmList.Any())
            {
                return;
            }

            IEnumerable<UniqueRecordEntity> uniqueList = await RYDWDbContext.GetUniqueReocrdByEmpIDAsync(uniqueAlarmList.Select(x => x.ALARMMANID).ToArray());

            var alarmList = uniqueAlarmList.Where(x => !uniqueList.Any(y => Math.Ceiling(Math.Abs(x.STARTDATE.Subtract(y.UniqueRecordTime).TotalMinutes)) <= ComparsionInterval));

            //转存并删除报警记录
            await this.TransferUniqueAlarmToHistoryAsync(alarmList.Select(x => x.ID).ToArray());
        }
    }

    public class SuncunAlarmMonitorService : DefaultAlarmMonitorService
    {
        public override async Task CompareAsync()
        {
            var uniqueAlarmList = await this.GetUnqiueAlarmRecordAsync();
            if (uniqueAlarmList is null || uniqueAlarmList.Any())
            {
                return;
            }

            IEnumerable<SuncunUniqueRecordEntity> uniqueList = await SCDbContext.GetUniqueRecordByEmployeeNameAsync(uniqueAlarmList.Select(x => x.ALARMMANNAME).ToArray());

            var alarmList = uniqueAlarmList.Where(x => !uniqueList.Any(y => Math.Ceiling(Math.Abs(x.STARTDATE.Subtract(y.times).TotalMinutes)) <= ComparsionInterval));

            //转存并删除报警记录
            await this.TransferUniqueAlarmToHistoryAsync(alarmList.Select(x => x.ID).ToArray());
        }
    }
}
