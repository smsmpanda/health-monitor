using HealthMonitor.Repository;
using HealthMonitor.Utility;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthMonitor.Domain
{
    public class GateMonitorViewModel : ViewModelBase
    {
        private bool _isBottomDrawOpen;
        private GateMonitorModel _currentGate;
        private ObservableCollection<GateMonitorModel> _gates;
        private GateMonitorModel _selectGate;

        public GateMonitorViewModel()
        {
            _gates = new ObservableCollection<GateMonitorModel>();
            OneClickMonitorCommand = new DelegateCommand<bool?>(OneClickMoniting);
            StartUpMonitorCommand = new DelegateCommand<bool?>(StartUpMoniting);
            GetGatesAsync();
            GetAreasAsync();
        }

        public GateMonitorModel CurrentGate
        {
            get => _currentGate;
            set => SetProperty(ref _currentGate, value);
        }

        public bool IsBottomDrawOpen
        {
            get => _isBottomDrawOpen;
            set => SetProperty(ref _isBottomDrawOpen, value);
        }

        public GateMonitorModel SelectGate
        {
            get => _selectGate;
            set => SetProperty(ref _selectGate, value);
        }

        /// <summary>
        /// 限员屏数据
        /// </summary>
        public ObservableCollection<GateMonitorModel> Gates
        {
            get => _gates;
            set => SetProperty(ref _gates, value);
        }

        /// <summary>
        /// 区域数据
        /// </summary>
        public IEnumerable<AreaModel> AreaItems { get; private set; }

        /// <summary>
        /// 添加或更新闸门信息
        /// </summary>
        public ICommand SaveGateCommand =>
            new AnotherCommandImplementation(SaveGate, CanSaveGate);

        public ICommand UpdateGateCommand =>
            new AnotherCommandImplementation(UpdateGate);

        public ICommand DeleteGateCommand =>
            new AnotherCommandImplementation(DeleteGate);

        public ICommand ExpandBottomDrawCommand =>
           new AnotherCommandImplementation(ExpandBottomDraw);

        public DelegateCommand<bool?> OneClickMonitorCommand { get; }

        public DelegateCommand<bool?> StartUpMonitorCommand { get; }

        public void ExpandBottomDraw(object m)
        {
            this.CurrentGate = new GateMonitorModel();
            IsBottomDrawOpen = !IsBottomDrawOpen;
        }

        private void OneClickMoniting(bool? flag)
        {
            Parallel.ForEach(this.Gates, db => db.StartUp(flag));
        }

        private void StartUpMoniting(bool? flag)
        {
            this.SelectGate.StartUp(flag);
        }

        /// <summary>
        /// 更新|新增闸门
        /// </summary>
        /// <param name="m"></param>
        public async void SaveGate(object m)
        {
            try
            {
                if (CurrentGate.ID == 0)
                {
                    await RYDWDbContext.InsertGateAsync(CurrentGate.ToMapEntity());
                }
                else
                {
                    await RYDWDbContext.UpdateGateAsync(CurrentGate.ToMapEntity());
                }
                GetGatesAsync(() => this.IsBottomDrawOpen = false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "添加或更新限员屏发生异常.");
            }
        }

        private bool CanSaveGate(object arg)
        {
            if (string.IsNullOrWhiteSpace(this.CurrentGate?.IP)
                   || string.IsNullOrWhiteSpace(this.CurrentGate?.NAME)
                   || this.CurrentGate?.AREA is null)
            {
                return false;
            }
            return true;
        }

        private void UpdateGate(object obj)
        {
            this.CurrentGate = (GateMonitorModel)obj;
            this.CurrentGate.AREA = this.AreaItems.FirstOrDefault(a => a.ID == this.CurrentGate.AREA.ID);
            this.IsBottomDrawOpen = true;
        }

        private async void DeleteGate(object obj)
        {
            try
            {
                int id = ((GateMonitorModel)obj).ID;
                await RYDWDbContext.DeletePassengerLimitAsync(id);
                this.Gates.Remove(this.Gates.FirstOrDefault(s => s.ID == id));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "删除限员屏发生异常.");
            }
        }


        private async void GetGatesAsync(Action after = null)
        {
            var Gates = await RYDWDbContext.GetGatesAsync();
            if (Gates != null && Gates.Any())
            {
                this.Gates = new ObservableCollection<GateMonitorModel>(Gates.Select(s => s.ToMapViewModel()));
                this.SelectGate = this.SelectGate ?? this.Gates[0];
            }
            after?.Invoke();
        }

        private async void GetAreasAsync()
        {
            this.AreaItems = await RYDWDbContext.GetAreasAsync();
        }
    }
}
