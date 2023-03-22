using HealthMonitor.Domain;
using HealthMonitor.Enums;
using HealthMonitor.Model;
using HealthMonitor.Utility;
using System;
using System.Threading.Tasks;

namespace HealthMonitor.ViewModel
{
    /// <summary>
    /// 指定进程监测
    /// </summary>
    public class VMProcess : ViewModelBase
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 进程名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 进程标识
        /// </summary>
        public string ProcessIdentity { get; set; }

        /// <summary>
        /// 是否开启监测
        /// </summary>
        private bool _isCheck;
        public bool IsCheck
        {
            get { return _isCheck; }
            set
            {
                SetProperty(ref _isCheck, value);
                this.StartMonitor();
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
                SetProperty(ref _status, value);
            }
        }


        /// <summary>
        /// 开始监测进程状态
        /// </summary>
        private void StartMonitor()
        {
            Task.Run(async () =>
            {
                while (this._isCheck)
                {
                    this.Status = ProcessHelper.GetProcessByProcessName(this.ProcessName);

                    //异常时将报警信息入库
                    AlarmRecord alarmRecord = AlarmRecord.GenerateAlarm($"{AlarmType.ATP_PROCESS_EXIT}", $"{this.ProcessName}-进程退出", this.ProcessName, DateTime.Now);

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
                await RYDWDbContext.DeleteAlarmRecord(AlarmRecord.GenerateAlarm($"{AlarmType.ATP_PROCESS_EXIT}", string.Empty, this.ProcessName, DateTime.Now));
                this.Status = false;
            });
        }
    }
}
