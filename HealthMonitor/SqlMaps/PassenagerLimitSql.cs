using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.SqlMaps
{
    public struct PassenagerLimitSql
    {
        public const string GetList = @"
            SELECT 
                screen.ID,
                screen.IP,
                screen.PORT,
                screen.PUSHINTERVAL,
                screen.NAME,
                screen.WORKFACENAME,
                screen.NAME,
                screen.AREAID,
                area.AREANAME 
            FROM TB_BUSINESS_SCREEN screen
            INNER JOIN TB_EMP_AREA area
            ON screen.AREAID = area.ID ORDER BY ID DESC";

        public const string Update = @"
            UPDATE TB_BUSINESS_SCREEN SET 
                IP=:IP,
                PORT=:PORT,
                PUSHINTERVAL=:PUSHINTERVAL,
                NAME=:NAME,
                WORKFACENAME=:WORKFACENAME,
                AREAID=:AREAID 
            WHERE ID=:ID";

        public const string Insert = @"
            INSERT INTO 
            TB_BUSINESS_SCREEN
            (IP,PORT,PUSHINTERVAL,NAME,WORKFACENAME,AREAID)  VALUES 
            (:IP,:PORT,:PUSHINTERVAL,:NAME,:WORKFACENAME,:AREAID)";


        public const string Delete = "DELETE FROM TB_BUSINESS_SCREEN WHERE ID=:ID";
    }
}
