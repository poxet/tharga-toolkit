using System;
using SampleConsoleHost.Business;
using SampleDataTransfer.Command;
using Tharga.Toolkit.ServerStorage;
using Tharga.Toolkit.ServerStorage.CommandBase;
using Tharga.Toolkit.ServerStorage.Interface;

namespace SampleConsoleHost.Handlers
{
    public sealed class SaveProductCommandHandler : SaveCommandHandlerBase, ICommandHandler<SaveProductCommand>
    {
        public void Handle(Guid realmId, SaveProductCommand command)
        {
            Handle(realmId, new ProductBusiness(), command.Product, command.NotifySubscribers, ServiceMessage.NotifyAllSaved);
        }
    }
}