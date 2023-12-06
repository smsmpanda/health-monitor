using System;

namespace HealthMonitor.Model.Entity
{
    public class AlarmRecordEntity
    {
        public long ID { get; set; }
        public long ALARMMANID { get; set; }
        public string DEPARTMENT { get; set; }
        public string TAGMAC { get; set; }
        public string ALARMTYPE { get; set; }
        public string DESCRIPTION { get; set; }
        public string ALARMMANNAME { get; set; }
        public DateTime STARTDATE { get; set; }

        public static AlarmRecordEntity GenerateAlarm(string alarmType, string description, string alarmName, DateTime startDate)
        {
            AlarmRecordEntity record = new AlarmRecordEntity();
            record.ALARMTYPE = alarmType;
            record.DESCRIPTION = description;
            record.STARTDATE = startDate;
            record.ALARMMANNAME = alarmName;
            return record;
        }

        public static AlarmRecordEntity GenerateAlarm(string alarmType, string description, string alarmName, DateTime startDate, long alarmManID, string tagMac, string departMent)
        {
            AlarmRecordEntity record = new AlarmRecordEntity();
            record.ALARMTYPE = alarmType;
            record.DESCRIPTION = description;
            record.ALARMMANNAME = alarmName;
            record.STARTDATE = startDate;
            record.ALARMMANID = alarmManID;
            record.TAGMAC = tagMac;
            record.DEPARTMENT = departMent;
            return record;
        }
    }
}
