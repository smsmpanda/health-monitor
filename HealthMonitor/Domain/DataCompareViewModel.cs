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
        private DataCompareDbItem _dbcItemA;
        private DataCompareDbItem _dbcItemB;
        public DataCompareViewModel()
        {
            DbCompareTasks = new ObservableCollection<DataCompareTask>()
            {
                new DataCompareTask("任务1",DateTime.Now,null,null),
                new DataCompareTask("任务2",DateTime.Now,null,null),
                new DataCompareTask("任务3",DateTime.Now,null,null),
                new DataCompareTask("任务4",DateTime.Now,null,null),
                new DataCompareTask("任务4",DateTime.Now,null,null),
                new DataCompareTask("任务4",DateTime.Now,null,null),
                new DataCompareTask("任务4",DateTime.Now,null,null),
                new DataCompareTask("任务4",DateTime.Now,null,null),
                new DataCompareTask("任务4",DateTime.Now,null,null),
                new DataCompareTask("任务4",DateTime.Now,null,null),
                new DataCompareTask("任务4",DateTime.Now,null,null),
                new DataCompareTask("任务4",DateTime.Now,null,null),
            };

            _dbcItemA = new DataCompareDbItem();
            _dbcItemB = new DataCompareDbItem();
        }


        public ObservableCollection<DataCompareTask> DbCompareTasks { get; }


        public DataCompareDbItem DbcItemA
        {
            get => _dbcItemA;
            set => _dbcItemA = value;
        }

        public DataCompareDbItem DbcItemB
        {
            get => _dbcItemB;
            set => _dbcItemB = value;
        }

        public List<string> DbTypeItems => new List<string> { $"{DbType.MYSQL}", $"{DbType.ORACLE}", $"{DbType.MSSQL}" };

        public ICommand AddNewDbCompareCommand => new AnotherCommandImplementation(AddNewDbCompare, CanExecuteAddNewDbCompare);
        public ICommand DataTestConnectionCommand => new AnotherCommandImplementation(DataBaseConnection, CanExecuteAddNewDbCompare);

        public void AddNewDbCompare(Object m)
        {
            this.DbCompareTasks.Add(new DataCompareTask($"比对{DateTime.Now:G}", DateTime.Now, this.DbcItemA, this.DbcItemB));
        }

        public void DataBaseConnection(object m)
        {
            Task.Factory.StartNew(async () =>
            {

                foreach (var dbItem in new List<DataCompareDbItem> { DbcItemA, DbcItemB })
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
            if (string.IsNullOrWhiteSpace(this.DbcItemA.DbHost)
                || string.IsNullOrWhiteSpace(this.DbcItemA.DbPort)
                || string.IsNullOrWhiteSpace(this.DbcItemA.DbUser)
                || string.IsNullOrWhiteSpace(this.DbcItemA.DbPwd)
                || string.IsNullOrWhiteSpace(this.DbcItemA.DbType)
                || string.IsNullOrWhiteSpace(this.DbcItemB.DbHost)
                || string.IsNullOrWhiteSpace(this.DbcItemB.DbPort)
                || string.IsNullOrWhiteSpace(this.DbcItemB.DbUser)
                || string.IsNullOrWhiteSpace(this.DbcItemB.DbPwd)
                || string.IsNullOrWhiteSpace(this.DbcItemB.DbType))
            {
                return false;
            }
            return true;
        }
    }
}
