﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Domain
{
    public class DataCompareDbItem : ViewModelBase
    {
        private string _dbHost;
        public string DbHost
        {
            get => _dbHost;
            set => SetProperty(ref _dbHost, value);
        }

        private string _dbPort;
        public string DbPort
        {
            get => _dbPort;
            set => SetProperty(ref _dbPort, value);
        }

        private string _dbCatalog;
        public string DbCatalog
        {
            get => _dbCatalog;
            set => SetProperty(ref _dbCatalog, value);
        }

        private string _dbUser;

        public string DbUser
        {
            get => _dbUser;
            set => SetProperty(ref _dbUser, value);
        }

        private string _dbPwd;

        public string DbPwd
        {
            get => _dbPwd;
            set => SetProperty(ref _dbPwd, value);
        }

        private string _dbType;

        public string DbType
        {
            get => _dbType;
            set => SetProperty(ref _dbType, value);
        }

        private bool _dbStatus;

        public bool DbStatus
        {
            get => _dbStatus;
            set => SetProperty(ref _dbStatus, value);
        }

        private string _dbTestMessage;

        public string DbTestMessage
        {
            get => _dbTestMessage;
            set => SetProperty(ref _dbTestMessage, value);
        }
    }
}
