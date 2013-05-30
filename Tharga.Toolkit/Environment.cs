using System;
using System.Reflection;
using System.Text;

namespace Tharga.Toolkit
{
    /// <summary>
    /// Static class containing environment information
    /// </summary>
    static public class Environment
    {
        #region Assembly Extension Methods


        /// <summary>
        /// Returnes the title attribute for an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static string Title(this Assembly assembly)
        {
            var assemblyTitleAttribute = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute)));
            return assemblyTitleAttribute == null ? null : assemblyTitleAttribute.Title;
        }

        /// <summary>
        /// Returnes the description attribute for an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static string Description(this Assembly assembly)
        {
            var assemblyTitleAttribute = ((AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute)));
            return assemblyTitleAttribute == null ? null : assemblyTitleAttribute.Description;
        }

        /// <summary>
        /// Returnes the company attribute for an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static string Company(this Assembly assembly)
        {
            var assemblyTitleAttribute = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute)));
            return assemblyTitleAttribute == null ? null : assemblyTitleAttribute.Company;
        }

        /// <summary>
        /// Returnes the product attribute for an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static string Product(this Assembly assembly)
        {
            var assemblyTitleAttribute = ((AssemblyProductAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyProductAttribute)));
            return assemblyTitleAttribute == null ? null : assemblyTitleAttribute.Product;
        }

        /// <summary>
        /// Returnes the copyright attribute for an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static string Copyright(this Assembly assembly)
        {
            var assemblyTitleAttribute = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute)));
            return assemblyTitleAttribute == null ? null : assemblyTitleAttribute.Copyright;
        }

        /// <summary>
        /// Returnes the trademark attribute for an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static string Trademark(this Assembly assembly)
        {
            var assemblyTitleAttribute = ((AssemblyTrademarkAttribute) Attribute.GetCustomAttribute(assembly, typeof (AssemblyTrademarkAttribute)));
            return assemblyTitleAttribute == null ? null : assemblyTitleAttribute.Trademark;
        }

        /// <summary>
        /// Returnes the version attribute for an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static Version Version(this Assembly assembly)
        {
            return assembly.GetName().Version;
        }        

        /// <summary>
        /// Returnes the key name for the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static string KeyName(this Assembly assembly)
        {
            return assembly.GetName().Name;
        }

        /// <summary>
        /// Returnes the location of the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static string Location(this Assembly assembly)
        {
            return assembly.Location;
        }

        /// <summary>
        /// Returnes the full data path for local file storage.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static string LocalDataPath(this Assembly assembly)
        {
            var subPath = new StringBuilder();
            subPath.Append(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));

            var asmNameArray = assembly.GetName().Name.Split('.');
            foreach (var part in asmNameArray)
                subPath.AppendFormat("\\{0}", part);

            return subPath.ToString();
        }


        #endregion        
        #region Toolkit Information


        /// <summary>
        /// Gets the version of toolkit used.
        /// </summary>
        /// <value>The toolkit version.</value>
        public static Version ToolkitVersion { get { return Assembly.GetExecutingAssembly().GetName().Version; } }


        #endregion
    }
}
