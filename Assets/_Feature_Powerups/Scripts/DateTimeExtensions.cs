using System;

namespace ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static int ToEpoch(this DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)timeSpan.TotalSeconds;
            return secondsSinceEpoch;
        }
        
        public static DateTime ToDateTime(int epochTime)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(epochTime);
            DateTime dateTime = dateTimeOffset.UtcDateTime;
            return dateTime;
        }
    }
}