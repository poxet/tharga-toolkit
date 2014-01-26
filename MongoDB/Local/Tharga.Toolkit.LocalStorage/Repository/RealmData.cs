using System;

namespace Tharga.Toolkit.LocalStorage.Repository
{
    public class RealmData<T>
        where T : IId
    {
        public Guid Id { get; private set; }
        public Guid RealmId { get; private set; }
        public T Entity { get; private set; }

        public RealmData(Guid realmId, T entity)
        {
            RealmId = realmId;
            Id = entity.Id;
            Entity = entity;
        }
    }
}