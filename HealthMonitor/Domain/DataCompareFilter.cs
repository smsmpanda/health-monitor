using System;

namespace HealthMonitor.Domain
{
    public class DataCompareFilter : ViewModelBase
    {
        private DateTime _inwellDate = DateTime.Now;
        public DateTime InwellDate
        {
            get => _inwellDate;
            set => SetProperty(ref _inwellDate, value);
        }

        private DateTime _outwellDate = DateTime.Now;
        public DateTime OutwellDate
        {
            get => _outwellDate;
            set => SetProperty(ref _outwellDate, value);
        }

        private int _inwellInterval = 30;
        public int InwellInterval
        {
            get => _inwellInterval;
            set => SetProperty(ref _inwellInterval, value);
        }


        private int _outwellInterval = 15;
        public int OutwellInterval
        {
            get => _outwellInterval;
            set => SetProperty(ref _outwellInterval, value);
        }

        private DateTime _inwellTime = DateTime.Now;
        public DateTime InwellTime
        {
            get => _inwellTime;
            set => SetProperty(ref _inwellTime, value);
        }


        private DateTime _outwellTime = DateTime.Now;
        public DateTime OutwellTime
        {
            get => _outwellTime;
            set => SetProperty(ref _outwellTime, value);
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
