﻿using HealthMonitor.Enums;
using System;

namespace HealthMonitor.Model.Entity
{
    public class UniqueInexitWellEntity
    {
        public long MaxID { get; set; }
        public long EmpID { get; set; }
        public string Name { get; set; }
        public string TagMac { get; set; }
        public string Department { get; set; }
        public DateTime LoginTime { get; set; }

        public AlarmRecordEntity ToMapAlarmRecord()
        {
            return AlarmRecordEntity.GenerateAlarm(AlarmType.ATP_UNIQUE_FAIL.ToString(), "唯一性验证失败", Name, LoginTime, EmpID, TagMac, Department);
        }
    }

    public class UniqueRecordEntity
    {
        public long EmpID { get; set; }
        public DateTime UniqueRecordTime { get; set; }
    }

    public struct MaxValueEntity
    {
        public ulong MaxID { get; set; }
    }

    public struct SuncunUniqueRecordEntity 
    {
        public string personid { get; set; }
        public string Name { get; set; }
        public string depname { get; set; }
        public DateTime times { get; set; }
        public int type { get; set; }
    }
}
