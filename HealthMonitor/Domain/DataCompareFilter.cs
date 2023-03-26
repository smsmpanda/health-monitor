using System;

namespace HealthMonitor.Domain
{
    public class DataCompareFilter
    {
        public int Interval { get; set; }
        public DateTime CompareDate { get; set; } = DateTime.Now;
    }
}
