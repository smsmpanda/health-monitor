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
    public class DataCompareDialogViewModel : ViewModelBase
    {
        private DataCompareDbItem _dbcItemA;
        private DataCompareDbItem _dbcItemB;

        public DataCompareDialogViewModel()
        {
            _dbcItemA = new DataCompareDbItem();
            _dbcItemB = new DataCompareDbItem();
            DbCompareTaskList = new ObservableCollection<DataCompareTask> { };
        }


        public ObservableCollection<DataCompareTask> DbCompareTaskList { get; }
        

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

        public List<string> DbTypeItems => new List<string> { "Oracle", "MSSQL", "MySQL" };

        public ICommand AddNewDbCompareCommand => new AnotherCommandImplementation(AddNewDbCompare, CanExecuteAddNewDbCompare);

        public void AddNewDbCompare(Object m)
        {
            DialogHost.Close("DbcRootDialog");
            this.DbCompareTaskList.Add(new DataCompareTask($"比对{DateTime.Now:G}", DateTime.Now, this.DbcItemA, this.DbcItemB));
            
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
