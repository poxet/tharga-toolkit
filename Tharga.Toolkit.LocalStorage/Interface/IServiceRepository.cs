using System;
using Tharga.Toolkit.LocalStorage.Entity;

namespace Tharga.Toolkit.LocalStorage.Interface
{
    public interface IServiceRepository<TEntity>
        where TEntity : IEntity
    {
        void Save(Guid sessionToken, TEntity item, bool notifySubscribers = true);
        void Delete(Guid sessionToken, Guid id);
        ISyncCompleteData Sync(Guid sessionToken, DateTime? syncTime);

        event EventHandler<EntityChangedEventArgs<TEntity>> EntityChangedEvent;
        event EventHandler<EntityChangedEventArgs<TEntity>> EntityChangedOnSyncEvent;
        event EventHandler<EntityDeletedEventArgs<TEntity>> EntityDeletedEvent;
        event EventHandler<EntityDeletedEventArgs<TEntity>> EntityDeletedOnSyncEvent;

        string OutgoingCommandQueueName { get; }
    }
}