using NLog;
using System;

namespace HealthMonitor.Utility
{
    public class Logger
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public static void Trace(string message)
        {
            _logger.Trace(message);
        }

        public static void Debug(string message)
        {
            _logger.Debug(message);
        }

        public static void Info(string message)
        {
            _logger.Info(message);
        }

        public static void Warn(string message)
        {
            _logger.Warn(message);
        }

        public static void Error(Exception ex, string message)
        {
            _logger.Error(ex,message);
        }
        public static void Error(string message)
        {
            _logger.Error(message);
        }

        public static void Error(Exception ex)
        {
            _logger.Error(ex);
        }

        public static void Fatal(string message)
        {
            _logger.Fatal(message);
        }
    }
}
