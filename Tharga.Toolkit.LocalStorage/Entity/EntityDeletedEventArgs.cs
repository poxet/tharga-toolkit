using System;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    public class EntityDeletedEventArgs<TEntity> : EventArgs
        where TEntity : IEntity
    {
        public Guid RealmId { get; protected set; }
        public TEntity Entity { get; protected set; }
        public Location? SaveLocation { get; protected set; }
        public DateTime? PreviousServerStoreTime { get; set; }
        public Exception Exception { get; set; }

        private EntityDeletedEventArgs()
        {

        }

        public static EntityDeletedEventArgs<TEntity> CreateLocal(Guid realmId, TEntity entity)
        {
            return new EntityDeletedEventArgs<TEntity>
            {
                RealmId = realmId,
                Entity = entity,
                SaveLocation = Location.Locally,
            };
        }

        public static EntityDeletedEventArgs<TEntity> CreateServer(Guid realmId, TEntity entity, DateTime? previousServerStoreTime)
        {
            return new EntityDeletedEventArgs<TEntity>
            {
                RealmId = realmId,
                Entity = entity,
                SaveLocation = Location.Server,
                PreviousServerStoreTime = previousServerStoreTime,
            };
        }

        public static EntityDeletedEventArgs<TEntity> CreateServerOnSync(Guid realmId, TEntity entity)
        {
            return new EntityDeletedEventArgs<TEntity>
            {
                RealmId = realmId,
                Entity = entity,
                SaveLocation = Location.Server,
            };
        }
    }
}