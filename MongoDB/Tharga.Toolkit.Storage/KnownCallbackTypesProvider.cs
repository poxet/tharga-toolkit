using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tharga.Toolkit.Storage
{
    public static class KnownCallbackTypesProvider
    {
        public static Func<IEnumerable<Type>> KnownCallbackTypesProviderLoader { get; set; }

        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            if (KnownCallbackTypesProviderLoader == null)
                throw new InvalidOperationException("No loader has been assigned for KnownCallbackTypesProvider.");

            var response = KnownCallbackTypesProviderLoader.Invoke();

            response = response.Union(new List<Type> {typeof (SyncCommand)});

            return response;
        }
    }
}