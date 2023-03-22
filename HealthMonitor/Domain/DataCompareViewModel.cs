using HealthMonitor.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthMonitor.Domain
{
    public class DataCompareViewModel : ViewModelBase
    {
        public DataCompareViewModel()
        {

        }


        public ICommand RunDialogCommand => new AnotherCommandImplementation(ExecuteRunDialog);

        private async void ExecuteRunDialog(object _)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new DataCompareDialog
            {
                DataContext = new DataCompareDialogViewModel()
            };

            //show the dialog
            var result = await DialogHost.Show(view, "DbcRootDialog", null, ClosingEventHandler, ClosedEventHandler);

            //check the result...
            Debug.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
            => Debug.WriteLine("You can intercept the closing event, and cancel here.");

        private void ClosedEventHandler(object sender, DialogClosedEventArgs eventArgs)
            => Debug.WriteLine("You can intercept the closed event here (1).");
    }
}
