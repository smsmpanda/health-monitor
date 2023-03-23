using HealthMonitor.Enums;
using HealthMonitor.Services;
using HealthMonitor.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthMonitor.Domain
{
    public class DataCompareViewModel : ViewModelBase
    {
        private bool _isBottomDrawOpen;
        private DataBaseItem _dataBaseItemForDw;
        private DataBaseItem _dataBaseItemForHm;
        public DataCompareFilter _filters;
        public ObservableCollection<DwInOutwellModel> _inOutWellList;
        public DataCompareViewModel()
        {
            _dataBaseItemForDw = new DataBaseItem();
            _dataBaseItemForHm = new DataBaseItem();
            _filters = new DataCompareFilter();
            _inOutWellList= new ObservableCollection<DwInOutwellModel>();
        }

        public DataBaseItem DataBaseForDw
        {
            get => _dataBaseItemForDw;
            set => SetProperty(ref _dataBaseItemForDw, value);
        }

        public DataBaseItem DataBaseItemForHm
        {
            get => _dataBaseItemForHm;
            set => SetProperty(ref _dataBaseItemForHm, value);
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

        public ICommand AddNewDbCompareCommand => 
            new AnotherCommandImplementation(AddNewDbCompare, CanExecuteAddNewDbCompare);
        
        public ICommand DataTestConnectionCommand => 
            new AnotherCommandImplementation(DataBaseConnection, CanExecuteAddNewDbCompare);
        
        public ICommand ExpandBottomDrawCommand => 
            new AnotherCommandImplementation(ExpandBottomDraw);


        public void ExpandBottomDraw(object m) 
        {
            IsBottomDrawOpen = !IsBottomDrawOpen;
        }

        public void AddNewDbCompare(object m)
        {
            for (int i = 1; i <= 100; i++)
            {
                _inOutWellList.Add(new DwInOutwellModel() { DepartMentName=$"部门{i}" });
            }
        }

        public void DataBaseConnection(object m)
        {
            Task.Factory.StartNew(async () =>
            {

                foreach (var dbItem in new List<DataBaseItem> { DataBaseForDw, DataBaseItemForHm })
                {
                    DbConfig config = new DbConfig()
                    {
                        DbType = (DbType)Enum.Parse(typeof(DbType), dbItem.DbType, true),
                        DbCatalog = dbItem.DbCatalog,
                        Host = dbItem.DbHost,
                        Port = dbItem.DbPort,
                        Password = dbItem.DbPwd,
                        User = dbItem.DbUser
                    };
                    (dbItem.DbStatus, dbItem.DbTestMessage) = await DbFactory.DbConnectionTestAsync(config);
                }
            });
        }

        public bool CanExecuteAddNewDbCompare(object m)
        {
            if (string.IsNullOrWhiteSpace(this.DataBaseForDw.DbHost)
                || string.IsNullOrWhiteSpace(this.DataBaseForDw.DbPort)
                || string.IsNullOrWhiteSpace(this.DataBaseForDw.DbUser)
                || string.IsNullOrWhiteSpace(this.DataBaseForDw.DbPwd)
                || string.IsNullOrWhiteSpace(this.DataBaseForDw.DbType)
                || string.IsNullOrWhiteSpace(this.DataBaseItemForHm.DbHost)
                || string.IsNullOrWhiteSpace(this.DataBaseItemForHm.DbPort)
                || string.IsNullOrWhiteSpace(this.DataBaseItemForHm.DbUser)
                || string.IsNullOrWhiteSpace(this.DataBaseItemForHm.DbPwd)
                || string.IsNullOrWhiteSpace(this.DataBaseItemForHm.DbType))
            {
                return false;
            }
            return true;
        }
    }
}
