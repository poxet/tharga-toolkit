using System;
using Tharga.Toolkit.ServerStorage.Interface;

namespace Tharga.Toolkit.ServerStorage.CommandBase
{
    public abstract class SaveCommandHandlerBase
    {
        protected void Handle<TOutput, TInput>(Guid realmId, BusinessBase<TOutput, TInput> business, TInput entity, bool notifySubscribers, Action<Guid,TOutput,DateTime?> notifyAction)
            where TOutput : IOutputDto
            where TInput : IInputDto        
        {
            var previousSyncTime = business.GetLastServerStoreTime(realmId);
            var output = business.Save(realmId, entity);

            if (notifySubscribers)
                notifyAction.Invoke(realmId, output, previousSyncTime);
        }        
    }
}