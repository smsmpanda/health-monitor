using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Domain
{
    public class LinkAlarmViewModel : ViewModelBase
    {
        public LinkAlarmViewModel()
        {
            Debug.WriteLine($"{nameof(LinkAlarmViewModel)}::ctor->{this.GetHashCode()}");
        }
        public string AlarmSettingAddress =>
            ConfigurationManager.AppSettings["AlarmSettingAddress"];
    }
}
