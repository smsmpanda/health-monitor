using HealthMonitor.Extensions;
using HealthMonitor.Views;
using Prism.Events;

namespace HealthMonitor.UserControls
{
    /// <summary>
    /// Monitors.xaml 的交互逻辑
    /// </summary>
    public partial class Monitors
    {
        public Monitors(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(arg =>
            {
                MonitorsDialogHost.IsOpen = arg.IsOpen;
                if (MonitorsDialogHost.IsOpen)
                {
                    MonitorsDialogHost.DialogContent = new ProgressBarView();
                }
                else
                {
                    MonitorsDialogHost.DialogContent = null;
                }
            });
            InitializeComponent();
        }
    }
}
