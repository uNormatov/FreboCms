using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCore.Helper
{
    public class UzbKeywordHelper
    {
        public static string GetDateString(DateTime from, DateTime to)
        {
            TimeSpan span = to - from;
            if (span.Days > 31)
                return string.Format("{0} oy avval.", span.Days / 30);
            if (span.Days > 0)
                return string.Format("{0} kun avval.", span.Days);
            if (span.Minutes > 0)
                return string.Format("{0} daqiqa avval.", span.Minutes);
            if (span.Seconds > 0)
                return string.Format("{0} soniya avval.", span.Seconds);
            return "Hozirgina e'lon qilindi.";
        }

    }
}
