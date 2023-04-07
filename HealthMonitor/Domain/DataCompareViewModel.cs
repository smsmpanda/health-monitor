using HealthMonitor.Enums;
using HealthMonitor.Extensions;
using HealthMonitor.Services;
using HealthMonitor.Services.imp;
using Magicodes.ExporterAndImporter.Excel;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace HealthMonitor.Domain
{
    public class DataCompareViewModel : NavigationViewModel
    {
        private bool _isBottomDrawOpen;
        private bool _isLeftDrawOpen;
        private DataBaseItem _dataBaseDw;
        private DataBaseItem _dataBaseHm;
        private int _totalCount;
        private DataCompareFilter _filters;
        private CompareDataFilter _compareDataFilter;
        private IEnumerable<DwInOutwellModel> CompareResult;
        public ObservableCollection<DwInOutwellModel> _inOutWellList;
        public DataCompareViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
            _dataBaseDw = new DataBaseItem().SettingDefault("1521", "mkgk", "MKDB3D", Enum.GetName(typeof(DbType), DbType.ORACLE));
            _dataBaseHm = new DataBaseItem().SettingDefault("1433", "sa", "kaoqin", Enum.GetName(typeof(DbType), DbType.MSSQL));
            _filters = new DataCompareFilter();
            _compareDataFilter = new CompareDataFilter();
            //_inOutWellList = new ObservableCollection<DwInOutwellModel>()
            //{
            //    new DwInOutwellModel { HmResult="已出井",EmployeeID=12,EmployeeName="张三1",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="未匹配",EmployeeID=12,EmployeeName="张三2",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="已出井",EmployeeID=12,EmployeeName="张三3",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="未匹配",EmployeeID=12,EmployeeName="张三4",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="已出井",EmployeeID=12,EmployeeName="张三5",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="未匹配",EmployeeID=12,EmployeeName="张三6",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="已出井",EmployeeID=12,EmployeeName="张三8",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="未匹配",EmployeeID=12,EmployeeName="张三9",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="未匹配",EmployeeID=12,EmployeeName="张三10",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="已出井",EmployeeID=12,EmployeeName="张三11",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="已出井",EmployeeID=12,EmployeeName="张三12",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="已出井",EmployeeID=12,EmployeeName="张三13",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="未匹配",EmployeeID=12,EmployeeName="张三15",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="未匹配",EmployeeID=12,EmployeeName="张三16",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="未匹配",EmployeeID=12,EmployeeName="张三17",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="已出井",EmployeeID=12,EmployeeName="张三18",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="未匹配",EmployeeID=12,EmployeeName="张三19",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="未匹配",EmployeeID=12,EmployeeName="张三20",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" },
            //    new DwInOutwellModel { HmResult="已出井",EmployeeID=12,EmployeeName="张三21",DwInwellTime=DateTime.Now,DepartMentName="信息科",GroupClass="班组1",HmInwellTime=DateTime.Now,HmOutwellTime=DateTime.Now,TagMac="12313" }
            //};
            //CompareResult = _inOutWellList;
            _inOutWellList = new ObservableCollection<DwInOutwellModel>();
            CompareResult = new List<DwInOutwellModel>();
        }

        public DataBaseItem DataBaseDw
        {
            get => _dataBaseDw;
            set => SetProperty(ref _dataBaseDw, value);
        }
        public int TotalCount
        {
            get => _totalCount;
            set => SetProperty(ref _totalCount, value);
        }
        public DataBaseItem DataBaseHm
        {
            get => _dataBaseHm;
            set => SetProperty(ref _dataBaseHm, value);
        }
        public ObservableCollection<DwInOutwellModel> InOutWellList
        {
            get => _inOutWellList;
            set
            {
                TotalCount = value.Count;
                foreach (var item in value)
                {
                    item.Id = value.IndexOf(item) + 1;
                }
                SetProperty(ref _inOutWellList, value);
            }
        }
        public DataCompareFilter Filters
        {
            get => _filters;
            set => SetProperty(ref _filters, value);
        }
        public CompareDataFilter DataFilters
        {
            get => _compareDataFilter;
            set => SetProperty(ref _compareDataFilter, value);
        }
        public bool IsBottomDrawOpen
        {
            get => _isBottomDrawOpen;
            set => SetProperty(ref _isBottomDrawOpen, value);
        }
        public bool IsLeftDrawOpen
        {
            get => _isLeftDrawOpen;
            set => SetProperty(ref _isLeftDrawOpen, value);
        }

        public List<string> DbTypeItems => new List<string> { $"{DbType.MYSQL}", $"{DbType.ORACLE}", $"{DbType.MSSQL}" };


        public ICommand StartNewCompareCommand =>
            new AnotherCommandImplementation(StartNewCompareExcute, CanExceuteStartCompare);
        public ICommand SearchDataCommand =>
            new AnotherCommandImplementation(QuyerDataExecute, CanExecuteExistsData);
        public ICommand DataTestConnectionCommand =>
            new AnotherCommandImplementation(DataBaseConnectionExecute, CanExecuteTestConnection);
        public ICommand ExpandBottomDrawCommand =>
            new AnotherCommandImplementation(ExpandBottomDraw);
        public ICommand ExpandLeftDrawCommand =>
            new AnotherCommandImplementation(ExpandLeftDraw);
        public ICommand ExportExcelCommand =>
            new AnotherCommandImplementation(ExportExcelAsyncExecute, CanExecuteExistsData);


        public void ExpandBottomDraw(object m)
        {
            IsBottomDrawOpen = !IsBottomDrawOpen;
        }
        public void ExpandLeftDraw(object m)
        {
            IsLeftDrawOpen = !IsLeftDrawOpen;
        }


        /// <summary>
        //  比对
        /// </summary>
        /// <param name="m"></param>
        public void StartNewCompareExcute(object m)
        {
            try
            {
                UpdateLoading(true);
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
                            CompareResult = await service.StartCompareAsync();

                            //部门
                            DataFilters.DepartMents = new ObservableCollection<string>(CompareResult.Select(x => x.DepartMentName).Distinct().OrderBy(x=>x));

                            InOutWellList = new ObservableCollection<DwInOutwellModel>(CompareResult);

                            //关闭底部比对栏
                            IsBottomDrawOpen = false;
                        }
                        catch (Exception ex)
                        {
                            DataBaseDw.DbStatus = false;
                            DataBaseDw.DbTestMessage = ex.Message;
                        }
                    }
                    UpdateLoading(false);
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool CanExceuteStartCompare(object _)
        {
            bool _f = CanExecuteTestConnection(_);
            return DataBaseHm.DbStatus && DataBaseDw.DbStatus && _f;
        }

        /// <summary>
        /// 数据库测试连接
        /// </summary>
        /// <param name="m"></param>
        public void DataBaseConnectionExecute(object m)
        {
            UpdateLoading(true);
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    foreach (var dbItem in new DataBaseItem[] { DataBaseDw, DataBaseHm })
                    {

                        DbConfig config = ManualMapperExtension.DbItemMapperDbConfig(dbItem);
                        (dbItem.DbStatus, dbItem.DbTestMessage) = await DbFactory.DbConnectionTestAsync(config);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    UpdateLoading(false);
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


        /// <summary>
        /// 数据检索
        /// </summary>
        /// <param name="m"></param>
        public void QuyerDataExecute(object m)
        {
            try
            {
                UpdateLoading(true);

                InOutWellList = new ObservableCollection<DwInOutwellModel>(CompareResult);

                if (!string.IsNullOrWhiteSpace(DataFilters.EmployeeName))
                {
                    this.InOutWellList = new ObservableCollection<DwInOutwellModel>(
                    InOutWellList.Where(dw => dw.EmployeeName.Contains(DataFilters.EmployeeName)));
                }
                if (!string.IsNullOrWhiteSpace(DataFilters.Tagmac))
                {
                    this.InOutWellList = new ObservableCollection<DwInOutwellModel>(
                    InOutWellList.Where(dw => dw.TagMac.Contains(DataFilters.Tagmac)));
                }
                if (!string.IsNullOrWhiteSpace(DataFilters.DepartMent))
                {
                    this.InOutWellList = new ObservableCollection<DwInOutwellModel>(
                   InOutWellList.Where(dw => dw.DepartMentName.Equals(DataFilters.DepartMent)));
                }

                if (DataFilters.ResultType != null)
                {
                    this.InOutWellList = new ObservableCollection<DwInOutwellModel>(
                   InOutWellList.Where(dw => dw.HmResult.Equals(DataFilters.ResultType?.Description)));
                }

            }
            catch (Exception)
            {
                //throw;
            }
            finally 
            {
                UpdateLoading(false);
            }
        }
        public bool CanExecuteExistsData(object m)
        {
            return CompareResult.Any();
        }


        /// <summary>
        /// 报表导出
        /// </summary>
        /// <param name="m"></param>
        public void ExportExcelAsyncExecute(object m)
        {
            string fileName = $"定位虹膜出井比对{DateTime.Now:yyMMddHHmmss}.xlsx";

            var exporter = new ExcelExporter();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel文件|*.xlsx|所有文件|*.*";
            saveFileDialog.FileName = fileName;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                UpdateLoading(true);
                Task.Factory.StartNew(async () =>
                {
                    await exporter.Export(saveFileDialog.FileName, InOutWellList);
                    UpdateLoading(false);
                });
            }
        }
    }
}
