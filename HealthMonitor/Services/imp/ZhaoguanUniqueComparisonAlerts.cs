using HealthMonitor.Model.Entity;
using HealthMonitor.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HealthMonitor.Services.imp
{
    public class ZhaoguanUniqueComparisonAlerts : DefaultUniqueComparisonAlerts
    {
        private static ulong ROLLID = ulong.MinValue;

        public override async Task ExecuteHandleAsync(CancellationToken cancellationToken = default)
        {
            //1.获取将要开始比对的数据编号
            ulong rollID = await RYDWDbContext.GetInexitWellMaxID();

            //1.1.监测数据无改变
            if (ROLLID == rollID)
            {
                return;
            }

            //2.主数据(定位入井数据)
            var inexitwellList = await RYDWDbContext.GetUniqueComparsionInexitWellData(rollID);
            if (inexitwellList is null || !inexitwellList.Any())
            {
                return;
            }

            //3.唯一性比对数据
            var uniqueRecordList = await RYDWDbContext.GetUniqueComparsionRecordData(rollID);
            if (uniqueRecordList is null || !uniqueRecordList.Any())
            {
                return;
            }

            //4.开始比对(1.不存在唯一性比对记录，2.存在唯一性记录，但时间不在范围指定时间范围内)
            List<AlarmRecordEntity> alarms = new List<AlarmRecordEntity>();
            foreach (var inexitWellItem in inexitwellList)
            {
                //4.1当前入井记录是否存在唯一性记录
                bool isMatch = uniqueRecordList.Any(x => x.EmpID == inexitWellItem.EmpID && Math.Ceiling(Math.Abs(inexitWellItem.LoginTime.Subtract(x.UniqueRecordTime).TotalMinutes)) <= ComparsionInterval);
                if (!isMatch)
                {
                    //4.2写入报警
                    alarms.Add(inexitWellItem.ToMapAlarmRecord());
                }
            }

            //5.写入报警记录
            //5.1.先删除当前职工的唯一性比对失败报警记录
            await RYDWDbContext.DeleteUniqueAlarmAsync(alarms.Select(x => x.ALARMMANID).ToArray());
            await RYDWDbContext.InsertAlarmBlukAsync(alarms.ToArray());
        }
    }
}
