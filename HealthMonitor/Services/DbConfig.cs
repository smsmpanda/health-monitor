using HealthMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Services
{
    public class DbConfig
    {
        public DbType DbType { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string DbCatalog { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

    }
}
