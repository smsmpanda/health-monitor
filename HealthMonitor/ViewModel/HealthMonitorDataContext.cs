using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Configuration;

namespace HealthMonitor.ViewModel
{
    /// <summary>
    /// 监测数据
    /// </summary>
    public class HealthMonitorDataContext
    {
        public HealthMonitorDataContext()
        {
            Init();
        }

        /// <summary>
        /// 监测数据库
        /// </summary>
        public ObservableCollection<VMDatabase> DbHealthItems { get; private set; }

        /// <summary>
        /// 监测进程
        /// </summary>
        public ObservableCollection<VMProcess> ProcessHealthItems { get; private set; }

        /// <summary>
        /// 监测FTP
        /// </summary>
        public ObservableCollection<VMFtp> FTPItems { get; private set; }

        #region 初始化配置
        private void Init()
        {
            InitDatabaseByConfigData();
            InitProcessesByConfigData();
            InitFTPByConfigData();
        }

        /// <summary>
        /// 初始化进程监控资源配置
        /// </summary>
        private void InitProcessesByConfigData()
        {
            this.ProcessHealthItems = new ObservableCollection<VMProcess>();
            var settings = (IDictionary)ConfigurationManager.GetSection("checkProcesses");

            ConfigSectionMapArray(settings, out string[] keys, out string[] values);

            for (int i = 0; i < keys.Length; i++)
            {
                this.ProcessHealthItems.Add(new VMProcess { Number = i + 1, IsCheck = false, ProcessIdentity = $"{keys[i]}", ProcessName = $"{values[i]}" });
            }
        }

        /// <summary>
        /// 初始化数据库监控配置
        /// </summary>
        private void InitDatabaseByConfigData()
        {
            this.DbHealthItems = new ObservableCollection<VMDatabase>();
            var settings = (IDictionary)ConfigurationManager.GetSection("checkDatabases");

            ConfigSectionMapArray(settings, out string[] keys, out string[] values);

            for (int i = 0; i < keys.Length; i++)
            {
                string[] vals = $"{values[i]}".Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                this.DbHealthItems.Add(new VMDatabase { Number = i + 1, IsCheck = false, DbName = keys[i], ConnectionString = $"{vals[1]}", DbType = vals[0] });
            }
        }

        /// <summary>
        /// 初始化FTP监测配置
        /// </summary>
        private void InitFTPByConfigData()
        {
            this.FTPItems = new ObservableCollection<VMFtp>();
            var settings = (IDictionary)ConfigurationManager.GetSection("checkFtp");

            ConfigSectionMapArray(settings, out string[] keys, out string[] values);

            for (int i = 0; i < keys.Length; i++)
            {
                string[] vals = $"{values[i]}".Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                this.FTPItems.Add(new VMFtp { Number = i + 1, IsCheck = false, FTPName = keys[i], FTPServerHost = $"{vals[0]}", FTPServerPort = int.Parse(vals[1]), FTPUser = vals[2], FTPPassword = vals[3] });
            }
        }

        private void ConfigSectionMapArray(IDictionary settings, out string[] keys, out string[] values)
        {
            keys = new string[settings.Keys.Count];
            values = new string[settings.Values.Count];
            settings.Keys.CopyTo(keys, 0);
            settings.Values.CopyTo(values, 0);
        }
        #endregion
    }
}
