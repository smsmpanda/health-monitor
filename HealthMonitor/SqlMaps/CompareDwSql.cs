using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.SqlMaps
{
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
}
