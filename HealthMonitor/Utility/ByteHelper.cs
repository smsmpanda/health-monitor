using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Utility
{
    public class ByteHelper
    {
        public static byte[] Int16ByteLittleEndian(int number)
        {
            byte[] bytes = new byte[2];
            bytes[0] = (byte)(number & 0xFF);
            bytes[1] = (byte)((number >> 8) & 0xFF);
            return bytes;
        }
    }
}
