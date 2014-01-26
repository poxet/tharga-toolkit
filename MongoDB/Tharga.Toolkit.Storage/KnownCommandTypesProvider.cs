using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tharga.Toolkit.Storage
{
    public static class KnownCommandTypesProvider
    {
        public static Func<IEnumerable<Type>> KnownCommandTypesProviderLoader { get; set; }

        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            if (KnownCommandTypesProviderLoader == null)
                throw new InvalidOperationException("No loader has been assigned for KnownCommandTypesProvider.");

            var response = KnownCommandTypesProviderLoader.Invoke();

            response = response.Union(new List<Type> {typeof (DeleteCommand), typeof (SaveCommand)});

            return response;
        }
    }
}