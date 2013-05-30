using System;
using System.Linq;
using SimpleInjector;
using SimpleInjector.Extensions;
using Tharga.Toolkit.ServerStorage.Interface;
using Tharga.Toolkit.ServerStorage.Utility;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.ServerStorage.CommandBase
{
    public abstract class GenericDeleteCommandHandlerBase : GenericCommandHandlerBase
    {
        public void Handle(Guid realmId, DeleteCommand command)
        {
            var typeName = GetTypeName(command);

            var type = GetCustomDtoCommand(typeName);
            if (type != null)
                ExecuteCustomDtoCommand(realmId, command, type);
            else
                ExecuteGenericDtoCommand(realmId, command, typeName);
        }

        protected virtual void ExecuteGenericDtoCommand(Guid realmId, DeleteCommand command, string typeName)
        {
            var businessWrapper = GetBusinessInstance(typeName);
            var business = businessWrapper.Unwrap();

            var businessType = business.GetType();

            var previousSyncTime = (DateTime?)businessType.GetMethod("GetLastServerStoreTime").Invoke(business, new object[] { realmId });
            var output = businessType.GetMethod("Delete").Invoke(business, new object[] { realmId, command.Id });

            ServiceMessage.NotifyAllDeleted(realmId, output, previousSyncTime);
        }

        protected virtual void ExecuteCustomDtoCommand(Guid realmId, DeleteCommand command, Type type)
        {
            var commandHandlerType = typeof(ICommandHandler<>).MakeGenericType(type);
            var commandHandler = Bootstrapper.GetInstance(commandHandlerType);

            var container = new Container();
            container.Register(type);
            container.Verify();
            var actualCommand = container.GetInstance(type);
            actualCommand.GetType().GetProperties().Single(x => x.Name == "Id").SetValue(actualCommand, command.Id);

            commandHandlerType.GetMethod("Handle").Invoke(commandHandler, new[] { realmId, actualCommand });
        }
    }
}