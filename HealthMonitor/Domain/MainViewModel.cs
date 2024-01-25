using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Reflection;

namespace HealthMonitor.Domain
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IRegionManager _regionManager;
        public ObservableCollection<DemoItem> DemoItems { get; }
        public DelegateCommand<DemoItem> NavigateCommand { get; }

        public MainViewModel(IRegionManager regionManager)
        {
            DemoItems = new ObservableCollection<DemoItem>(new[]
            {
                new DemoItem("Home","ViewDashboard","看板","看板"),
                new DemoItem("Monitors","MonitorDashboard","监测","监测"),
                new DemoItem("DataCompare","DatabaseCogOutline","数据","出入井及虹膜比对"),
                new DemoItem("TaskPanel","CheckboxMarkedCircleAutoOutline","任务","定时任务"),
                new DemoItem("LinkAlarm","AlarmLight","报警","报警监测"),
                new DemoItem("PassengerLimitSceen","ProjectorScreen","限员屏","限员屏显示"),
                new DemoItem("GateMonitor","BoomGate","闸机","闸门监测"),
            });

            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<DemoItem>(Navigate);
        }

        private void Navigate(DemoItem obj)
        {
            if (string.IsNullOrWhiteSpace(obj.ViewName))
            {
                return;
            }
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.ViewName);
        }

        public string ApplicationName =>
            this.GetType().Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;

        public string ApplicationVersion =>
            this.GetType().Assembly.GetName().Version.ToString();

        public string AlarmSettingAddress =>
            ConfigurationManager.AppSettings["AlarmSettingAddress"];

    }
}
