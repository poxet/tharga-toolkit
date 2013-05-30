using System;
using MongoDB.Bson.Serialization;
using SampleBusiness.Repository.Service;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.LocalStorage.Repository;

namespace SampleBusiness.Business
{
    public class GenericRealmBusiness<TEntity> : RealmBusinessBase<TEntity>
        where TEntity : IEntity
    {
        public GenericRealmBusiness(Func<TEntity, object> entityToDtoConverter, ISubscriptionHandler subscriptionHandler, Type entityType = null)
            : this(new GenericLocalRepository<TEntity>(), new GenericServiceRepository<TEntity>(entityToDtoConverter), subscriptionHandler, entityType)
        {

        }

        public GenericRealmBusiness(ILocalRepository<TEntity> localRepository, IServiceRepository<TEntity> serviceRepository, ISubscriptionHandler subscriptionHandler, Type entityType = null)
            : base(localRepository, serviceRepository, subscriptionHandler)
        {
            if (entityType != null)
                BsonClassMap.LookupClassMap(entityType);
        }
    }
}