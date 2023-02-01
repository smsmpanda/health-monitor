using HealthMonitor.ViewModel;
using System;
using System.Windows;
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
            exit.Click += (s, e) => this.Close();


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
    }
}
