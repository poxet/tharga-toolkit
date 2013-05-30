using System;
using Tharga.Toolkit.ServerStorage.Interface;

namespace Tharga.Toolkit.ServerStorage.CommandBase
{
    public abstract class DeleteCommandHandlerBase
    {
        protected void Handle<TOutput, TInput>(Guid realmId, BusinessBase<TOutput, TInput> business, Guid id, Action<Guid, TOutput, DateTime?> notifyAction)
            where TOutput : IOutputDto
            where TInput : IInputDto
        {
            var previousSyncTime = business.GetLastServerStoreTime(realmId);
            var entity = business.Delete(realmId, id);

            notifyAction(realmId, entity, previousSyncTime);
        }        
    }
}