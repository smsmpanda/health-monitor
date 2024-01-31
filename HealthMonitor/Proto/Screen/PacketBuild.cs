using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Proto.Screen
{
    public abstract class PacketBuild
    {
        protected Phy1 Packet { get; set; } = new Phy1();


        /// <summary>
        /// 构建报文包头
        /// </summary>
        public abstract void BuilderPacketHeader();

        /// <summary>
        /// 构建报文数据域
        /// </summary>
        public abstract void BuilderPacketDataDomain();

        /// <summary>
        /// 构建报文校验码
        /// </summary>
        public abstract void BuilderPacketCheck();

        public Phy1 GetResult() 
        {
            return Packet;
        }
    }
}
