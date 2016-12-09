using System;

namespace Tharga.Toolkit.Registry
{
    internal static class LevelConverter
    {
        public static RegistryHKey ToLevel(this ELocalLevel level)
        {
            RegistryHKey hkey;
            switch (level)
            {
                case ELocalLevel.CurrentUser:
                    hkey = RegistryHKey.CurrentUser;
                    break;
                case ELocalLevel.LocalMachine:
                    hkey = RegistryHKey.LocalMachine;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown level {level}.");
            }
            return hkey;
        }
    }
}