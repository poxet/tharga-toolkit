using System;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.StorageConsole.Utility
{
    static class ColorHelper
    {
        public static ConsoleColor GetColor(this IEntity entity)
        {
            return entity.StoreInfo.IsInSync ? ConsoleColor.Gray : ConsoleColor.DarkGray;
        }
    }
}