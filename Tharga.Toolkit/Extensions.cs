using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Tharga.Toolkit
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class Extensions
    {
        public static IEnumerable<TTo> MapEnum<TTo, TFrom>(this IEnumerable<TFrom> from) where TTo : struct
        {
            return from.Select(item => item.MapEnum<TTo, TFrom>());
        }

        public static TTo MapEnum<TTo, TFrom>(this TFrom from) where TTo : struct
        {
            if (!typeof(TTo).IsEnum) throw new InvalidOperationException(string.Format("The to-type is not an enum, it is of type {0}.", typeof(TTo)));
            if (!typeof(TFrom).IsEnum) throw new InvalidOperationException(string.Format("The from-type is not an enum, it is of type {0}.", typeof(TTo)));

            try
            {
                return (TTo)Enum.Parse(typeof(TTo), from.ToString());
            }
            catch
            {
                throw new InvalidOperationException(string.Format("Cannot convert {0} from enum {1} to enum {2}.", from, typeof(TFrom), typeof(TTo)));
            }
        }

        public static T TakeRandom<T>(this IEnumerable<T> values)
        {
            var list = values.ToList();
            if (!list.Any()) return default(T);
            var index = RandomUtility.GetRandomInt(0, list.Count());
            return list[index];
        }

        public static IEnumerable<T> TakeAllButLast<T>(this IEnumerable<T> values)
        {
            var it = values.GetEnumerator();
            bool hasRemainingItems;
            var isFirst = true;
            var item = default(T);
            do
            {
                hasRemainingItems = it.MoveNext();
                if (hasRemainingItems)
                {
                    if (!isFirst) yield return item;
                    item = it.Current;
                    isFirst = false;
                }
            } while (hasRemainingItems);
        }

        public static string ToDateTimeString(this DateTime dateTime)
        {
            return string.Format("{0} {1}", dateTime.ToShortDateString(), dateTime.ToLongTimeString());
        }

        public static string ToTimeString(this TimeSpan timeSpan)
        {
            return string.Format("{0}:{1}:{2}", timeSpan.Hours, timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
        }
    }
}
