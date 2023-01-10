using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.ViewModel
{
    /// <summary>
    /// 数据库健康监测类
    /// </summary>
    public class VMDatabase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 数据库IP
        /// </summary>
        private string _dbName;
        public string DbName
        {
            get { return this._dbName; }
            set
            {
                if (this._dbName != value)
                {
                    this._dbName = value;
                    this.NotifyPropertyChanged("DbName");
                }
            }
        }


        /// <summary>
        /// 数据库类型（Mysql/MSSQL/Oracl）
        /// </summary>
        private string _dbType;
        public string DbType
        {
            get { return this._dbType; }
            set
            {
                if (this._dbType != value)
                {
                    this._dbType = value;
                    this.NotifyPropertyChanged("DbType");
                }
            }
        }

        /// <summary>
        /// 数据库连接串
        /// </summary>
        private string _connectionString;
        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                if (this._connectionString != value)
                {
                    this._connectionString = value;
                    this.NotifyPropertyChanged("ConnectionString");
                }
            }
        }

        /// <summary>
        /// 是否开启监测
        /// </summary>
        private bool _isCheck;
        public bool IsCheck
        {
            get { return _isCheck; }
            set
            {
                if (this._isCheck != value)
                {
                    this._isCheck = value;
                    this.NotifyPropertyChanged("IsCheck");
                }
            }
        }

        /// <summary>
        /// 健康状态
        /// </summary>
        private bool _status;
        public bool Status
        {
            get { return _status; }
            set
            {
                if (this._status != value)
                {
                    this._status = value;
                    this.NotifyPropertyChanged("Status");
                }
            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
