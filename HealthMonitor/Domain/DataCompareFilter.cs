using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Domain
{
    public class DataCompareFilter
    {
        public int ValidInterval { get; set; }
        public DateTime InOutwellData { get; set; }
    }
}
