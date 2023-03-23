using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Domain
{
    public class DataCompareTask : ViewModelBase
    {
        public DataCompareTask(string taskName, DateTime taskStartDate, DataBaseItem dbcItemA, DataBaseItem dbcItemB)
        {
            TaskName = taskName;
            TaskStartDate = taskStartDate;
            DbcItemA = dbcItemA;
            DbcItemB = dbcItemB;
        }

        public string TaskName { get; }
       
        public DateTime TaskStartDate { get; }

        public DataBaseItem DbcItemA { get; }
        public DataBaseItem DbcItemB { get; }
    }
}
