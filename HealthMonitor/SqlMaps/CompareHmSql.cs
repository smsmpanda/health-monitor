using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.SqlMaps
{
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
}
