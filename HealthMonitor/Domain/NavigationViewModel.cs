using HealthMonitor.Events;
using HealthMonitor.Extensions;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System;

namespace HealthMonitor.Domain
{
    public class NavigationViewModel : ViewModelBase, INavigationAware
    {
        private readonly IContainerProvider containerProvider;
        private readonly IEventAggregator aggregator;

        public NavigationViewModel(IContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
            this.aggregator = containerProvider.Resolve<IEventAggregator>();
        }
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public virtual void UpdateLoading(bool isOpen)
        {
            aggregator.UpdateLoading(new UpdateModel() { IsOpen = isOpen });
        }
    }
}
