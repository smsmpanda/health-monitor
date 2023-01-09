using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace HealthMonitor.ViewModel
{
    /// <summary>
    /// 指定进程监测
    /// </summary>
    public class VMProcess
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 进程名称
        /// </summary>
        private string _processName;
        public string ProcessName
        {
            get { return _processName; }
            set
            {
                if (this._processName != value)
                {
                    this._processName = value;
                    this.NotifyPropertyChanged("ProcessName");
                }
            }
        }

        /// <summary>
        /// 进程启动位置
        /// </summary>
        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                if (this._location != value)
                {
                    this._location = value;
                    this.NotifyPropertyChanged("Location");
                }
            }
        }


        /// <summary>
        /// 是否开启监测
        /// </summary>
        private bool _isCheck;
        public bool IsCheck
        {
            get { return _isCheck; }
            set
            {
                if (this._isCheck != value)
                {
                    this._isCheck = value;
                    this.NotifyPropertyChanged("IsCheck");
                }
                //this.StartMonitor();
            }
        }

        /// <summary>
        /// 健康状态
        /// </summary>
        private bool _status;
        public bool Status
        {
            get { return _status; }
            set
            {
                if (this._status != value)
                {
                    this._status = value;
                    this.NotifyPropertyChanged("Status");
                }
            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// 开始监测进程状态
        /// </summary>
        private void StartMonitor() 
        {
            Task.Run(() =>
            {
                while (this._isCheck)
                {
                    this.Status = DateTime.Now.Second % 3 == 0 ? true : false;

                    System.Diagnostics.Debug.WriteLine(this.Status); //Debug

                    Task.Delay(2000).GetAwaiter().GetResult();
                }
            });
        }
    }
}
