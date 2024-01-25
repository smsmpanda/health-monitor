using DryIoc;
using HealthMonitor.Repository;
using HealthMonitor.Utility;
using HealthMonitor.ViewModel;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using static Slapper.AutoMapper;

namespace HealthMonitor.Domain
{
    public class PassengerLimitScreenViewModel : ViewModelBase
    {
        private bool _isBottomDrawOpen;
        private ScreenModel _currentScreen;
        private ObservableCollection<ScreenModel> _screens;
        private ScreenModel _selectScreen;

        public PassengerLimitScreenViewModel()
        {
            _screens = new ObservableCollection<ScreenModel>();
            OneClickMonitorCommand = new DelegateCommand<bool?>(OneClickMoniting);

            GetScreensAsync();
            GetAreasAsync();
        }

        public ScreenModel CurrentScreen
        {
            get => _currentScreen;
            set => SetProperty(ref _currentScreen, value);
        }

        public bool IsBottomDrawOpen
        {
            get => _isBottomDrawOpen;
            set => SetProperty(ref _isBottomDrawOpen, value);
        }

        /// <summary>
        /// 限员屏数据
        /// </summary>
        public ObservableCollection<ScreenModel> Screens
        {
            get => _screens;
            set => SetProperty(ref _screens, value);
        }


        public ScreenModel SelectScreen
        {
            get => _selectScreen;
            set => SetProperty(ref _selectScreen, value);
        }

        /// <summary>
        /// 区域数据
        /// </summary>
        public IEnumerable<AreaModel> AreaItems { get; private set; }

        /// <summary>
        /// 添加或更新屏幕信息
        /// </summary>
        public ICommand SaveScreenCommand =>
            new AnotherCommandImplementation(SaveScreen,CanSaveScreen);

        public ICommand UpdateScreenCommand =>
            new AnotherCommandImplementation(UpdateScreen);

        public ICommand DeleteScreenCommand =>
            new AnotherCommandImplementation(DeleteScreen);

        public ICommand ExpandBottomDrawCommand =>
           new AnotherCommandImplementation(ExpandBottomDraw);

        public DelegateCommand<bool?> OneClickMonitorCommand { get; }

        public void ExpandBottomDraw(object m)
        {
            this.CurrentScreen = new ScreenModel();
            IsBottomDrawOpen = !IsBottomDrawOpen;
        }

        private void OneClickMoniting(bool? flag)
        {
            Parallel.ForEach(this.Screens, db => db.STARTUP = flag.Value);
        }

        /// <summary>
        /// 更新|新增屏幕
        /// </summary>
        /// <param name="m"></param>
        public async void SaveScreen(object m)
        {
            try
            {
                if (CurrentScreen.ID == 0)
                {
                    await RYDWDbContext.InsertPassengerLimitScreenAsync(CurrentScreen.ToMapEntity());
                }
                else
                {
                    await RYDWDbContext.UpdatePassengerLimitScreenAsync(CurrentScreen.ToMapEntity());
                }
                GetScreensAsync(() => this.IsBottomDrawOpen = false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "添加或更新限员屏发生异常.");
            }
        }

        private bool CanSaveScreen(object arg)
        {
            if (string.IsNullOrWhiteSpace(this.CurrentScreen?.IP)
                   || string.IsNullOrWhiteSpace(this.CurrentScreen?.WORKFACENAME)
                   || string.IsNullOrWhiteSpace(this.CurrentScreen?.NAME)
                   || this.CurrentScreen?.AREA is null)
            {
                return false;
            }
            return true;
        }

        private void UpdateScreen(object obj)
        {
            this.CurrentScreen = (ScreenModel)obj;
            this.CurrentScreen.AREA = this.AreaItems.FirstOrDefault(a => a.ID == this.CurrentScreen.AREA.ID);
            this.IsBottomDrawOpen = true;
        }

        private async void DeleteScreen(object obj)
        {
            try
            {
                int id = ((ScreenModel)obj).ID;
                await RYDWDbContext.DeletePassengerLimitAsync(id);
                this.Screens.Remove(this.Screens.FirstOrDefault(s => s.ID == id));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "删除限员屏发生异常.");
            }
        }


        private async void GetScreensAsync(Action after = null)
        {
            var screens = await RYDWDbContext.GetPassengerLimitScreensAsync();
            if (screens != null && screens.Any())
            {
                this.Screens = new ObservableCollection<ScreenModel>(screens.Select(s => s.ToMapViewModel()));
                this.SelectScreen = this.SelectScreen ?? this.Screens[0];
            }
            after?.Invoke();
        }

        private async void GetAreasAsync() 
        {
            this.AreaItems = await RYDWDbContext.GetAreasAsync();
        }
    }
}
