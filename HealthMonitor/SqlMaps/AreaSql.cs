using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.SqlMaps
{
    public struct AreaSql
    {
        public const string Get = @"
            SELECT 
                ID,
                AREANAME AS NAME
            FROM TB_EMP_AREA 
            WHERE ENABLED=1 start with parentid = 0 CONNECT BY PRIOR id = parentid";

        public const string GetAreaRealData = @"
            SELECT
	            AREA.ID  AS AREAID,
	            AREANAME,
                CASE WHEN MAXMANCOUNT IS NOT NULL THEN MAXMANCOUNT ELSE 0 
                END AS MAXMANCOUNT,
	            PARENTID,
            CASE WHEN MANONAREA.RENSHU IS NOT NULL THEN MANONAREA.RENSHU ELSE 0 
	            END AS RENSHU 
            FROM
            TB_EMP_AREA AREA
            LEFT JOIN (
            SELECT
	            areaid,
	            count( 1 ) AS RENSHU 
            FROM
	            (
	            SELECT
		            * 
	            FROM
		            (
		            SELECT
			            row_number ( ) over ( partition BY employeeid, AREAID ORDER BY ontime DESC ) ROW_RN,
			            MANAREA.EMPLOYEEID,
			            MANAREA.AREAID,
			            MANAREA.ONTIME 
		            FROM
			            TB_EMP_MANONAREA MANAREA
			            LEFT JOIN TB_EMP_EMPLOYEE E ON MANAREA.EMPLOYEEID = E.ID 
		            WHERE
			            E.ISEMPLOYEE = 1 
			            AND E.ENABLED = 1 
			            AND E.ISSHOW = 1 
		            ) 
	            WHERE
		            ROW_RN = 1 
	            ) 
            GROUP BY
	            areaid 
            ) MANONAREA ON AREA.ID = MANONAREA.AREAID 
            WHERE AREA.ENABLED = 1 AND ID=:ID
            ORDER BY ID";

        public const string GetRealAreaEmployee = @"
            SELECT
	            CNAME,
	            EID
            FROM
	            (
	            SELECT
		            row_number ( ) over ( partition BY employeeid, AREAID ORDER BY ontime DESC ) ROW_RN,
		            MANAREA.AREAID,
		            E.ID AS EID,
	              E.CNAME	
	            FROM
		            TB_EMP_MANONAREA MANAREA
		            LEFT JOIN TB_EMP_EMPLOYEE E ON MANAREA.EMPLOYEEID = E.ID 
	            WHERE
		            E.ISEMPLOYEE = 1 
		            AND E.ENABLED = 1 
		            AND E.ISSHOW = 1 
	            ) 
            WHERE
	            ROW_RN = 1 AND AREAID =:AreaID
            ORDER BY AREAID";
    }
}
