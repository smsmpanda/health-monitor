using HealthMonitor.Model.Entity;
using HealthMonitor.Repository;
using HealthMonitor.Services.imp;
using HealthMonitor.SqlMaps;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HealthMonitor.Services
{
    public interface IAlarmMonitService: IPollingTaskService
    {
        Task<IEnumerable<AlarmRecordEntity>> GetUnqiueAlarmRecordAsync();

        Task MonitorExecuteAsync();

        Task TransferUniqueAlarmToHistoryAsync(params long[] alarmId);
    }
}
