using System;
using SampleConsoleHost.Business;
using SampleDataTransfer.Command;
using Tharga.Toolkit.ServerStorage;
using Tharga.Toolkit.ServerStorage.CommandBase;
using Tharga.Toolkit.ServerStorage.Interface;

namespace SampleConsoleHost.Handlers
{
    public class DeleteProductCommandHandler : DeleteCommandHandlerBase, ICommandHandler<DeleteProductCommand>
    {
        public void Handle(Guid realmId, DeleteProductCommand command)
        {
            Handle(realmId, new ProductBusiness(), command.Id, ServiceMessage.NotifyAllDeleted);
        }
    }
}