using HealthMonitor.Model.Entity;
using HealthMonitor.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1Async()
        {
            List<AlarmRecordEntity> alarms = new List<AlarmRecordEntity>()
            {
                new AlarmRecordEntity
                {
                    ALARMMANID = 1001,
                    ALARMMANNAME = "test",
                    ALARMTYPE = "test",
                    DEPARTMENT = "test",
                    DESCRIPTION = "test",
                    ID = 1021,
                    STARTDATE = DateTime.Now,
                    TAGMAC = "A111100",
                },
                new AlarmRecordEntity
                {
                    ALARMMANID = 1002,
                    ALARMMANNAME = "test",
                    ALARMTYPE = "test",
                    DEPARTMENT = "test",
                    DESCRIPTION = "test",
                    ID = 1021,
                    STARTDATE = DateTime.Now,
                    TAGMAC = "A111100",
                }
            };
            await RYDWDbContext.InsertAlarmBlukAsync(alarms.ToArray());

            //await RYDWDbContext.DeleteUniqueAlarmAsync(alarms.Select(x => x.ALARMMANID).ToArray());
        }
    }
}
