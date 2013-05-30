using System;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    public abstract class EntityBase : IEntity
    {
        public Guid Id { get; protected set; }
        public StoreInfo StoreInfo { get; protected set; }

        protected EntityBase()
        {
            Id = Guid.NewGuid();
            StoreInfo = new StoreInfo();
        }
    }
}