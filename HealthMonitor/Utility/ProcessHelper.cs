using System.Diagnostics;
using System.Linq;

namespace HealthMonitor.Utility
{
    public class ProcessHelper
    {
        /// <summary>
        /// 获取系统进程
        /// </summary>
        /// <returns></returns>
        public static Process[] GetAllProcessesInSystem()
        {
            Process[] processes = Process.GetProcesses();
            return processes;
        }

        public static bool GetProcessByProcessName(string processName)
        {
            if (Process.GetProcessesByName(processName).ToList().Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
