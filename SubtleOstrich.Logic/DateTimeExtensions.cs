using System;

namespace SubtleOstrich.Logic
{
    public static class DateTimeExtensions
    {
        public static double ToUnixTimestamp(this DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static DateTime ToDateTime(this double unixTimeStamp)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return epoch.AddSeconds(unixTimeStamp);
        }
    }
}