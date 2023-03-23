﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Services
{
    /// <summary>
    /// 数据库连接模板
    /// </summary>
    public class DbConnectionTemplate
    {
        public const string MYSQL = "Data Source={0};Port={1};Initial Catalog={2};User Id={3};Password={4};Trusted_Connection=True;";

        public const string MSSQL = "Server={0};Port={1};Database={2};charset=utf8;uid={3};pwd={4};";

        public const string ORACLE = "Data Source={0}:{1}/{2};Persist Security Info=True;User ID={3};Password={4};Connection Timeout=3;";


        public static string CreateOracleString(DbConfig config)
        {
            return string.Format(ORACLE, config.Host, config.Port, config.DbCatalog, config.User, config.Password);
        }

        public static string CreateMySQLString(DbConfig config)
        {
            return string.Format(MYSQL, config.Host, config.Port, config.DbCatalog, config.User, config.Password);
        }

        public static string CreateMSSQLString(DbConfig config)
        {
            return string.Format(MSSQL, config.Host, config.Port, config.DbCatalog, config.User, config.Password);
        }
    }
}
