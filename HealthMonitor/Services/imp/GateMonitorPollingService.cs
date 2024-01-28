using HealthMonitor.Domain;
using HealthMonitor.Services.PollingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Services.imp
{
    public class GateMonitorPollingService : DefaultPollingMonitorService<GateMonitorModel>
    {
        public override async Task ExecuteHandleAsync()
        {
            await Console.Out.WriteLineAsync(this.Entity.NAME);
        }
    }
}
