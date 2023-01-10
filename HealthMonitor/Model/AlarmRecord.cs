using System;

namespace HealthMonitor.Model
{
    public class AlarmRecord
    {
        public string ALARMTYPE { get; private set; }
        public string DESCRIPTION { get; private set; }
        public DateTime STARTDATE { get; private set; }

        public AlarmRecord(string alarmType, string description, DateTime startDate)
        {
            ALARMTYPE = alarmType;
            DESCRIPTION = description;
            STARTDATE = startDate;
        }
    }
}
