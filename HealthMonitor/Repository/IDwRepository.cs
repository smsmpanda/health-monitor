using HealthMonitor.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthMonitor.Repository
{
    public interface IDwRepository : IRepository
    {
        Task<IEnumerable<DwInOutwellModel>> GetInOutwellListByCompareDateAsync(DateTime compareData);
    }
}
