using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Domain
{
    public class AreaModel : ViewModelBase
    {
        private string _name;

        public int ID { get; set; }
        public string NAME
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}
