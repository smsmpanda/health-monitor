using HealthMonitor.Utility;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Proto.Screen
{
    public class Phy0
    {
        private List<byte> Result;
        public Phy0() 
        {
            Result = new List<byte>();  
        }
        /// <summary>
        /// 帧头
        /// </summary>
        public byte[] FrameHeader => new byte[8] { 0x5A, 0x5A, 0x5A, 0x5A, 0x5A, 0x5A, 0x5A, 0x5A };

        /// <summary>
        /// 物理层1
        /// </summary>
        public Phy1 Phy1 { get; private set; }

        /// <summary>
        /// 帧尾
        /// </summary>
        public byte FrameEnd => 0x5A;

        /// <summary>
        /// 封帧
        /// </summary>
        public byte[] GetResult() 
        {
            // notice:结果组装顺序参照协议要求

            // 帧头
            Result.AddRange(FrameHeader);

            // phy1 转义
            Result.AddRange(Escape(Phy1.GetResult()));

            // 帧尾
            Result.Add(FrameEnd);

            return Result.ToArray();
        }

        public void SetPhy1(Phy1 phy1) => this.Phy1 = phy1;

        /// <summary>
        /// 转义
        /// 封帧中遇到0xA5，则将之转义为0xA6，0x02，如遇到0xA6，则将之转义为0xA6，0x01。</summary>
        /// 封帧中遇到0x5A，则将之转义为0x5B，0x02，如遇到0x5B，则将之转义为0x5B，0x01。<param name="data"></param>
        /// <returns></returns>
        public static List<byte> Escape(List<byte> data)
        {
            int length = data.Count();
            int count = 0;

            for (int i = 0; i < length; i++)
            {
                if (data[i] == 0xA5 || data[i] == 0xA6 || data[i] == 0x5A || data[i] == 0x5B)
                {
                    count++;
                }
            }

            byte[] escapedData = new byte[length + count];
            int j = 0;

            for (int i = 0; i < length; i++)
            {
                if (data[i] == 0xA5)
                {
                    escapedData[j++] = 0xA6;
                    escapedData[j++] = 0x02;
                }
                else if (data[i] == 0xA6)
                {
                    escapedData[j++] = 0xA6;
                    escapedData[j++] = 0x01;
                }
                else if (data[i] == 0x5A)
                {
                    escapedData[j++] = 0x5B;
                    escapedData[j++] = 0x02;
                }
                else if (data[i] == 0x5B)
                {
                    escapedData[j++] = 0x5B;
                    escapedData[j++] = 0x01;
                }
                else
                {
                    escapedData[j++] = data[i];
                }
            }

            return escapedData.ToList();
        }

        /// <summary>
        /// 解帧过程如果遇到连续两个字节为0xA6，0x02，则反转义为 0xA5。
        /// 解帧过程如果遇到连续两个字节为0xA6，0x01，则反转义为 0xA6。
        /// 解帧过程如果遇到连续两个字节为0x5B，0x02，则反转义为 0x5A。
        /// 解帧过程如果遇到连续两个字节为0x5B，0x01，则反转义为 0x5B。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Unescape(byte[] data)
        {
            int length = data.Length;
            int count = 0;

            for (int i = 0; i < length; i++)
            {
                if (data[i] == 0xA6 && i + 1 < length && data[i + 1] == 0x02)
                {
                    data[i] = 0xA5;
                    Array.Copy(data, i + 2, data, i + 1, length - i - 2);
                    length--;
                    count++;
                }
                else if (data[i] == 0xA6 && i + 1 < length && data[i + 1] == 0x01)
                {
                    data[i] = 0xA6;
                    Array.Copy(data, i + 2, data, i + 1, length - i - 2);
                    length--;
                    count++;
                }
                else if (data[i] == 0x5B && i + 1 < length && data[i + 1] == 0x02)
                {
                    data[i] = 0x5A;
                    Array.Copy(data, i + 2, data, i + 1, length - i - 2);
                    length--;
                    count++;
                }
                else if (data[i] == 0x5B && i + 1 < length && data[i + 1] == 0x01)
                {
                    data[i] = 0x5B;
                    Array.Copy(data, i + 2, data, i + 1, length - i - 2);
                    length--;
                    count++;
                }
            }

            Array.Resize(ref data, length);

            return data;
        }
    }

    public class Phy1
    {
        private List<byte> Result = new List<byte>();

        public Phy1()
        {
            PacketHeader = new PacketHeader();
        }

        /// <summary>
        /// 包头
        /// </summary>
        public PacketHeader PacketHeader { get; private set; }

        /// <summary>
        /// 数据域
        /// </summary>
        private PacketDataDomain DataDomain { get; set; }

        /// <summary>
        /// 包校验
        /// </summary>
        public List<byte> PacketCheck { get; set; } = new List<byte>(2);


        public Phy1 SetDataDomain(PacketDataDomain dataDomain)
        {
            DataDomain = dataDomain;
            return this;
        }

        public List<byte> GetResult()
        {
            // notice:结果组装顺序参照协议要求
            // 包头中需设置整个数据域的长度（转义前）
            List<byte> domain = this.DataDomain.GetResult();

            // 包头
            this.Result.AddRange(this.PacketHeader.SetDataLength(domain.Count).GetResult());

            // 数据域
            this.Result.AddRange(domain);

            // CRC16校验
            ushort crc = CRCHelper.ComputeChecksum(this.Result.ToArray());
            this.Result.AddRange(ByteHelper.Int16ByteLittleEndian(crc));
            
            return Result;
        }
    }

    public class PacketDataDomain
    {
        public PacketDataDomain()
        {
            AreasData = new List<AreaData>();
            Result = new List<byte>();
        }
        private List<byte> Result { get; set; }
        public byte CmdGroup { get; set; } = 0xA3;
        public byte Cmd { get; set; } = 0x06;
        public byte Response { get; set; } = 0x02;
        public byte ProcessMode { get; set; } = 0x00;
        public byte Reserved { get; set; } = 0x00;
        public byte DeleteAreaNum { get; set; } = 0x00;
        public byte AreaNum { get; set; } = 0x01;

        private List<AreaData> AreasData { get; set; }

        public void SetAreaData(AreaData areaData)
        {
            this.AreasData.Add(areaData);
        }

        public List<byte> GetResult()
        {
            // notice:结果组装顺序参照协议要求
            this.Result.Add(CmdGroup);
            this.Result.Add(Cmd);
            this.Result.Add(Response);
            this.Result.Add(ProcessMode);
            this.Result.Add(Reserved);
            this.Result.Add(DeleteAreaNum);
            this.Result.Add(AreaNum);

            foreach (var area in AreasData)
            {
                var areaData = area.GetResult();
                this.Result.AddRange(area.AreaDataLen);
                this.Result.AddRange(areaData);
            }

            return this.Result;
        }
    }

    public class AreaData
    {
        public AreaData()
        {
            this.Result = new List<byte>();
        }
        private List<byte> Result { get; set; }

        public byte[] AreaDataLen { get; private set; }
        public ConcreteAreaData ConcreteAreaData { get; set; }

        public List<byte> GetResult() 
        {
            var concreteAreaData = this.ConcreteAreaData.GetResult();

            this.AreaDataLen = ByteHelper.Int16ByteLittleEndian(concreteAreaData.Count);
            // notice:结果组装顺序参照协议要求
            this.Result.AddRange(this.AreaDataLen);
            this.Result.AddRange(concreteAreaData);
            return this.Result;
        }
    }

    public class ConcreteAreaData
    {
        public ConcreteAreaData()
        {
            this.Result = new List<byte>();
        }
        private List<byte> Result { get; set; }
        public byte AreaType { get; set; } = 0x00;
        public byte[] AreaX { get; set; } = new byte[2] { 0x80, 0x80 };
        public byte[] AreaY { get; set; } = new byte[2] { 0x00, 0x00 };
        public byte[] AreaWidth { get; set; } = new byte[2] { 0x80, 0x80 };
        public byte[] AreaHeight { get; set; } = new byte[2] { 0x00, 0x00 };
        public byte DynamicAreaLoc { get; set; } = 0xFF;
        public byte LinesSizes { get; set; } = 0x00;
        public byte RunMode { get; set; } = 0x00;
        public byte[] Timeout { get; set; } = new byte[2] { 0x00, 0x00 };
        public byte SoundMode { get; set; } = 0x00;
        public byte ExtendParaLen { get; set; } = 0x00;
        public byte TextAlignment { get; set; } = 0x0A;
        public byte SingleLine { get; set; } = 0x02;
        public byte NewLine { get; set; } = 0x01;
        public byte DisplayMode { get; set; } = 0x02;
        public byte ExitMode { get; set; } = 0x00;
        public byte Speed { get; set; } = 0x00;
        public byte StayTime { get; set; } = 0x0A;
        public byte[] DataLen { get; private set; }
        public byte[] Data { get; private set; }

        public List<byte> GetResult()
        {
            // notice:结果组装顺序参照协议要求
            this.Result.Add(this.AreaType);
            this.Result.AddRange(this.AreaX);
            this.Result.AddRange(this.AreaY);
            this.Result.AddRange(this.AreaWidth);
            this.Result.AddRange(this.AreaHeight);
            this.Result.Add(this.DynamicAreaLoc);
            this.Result.Add(LinesSizes);
            this.Result.Add(RunMode);
            this.Result.AddRange(Timeout);
            this.Result.Add(SoundMode);
            this.Result.Add(ExtendParaLen);
            this.Result.Add(TextAlignment);
            this.Result.Add(SingleLine);
            this.Result.Add(NewLine);
            this.Result.Add(DisplayMode);
            this.Result.Add(ExitMode);
            this.Result.Add(Speed);
            this.Result.Add(StayTime);
            this.Result.AddRange(DataLen);
            this.Result.AddRange(Data);
            return Result;
        }

        public void SetDisplayData(byte[] data)
        {
            this.Data = data;
            this.DataLen = BitConverter.GetBytes(this.Data.Length);
        }
    }

    public class PacketHeader 
    {
        public PacketHeader()
        {
            this.Result = new List<byte>();
        }
        private List<byte> Result { get; set; }
        public byte[] DstAddr { get; set; } = new byte[2] { 0xFE,0xFF };
        public byte[] SrcAddr { get; set; } = new byte[2] { 0x00, 0x80 };
        public byte[] Reserved { get; set; } = new byte[3] { 0x00, 0x00, 0x00 };
        public byte BarCodeOption { get; set; } = 0x00;
        public byte CheckMode { get; set; } = 0x00;
        public byte DisplayMode { get; set; } = 0x00;
        public byte DeviceType { get; set; } = 0xFE;
        public byte ProtocolVersion { get; set; } = 0x02;
        public byte[] DataLen { get; set; } = new byte[2];

        public List<byte> GetResult() 
        {
            // notice:结果组装顺序参照协议要求
            this.Result.AddRange(DstAddr);
            this.Result.AddRange(SrcAddr);
            this.Result.AddRange(Reserved);
            this.Result.Add(BarCodeOption);
            this.Result.Add(CheckMode);
            this.Result.Add(DisplayMode);
            this.Result.Add(DeviceType);
            this.Result.Add(ProtocolVersion);
            this.Result.AddRange(DataLen);
            return Result;
        }

        public PacketHeader SetDataLength(int length) 
        {
            this.DataLen = ByteHelper.Int16ByteLittleEndian(length);
            return this;
        }
    }
}
