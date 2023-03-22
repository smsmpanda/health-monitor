namespace HealthMonitor.Enums
{
    internal enum AlarmType
    {
        /// <summary>
        /// 数据库状态异常
        /// </summary>
        ATP_DATABASE_ERROR,

        /// <summary>
        /// 进程退出
        /// </summary>
        ATP_PROCESS_EXIT,

        /// <summary>
        /// FTP服务异常
        /// </summary>
        ATP_FTP_ERROR
    }
}
