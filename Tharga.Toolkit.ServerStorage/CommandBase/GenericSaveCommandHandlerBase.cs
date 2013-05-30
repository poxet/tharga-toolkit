using System;
using System.Linq;
using SimpleInjector;
using SimpleInjector.Extensions;
using Tharga.Toolkit.ServerStorage.Interface;
using Tharga.Toolkit.ServerStorage.Utility;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.ServerStorage.CommandBase
{
    public abstract class GenericSaveCommandHandlerBase : GenericCommandHandlerBase
    {
        public void Handle(Guid realmId, SaveCommand command)
        {
            var typeName = GetTypeName(command);

            var type = GetCustomDtoCommand(typeName);
            if (type != null)
                ExecuteCustomDtoCommand(realmId, command, type, typeName);
            else
                ExecuteGenericDtoCommand(realmId, command, typeName);
        }
        
        protected virtual void ExecuteGenericDtoCommand(Guid realmId, SaveCommand command, string typeName)
        {
            var businessWrapper = GetBusinessInstance(typeName);
            var business = businessWrapper.Unwrap();

            var businessType = business.GetType();

            var previousSyncTime = (DateTime?)businessType.GetMethod("GetLastServerStoreTime").Invoke(business, new object[] { realmId });
            var output = businessType.GetMethod("Save").Invoke(business, new[] { realmId, command.Item });

            if (command.NotifySubscribers)
                ServiceMessage.NotifyAllSaved(realmId, output, previousSyncTime);
        }

        protected virtual void ExecuteCustomDtoCommand(Guid realmId, SaveCommand command, Type type, string typeName)
        {
            var commandHandlerType = typeof(ICommandHandler<>).MakeGenericType(type);
            var commandHandler = Bootstrapper.GetInstance(commandHandlerType);

            var container = new Container();
            container.Register(type);
            container.Verify();
            var actualCommand = container.GetInstance(type);
            actualCommand.GetType().GetProperties().Single(x => x.Name == typeName).SetValue(actualCommand, command.Item);
            actualCommand.GetType().GetProperties().Single(x => x.Name == "NotifySubscribers").SetValue(actualCommand, command.NotifySubscribers);

            commandHandlerType.GetMethod("Handle").Invoke(commandHandler, new[] { realmId, actualCommand });
        }
    }
}