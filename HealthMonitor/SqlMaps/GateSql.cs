using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.SqlMaps
{
    public struct GateSql
    {
        public const string GetGateSql = @"
        SELECT 
            gate.ID,
            gate.IP,
            gate.NAME,
            gate.PORT,
            gate.AREAID,
            area.AREANAME
        FROM TB_BUSINESS_GATE gate
        INNER JOIN TB_EMP_AREA area
        ON gate.AREAID = area.ID ORDER BY gate.ID";

        public const string Update = @"
            UPDATE TB_BUSINESS_GATE SET 
                IP=:IP,
                PORT=:PORT,
                NAME=:NAME,
                AREAID=:AREAID 
            WHERE ID=:ID";

        public const string Insert = @"
            INSERT INTO 
            TB_BUSINESS_GATE
            (IP,PORT,NAME,AREAID)  VALUES 
            (:IP,:PORT,:NAME,:AREAID)";


        public const string Delete = "DELETE FROM TB_BUSINESS_GATE WHERE ID=:ID";
    }
}
