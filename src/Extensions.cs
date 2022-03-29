using System;

namespace moderndrummer.binance
{
    public static class Extensions
    {
        public static DateTime ToDateTime(this string ticks)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(long.Parse(ticks));
        }

        public static string ToMinStringLength(this string value, short minLength)
        {
            return value.PadLeft(minLength, ' ');
        }


    }
}