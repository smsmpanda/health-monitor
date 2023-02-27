using CefSharp;
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
    public partial class App : Application
    {
        private const string MUTEX_NAME = "Global\\HealthMonitor";
        private static Mutex AppMutex;

        public App()
        {
            CefRuntime.SubscribeAnyCpuAssemblyResolver();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            /*创建具有唯一名称的互斥锁*/
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
            else
            {
                base.OnStartup(e);
            }
        }

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        public static extern void SetForegroundWindow(IntPtr hwnd);
    }
}
