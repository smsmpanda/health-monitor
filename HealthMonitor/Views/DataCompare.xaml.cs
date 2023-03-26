using HealthMonitor.Extensions;
using Prism.Events;
using System.Windows.Controls;

namespace HealthMonitor.Views
{
    /// <summary>
    /// DataCompare.xaml 的交互逻辑
    /// </summary>
    public partial class DataCompare : UserControl
    {
        public DataCompare(IEventAggregator eventAggregator)
        {
            eventAggregator.Register(arg =>
            {
                DataCompareDialog.IsOpen = arg.IsOpen;
                if (DataCompareDialog.IsOpen)
                {
                    DataCompareDialog.DialogContent = new ProgressBarView();
                }
            });
            InitializeComponent();
        }
    }
}
