using HealthMonitor.Extensions;
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
                new DemoItem("Home","HomeAnalytics","首页","首页"),
                new DemoItem("Monitors","AlarmLight","监控","报警监控"),
                new DemoItem("DataCompare","DatabaseCogOutline","数据","数据比对")
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
