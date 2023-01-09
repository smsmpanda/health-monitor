using HealthMonitor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HealthMonitor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HealthMonitorDataContext _healthMonitorDataContext;
        public MainWindow()
        {
            InitializeComponent();
            _healthMonitorDataContext = new HealthMonitorDataContext();
            this.DataContext = _healthMonitorDataContext;
        }

        private void Process_CheckChanged(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(1);
        }

        private void Process_UnCheckChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Db_CheckChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Db_UnCheckChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
