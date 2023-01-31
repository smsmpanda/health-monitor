using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
