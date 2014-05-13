using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HM.Order.OrderService.Business.Tests.UnitTests.CompareExtensions
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Used for testing.")]
    public struct SomeTestStruct
    {
        public string StringMember;
        public List<string> StringListMember;
        public string StringProperty { get; set; }
        public List<SomeTestStruct> StructListProperty { get; set; }
    }
}