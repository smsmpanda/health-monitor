using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.SqlMaps
{
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
