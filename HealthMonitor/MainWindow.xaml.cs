﻿using HealthMonitor.ViewModel;
using System;
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
        private readonly HealthMonitorDataContext _healthMonitorDataContext;

        private readonly System.Windows.Forms.NotifyIcon notifyIcon;
        private const string HealCheckMonitor = "健康监测";
        public MainWindow()
        {
            InitializeComponent();
            _healthMonitorDataContext = new HealthMonitorDataContext();
            this.DataContext = _healthMonitorDataContext;

            var exit = new System.Windows.Forms.MenuItem("关闭应用");
            exit.Click += (s, e) =>
            {
                ConfirmWhenCloseApp();
            };


            this.Title = $"{HealCheckMonitor}";
            this.notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Visible = true,
                Text = HealCheckMonitor,
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

            btnClose.Click += (s, e) => {
                ConfirmWhenCloseApp();
            };
            btnMax.Click += (s, e) => {
                if (WindowState == WindowState.Maximized) { 
                    WindowState= WindowState.Normal;
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

        /// <summary>
        /// 拦截最小化事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var hwndSource = (HwndSource)PresentationSource.FromVisual(this);
            hwndSource.AddHook(WndProc);

            IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
            {
                const int WM_SYSCOMMAND = 0x112;
                const int SC_MINIMIZE = 0xf020;
                const int SC_CLOSE = 0xf060;

                if (msg == WM_SYSCOMMAND)
                {
                    if (wParam.ToInt32() == SC_MINIMIZE || wParam.ToInt32() == SC_CLOSE)
                    {
                        this.Hide();
                        handled = true;
                    }
                }
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// 关闭时
        /// </summary>
        /// <param name="e"></param>
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
    }
}
