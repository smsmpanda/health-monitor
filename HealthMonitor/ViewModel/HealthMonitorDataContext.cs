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

        #region 初始化配置
        private void Init()
        {
            InitDatabaseByConfigData();
            InitProcessesByConfigData();
        }

        private void InitProcessesByConfigData()
        {
            this.ProcessHealthItems = new ObservableCollection<VMProcess>();
            var settings = (IDictionary)ConfigurationManager.GetSection("checkProcesses");
            foreach (var value in settings.Values)
            {
                this.ProcessHealthItems.Add(new VMProcess { IsCheck = true, ProcessName = $"{value}" });
            }
        }

        private void InitDatabaseByConfigData()
        {
            this.DbHealthItems = new ObservableCollection<VMDatabase>();
            var settings = (IDictionary)ConfigurationManager.GetSection("checkDatabases");

            ConfigSectionMapArray(settings, out string[] keys, out string[] values);

            for (int i = 0; i < keys.Length; i++)
            {
                string[] vals = $"{values[i]}".Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                this.DbHealthItems.Add(new VMDatabase { IsCheck = true, DbName = keys[i], ConnectionString = $"{vals[1]}", DbType = vals[0] });
            }
        }

        private void ConfigSectionMapArray(IDictionary settings,out string[]keys,out string[]values) 
        {
            keys = new string[settings.Keys.Count];
            values = new string[settings.Values.Count];
            settings.Keys.CopyTo(keys, 0);
            settings.Values.CopyTo(values, 0);
        }
        #endregion
    }
}
