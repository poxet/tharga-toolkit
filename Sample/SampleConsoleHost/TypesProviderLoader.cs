using System;
using System.Collections.Generic;
using System.Linq;
using SampleDataTransfer.Command;
using SampleDataTransfer.Entities;

namespace SampleConsoleHost
{
    public static class TypesProviderLoader
    {
        public static IEnumerable<Type> KnownCommandTypesProviderLoader()
        {
            //var commandAssembly = typeof(SaveCustomerCommand).Assembly;
            var commandAssembly = typeof(SaveProductCommand).Assembly;

            var commandTypes =
                from type in commandAssembly.GetExportedTypes()
                where type.FullName.StartsWith("SampleDataTransfer.Command.")
                select type;

            return commandTypes.Union(KnownCallbackTypesProviderLoader());
        }

        public static IEnumerable<Type> KnownCallbackTypesProviderLoader()
        {
            var commandAssembly = typeof(CustomerDto).Assembly;

            var commandTypes =
                from type in commandAssembly.GetExportedTypes()
                where type.FullName.StartsWith("SampleDataTransfer.Entities.")
                select type;

            return commandTypes;
        }
    }
}