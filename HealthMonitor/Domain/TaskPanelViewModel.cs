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
                    TaskName = "孙村-唯一性比对异常报警",
                    TaskExecute = new SuncunUniqueComparisonAlerts()
                },
                new TaskItem
                {
                    TaskName = "赵官-唯一性比对异常报警",
                    TaskExecute = new ZhaoguanUniqueComparisonAlerts()
                },
                new TaskItem
                {
                    TaskName = "赵官-唯一性比对异常报警",
                    TaskExecute = new ZhaoGuanAlarmMonitorService()
                },
                new TaskItem
                {
                    TaskName = "孙村-唯一性比对异常报警",
                    TaskExecute = new SuncunAlarmMonitorService()
                }
            };
        }
    }
}
