using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Utility
{
    public class DateTimeHelper
    {
        /// <summary>
        /// 一周中的星期几
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentChineseDayOfWeek()
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("zh-CN");
            DateTimeFormatInfo dateTimeFormat = culture.DateTimeFormat;
            string currentChineseDayOfWeek = dateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
            return currentChineseDayOfWeek;
        }
    }
}
