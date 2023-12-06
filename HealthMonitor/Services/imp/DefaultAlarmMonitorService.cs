using HealthMonitor.Model.Entity;
using HealthMonitor.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HealthMonitor.Services.imp
{
    public abstract class DefaultAlarmMonitorService : DefultPollingTaskService,IAlarmMonitService
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
        
        public override async Task ExecuteHandleAsync(CancellationToken cancellationToken = default)
        {
            await MonitorExecuteAsync();
        }

        /// <summary>
        /// 获取唯一性比对失败报警记录
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<AlarmRecordEntity>> GetUnqiueAlarmRecordAsync()
        {
            return await RYDWDbContext.GetUniqueAlarmRecordAsync();
        }

        public abstract Task MonitorExecuteAsync();


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
}
