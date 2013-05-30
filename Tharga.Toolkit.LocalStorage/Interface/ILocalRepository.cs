using System;
using System.Collections.Generic;

namespace Tharga.Toolkit.LocalStorage.Interface
{
    public interface ILocalRepository<TEntity>
        where TEntity : IEntity
    {
        IEnumerable<TEntity> GetAll(Guid realmId);
        IEnumerable<TEntity> GetOutOfSync(Guid realmId);
        IEnumerable<TEntity> GetInSync(Guid realmId);
        TEntity Get(Guid realmId, Guid id);
        void Save(Guid realmId, TEntity entity);
        void Delete(Guid realmId, Guid id);
        void DeleteAll(Guid realmId);

        //Type GetEntityType();
    }
}