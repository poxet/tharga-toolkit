using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Tharga.Test.Toolkit
{
    public class AssemblyReferenceTest
    {
        private static readonly List<string> TestFrameworkAssemblies = new List<string> { "Moq", "nunit.framework", "HM.Order.Utils.Tests" };
        private static Assembly _assemblyBeeingTested;
        private static Assembly _testAssembly;

        public IEnumerable<AssemblyName> References { get; private set; }
        public IEnumerable<AssemblyName> TestReferences { get; private set; }
        public List<string> NotAllowed { get; private set; }
        public List<string> Needed { get; private set; }
        public List<string> AllowedInTestAssembly { get; private set; }

        public static AssemblyReferenceTest Given_there_is_an_assembly(Assembly testAssembly)
        {
            _testAssembly = testAssembly;
            _assemblyBeeingTested = Assembly.Load(testAssembly.GetName().Name.Replace(".Tests", string.Empty));

            var notAllowed = new List<string>();
            notAllowed.AddRange(TestFrameworkAssemblies);

            var allowed = new List<string>();
            allowed.AddRange(TestFrameworkAssemblies);
            allowed.Add(_assemblyBeeingTested.GetName().Name);

            return new AssemblyReferenceTest
                       {
                           AllowedInTestAssembly = allowed,
                           NotAllowed = notAllowed,
                           Needed = new List<string>(),
                           References = _assemblyBeeingTested.GetReferencedAssemblies(),
                           TestReferences = testAssembly.GetReferencedAssemblies()
                       };
        }

        public void AssertNotAllowed()
        {
            foreach (var referencedAssembly in References)
            {
                Assert.IsFalse(NotAllowed.Where(x => x.Contains("*")).Any(x => new Regex(x.Replace("*", ".*?")).Match(referencedAssembly.Name).Length != 0), string.Format("Assembly {0} is referenced, it should not be!", referencedAssembly.Name));
                Assert.IsFalse(NotAllowed.Where(x => !x.Contains("*")).Any(x => x == referencedAssembly.Name), string.Format("Assembly {0} is referenced, it should not be!", referencedAssembly.Name));
            }
        }

        public void AssertNeeded()
        {
            foreach (var item in Needed)
            {
                Assert.IsTrue(References.Any(z => z.Name == item), string.Format("Assembly {0} needs to be referenced from {1}.", item, _assemblyBeeingTested.GetName().Name));
            }
        }

        public void AssertTestAssemblyReferences()
        {
            foreach (var referencedAssembly in TestReferences)
            {
                var name = referencedAssembly.Name;

                if (AllowedInTestAssembly.Any(x => x == name))
                    continue;

                var rx = References.ToArray();
                var refInMaster = rx.Any(x => string.Compare(x.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0);
                if (!refInMaster)
                    System.Diagnostics.Debug.WriteLine(string.Empty);
                if (!refInMaster && name.EndsWith(".Tests"))
                {
                    name = name.Substring(0, name.Length - 6);
                    refInMaster = References.Any(x => string.Compare(x.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0);
                }

                Assert.IsTrue(refInMaster, string.Format("Assembly {0} should not be referenced from {1} since it is not referenced from {2}.", referencedAssembly, _testAssembly.GetName().Name, _assemblyBeeingTested.GetName().Name));
            }
        }
    }
}