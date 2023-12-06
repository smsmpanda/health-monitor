using HealthMonitor.Services.imp;
using System.Collections.ObjectModel;

namespace HealthMonitor.Domain
{
    public class TaskPanelViewModel : ViewModelBase
    {
        public ObservableCollection<TaskItem> TaskItems { get; set; }
        public TaskPanelViewModel()
        {
            TaskItems = new ObservableCollection<TaskItem>()
            {
                new TaskItem
                {
                    TaskName = "孙村唯一性比对异常报警",
                    TaskExecute = new SuncunUniqueComparisonAlerts()
                },
                new TaskItem
                {
                    TaskName = "赵官唯一性比对异常报警",
                    TaskExecute = new ZhaoguanUniqueComparisonAlerts()
                }
            };
        }
    }
}
