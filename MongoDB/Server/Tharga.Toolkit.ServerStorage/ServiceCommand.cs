using System;
using Tharga.Toolkit.ServerStorage.CommandBase;
using Tharga.Toolkit.ServerStorage.Interface;
using Tharga.Toolkit.ServerStorage.Utility;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.ServerStorage
{
    public class ServiceCommand : ServiceCommandBase, IServiceCommand
    {
        public void CheckSubscription(CheckSubscriptionRequest request)
        {
            ExecuteCommand(request.GetType().Name, () => ServiceMessage.NotifyAllSubscriptionChecked(null, null, "Command"));
        }

        public void Execute(Guid sessionToken, object command)
        {
            ExecuteCommand(command.GetType().Name, () =>
                {
                    var commandHandlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
                    var commandHandler = Bootstrapper.GetInstance(commandHandlerType);

                    var realmId = Guid.Empty;
                    if (sessionToken != Guid.Empty)
                    {
                        var session = SessionRepository.Get(sessionToken);
                        realmId = session.RealmId;
                    }

                    commandHandlerType.GetMethod("Handle").Invoke(commandHandler, new[] { realmId, command });
                });
        }
    }
}