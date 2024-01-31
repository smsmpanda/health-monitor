using HealthMonitor.Domain;
using HealthMonitor.Repository;
using HealthMonitor.Services.PollingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyModbus;
using static OfficeOpenXml.ExcelErrorValue;

namespace HealthMonitor.Services.imp
{
    public class GateMonitorPollingService : DefaultPollingMonitorService<GateMonitorModel>
    {
        public override async Task ExecuteHandleAsync()
        {
            // 1.获取到当前区域实时人数信息（最大人数、已进入人数）
            var areaInfo = await RYDWDbContext.GetAreaRealDataAsync(this.Entity.AREA.ID);

            ModbusClient modbusClient = new ModbusClient(this.Entity.IP, this.Entity.PORT);
            modbusClient.Connect();

            // 获取当前闸门状态
            int gateStatus = modbusClient.ReadHoldingRegisters(1, 1).First();

            if (areaInfo.RENSHU > areaInfo.MAXMANCOUNT)
            {
                if (gateStatus != 0) 
                {
                    modbusClient.WriteMultipleRegisters(0, new int[] { 2 });
                }
            }
            else
            {
                if (gateStatus != 1)
                {
                    modbusClient.WriteMultipleRegisters(0, new int[] { 1 });
                }
            }

            modbusClient.Disconnect();
        }
    }
}
