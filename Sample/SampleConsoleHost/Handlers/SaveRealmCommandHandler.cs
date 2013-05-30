using System;
using SampleConsoleHost.Business;
using SampleDataTransfer.Command;
using Tharga.Toolkit.ServerStorage;
using Tharga.Toolkit.ServerStorage.CommandBase;
using Tharga.Toolkit.ServerStorage.Interface;

namespace SampleConsoleHost.Handlers
{
    public class SaveRealmCommandHandler : SaveCommandHandlerBase, ICommandHandler<SaveRealmCommand>
    {
        public void Handle(Guid realmId, SaveRealmCommand command)
        {
            Handle(realmId, new RealmBusiness(), command.Realm, command.NotifySubscribers, ServiceMessage.NotifyAllSaved);
        }
    }
}