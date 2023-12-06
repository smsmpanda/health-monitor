using System.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HealthMonitor.Services
{
    public interface ITaskPanelExecuteService
    {
        Task HandlerAsync(CancellationToken cancellationToken = default);
    }

    public abstract class DefaultUniqueComparisonAlerts : ITaskPanelExecuteService
    {

        public double ComparsionInterval
        {
            get
            {
                string intervalSetting = ConfigurationManager.AppSettings["UniqueComparsionInterval"].ToString();
                if (string.IsNullOrWhiteSpace(intervalSetting))
                {
                    return 30;
                }
                else
                {
                    return Convert.ToDouble(intervalSetting);
                }
            }
        }

        /// <summary>
        /// 定时任务执行间隔
        /// </summary>
        public virtual int ExecuteInterval { get; set; } = 5;

        /// <summary>
        /// 定时任务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task HandlerAsync(CancellationToken cancellationToken = default);
    }
}
