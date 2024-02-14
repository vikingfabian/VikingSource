using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;

namespace VikingEngine
{
    static class TimeExt
    {
        public const int SecondInMs = 1000;
        public const float MsToSec = 1f / SecondInMs;
        public const int MinuteInSeconds = 60;
        const int MinuteInMs = SecondInMs * MinuteInSeconds;

        public static float MinutesToMS(float min)
        {
            return min * MinuteInMs;
        }

        public static float SecondsToMS(float sec)
        {
            return sec * SecondInMs;
        }

        public static float SecondsToHours(float sec)
        {
            return sec / 3600;
        }

        public static float MillsSecToSec(float ms)
        {
            return ms / SecondInMs;
        }

        public static float ValuePerSec_toMs(float value)
        {
            return value * 0.001f;
        }


        public static string TimeAgo(DateTime time)
        {
            //const string ClockFormat = "hh:mm";

            DateTime now = DateTime.Now;
            if (time.DayOfYear == now.DayOfYear)
            {
                TimeSpan span = new TimeSpan(now.Ticks - time.Ticks);
                if (time.Hour == now.Hour && span.TotalMinutes <= 5)
                {
                    if (span.TotalMinutes < 1)
                    {
                        return span.Seconds.ToString() + "sec ago";
                    }
                    else
                    {
                        return span.Minutes.ToString() + "min ago";
                    }
                }
                else
                {
                    return clockFormat(time.TimeOfDay);
                }
            }
            else
            {
                return MonthNameShort(time.Month) + time.Day.ToString() + ", " + clockFormat(time.TimeOfDay);
            }
        }

        static string clockFormat(TimeSpan time)
        {
            StringBuilder sb = new StringBuilder();
            if (time.Hours < 10)
                sb.Append('0');
            sb.Append(time.Hours);
            sb.Append(':');
            if (time.Minutes < 10)
                sb.Append('0');
            sb.Append(time.Minutes);
            return sb.ToString();
        }

        public static string MonthNameShort(int month)
        {
            switch (month)
            {
                case 1:
                    return "Jan";
                case 2:
                    return "Feb";
                case 3:
                    return "Mar";
                case 4:
                    return "Apr";
                case 5:
                    return "May";
                case 6:
                    return "Jun";
                case 7:
                    return "Jul";
                case 8:
                    return "Aug";
                case 9:
                    return "Sep";
                case 10:
                    return "Oct";
                case 11:
                    return "Nov";
                case 12:
                    return "Dec";

            }
            throw new IndexOutOfRangeException();
        }
    }
}


