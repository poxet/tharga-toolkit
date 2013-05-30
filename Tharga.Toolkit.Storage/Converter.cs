using System;

namespace Tharga.Toolkit.Storage
{
    public static class Converter
    {
        public static string ToShortString(this Type type)
        {
            var fullName = type.ToString();
            var pos = fullName.LastIndexOf(".", StringComparison.Ordinal);
            return fullName.Substring(pos + 1);
        }
    }
}