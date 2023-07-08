using System;

namespace KdyWeb.Utility
{
    /// <summary>
    /// 时间扩展
    /// </summary>
    public static class DateTimeExt
    {
        /// <summary>
        /// 转秒 时间戳
        /// </summary>
        /// <returns></returns>
        public static long ToSecondTimestamp(this DateTime date)
        {
            return (date.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        /// <summary>
        /// 转毫秒 时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long ToMillisecondTimestamp(this DateTime date)
        {
            return (date.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        }

        /// <summary>
        /// 秒时间戳转 时间
        /// </summary>
        /// <returns></returns>
        public static DateTime ToDateTimeBySeconds(this long timestamp)
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return dateTimeOffset.UtcDateTime;
        }

        /// <summary>
        /// 毫秒时间戳转 时间
        /// </summary>
        /// <returns></returns>
        public static DateTime ToDateTimeByMilliseconds(this long timestamp)
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
            return dateTimeOffset.UtcDateTime;
        }
    }
}
