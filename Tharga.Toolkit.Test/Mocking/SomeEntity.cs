using System;

namespace Tharga.Toolkit.Test.Mocking
{
    class SomeParentEntity
    {
        public SomeEntity SomeField;
        public SomeEntity SomeProperty { get; set; }
    }

    class SomeEntity
    {
        public Guid Id { get; set; }
    }
}