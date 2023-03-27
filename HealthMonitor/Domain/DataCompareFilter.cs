using System;

namespace HealthMonitor.Domain
{
    public class DataCompareFilter : ViewModelBase
    {
        private DateTime _compareDate = DateTime.Now;

        public DateTime CompareDate
        {
            get => _compareDate;
            set => SetProperty(ref _compareDate, value);
        }

        private int _interval = 30;

        public int Interval
        {
            get => _interval;
            set => SetProperty(ref _interval, value);
        }
    }
}
