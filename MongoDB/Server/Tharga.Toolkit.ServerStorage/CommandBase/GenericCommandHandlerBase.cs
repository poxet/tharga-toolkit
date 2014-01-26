using System;
using System.Runtime.Remoting;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.ServerStorage.CommandBase
{
    public abstract class GenericCommandHandlerBase
    {
        protected virtual string GetTypeName<T>(T command)
            where T : ICommand
        {
            var typeName = command.TypeName;
            if (typeName.EndsWith("Entity")) typeName = typeName.Substring(0, typeName.Length - 6);
            if (typeName.StartsWith("I")) typeName = typeName.Substring(1);
            return typeName;
        }

        protected abstract Type GetCustomDtoCommand(string typeName);
        protected abstract ObjectHandle GetBusinessInstance(string typeName);
    }
}