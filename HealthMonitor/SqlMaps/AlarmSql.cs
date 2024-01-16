using System.CodeDom;

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

        /// <summary>
        /// 批量新增报警记录
        /// </summary>
        public const string InsertAlarmBlukSql = @" INSERT ALL  {0} SELECT 1 FROM DUAL ";
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
                emp.MANCODE as ManCode,
                inwell.ID as InexitWellID,
                inwell.LOGINTIME as DwInwellTime,
                inwell.OFFTIME as DwOutwellTime,
                CASE 
	            WHEN inwell.OFFTIME is null THEN
		            0
	            ELSE
		            1
                END AS IsOutwell
            FROM TB_EMP_INEXITWELL inwell
            INNER JOIN TB_EMP_EMPLOYEE emp
            ON inwell.EMPLOYEENUMBER = emp.ID AND emp.ISEMPLOYEE = 1
            WHERE 
             {2} AND
            inwell.LOGINTIME >= to_date('{0}','yyyy-MM-dd HH24:mi:ss') AND inwell.OFFTIME <= to_date('{1}','yyyy-MM-dd HH24:mi:ss')
            OR inwell.OFFTIME IS NULL
            ORDER BY emp.ID,inwell.LOGINTIME";
    }

    public struct CompareHmSql
    {
        public const string QueryKaoqin = @"
            SELECT  
                EmployeeID,
                OnTime,
                OffTime 
            FROM [kaoqin].[dbo].[kaoqin] 
            WHERE  
            OffTime is not null
            AND OnTime>='{0}' AND OffTime <= '{1}'
            order by EmployeeID, OnTime";
    }

    public struct UniqueComparsionSql
    {
        //比对起始ID
        public const string GetInExitWellStartID = @"
            SELECT MAX( ID ) AS MAXID FROM TB_EMP_INEXITWELL ";

        //滚动比对数据(出入井数据)
        public const string GetInExitWellDataByStartID = @"
            SELECT
                W.ID AS MAXID,
	            W.EMPLOYEENUMBER AS EMPID,
                W.LOGINTIME,
	            E.CNAME AS NAME,
	            E.DEPARTNAME AS DEPARTMENTNAME 
            FROM
	            TB_EMP_INEXITWELL W
	            INNER JOIN TB_EMP_EMPLOYEE E ON W.EMPLOYEENUMBER = E.ID 
            WHERE
	            W.ID > {0} ORDER BY W.ID DESC ";

        //滚动比对数据(唯一性通过记录)
        public const string GetUniqueData = @"
            SELECT
	            EMPID,
	            IRISOUTINTIME AS UniqueRecordTime
            FROM
	            (
	            SELECT
		            T1.EMPID,
		            T1.IRISOUTINTIME,
		            ROW_NUMBER ( ) OVER ( PARTITION BY EMPID ORDER BY IRISOUTINTIME DESC ) AS RN 
	            FROM
		            TB_EMP_IRISINEXITWELL T1 
	            ) U 
            WHERE
	            U.RN = 1 
	            AND EXISTS (
	            SELECT
		            W.EMPLOYEENUMBER AS EMPID
	            FROM
		            TB_EMP_INEXITWELL W
	            WHERE
		            W.ID > {0}
		            AND U.EMPID = EMPID 
	            )";

        /// <summary>
        /// 删除指定职工的报警记录
        /// </summary>
        public const string DeleteUniqueAlarm = @"DELETE FROM TB_EMP_REALTIME_ALARM WHERE ALARMTYPE = '{0}' AND ALARMMANID IN ({1})";

        /// <summary>
        /// 孙村唯一性记录
        /// </summary>
        public const string SuncunUniqueRecord = @"
            select personid,Name,depname,times,type from tshm where type = 0 and times >= '{0}' ";
        
        /// <summary>
        /// 孙村唯一性记录
        /// </summary>
        public const string SuncunUniqueRecordByName = @"
            select personid,Name,depname,times,type from tshm where type = 0 and times >= DATE_ADD(now(),INTERVAL -7 day) and Name in ({0}) order by times ";

        /// <summary>
        /// 根据职工姓名检索职工信息
        /// </summary>
        public const string QueryEmployeeInfoByName = @" 
            select ID,CNAME,DEPARTMENT,DEPARTNAME from tb_emp_employee where cname = '{0}' ";


        /// <summary>
        /// 检索所有的唯一性比对失败报警记录(重新去比对)
        /// </summary>
        public const string GetUniqueAlarmData = @"select ID,ALARMMANID,ALARMMANNAME,STARTDATE from TB_EMP_REALTIME_ALARM WHERE ALARMTYPE = '{0}' ";


        /// <summary>
        /// 查找指定职工的所有唯一性通过记录
        /// </summary>
        public const string GetUniqueReocrdByEmpID = @" 
            SELECT
	            EMPID,
	            IRISOUTINTIME AS  UniqueRecordTime
            FROM  TB_EMP_IRISINEXITWELL where  EMPID IN ({0}) AND IRISOUTINTIME >= sysdate - 7 ";

        /// <summary>
        /// 删除报警记录
        /// </summary>
        public const string DeleteUniqueAlarmById = @"DELETE FROM TB_EMP_REALTIME_ALARM WHERE ALARMTYPE = '{0}' AND ID IN({1})";

        /// <summary>
        /// 转存报警记录
        /// </summary>
        public const string TransferAlarmSql = "INSERT INTO TB_EMP_ALARM_RECORD(ALARMTYPE,ALARMMANID,DESCRIPTION,ALARMMANNAME,STARTDATE) SELECT ALARMTYPE,ALARMMANID,DESCRIPTION,ALARMMANNAME,STARTDATE FROM TB_EMP_REALTIME_ALARM WHERE ALARMTYPE = '{0}' AND ID in ({1})";
    }
}
