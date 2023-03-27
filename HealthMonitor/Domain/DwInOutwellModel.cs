using System;

namespace HealthMonitor.Domain
{
    public class DwInOutwellModel : ViewModelBase
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public string DepartMentName { get; set; }
        public string GroupClass { get; set; }
        public string EmployeeName { get; set; }
        public string TagMac { get; set; }
        public DateTime DwInwellTime { get; set; }
        public DateTime HmInwellTime { get; set; }
        public DateTime HmOutwellTime { get; set; }
        public string HmResult { get; set; } = "已出井";
    }
}
