using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Model.Entity
{
    public class AreaRealDataEntity
    {
        public int AREAID { get; set; }
        public int MAXMANCOUNT { get; set; }
        public int RENSHU { get; set; }
    }

    public class AreaRealEmployeeEntity 
    {
        public int EID { get; set; }
        public string CNAME { get; set; }
    }
}
