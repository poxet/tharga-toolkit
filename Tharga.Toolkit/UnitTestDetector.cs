using System;
using System.Linq;

namespace Tharga.Toolkit.Test
{
    public static class UnitTestDetector
    {
        static UnitTestDetector()
        {
            const string testAssemblyName = "Microsoft.VisualStudio.QualityTools.UnitTestFramework";
            IsInUnitTest = AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.FullName.StartsWith(testAssemblyName));
        }

        public static bool IsInUnitTest { get; private set; }
    }
}
