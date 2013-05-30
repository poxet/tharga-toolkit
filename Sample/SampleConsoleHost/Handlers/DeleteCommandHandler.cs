using System;
using System.Runtime.Remoting;
using Tharga.Toolkit.ServerStorage.CommandBase;
using Tharga.Toolkit.ServerStorage.Interface;
using Tharga.Toolkit.Storage;

namespace SampleConsoleHost.Handlers
{
    public class DeleteCommandHandler : GenericDeleteCommandHandlerBase, ICommandHandler<DeleteCommand>
    {
        protected override Type GetCustomDtoCommand(string typeName)
        {
            var tn = string.Format("SampleDataTransfer.Command.Delete{0}Command, SampleDataTransfer", typeName);
            var type = Type.GetType(tn);
            return type;
        }

        protected override ObjectHandle GetBusinessInstance(string typeName)
        {
            var businessWrapper = Activator.CreateInstance(null, string.Format("SampleConsoleHost.Business.{0}Business", typeName));
            return businessWrapper;
        }        
    }
}