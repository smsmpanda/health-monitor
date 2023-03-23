using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Domain
{
    public class HmInOutwellModel : ViewModelBase
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string TagMac { get; set; }
        public string HmInwellTime { get; set; }
        public string HmOutwellTime { get; set; }
        public string HmResult { get; set; }
    }
}
