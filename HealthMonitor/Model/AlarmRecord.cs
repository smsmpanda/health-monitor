using System;

namespace HealthMonitor.Model
{
    public class AlarmRecord
    {
        public string ALARMTYPE { get; private set; }
        public string DESCRIPTION { get; private set; }
        public string ALARMMANNAME { get; private set; }
        public DateTime STARTDATE { get; private set; }

        public static AlarmRecord GenerateAlarm(string alarmType, string description, string alarmName, DateTime startDate)
        {
            AlarmRecord record = new AlarmRecord();
            record.ALARMTYPE = alarmType;
            record.DESCRIPTION = description;
            record.STARTDATE = startDate;
            record.ALARMMANNAME = alarmName;
            return record;
        }
    }
}
