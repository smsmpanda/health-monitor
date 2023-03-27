using HealthMonitor.Enums;
using HealthMonitor.Extensions;
using HealthMonitor.Services;
using HealthMonitor.Services.imp;
using Prism.Ioc;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthMonitor.Domain
{
    public class DataCompareViewModel : NavigationViewModel
    {
        private bool _isBottomDrawOpen;
        private DataBaseItem _dataBaseDw;
        private DataBaseItem _dataBaseHm;
        public DataCompareFilter _filters;
        public ObservableCollection<DwInOutwellModel> _inOutWellList;
        public DataCompareViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
            _dataBaseDw = new DataBaseItem();
            _dataBaseHm = new DataBaseItem();
            _filters = new DataCompareFilter();
            _inOutWellList = new ObservableCollection<DwInOutwellModel>();
        }

        public DataBaseItem DataBaseDw
        {
            get => _dataBaseDw;
            set => SetProperty(ref _dataBaseDw, value);
        }

        public DataBaseItem DataBaseHm
        {
            get => _dataBaseHm;
            set => SetProperty(ref _dataBaseHm, value);
        }

        public ObservableCollection<DwInOutwellModel> InOutWellList
        {
            get => _inOutWellList;
            set => SetProperty(ref _inOutWellList, value);
        }

        public DataCompareFilter Filters
        {
            get => _filters;
            set => SetProperty(ref _filters, value);
        }

        public bool IsBottomDrawOpen
        {
            get => _isBottomDrawOpen;
            set => SetProperty(ref _isBottomDrawOpen, value);
        }

        public List<string> DbTypeItems => new List<string> { $"{DbType.MYSQL}", $"{DbType.ORACLE}", $"{DbType.MSSQL}" };

        public ICommand StartNewCompareCommand =>
            new AnotherCommandImplementation(StartNewCompare, CanExceuteStartCompare);

        public ICommand DataTestConnectionCommand =>
            new AnotherCommandImplementation(DataBaseConnection, CanExecuteTestConnection);

        public ICommand ExpandBottomDrawCommand =>
            new AnotherCommandImplementation(ExpandBottomDraw);


        public void ExpandBottomDraw(object m)
        {
            IsBottomDrawOpen = !IsBottomDrawOpen;
        }

        public void StartNewCompare(object m)
        {
            DbConfig dwDbConfig = ManualMapperExtension.DbItemMapperDbConfig(DataBaseDw);
            DbConfig hmDbConfig = ManualMapperExtension.DbItemMapperDbConfig(DataBaseHm);

            Task.Factory.StartNew(async () =>
            {
                (DataBaseDw.DbStatus, DataBaseDw.DbTestMessage) = await DbFactory.DbConnectionTestAsync(dwDbConfig);
                (DataBaseHm.DbStatus, DataBaseHm.DbTestMessage) = await DbFactory.DbConnectionTestAsync(hmDbConfig);
            }).ContinueWith(async (s) =>
            {
                if (DataBaseDw.DbStatus && DataBaseHm.DbStatus)
                {
                    try
                    {
                        IInOutwellDataCompareService service = new InOutwellDataCompareService(dwDbConfig, hmDbConfig, Filters);
                        InOutWellList = (ObservableCollection<DwInOutwellModel>)await service.StartCompareAsync();
                    }
                    catch (System.Exception ex)
                    {
                        DataBaseDw.DbStatus = false;
                        DataBaseDw.DbTestMessage = ex.Message;
                    }
                }
            });
        }

        public void DataBaseConnection(object m)
        {
            Task.Factory.StartNew(async () =>
            {
                foreach (var dbItem in new DataBaseItem[] { DataBaseDw, DataBaseHm })
                {
                    DbConfig config = ManualMapperExtension.DbItemMapperDbConfig(dbItem);
                    (dbItem.DbStatus, dbItem.DbTestMessage) = await DbFactory.DbConnectionTestAsync(config);
                }
            });
        }

        public bool CanExecuteTestConnection(object m)
        {
            if (string.IsNullOrWhiteSpace(this.DataBaseDw.DbHost)
                || string.IsNullOrWhiteSpace(this.DataBaseDw.DbPort)
                || string.IsNullOrWhiteSpace(this.DataBaseDw.DbUser)
                || string.IsNullOrWhiteSpace(this.DataBaseDw.DbPwd)
                || string.IsNullOrWhiteSpace(this.DataBaseDw.DbType)
                || string.IsNullOrWhiteSpace(this.DataBaseHm.DbHost)
                || string.IsNullOrWhiteSpace(this.DataBaseHm.DbPort)
                || string.IsNullOrWhiteSpace(this.DataBaseHm.DbUser)
                || string.IsNullOrWhiteSpace(this.DataBaseHm.DbPwd)
                || string.IsNullOrWhiteSpace(this.DataBaseHm.DbType))
            {
                return false;
            }
            return true;
        }

        public bool CanExceuteStartCompare(object _)
        {
            bool _f = CanExecuteTestConnection(_);
            return DataBaseHm.DbStatus && DataBaseDw.DbStatus && _f;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
        }
    }
}
