using HealthMonitor.Domain;
using HealthMonitor.Repository;
using HealthMonitor.Repository.imp;
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
                return await repository.GetInOutwellListByCompareDateAsync(_filters.CompareDate);
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
                return await repository.GetHongMoKaoqinListByCompareDateAsync(_filters.CompareDate);
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
                //取数据
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
                    var matchHmData = hmInOutwellDataList.FirstOrDefault(hm => dw.DwInwellTime - hm.OnTime <= TimeSpan.FromMinutes(_filters.Interval));

                    if (matchHmData != null)
                    {
                        dw.HmInwellTime = matchHmData.OnTime;
                        dw.HmOutwellTime = matchHmData.OffTime;
                        result.Add(dw);
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
