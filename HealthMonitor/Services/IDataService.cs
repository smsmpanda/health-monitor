using HealthMonitor.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Services
{
    public interface IDataService
    {
        Task<DwInOutwellModel> GetCompareOfDw();
    }
}
