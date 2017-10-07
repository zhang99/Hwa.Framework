using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hwa
{
    public static class DateTimeExtension
    {     
        /// <summary>
        /// 日期转byte[]
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this DateTime dt)
        {
            return BitConverter.GetBytes(dt.Ticks);
        }

        /// <summary>
        /// 返回对应时区的时间
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime ZoneTime(this DateTime time, string timeZoneId)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTime(time, timeZone);
        }

        /// <summary>
        /// 返回对应时区的时间
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime TryZoneTime(this DateTime time, string timeZoneId)
        {
            try
            {
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return TimeZoneInfo.ConvertTime(time, timeZone);
            }
            catch { }
            return time;
        }

        /// <summary>
        /// 返回对应时区的时间
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime ZoneTime(this DateTime time, TimeZoneInfo timeZoneInfo)
        {
            return TimeZoneInfo.ConvertTime(time, timeZoneInfo);
        }

        /// <summary>
        /// 从byte[]获取日期
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static Nullable<DateTime> GetDateTime(byte[] bytes)
        {
            if (bytes != null)
            {
                long ticks = BitConverter.ToInt64(bytes, 0);
                if (ticks < DateTime.MaxValue.Ticks && ticks > DateTime.MinValue.Ticks)
                {
                    DateTime dt = new DateTime(ticks);
                    return dt;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取某天开始时间
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static DateTime GetDayStart(this DateTime dt)
        {
            return dt.Date;
        }

        /// <summary>
        /// 获取某天结束时间
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static DateTime GetDayEnd(this DateTime dt)
        {
            return dt.Date.AddDays(1).AddSeconds(-1);
        }

        /// <summary>
        /// 获取本月开始时间
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static DateTime GetMonthStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        /// <summary>
        /// 获取本月月末时间
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static DateTime GetMonthEnd(this DateTime dt)
        {
            DateTime nextFirst = dt.AddMonths(1).GetMonthStart();
            return new DateTime(dt.Year, dt.Month, nextFirst.AddDays(-1).Day, 23, 59, 59, 999);
        }

        /// <summary>
        /// 获取日期所属年份开始时间
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static DateTime GetYearStart(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }

        /// <summary>
        /// 获取日期所属年份截止时间
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static DateTime GetYearEnd(this DateTime dt)
        {
            DateTime nextFirst = dt.AddYears(1).GetYearStart();
            return nextFirst.AddMilliseconds(-1);
        }

        /// <summary>
        /// 获取周开始时间
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static DateTime GetWeekStart(this DateTime dt)
        {
            var weekIndex = (byte)dt.DayOfWeek;
            return dt.AddDays(-weekIndex);
        }

        /// <summary>
        /// 获取周结束时间
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static DateTime GetWeekEnd(this DateTime dt)
        {
            var weekIndex = (byte)dt.DayOfWeek;
            return dt.AddDays(6 - weekIndex);
        }
       
    }
}
