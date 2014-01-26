using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using SimpleInjector;
using SimpleInjector.Extensions;
using Tharga.Toolkit.ServerStorage.Interface;
using Tharga.Toolkit.ServerStorage.Utility;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.ServerStorage.CommandBase
{
    public abstract class GenericSyncCommandHandlerBase : GenericCommandHandlerBase
    {
        protected abstract ObjectHandle GetEntityInstance(string typeName);

        public void Handle(Guid realmId, SyncCommand command, IServiceMessageBase serviceMessage)
        {
            var typeName = GetTypeName(command);

            var type = GetCustomDtoCommand(typeName);
            if (type != null)
                ExecuteCustomDtoCommand(realmId, command, type, serviceMessage);
            else
                ExecuteGenericDtoCommand(realmId, command, typeName, serviceMessage);
        }

        protected virtual void ExecuteGenericDtoCommand(Guid realmId, SyncCommand command, string typeName, IServiceMessageBase serviceMessage)
        {
            var businessWrapper = GetBusinessInstance(typeName);
            var business = businessWrapper.Unwrap();

            var businessType = business.GetType();
            var serverStoreTime = command.SyncRequest.ServerStoreTime;

            var changed = businessType.GetMethod("GetSyncList").Invoke(business, new object[] { realmId, serverStoreTime }) as IEnumerable<object>;

            IEnumerable<object> deleted = new List<object>();
            if (serverStoreTime != null)
                deleted = businessType.GetMethod("GetDeleted").Invoke(business, new object[] { realmId, serverStoreTime }) as IEnumerable<object>;

            var defaultEntityWrapper = GetEntityInstance(typeName);
            var defaultEntity = defaultEntityWrapper.Unwrap();

            serviceMessage.NotifySyncComplete(realmId, changed, deleted, defaultEntity);
        }

        protected virtual void ExecuteCustomDtoCommand(Guid realmId, SyncCommand command, Type type, IServiceMessageBase serviceMessage)
        {
            var commandHandlerType = typeof(IMessageHandler<>).MakeGenericType(type);
            var commandHandler = Bootstrapper.GetInstance(commandHandlerType);

            var container = new Container();
            container.Register(type);
            container.Verify();
            var actualCommand = container.GetInstance(type);
            actualCommand.GetType().GetProperties().Single(x => x.Name == "SyncRequest").SetValue(actualCommand, command.SyncRequest);

            commandHandlerType.GetMethod("Handle").Invoke(commandHandler, new[] { realmId, actualCommand, serviceMessage });
        }
    }
}