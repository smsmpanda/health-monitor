using CefSharp;
using HealthMonitor.Domain;
using HealthMonitor.UserControls;
using HealthMonitor.ViewModel;
using HealthMonitor.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace HealthMonitor
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication
    {
        private const string MUTEX_NAME = "Global\\HealthMonitor";
        private static Mutex AppMutex;

        public App()
        {
            CefRuntime.SubscribeAnyCpuAssemblyResolver();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow, MainViewModel>();
            containerRegistry.RegisterForNavigation<Home, HomeViewModel>();
            containerRegistry.RegisterForNavigation<DataCompare, DataCompareViewModel>();
            containerRegistry.RegisterForNavigation<Monitors, MonitorViewModel>();
            containerRegistry.RegisterForNavigation<TaskPanel, TaskPanelViewModel>();
        }

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        public static extern void SetForegroundWindow(IntPtr hwnd);

        protected override Window CreateShell()
        {
            AppMutex = new Mutex(true, MUTEX_NAME, out var createdNew);

            if (!createdNew)
            {
                var current = Process.GetCurrentProcess();

                foreach (var process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
                Shutdown();
            }
            return Container.Resolve<MainWindow>();
        }
    }
}
