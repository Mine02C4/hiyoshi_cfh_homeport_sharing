using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HiyoshiCfhWeb.Extensions
{
    public static class DateTimeExtensions
    {
        static TimeZoneInfo JapanStandardTime = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");

        public static DateTime UtcToJst(this DateTime date)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(date, JapanStandardTime);
        }

        public static DateTime JstToUtc(this DateTime date)
        {
            return TimeZoneInfo.ConvertTimeToUtc(date, JapanStandardTime);
        }
    }
}