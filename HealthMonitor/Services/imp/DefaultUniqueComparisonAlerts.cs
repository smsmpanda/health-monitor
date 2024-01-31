using HealthMonitor.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Services.imp
{
    public abstract class DefaultUniqueComparisonAlerts : DefultPollingTaskService
    {
        public double ComparsionInterval
        {
            get
            {
                string intervalSetting = ApplicationConfig.GetValue("UniqueComparsionInterval");
                if (string.IsNullOrWhiteSpace(intervalSetting))
                {
                    return 30;
                }
                else
                {
                    return Convert.ToDouble(intervalSetting);
                }
            }
        }
    }
}
