using HealthMonitor.Domain;
using HealthMonitor.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HealthMonitor.Services.PollingService
{
    public abstract class DefaultPollingMonitorService<T> : IPollingMonitorService<T> where T : class,IMonitorEntity
    {
        protected T Entity { get; set; }

        public async void ExecuteAsync(T model ,CancellationToken cancellationToken) 
        {
            this.Entity = model;

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
                    await Task.Delay(TimeSpan.FromSeconds(this.Entity.PUSHINTERVAL));
                }
            }
        }

        public abstract Task ExecuteHandleAsync();
    }
}
