using System.ComponentModel;

namespace Tharga.Toolkit
{
    public class Settings
    {
        public static T GetSetting<T>(string name, T defaultValue)
        {
            var value = System.Configuration.ConfigurationManager.AppSettings[name];
            if (value == null) return defaultValue;
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value);
        }
    }
}