using System;

namespace Tharga.Toolkit.StorageConsole.Utility
{
    static class Converter
    {
        public static string ToDateTimeString(this DateTime? value)
        {
            return value == null ? "N/A" : string.Format("{0} {1}", value.Value.ToShortDateString(), value.Value.ToLongTimeString());
        }
    }
}