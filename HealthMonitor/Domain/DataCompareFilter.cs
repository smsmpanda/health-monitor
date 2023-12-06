using HealthMonitor.Enums;
using System;
using System.Collections.ObjectModel;

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

        private bool _isHongmo;
        public bool IsHongmo
        {
            get => _isHongmo;
            set => SetProperty(ref _isHongmo, value);
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

        private ObservableCollection<string> _departMents = new ObservableCollection<string>();
        public ObservableCollection<string> DepartMents
        {
            get => _departMents;
            set => SetProperty(ref _departMents, value);
        }

        private string _departMent;
        public string DepartMent
        {
            get => _departMent;
            set => SetProperty(ref _departMent, value);
        }


        private ObservableCollection<ResultItem> _resultTypes = new ObservableCollection<ResultItem>
        {
            Enums.ResultType.Success,
            Enums.ResultType.Failure,
            Enums.ResultType.OutWell,
            Enums.ResultType.OutwellFailure,
            Enums.ResultType.InwellFailure,
        };
        public ObservableCollection<ResultItem> ResultTypes
        {
            get => _resultTypes;
            set => SetProperty(ref _resultTypes, value);
        }

        private ResultItem? _resultType;
        public ResultItem? ResultType
        {
            get => _resultType;
            set => SetProperty(ref _resultType, value);
        }
    }
}
