using System;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    public enum Location { Locally, Server };

    public class EntityChangedEventArgs<TEntity> : EventArgs
        where TEntity : IEntity
    {
        public Guid RealmId { get; protected set; }
        public TEntity Entity { get; protected set; }
        public Location? SaveLocation { get; protected set; }
        public DateTime? PreviousServerStoreTime { get; protected set; }
        public Exception Exception { get; set; }

        private EntityChangedEventArgs()
        {
            
        }

        public static EntityChangedEventArgs<TEntity> CreateLocal(Guid realmId, TEntity entity)
        {
            return new EntityChangedEventArgs<TEntity>
                       {
                           RealmId = realmId,
                           Entity = entity,
                           SaveLocation = Location.Locally
                       };
        }

        public static EntityChangedEventArgs<TEntity> CreateServerOnSync(Guid realmId, TEntity entity)
        {
            return new EntityChangedEventArgs<TEntity>
                       {
                           RealmId = realmId,
                           Entity = entity,
                           SaveLocation = Location.Server,
                       };
        }

        public static EntityChangedEventArgs<TEntity> CreateServer(Guid realmId, TEntity entity, DateTime? previousServerStoreTime)
        {
            return new EntityChangedEventArgs<TEntity>
            {
                RealmId = realmId,
                Entity = entity,
                SaveLocation = Location.Server,
                PreviousServerStoreTime = previousServerStoreTime,
            };
        }
    }
}