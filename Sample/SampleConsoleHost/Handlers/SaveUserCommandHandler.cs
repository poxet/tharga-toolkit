using System;
using SampleConsoleHost.Business;
using SampleDataTransfer.Command;
using Tharga.Toolkit.ServerStorage;
using Tharga.Toolkit.ServerStorage.CommandBase;
using Tharga.Toolkit.ServerStorage.Interface;

namespace SampleConsoleHost.Handlers
{
    public class SaveUserCommandHandler : SaveCommandHandlerBase, ICommandHandler<SaveUserCommand>
    {
        public void Handle(Guid userId, SaveUserCommand command)
        {
            Handle(userId, new UserBusiness(), command.User, command.NotifySubscribers, ServiceMessage.NotifyAllSaved);
        }
    }
}