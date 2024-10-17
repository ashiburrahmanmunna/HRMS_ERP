using System;

namespace GTERP.Models.Common
{
    public static class DateFormatextension
    {
        public static string ToFormatslasMMddyyyy(this DateTime dateTime)
        {
            return $"{dateTime.Month}/{dateTime.Day}/{dateTime.Year}";

        }
    }
}
