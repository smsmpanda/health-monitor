using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Proto.Screen
{
    public class PacketDirector
    {
        private PacketBuild builder;

        public PacketDirector()
        {
            
        }

        public PacketDirector(PacketBuild builder)
        {
            this.builder = builder;
        }

        public void SetBuilder(PacketBuild builder)
        {
            this.builder = builder;
        }

        public Phy1 Construct()
        {
            builder.BuilderPacketHeader();
            builder.BuilderPacketDataDomain();
            builder.BuilderPacketCheck();

            return builder.GetResult();
        }
    }
}
