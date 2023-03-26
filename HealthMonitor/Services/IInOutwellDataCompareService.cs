using HealthMonitor.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthMonitor.Services
{
    public interface IInOutwellDataCompareService
    {
        /// <summary>
        /// 获取人员定位数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DwInOutwellModel>> GetDwInOutwellListAsync();

        /// <summary>
        /// 获取虹膜考勤数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<HongmoKaoqinModel>> GetHmInOutwellListAsync();

        /// <summary>
        /// 定位|虹膜数据比对
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DwInOutwellModel>> StartCompareAsync();
    }
}
