using System;
using System.Reflection;
using System.Text;

namespace Tharga.Toolkit
{
    public static class Environment
    {
        public static string Title(this Assembly assembly)
        {
            var assemblyTitleAttribute = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute)));
            return assemblyTitleAttribute == null ? null : assemblyTitleAttribute.Title;
        }

        public static string Description(this Assembly assembly)
        {
            var assemblyTitleAttribute = ((AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute)));
            return assemblyTitleAttribute == null ? null : assemblyTitleAttribute.Description;
        }

        public static string Company(this Assembly assembly)
        {
            var assemblyTitleAttribute = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute)));
            return assemblyTitleAttribute == null ? null : assemblyTitleAttribute.Company;
        }

        public static string Product(this Assembly assembly)
        {
            var assemblyTitleAttribute = ((AssemblyProductAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyProductAttribute)));
            return assemblyTitleAttribute == null ? null : assemblyTitleAttribute.Product;
        }

        public static string Copyright(this Assembly assembly)
        {
            var assemblyTitleAttribute = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute)));
            return assemblyTitleAttribute == null ? null : assemblyTitleAttribute.Copyright;
        }

        public static string Trademark(this Assembly assembly)
        {
            var assemblyTitleAttribute = ((AssemblyTrademarkAttribute) Attribute.GetCustomAttribute(assembly, typeof (AssemblyTrademarkAttribute)));
            return assemblyTitleAttribute == null ? null : assemblyTitleAttribute.Trademark;
        }

        public static Version Version(this Assembly assembly)
        {
            return assembly.GetName().Version;
        }

        public static string KeyName(this Assembly assembly)
        {
            return assembly.GetName().Name;
        }

        public static string Location(this Assembly assembly)
        {
            return assembly.Location;
        }

        public static string LocalDataPath(this Assembly assembly)
        {
            var subPath = new StringBuilder();
            subPath.Append(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));

            var asmNameArray = assembly.GetName().Name.Split('.');
            foreach (var part in asmNameArray)
                subPath.AppendFormat("\\{0}", part);

            return subPath.ToString();
        }

        public static Version ToolkitVersion { get { return Assembly.GetExecutingAssembly().GetName().Version; } }
    }
}