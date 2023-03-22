using HealthMonitor.UserControls;
using HealthMonitor.ViewModel;
using HealthMonitor.Views;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace HealthMonitor.Domain
{
    public class MainWindowViewModel : ViewModelBase
    {
        private DemoItem _selectedItem;
        private int _selectedIndex;

        public DemoItem SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        public ObservableCollection<DemoItem> DemoItems { get; }

        public MainWindowViewModel()
        {
            DemoItems = new ObservableCollection<DemoItem>(new[]
            {
                new DemoItem("HomeAnalytics","首页","首页",typeof(Home),new HomeViewModel()),
                new DemoItem("AlarmLight","监控","报警监控",typeof(Monitors),new MonitorViewModel()),
                new DemoItem("DatabaseCogOutline","数据","数据比对",typeof(DataCompare),new DataCompareViewModel())
            });
            SelectedItem = DemoItems.First();
        }



        public string ApplicationName => 
            this.GetType().Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;

        public string ApplicationVersion => 
            this.GetType().Assembly.GetName().Version.ToString();

        public string AlarmSettingAddress => 
            ConfigurationManager.AppSettings["AlarmSettingAddress"];

    }
}
