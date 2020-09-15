using System;

namespace TradingViewUdfProvider.Utils
{
    public static class TvDateTimeConverter
    {
        /// <summary>
        /// Unix base datetime (1.1. 1970)
        /// </summary>
        public static readonly DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        ///// <summary>
        ///// Convert from unix seconds into DateTime with high resolution (6 decimal places for milliseconds)
        ///// </summary>
        //public static DateTime ConvertFromUnixSeconds(double timeInSec)
        //{
        //    var unixTimeStampInTicks = (long)(timeInSec * TimeSpan.TicksPerSecond);
        //    return new DateTime(UnixBase.Ticks + unixTimeStampInTicks, DateTimeKind.Utc);
        //}

        ///// <summary>
        ///// Convert from unix seconds into DateTime with high resolution (6 decimal places for milliseconds)
        ///// </summary>
        //public static DateTime? ConvertFromUnixSeconds(double? timeInSec)
        //{
        //    if (!timeInSec.HasValue)
        //        return null;
        //    return ConvertFromUnixSeconds(timeInSec.Value);
        //}

        /// <summary>
        /// Convert from unix seconds into DateTime with high resolution (6 decimal places for milliseconds)
        /// </summary>
        public static DateTime ConvertFromUnixSeconds(double timeInSec)
        {
            var unixTimeStampInTicks = (long)(timeInSec * TimeSpan.TicksPerSecond);
            return new DateTime(UnixBase.Ticks + unixTimeStampInTicks, DateTimeKind.Utc);
            //return UnixBase.AddSeconds(timeInSec);
        }

        /// <summary>
        /// Convert from unix seconds into DateTime with high resolution (6 decimal places for milliseconds)
        /// </summary>
        public static DateTime? ConvertFromUnixSeconds(double? timeInSec)
        {
            if (!timeInSec.HasValue)
                return null;
            return ConvertFromUnixSeconds(timeInSec.Value);
        }

        /// <summary>
        /// Convert DateTime into unix seconds with high resolution (6 decimal places for milliseconds)
        /// </summary>
        public static double ToUnixSeconds(this DateTime date)
        {
            var unixTimeStampInTicks = (date.ToUniversalTime() - UnixBase).Ticks;
            return (double)unixTimeStampInTicks / TimeSpan.TicksPerSecond;
            //var elapsedTime = date - UnixBase;
            //return (long)elapsedTime.TotalSeconds;
        }

        /// <summary>
        /// Convert DateTime into unix seconds with high resolution (6 decimal places for milliseconds)
        /// </summary>
        public static double? ToUnixSeconds(this DateTime? date)
        {
            if (!date.HasValue)
                return null;
            return ToUnixSeconds(date.Value);
        }

        ///// <summary>
        ///// Convert DateTime into unix seconds with high resolution (6 decimal places for milliseconds)
        ///// </summary>
        //public static double ToUnixMilliseconds(this DateTime date)
        //{
        //    var unixTimeStampInTicks = (date.ToUniversalTime() - UnixBase).Ticks;
        //    return (double)unixTimeStampInTicks / TimeSpan.TicksPerMillisecond;
        //}

        ///// <summary>
        ///// Convert DateTime into unix seconds with high resolution (6 decimal places for milliseconds)
        ///// </summary>
        //public static double? ToUnixMilliseconds(this DateTime? date)
        //{
        //    if (!date.HasValue)
        //        return null;
        //    return ToUnixMilliseconds(date.Value);
        //}
    }
}
