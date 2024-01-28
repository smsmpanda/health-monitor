using HealthMonitor.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HealthMonitor.Services.PollingService
{
    public interface IPollingMonitorService<T> where T : class, IMonitorEntity
    {
        void ExecuteAsync(T model, CancellationToken cancellationToken);
    }
}
