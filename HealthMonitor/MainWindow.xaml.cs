using HealthMonitor.UserControls;
using HealthMonitor.Utility;
using Prism.Regions;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace HealthMonitor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon notifyIcon;
        public const string AppName = "健康监测";

        public MainWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            WindowsFullScreenHelper.SetWindowFullScrreng(this);

            ApplicationTopBtnEventBind();
            ApplicationSystemTrapMount();

            //注册默认区域
            regionManager.RegisterViewWithRegion(PrismManager.MainViewRegionName, typeof(Home));
        }

        protected override void OnClosed(EventArgs e)
        {
            this.notifyIcon?.Dispose();
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
                this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal: WindowState.Maximized;
            }
        }

        //关闭应用时事件处理
        private void ApplicationExitHandler()
        {
            if (System.Windows.MessageBox.Show("确定退出?", "温馨提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        //应用系统托盘菜单创建
        private void ApplicationSystemTrapMount()
        {
            var exit = new MenuItem("关闭应用", (s, e) =>
            {
                ApplicationExitHandler();
            });
            this.Title = AppName;
            this.notifyIcon = new NotifyIcon
            {
                Visible = true,
                Text = AppName,
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
                    this.WindowState = WindowState.Normal;
                    btnMaxIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
                }
                else
                {
                    WindowState = WindowState.Maximized;
                    btnMaxIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
                }
            };
            btnMin.Click += (s, e) =>
            {
                WindowState = WindowState.Minimized;
            };
        }
    }
}
