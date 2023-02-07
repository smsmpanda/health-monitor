using HealthMonitor.ViewModel;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace HealthMonitor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HealthMonitorDataContext dataContext;

        private System.Windows.Forms.NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
            dataContext = new HealthMonitorDataContext();
            this.DataContext = dataContext;

            WindowsTrayNotify();
            WindowTopOperations();
        }

        protected override void OnClosed(EventArgs e)
        {
            this.notifyIcon.Icon = null;
            this.notifyIcon.Dispose();
            base.OnClosed(e);
        }
        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        bool IsMaximized = false;
        private void Border_LeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    IsMaximized = true;
                }
            }
        }

        private void ConfirmWhenCloseApp() 
        {
            if (MessageBox.Show("（当前应用正在监测指定数据库及联网上报等程序运行状况，请谨慎操作！）确定退出系统?", "温馨提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        private void WindowsTrayNotify() 
        {
            var exit = new System.Windows.Forms.MenuItem("关闭应用");
            exit.Click += (s, e) =>
            {
                ConfirmWhenCloseApp();
            };


            this.Title = $"{dataContext.ApplicationName}";
            this.notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Visible = true,
                Text = dataContext.ApplicationName,
                ContextMenu = new System.Windows.Forms.ContextMenu(new[] { exit }),
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath)
            };

            this.notifyIcon.MouseClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    this.Show();
                    this.Activate();
                    this.WindowState = WindowState.Normal;
                }
            };
        }

        /// <summary>
        /// Window窗体最大化，最小化，退出操作处理
        /// </summary>
        private void WindowTopOperations() 
        {
            btnClose.Click += (s, e) => {
                ConfirmWhenCloseApp();
            };
            btnMax.Click += (s, e) => {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
                else
                {
                    WindowState = WindowState.Maximized;
                }
            };
            btnMin.Click += (s, e) => {
                WindowState = WindowState.Minimized;
            };
        }
    }
}
