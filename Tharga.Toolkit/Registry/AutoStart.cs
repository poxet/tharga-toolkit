using System;
using System.IO;
using System.Threading.Tasks;

namespace Tharga.Toolkit.Registry
{
    public class AutoStart
    {
        private readonly Setting _setting;
        public const string AutoStartLocation = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public AutoStart()
        {
            _setting = new Setting();
        }

        public async Task SetAutoStartAsync(RegistryHKey registryHKey, string keyName, string assemblyLocation)
        {
            if (!assemblyLocation.ToLower().EndsWith(".exe"))
                throw new InvalidOperationException($"The assembly {keyName} is not an executable (does not end with .exe), {assemblyLocation}.");

            if (!File.Exists(assemblyLocation))
                throw new InvalidOperationException("The assembly {keyName} does not exist at location {assemblyLocation}.");

            throw new NotImplementedException();
            //TODO: await _setting.SetSettingAsync(keyName, ELocalLevel.LocalMachine, ELocalLevel.LocalMachine, AutoStartLocation + @"\" + assemblyLocation);
        }

        public void RemoveAutoStart(RegistryHKey registryHKey, string keyName)
        {
            throw new NotImplementedException();
            //TODO: RemoveSetting(registryHKey, Registry.AutoStartLocation, keyName);
        }

        public bool IsAutoStartEnabled(RegistryHKey registryHKey, string keyName, string assemblyLocation)
        {
            throw new NotImplementedException();
            //TODO:
            //if (!HasSetting(registryHKey, Registry.AutoStartLocation, keyName))
            //    return false;
            //
            //var result = GetSetting(registryHKey, Registry.AutoStartLocation, keyName, null) as string;
            //return string.Compare(result, assemblyLocation, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
    }
}