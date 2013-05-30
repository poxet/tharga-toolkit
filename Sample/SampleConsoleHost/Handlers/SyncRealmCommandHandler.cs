using System;
using SampleConsoleHost.Business;
using SampleDataTransfer.Command;
using Tharga.Toolkit.ServerStorage.CommandBase;
using Tharga.Toolkit.ServerStorage.Interface;

namespace SampleConsoleHost.Handlers
{
    public class SyncRealmCommandHandler : SyncCommandHandlerBase, IMessageHandler<SyncRealmCommand>
    {
        public void Handle(Guid realmId, SyncRealmCommand command, IServiceMessageBase serviceMessage)
        {
            Handle(realmId, new RealmBusiness(), command.SyncRequest.ServerStoreTime, serviceMessage);
        }
    }
}