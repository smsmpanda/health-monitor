using HealthMonitor.Model.Entity;
using HealthMonitor.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Services.imp
{
    public class SuncunAlarmMonitorService : DefaultAlarmMonitorService
    {
        public override async Task MonitorExecuteAsync()
        {
            var uniqueAlarmList = await this.GetUnqiueAlarmRecordAsync();
            if (uniqueAlarmList is null || !uniqueAlarmList.Any())
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
