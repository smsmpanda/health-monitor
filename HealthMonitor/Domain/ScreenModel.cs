using HealthMonitor.Model.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace HealthMonitor.Domain
{
    public class ScreenModel : ViewModelBase
    {
        public string _ip;
        public string _name;
        public int _port = 5005;
        private bool _startUp;
        private bool _status;
        private AreaModel _area;
        private decimal _pushInterval = 3;
        private string _workfaceName;

        public ScreenModel()
        {
            Debug.WriteLine($"{nameof(ScreenModel)}::ctor --> {this.GetHashCode()}");
        }

        /// <summary>
        /// 屏幕编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 屏幕IP
        /// </summary>
        public string IP
        {
            get => _ip;
            set => SetProperty(ref _ip, value);
        }

        /// <summary>
        /// 屏幕名称
        /// </summary>
        public string NAME
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// 屏幕通讯端口
        /// </summary>
        public int PORT
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }

        

        /// <summary>
        /// 工作面名称
        /// </summary>
        public string WORKFACENAME
        {
            get => _workfaceName;
            set => SetProperty(ref _workfaceName, value);
        }

        /// <summary>
        /// 推送间隔
        /// </summary>
        public decimal PUSHINTERVAL
        {
            get => _pushInterval;
            set => SetProperty(ref _pushInterval, value);
        }

        /// <summary>
        /// 设备（屏幕）状态
        /// </summary>
        public bool STATUS
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        /// <summary>
        /// 关联区域编号
        /// </summary>
        public AreaModel AREA
        {
            get => _area;
            set => SetProperty(ref _area, value);
        }
        

        /// <summary>
        /// 开始往设备（屏幕）发送消息
        /// </summary>
        public bool STARTUP
        {
            get => _startUp;
            set => SetProperty(ref _startUp, value);
        }

        public ScreenModel SetID(int id)
        {
            this.ID = id;
            return this;
        }

        public PassengerScreenEntity ToMapEntity()
        {
            return new PassengerScreenEntity
            {
                ID = this.ID,
                AREAID = this.AREA.ID,
                IP = this.IP,
                NAME = this.NAME,
                PORT = this.PORT,
                WORKFACENAME = this.WORKFACENAME,
                PUSHINTERVAL = this.PUSHINTERVAL,
            };
        }
    }
}
