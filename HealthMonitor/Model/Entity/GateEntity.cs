using HealthMonitor.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Model.Entity
{
    public class GateEntity
    {
        /// <summary>
        /// 屏幕编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 屏幕IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 屏幕名称
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 屏幕通讯端口
        /// </summary>
        public int PORT { get; set; }

        /// <summary>
        /// 关联区域编号
        /// </summary>
        public int AREAID { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string AREANAME { get; set; }

        public GateMonitorModel ToMapViewModel()
        {
            return new GateMonitorModel()
            {
                ID = ID,
                IP = IP,
                NAME = NAME,
                PORT = PORT,
                AREA = new AreaModel() { ID = AREAID, NAME = AREANAME },
            };
        }
    }
}
