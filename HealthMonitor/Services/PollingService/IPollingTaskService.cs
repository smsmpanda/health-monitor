using System.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HealthMonitor.Services
{
    /// <summary>
    /// 定时任务
    /// </summary>
    public interface IPollingTaskService
    {
        /// <summary>
        /// 执行时间间隔
        /// </summary>
        TimeSpan ExecuteInterval { get; set; }

        /// <summary>
        /// 任务执行器
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExecutorStartPolling(CancellationToken cancellationToken = default);

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExecuteHandleAsync(CancellationToken cancellationToken = default);
    }
}
