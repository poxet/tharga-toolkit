using System;
using System.Collections.Generic;
using System.Linq;
using Tharga.Toolkit.ServerStorage.Interface;

namespace Tharga.Toolkit.ServerStorage.CommandBase
{
    public abstract class SyncCommandHandlerBase
    {
        protected void Handle<TOutput, TInput>(Guid realmId, BusinessBase<TOutput, TInput> business, DateTime? serverStoreTime, IServiceMessageBase serviceMessage)
            where TOutput : IOutputDto, new()
            where TInput : IInputDto
        {
            var changed = business.GetSyncList(realmId, serverStoreTime).ToList();

            var deleted = new List<TOutput>();
            if (serverStoreTime != null)
                deleted = business.GetDeleted(realmId, serverStoreTime).ToList();

            serviceMessage.NotifySyncComplete(realmId, changed, deleted);
        }
    }
}