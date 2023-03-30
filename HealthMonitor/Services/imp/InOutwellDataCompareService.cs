using HealthMonitor.Domain;
using HealthMonitor.Extensions;
using HealthMonitor.Repository;
using HealthMonitor.Repository.imp;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthMonitor.Services.imp
{
    public class InOutwellDataCompareService : IInOutwellDataCompareService
    {
        private DbConfig _dbDwConfig;
        private DbConfig _dbHmConfig;
        private readonly DataCompareFilter _filters;

        private DateTime _inwellDateTime;
        private DateTime _outwellDateTime;

        public InOutwellDataCompareService(DbConfig dbDwConfig, DbConfig dbHmConfig, DataCompareFilter Filters)
        {
            _dbDwConfig = dbDwConfig;
            _dbHmConfig = dbHmConfig;
            _filters = Filters;
        }

        public async Task<IEnumerable<DwInOutwellModel>> GetDwInOutwellListAsync()
        {
            try
            {
                IDwRepository repository = new DwRepository(await DbFactory.GetDbConnection(_dbDwConfig));
                return await repository.GetInOutwellListByCompareDateAsync(_inwellDateTime, _outwellDateTime);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<HongmoKaoqinModel>> GetHmInOutwellListAsync()
        {
            try
            {
                IHmRepository repository = new HmRepository(await DbFactory.GetDbConnection(_dbHmConfig));
                return await repository.GetHongMoKaoqinListByCompareDateAsync(_inwellDateTime, _outwellDateTime);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DwInOutwellModel>> StartCompareAsync()
        {
            try
            {
                _inwellDateTime = DateTimeExtension.MergeDateTime(_filters.InwellDate,_filters.InwellTime);
                _outwellDateTime = DateTimeExtension.MergeDateTime(_filters.OutwellDate, _filters.OutwellTime);


                Task<IEnumerable<DwInOutwellModel>> dwInOutwellDataList = GetDwInOutwellListAsync();
                Task<IEnumerable<HongmoKaoqinModel>> hmInOutwellDataList = GetHmInOutwellListAsync();

                //比对
                var result = await CompareAsync(await dwInOutwellDataList, await hmInOutwellDataList);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<IEnumerable<DwInOutwellModel>> CompareAsync(IEnumerable<DwInOutwellModel> dwInOutwellDataList, IEnumerable<HongmoKaoqinModel> hmInOutwellDataList)
        {
            try
            {
                if (dwInOutwellDataList is null)
                {
                    throw new ArgumentNullException(nameof(dwInOutwellDataList));
                }
                if (hmInOutwellDataList is null)
                {
                    throw new ArgumentNullException(nameof(hmInOutwellDataList));
                }

                //比对
                List<DwInOutwellModel> result = new List<DwInOutwellModel>();
                foreach (var dw in dwInOutwellDataList)
                {
                    HongmoKaoqinModel matchHongmoData = null;
                    //定位未出井
                    if (dw.IsOutwell == 0)
                    {
                        matchHongmoData = hmInOutwellDataList.FirstOrDefault(hm => hm.EmployeeID == dw.EmployeeID && (dw.DwInwellTime - hm.OnTime <= TimeSpan.FromMinutes(_filters.InwellInterval) || hm.OnTime - dw.DwInwellTime <= TimeSpan.FromMinutes(_filters.InwellInterval)));
                        
                        if (matchHongmoData != null)
                        {
                            dw.HmInwellTime = matchHongmoData.OnTime;
                            dw.HmOutwellTime = matchHongmoData.OffTime;
                            dw.HmResult = "已出井";
                            result.Add(dw);
                        }
                    }
                    else if (dw.IsOutwell == 1)
                    {
                        matchHongmoData = hmInOutwellDataList.FirstOrDefault(hm => hm.EmployeeID == dw.EmployeeID && (dw.DwInwellTime - hm.OnTime <= TimeSpan.FromMinutes(_filters.InwellInterval) || hm.OnTime - dw.DwInwellTime <= TimeSpan.FromMinutes(_filters.InwellInterval)) && (dw.DwOutwellTime - hm.OffTime <= TimeSpan.FromMinutes(_filters.OutwellInterval) || hm.OffTime - dw.DwOutwellTime <= TimeSpan.FromMinutes(_filters.OutwellInterval)));
                        if (matchHongmoData == null)
                        {
                            dw.HmResult = "未匹配";
                            result.Add(dw);
                        }
                    }
                }

                return await Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
