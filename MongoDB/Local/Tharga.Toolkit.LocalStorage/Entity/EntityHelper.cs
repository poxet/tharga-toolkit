using System;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    static class EntityHelper
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value) || value.Trim() == String.Empty;
        }
    }
}