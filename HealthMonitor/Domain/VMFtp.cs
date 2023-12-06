using HealthMonitor.Domain;
using HealthMonitor.Enums;
using HealthMonitor.Model.Entity;
using HealthMonitor.Repository;
using HealthMonitor.Utility;
using System;
using System.Threading.Tasks;

namespace HealthMonitor.ViewModel
{
    public class VMFtp : ViewModelBase
    {

        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// FTP名称
        /// </summary>
        public string FTPName { get; set; }

        /// <summary>
        /// FTP服务IP地址
        /// </summary>
        public string FTPServerHost { get; set; }

        /// <summary>
        /// FTP服务端口
        /// </summary>
        public int FTPServerPort { get; set; }

        /// <summary>
        /// FTP用户登录账户
        /// </summary>
        public string FTPUser { get; set; }

        /// <summary>
        /// FTP用户登录密码
        /// </summary>
        public string FTPPassword { get; set; }


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
                SetProperty(ref _status, value);
            }
        }


        private void StartMonitor()
        {
            Task.Run(async () =>
            {
                while (this._isCheck)
                {

                    this.Status = await FTPHelper.TryConnectFTPAsync(this.FTPServerHost, this.FTPUser, this.FTPPassword, this.FTPServerPort);

                    //异常时将报警信息入库
                    AlarmRecordEntity alarmRecord = AlarmRecordEntity.GenerateAlarm($"{AlarmType.ATP_FTP_ERROR}", $"FTP连接异常", this.FTPName, DateTime.Now);
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
                await RYDWDbContext.DeleteAlarmRecord(AlarmRecordEntity.GenerateAlarm($"{AlarmType.ATP_FTP_ERROR}", string.Empty, this.FTPName, DateTime.Now));
                this.Status = false;
            });
        }
    }
}
