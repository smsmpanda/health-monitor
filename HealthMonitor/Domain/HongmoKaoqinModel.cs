using System;

namespace HealthMonitor.Domain
{
    public class HongmoKaoqinModel : ViewModelBase
    {
        public int EmployeeID { get; set; }
        public DateTime OnTime { get; set; }
        public DateTime OffTime { get; set; }
    }
}
