using HealthMonitor.Domain;
using HealthMonitor.Repository;
using HealthMonitor.Repository.imp;
using System;
using System.Collections.Generic;
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
            IDwRepository repository = new DwRepository(await DbFactory.GetDbConnection(_dbDwConfig));
            return await repository.GetInOutwellListByCompareDateAsync(_filters.CompareDate);
        }

        public async Task<IEnumerable<HongmoKaoqinModel>> GetHmInOutwellListAsync()
        {
            IHmRepository repository = new HmRepository(await DbFactory.GetDbConnection(_dbHmConfig));
            return await repository.GetHongMoKaoqinListByCompareDateAsync(_filters.CompareDate);
        }

        public async Task<IEnumerable<DwInOutwellModel>> StartCompareAsync()
        {
            //取数据
            Task<IEnumerable<DwInOutwellModel>> dwInOutwellDataList = GetDwInOutwellListAsync();
            Task<IEnumerable<HongmoKaoqinModel>> hmInOutwellDataList = GetHmInOutwellListAsync();

            //比对
            var result = await CompareAsync(await dwInOutwellDataList, await hmInOutwellDataList);

            return result;
        }

        private async Task<IEnumerable<DwInOutwellModel>> CompareAsync(IEnumerable<DwInOutwellModel> dwInOutwellDataList, IEnumerable<HongmoKaoqinModel> hmInOutwellDataList)
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




            return await Task.FromResult(dwInOutwellDataList);
        }
    }
}
