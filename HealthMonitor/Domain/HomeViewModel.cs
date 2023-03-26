using System.Windows.Input;

namespace HealthMonitor.Domain
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel()
        {

        }
        public ICommand OpenSample4DialogCommand { get; }
        public ICommand AcceptSample4DialogCommand { get; }
        public ICommand CancelSample4DialogCommand { get; }
    }
}
