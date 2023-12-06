using HealthMonitor.Enums;

namespace HealthMonitor.Model.Common
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
