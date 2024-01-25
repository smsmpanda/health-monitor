using HealthMonitor.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace HealthMonitor.Domain
{
    public class GateMonitorModel : ViewModelBase
    {
        public string _ip;
        public string _name;
        public int _port = 502;
        private bool _startUp;
        private bool _status;
        private AreaModel _area;
        private decimal _pushInterval = 3;
        /// <summary>
        /// 闸门编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 闸门IP
        /// </summary>
        public string IP
        {
            get => _ip;
            set => SetProperty(ref _ip, value);
        }

        /// <summary>
        /// 闸门通讯端口
        /// </summary>
        public int PORT
        {
            get => _port;
            set => SetProperty(ref _port, value);
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
        /// 闸门名称
        /// </summary>
        public string NAME
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
 
        public bool STARTUP
        {
            get => _startUp;
            set => SetProperty(ref _startUp, value);
        }

        /// <summary>
        /// 设备（闸门）状态
        /// </summary>
        public bool STATUS
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        /// <summary>
        /// 检测间隔
        /// </summary>
        public decimal PUSHINTERVAL
        {
            get => _pushInterval;
            set => SetProperty(ref _pushInterval, value);
        }

        public GateEntity ToMapEntity()
        {
            return new GateEntity
            {
                ID = this.ID,
                AREAID = this.AREA.ID,
                IP = this.IP,
                NAME = this.NAME,
                PORT = this.PORT,
            };
        }
    }
}
