using HealthMonitor.Domain;
using HealthMonitor.Enums;
using HealthMonitor.Extensions;
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
                return await repository.GetInOutwellListByCompareDateAsync(_inwellDateTime, _outwellDateTime, _filters.IsHongmo);
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
                _inwellDateTime = DateTimeExtension.MergeDateTime(_filters.InwellDate, _filters.InwellTime);
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
                        matchHongmoData = hmInOutwellDataList.FirstOrDefault(hm => hm.EmployeeID == dw.EmployeeID && TimeIntervalAbs(dw.DwInwellTime, hm.OnTime) <= _filters.InwellInterval);

                        if (matchHongmoData != null)
                        {
                            dw.HmInwellTime = matchHongmoData.OnTime;
                            dw.HmOutwellTime = matchHongmoData.OffTime;
                            dw.HmResult = ResultType.OutWell;
                            result.Add(dw);
                        }
                    }
                    else if (dw.IsOutwell == 1)
                    {
                        var  _inwellIsMatch  = hmInOutwellDataList.Any(hm => hm.EmployeeID == dw.EmployeeID 
                                    && TimeIntervalAbs(dw.DwInwellTime, hm.OnTime) <= _filters.InwellInterval);

                        if (!_inwellIsMatch) {
                            dw.HmResult = ResultType.InwellFailure;
                            result.Add(dw);
                            continue;
                        }

                        var _outwellIsMatch = hmInOutwellDataList.Any(hm => hm.EmployeeID == dw.EmployeeID 
                                    && TimeIntervalAbs(dw.DwOutwellTime, hm.OffTime) <= _filters.OutwellInterval);

                        if (!_outwellIsMatch)
                        {
                            dw.HmResult = ResultType.OutwellFailure;
                            result.Add(dw);
                            continue;
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

        private bool AnalyseCompare(HongmoKaoqinModel hm, DwInOutwellModel dw)
        {
            bool _in = InwellTimeCompare(hm, dw);
            if (!_in)
            {
                dw.HmResult = ResultType.InwellFailure;
                return false;
            }

            bool _out = OutwellTimeCompare(hm,dw);
            if (!_out) {
                dw.HmResult = ResultType.OutwellFailure;
                return false;
            }

            dw.HmResult = ResultType.Success;
            return true;
        }
        private bool InwellTimeCompare(HongmoKaoqinModel hm, DwInOutwellModel dw)
        {
            if (TimeIntervalAbs(dw.DwInwellTime, hm.OnTime) <= _filters.InwellInterval)
            {
                return true;
            }
            return false;
        }
        private bool OutwellTimeCompare(HongmoKaoqinModel hm, DwInOutwellModel dw)
        {
            if (TimeIntervalAbs(dw.DwOutwellTime, hm.OffTime) <= _filters.OutwellInterval)
            {
                return true;
            }
            return false;
        }

        private double TimeIntervalAbs(DateTime start, DateTime end)
        {
            TimeSpan ts = start - end;
            return Math.Abs(ts.TotalMinutes);
        }
    }
}
