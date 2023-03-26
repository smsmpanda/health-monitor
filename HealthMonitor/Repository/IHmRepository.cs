using HealthMonitor.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthMonitor.Repository
{
    public interface IHmRepository : IRepository
    {
        Task<IEnumerable<HongmoKaoqinModel>> GetHongMoKaoqinListByCompareDateAsync(DateTime compareDate);
    }
}
