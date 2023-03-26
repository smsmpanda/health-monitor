using HealthMonitor.Domain;
using HealthMonitor.Enums;
using HealthMonitor.Services;
using System;

namespace HealthMonitor.Extensions
{
    public static class ManualMapperExtension
    {
        internal static DbConfig DbItemMapperDbConfig(this DataBaseItem dbItem)
        {
            DbConfig config = new DbConfig()
            {
                DbType = (DbType)Enum.Parse(typeof(DbType), dbItem.DbType, true),
                DbCatalog = dbItem.DbCatalog,
                Host = dbItem.DbHost,
                Port = dbItem.DbPort,
                Password = dbItem.DbPwd,
                User = dbItem.DbUser
            };
            return config;
        }
    }
}
