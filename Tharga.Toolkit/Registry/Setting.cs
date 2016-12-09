using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Tharga.Toolkit.Registry
{
    public class Setting
    {
        private readonly string _environment;

        public Setting(string environment = null)
        {
            _environment = environment;
        }

        public async Task<T> GetSettingAsync<T>(string keyName, ELocalLevel level = ELocalLevel.LocalMachine,
            T defaultValue = default(T))
        {
            var result = await Task.Run(() =>
            {
                var fullPath = GetFullPath(null);

                var key = GetKey(level.ToLevel(), fullPath);
                if (key == null)
                    throw new InvalidOperationException($"Cannot get key for registry path {fullPath}.");

                var value = key.GetValue(keyName);
                if (value == null)
                {
                    if (defaultValue == null)
                        throw new InvalidOperationException($"Cannot find setting for registry path {fullPath} and key {keyName} and there is no default value provided.");

                    key.SetValue(keyName, defaultValue);
                    return defaultValue;
                }
                return ConvertValue<T>(value.ToString());
            });

            return result;
        }

        public async Task SetSettingAsync<T>(string keyName, T value, ELocalLevel level, string subPath)
        {
            await Task.Run(() =>
            {
                var fullPath = GetFullPath(subPath);

                var key = GetKey(level.ToLevel(), fullPath);
                if (key == null)
                    throw new InvalidOperationException($"Cannot get key for registry path {fullPath}.");

                key.SetValue(keyName, value.ToString());
            });
        }

        public void ClearAllLocalSettings()
        {
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree($"Software\\{Assembly.GetEntryAssembly().GetName().Name.Split('.')[0]}");
        }

        public async Task<IDictionary<string, T>> GetLocalSettingsAsync<T>(ELocalLevel level, string subPath)
        {
            var result = await Task.Run(() =>
            {
                var fullPath = GetFullPath(subPath);

                var key = GetKey(level.ToLevel(), fullPath);
                if (key == null)
                    throw new InvalidOperationException($"Cannot get key for registry path {fullPath}.");

                var values = key.GetValueNames();

                var dict = new Dictionary<string, T>();

                foreach (var value in values)
                {
                    var data = key.GetValue(value);
                    dict.Add(ConvertValue<string>(value), ConvertValue<T>(data.ToString()));
                }

                return dict;
            });

            return result;
        }

        private static T ConvertValue<T>(string value)
        {
            return (T) Convert.ChangeType(value, typeof(T));
        }

        private static RegistryKey GetKey(RegistryHKey registryHKey, string path)
        {
            RegistryKey key;
            switch (registryHKey)
            {
                case RegistryHKey.CurrentUser:
                    key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(path);
                    break;
                case RegistryHKey.LocalMachine:
                    key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(path);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown environment {registryHKey}.");
            }

            return key;
        }

        private string GetFullPath(string subPath)
        {
            if (!string.IsNullOrEmpty(subPath))
                subPath = "\\" + subPath;

            var fullPath = $@"Software\{GetPath(Assembly.GetEntryAssembly())}{subPath}";
            return fullPath;
        }

        private string GetPath(Assembly assembly)
        {
            //var company = assembly.Company();

            var env = string.Empty;
            if (!string.IsNullOrEmpty(_environment))
                env = "\\" + _environment;

            var result = assembly.GetName().Name.Replace(".", "\\") + env;
            return result;
        }
    }
}