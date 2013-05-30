using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tharga.Toolkit.Storage
{
    public static class KnownEntityTypesProvider
    {
        public static Func<IEnumerable<Type>> KnownEntityTypesProviderLoader { get; set; }

        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            if (KnownEntityTypesProviderLoader == null)
                throw new InvalidOperationException("No loader has been assigned for KnownEntityTypesProvider.");

            return KnownEntityTypesProviderLoader.Invoke();
        }
    }
}