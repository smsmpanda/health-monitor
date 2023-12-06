using HealthMonitor.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HealthMonitor.Services
{
    public class DefultPollingTaskService : IPollingTaskService
    {
        public virtual TimeSpan ExecuteInterval { get; set; } = TimeSpan.FromSeconds(5);

        public virtual async Task ExecuteHandleAsync(CancellationToken cancellationToken = default) => await Task.CompletedTask;

        public async Task ExecutorStartPolling(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await ExecuteHandleAsync();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
                finally
                {
                    await Task.Delay(ExecuteInterval);
                }
            }
        }
    }
}
