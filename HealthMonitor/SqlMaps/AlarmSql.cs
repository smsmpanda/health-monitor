namespace HealthMonitor.SqlMaps
{
    public struct AlarmSql
    {
        /// <summary>
        /// 报警记录插入
        /// </summary>
        public const string InsertAlarmSql = "INSERT INTO TB_EMP_REALTIME_ALARM(ALARMTYPE,ALARMMANNAME,DESCRIPTION,STARTDATE) VALUES(:ALARMTYPE, :ALARMMANNAME, :DESCRIPTION, :STARTDATE)";

        /// <summary>
        /// 检索指定报警类型记录
        /// </summary>
        public const string QueryAlarmSql = "SELECT ALARMTYPE,ALARMMANNAME,DESCRIPTION FROM TB_EMP_REALTIME_ALARM WHERE ALARMTYPE=:ALARMTYPE and ALARMMANNAME=:ALARMMANNAME";

        /// <summary>
        /// 转存报警记录
        /// </summary>
        public const string TransferAlarmSql = "INSERT INTO TB_EMP_ALARM_RECORD(ALARMTYPE,DESCRIPTION,ALARMMANNAME,STARTDATE) SELECT ALARMTYPE,DESCRIPTION,ALARMMANNAME,STARTDATE FROM TB_EMP_REALTIME_ALARM WHERE ALARMTYPE = :ALARMTYPE AND ALARMMANNAME = :ALARMMANNAME";

        /// <summary>
        /// 删除报警记录
        /// </summary>
        public const string DeleteAlarmSql = "DELETE FROM TB_EMP_REALTIME_ALARM WHERE ALARMTYPE = :ALARMTYPE AND ALARMMANNAME = :ALARMMANNAME";

        /// <summary>
        /// 更新报警记录时间
        /// </summary>
        public const string UpdateAlarmByTypeSql = "UPDATE TB_EMP_REALTIME_ALARM SET STARTDATE=:STARTDATE WHERE ALARMTYPE=:ALARMTYPE and ALARMMANNAME=:ALARMMANNAME";
    }

    public struct CompareDwSql
    {
        public const string QueryInOutwell = @"
            SELECT 
                emp.ID as EmployeeID,
                emp.TAGNUMBER as TagMac,
                emp.CNAME as EmployeeName,
                emp.DEPARTNAME as DepartMentName,
                emp.CLASSNAME as GroupClass,
                inwell.LOGINTIME as DwInwellTime,
            FROM TB_EMP_INEXITWELL inwell
            INNER JOIN TB_EMP_EMPLOYEE emp
            ON inwell.EMPLOYEENUMBER = emp.ID
            WHERE   inwell.OFFTIME IS NULL 
            AND inwell.LOGINTIME >= :compareStartDate 
            AND inwell.LOGINTIME < :compareEndDate
            ORDER BY inwell.LOGINTIME ASC";
    }

    public struct CompareHmSql
    {
        public const string QueryKaoqin = @"
            SELECT  
                EmployeeID,
                OnTime,
                OffTime 
            FROM [kaoqin].[dbo].[kaoqin] where  OnTime>='{0}' and OnTime < '{1}' and OffTime is not null  order by OnTime";
    }
}
