using System;
using SampleConsoleHost.Business;
using SampleDataTransfer.Command;
using Tharga.Toolkit.ServerStorage.CommandBase;
using Tharga.Toolkit.ServerStorage.Interface;

namespace SampleConsoleHost.Handlers
{
    public class SyncUserCommandHandler : SyncCommandHandlerBase, IMessageHandler<SyncUserCommand>
    {
        public void Handle(Guid realmId, SyncUserCommand command, IServiceMessageBase serviceMessage)
        {
            Handle(realmId, new UserBusiness(), command.SyncRequest.ServerStoreTime, serviceMessage);
        }
    }
}