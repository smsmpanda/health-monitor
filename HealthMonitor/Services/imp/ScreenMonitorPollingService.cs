using HealthMonitor.Domain;
using HealthMonitor.Proto.Screen;
using HealthMonitor.Repository;
using HealthMonitor.Services.PollingService;
using HealthMonitor.Utility;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HealthMonitor.Services.imp
{
    public class ScreenMonitorPollingService : DefaultPollingMonitorService<ScreenModel>
    {
        public override async Task ExecuteHandleAsync()
        {
            // 1.获取当前区域实时人员信息（区域最大人数、已进入人数、可进入人数、名单）
            var areaInfo = await RYDWDbContext.GetAreaRealDataAsync(this.Entity.AREA.ID);
            //var employees = await RYDWDbContext.GetAreaRealEmployeesAsync(this.Entity.AREA.ID);

            // 信息 - 矿名称 | 日期 |  工作面名称 | 人数信息
            StringBuilder sbBuilder = new StringBuilder();
            sbBuilder.AppendLine(ApplicationConfig.GetValue("MineName"));
            sbBuilder.AppendLine($"{DateTime.Now:yyyy-MM-dd} {DateTimeHelper.GetCurrentChineseDayOfWeek()} {DateTime.Now:hh:mm:ss}");
            sbBuilder.AppendLine(this.Entity.WORKFACENAME);
            sbBuilder.AppendLine($"限员{areaInfo.MAXMANCOUNT}人 已进入{areaInfo.RENSHU}人 可进{areaInfo.MAXMANCOUNT - areaInfo.RENSHU}人");

            // 数据正文
            byte[] bytes = EncodingHelper.GB2312.GetBytes(sbBuilder.ToString());

            // 报文组装
            byte[] buffer = ProtocolPacketBuilder(bytes);

            // 发送报文
            await SendAsync(buffer);
        }

        public async Task SendAsync(byte[] buffer) 
        {
            TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(this.Entity.IP, this.Entity.PORT);
            tcpClient.SendTimeout = 3000;

            NetworkStream stream = tcpClient.GetStream();

            // 发送
            await stream.WriteAsync(buffer, 0, buffer.Length);

            //buffer = new byte[1024];
            //// 接收
            //int bytesRead = stream.Read(buffer, 0, buffer.Length);
            //string response = EncodingHelper.GB2312.GetString(buffer,0,bytesRead);
            //Console.WriteLine("Received: {0}", response);

            // 关闭
            stream.Close();
            tcpClient.Close();
        }

        public byte[] ProtocolPacketBuilder(byte[] content) 
        {
            Phy0 phy0 = new Phy0();

            // PHY1 (包头 + 数据域 + 包校验)
            Phy1 phy1 = new Phy1();

            // 数据域
            PacketDataDomain packetDataDomain = new PacketDataDomain();

            {
                // 区域数据
                AreaData area = new AreaData();
                area.ConcreteAreaData = new ConcreteAreaData();
                area.ConcreteAreaData.SetDisplayData(content);

                // PHY1填充数据域数据
                phy1.SetDataDomain(packetDataDomain);
                packetDataDomain.SetAreaData(area);
            }

            phy0.SetPhy1(phy1);

            byte[] buffer = phy0.GetResult();

            return buffer;
        }
    }
}
