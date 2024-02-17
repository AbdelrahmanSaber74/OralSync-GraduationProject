using System;

namespace SharedClassLibrary.Helper
{
    public static class DateTimeHelper
    {
        public static string FormatDate(DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/dd");
        }

        public static string FormatTime(DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

        // Additional method to get time in a specific time zone
        public static DateTime GetTimeInTimeZone(string timeZoneId)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTime timeInTimeZone = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            return timeInTimeZone;
        }
    }
}
