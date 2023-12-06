using HealthMonitor.Model.Entity;
using HealthMonitor.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Services.imp
{
    public class ZhaoGuanAlarmMonitorService : DefaultAlarmMonitorService
    {
        public override async Task MonitorExecuteAsync()
        {
            var uniqueAlarmList = await this.GetUnqiueAlarmRecordAsync();
            if (uniqueAlarmList is null || !uniqueAlarmList.Any())
            {
                return;
            }

            IEnumerable<UniqueRecordEntity> uniqueList = await RYDWDbContext.GetUniqueReocrdByEmpIDAsync(uniqueAlarmList.Select(x => x.ALARMMANID).ToArray());

            var alarmList = uniqueAlarmList.Where(x => !uniqueList.Any(y => Math.Ceiling(Math.Abs(x.STARTDATE.Subtract(y.UniqueRecordTime).TotalMinutes)) <= ComparsionInterval));

            //转存并删除报警记录
            await this.TransferUniqueAlarmToHistoryAsync(alarmList.Select(x => x.ID).ToArray());
        }
    }
}
