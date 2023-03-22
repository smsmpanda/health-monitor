using FluentFTP;
using System.Threading.Tasks;

namespace HealthMonitor.Utility
{
    /// <summary>
    /// FTP操作
    /// </summary>
    public class FTPHelper
    {
        public static async Task<bool> TryConnectFTPAsync(string ftpHost, string user = null, string password = null, int port = 0)
        {
            var ftpClient = new AsyncFtpClient(ftpHost, user, password, port);
            var ftpProfile = await ftpClient.AutoConnect();
            if (ftpProfile != null)
            {
                return true;
            }
            return false;
        }
    }
}
