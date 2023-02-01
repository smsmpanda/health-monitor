using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HealthMonitor
{
    class Program
    {
        private const string MUTEX_NAME = "Global\\HealthMonitor";
        private const string MAIN_WINDOWS = "MainWindow.xaml";

        [STAThread]
        static void Main(string[] args)
        {
            var mutex = new Mutex(true, MUTEX_NAME, out var isFirstInstance);
            if (isFirstInstance == false)
            {
                return;
            }

            var app = new Application();
            app.StartupUri = new Uri(MAIN_WINDOWS, UriKind.Relative);
            app.Run();
        }
    }
}
