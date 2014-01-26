using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tharga.Toolkit.ServerStorage
{
    public static class KnownMessageTypesProvider
    {
        public static Func<IEnumerable<Type>> KnownMessageTypesProviderLoader { get; set; }

        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            if (KnownMessageTypesProviderLoader == null)
                throw new InvalidOperationException("No loader has been assigned for KnownMessageTypesProvider.");

            return KnownMessageTypesProviderLoader.Invoke();
        }
    }
}