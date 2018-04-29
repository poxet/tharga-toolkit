using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HM.Order.OrderService.Business.Tests.UnitTests.CompareExtensions
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Used for testing.")]
    public class SomeTestClass
    {
        public string StringMember;
        public List<string> StringListMember;
        private readonly int _immutableIntMember;

        public SomeTestClass(int immutableIntMember)
        {
            _immutableIntMember = immutableIntMember;
        }

        public int IntReadProperty { get { return _immutableIntMember; } }
        public string StringProperty { get; set; }
        public List<SomeTestClass> ClassListProperty { get; set; }

        public Dictionary<string, SomeTestClass> Dictionary { get; set; }

        public static SomeTestClass Create()
        {
            return new SomeTestClass(123)
                {
                    StringMember = "DEF456",
                    StringListMember = new List<string> { "A", "B", "C" },
                    StringProperty = "ABC123",
                    ClassListProperty = new List<SomeTestClass> { new SomeTestClass(1) { StringProperty = "x1" } },
                    Dictionary = new Dictionary<string, SomeTestClass> { { "A", new SomeTestClass(1) { StringProperty = "x" } }, { "B", new SomeTestClass(2) { StringProperty = "x2" } } },
                };
        }
    }
}