using HealthMonitor.Domain;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace HealthMonitor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool IsMaximized = false;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private readonly MainWindowViewModel dataContext;

        public MainWindow()
        {
            dataContext = new MainWindowViewModel();
            DataContext = dataContext;

            WindowOutStyleControl();
            ApplicationSystemTrapMount();

            InitializeComponent();
            ApplicationTopBtnEventBind();
        }

        protected override void OnClosed(EventArgs e)
        {
            this.notifyIcon.Icon = null;
            this.notifyIcon.Dispose();
            base.OnClosed(e);
        }

        //鼠标左键应用拖动
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void Border_LeftButtonDown(object sender, MouseButtonEventArgs e)
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

        //关闭应用时事件处理
        private void ApplicationExitHandler()
        {
            if (MessageBox.Show("（当前应用正在监测指定数据库及联网上报等程序运行状况，请谨慎操作！）确定退出系统?", "温馨提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        //应用系统托盘菜单创建
        private void ApplicationSystemTrapMount()
        {
            var exit = new System.Windows.Forms.MenuItem("关闭应用", (s, e) =>
            {
                ApplicationExitHandler();
            });

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

        //应用顶栏最大化|最小化|关闭按钮时间绑定
        private void ApplicationTopBtnEventBind()
        {
            btnClose.Click += (s, e) =>
            {
                ApplicationExitHandler();
            };
            btnMax.Click += (s, e) =>
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
                else
                {
                    WindowState = WindowState.Maximized;
                }
            };
            btnMin.Click += (s, e) =>
            {
                WindowState = WindowState.Minimized;
            };
        }

        //避免全屏时应用高度超出Windows底栏
        private void WindowOutStyleControl()
        {
            this.MaxHeight = SystemParameters.PrimaryScreenHeight;
        }
    }
}
