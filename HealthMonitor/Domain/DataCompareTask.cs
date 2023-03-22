using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Domain
{
    public class DataCompareTask : ViewModelBase
    {
        public DataCompareTask(string taskName, DateTime taskStartDate, DataCompareDbItem dbcItemA, DataCompareDbItem dbcItemB)
        {
            TaskName = taskName;
            TaskStartDate = taskStartDate;
            DbcItemA = dbcItemA;
            DbcItemB = dbcItemB;
        }

        public string TaskName { get; }
       
        public DateTime TaskStartDate { get; }

        public DataCompareDbItem DbcItemA { get; }
        public DataCompareDbItem DbcItemB { get; }
    }
}
