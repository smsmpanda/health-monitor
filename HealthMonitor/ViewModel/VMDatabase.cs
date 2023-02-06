using HealthMonitor.Enums;
using HealthMonitor.Model;
using HealthMonitor.Services;
using HealthMonitor.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.ViewModel
{
    /// <summary>
    /// 数据库健康监测类
    /// </summary>
    public class VMDatabase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DbName { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }


        /// <summary>
        /// 数据库类型（Mysql/MSSQL/Oracl）
        /// </summary>
        public string DbType { get; set; }


        /// <summary>
        /// 数据库连接串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 是否开启监测
        /// </summary>
        private bool _isCheck;
        public bool IsCheck
        {
            get { return _isCheck; }
            set
            {
                if (this._isCheck != value)
                {
                    this._isCheck = value;
                    NotifyPropertyChanged();
                }
                StartMonitor();
            }
        }

        /// <summary>
        /// 健康状态
        /// </summary>
        private bool _status;
        public bool Status
        {
            get { return _status; }
            set
            {
                if (this._status != value)
                {
                    this._status = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void NotifyPropertyChanged([CallerMemberName] string propName = "Default")
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private void StartMonitor()
        {
            Task.Run(async () =>
            {
                while (this._isCheck)
                {
                    this.Status = DbFactory
                                .GetDbByType(this.ConnectionString, (DbType)Enum.Parse(typeof(DbType), this.DbType, true))
                                .HealthCheck();

                    //异常时将报警信息入库
                    AlarmRecord alarmRecord = AlarmRecord.GenerateAlarm($"{AlarmType.ATP_DATABASE_ERROR}", $"数据库异常", this.DbName, DateTime.Now);
                    if (this.Status)
                    {
                        await RYDWDbContext.TransferAlarmRecordAsync(alarmRecord);
                    }
                    else 
                    {
                        await RYDWDbContext.InsertAlarmAsync(alarmRecord);
                    }

                    await Task.Delay(2000);
                }

                //停止监听删除相应的实时报警记录
                await RYDWDbContext.DeleteAlarmRecord(AlarmRecord.GenerateAlarm($"{AlarmType.ATP_DATABASE_ERROR}",string.Empty,this.DbName,DateTime.Now));
                this.Status = false;
            });
        }
    }
}
