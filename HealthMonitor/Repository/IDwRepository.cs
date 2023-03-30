using HealthMonitor.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthMonitor.Repository
{
    public interface IDwRepository : IRepository
    {
        /// <summary>
        /// 获取定位指定时间范围内出入井数据
        /// </summary>
        /// <param name="inwellDatetime">入井时间</param>
        /// <param name="outwellDatetime">出井时间</param>
        /// <returns></returns>
        Task<IEnumerable<DwInOutwellModel>> GetInOutwellListByCompareDateAsync(DateTime inwellDatetime,DateTime outwellDatetime);
    }
}
