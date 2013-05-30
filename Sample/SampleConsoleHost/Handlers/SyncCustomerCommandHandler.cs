using System;
using SampleConsoleHost.Business;
using SampleDataTransfer.Command;
using Tharga.Toolkit.ServerStorage.CommandBase;
using Tharga.Toolkit.ServerStorage.Interface;

namespace SampleConsoleHost.Handlers
{
    public class SyncCustomerCommandHandler : SyncCommandHandlerBase, IMessageHandler<SyncCustomerCommand>
    {
        public void Handle(Guid realmId, SyncCustomerCommand command, IServiceMessageBase serviceMessage)
        {
            Handle(realmId, new CustomerBusiness(), command.SyncRequest.ServerStoreTime, serviceMessage);
        }
    }
}