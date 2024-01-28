using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Domain
{
    public interface IMonitorEntity
    {
        double PUSHINTERVAL { get; set; }
    }
}
