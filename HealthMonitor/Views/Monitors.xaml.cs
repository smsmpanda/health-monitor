using CefSharp;
using CefSharp.Wpf;
using HealthMonitor.Extensions;
using HealthMonitor.Views;
using Prism.Events;
using System.Threading.Tasks;

namespace HealthMonitor.UserControls
{
    /// <summary>
    /// Monitors.xaml 的交互逻辑
    /// </summary>
    public partial class Monitors
    {
        public Monitors(IEventAggregator eventAggregator)
        {
            eventAggregator.Register(arg =>
            {
                MonitorsDialogHost.IsOpen = arg.IsOpen;
                if (MonitorsDialogHost.IsOpen)
                {
                    MonitorsDialogHost.DialogContent = new ProgressBarView();
                }
            });
            InitializeComponent();
        }
    }
}
