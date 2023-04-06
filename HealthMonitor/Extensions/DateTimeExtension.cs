using System;

namespace HealthMonitor.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime MergeDateTime(this DateTime originalDate, DateTime contractDate)
        {
            DateTime mergedDateTime = new DateTime(originalDate.Year, originalDate.Month, originalDate.Day,
                contractDate.Hour, contractDate.Minute, contractDate.Second);
            return mergedDateTime;
        }
    }
}
