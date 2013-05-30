using System;
using System.Reflection;

namespace Tharga.Toolkit
{
    /// <summary>
    /// 
    /// </summary>
    public enum RegistryHKey
    {
        /// <summary>
        /// 
        /// </summary>
        CurrentUser,
        /// <summary>
        /// 
        /// </summary>
        LocalMachine
    }

    internal interface IRegistryRepository
    {
        object GetSetting(RegistryHKey registryHKey, string path, string keyName, object defaultValue);
        void SetSetting(RegistryHKey registryHKey, string path, string keyName, object value);
        void RemoveSetting(RegistryHKey registryHKey, string path, string keyName);
        bool HasSetting(RegistryHKey registryHKey, string registryPath, string keyName);

        void SetAutoStart(RegistryHKey registryHKey, string keyName, string assemblyLocation);
        void RemoveAutoStart(RegistryHKey registryHKey, string keyName);
        bool IsAutoStartEnabled(RegistryHKey registryHKey, string keyName, string assemblyLocation);
    }

    internal class RegistryRepository : IRegistryRepository
    {
        //NOTE: Minimize code here as it is not easy testable

        public object GetSetting(RegistryHKey registryHKey, string path, string keyName, object defaultValue)
        {
            //var fullPath = string.Format(@"Software\{0}", path);

            var key = GetKey(registryHKey, path);
            if (key == null) throw new InvalidOperationException(string.Format("Cannot get key for registry path {0}.", path));

            var value = key.GetValue(keyName);
            if (value == null)
            {
                if (defaultValue == null) throw new InvalidOperationException(string.Format("Cannot find setting for registry path {0} and key {1} and there is no default value provided.", path, keyName));

                key.SetValue(keyName, defaultValue);
                return defaultValue;
            }
            return value;
        }

        public void SetSetting(RegistryHKey registryHKey, string path, string keyName, object value)
        {
            //var fullPath = string.Format(@"Software\{0}", path);

            var key = GetKey(registryHKey, path);
            if (key == null) throw new InvalidOperationException(string.Format("Cannot get key for registry path {0}.", path));

            key.SetValue(keyName, value);
        }

        public void RemoveSetting(RegistryHKey registryHKey, string path, string keyName)
        {
            //var fullPath = string.Format(@"Software\{0}", path);

            var key = GetKey(registryHKey, path);
            if (key == null) throw new InvalidOperationException(string.Format("Cannot get key for registry path {0}.", path));

            key.DeleteValue(keyName);
        }

        public bool HasSetting(RegistryHKey registryHKey, string path, string keyName)
        {
            //var fullPath = string.Format(@"Software\{0}", path);

            var key = GetKey(registryHKey, path);
            if (key == null)
                return false;

            var value = (string)key.GetValue(keyName);
            if (value == null)
                return false;

            return true;
        }

        public void SetAutoStart(RegistryHKey registryHKey, string keyName, string assemblyLocation)
        {
            if (!assemblyLocation.ToLower().EndsWith(".exe")) throw new InvalidOperationException(string.Format("The assembly {0} is not an executable (does not end with .exe), {1}.", keyName, assemblyLocation));
            if (!System.IO.File.Exists(assemblyLocation)) throw new InvalidOperationException(string.Format("The assembly {0} does not exist at location {1}.", keyName, assemblyLocation));

            SetSetting(registryHKey, Registry.AutoStartLocation, keyName, assemblyLocation);
        }

        public void RemoveAutoStart(RegistryHKey registryHKey, string keyName)
        {
            RemoveSetting(registryHKey, Registry.AutoStartLocation, keyName);
        }

        public bool IsAutoStartEnabled(RegistryHKey registryHKey, string keyName, string assemblyLocation)
        {
            if (!HasSetting(registryHKey, Registry.AutoStartLocation, keyName))
                return false;

            var result = GetSetting(registryHKey, Registry.AutoStartLocation, keyName, null) as string;
            return string.Compare(result, assemblyLocation, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        #region Private support methods


        private static Microsoft.Win32.RegistryKey GetKey(RegistryHKey environment, string path)
        {
            Microsoft.Win32.RegistryKey key;
            switch (environment)
            {
                case RegistryHKey.CurrentUser:
                    key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(path);
                    break;
                case RegistryHKey.LocalMachine:
                    key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(path);
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown environment {0}.", environment));
            }

            return key;
        }


        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class Registry
    {
        /// <summary>
        /// Location in registry for auto start settings
        /// </summary>
        public const string AutoStartLocation = @"Software\Microsoft\Windows\CurrentVersion\Run";        

        #region Members


        private readonly IRegistryRepository _registryRepository;
        private readonly RegistryHKey _registryHKey;
        private readonly Assembly _assembly;


        #endregion
        #region properties


        private string RegistryPath
        {
            get
            {
                var asmNameArray = _assembly.GetName().Name.Split('.');
                return string.Format("Software\\{0}\\{1}\\{2}", asmNameArray[0], asmNameArray[1], asmNameArray[2]);
            }
        }

        #endregion
        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="Registry"/> class.
        /// </summary>
        /// <param name="registryHKey">The registry H key.</param>
        /// <param name="assembly">The assembly.</param>
        public Registry(RegistryHKey registryHKey, Assembly assembly)
        {
            _registryRepository = new RegistryRepository();
            _registryHKey = registryHKey;
            _assembly = assembly;
        }

        internal Registry(IRegistryRepository registryRepository, RegistryHKey registryHKey, Assembly assembly)
        {
            _registryRepository = registryRepository;
            _registryHKey = registryHKey;
            _assembly = assembly;
        }


        #endregion
        #region Setting


        /// <summary>
        /// Gets the setting.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public object GetSetting(string keyName, object defaultValue = null)
        {
            return _registryRepository.GetSetting(_registryHKey, RegistryPath, keyName, defaultValue);
        }

        /// <summary>
        /// Sets the setting.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="value">The value.</param>
        public void SetSetting(string keyName, object value)
        {
            _registryRepository.SetSetting(_registryHKey, RegistryPath, keyName, value);
        }

        /// <summary>
        /// Disables auto start for provided assembly.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        public void RemoveSetting(string keyName)
        {
            _registryRepository.RemoveSetting(_registryHKey, RegistryPath, keyName);
        }

        /// <summary>
        /// Determines whether the specified key name has setting.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <returns>
        /// 	<c>true</c> if the specified key name has setting; otherwise, <c>false</c>.
        /// </returns>
        public bool HasSetting(string keyName)
        {
            return _registryRepository.HasSetting(_registryHKey, RegistryPath, keyName);
        }


        #endregion
        #region AutoStart


        /// <summary>
        /// Enables auto start for provided assembly.
        /// </summary>
        public void SetAutoStart()
        {
            _registryRepository.SetAutoStart(_registryHKey, _assembly.GetName().Name, _assembly.Location);
        }

        /// <summary>
        /// Determines whether auto start is enabled for the provided assembly.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if auto start is enabled; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAutoStartEnabled()
        {
            return _registryRepository.IsAutoStartEnabled(_registryHKey, _assembly.GetName().Name, _assembly.Location);
        }

        /// <summary>
        /// Removes the auto start.
        /// </summary>
        public void RemoveAutoStart()
        {
            _registryRepository.RemoveAutoStart(_registryHKey, _assembly.GetName().Name);
        }

        ///// <summary>
        ///// Uns the set auto start.
        ///// </summary>
        ///// <param name="environment">The environment.</param>
        ///// <param name="assembly">The assembly.</param>
        //public static void UnSetAutoStart(Environment environment, Assembly assembly)
        //{
        //    UnSetAutoStart(environment, assembly.GetName().Name);
        //}

        //private static void UnSetAutoStart(Environment environment, string keyName)
        //{
        //    var key = GetKey(environment, RunLocation);
        //    if (key == null) throw new InvalidOperationException(string.Format("Cannot get key for registry path {0}.", RunLocation));
        //    key.DeleteValue(keyName);
        //}

        /////// <summary>
        /////// Sets the auto start.
        /////// </summary>
        /////// <param name="doAutoStart">if set to <c>true</c> [do auto start].</param>
        /////// <param name="environment">The environment.</param>
        ////public static void SetAutoStart(bool doAutoStart, Environment environment = Environment.CurrentUser)
        ////{
        ////    if (doAutoStart)
        ////        SetAutoStart(environment, Assembly.GetEntryAssembly());
        ////    else
        ////    {
        ////        if (IsAutoStartEnabled(environment, Assembly.GetEntryAssembly()))
        ////            UnSetAutoStart(environment, Assembly.GetEntryAssembly());
        ////    }
        ////}


        #endregion
    }
}

