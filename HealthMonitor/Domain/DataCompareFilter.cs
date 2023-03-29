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

    public class CompareDataFilter : ViewModelBase
    {
        private string _tagMac;
        public string Tagmac
        {
            get => _tagMac;
            set => SetProperty(ref _tagMac, value);
        }

        private string _employeeName;
        public string EmployeeName
        {
            get => _employeeName;
            set => SetProperty(ref _employeeName, value);
        }


    }
}
