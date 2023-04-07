using HealthMonitor.Domain;
using HealthMonitor.Enums;
using HealthMonitor.Extensions;
using HealthMonitor.Repository;
using HealthMonitor.Repository.imp;
using Magicodes.ExporterAndImporter.Core.Filters;
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

                List<DwInOutwellModel> result = new List<DwInOutwellModel>();
                HongmoKaoqinModel matchDataItem;

                foreach (var dw in dwInOutwellDataList)
                {
                    matchDataItem = null;

                    //过滤出当前职工的虹膜记录
                    IEnumerable<HongmoKaoqinModel> _hongmoListByEmployeeID = hmInOutwellDataList.Where(hm => hm.EmployeeID == dw.EmployeeID);

                    #region 定位未出井
                    if (dw.IsOutwell == 0)
                    {
                        matchDataItem = _hongmoListByEmployeeID.FirstOrDefault(hm => TimeIntervalAbs(dw.DwInwellTime, hm.OnTime) <= _filters.InwellInterval);

                        if (matchDataItem != null)
                        {
                            dw.HmInwellTime = matchDataItem.OnTime;
                            dw.HmOutwellTime = matchDataItem.OffTime;
                            dw.HmResult = ResultType.OutWell.Description;
                            result.Add(dw);
                        }
                        continue;
                    }
                    #endregion

                    #region 定位已出井
                    bool _allMatch = _hongmoListByEmployeeID.Any(
                        hm => DateTimeWithInRangeCompare(dw.DwInwellTime, hm.OnTime, _filters.InwellInterval) &&
                        DateTimeWithInRangeCompare(dw.DwOutwellTime.Value, hm.OffTime, _filters.OutwellInterval));
                    
                    if (_allMatch)
                    {
                        continue;
                    }

                    //入井
                    bool _inwellMatch = _hongmoListByEmployeeID
                        .Any(hm => DateTimeWithInRangeCompare(dw.DwInwellTime, hm.OnTime, _filters.InwellInterval));

                    //出井
                    bool _outwellMatch = _hongmoListByEmployeeID
                        .Any(hm => DateTimeWithInRangeCompare(dw.DwOutwellTime.Value, hm.OffTime, _filters.OutwellInterval));

                    
                    //出入井全部未匹配
                    if (!_inwellMatch && !_outwellMatch)
                    {
                        matchDataItem = _hongmoListByEmployeeID.OrderBy(hm => TimeIntervalAbs(hm.OnTime, dw.DwInwellTime)).FirstOrDefault();
                        if (matchDataItem != null) {
                            matchDataItem = _hongmoListByEmployeeID.OrderBy(hm => TimeIntervalAbs(hm.OffTime, dw.DwOutwellTime.Value)).FirstOrDefault();
                        }
                        dw.HmResult = ResultType.Failure.Description;
                    }
                    //入井未匹配成功
                    else if (!_inwellMatch)
                    {
                        matchDataItem = _hongmoListByEmployeeID.OrderBy(hm => TimeIntervalAbs(hm.OnTime, dw.DwInwellTime)).FirstOrDefault();
                        dw.HmResult = ResultType.InwellFailure.Description;
                    }
                    //升井未匹配成功
                    else if (!_outwellMatch)
                    {
                        matchDataItem = _hongmoListByEmployeeID.OrderBy(hm => TimeIntervalAbs(hm.OffTime, dw.DwOutwellTime.Value)).FirstOrDefault();
                        dw.HmResult = ResultType.OutwellFailure.Description;
                    }

                    dw.HmInwellTime = matchDataItem?.OnTime;
                    dw.HmOutwellTime = matchDataItem?.OffTime;
                    result.Add(dw);
                    #endregion
                }

                return await Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private double TimeIntervalAbs(DateTime start, DateTime end)
        {
            TimeSpan ts = start - end;
            return Math.Abs(ts.TotalMinutes);
        }

        private bool DateTimeWithInRangeCompare(DateTime rangeTime, DateTime compareTime, float interval)
        {
           return  compareTime >= rangeTime.AddMinutes(-interval) && compareTime <= rangeTime.AddMinutes(interval);
        }
    }
}
