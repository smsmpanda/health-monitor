using HealthMonitor.Services;
using System.Threading;

namespace HealthMonitor.Domain
{
    public class TaskItem : ViewModelBase
    {
        public TaskItem()
        {
            TaskTokenSource = new CancellationTokenSource();
        }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 是否开始执行
        /// </summary>
        private bool _isStart;
        public bool IsStart
        {
            get { return _isStart; }
            set
            {
                SetProperty(ref _isStart, value);
                if (value)
                {

                    if (this.TaskTokenSource.IsCancellationRequested)
                    {
                        this.TaskTokenSource = new CancellationTokenSource();
                    }
                    this.TaskExecute.HandlerAsync(this.TaskTokenSource.Token);
                }
                else
                {
                    this.TaskTokenSource.Cancel();
                }
            }
        }

        /// <summary>
        /// 任务Token
        /// </summary>
        public CancellationTokenSource TaskTokenSource { get; set; }

        /// <summary>
        /// 任务执行器
        /// </summary>
        public ITaskPanelExecuteService TaskExecute { get; set; }
    }
}
